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
    public class AgeService : IAgeService
    {
        public GetAgeResponse GetAge(GetAgeRequest page)
        {
            try
            {
                GetAgeResponse response = new GetAgeResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.Age.Include(x=> x.FkCategory).ThenInclude(x=> (x as Categories).CategoriesDetails).Include(x=> x.FkSubCategory).ThenInclude(x => (x as Categories).CategoriesDetails)
                        .Where(x => x.IsActive == true && (page.FK_CategoryId == null ? true : x.FkCategoryId == page.FK_CategoryId) && (page.FK_SubCategoryId == null ? true : x.AgeCategories.Where(y => y.FkSubCategoryId == page.FK_SubCategoryId).Count() > 0)).OrderByDescending(x => x.CreatedDate)
                        .OrderByDescending(x => x.CreatedOn)
                        .OrderBy(x=> x.Priority)
                        .Select(x => new GetAge
                        {
                            Id = x.Id,
                            FK_CategoryId = new General<Guid?> { Id = x.FkCategoryId, Name = x.FkCategory.CategoriesDetails.FirstOrDefault(y =>  y.FkLanguageId == (int)ELanguage.English && y.IsActive == true).Name },
                            FK_SubCategoryId = x.AgeCategories.Select(y=> new General<Guid?> { Id = y.FkSubCategoryId, Name = y.FkSubCategory.CategoriesDetails.FirstOrDefault(y => y.FkLanguageId == (int)ELanguage.English && y.IsActive == true).Name }).ToList(),
                            Name = x.AgeDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == (int)ELanguage.English).Name,
                            //IsActive = x.IsActive,
                        })
                        .AsQueryable();

                    if (!string.IsNullOrEmpty(page.Search))
                    {
                        var date = new DateTime();
                        var sdate = DateTime.TryParse(page.Search, out date);
                        int totalCases = -1;
                        var isNumber = Int32.TryParse(page.Search, out totalCases);

                        query = query.Where(
                        x => x.Name.ToLower().Contains(page.Search.ToLower())
                    );
                    }

                    var orderedQuery = query;
                    switch (page.SortIndex)
                    {
                        case 0:
                            orderedQuery = page.SortBy == "desc" ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                            break;
                        case 2:
                            orderedQuery = page.SortBy == "desc" ? query.OrderByDescending(x => x.IsActive) : query.OrderBy(x => x.IsActive);
                            break;
                    }


                    response.Page = page.Page;
                    response.PageSize = page.PageSize;
                    response.TotalRecords = orderedQuery.Count();
                    if (page.PageSize > 0)
                    {
                        response.ItemList = orderedQuery.Skip(page.Page).Take(page.PageSize).ToList();
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

        public List<GetAgesLsit> GetAgeByCategories(GetAgeByCategoryRequest page)
        {
            try
            {
                List<GetAgesLsit> response = new List<GetAgesLsit>();

                using (var db = new HilalDbContext())
                {
                    var query = db.Age.Include(x => x.FkCategory).ThenInclude(x => (x as Categories).CategoriesDetails).Include(x => x.FkSubCategory).ThenInclude(x => (x as Categories).CategoriesDetails)
                        .Where(x => x.IsActive == true && (x.FkCategoryId != null && page.FK_CategoryId.Contains(x.FkCategoryId.Value)) && (x.FkSubCategoryId != null && page.FK_SubCategoryId.Contains(x.FkSubCategoryId.Value)))
                        .OrderByDescending(x => x.CreatedOn)
                        .OrderBy(x => x.Priority)
                        .Select(x => new GetAgesLsit
                        {
                            Id = x.Id,
                            Name = x.AgeDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == (int)ELanguage.English).Name,
                        }).AsQueryable();

                    if (page.PageSize > 0)
                    {
                        response = query.Skip(page.Page).Take(page.PageSize).ToList();
                    }
                    else
                    {
                        response = query.ToList();
                    }
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SaveAge(CreateAgeRequest AgeRequest, Guid userId)
        {
            try
            {
                bool response = false;

                List<AgeDetails> informations = new List<AgeDetails>();

                foreach (var information in AgeRequest.AgeInformations)
                {
                    informations.Add(new AgeDetails
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

                List<AgeCategories> ageCategories = new List<AgeCategories>();

                foreach (var information in AgeRequest.subcategoryList)
                {
                    ageCategories.Add(new AgeCategories
                    {
                        Id = SystemGlobal.GetId(),
                        FkCategoryId = AgeRequest.FK_CategoryId.Value,
                        FkSubCategoryId = information.Id,
                        IsActive = true,
                        CreatedBy = userId.ToString(),                        
                        CreatedOn = DateTime.UtcNow,
                        CreatedDate = DateTime.UtcNow
                    });
                }

                if (AgeRequest.Id == null)
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                await db.Age.AddAsync(new Age
                                {
                                    Id = SystemGlobal.GetId(),
                                    FkSubCategoryId = AgeRequest.FK_SubCategoryId,
                                    FkCategoryId = AgeRequest.FK_CategoryId,
                                    AgeDetails = informations,
                                    IsActive = true,
                                    CreatedBy = userId.ToString(),
                                    CreatedOn = DateTime.UtcNow,
                                    CreatedDate = DateTime.UtcNow,
                                    Priority = AgeRequest.Priority,
                                    AgeCategories = ageCategories
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
                                await db.AgeDetails
                                    .Where(x => x.IsActive ==  true && x.FkAgeId.Equals(AgeRequest.Id))
                                    .ForEachAsync(x =>
                                    {
                                        x.IsActive = false;
                                        x.UpdatedBy = userId.ToString();
                                        x.UpdatedDate = DateTime.UtcNow;
                                    });

                                await db.SaveChangesAsync();

                                var agecateList = db.AgeCategories
                                    .Where(x => x.FkAgeId.Equals(AgeRequest.Id)).ToList();
                                db.AgeCategories.RemoveRange(agecateList);
                                db.SaveChanges();

                                //add Newer

                                var category = db.Age.FirstOrDefault(x => x.Id.Equals(AgeRequest.Id));
                                
                                category.FkSubCategoryId = AgeRequest.FK_SubCategoryId;
                                category.FkCategoryId = AgeRequest.FK_CategoryId;
                                category.Priority = AgeRequest.Priority;
                                category.IsActive = true;
                                category.UpdatedBy = userId.ToString();
                                category.UpdatedDate = DateTime.UtcNow;

                                db.Entry(category).State = EntityState.Modified;
                                await db.SaveChangesAsync();

                                informations.ForEach(x => x.FkAgeId = category.Id);
                                await db.AgeDetails.AddRangeAsync(informations);

                                ageCategories.ForEach(x => x.FkAgeId = category.Id);
                                await db.AgeCategories.AddRangeAsync(ageCategories);
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

        public CreateAgeRequest GetEditAge(Guid id, Guid userId)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.Age
                        .Where(x => x.Id.Equals(id))
                        .Select(x => new CreateAgeRequest
                        {
                            Id = x.Id,
                            FK_SubCategoryId = x.FkSubCategoryId,
                            FK_CategoryId = x.FkCategoryId,
                            Priority = x.Priority,
                            AgeInformations = x.AgeDetails.Where(y => y.IsActive == true).Select(y => new GenericDetailRequest
                            {
                                Id = y.Id,
                                LanguageId = y.FkLanguageId,
                                Name = y.Name
                            }).ToList(),
                            subcategoryList = x.AgeCategories.Select(y => new General<Guid>
                            {
                                Id = y.FkSubCategoryId,
                                Name = y.FkSubCategory.CategoriesDetails.FirstOrDefault(z => z.IsActive == true && z.FkLanguageId == 2).Name
                            }).ToList()
                        })
                        .FirstOrDefault() ?? new CreateAgeRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ControllAgeActivation(Guid AgeId, bool activation, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.Age.FirstOrDefault(x => x.Id.Equals(AgeId));

                    if (category == null) throw new Exception("AgeId Doesn't Exists");

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

        public List<GetAgesLsit> GetAgeList(GetAgeRequest page)
        {
            try
            {
                List<GetAgesLsit> response = new List<GetAgesLsit>();

                using (var db = new HilalDbContext())
                {
                    response = db.Age
                        .Where(x => x.IsActive == true && (page.FK_CategoryId == null ? true : x.FkCategoryId == page.FK_CategoryId) && (page.FK_SubCategoryId == null ? true : x.AgeCategories.Where(y => y.FkSubCategoryId == page.FK_SubCategoryId).Count() > 0)).OrderByDescending(x=> x.CreatedDate)
                        .OrderByDescending(x => x.CreatedOn)
                        .OrderBy(x => x.Priority)
                        .Select(x => new GetAgesLsit
                        {
                            Id = x.Id,
                            Name = x.AgeDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == page.LanguageId).Name,
                            //IsActive = x.IsActive,
                        })
                        .ToList();
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
