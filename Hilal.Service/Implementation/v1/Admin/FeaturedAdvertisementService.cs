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
using Hilal.DataViewModel.Response.App.v1;
using Hilal.DataViewModel.Request.App.v1;

namespace Hilal.Service.Implementation.v1.Admin
{
    public class FeaturedAdvertisementService : IFeaturedAdvertisementService
    {
        public GetFeaturedAdvertisementResponse GetFeaturedAdvertisement(ListGeneralModel page)
        {
            try
            {
                GetFeaturedAdvertisementResponse response = new GetFeaturedAdvertisementResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.FeaturedAdvertisements.OrderByDescending(x => x.CreatedDate)
                        .Include(x=> x.FkAdvertisement).ThenInclude(x=> (x as Advertisement).AdvertisementDetails)
                        .OrderByDescending(x => x.CreatedOn)
                        .Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedDate)
                        .Select(x => new GetFeaturedAdvertisementList
                        {
                            Id = x.Id,
                            FK_AdvertisementId = x.FkAdvertisementId,
                            Priority = x.Priority,
                            Title = x.FkAdvertisement.AdvertisementDetails.FirstOrDefault(y=> y.IsActive == true && y.FkLanguageId == page.LanguageId ).Title,
                            RefId = x.FkAdvertisement.RefId
                        })
                        .AsQueryable();

                    if (!string.IsNullOrEmpty(page.Search))
                    {
                        var date = new DateTime();
                        var sdate = DateTime.TryParse(page.Search, out date);
                        int totalCases = -1;
                        var isNumber = Int32.TryParse(page.Search, out totalCases);

                        query = query.Where(
                        x => x.Title.ToLower().Contains(page.Search.ToLower())
                        || x.RefId.ToLower().Contains(page.Search.ToLower())
                    );
                    }

                    var orderedQuery = query;

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

        public async Task<bool> SaveFeaturedAdvertisement(CreateFeaturedAdvertisementRequest FeaturedAdvertisementRequest, Guid userId)
        {
            try
            {
                bool response = false;
                if (FeaturedAdvertisementRequest.Id == null)
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                var feturedAdvertisementObject = db.FeaturedAdvertisements.FirstOrDefault(x => x.FkAdvertisementId == FeaturedAdvertisementRequest.FK_AdvertisementId);
                                if (feturedAdvertisementObject == null)
                                {
                                    await db.FeaturedAdvertisements.AddAsync(new FeaturedAdvertisements
                                    {
                                        Id = SystemGlobal.GetId(),
                                        FkAdvertisementId = FeaturedAdvertisementRequest.FK_AdvertisementId,
                                        Priority = FeaturedAdvertisementRequest.Priority,
                                        IsActive = true,
                                        CreatedBy = userId.ToString(),
                                        CreatedOn = DateTime.UtcNow,
                                        CreatedDate = DateTime.UtcNow
                                    });
                                }
                                else
                                {
                                    feturedAdvertisementObject.FkAdvertisementId = FeaturedAdvertisementRequest.FK_AdvertisementId;
                                    feturedAdvertisementObject.Priority = FeaturedAdvertisementRequest.Priority;
                                    feturedAdvertisementObject.IsActive = true;
                                    feturedAdvertisementObject.UpdatedBy = userId.ToString();
                                    feturedAdvertisementObject.UpdatedDate = DateTime.UtcNow;
                                }
                                
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
                                var category = db.FeaturedAdvertisements.FirstOrDefault(x => x.Id.Equals(FeaturedAdvertisementRequest.Id));
                                
                                category.FkAdvertisementId = FeaturedAdvertisementRequest.FK_AdvertisementId;
                                category.Priority = FeaturedAdvertisementRequest.Priority;
                                category.IsActive = true;
                                category.UpdatedBy = userId.ToString();
                                category.UpdatedDate = DateTime.UtcNow;

                                db.Entry(category).State = EntityState.Modified;
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

        public CreateFeaturedAdvertisementRequest GetEditFeaturedAdvertisement(Guid id)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.FeaturedAdvertisements
                        .Where(x => x.Id.Equals(id))
                        .Select(x => new CreateFeaturedAdvertisementRequest
                        {
                            Id = x.Id,
                            Priority = x.Priority,
                            FK_AdvertisementId = x.FkAdvertisementId,
                        }).FirstOrDefault() ?? new CreateFeaturedAdvertisementRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ControllFeaturedAdvertisementActivation(Guid FeaturedAdvertisementId, bool activation, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.FeaturedAdvertisements.FirstOrDefault(x => x.Id.Equals(FeaturedAdvertisementId));

                    if (category == null) throw new Exception("FeaturedAdvertisementId Doesn't Exists");

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

        public List<GetAdvertisementList> GetFeaturedAdvertisementList(int LanguageId)
        {
            try
            {
                List<GetAdvertisementList> response = new List<GetAdvertisementList>();

                using (var db = new HilalDbContext())
                {
                    var query = db.FeaturedAdvertisements
                        //.Include(x => x.FkAdvertisement)
                        //.ThenInclude(x => (x as Advertisement).FkStatus)
                        //.Include(x => x.FkAdvertisement)
                        //.ThenInclude(x => (x as Advertisement).Attachement)
                        //.Include(x => x.FkAdvertisement)
                        //.ThenInclude(x => (x as Advertisement).FkAge).ThenInclude(x => (x as Age).AgeDetails)
                        //.Include(x => x.FkAdvertisement)
                        //.ThenInclude(x => (x as Advertisement).FkBreed).ThenInclude(x => (x as Breed).BreedDetails)
                        //.Include(x => x.FkAdvertisement)
                        //.ThenInclude(x => (x as Advertisement).FkGender).ThenInclude(x => (x as HilalGenders).GenderDetails)
                        //.Include(x => x.FkAdvertisement)
                        //.ThenInclude(x => (x as Advertisement).FkCity).ThenInclude(x => (x as Cities).Citydetails)
                        //.Include(x => x.FkAdvertisement)
                        //.ThenInclude(x => (x as Advertisement).FkCity).ThenInclude(x => (x as Cities).FkCountryNavigation).ThenInclude(x => (x as Countries).CountryDetails)
                        .OrderBy(x=> x.Priority)
                        .OrderByDescending(x => x.CreatedOn)
                        .Where(x => x.IsActive == true).AsQueryable();

                    var queryable = query.Select(x => new GetAdvertisementList
                    {
                        Id = x.FkAdvertisementId,
                        Category = x.FkAdvertisement.FkCategory.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                        SubCategory = x.FkAdvertisement.FkSubCategory.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                        Age = x.FkAdvertisement.FkAge.AgeDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                        Gender = x.FkAdvertisement.FkGender.GenderDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                        Breed = x.FkAdvertisement.FkBreed.BreedDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                        City = x.FkAdvertisement.FkCity.Citydetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                        Country = x.FkAdvertisement.FkCity.FkCountryNavigation.CountryDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                        Email = x.FkAdvertisement.Email,
                        RefId = x.FkAdvertisement.RefId,
                        Status = x.FkAdvertisement.FkStatus.Name,
                        CreatedDate = x.FkAdvertisement.CreatedOn,
                        PhoneNumber = x.FkAdvertisement.PhoneNumber,
                        SalePrice = x.FkAdvertisement.SalePrice,
                        Title = x.FkAdvertisement.AdvertisementDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Title,
                        WhatsappNumber = x.FkAdvertisement.WhatsAppNumber,
                        Image = new FileUrlResponce { URL = x.FkAdvertisement.Attachement.FirstOrDefault(y => y.IsActive == true && y.IsVideo == false).Url , ThumbnailUrl = x.FkAdvertisement.Attachement.FirstOrDefault(y => y.IsActive == true && y.IsVideo == false).ThumbnilUrl }
                    }).AsQueryable();

                    response = queryable.ToList();
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
