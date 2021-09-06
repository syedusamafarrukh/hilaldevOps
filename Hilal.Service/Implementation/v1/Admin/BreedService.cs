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
    public class BreedService : IBreedService
    {
        public GetBreedResponse GetBreed(GetBreedRequest page)
        {
            try
            {
                GetBreedResponse response = new GetBreedResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.Breed
                        .Where(x => x.IsActive == true && (page.BreedId == null ? true : x.FkSelfBreedId.Equals(page.BreedId)) && (page.FK_CategoryId == null ? true : x.FkCategoryId == page.FK_CategoryId) && (page.FK_SubCategoryId == null ? true : x.BreedCategories.Where(y=> y.FkSubCategoryId == page.FK_SubCategoryId).Count() > 0))
                        .Select(x => new GetBreed
                        {
                            Id = x.Id,
                            Priority = x.Priority,
                            FK_CategoryId = new General<Guid?> { Id = x.FkCategoryId, Name = x.FkCategory.CategoriesDetails.FirstOrDefault(y => y.FkLanguageId == (int) ELanguage.English &&  y.IsActive == true).Name },
                            FK_SubCategoryId = x.BreedCategories.Select(y => new General<Guid?> { Id = y.FkSubCategoryId, Name = y.FkSubCategory.CategoriesDetails.FirstOrDefault(y => y.FkLanguageId == (int)ELanguage.English && y.IsActive == true).Name }).ToList(),
                            Name = x.BreedDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == (int)ELanguage.English).Name,
                            Image = new FileUrlResponce
                            {
                                URL = x.ImageUrl,
                                ThumbnailUrl = x.ImageThumbnailUrl
                            },
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

                    var orderedQuery = query.OrderByDescending(x => x.Name);
                    switch (page.SortIndex)
                    {
                        case 0:
                            orderedQuery = page.SortBy == "desc" ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                            break;
                        case 1:
                            orderedQuery = page.SortBy == "desc" ? query.OrderByDescending(x => x.Priority) : query.OrderBy(x => x.Priority);
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

        public List<GetBreedList> GetBreedByCategoryList(GetBreedByCategoryRequest page)
        {
            try
            {
                List<GetBreedList> response = new List<GetBreedList>();

                using (var db = new HilalDbContext())
                {
                    var query = db.Breed
                        .Where(x => x.IsActive == true && (x.FkCategoryId != null && page.FK_CategoryId.Contains(x.FkCategoryId.Value)) && (x.FkSubCategoryId != null && page.FK_SubCategoryId.Contains(x.FkSubCategoryId.Value)))
                        .Select(x => new GetBreedList
                        {
                            Id = x.Id,
                            Priority = x.Priority,
                            Name = x.BreedDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == (int)ELanguage.English).Name,
                            Image = new FileUrlResponce
                            {
                                URL = x.ImageUrl,
                                ThumbnailUrl = x.ImageThumbnailUrl
                            }
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

        public async Task<bool> SaveBreed(CreateBreedRequest breedRequest, Guid userId)
        {
            try
            {
                bool response = false;

                List<BreedDetails> informations = new List<BreedDetails>();

                foreach (var information in breedRequest.BreedInformations)
                {
                    informations.Add(new BreedDetails
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

                List<BreedCategories> breedCategories = new List<BreedCategories>();

                foreach (var information in breedRequest.subcategoryList)
                {
                    breedCategories.Add(new BreedCategories
                    {
                        Id = SystemGlobal.GetId(),
                        FkCategoryId = breedRequest.FK_CategoryId.Value,
                        FkSubCategoryId = information.Id,
                        IsActive = true,
                        CreatedBy = userId.ToString(),
                        CreatedOn = DateTime.UtcNow,
                        CreatedDate = DateTime.UtcNow
                    });
                }

                if (breedRequest.Id == null)
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                await db.Breed.AddAsync(new Breed
                                {
                                    Id = SystemGlobal.GetId(),
                                    FkSelfBreedId = breedRequest.FK_SelfBreedId,
                                    FkSubCategoryId = breedRequest.FK_SubCategoryId,
                                    FkCategoryId = breedRequest.FK_CategoryId,
                                    Priority = breedRequest.Priority,
                                    ImageUrl = breedRequest.Image.URL,
                                    ImageThumbnailUrl = breedRequest.Image.ThumbnailUrl,
                                    BreedDetails = informations,
                                    IsActive = true,
                                    CreatedBy = userId.ToString(),
                                    CreatedOn = DateTime.UtcNow,
                                    CreatedDate = DateTime.UtcNow,
                                    BreedCategories = breedCategories,
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
                                await db.BreedDetails
                                    .Where(x => x.IsActive ==  true && x.FkBreedId.Equals(breedRequest.Id))
                                    .ForEachAsync(x =>
                                    {
                                        x.IsActive = false;
                                        x.UpdatedBy = userId.ToString();
                                        x.UpdatedDate = DateTime.UtcNow;
                                    });

                                await db.SaveChangesAsync();

                                var breddcateList = db.BreedCategories
                                    .Where(x => x.IsActive == true && x.FkBreedId.Equals(breedRequest.Id)).ToList();
                                db.BreedCategories.RemoveRange(breddcateList);
                                db.SaveChanges();

                                //add Newer

                                var category = db.Breed.FirstOrDefault(x => x.Id.Equals(breedRequest.Id));
                                category.FkSelfBreedId = breedRequest.FK_SelfBreedId;
                                category.FkSubCategoryId = breedRequest.FK_SubCategoryId;
                                category.FkCategoryId = breedRequest.FK_CategoryId;
                                category.Priority = breedRequest.Priority;
                                category.ImageUrl = breedRequest.Image.URL;
                                category.ImageThumbnailUrl = breedRequest.Image.ThumbnailUrl;
                                category.IsActive = true;
                                category.UpdatedBy = userId.ToString();
                                category.UpdatedDate = DateTime.UtcNow;

                                db.Entry(category).State = EntityState.Modified;
                                await db.SaveChangesAsync();

                                informations.ForEach(x => x.FkBreedId = category.Id);
                                await db.BreedDetails.AddRangeAsync(informations);

                                breedCategories.ForEach(x => x.FkBreedId = category.Id);
                                await db.BreedCategories.AddRangeAsync(breedCategories);
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

        public CreateBreedRequest GetEditBreed(Guid id, Guid userId)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.Breed
                        .Where(x => x.Id.Equals(id))
                        .Select(x => new CreateBreedRequest
                        {
                            Id = x.Id,
                            FK_SelfBreedId = x.FkSelfBreedId,
                            FK_SubCategoryId = x.FkSubCategoryId,
                            FK_CategoryId = x.FkCategoryId,
                            Priority = x.Priority,
                            Image = new FileUrlResponce
                            {
                                URL = x.ImageUrl,
                                ThumbnailUrl = x.ImageThumbnailUrl
                            },
                            BreedInformations = x.BreedDetails.Where(y => y.IsActive == true).Select(y => new GenericDetailRequest
                            {
                                Id = y.Id,
                                LanguageId = y.FkLanguageId,
                                Name = y.Name
                            }).ToList(),
                            subcategoryList = x.BreedCategories.Select(y => new General<Guid> { 
                            Id = y.FkSubCategoryId,
                            Name = y.FkSubCategory.CategoriesDetails.FirstOrDefault(z=> z.IsActive == true && z.FkLanguageId == 2).Name
                            }).ToList()
                        })
                        .FirstOrDefault() ?? new CreateBreedRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ControllBreedActivation(Guid breedId, bool activation, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.Breed.FirstOrDefault(x => x.Id.Equals(breedId));

                    if (category == null) throw new Exception("categoryId Doesn't Exists");

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

        public List<GetBreedList> GetBreedList(GetBreedRequest page)
        {
            try
            {
                List<GetBreedList> response = new List<GetBreedList>();

                using (var db = new HilalDbContext())
                {
                    response = db.Breed
                        .Where(x => x.IsActive == true && (page.FK_CategoryId == null ? true : x.FkCategoryId == page.FK_CategoryId) && (page.FK_SubCategoryId == null ? true : x.BreedCategories.Where(y => y.FkSubCategoryId == page.FK_SubCategoryId).Count() > 0))
                        .OrderByDescending(x=> x.CreatedDate)
                        .Select(x => new GetBreedList
                        {
                            Id = x.Id,
                            Priority = x.Priority,
                            Name = x.BreedDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == page.LanguageId).Name,
                            Image = new FileUrlResponce
                            {
                                URL = x.ImageUrl,
                                ThumbnailUrl = x.ImageThumbnailUrl
                            },
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
