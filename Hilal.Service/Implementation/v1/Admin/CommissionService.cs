using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Hilal.Common;
using Hilal.Data.Context;
using Hilal.Data.DTOs;
using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using Hilal.Service.Interface.v1.Admin;
using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Enum;

namespace Hilal.Service.Implementation.v1.Admin
{
    public class CommissionService : ICommissionService
    {
        public GetCommissionResponse GetCommission(ListGeneralModel Commission)
        {
            try
            {
                GetCommissionResponse response = new GetCommissionResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.Commission
                        .Include(x=> x.FkCategory).ThenInclude(x=> (x as Categories).CategoriesDetails)
                        .OrderByDescending(x=> x.CreatedDate)
                        .Where(x => x.IsActive == true)
                        .Select(x => new GetCommission
                        {
                            Id = x.Id,
                            FK_CategoryId = new General<Guid?> { Id = x.FkCategoryId , Name = x.FkCategory.CategoriesDetails.FirstOrDefault(y=> y.FkLanguageId == (int) ELanguage.English).Name},
                            DisplayPercentage = x.CommissionDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == (int)ELanguage.English).DisplayPercentage,
                            DisplayRange = x.CommissionDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == (int) ELanguage.English).DisplayRange,
                            StartRange = x.StartRange,
                            EndRange = x.EndRange,
                            Percentage = x.Percentage,
                        })
                        .AsQueryable();

                    if (!string.IsNullOrEmpty(Commission.Search))
                    {
                        var date = new DateTime();
                        var sdate = DateTime.TryParse(Commission.Search, out date);
                        int totalCases = -1;
                        var isNumber = Int32.TryParse(Commission.Search, out totalCases);

                        query = query.Where(
                        x => x.Name.ToLower().Contains(Commission.Search.ToLower())
                    );
                    }

                    var orderedQuery = query;
                    switch (Commission.SortIndex)
                    {
                        case 0:
                            orderedQuery = Commission.SortBy == "desc" ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                            break;
                        case 2:
                            orderedQuery = Commission.SortBy == "desc" ? query.OrderByDescending(x => x.DisplayRange) : query.OrderBy(x => x.DisplayRange);
                            break;
                        case 3:
                            orderedQuery = Commission.SortBy == "desc" ? query.OrderByDescending(x => x.DisplayPercentage) : query.OrderBy(x => x.DisplayPercentage);
                            break;
                    }


                    response.PageSize = Commission.PageSize;
                    response.Page = Commission.Page;
                    response.TotalRecords = orderedQuery.Count();
                    if (Commission.PageSize > 0)
                    {
                        response.ItemList = orderedQuery.Skip(response.Page).Take(Commission.PageSize).ToList();
                    }
                    else
                    {
                        response.ItemList = orderedQuery.ToList();

                    }
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SaveCommission(CreateCommissionRequest CommissionRequest, Guid userId)
        {
            try
            {
                bool response = false;

                List<CommissionDetails> informations = new List<CommissionDetails>();

                foreach (var information in CommissionRequest.CommissionInformations)
                {
                    informations.Add(new CommissionDetails
                    {
                        Id = SystemGlobal.GetId(),
                        FkLanguageId = information.LanguageId,
                        IsActive = true,
                        CreatedBy = userId.ToString(),
                        CreatedOn = DateTime.UtcNow,
                        CreatedDate = DateTime.UtcNow,
                        DisplayPercentage = information.DisplayRange,
                        DisplayRange = information.DisplayRange
                    });
                }

                if (CommissionRequest.Id == null)
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                await db.Commission.AddAsync(new Commission
                                {
                                    Id = SystemGlobal.GetId(),
                                    CommissionDetails = informations,
                                    FkCategoryId = CommissionRequest.FK_CategoryId,
                                    StartRange = CommissionRequest.StartRange,
                                    EndRange = CommissionRequest.EndRange,
                                    Percentage = CommissionRequest.Percentage,
                                    CreatedDate = DateTime.UtcNow,
                                    IsActive = true,
                                    CreatedBy = userId.ToString(),
                                    CreatedOn = DateTime.UtcNow,
                                });

                                await db.SaveChangesAsync();
                                trans.Commit();

                                response = true;
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                throw ex;
                            }
                        }
                    }
                }
                else
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                await db.CommissionDetails
                                    .Where(x => x.IsActive ==  true && x.FkCommissionId.Equals(CommissionRequest.Id))
                                    .ForEachAsync(x =>
                                    {
                                        x.IsActive = false;
                                        x.UpdatedBy = userId.ToString();
                                        x.UpdatedDate = DateTime.UtcNow;
                                    });

                                await db.SaveChangesAsync();

                                //add Newer

                                var category = db.Commission.FirstOrDefault(x => x.Id.Equals(CommissionRequest.Id));
                                
                                category.Percentage = CommissionRequest.Percentage;
                                category.StartRange = CommissionRequest.StartRange;
                                category.EndRange  = CommissionRequest.EndRange;
                                category.FkCategoryId  = CommissionRequest.FK_CategoryId;
                                category.UpdatedBy = userId.ToString();
                                category.UpdatedDate = DateTime.UtcNow;

                                db.Entry(category).State = EntityState.Modified;
                                await db.SaveChangesAsync();

                                informations.ForEach(x => x.FkCommissionId = category.Id);
                                await db.CommissionDetails.AddRangeAsync(informations);

                                await db.SaveChangesAsync();

                                trans.Commit();

                                response = true;
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                throw ex;
                            }
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CreateCommissionRequest GetEditCommission(Guid id, Guid userId)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.Commission
                        .Where(x => x.Id.Equals(id))
                        .Select(x => new CreateCommissionRequest
                        {
                            Id = x.Id,
                            FK_CategoryId = x.FkCategoryId,
                            Percentage = x.Percentage,
                            EndRange = x.EndRange,
                            StartRange = x.StartRange, 
                            CommissionInformations = x.CommissionDetails.Where(y => y.IsActive == true).Select(y => new CommissionDetailRequest
                            {
                                Id = y.Id,
                                LanguageId = y.FkLanguageId,
                                DisplayRange = y.DisplayRange,
                                DisplayPercentage = y.DisplayPercentage,
                                FKMasterId = y.FkCommissionId,
                            }).ToList()
                        })
                        .FirstOrDefault() ?? new CreateCommissionRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ControllCommissionActivation(Guid CommissionId, bool activation, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.Commission.FirstOrDefault(x => x.Id.Equals(CommissionId));

                    if (category == null) throw new Exception("CommissionId Doesn't Exists");

                    category.IsActive = activation;
                    category.UpdatedBy = userId.ToString();
                    category.UpdatedDate = DateTime.UtcNow;

                    db.Entry(category).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    response = true;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
