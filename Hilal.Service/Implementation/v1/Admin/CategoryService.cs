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
    public class CategoryService : ICategoryService
    {
        public GetCategoryResponse GetCategories(GetCategoriesRequest page)
        {
            try
            {
                GetCategoryResponse response = new GetCategoryResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.Categories.Include(x=> x.CategorySelf).ThenInclude(x=> (x as Categories).CategoriesDetails)
                        .OrderByDescending(x=> x.CreatedOn)
                        .Where(x => x.IsActive ==  true && (page.FkCategoryType == null ? true : x.FkCategoryType == page.FkCategoryType) && (page.CategoryId == null ? true : x.CategorySelfId.Equals(page.CategoryId)) && (page.SubCategoryList == true ? x.CategorySelfId != null : x.CategorySelfId == null))
                        .Select(x => new GetCategory
                        {
                            Id = x.Id,
                            IsSubCategory = x.IsSubCategory,
                            Priority = x.Priority,
                            FK_CategoryId = new General<Guid?> { Id = x.CategorySelfId , Name = x.CategorySelf.CategoriesDetails.FirstOrDefault(y => y.FkLanguageId == (int) ELanguage.English && y.IsActive == true).Name},
                            Name = x.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == (int)ELanguage.English).Name,
                            Image = new FileUrlResponce
                            {
                                URL = x.ImageUrl,
                                ThumbnailUrl = x.ImageThumbnailUrl
                            },
                            FkCategoryType = x.FkCategoryType
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

                    var orderedQuery = query.OrderBy(x=> x.Priority);
                    switch (page.SortIndex)
                    {
                        case 0:
                            orderedQuery = page.SortBy == "desc" ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                            break;
                        case 1:
                            orderedQuery = page.SortBy == "desc" ? query.OrderByDescending(x => x.Priority) : query.OrderBy(x => x.Priority);
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

        public List<GetCategory> GetSubCategoriesByCategoryList(GetSubCategoriesRequest page)
        {
            try
            {
                List<GetCategory> response = new List<GetCategory>();

                using (var db = new HilalDbContext())
                {
                    var query = db.Categories.Include(x => x.CategorySelf).ThenInclude(x => (x as Categories).CategoriesDetails)
                        .OrderByDescending(x => x.CreatedOn)
                        .Where(x => x.IsActive == true && (page.FkCategoryType == null ? true : x.FkCategoryType == page.FkCategoryType) && (x.CategorySelfId != null && page.CategoryId.Contains(x.CategorySelfId.Value)))
                        .Select(x => new GetCategory
                        {
                            Id = x.Id,
                            IsSubCategory = x.IsSubCategory,
                            Priority = x.Priority,
                            FK_CategoryId = new General<Guid?> { Id = x.CategorySelfId, Name = x.CategorySelf.CategoriesDetails.FirstOrDefault(y => y.FkLanguageId == (int)ELanguage.English && y.IsActive == true).Name },
                            Name = x.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == (int)ELanguage.English).Name,
                            Image = new FileUrlResponce
                            {
                                URL = x.ImageUrl,
                                ThumbnailUrl = x.ImageThumbnailUrl
                            },
                            FkCategoryType = x.FkCategoryType
                            //IsActive = x.IsActive,
                        })
                        .AsQueryable();
                    query = query.OrderBy(x => x.Priority);

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

        public async Task<bool> SaveCategory(CreateCategoryRequest categoryRequest, Guid userId)
        {
            try
            {
                bool response = false;

                List<CategoriesDetails> informations = new List<CategoriesDetails>();

                foreach (var information in categoryRequest.CategoryInformations)
                {
                    informations.Add(new CategoriesDetails
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

                if (categoryRequest.Id == null)
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                await db.Categories.AddAsync(new Categories
                                {
                                    Id = SystemGlobal.GetId(),
                                    CategorySelfId = categoryRequest.CategoryId,
                                    Priority = categoryRequest.Priority,
                                    ImageUrl = categoryRequest.Image.URL,
                                    ImageThumbnailUrl = categoryRequest.Image.ThumbnailUrl,
                                    CategoriesDetails = informations,
                                    IsActive = true,
                                    CreatedBy = userId.ToString(),
                                    CreatedOn = DateTime.UtcNow,
                                    CreatedDate = DateTime.UtcNow,
                                    IsSubCategory = categoryRequest.IsSubCategory,
                                    FkCategoryType = categoryRequest.FkCategoryType
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
                                await db.CategoriesDetails
                                    .Where(x => x.IsActive ==  true && x.FkCategoryId.Equals(categoryRequest.Id))
                                    .ForEachAsync(x =>
                                    {
                                        x.IsActive = false;
                                        x.UpdatedBy = userId.ToString();
                                        x.UpdatedDate = DateTime.UtcNow;
                                    });

                                await db.SaveChangesAsync();

                                //add Newer

                                var category = db.Categories.FirstOrDefault(x => x.Id.Equals(categoryRequest.Id));
                                category.IsSubCategory = categoryRequest.IsSubCategory;
                                category.CategorySelfId = categoryRequest.CategoryId;
                                category.Priority = categoryRequest.Priority;
                                category.FkCategoryType = categoryRequest.FkCategoryType;
                                category.ImageUrl = categoryRequest.Image.URL;
                                category.ImageThumbnailUrl = categoryRequest.Image.ThumbnailUrl;
                                category.IsActive = true;
                                category.UpdatedBy = userId.ToString();
                                category.UpdatedDate = DateTime.UtcNow;

                                db.Entry(category).State = EntityState.Modified;
                                await db.SaveChangesAsync();

                                informations.ForEach(x => x.FkCategoryId = category.Id);
                                await db.CategoriesDetails.AddRangeAsync(informations);

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

        public CreateCategoryRequest GetEditCategory(Guid id, Guid userId)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.Categories.Include(x=> x.CategoriesDetails)
                        .Where(x => x.Id.Equals(id))
                        .Select(x => new CreateCategoryRequest
                        {
                            Id = x.Id,
                            CategoryId = x.CategorySelfId,
                            Priority = x.Priority,
                            FkCategoryType = x.FkCategoryType,
                            IsSubCategory = x.IsSubCategory,
                            Image = new FileUrlResponce
                            {
                                URL = x.ImageUrl,
                                ThumbnailUrl = x.ImageThumbnailUrl
                            },
                            CategoryInformations = x.CategoriesDetails.Where(y => y.IsActive == true).Select(y => new GenericDetailRequest
                            {
                                Id = y.Id,
                                LanguageId = y.FkLanguageId,
                                Name = y.Name
                            }).ToList()
                        })
                        .FirstOrDefault() ?? new CreateCategoryRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ControllCategoryActivation(Guid categoryId, bool activation, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.Categories.FirstOrDefault(x => x.Id.Equals(categoryId));

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

        public List<GetCategory> GetCategoriesList(GetCategoriesRequest page)
        {
            try
            {
                List<GetCategory> response = new List<GetCategory>();

                using (var db = new HilalDbContext())
                {
                    response = db.Categories.Include(x => x.CategorySelf).ThenInclude(x => (x as Categories).CategoriesDetails)
                        .Where(x => x.IsActive == true && (page.FkCategoryType == null ? true : x.FkCategoryType == page.FkCategoryType) && (page.CategoryId == null ? page.SubCategoryList == true ? x.CategorySelfId != null : x.CategorySelfId == null : x.CategorySelfId.Equals(page.CategoryId)))
                        .OrderByDescending(x=> x.CreatedOn)
                        
                        .Select(x => new GetCategory
                        {
                            Id = x.Id,
                            IsSubCategory = x.IsSubCategory,
                            Priority = x.Priority,
                            FK_CategoryId = new General<Guid?> { Id = x.CategorySelfId, Name = x.CategorySelf.CategoriesDetails.FirstOrDefault(y => y.FkLanguageId == page.LanguageId && y.IsActive == true).Name },
                            Name = x.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == page.LanguageId).Name,
                            Image = new FileUrlResponce
                            {
                                URL = x.ImageUrl,
                                ThumbnailUrl = x.ImageThumbnailUrl
                            },
                            FkCategoryType = x.FkCategoryType
                        }).OrderBy(x => x.Priority).ToList();
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
