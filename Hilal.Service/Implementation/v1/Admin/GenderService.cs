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
    public class GenderService : IGenderService
    {
        public GetGenderResponse GetGender(GetGenderRequest Gender)
        {
            try
            {
                GetGenderResponse response = new GetGenderResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.HilalGenders
                        .Include(x => x.GenderDetails)
                        .OrderByDescending(x=> x.CreatedOn)
                        .Where(x => x.IsActive == true)
                        .Select(x => new GetGender
                        {
                            Id = x.Id,
                            Name = x.GenderDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == Gender.LanguageId).Name,
                            //IsActive = x.IsActive,
                        })
                        .AsQueryable();

                    if (!string.IsNullOrEmpty(Gender.Search))
                    {
                        var date = new DateTime();
                        var sdate = DateTime.TryParse(Gender.Search, out date);
                        int totalCases = -1;
                        var isNumber = Int32.TryParse(Gender.Search, out totalCases);

                        query = query.Where(
                        x => x.Name.ToLower().Contains(Gender.Search.ToLower())
                    );
                    }
                    var orderedQuery = query;
                    switch (Gender.SortIndex)
                    {
                        case 0:
                            orderedQuery = Gender.SortBy == "desc" ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                            break;
                            //case 2:
                            //    orderedQuery = Gender.SortBy == "desc" ? query.OrderByDescending(x => x.IsActive) : query.OrderBy(x => x.IsActive);
                            //    break;
                    }


                    response.PageSize = Gender.PageSize;
                    response.Page = Gender.Page;
                    response.TotalRecords = orderedQuery.Count();
                    if (response.PageSize > 0)
                    {
                        response.ItemList = orderedQuery.Skip(response.Page).Take(Gender.PageSize).ToList();
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

        public List<GetGenderList> GetGenderByCategory(GetGenderByCategoryRequest Gender)
        {
            try
            {
                List<GetGenderList> response = new List<GetGenderList>();

                using (var db = new HilalDbContext())
                {
                    var query = db.HilalGenders
                        .Include(x => x.GenderDetails)
                        .OrderByDescending(x => x.CreatedOn)
                        .Where(x => x.IsActive == true)
                        .Select(x => new GetGenderList
                        {
                            Id = x.Id,
                            Name = x.GenderDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == Gender.LanguageId).Name,
                            //IsActive = x.IsActive,
                        })
                        .AsQueryable();

                        response = query.ToList();
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SaveGender(CreateGenderRequest GenderRequest, Guid userId)
        {
            try
            {
                bool response = false;

                List<GenderDetails> informations = new List<GenderDetails>();

                foreach (var information in GenderRequest.GenderInformations)
                {
                    informations.Add(new GenderDetails
                    {
                        Id = SystemGlobal.GetId(),
                        Name = information.Name,
                        FkLanguageId = information.LanguageId,
                        IsActive = true,
                        CreatedBy = userId.ToString(),
                        CreatedOn = DateTime.UtcNow,
                        CreatedDate = DateTime.UtcNow
                    });
                }

                //List<HilalGenderAges> GenderAges = new List<HilalGenderAges>();

                //foreach (var information in GenderRequest.GenderAgesInformation)
                //{
                //    GenderAges.Add(new HilalGenderAges
                //    {
                //        Id = SystemGlobal.GetId(),
                //        FkAgeId = information.FK_AgeId,
                //        FkCategoryId = information.FK_CategoryId,
                //        CreatedOnDate = DateTime.UtcNow,
                //        CreatedBy = userId.ToString(),
                //        CreatedOn = DateTime.UtcNow,
                //        IsActive = true
                //    });
                //}

                if (GenderRequest.Id == null)
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                await db.HilalGenders.AddAsync(new HilalGenders
                                {
                                    Id = SystemGlobal.GetId(),
                                    GenderDetails = informations,
                                    CreatedOnDate = DateTime.UtcNow,
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
                                await db.GenderDetails
                                    .Where(x => x.IsActive == true && x.FkGenderId.Equals(GenderRequest.Id))
                                    .ForEachAsync(x =>
                                    {
                                        x.IsActive = false;
                                        x.UpdatedBy = userId.ToString();
                                        x.UpdatedDate = DateTime.UtcNow;
                                    });

                                //await db.HilalGenderAges
                                //    .Where(x => x.IsActive == true && x.FkGenderId.Equals(GenderRequest.Id))
                                //    .ForEachAsync(x =>
                                //    {
                                //        x.IsActive = false;
                                //        x.UpdatedBy = userId.ToString();
                                //        x.UpdatedOn = DateTime.UtcNow;
                                //    });

                                await db.SaveChangesAsync();

                                //add Newer

                                var category = db.HilalGenders.FirstOrDefault(x => x.Id.Equals(GenderRequest.Id));

                                category.UpdatedBy = userId.ToString();
                                category.UpdatedOn = DateTime.UtcNow;

                                db.Entry(category).State = EntityState.Modified;
                                await db.SaveChangesAsync();

                                informations.ForEach(x => x.FkGenderId = category.Id);
                                await db.GenderDetails.AddRangeAsync(informations);

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

        public CreateGenderRequest GetEditGender(Guid id, Guid userId)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.HilalGenders.Include(x=> x.GenderDetails)
                        .Where(x => x.Id.Equals(id))
                        .Select(x => new CreateGenderRequest
                        {
                            Id = x.Id,
                            //GenderAgesInformation = x.HilalGenderAges.Where(z => z.IsActive == true).Select(z => new CreateGenderAges
                            //{
                            //    FK_AgeId = z.FkAgeId,
                            //    FK_CategoryId = z.FkAgeId,
                            //    FK_GenderId = z.FkGenderId,
                            //}).ToList(),
                            GenderInformations = x.GenderDetails.Where(y => y.IsActive == true).Select(y => new GenericDetailRequest
                            {
                                Id = y.Id,
                                Name = y.Name
                            }).ToList()
                        })
                        .FirstOrDefault() ?? new CreateGenderRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ControllGenderActivation(Guid GenderId, bool activation, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.HilalGenders.FirstOrDefault(x => x.Id.Equals(GenderId));

                    if (category == null) throw new Exception("GenderId Doesn't Exists");

                    category.IsActive = activation;
                    category.UpdatedBy = userId.ToString();
                    category.UpdatedOn = DateTime.UtcNow;

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

        public List<GetGenderList> GetGenderList(GetGenderRequest Gender)
        {
            try
            {
                List<GetGenderList> response = new List<GetGenderList>();

                using (var db = new HilalDbContext())
                {
                    response = db.HilalGenders
                        .Include(x => x.GenderDetails)
                        .OrderByDescending(x => x.CreatedOn)
                        .Where(x => x.IsActive == true)
                        .Select(x => new GetGenderList
                        {
                            Id = x.Id,
                            Name = x.GenderDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == Gender.LanguageId).Name,
                            //IsActive = x.IsActive,
                        }).ToList();
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
