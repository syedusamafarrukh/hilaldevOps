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
using Hilal.Service.Interface.v1.App;
using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Enum;
using Hilal.DataViewModel.Response.App.v1;
using Hilal.DataViewModel.Request.App.v1;
using Hilal.DataViewModel.Request;
using Microsoft.Extensions.Configuration;
using Hilal.DataViewModel.Enum.Admin.v1;

namespace Hilal.Service.Implementation.v1.App
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly IConfiguration configuration;
        public AdvertisementService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public List<GetAdvertisementList> GetAdvertisement(GetAdvertisementRequest advertisementRequest , Guid? userId)
        {
            try
            {
                List<GetAdvertisementList> response = new List<GetAdvertisementList>();

                using (var db = new HilalDbContext())
                {
                    var query = db.Advertisement
                        //.Include(x=> x.AdvertisementDetails)
                        //.Include(x=> x.FkStatus)
                        //.Include(x=> x.FkAge).ThenInclude(x=> (x as Age).AgeDetails)
                        //.Include(x=> x.FkBreed).ThenInclude(x=> (x as Breed).BreedDetails)
                        //.Include(x=> x.FkCategory).ThenInclude(x=> (x as Categories).CategoriesDetails)
                        //.Include(x=> x.FkSubCategory).ThenInclude(x=> (x as Categories).CategoriesDetails)
                        //.Include(x=> x.FkGender).ThenInclude(x=> (x as HilalGenders).GenderDetails)
                        //.Include(x=> x.FkCity).ThenInclude(x=> (x as Cities).Citydetails)
                        //.Include(x=> x.FkCity).ThenInclude(x=> (x as Cities).FkCountryNavigation).ThenInclude(x=> (x as Countries).CountryDetails)
                        //.Include(x => x.ChatThreads)
                        //.Include(x => x.UserBookmarks)
                        .OrderByDescending(x => x.CreatedOn)
                        .Where(x => x.IsActive == true).AsQueryable();

                    if (advertisementRequest.FK_AppUserId != null)
                    {
                        query = query.Where(x => x.FkAppUserId == advertisementRequest.FK_AppUserId);
                    }
                    if (advertisementRequest.FK_StatusId != null)
                    {
                        query = query.Where(x => x.FkStatusId == advertisementRequest.FK_StatusId);
                    }
                    if (advertisementRequest.FK_CategoryId != null)
                    {
                        query = query.Where(x => x.FkCategoryId == advertisementRequest.FK_CategoryId);
                    }
                    if (advertisementRequest.FK_SubCategoryId != null)
                    {
                        query = query.Where(x => x.FkSubCategoryId == advertisementRequest.FK_SubCategoryId);
                    }
                    if (advertisementRequest.FK_BreedId != null)
                    {
                        query = query.Where(x => x.FkBreedId == advertisementRequest.FK_BreedId);
                    }
                    if (advertisementRequest.FK_GenderId != null)
                    {
                        query = query.Where(x => x.FkGenderId == advertisementRequest.FK_GenderId);
                    }
                    if (advertisementRequest.FK_CountryId != null)
                    {
                        query = query.Where(x => x.FkCity.FkCountry == advertisementRequest.FK_CountryId);
                    }
                    if (advertisementRequest.FK_CityId != null)
                    {
                        query = query.Where(x => x.FkCityId == advertisementRequest.FK_CityId);
                    }
                    if (advertisementRequest.FK_AgeId != null)
                    {
                        query = query.Where(x => x.FkAgeId == advertisementRequest.FK_AgeId);
                    }

                    var queryable = query.Select(x => new GetAdvertisementList
                    {
                        Id = x.Id,
                        Category = x.FkCategory.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        SubCategory = x.FkSubCategory.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        Age = x.FkAge.AgeDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        Gender = x.FkGender.GenderDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        Breed = x.FkBreed.BreedDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        CityId = x.FkCityId,
                        CountryId = x.FkCity.FkCountry,
                        City = x.FkCity.Citydetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        Country = x.FkCity.FkCountryNavigation.CountryDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        Email = x.Email,
                        FK_StatusId = x.FkStatusId,
                        CreatedDate = x.CreatedOn,
                        PhoneNumber =  x.PhoneNumberCountryCode +"-" + x.PhoneNumber,
                        WhatsappNumber = x.WhatsAppCountryCode + "-" + x.WhatsAppNumber,
                        SalePrice = x.SalePrice,
                        SellerId = x.FkAppUserId,
                        Status = x.FkStatus.Name,
                        RefId = x.RefId,
                        SellerName = x.AdvertisementDetails.FirstOrDefault(y=> y.FkLanguageId == advertisementRequest.LanguageId && y.IsActive == true).SellerName,
                        UserName = x.FkAppUser.AppUserProfiles.FirstOrDefault(y=> y.IsEnabled== true).Name,
                        IsBookMark = x.UserBookmarks.FirstOrDefault(y => y.FkAppUserId == userId) == null ? false : x.UserBookmarks.FirstOrDefault(y => y.FkAppUserId == userId).IsActive,
                        ThreadId = x.ChatThreads.FirstOrDefault(x => x.IsActive == true && x.FkUserId == userId) == null ? (Guid?)null : x.ChatThreads.FirstOrDefault(x => x.IsActive == true && x.FkUserId == userId).Id,
                        Title = x.AdvertisementDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Title,
                        Image = new FileUrlResponce { URL = x.Attachement.FirstOrDefault(y=> y.IsActive == true && y.IsVideo == false).Url , ThumbnailUrl = x.Attachement.FirstOrDefault(y => y.IsActive == true && y.IsVideo == false).ThumbnilUrl }
                    }).AsQueryable();

                    if (!string.IsNullOrEmpty(advertisementRequest.Search))
                    {
                        var date = new DateTime();
                        var sdate = DateTime.TryParse(advertisementRequest.Search, out date);
                        int totalCases = -1;
                        var isNumber = Int32.TryParse(advertisementRequest.Search, out totalCases);

                        queryable = queryable.Where(
                        x => x.Gender.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.Age.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.RefId.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.Breed.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.City.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.Country.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.Title.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.PhoneNumber.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.PhoneNumber.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.Email.ToLower().Contains(advertisementRequest.Search.ToLower())
                    );
                    }
                    response = queryable.ToList();
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetAppAdvertisementResponse GetFiltersAdvertisements(GetFiltersResquest advertisementRequest)
        {
            try
            {
                GetAppAdvertisementResponse response = new GetAppAdvertisementResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.Advertisement
                        .Include(x => x.AdvertisementDetails)
                        .Include(x => x.FkStatus)
                        .Include(x => x.FkAge).ThenInclude(x => (x as Age).AgeDetails)
                        .Include(x => x.FkBreed).ThenInclude(x => (x as Breed).BreedDetails)
                        .Include(x => x.FkCategory).ThenInclude(x => (x as Categories).CategoriesDetails)
                        .Include(x => x.FkSubCategory).ThenInclude(x => (x as Categories).CategoriesDetails)
                        .Include(x => x.FkGender).ThenInclude(x => (x as HilalGenders).GenderDetails)
                        .Include(x => x.FkCity).ThenInclude(x => (x as Cities).Citydetails)
                        .Include(x => x.FkCity).ThenInclude(x => (x as Cities).FkCountryNavigation).ThenInclude(x => (x as Countries).CountryDetails)
                        .Include(x => x.ChatThreads)
                        .OrderByDescending(x => x.CreatedOn)
                        .Where(x => x.IsActive == true).AsQueryable();

                    if (advertisementRequest.FK_StatusId != null)
                    {
                        query = query.Where(x => x.FkStatusId == advertisementRequest.FK_StatusId);
                    }
                    if (advertisementRequest.Categories != null && advertisementRequest.Categories.Count > 0)
                    {
                        query = query.Where(x => advertisementRequest.Categories.Contains(x.FkCategoryId));
                    }
                    if (advertisementRequest.SubCategories != null && advertisementRequest.SubCategories.Count > 0)
                    {
                        query = query.Where(x => advertisementRequest.SubCategories.Contains(x.FkSubCategoryId));
                    }
                    if (advertisementRequest.Breeds != null && advertisementRequest.Breeds.Count > 0)
                    {
                        query = query.Where(x => advertisementRequest.Breeds.Contains(x.FkBreedId));
                    }
                    if (advertisementRequest.Genders != null && advertisementRequest.Genders.Count > 0)
                    {
                        query = query.Where(x => advertisementRequest.Genders.Contains(x.FkGenderId));
                    }
                    if (advertisementRequest.Ages != null && advertisementRequest.Ages.Count > 0)
                    {
                        query = query.Where(x => advertisementRequest.Ages.Contains(x.FkAgeId));
                    }

                    var queryable = query.Select(x => new GetAdvertisementList
                    {
                        Id = x.Id,
                        Category = x.FkCategory.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        SubCategory = x.FkSubCategory.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        Age = x.FkAge.AgeDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        Gender = x.FkGender.GenderDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        Breed = x.FkBreed.BreedDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        City = x.FkCity.Citydetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        Country = x.FkCity.FkCountryNavigation.CountryDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        Email = x.Email,
                        RefId = x.RefId,
                        Status = x.FkStatus.Name,
                        IsBookMark = false,
                        SellerId = x.FkAppUserId,
                        FK_StatusId = x.FkStatusId,
                        CreatedDate = x.CreatedOn,
                        PhoneNumber = x.PhoneNumberCountryCode + "-" + x.PhoneNumber,
                        WhatsappNumber = x.WhatsAppCountryCode + "-" + x.WhatsAppNumber,
                        SalePrice = x.SalePrice,
                        ThreadId = x.ChatThreads.FirstOrDefault(x => x.IsActive == true).Id,
                        Title = x.AdvertisementDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Title,
                        Image = new FileUrlResponce { URL = x.Attachement.FirstOrDefault(y => y.IsActive == true && y.IsVideo == false).Url, ThumbnailUrl = x.Attachement.FirstOrDefault(y => y.IsActive == true && y.IsVideo == false).ThumbnilUrl, IsVideo = x.Attachement.FirstOrDefault(y => y.IsActive == true && y.IsVideo == false).IsVideo, WaterMarkImage = x.Attachement.FirstOrDefault(y => y.IsActive == true && y.IsVideo == false).WaterMarkImage }
                    }).AsQueryable();

                    if (!string.IsNullOrEmpty(advertisementRequest.Search))
                    {
                        var date = new DateTime();
                        var sdate = DateTime.TryParse(advertisementRequest.Search, out date);
                        int totalCases = -1;
                        var isNumber = Int32.TryParse(advertisementRequest.Search, out totalCases);

                        queryable = queryable.Where(
                        x => x.Gender.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.Age.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.Breed.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.City.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.Country.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.Title.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.PhoneNumber.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.PhoneNumber.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.Email.ToLower().Contains(advertisementRequest.Search.ToLower())
                    );
                    }
                    response.Skip = advertisementRequest.Skip;
                    response.Take = advertisementRequest.Take;
                    response.TotalRecords = queryable.Count();
                    if (response.Take > 0)
                    {
                        response.AdvertisementList = queryable.Skip(response.Skip).Take(response.Take).ToList();
                    }
                    else
                    {
                        response.AdvertisementList = queryable.ToList();
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetAdvertisemetDetail GetAdvertisementDetail(Guid Id, int LanguageId, Guid? userId)
        {
            try
            {
                GetAdvertisemetDetail response = new GetAdvertisemetDetail();

                using (var db = new HilalDbContext())
                {
                    response = db.Advertisement
                        //.Include(x=> x.AdvertisementDetails)
                        //.Include(x=> x.FkStatus)
                        //.Include(x=> x.FkAge).ThenInclude(x=> (x as Age).AgeDetails)
                        //.Include(x=> x.FkBreed).ThenInclude(x=> (x as Breed).BreedDetails)
                        //.Include(x=> x.FkCategory).ThenInclude(x=> (x as Categories).CategoriesDetails)
                        //.Include(x=> x.FkSubCategory).ThenInclude(x=> (x as Categories).CategoriesDetails)
                        //.Include(x=> x.FkGender).ThenInclude(x=> (x as HilalGenders).GenderDetails)
                        //.Include(x=> x.FkCity).ThenInclude(x=> (x as Cities).Citydetails)
                        //.Include(x=> x.FkCity).ThenInclude(x=> (x as Cities).FkCountryNavigation).ThenInclude(x=> (x as Countries).CountryDetails)
                        //.Include(x=> x.ChatThreads)
                        //.Include(x=> x.UserBookmarks)
                        .OrderByDescending(x => x.CreatedDate)
                        .Where(x => x.Id == Id).Select(x => new GetAdvertisemetDetail
                        {
                            Id = x.Id,
                            Category = x.FkCategory.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                            SubCategory = x.FkSubCategory.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                            Age = x.FkAge.AgeDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name == null ? "" : x.FkAge.AgeDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                            Gender = x.FkGender.GenderDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name ==null ? "" : x.FkGender.GenderDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                            Breed = x.FkBreed.BreedDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name == null ? "" : x.FkBreed.BreedDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                            City = x.FkCity.Citydetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                            Country = x.FkCity.FkCountryNavigation.CountryDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                            CityId = x.FkCityId,
                            CountryId = x.FkCity.FkCountry,
                            Status = x.FkStatus.Name,
                            Email = x.Email,
                            SellerId = x.FkAppUserId,
                            CreatedDate = x.CreatedOn,
                            CommissionAmount = x.CommissionAmount,
                            PhoneNumber = x.PhoneNumberCountryCode + "-" + x.PhoneNumber,
                            WhatsappNumber = x.WhatsAppCountryCode + "-" + x.WhatsAppNumber,
                            SalePrice = x.SalePrice,
                            RefId = x.RefId,
                            SellerName = x.AdvertisementDetails.FirstOrDefault(y => y.FkLanguageId == LanguageId).SellerName,
                            UserName = x.FkAppUser.AppUserProfiles.FirstOrDefault(y => y.IsEnabled == true).Name,
                            FK_StatusId = x.FkStatusId,
                            Title = x.AdvertisementDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Title,
                            Address = x.AdvertisementDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Address,
                            Description = x.AdvertisementDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Description,
                            Father = x.AdvertisementDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Father,
                            Mother = x.AdvertisementDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Mother,
                            IsDeliveryAvailable = x.IsDeliveryAvailable,
                            IsBookMark = x.UserBookmarks.FirstOrDefault(y => y.FkAppUserId == userId) == null ? false : x.UserBookmarks.FirstOrDefault(y => y.FkAppUserId == userId).IsActive,
                            ThreadId = x.ChatThreads.FirstOrDefault(x => x.IsActive == true && x.FkUserId == userId) == null ? (Guid?)null : x.ChatThreads.FirstOrDefault(x => x.IsActive == true && x.FkUserId == userId).Id,
                            Attachements = x.Attachement.Where(y=> y.IsActive == true && y.FkAdvertisementId == Id).Select(y=> new GetAttachementsResponseList
                            {
                                Url = y.Url,
                                IsVideo = y.IsVideo,
                                ThubnilUrl = y.ThumbnilUrl,
                                WaterMarkImage = y.WaterMarkImage,
                                ThumbnailUrl = y.ThumbnilUrl,
                            }).ToList(),
                            
                            //Image = new FileUrlResponce { URL = x.Attachement.Where(y=> y.)}
                        }).FirstOrDefault();
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Guid> SaveAdvertisement(CreateAdvertisementRequest advertisementRequest, Guid userId)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    var appUser = db.AppUsers.FirstOrDefault(x => x.Id == userId);
                    if (appUser == null) throw new Exception("User Doesn't Exists");
                    //if (appUser.IsSubscribed == false) throw new Exception("User Didn't Subscribed");
                    //if (appUser.IsSubscribedExpired == true) throw new Exception("User Subscription has Expired");
                }

                bool response = false;
                var idd = SystemGlobal.GetId();
                List<AdvertisementDetails> informations = new List<AdvertisementDetails>();
                List<Attachement> attachementList = new List<Attachement>();

                foreach (var information in advertisementRequest.AdvertisementDetailsInformation)
                {
                    informations.Add(new AdvertisementDetails
                    {
                        Id = SystemGlobal.GetId(),
                        Title = information.Title,
                        Address = information.Address,
                        FkLanguageId = information.FK_LanguageId,
                        Mother = information.Mother,
                        Father = information.Father,
                        Description = information.Description,
                        SellerName = information.SellerName,
                        IsActive = true,
                        CreatedBy = userId.ToString(),
                        CreatedOn = DateTime.UtcNow,
                        CreatedDate = DateTime.UtcNow
                    });
                }

                if (advertisementRequest.Id == null)
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                string NotifyString = "Advertisement is Pending for Approvel";
                                var autoAddApprovel = db.ApprovelSettings.Where(x => x.IsActive == true && x.CategoryType == 1).FirstOrDefault();
                                var userObject = db.AppUsers.FirstOrDefault(x => x.Id == userId);
                                var RefId = GetAdvertisementRefId();
                                var commsisionObject = db.Commission.FirstOrDefault(x => x.FkCategoryId == advertisementRequest.FK_CategoryId && x.IsActive == true && (advertisementRequest.SalePrice != null ? (Convert.ToDouble(advertisementRequest.SalePrice) >= x.StartRange && Convert.ToDouble(advertisementRequest.SalePrice) <= x.EndRange) : (Convert.ToDouble(advertisementRequest.MinimumPrice) >= x.StartRange && Convert.ToDouble(advertisementRequest.MinimumPrice) <= x.EndRange)));
                                if (commsisionObject != null)
                                {
                                    if (advertisementRequest.SalePrice != null)
                                    {
                                        advertisementRequest.CommissionAmount = (advertisementRequest.SalePrice * Convert.ToDecimal(commsisionObject?.Percentage)) / 100;
                                    }
                                    else
                                    {
                                        advertisementRequest.CommissionAmount = (advertisementRequest.MinimumPrice * Convert.ToDecimal(commsisionObject?.Percentage)) / 100;
                                    }
                                }

                                if (advertisementRequest.AttachementList != null && advertisementRequest.AttachementList.Count > 0)
                                {
                                    foreach (var information in advertisementRequest.AttachementList)
                                    {
                                        attachementList.Add(new Attachement
                                        {
                                            Url = information.Url,
                                            ThumbnilUrl = information.ThubnilUrl,
                                            WaterMarkImage = information.WaterMarkImage,
                                            FkAdvertisementId = idd,
                                            CreatedOnDate = DateTime.UtcNow,
                                            CreatedBy = userId.ToString(),
                                            CreatedOn = DateTime.UtcNow,
                                            IsActive = true,
                                            IsVideo = information.IsVideo.Value,
                                        });
                                        await db.SaveChangesAsync();
                                    }
                                }
                                bool abc = Convert.ToBoolean(advertisementRequest.IsSelfPickup);
                                await db.Advertisement.AddAsync(new Advertisement
                                {
                                    Id = idd,
                                    AdvertisementDetails = informations,
                                    Attachement = attachementList,
                                    CommissionAmount = advertisementRequest.CommissionAmount,
                                    Email = advertisementRequest.Email,
                                    IsSelfPickup = Convert.ToBoolean(advertisementRequest.IsSelfPickup),
                                    IsDeliveryAvailable = Convert.ToBoolean(advertisementRequest.IsDeliveryAvailable),
                                    WhatsAppCountryCode = advertisementRequest.WhatsAppNumber?.CountryCode,
                                    WhatsAppNumber = advertisementRequest.WhatsAppNumber?.PhoneNumber,
                                    PhoneNumberCountryCode = advertisementRequest.PhoneNumber?.CountryCode,
                                    PhoneNumber = advertisementRequest.PhoneNumber?.PhoneNumber,
                                    FkCategoryId = advertisementRequest.FK_CategoryId,
                                    FkAppUserId = userId,
                                    FkCityId = advertisementRequest.FK_CityId,
                                    FkStatusId = autoAddApprovel.ApprovelType == (int)EApprovelTypes.AutoApproved ? (int)EStatuses.Approved : (autoAddApprovel.ApprovelType == (int)EApprovelTypes.AutoDisApproved ? (int)EStatuses.DisApproved : (userObject.IsAddAutoApprovel == true ? (int)EStatuses.Approved : (int)EStatuses.Pending)),
                                    FkSubCategoryId = advertisementRequest.FK_SubCategoryId,
                                    FkBreedId = advertisementRequest.FK_BreedId,
                                    FkAgeId = advertisementRequest.FK_AgeId,
                                    FkGenderId = advertisementRequest.FK_GenderId,
                                    MinimumPrice = advertisementRequest.MinimumPrice,
                                    SalePrice = advertisementRequest.SalePrice,
                                    Video = advertisementRequest.Video,
                                    VideoThunbnil = advertisementRequest.VideoThumbnil,
                                    CreatedDate = DateTime.UtcNow,
                                    IsFeatured = false,
                                    IsActive = true,
                                    CreatedBy = userId.ToString(),
                                    CreatedOn = DateTime.UtcNow,
                                    RefId = RefId,
                                    WaterMarkImage = advertisementRequest.WaterMarkImage
                                });

                                await db.SaveChangesAsync();

                                if (autoAddApprovel.ApprovelType == (int)EApprovelTypes.AutoApproved)
                                {
                                    NotifyString = "Advertisement is Auto Approved";
                                }
                                else if (autoAddApprovel.ApprovelType == (int)EApprovelTypes.AutoDisApproved)
                                {
                                    NotifyString = "Advertisement is Auto DisApproved";
                                }

                                await db.AdvertisementNotifications.AddAsync(new AdvertisementNotifications
                                {
                                    Id = SystemGlobal.GetId(),
                                    FkAdvertisementId = idd,
                                    IsAdminNotify = true,
                                    BodyText = advertisementRequest.AdvertisementDetailsInformation.FirstOrDefault(x => x.FK_LanguageId == (int)ELanguage.English).Title + "/" + NotifyString,
                                    DeviceToken = "",
                                    IsSeen = false,
                                    CreatedDate = DateTime.UtcNow,
                                    IsActive = true,
                                    CreatedBy = userId.ToString(),
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
                    string NotifyString = "Advertisement is again Pending for Approvel";
                    idd = advertisementRequest.Id.Value;
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                await db.AdvertisementDetails
                                    .Where(x => x.IsActive == true && x.FkAdvertisementId.Equals(advertisementRequest.Id))
                                    .ForEachAsync(x =>
                                    {
                                        x.IsActive = false;
                                        x.UpdatedBy = userId.ToString();
                                        x.UpdatedDate = DateTime.UtcNow;
                                    });

                                await db.Attachement
                                    .Where(x => x.IsActive == true && x.FkAdvertisementId.Equals(advertisementRequest.Id))
                                    .ForEachAsync(x =>
                                    {
                                        x.IsActive = false;
                                        x.UpdatedBy = userId.ToString();
                                        x.UpdatedOn = DateTime.UtcNow;
                                    });

                                await db.SaveChangesAsync();

                                //add Newer
                                if (advertisementRequest.AttachementList != null && advertisementRequest.AttachementList.Count > 0)
                                {
                                    foreach (var information in advertisementRequest.AttachementList)
                                    {
                                        attachementList.Add(new Attachement
                                        {
                                            Url = information.Url,
                                            ThumbnilUrl = information.ThubnilUrl,
                                            FkAdvertisementId = idd,
                                            CreatedOnDate = DateTime.UtcNow,
                                            CreatedBy = userId.ToString(),
                                            CreatedOn = DateTime.UtcNow,
                                            IsActive = true,
                                            IsVideo = information.IsVideo.Value,
                                        });
                                        await db.SaveChangesAsync();
                                    }
                                }
                                var category = db.Advertisement.FirstOrDefault(x => x.Id.Equals(advertisementRequest.Id));

                                var autoAddApprovel = db.ApprovelSettings.FirstOrDefault(x => x.IsActive == true && x.CategoryType == (int)ECategoryType.Seller);
                                if (autoAddApprovel.ApprovelType == (int)EApprovelTypes.AutoApproved)
                                {
                                    category.FkStatusId = (int)EStatuses.Approved;
                                    NotifyString = "Advertisement is agin Auto Approved";
                                }
                                else if (autoAddApprovel.ApprovelType == (int)EApprovelTypes.AutoDisApproved)
                                {
                                    category.FkStatusId = (int)EStatuses.DisApproved;
                                    NotifyString = "Advertisement is again Auto DisApproved";
                                }
                                else
                                {
                                    category.FkStatusId = (int)EStatuses.Pending;
                                    var userObject = db.AppUsers.FirstOrDefault(x => x.Id == userId);
                                    if (userObject != null)
                                    {
                                        if (userObject.IsAddAutoApprovel == true)
                                        {
                                            category.FkStatusId = (int)EStatuses.Approved;
                                        }
                                    }
                                }

                                category.WhatsAppNumber = advertisementRequest.WhatsAppNumber.PhoneNumber;
                                category.WhatsAppCountryCode = advertisementRequest.WhatsAppNumber.CountryCode;
                                category.PhoneNumber = advertisementRequest.PhoneNumber.PhoneNumber;
                                category.PhoneNumberCountryCode = advertisementRequest.PhoneNumber.CountryCode;
                                category.Email = advertisementRequest.Email;
                                category.FkAgeId = advertisementRequest.FK_AgeId;
                                category.FkBreedId = advertisementRequest.FK_BreedId;
                                category.FkCategoryId = advertisementRequest.FK_CategoryId;
                                category.FkCityId = advertisementRequest.FK_CityId;
                                category.FkGenderId = advertisementRequest.FK_GenderId;
                                category.FkSubCategoryId = advertisementRequest.FK_SubCategoryId;
                                category.IsDeliveryAvailable = Convert.ToBoolean(advertisementRequest.IsDeliveryAvailable);
                                category.IsSelfPickup = Convert.ToBoolean(advertisementRequest.IsSelfPickup);
                                category.MinimumPrice = advertisementRequest.MinimumPrice;
                                category.SalePrice = advertisementRequest.SalePrice;
                                category.Video = advertisementRequest.Video;
                                category.WaterMarkImage = advertisementRequest.WaterMarkImage;
                                category.VideoThunbnil = advertisementRequest.VideoThumbnil;
                                category.UpdatedBy = userId.ToString();
                                category.UpdatedDate = DateTime.UtcNow;
                                db.Entry(category).State = EntityState.Modified;
                                await db.SaveChangesAsync();

                                informations.ForEach(x => x.FkAdvertisementId = category.Id);
                                await db.AdvertisementDetails.AddRangeAsync(informations);

                                attachementList.ForEach(x => x.FkAdvertisementId = category.Id);
                                await db.Attachement.AddRangeAsync(attachementList);

                                await db.AdvertisementNotifications.AddAsync(new AdvertisementNotifications
                                {
                                    Id = SystemGlobal.GetId(),
                                    FkAdvertisementId = category.Id,
                                    IsAdminNotify = true,
                                    BodyText = category.AdvertisementDetails.FirstOrDefault(x => x.IsActive == true && x.FkLanguageId == (int)ELanguage.English).Title + "/" + NotifyString,
                                    DeviceToken = "",
                                    IsSeen = false,
                                    CreatedDate = DateTime.UtcNow,
                                    IsActive = true,
                                    CreatedBy = userId.ToString(),
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

                return idd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CreateAdvertisementCommission(CreateCommisionPaymentRequest CreateAdvertisementCommission, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    using (var trans = db.Database.BeginTransaction())
                    {
                        try
                        {
                            string NotifyString = "Advertisement is Pending for Approvel";
                            var checkStatus = NetworkGlobal.checkOrderStatus(CreateAdvertisementCommission.OrderRefId);
                            if (checkStatus == "CAPTURED" || checkStatus == "SUCCESS")
                            {
                                var AppUserSubscription = db.AdvertisementCommission.Where(x => x.FkAdvertisementId == CreateAdvertisementCommission.AdvertisementId && x.IsActive == true).FirstOrDefault();
                                if (AppUserSubscription == null)
                                {
                                    var advertisementObject = db.Advertisement.FirstOrDefault(x => x.Id == CreateAdvertisementCommission.AdvertisementId);
                                    await db.AdvertisementCommission.AddAsync(new AdvertisementCommission
                                    {
                                        Id = SystemGlobal.GetId(),
                                        FkAdvertisementId = CreateAdvertisementCommission.AdvertisementId,
                                        Commission = advertisementObject.CommissionAmount == null ? 0 : advertisementObject.CommissionAmount.Value,
                                        CreatedDate = DateTime.UtcNow,
                                        IsActive = true,
                                        CreatedBy = userId.ToString(),
                                        CreatedOn = DateTime.UtcNow,
                                    });

                                    if (advertisementObject != null)
                                    {
                                        var autoAddApprovel = db.ApprovelSettings.Where(x=> x.IsActive == true && x.CategoryType == 1).FirstOrDefault();
                                        if (autoAddApprovel.ApprovelType == (int) EApprovelTypes.AutoApproved)
                                        {
                                            advertisementObject.FkStatusId = (int)EStatuses.Approved;
                                            NotifyString = "Advertisement is Auto Approved";
                                        }
                                        else if(autoAddApprovel.ApprovelType == (int)EApprovelTypes.AutoDisApproved)
                                        {
                                            advertisementObject.FkStatusId = (int)EStatuses.DisApproved;
                                            NotifyString = "Advertisement is Auto DisApproved";
                                        }
                                        else
                                        {
                                            advertisementObject.FkStatusId = (int)EStatuses.Pending;
                                            var userObject = db.AppUsers.FirstOrDefault(x => x.Id == userId);
                                            if (userObject != null)
                                            {
                                                if (userObject.IsAddAutoApprovel == true)
                                                {
                                                    advertisementObject.FkStatusId = (int)EStatuses.Approved;
                                                }
                                            }
                                        }

                                        await db.AdvertisementNotifications.AddAsync(new AdvertisementNotifications
                                        {
                                            Id = SystemGlobal.GetId(),
                                            FkAdvertisementId = CreateAdvertisementCommission.AdvertisementId,
                                            IsAdminNotify = true,
                                            BodyText = advertisementObject.AdvertisementDetails.FirstOrDefault(x => x.IsActive == true && x.FkLanguageId == (int)ELanguage.English).Title + "/" + NotifyString,
                                            DeviceToken = "",
                                            IsSeen = false,
                                            CreatedDate = DateTime.UtcNow,
                                            IsActive = true,
                                            CreatedBy = userId.ToString(),
                                        });
                                        await db.SaveChangesAsync();
                                    }
                                }
                                else
                                {
                                    AppUserSubscription.IsActive = false;
                                    await db.SaveChangesAsync();
                                    var advertisementObject = db.Advertisement.FirstOrDefault(x => x.Id == CreateAdvertisementCommission.AdvertisementId);
                                    await db.AdvertisementCommission.AddAsync(new AdvertisementCommission
                                    {
                                        Id = SystemGlobal.GetId(),
                                        FkAdvertisementId = CreateAdvertisementCommission.AdvertisementId,
                                        Commission = advertisementObject.CommissionAmount == null ? 0 : advertisementObject.CommissionAmount.Value,
                                        CreatedDate = DateTime.UtcNow,
                                        IsActive = true,
                                        CreatedBy = userId.ToString(),
                                        CreatedOn = DateTime.UtcNow,
                                    });

                                    if (advertisementObject != null)
                                    {
                                        var autoAddApprovel = db.ApprovelSettings.FirstOrDefault(x => x.IsActive == true && x.CategoryType == (int)ECategoryType.Seller);
                                        if (autoAddApprovel.ApprovelType == (int)EApprovelTypes.AutoApproved)
                                        {
                                            advertisementObject.FkStatusId = (int)EStatuses.Approved;
                                            NotifyString = "Advertisement is Auto Approved";
                                        }
                                        else if (autoAddApprovel.ApprovelType == (int)EApprovelTypes.AutoDisApproved)
                                        {
                                            advertisementObject.FkStatusId = (int)EStatuses.DisApproved;
                                            NotifyString = "Advertisement is Auto DisApproved";
                                        }
                                        else
                                        {
                                            advertisementObject.FkStatusId = (int)EStatuses.Pending;
                                            var userObject = db.AppUsers.FirstOrDefault(x => x.Id == userId);
                                            if (userObject != null)
                                            {
                                                if (userObject.IsAddAutoApprovel == true)
                                                {
                                                    advertisementObject.FkStatusId = (int)EStatuses.Approved;
                                                }
                                            }
                                        }
                                        await db.SaveChangesAsync();

                                        await db.AdvertisementNotifications.AddAsync(new AdvertisementNotifications
                                        {
                                            Id = SystemGlobal.GetId(),
                                            FkAdvertisementId = CreateAdvertisementCommission.AdvertisementId,
                                            IsAdminNotify = true,
                                            BodyText = advertisementObject.AdvertisementDetails.FirstOrDefault(x => x.IsActive == true && x.FkLanguageId == (int)ELanguage.English).Title + "/" + NotifyString,
                                            DeviceToken = "",
                                            IsSeen = false,
                                            CreatedDate = DateTime.UtcNow,
                                            IsActive = true,
                                            CreatedBy = userId.ToString(),
                                        });
                                        await db.SaveChangesAsync();
                                    }
                                }
                                response = true;
                            }

                            await db.SaveChangesAsync();
                            trans.Commit();

                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw ex;
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

        public CreateAdvertisementRequest GetEditAdvertisement(Guid id)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.Advertisement
                        .Where(x => x.Id.Equals(id))
                        .Select(x => new CreateAdvertisementRequest
                        {
                            Id = x.Id,
                            FK_SubCategoryId = x.FkSubCategoryId,
                            FK_AppUserId = x.FkAppUserId,
                            FK_StatusId = x.FkStatusId,
                            FK_CategoryId = x.FkCategoryId,
                            FK_CityId = x.FkCityId,
                            FK_AgeId = x.FkAgeId,
                            FK_GenderId = x.FkGenderId,
                            FK_BreedId = x.FkBreedId,
                            countryId = x.FkCity.FkCountry,
                            IsDeliveryAvailable =Convert.ToString(x.IsDeliveryAvailable),
                            IsSelfPickup = Convert.ToString(x.IsSelfPickup),
                            Video = x.Video,
                            Email = x.Email,
                            CommissionAmount = x.CommissionAmount,
                            MinimumPrice = x.MinimumPrice,
                            PhoneNumber = new PhoneNumberModel { CountryCode = x.PhoneNumberCountryCode, PhoneNumber = x.PhoneNumber },
                            WhatsAppNumber = new PhoneNumberModel { CountryCode = x.WhatsAppCountryCode, PhoneNumber = x.WhatsAppNumber },
                            SalePrice = x.SalePrice,
                            AdvertisementDetailsInformation = x.AdvertisementDetails.Where(z=> z.IsActive==  true).Select(z=> new CreateAdvertisementDetails
                            {
                                Address = z.Address,
                                FK_LanguageId = z.FkLanguageId,
                                SellerName = z.SellerName,
                                Mother = z.Mother,
                                Father = z.Father,
                                Id = z.Id,
                                Title = z.Title,
                                FK_AdvertisementId = z.FkAdvertisementId,
                                Description = z.Description,
                            }).ToList(),
                            AttachementList = x.Attachement.Where(y => y.IsActive == true).Select(y => new CreateAttachementsRequest
                            {
                                Id = y.Id,
                                FK_AdvertisementId = y.FkAdvertisementId,
                                Url = y.Url,
                                IsVideo = y.IsVideo,
                                WaterMarkImage = y.WaterMarkImage,
                                ThubnilUrl = y.ThumbnilUrl,
                            }).ToList()
                        })
                        .FirstOrDefault() ?? new CreateAdvertisementRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public getEditAdvertisementRequest GetEditAdvertisementforAPP(Guid id, long LanguageId)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.Advertisement
                        .Include(x=> x.FkCategory).ThenInclude(x=> (x as Categories).CategoriesDetails)
                        .Include(x=> x.FkSubCategory).ThenInclude(x=> (x as Categories).CategoriesDetails)
                        .Include(x=> x.FkAge).ThenInclude(x=> (x as Age).AgeDetails)
                        .Include(x=> x.FkBreed).ThenInclude(x=> (x as Breed).BreedDetails)
                        .Include(x=> x.FkGender).ThenInclude(x=> (x as HilalGenders).GenderDetails)
                        .Include(x=> x.FkAppUser).ThenInclude(x=> (x as AppUsers).AppUserProfiles)
                        .Include(x=> x.FkCity).ThenInclude(x=> (x as Cities).Citydetails)
                        .Include(x => x.FkCity).ThenInclude(x=> (x as Cities).FkCountryNavigation).ThenInclude(x=> (x as Countries).CountryDetails)
                        .Where(x => x.Id.Equals(id))
                        .Select(x => new getEditAdvertisementRequest
                        {
                            Id = x.Id,
                            FK_SubCategoryId = new General<Guid?> { Id = x.FkSubCategoryId , Name = x.FkSubCategory.CategoriesDetails.FirstOrDefault(y=> y.IsActive == true && y.FkLanguageId == LanguageId).Name },
                            FK_AppUserId = new General<Guid?> { Id = x.FkAppUserId, Name = x.FkAppUser.AppUserProfiles.FirstOrDefault(y => y.IsEnabled == true).Name },
                            FK_StatusId = x.FkStatusId,
                            FK_CategoryId = new General<Guid> { Id = x.FkCategoryId, Name = x.FkCategory.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name },
                            FK_CityId = new General<Guid> { Id = x.FkCityId, Name = x.FkCity.Citydetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name },
                            FK_CountryId = new General<Guid?> { Id = x.FkCity.FkCountry , Name = x.FkCity.FkCountryNavigation.CountryDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name },
                            FK_AgeId = new General<Guid?> { Id = x.FkAgeId, Name = x.FkAge.AgeDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name },
                            FK_GenderId = new General<Guid?> { Id = x.FkGenderId, Name = x.FkGender.GenderDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name },
                            FK_BreedId = new General<Guid?> { Id = x.FkBreedId, Name = x.FkBreed.BreedDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name },
                            IsDeliveryAvailable =Convert.ToString(x.IsDeliveryAvailable),
                            IsSelfPickup = Convert.ToString(x.IsSelfPickup),
                            Video = x.Video,
                            Email = x.Email,
                            CommissionAmount = x.CommissionAmount,
                            MinimumPrice = x.MinimumPrice,
                            PhoneNumber = new PhoneNumberModel { CountryCode = x.PhoneNumberCountryCode, PhoneNumber = x.PhoneNumber },
                            WhatsAppNumber = new PhoneNumberModel { CountryCode = x.WhatsAppCountryCode, PhoneNumber = x.WhatsAppNumber },
                            SalePrice = x.SalePrice,
                            AdvertisementDetailsInformation = x.AdvertisementDetails.Where(z=> z.IsActive==  true).Select(z=> new CreateAdvertisementDetails
                            {
                                Address = z.Address,
                                FK_LanguageId = z.FkLanguageId,
                                SellerName = z.SellerName,
                                Id = z.Id,
                                Title = z.Title,
                                FK_AdvertisementId = z.FkAdvertisementId,
                                Description = z.Description,
                                Father = z.Father,
                                Mother = z.Mother
                            }).ToList(),
                            AttachementList = x.Attachement.Where(y => y.IsActive == true).Select(y => new CreateAttachementsRequest
                            {
                                Id = y.Id,
                                FK_AdvertisementId = y.FkAdvertisementId,
                                Url = y.Url,
                                IsVideo = y.IsVideo,
                                WaterMarkImage = y.WaterMarkImage,
                                ThubnilUrl = y.ThumbnilUrl
                            }).ToList()
                        })
                        .FirstOrDefault() ?? new getEditAdvertisementRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ControllAdvertisementActivation(Guid AdvertisementId, bool activation, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.Advertisement.FirstOrDefault(x => x.Id.Equals(AdvertisementId));

                    if (category == null) throw new Exception("AdvertisementId Doesn't Exists");

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

        public async Task<bool> ControllAdvertisementStatus(Guid AdvertisementId, int StatusId, Guid userId, String Comments)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.Advertisement.Include(x=> x.FkAppUser).ThenInclude(x=> (x as AppUsers).UserDeviceInformations)
                        .Include(x=> x.FkStatus).Include(x=> x.AdvertisementDetails)
                        .FirstOrDefault(x => x.Id.Equals(AdvertisementId));

                    if (category == null) throw new Exception("AdvertisementId Doesn't Exists");

                    category.FkStatusId = StatusId;
                    category.UpdatedBy = userId.ToString();
                    category.UpdatedDate = DateTime.UtcNow;

                    db.Entry(category).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    if (!String.IsNullOrEmpty(Comments))
                    {
                        await db.Comments.AddAsync(new Comments
                        {
                            Id = SystemGlobal.GetId(),
                            Comments1 = Comments,
                            FkAdvertisementId = category.Id,
                            CreatedBy = userId.ToString(),
                            CreatedDate = DateTime.UtcNow,
                            CreatedOn = DateTime.UtcNow,
                            IsActive = true
                        });
                        await db.SaveChangesAsync();
                    }

                    var stringNotify  = NotificationsStrings.GetStringNotification(category.FkStatusId, category.AdvertisementDetails.FirstOrDefault(x => x.FkLanguageId == (int)ELanguage.English)?.Title, 1);
                    var Body = stringNotify;
                    var Body1 = stringNotify;
                    var devicetoken = category.FkAppUser.UserDeviceInformations.OrderByDescending(x=> x.CreatedOn).FirstOrDefault(x=> x.IsEnabled == true)?.DeviceToken;
                    await db.AdvertisementNotifications.AddAsync(new AdvertisementNotifications
                    {
                        Id = SystemGlobal.GetId(),
                        FkAdvertisementId = category.Id,
                        IsAdminNotify = false,
                        ReceiverId = category.FkAppUserId,
                        BodyText = Body,
                        DeviceToken = devicetoken == null ? "" : devicetoken,
                        IsSeen = false,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        CreatedBy = userId.ToString(),
                    });
                    await db.SaveChangesAsync();

                    var newObj = new
                    {
                        AdvertisementId = category.Id,
                        UserId = category.FkAppUserId,
                        isAdvertisement = true,
                        Type = (int)ENotificationType.AdvertisementDetail
                    };

                    if (StatusId == (int) EStatuses.DisApproved)
                    {
                        Body1 = Comments;
                    }
                    else
                    {
                        Body1 = "";
                    }

                    if (devicetoken != null)
                    {
                        FCMNotification.Sentnotify(configuration.GetValue<string>("FCM:ServerKey"), configuration.GetValue<string>("FCM:SenderId"), devicetoken, Body1, Body, "", newObj, newObj.Type);
                    }

                    response = true;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ControllAdvertisementStatusfromApp(Guid AdvertisementId, int StatusId, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.Advertisement.Include(x => x.FkAppUser).ThenInclude(x => (x as AppUsers).UserDeviceInformations)
                        .Include(x => x.FkStatus).Include(x => x.AdvertisementDetails)
                        .FirstOrDefault(x => x.Id.Equals(AdvertisementId));

                    if (category == null) throw new Exception("AdvertisementId Doesn't Exists");

                    category.FkStatusId = StatusId;
                    category.UpdatedBy = userId.ToString();
                    category.UpdatedDate = DateTime.UtcNow;

                    db.Entry(category).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    //var stringNotify = NotificationsStrings.GetStringNotification(category.FkStatusId, category.AdvertisementDetails.FirstOrDefault(x => x.FkLanguageId == (int)ELanguage.English)?.Title, 1);
                    //var Body = stringNotify;
                    //var devicetoken = category.FkAppUser.UserDeviceInformations.OrderByDescending(x => x.CreatedOn).FirstOrDefault(x => x.IsEnabled == true)?.DeviceToken;
                    //await db.AdvertisementNotifications.AddAsync(new AdvertisementNotifications
                    //{
                    //    Id = SystemGlobal.GetId(),
                    //    FkAdvertisementId = category.Id,
                    //    IsAdminNotify = false,
                    //    BodyText = Body,
                    //    DeviceToken = devicetoken == null ? "" : devicetoken,
                    //    IsSeen = false,
                    //    CreatedDate = DateTime.UtcNow,
                    //    IsActive = true,
                    //    CreatedBy = userId.ToString(),
                    //});
                    //await db.SaveChangesAsync();

                    //object newObj = new
                    //{
                    //    AdvertisementId = category.Id,
                    //    UserId = category.FkAppUserId,
                    //};

                    //if (devicetoken != null)
                    //{
                    //    FCMNotification.Sentnotify(configuration.GetValue<string>("FCM:ServerKey"), configuration.GetValue<string>("FCM:SenderId"), devicetoken, Body, Body, "", newObj);
                    //}

                    response = true;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetAppAdvertisementResponse GetAdvertisementServicePList(GetAdvertisementRequest advertisementRequest, Guid? userId, int adminType)
        {
            try
            {
                GetAppAdvertisementResponse response = new GetAppAdvertisementResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.Advertisement
                        .Include(x => x.AdvertisementDetails)
                        .Include(x => x.FkAge).ThenInclude(x => (x as Age).AgeDetails)
                        .Include(x => x.FkBreed).ThenInclude(x => (x as Breed).BreedDetails)
                        .Include(x => x.FkCategory).ThenInclude(x => (x as Categories).CategoriesDetails)
                        .Include(x => x.FkSubCategory).ThenInclude(x => (x as Categories).CategoriesDetails)
                        .Include(x => x.FkGender).ThenInclude(x => (x as HilalGenders).GenderDetails)
                        .Include(x => x.FkCity).ThenInclude(x => (x as Cities).Citydetails)
                        .Include(x => x.FkCity).ThenInclude(x => (x as Cities).FkCountryNavigation).ThenInclude(x => (x as Countries).CountryDetails)
                        .Include(x => x.UserBookmarks)
                        .Include(x => x.Comments)
                        .OrderByDescending(x=> x.CreatedOn)
                        .Where(x => x.IsActive == true && (adminType == 1 ? x.FkStatusId != 1 : true)).AsQueryable();

                    //if (advertisementRequest.FK_AppUserId != null)
                    //{
                    //    query = query.Where(x => x.FkAppUserId == advertisementRequest.FK_AppUserId);
                    //}
                    //if (advertisementRequest.FK_StatusId != null)
                    //{
                    //    query = query.Where(x => x.FkStatusId == advertisementRequest.FK_StatusId);
                    //}
                    //if (advertisementRequest.FK_CategoryId != null)
                    //{
                    //    query = query.Where(x => x.FkCategoryId == advertisementRequest.FK_CategoryId);
                    //}
                    //if (advertisementRequest.FK_SubCategoryId != null)
                    //{
                    //    query = query.Where(x => x.FkSubCategoryId == advertisementRequest.FK_SubCategoryId);
                    //}
                    //if (advertisementRequest.FK_BreedId != null)
                    //{
                    //    query = query.Where(x => x.FkBreedId == advertisementRequest.FK_BreedId);
                    //}
                    //if (advertisementRequest.FK_GenderId != null)
                    //{
                    //    query = query.Where(x => x.FkGenderId == advertisementRequest.FK_GenderId);
                    //}
                    //if (advertisementRequest.FK_CountryId != null)
                    //{
                    //    query = query.Where(x => x.FkCity.FkCountry == advertisementRequest.FK_CountryId);
                    //}
                    //if (advertisementRequest.FK_CityId != null)
                    //{
                    //    query = query.Where(x => x.FkCityId == advertisementRequest.FK_CityId);
                    //}
                    //if (advertisementRequest.FK_AgeId != null)
                    //{
                    //    query = query.Where(x => x.FkAgeId == advertisementRequest.FK_AgeId);
                    //}

                    var orderedQuery = query.Where(x=> x.IsActive == true).Select(x => new GetAdvertisementList
                    {
                        Id = x.Id,
                        Category = x.FkCategory.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        SubCategory = x.FkSubCategory.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        Age = x.FkAge.AgeDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        Gender = x.FkGender.GenderDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        Breed = x.FkBreed.BreedDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        City = x.FkCity.Citydetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        Country = x.FkCity.FkCountryNavigation.CountryDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Name,
                        CityId =x.FkCityId,
                        CountryId = x.FkCity.FkCountry,
                        Email = x.Email,
                        Status = x.FkStatus.Name,
                        CreatedDate = x.CreatedOn,
                        FK_StatusId = x.FkStatusId,
                        RefId = x.RefId,
                        PhoneNumber = x.PhoneNumberCountryCode + "-" + x.PhoneNumber,
                        WhatsappNumber = x.WhatsAppCountryCode + "-" + x.WhatsAppNumber,
                        SellerId =x.FkAppUserId,
                        SalePrice = x.SalePrice,
                        Comments = x.Comments.OrderByDescending(y=> y.CreatedOn).FirstOrDefault(y=> y.IsActive == true).Comments1,
                        SellerName = x.AdvertisementDetails.FirstOrDefault(y => y.FkLanguageId == advertisementRequest.LanguageId && y.IsActive == true).SellerName,
                        UserName = x.FkAppUser.AppUserProfiles.FirstOrDefault(y => y.IsEnabled == true).Name,
                        IsBookMark = x.UserBookmarks.FirstOrDefault(y => y.FkAppUserId == userId) == null ? false : x.UserBookmarks.FirstOrDefault(y => y.FkAppUserId == userId).IsActive,
                        ThreadId = x.ChatThreads.FirstOrDefault(x => x.IsActive == true && x.FkUserId == userId) == null ? (Guid?) null : x.ChatThreads.FirstOrDefault(x => x.IsActive == true && x.FkUserId == userId).Id,
                        Title = x.AdvertisementDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == advertisementRequest.LanguageId).Title,
                        Image = new FileUrlResponce { URL = x.Attachement.FirstOrDefault(y=> y.IsActive == true && y.IsVideo == false).Url , ThumbnailUrl = x.Attachement.FirstOrDefault(y => y.IsActive == true && y.IsVideo == false).ThumbnilUrl, WaterMarkImage = x.Attachement.FirstOrDefault(y => y.IsActive == true && y.IsVideo == false).WaterMarkImage }
                    }).AsQueryable();

                    if (!string.IsNullOrEmpty(advertisementRequest.Search))
                    {
                        var date = new DateTime();
                        var sdate = DateTime.TryParse(advertisementRequest.Search, out date);
                        int totalCases = -1;
                        var isNumber = Int32.TryParse(advertisementRequest.Search, out totalCases);

                        orderedQuery = orderedQuery.Where(
                        x => x.Gender.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.RefId.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.Age.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.Breed.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.City.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.Country.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.Title.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.PhoneNumber.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.PhoneNumber.ToLower().Contains(advertisementRequest.Search.ToLower())
                        || x.Email.ToLower().Contains(advertisementRequest.Search.ToLower())
                    );
                    }
                    response.Skip = advertisementRequest.Skip;
                    response.Take = advertisementRequest.Take;
                    response.TotalRecords = orderedQuery.Count();
                    if (response.Take > 0)
                    {
                        response.AdvertisementList = orderedQuery.Skip(response.Skip).Take(response.Take).ToList();
                    }
                    else
                    {
                        response.AdvertisementList = orderedQuery.ToList();
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GetAppAdvertisementResponse> GetAdvertisementService(GetAdvertisementRequest advertisementRequest, Guid? userId, int adminType)
        {
            GetAppAdvertisementResponse response = new GetAppAdvertisementResponse();
            try
            {
                using (var db = new HilalDbContext())
                {
                    if (advertisementRequest.Take < 1)
                    {
                        advertisementRequest.Take = 10000;
                    }
                    //string query = @"EXEC[dbo].[GetAdvertisements] @languageId = '" + advertisementRequest.LanguageId + "', @Search = '" + advertisementRequest.Search + "', " + (advertisementRequest.FK_CategoryId == null ? "@FK_CategoryId = NULL" : "@FK_CategoryId = '" + advertisementRequest.FK_CategoryId.ToString() + "',") + (advertisementRequest.FK_SubCategoryId == null ? "@FK_SubCategoryId = NULL" : "@FK_SubCategoryId = '" + advertisementRequest.FK_SubCategoryId.ToString() + "',") + (advertisementRequest.FK_AppUserId == null ? "@FK_AppUserId = NULL" : "@FK_AppUserId = '" + advertisementRequest.FK_AppUserId.ToString() + "',") + (advertisementRequest.FK_BreedId == null ? "@FK_BreedId = NULL" : "@FK_BreedId = '" + advertisementRequest.FK_BreedId.ToString() + "',") + (advertisementRequest.FK_AgeId == null ? "@FK_AgeId = NULL" : "@FK_AgeId = '" + advertisementRequest.FK_AgeId.ToString() + "',") + (advertisementRequest.FK_GenderId == null ? "@FK_GenderId = NULL" : "@FK_GenderId = '" + advertisementRequest.FK_GenderId.ToString() + "',") + (advertisementRequest.FK_CityId == null ? "@FK_CityId = NULL" : "@FK_CityId = '," + advertisementRequest.FK_CityId.ToString() + "',") + (advertisementRequest.FK_CountryId == null ? "@FK_CountryId = NULL" : "@FK_CountryId = '" + advertisementRequest.FK_CountryId + "',") + (advertisementRequest.FK_StatusId == null ? "@FK_StatusId = NULL" : "@FK_StatusId = '" + advertisementRequest.FK_StatusId + "'") + ", @Skip = '" + advertisementRequest.Skip + "', @Take = '" + advertisementRequest.Take + "'";
                    //string query = @"EXEC[dbo].[GetAdvertisements] @languageId = '" + advertisementRequest.LanguageId + "', @Search = '" + advertisementRequest.Search + "', " + (advertisementRequest.FK_CategoryId.ToString().Equals("null") ? "@FK_CategoryId = NULL" : "@FK_CategoryId = '" + advertisementRequest.FK_CategoryId + "',") + (advertisementRequest.FK_SubCategoryId == null ? "@FK_SubCategoryId = NULL" : "@FK_SubCategoryId = '" + advertisementRequest.FK_SubCategoryId.ToString() + "',") + (advertisementRequest.FK_AppUserId == null ? "@FK_AppUserId = NULL" : "@FK_AppUserId = '" + advertisementRequest.FK_AppUserId.ToString() + "',") + (advertisementRequest.FK_BreedId == null ? "@FK_BreedId = NULL" : "@FK_BreedId = '" + advertisementRequest.FK_BreedId.ToString() + "',") + (advertisementRequest.FK_AgeId == null ? "@FK_AgeId = NULL" : "@FK_AgeId = '" + advertisementRequest.FK_AgeId.ToString() + "',") + (advertisementRequest.FK_GenderId == null ? "@FK_GenderId = NULL" : "@FK_GenderId = '" + advertisementRequest.FK_GenderId.ToString() + "',") + (advertisementRequest.FK_CityId == null ? "@FK_CityId = NULL" : "@FK_CityId = '," + advertisementRequest.FK_CityId.ToString() + "',") + (advertisementRequest.FK_CountryId == null ? "@FK_CountryId = NULL" : "@FK_CountryId = '" + advertisementRequest.FK_CountryId + "',") + (advertisementRequest.FK_StatusId == null ? "@FK_StatusId = NULL" : "@FK_StatusId = '" + advertisementRequest.FK_StatusId + "'") + ", @Skip = '" + advertisementRequest.Skip + "', @Take = '" + advertisementRequest.Take + "'";
                    string query = @"EXEC[dbo].[GetAdvertisements] @languageId = '" + advertisementRequest.LanguageId + "', @Search = '" + advertisementRequest.Search + "', @FK_CategoryId = '" + advertisementRequest.FK_CategoryId+ "', @FK_SubCategoryId = '" + advertisementRequest.FK_SubCategoryId+ "', @FK_AppUserId = '" + advertisementRequest.FK_AppUserId+ "', @FK_BreedId = '" + advertisementRequest.FK_BreedId + "', @FK_AgeId = '" + advertisementRequest.FK_AgeId + "', @FK_GenderId = '" + advertisementRequest.FK_GenderId + "', @FK_CityId = '" + advertisementRequest.FK_CityId + "', @FK_CountryId = '" + advertisementRequest.FK_CountryId + "', @FK_StatusId = '" + advertisementRequest.FK_StatusId + "', "+ "@Skip = '" + advertisementRequest.Skip + "', @Take = '" + advertisementRequest.Take + "'";

                    var res = await db.Set<GetAdvertisementRes>().FromSqlRaw(query)?.ToListAsync();
                    response.Take = advertisementRequest.Take;
                    response.Skip = advertisementRequest.Skip;
                    response.AdvertisementList = res.Where(x=> (adminType == 1 ? x.FK_StatusId != 1 : true)).Select(x=> new GetAdvertisementList { 
                        Id = x.Id,
                        Age = x.Age,
                        Breed = x.Breed,
                        Category = x.Category,
                        Comments = x.Comments,
                        Country = x.Country,
                        CountryId = x.CountryId,
                        CreatedDate = x.CreatedDate,
                        Email = x.Email,
                        FK_StatusId = x.FK_StatusId,
                        Gender = x.Gender,
                        IsBookMark = x.IsBookMark,
                        PhoneNumber = x.PhoneNumber,
                        RefId = x.RefId,
                        Title = x.Title,
                        ThreadId =x.ThreadId,
                        SellerName = x.SellerName,
                        SalePrice = x.SalePrice,
                        SellerId = x.SellerId,
                        SubCategory = x.SubCategory,
                        Status = x.Status,
                        UserName = x.UserName,
                        WhatsappNumber = x.WhatsappNumber,
                        Image = new FileUrlResponce { URL = x.FK_StatusId == (int) EStatuses.Sold ? (String.IsNullOrEmpty(x.WaterMarkImage) ? x.Url : x.WaterMarkImage)  : x.Url , ThumbnailUrl = x.FK_StatusId == (int)EStatuses.Sold ? (String.IsNullOrEmpty(x.WaterMarkImage) ? x.ThumbnilUrl : x.WaterMarkImage) : x.ThumbnilUrl , IsVideo = x.IsVideo , WaterMarkImage = x.WaterMarkImage },
                        IsVideo = x.IsVideo,
                        CityId = x.CityId,
                        City = x.City,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public async Task<bool> SaveBookMarksAdvertisement(CreateBookMarksAdvertisementRequest advertisementRequest, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    using (var trans = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var bookmarksExist = db.UserBookmarks.Where(x => x.FkAppUserId == userId && x.FkAdvertisementId == advertisementRequest.FK_AdvertisementId).FirstOrDefault();
                            if (bookmarksExist == null)
                            {
                                await db.UserBookmarks.AddAsync(new UserBookmarks
                                {
                                    Id = SystemGlobal.GetId(),
                                    FkAdvertisementId = advertisementRequest.FK_AdvertisementId,
                                    Description = "",
                                    FkAppUserId = userId,
                                    IsActive = true,
                                    CreatedBy = userId.ToString(),
                                    CreatedOnDate = DateTime.UtcNow,
                                    CreatedOn = DateTime.UtcNow,
                                });
                            }
                            else
                            {
                                var advertisementObject = db.Advertisement.FirstOrDefault(x => x.Id == advertisementRequest.FK_AdvertisementId);
                                if (bookmarksExist.IsActive == true)
                                {
                                    bookmarksExist.IsActive = false;
                                }
                                else
                                {
                                    bookmarksExist.IsActive = true;
                                }
                                db.Entry(advertisementObject).State = EntityState.Modified;
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

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetAdvertisementList> GetBookmarksAdvertisementByUserId(int LanguageId , Guid userId)
        {
            try
            {
                List<GetAdvertisementList> response = new List<GetAdvertisementList>();

                using (var db = new HilalDbContext())
                {
                    var query = db.UserBookmarks
                        .Include(x => x.FkAdvertisement)
                        .ThenInclude(x=> (x as Advertisement).FkStatus)
                        .Include(x => x.FkAdvertisement)
                        .ThenInclude(x=> (x as Advertisement).ChatThreads)
                        .Include(x => x.FkAdvertisement)
                        .ThenInclude(x=> (x as Advertisement).FkAge).ThenInclude(x => (x as Age).AgeDetails)
                        .Include(x => x.FkAdvertisement)
                        .ThenInclude(x => (x as Advertisement).FkBreed).ThenInclude(x => (x as Breed).BreedDetails)
                        .Include(x => x.FkAdvertisement)
                        .ThenInclude(x => (x as Advertisement).FkGender).ThenInclude(x => (x as HilalGenders).GenderDetails)
                        .Include(x => x.FkAdvertisement)
                        .ThenInclude(x => (x as Advertisement).FkCity).ThenInclude(x => (x as Cities).Citydetails)
                        .Include(x => x.FkAdvertisement)
                        .ThenInclude(x => (x as Advertisement).FkCity).ThenInclude(x => (x as Cities).FkCountryNavigation).ThenInclude(x => (x as Countries).CountryDetails)
                        .OrderByDescending(x => x.CreatedOn)
                        .Where(x => x.IsActive == true && x.FkAppUserId == userId).AsQueryable();

                    var queryable = query.Select(x => new GetAdvertisementList
                    {
                        Id = x.FkAdvertisementId.Value,
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
                        CreatedDate = x.FkAdvertisement.CreatedDate,
                        PhoneNumber = x.FkAdvertisement.PhoneNumber,
                        SalePrice = x.FkAdvertisement.SalePrice,
                        SellerId = x.FkAppUserId,
                        FK_StatusId = x.FkAdvertisement.FkStatusId,
                        IsBookMark = x.IsActive,
                        ThreadId = x.FkAdvertisement.ChatThreads.FirstOrDefault(x => x.IsActive == true && x.FkUserId == userId) == null ? (Guid?)null : x.FkAdvertisement.ChatThreads.FirstOrDefault(x => x.IsActive == true && x.FkUserId == userId).Id,
                        Title = x.FkAdvertisement.AdvertisementDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Title,
                        WhatsappNumber = x.FkAdvertisement.WhatsAppNumber,
                        Image = new FileUrlResponce { URL = x.FkAdvertisement.Attachement.FirstOrDefault(y=> y.IsActive ==  true).Url}
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

        public async Task<bool> AddUserSubscription(CreateUserSubscription CreateUserSubscription, Guid userId)
        {
            try
            {
                bool response = false;

                if (CreateUserSubscription.Id == null)
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                var AppUserSubscription = db.AppUserSubscription.Where(x => x.FkUserId == CreateUserSubscription.FK_UserId && x.IsActive == true).FirstOrDefault();
                                if (AppUserSubscription == null)
                                {
                                    await db.AppUserSubscription.AddAsync(new AppUserSubscription
                                    {
                                        Id = SystemGlobal.GetId(),
                                        FkSubscribedPlanId = CreateUserSubscription.FK_SubscribedPlanId,
                                        FkUserId = userId,
                                        StartDate = DateTime.UtcNow,
                                        CreatedDate = DateTime.UtcNow,
                                        IsActive = true,
                                        CreatedBy = userId.ToString(),
                                        CreatedOn = DateTime.UtcNow,
                                    });

                                    var userObject = db.AppUsers.FirstOrDefault(x => x.Id == userId);
                                    if (userObject != null)
                                    {
                                        userObject.IsSubscribed = true;
                                        userObject.IsSubscribedExpired = false;
                                    }
                                }
                                else
                                {
                                    AppUserSubscription.IsActive = false;
                                    await db.SaveChangesAsync();

                                    await db.AppUserSubscription.AddAsync(new AppUserSubscription
                                    {
                                        Id = SystemGlobal.GetId(),
                                        FkSubscribedPlanId = CreateUserSubscription.FK_SubscribedPlanId,
                                        FkUserId = userId,
                                        StartDate = DateTime.UtcNow,
                                        CreatedDate = DateTime.UtcNow,
                                        IsActive = true,
                                        CreatedBy = userId.ToString(),
                                        CreatedOn = DateTime.UtcNow,
                                    });

                                    var userObject = db.AppUsers.FirstOrDefault(x => x.Id == userId);
                                    if (userObject != null)
                                    {
                                        userObject.IsSubscribed = true;
                                        userObject.IsSubscribedExpired = false;
                                    }
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

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public GetSubscriptionList GetUserSubscribedDetail(Guid userId , int LanguageId)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    var alreadySubscribe = db.AppUserSubscription.Where(x => x.IsActive == true && x.FkUserId == userId && DateTime.UtcNow <= x.CreatedOn.AddDays(x.FkSubscribedPlan.ValidityDays)).FirstOrDefault();
                    if (alreadySubscribe != null)
                    {
                        return db.AppUserSubscription
                                        .Include(x => x.FkSubscribedPlan)
                                        .Where(x => x.IsActive == true && x.FkUserId == userId)
                                        .OrderByDescending(x => x.CreatedDate)
                                        .Select(x => new GetSubscriptionList
                                        {
                                            Id = x.FkSubscribedPlanId,
                                            Amount = x.FkSubscribedPlan.Amount,
                                            ValidityDays = x.FkSubscribedPlan.ValidityDays,
                                            ValidateDate = x.CreatedDate.AddDays(x.FkSubscribedPlan.ValidityDays),
                                            DisplayValidityDays = x.FkSubscribedPlan.SubscriptionDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).DisplayValidityDays,
                                            Name = x.FkSubscribedPlan.SubscriptionDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                                        }).FirstOrDefault() ?? new GetSubscriptionList();
                    }
                    else
                    {
                        return new GetSubscriptionList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetAdminNotificationResponse GetAdminNotifications(ListGeneralModel page)
        {
            try
            {
                GetAdminNotificationResponse response = new GetAdminNotificationResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.AdvertisementNotifications
                        .OrderByDescending(x=> x.CreatedDate)
                        .Where(x => x.IsActive == true &&  x.IsAdminNotify == true)
                        .Select(x => new GetNotificationsViewModel
                        {
                            Id = x.Id,
                            Title = x.FkAdvertisement.AdvertisementDetails.FirstOrDefault(x=> x.FkLanguageId == (int) ELanguage.English).Title,
                            BodyText = x.BodyText,
                            FK_AdvertisementId = x.FkAdvertisementId,
                            FK_ServiceId = x.FkServiceId,
                            IsSeen = x.IsSeen
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
                        x => x.Title.ToLower().Contains(page.Search.ToLower())
                        || x.BodyText.ToLower().Contains(page.Search.ToLower())
                    );
                    }

                    response.Page = page.Page;
                    response.PageSize = page.PageSize;
                    response.TotalRecords = query.Count();
                    response.IsSeenCount = query.Where(x=> x.IsSeen == false).Count();
                    if (page.PageSize > 0)
                    {
                        response.ItemList = query.Skip(page.Page).Take(page.PageSize).ToList();
                    }
                    else
                    {
                        response.ItemList = query.ToList();
                    }
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetAdvertisementRefId()
        {
            using (var db = new HilalDbContext())
            {
                while (true)
                {
                    Random generator = new Random();
                    string r = "Ad-" + generator.Next(0, 99999).ToString("D5");
                    var contain = db.Advertisement.FirstOrDefault(x => x.RefId == r && x.IsActive == true);
                    if (contain == null)
                        return r;
                }
            }
        }

        public async Task<string> CreateOrderRequest(CreateAdCommisionRequest createPaymentOrderRequest, Guid userId)
        {
            using (var db = new HilalDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var resultString = "";
                        
                        createPaymentOrderRequest.Amount = createPaymentOrderRequest.Amount * 100;
                        resultString = NetworkGlobal.commissionPaymentMethod(createPaymentOrderRequest);
                        if (!String.IsNullOrEmpty(resultString))
                        {
                            PaymentHistory paymentHistory = new PaymentHistory
                            {
                                Id = SystemGlobal.GetId(),
                                Address = createPaymentOrderRequest.Address,
                                Amount = createPaymentOrderRequest.Amount,
                                Email = createPaymentOrderRequest.EmailAddress,
                                FirstName = createPaymentOrderRequest.firstName,
                                LastName = createPaymentOrderRequest.lastName,
                                IsSubscription = false,
                                AppUserId = userId,
                                IsActive = true,
                                IsPaymentDone = false,
                                CreatedDate = DateTime.UtcNow,
                                CreatedBy = userId.ToString(),
                            };
                            db.PaymentHistory.Add(paymentHistory);
                            db.SaveChanges();
                        
                            await db.PaymentHistory
                                    .Where(x => x.IsActive == true && x.IsSubscription == true && x.AppUserId.Equals(userId))
                                    .ForEachAsync(x =>
                                    {
                                        x.IsActive = false;
                                        x.UpdatedBy = userId.ToString();
                                        x.UpdatedDate = DateTime.UtcNow;
                                    });
                            await db.SaveChangesAsync();
                            trans.Commit();
                        }

                        return resultString;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}
