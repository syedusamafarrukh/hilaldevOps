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

namespace Hilal.Service.Implementation.v1.App
{
    public class BusinessProfileService : IBusinessProfileService
    {
        private readonly IConfiguration configuration;

        public BusinessProfileService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public GetUserBusinessProfileResponse GetBusinessProfileService(GetBusinessProfileRequest BusinessProfile , Guid? userId)
        {
            try
            {
                GetUserBusinessProfileResponse response = new GetUserBusinessProfileResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.UserBusinessProfile
                        //.Include(x=> x.BuinessProfileDetails)
                        //.Include(x=> x.FkCategory).ThenInclude(x => (x as Categories).CategoriesDetails)
                        //.Include(x=> x.FkSubCategory).ThenInclude(x => (x as Categories).CategoriesDetails)
                        //.Include(x=> x.FkAppUser).ThenInclude(x => (x as AppUsers).AppUserProfiles)
                        //.Include(x=> x.FkCity).ThenInclude(x => (x as Cities).Citydetails)
                        //.Include(x=> x.ChatThreads)
                        //.Include(x=> x.Comments)
                        .OrderByDescending(x=> x.CreatedOn)
                        .Where(x => x.IsActive == true)
                        .Select(x => new GetBusinessProfile
                        {
                            Id = x.Id,
                            UserName = x.FkAppUser.AppUserProfiles.FirstOrDefault(y => y.IsEnabled == true).Name,
                            SellerName = x.BuinessProfileDetails.FirstOrDefault(y => y.FkLanguageId == BusinessProfile.LanguageId && x.IsActive == true).SellerName,
                            SellerId = x.FkAppUserId,
                            Address = x.BuinessProfileDetails.FirstOrDefault(y=> y.FkLanguageId == BusinessProfile.LanguageId).Address,
                            Description = x.BuinessProfileDetails.FirstOrDefault(y=> y.FkLanguageId == BusinessProfile.LanguageId).Description,
                            Title = x.BuinessProfileDetails.FirstOrDefault(y=> y.FkLanguageId == BusinessProfile.LanguageId).Title,
                            ContactNumber = x.PhoneNumberCountryCode + "-" + x.ContactNumber,
                            WhatsAppNumber = x.WhatsAppCountryCode + "-" + x.WhatsAppNumber,
                            EmailId = x.EmailId,
                            LogoIcon = x.LogoIcon,
                            Website = x.Website,
                            IsSelfPickup = x.IsSelfPickup,
                            IsDeliveryAvailable = x.IsDeliveryAvailable,
                            FK_SubCategoryId = new General<Guid?> { Id = x.FkSubCategoryId , Name = x.FkSubCategory.CategoriesDetails.FirstOrDefault(y=> y.FkLanguageId == BusinessProfile.LanguageId && x.IsActive == true).Name},
                            FK_CategoryId = new General<Guid> { Id = x.FkCategoryId , Name = x.FkCategory.CategoriesDetails.FirstOrDefault(y=> y.FkLanguageId == BusinessProfile.LanguageId && x.IsActive == true).Name},
                            FK_AppUserId = new General<Guid> { Id = x.FkAppUserId , Name = x.FkAppUser.AppUserProfiles.FirstOrDefault(y=> y.IsEnabled == true).Name},
                            FK_CityId = new General<Guid> { Id = x.FkCityId , Name = x.FkCity.Citydetails.FirstOrDefault(y=> y.FkLanguageId == BusinessProfile.LanguageId && x.IsActive == true).Name},
                            FK_CountryId = new General<Guid?> { Id = x.FkCity.FkCountry , Name = x.FkCity.FkCountryNavigation.CountryDetails.FirstOrDefault(y=> y.FkLanguageId == BusinessProfile.LanguageId && x.IsActive == true).Name},
                            FK_StatusId = x.FkStatusId,
                            CreatedDate = x.CreatedOn,
                            Comments = x.Comments.OrderByDescending(y=> y.CreatedOn).FirstOrDefault(y=> y.IsActive == true).Comments1,
                            ThreadId = x.ChatThreads.FirstOrDefault(x => x.IsActive == true && x.FkUserId == userId) == null ? (Guid?) null : x.ChatThreads.FirstOrDefault(x => x.IsActive == true && x.FkUserId == userId).Id,
                            AttachementList = x.Attachement.Where(y=> y.IsActive == true).Select(y=> new GetAttachementsResponseList
                            { 
                                Url = y.Url,
                                IsVideo = y.IsVideo,
                                ThubnilUrl = y.ThumbnilUrl,
                                ThumbnailUrl = y.ThumbnilUrl
                            }).ToList(),
                        }).AsQueryable();

                    if (BusinessProfile.FK_CountryId != null)
                    {
                        query = query.Where(x => x.FK_CountryId.Id == BusinessProfile.FK_CountryId);
                    }
                    if (BusinessProfile.FK_CityId != null)
                    {
                        query = query.Where(x => x.FK_CityId.Id == BusinessProfile.FK_CityId);
                    }
                    if (BusinessProfile.FK_CategoryId != null)
                    {
                        query = query.Where(x => x.FK_CategoryId.Id == BusinessProfile.FK_CategoryId);
                    }
                    if (BusinessProfile.FK_SubCategoryId != null)
                    {
                        query = query.Where(x => x.FK_SubCategoryId.Id == BusinessProfile.FK_SubCategoryId);
                    }
                    if (BusinessProfile.FK_AppUserId != null)
                    {
                        query = query.Where(x => x.FK_AppUserId.Id == BusinessProfile.FK_AppUserId);
                    }
                    if (BusinessProfile.FK_StatusId != null)
                    {
                        query = query.Where(x => x.FK_StatusId == BusinessProfile.FK_StatusId);
                    }
                    

                    if (!string.IsNullOrEmpty(BusinessProfile.Search))
                    {
                        var date = new DateTime();
                        var sdate = DateTime.TryParse(BusinessProfile.Search, out date);
                        int totalCases = -1;
                        var isNumber = Int32.TryParse(BusinessProfile.Search, out totalCases);

                        query = query.Where(
                        x => x.Title.ToLower().Contains(BusinessProfile.Search.ToLower())
                        || x.SellerName.ToLower().Contains(BusinessProfile.Search.ToLower())
                        || x.EmailId.ToLower().Contains(BusinessProfile.Search.ToLower())
                        || x.FK_AppUserId.Name.ToLower().Contains(BusinessProfile.Search.ToLower())
                        || x.FK_CategoryId.Name.ToLower().Contains(BusinessProfile.Search.ToLower())
                        || x.FK_SubCategoryId.Name.ToLower().Contains(BusinessProfile.Search.ToLower())
                        || x.FK_AppUserId.Name.ToLower().Contains(BusinessProfile.Search.ToLower())
                        || x.FK_CityId.Name.ToLower().Contains(BusinessProfile.Search.ToLower())
                        || x.WhatsAppNumber.ToLower().Contains(BusinessProfile.Search.ToLower())
                        || x.ContactNumber.ToLower().Contains(BusinessProfile.Search.ToLower())
                        || x.Address.ToLower().Contains(BusinessProfile.Search.ToLower())
                        || x.Description.ToLower().Contains(BusinessProfile.Search.ToLower())
                    );
                    }

                    var orderedQuery = query.OrderByDescending(x => x.CreatedDate);
                    switch (BusinessProfile.SortIndex)
                    {
                        case 0:
                            orderedQuery = BusinessProfile.SortBy == "desc" ? query.OrderByDescending(x => x.Title) : query.OrderBy(x => x.Title);
                          break;
                        case 2:
                            orderedQuery = BusinessProfile.SortBy == "desc" ? query.OrderByDescending(x => x.SellerName) : query.OrderBy(x => x.SellerName);
                            break;
                        case 3:
                            orderedQuery = BusinessProfile.SortBy == "desc" ? query.OrderByDescending(x => x.EmailId) : query.OrderBy(x => x.EmailId);
                            break;
                        case 4:
                            orderedQuery = BusinessProfile.SortBy == "desc" ? query.OrderByDescending(x => x.Address) : query.OrderBy(x => x.Address);
                            break;
                        case 5:
                            orderedQuery = BusinessProfile.SortBy == "desc" ? query.OrderByDescending(x => x.ContactNumber) : query.OrderBy(x => x.ContactNumber);
                            break;
                        case 6:
                            orderedQuery = BusinessProfile.SortBy == "desc" ? query.OrderByDescending(x => x.WhatsAppNumber) : query.OrderBy(x => x.WhatsAppNumber);
                            break;
                        case 7:
                            orderedQuery = BusinessProfile.SortBy == "desc" ? query.OrderByDescending(x => x.FK_CategoryId.Name) : query.OrderBy(x => x.FK_CategoryId.Name);
                            break;
                        case 8:
                            orderedQuery = BusinessProfile.SortBy == "desc" ? query.OrderByDescending(x => x.FK_SubCategoryId.Name) : query.OrderBy(x => x.FK_SubCategoryId.Name);
                            break;
                        case 9:
                            orderedQuery = BusinessProfile.SortBy == "desc" ? query.OrderByDescending(x => x.FK_AppUserId.Name) : query.OrderBy(x => x.FK_AppUserId.Name);
                            break;
                        case 10:
                            orderedQuery = BusinessProfile.SortBy == "desc" ? query.OrderByDescending(x => x.Website) : query.OrderBy(x => x.Website);
                            break;
                    }


                    response.Skip = BusinessProfile.Skip;
                    response.Take = BusinessProfile.Take;
                    response.TotalRecords = orderedQuery.Count();
                    if (response.Take > 0)
                    {
                        response.ItemList = orderedQuery.Skip(response.Skip).Take(response.Take).ToList();
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
        
        public GetUserBusinessProfileListResponse GetBusinessServices(GetBusinessProfileRequest BusinessProfile, Guid? userId)
        {
            try
            {
                GetUserBusinessProfileListResponse response = new GetUserBusinessProfileListResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.UserBusinessProfile
                        //.Include(x => x.BuinessProfileDetails)
                        //.Include(x => x.FkCategory).ThenInclude(x => (x as Categories).CategoriesDetails)
                        //.Include(x => x.FkSubCategory).ThenInclude(x => (x as Categories).CategoriesDetails)
                        //.Include(x => x.FkAppUser).ThenInclude(x => (x as AppUsers).AppUserProfiles)
                        //.Include(x => x.FkCity).ThenInclude(x => (x as Cities).Citydetails)
                        //.Include(x => x.FkCity).ThenInclude(x => (x as Cities).FkCountryNavigation).ThenInclude(x => (x as Countries).CountryDetails)
                        //.Include(x => x.ChatThreads)
                        //.Include(x => x.Comments)
                        .OrderByDescending(x => x.CreatedOn)
                        .Where(x => x.IsActive == true)
                        .AsQueryable();

                    if (BusinessProfile.FK_CountryId != null)
                    {
                        query = query.Where(x => x.FkCity.FkCountry == BusinessProfile.FK_CountryId);
                    }
                    if (BusinessProfile.FK_CityId != null)
                    {
                        query = query.Where(x => x.FkCityId == BusinessProfile.FK_CityId);
                    }
                    if (BusinessProfile.FK_CategoryId != null)
                    {
                        query = query.Where(x => x.FkCategoryId == BusinessProfile.FK_CategoryId);
                    }
                    if (BusinessProfile.FK_SubCategoryId != null)
                    {
                        query = query.Where(x => x.FkSubCategoryId == BusinessProfile.FK_SubCategoryId);
                    }
                    if (BusinessProfile.FK_AppUserId != null)
                    {
                        query = query.Where(x => x.FkAppUserId == BusinessProfile.FK_AppUserId);
                    }
                    if (BusinessProfile.FK_StatusId != null)
                    {
                        query = query.Where(x => x.FkStatusId == BusinessProfile.FK_StatusId);
                    }

                    var orderQuery = query.Select(x => new GetServicesList
                    {
                        Id = x.Id,
                        Category = x.FkCategory.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == BusinessProfile.LanguageId).Name,
                        SubCategory = x.FkSubCategory.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == BusinessProfile.LanguageId).Name,
                        UserName = x.FkAppUser.AppUserProfiles.FirstOrDefault(y => y.IsEnabled == true).Name,
                        SellerName = x.BuinessProfileDetails.FirstOrDefault(y => y.FkLanguageId == BusinessProfile.LanguageId && x.IsActive == true).SellerName,
                        SellerId = x.FkAppUserId,
                        City = x.FkCity.Citydetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == BusinessProfile.LanguageId).Name,
                        Country = x.FkCity.FkCountryNavigation.CountryDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == BusinessProfile.LanguageId).Name,
                        Title = x.BuinessProfileDetails.FirstOrDefault(y => y.FkLanguageId == BusinessProfile.LanguageId).Title,
                        Status = x.FkStatus.Name,
                        WhatsappNumber = x.WhatsAppCountryCode + "-" + x.WhatsAppNumber,
                        PhoneNumber = x.PhoneNumberCountryCode + "-" + x.ContactNumber,
                        Image = new FileUrlResponce { URL = x.Attachement.FirstOrDefault(y => y.IsActive).Url },
                        Email = x.EmailId,
                        Comments = x.Comments.OrderByDescending(y => y.CreatedOn).FirstOrDefault(y => y.IsActive == true).Comments1,
                        ThreadId = x.ChatThreads.FirstOrDefault(x => x.IsActive == true && x.FkUserId == userId) == null ? (Guid?) null : x.ChatThreads.FirstOrDefault(x => x.IsActive == true && x.FkUserId == userId).Id,
                        LogoIcon = x.LogoIcon,
                        Website = x.Website,
                        CreatedDate = x.CreatedOn,
                    }).AsQueryable();

                    if (!string.IsNullOrEmpty(BusinessProfile.Search))
                    {
                        var date = new DateTime();
                        var sdate = DateTime.TryParse(BusinessProfile.Search, out date);
                        int totalCases = -1;
                        var isNumber = Int32.TryParse(BusinessProfile.Search, out totalCases);

                        orderQuery = orderQuery.Where(
                        x => x.Title.ToLower().Contains(BusinessProfile.Search.ToLower())
                        || x.City.ToLower().Contains(BusinessProfile.Search.ToLower())
                        || x.Country.ToLower().Contains(BusinessProfile.Search.ToLower())
                        || x.Status.ToLower().Contains(BusinessProfile.Search.ToLower())
                        || x.WhatsappNumber.ToLower().Contains(BusinessProfile.Search.ToLower())
                        || x.PhoneNumber.ToLower().Contains(BusinessProfile.Search.ToLower())
                        || x.Email.ToLower().Contains(BusinessProfile.Search.ToLower())
                    );
                    }

                    response.Skip = BusinessProfile.Skip;
                    response.Take = BusinessProfile.Take;
                    response.TotalRecords = orderQuery.Count();
                    if (response.Take > 0)
                    {
                        response.ItemList = orderQuery.Skip(response.Skip).Take(response.Take).ToList();
                    }
                    else
                    {
                        response.ItemList = orderQuery.ToList();
                    }
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GetUserBusinessProfileResponse> GetBusinessProfileServiceSP(GetBusinessProfileRequest BusinessProfile, Guid? userId)
        {
            GetUserBusinessProfileResponse response = new GetUserBusinessProfileResponse();
            try
            {
                using (var db = new HilalDbContext())
                {
                    if (BusinessProfile.Take < 1)
                    {
                        BusinessProfile.Take = 10000;
                    }
                    string query = @"EXEC[dbo].[GetAdvertisements] @languageId = '" + BusinessProfile.LanguageId + "', @Search = '" + BusinessProfile.Search + "', @FK_CategoryId = '" + BusinessProfile.FK_CategoryId + "', @FK_SubCategoryId = '" + BusinessProfile.FK_SubCategoryId + "', @FK_AppUserId = '" + BusinessProfile.FK_AppUserId + "', @FK_CityId = '" + BusinessProfile.FK_CityId + "', @FK_CountryId = '" + BusinessProfile.FK_CountryId + "', @FK_StatusId = '" + BusinessProfile.FK_StatusId + "', " + "@Skip = '" + BusinessProfile.Skip + "', @Take = '" + BusinessProfile.Take + "'";

                    var res = await db.Set<GetBusinessProfileSP>().FromSqlRaw(query)?.ToListAsync();
                    response.Take = BusinessProfile.Take;
                    response.Skip = BusinessProfile.Skip;
                    response.ItemList = res.Select(x => new GetBusinessProfile
                    {
                        Id = x.Id,
                        Comments = x.Comments,
                        CreatedDate = x.CreatedDate,
                        FK_StatusId = x.FK_StatusId,
                        Title = x.Title,
                        ThreadId = x.ThreadId,
                        SellerName = x.SellerName,
                        SellerId = x.SellerId,
                        UserName = x.UserName,
                        Address = x.Address,
                        ContactNumber = x.PhoneNumber,
                        WhatsAppNumber = x.WhatsappNumber,
                        EmailId = x.Email,
                        FK_AppUserId = new General<Guid> { Id = x.SellerId ,Name = x.UserName},
                        FK_CategoryId = new General<Guid> { Id = x.FK_CategoryId , Name = x.Category},
                        FK_SubCategoryId = new General<Guid?> { Id = x.FK_SubCategoryId , Name = x.SubCategory},
                        FK_CityId = new General<Guid> { Id = x.FK_CityId , Name = x.City},
                        FK_CountryId = new General<Guid?> { Id = x.FK_CountryId , Name = x.Country},
                        IsDeliveryAvailable = x.IsDeliveryAvailable,
                        IsSelfPickup = x.IsSelfPickup,
                        LogoIcon = x.LogoIcon,
                        Website = x.Website,
                        Description = x.Description,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public async Task<GetUserBusinessProfileListResponse> GetBusinessServicesSP(GetBusinessProfileRequest BusinessProfile, Guid? userId)
        {
            GetUserBusinessProfileListResponse response = new GetUserBusinessProfileListResponse();
            try
            {
                using (var db = new HilalDbContext())
                {
                    if (BusinessProfile.Take < 1)
                    {
                        BusinessProfile.Take = 10000;
                    }
                    BusinessProfile.FK_CategoryId = BusinessProfile.FK_CategoryId == null ? new Guid("00000000-0000-0000-0000-000000000000") : BusinessProfile.FK_CategoryId;
                    BusinessProfile.FK_SubCategoryId = BusinessProfile.FK_SubCategoryId == null ? new Guid("00000000-0000-0000-0000-000000000000") : BusinessProfile.FK_SubCategoryId;
                    BusinessProfile.FK_AppUserId = BusinessProfile.FK_AppUserId == null ? new Guid("00000000-0000-0000-0000-000000000000") : BusinessProfile.FK_AppUserId;
                    BusinessProfile.FK_CountryId = BusinessProfile.FK_CountryId == null ? new Guid("00000000-0000-0000-0000-000000000000") : BusinessProfile.FK_CountryId;
                    BusinessProfile.FK_CityId = BusinessProfile.FK_CityId == null ? new Guid("00000000-0000-0000-0000-000000000000") : BusinessProfile.FK_CityId;
                    BusinessProfile.FK_StatusId = BusinessProfile.FK_StatusId == null ? 0 : BusinessProfile.FK_StatusId;
                    
                    string query = @"EXEC[dbo].[GetServices] @languageId = '" + BusinessProfile.LanguageId + "', @Search = '" + BusinessProfile.Search + "', @FK_CategoryId = '" + BusinessProfile.FK_CategoryId + "', @FK_SubCategoryId = '" + BusinessProfile.FK_SubCategoryId + "', @FK_AppUserId = '" + BusinessProfile.FK_AppUserId + "', @FK_CityId = '" + BusinessProfile.FK_CityId + "', @FK_CountryId = '" + BusinessProfile.FK_CountryId + "', @FK_StatusId = '" + BusinessProfile.FK_StatusId + "', " + "@Skip = '" + BusinessProfile.Skip + "', @Take = '" + BusinessProfile.Take + "'";

                    var res = await db.Set<GetBusinessProfileSP>().FromSqlRaw(query)?.ToListAsync();
                    response.Take = BusinessProfile.Take;
                    response.Skip = BusinessProfile.Skip;
                    response.ItemList = res.Select(x => new GetServicesList
                    {
                        Id = x.Id,
                        Comments = x.Comments,
                        CreatedDate = x.CreatedDate,
                        Title = x.Title,
                        ThreadId = x.ThreadId,
                        SellerName = x.SellerName,
                        SellerId = x.SellerId,
                        UserName = x.UserName,
                        LogoIcon = x.LogoIcon,
                        Website = x.Website,
                        Category = x.Category,
                        City = x.City,
                        Country = x.Country,
                        Email = x.Email,
                        Image = new FileUrlResponce { URL = x.Url , ThumbnailUrl = x.ThumbnilUrl ,IsVideo = x.IsVideo},
                        PhoneNumber = x.PhoneNumber,
                        Status = x.Status,
                        SubCategory = x.SubCategory,
                        WhatsappNumber = x.WhatsappNumber,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public GetServicesDetail GetBusinessServicesDetail(Guid Id, int LanguageId, Guid? userId)
        {
            try
            {
                GetServicesDetail response = new GetServicesDetail();

                using (var db = new HilalDbContext())
                {
                    response = db.UserBusinessProfile
                       //.Include(x => x.BuinessProfileDetails)
                       // .Include(x => x.FkCategory).ThenInclude(x => (x as Categories).CategoriesDetails)
                       // .Include(x => x.FkSubCategory).ThenInclude(x => (x as Categories).CategoriesDetails)
                       // .Include(x => x.FkAppUser).ThenInclude(x => (x as AppUsers).AppUserProfiles)
                       // .Include(x => x.FkCity).ThenInclude(x => (x as Cities).Citydetails)
                       // .Include(x => x.FkCity).ThenInclude(x => (x as Cities).FkCountryNavigation).ThenInclude(x => (x as Countries).CountryDetails)
                       // .Include(x => x.ChatThreads)
                        .OrderByDescending(x => x.CreatedDate)
                        .Where(x => x.Id == Id).Select(x => new GetServicesDetail
                        {
                            Id = x.Id,
                            Category = x.FkCategory.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                            SubCategory = x.FkSubCategory.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                            UserName = x.FkAppUser.AppUserProfiles.FirstOrDefault(y => y.IsEnabled == true).Name,
                            SellerName = x.BuinessProfileDetails.FirstOrDefault(y => y.FkLanguageId == LanguageId && x.IsActive == true).SellerName,
                            SellerId = x.FkAppUserId,
                            City = x.FkCity.Citydetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                            Country = x.FkCity.FkCountryNavigation.CountryDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                            Status = x.FkStatus.Name,
                            Email = x.EmailId,
                            Website = x.Website,
                            CreatedDate = x.CreatedOn,
                            PhoneNumber = x.PhoneNumberCountryCode + "-" + x.ContactNumber,
                            Title = x.BuinessProfileDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Title,
                            WhatsappNumber = x.WhatsAppCountryCode + "-" + x.WhatsAppNumber,
                            Address = x.BuinessProfileDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Address,
                            Description = x.BuinessProfileDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Description,
                            IsDeliveryAvailable = x.IsDeliveryAvailable,
                            ThreadId = x.ChatThreads.FirstOrDefault(x => x.IsActive == true && x.FkUserId == userId) == null ? (Guid?)null : x.ChatThreads.FirstOrDefault(x => x.IsActive == true && x.FkUserId == userId).Id,
                            LogoIcon = x.LogoIcon,
                            Attachements = x.Attachement.Where(y => y.IsActive == true && y.FkBusinessProfileId == Id).Select(y => new GetAttachementsResponseList
                            {
                                Url = y.Url,
                                IsVideo = y.IsVideo,
                                ThubnilUrl = y.ThumbnilUrl,
                                ThumbnailUrl = y.ThumbnilUrl
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

        public async Task<bool> SaveBusinessProfile(CreateAppUserBusinessProfileRequest BusinessProfileRequest, Guid userId)
        {
            try
            {
                bool response = false;

                List<BuinessProfileDetails> informations = new List<BuinessProfileDetails>();
                List<Attachement> attachementList = new List<Attachement>();

                foreach (var information in BusinessProfileRequest.UserBusinessProfileDetailInformation)
                {
                    informations.Add(new BuinessProfileDetails
                    {
                        Id = SystemGlobal.GetId(),
                        Title = information.Title,
                        Address = information.Address,
                        FkLanguageId = information.FK_LanguageId,
                        Description = information.Description,
                        FkUserBusinessProfileId = information.FK_UserBusinessProfileId,
                        SellerName = information.SellerName,
                        IsActive = true,
                        CreatedBy = userId.ToString(),
                        CreatedOn = DateTime.UtcNow,
                        CreatedDate = DateTime.UtcNow
                    });
                    
                }

                if (BusinessProfileRequest.AttachementList != null && BusinessProfileRequest.AttachementList.Count > 0)
                {
                    foreach (var information in BusinessProfileRequest.AttachementList)
                    {
                        attachementList.Add(new Attachement
                        {
                            Id = 0,
                            FkBusinessProfileId = information.FK_BusinessProfileId,
                            Url = information.Url,
                            IsVideo = information.IsVideo.Value,
                            ThumbnilUrl = information.ThubnilUrl,
                            CreatedOnDate = DateTime.UtcNow,
                            CreatedBy = userId.ToString(),
                            CreatedOn = DateTime.UtcNow,
                            IsActive = true
                        });
                    } 
                }

                if (BusinessProfileRequest.Id == null)
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                var id = SystemGlobal.GetId();
                                await db.UserBusinessProfile.AddAsync(new UserBusinessProfile
                                {
                                    Id = id,
                                    BuinessProfileDetails = informations,
                                    Attachement = attachementList,
                                    ContactNumber = BusinessProfileRequest.ContactNumber.PhoneNumber,
                                    PhoneNumberCountryCode = BusinessProfileRequest.ContactNumber.CountryCode,
                                    WhatsAppNumber = BusinessProfileRequest.WhatsAppNumber.PhoneNumber,
                                    WhatsAppCountryCode = BusinessProfileRequest.WhatsAppNumber.CountryCode,
                                    EmailId = BusinessProfileRequest.EmailId,
                                    IsSelfPickup = Convert.ToBoolean(BusinessProfileRequest.IsSelfPickup),
                                    IsDeliveryAvailable = Convert.ToBoolean(BusinessProfileRequest.IsDeliveryAvailable),
                                    LogoIcon = BusinessProfileRequest.LogoIcon.URL,
                                    FkCategoryId = BusinessProfileRequest.FK_CategoryId,
                                    FkAppUserId = userId,
                                    FkCityId = BusinessProfileRequest.FK_CityId,
                                    FkStatusId = (int) EStatuses.Pending,
                                    FkSubCategoryId = BusinessProfileRequest.FK_SubCategoryId,
                                    Website = BusinessProfileRequest.Website,
                                    CreatedDate = DateTime.UtcNow,
                                    IsActive = true,
                                    CreatedBy = userId.ToString(),
                                    CreatedOn = DateTime.UtcNow,
                                });
                                await db.SaveChangesAsync();
                                await db.AdvertisementNotifications.AddAsync(new AdvertisementNotifications
                                {
                                    Id = SystemGlobal.GetId(),
                                    FkServiceId = id,
                                    IsAdminNotify = true,
                                    BodyText = BusinessProfileRequest.UserBusinessProfileDetailInformation.FirstOrDefault(x => x.FK_LanguageId == (int)ELanguage.English).Title + "/" + "Service is Pending for Approvel",
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
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                await db.BuinessProfileDetails
                                    .Where(x => x.IsActive ==  true && x.FkUserBusinessProfileId.Equals(BusinessProfileRequest.Id))
                                    .ForEachAsync(x =>
                                    {
                                        x.IsActive = false;
                                        x.UpdatedBy = userId.ToString();
                                        x.UpdatedDate = DateTime.UtcNow;
                                    });

                                await db.Attachement
                                    .Where(x => x.IsActive == true && x.FkBusinessProfileId.Equals(BusinessProfileRequest.Id))
                                    .ForEachAsync(x =>
                                    {
                                        x.IsActive = false;
                                        x.UpdatedBy = userId.ToString();
                                        x.UpdatedOn = DateTime.UtcNow;
                                    });

                                await db.SaveChangesAsync();

                                //add Newer

                                var category = db.UserBusinessProfile.FirstOrDefault(x => x.Id.Equals(BusinessProfileRequest.Id));
                                category.ContactNumber = BusinessProfileRequest.ContactNumber.PhoneNumber;
                                category.PhoneNumberCountryCode = BusinessProfileRequest.ContactNumber.CountryCode;
                                category.EmailId = BusinessProfileRequest.EmailId;
                                category.FkCategoryId = BusinessProfileRequest.FK_CategoryId;
                                category.FkSubCategoryId = BusinessProfileRequest.FK_SubCategoryId;
                                category.FkCityId = BusinessProfileRequest.FK_CityId;
                                category.IsDeliveryAvailable = Convert.ToBoolean(BusinessProfileRequest.IsDeliveryAvailable);
                                category.IsSelfPickup = Convert.ToBoolean(BusinessProfileRequest.IsSelfPickup);
                                category.LogoIcon = BusinessProfileRequest.LogoIcon.URL;
                                category.WhatsAppNumber = BusinessProfileRequest.WhatsAppNumber.PhoneNumber;
                                category.WhatsAppCountryCode = BusinessProfileRequest.WhatsAppNumber.CountryCode;
                                category.Website = BusinessProfileRequest.Website;
                                category.EmailId = BusinessProfileRequest.EmailId;
                                category.FkStatusId = (int) EStatuses.Pending;
                                category.UpdatedBy = userId.ToString();
                                category.UpdatedDate = DateTime.UtcNow;

                                db.Entry(category).State = EntityState.Modified;
                                await db.SaveChangesAsync();

                                informations.ForEach(x => x.FkUserBusinessProfileId = category.Id);
                                await db.BuinessProfileDetails.AddRangeAsync(informations);

                                attachementList.ForEach(x => x.FkBusinessProfileId = category.Id);
                                await db.Attachement.AddRangeAsync(attachementList);

                                await db.AdvertisementNotifications.AddAsync(new AdvertisementNotifications
                                {
                                    Id = SystemGlobal.GetId(),
                                    FkServiceId = category.Id,
                                    IsAdminNotify = true,
                                    BodyText = BusinessProfileRequest.UserBusinessProfileDetailInformation.FirstOrDefault(x => x.FK_LanguageId == (int)ELanguage.English)?.Title +"/"+ "Service is Pending for Approvel",
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

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CreateAppUserBusinessProfileRequest GetEditBusinessProfile(Guid id)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.UserBusinessProfile
                        .Include(x=> x.BuinessProfileDetails)
                        .Include(x=> x.Attachement)
                        .Where(x => x.Id.Equals(id))
                        .Select(x => new CreateAppUserBusinessProfileRequest
                        {
                            Id = x.Id,
                            FK_SubCategoryId = x.FkSubCategoryId,
                            ContactNumber = new PhoneNumberModel { CountryCode = x.PhoneNumberCountryCode , PhoneNumber = x.ContactNumber },
                            WhatsAppNumber = new PhoneNumberModel { CountryCode = x.WhatsAppCountryCode , PhoneNumber = x.WhatsAppNumber },
                            EmailId = x.EmailId,
                            FK_AppUserId = x.FkAppUserId,
                            FK_StatusId = x.FkStatusId,
                            FK_CategoryId = x.FkCategoryId,
                            FK_CityId = x.FkCityId,
                            Website = x.Website,
                            IsDeliveryAvailable = Convert.ToString(x.IsDeliveryAvailable),
                            IsSelfPickup = Convert.ToString(x.IsSelfPickup),
                            LogoIcon = new FileUrlResponce
                            {
                                URL = x.LogoIcon,
                            },
                            UserBusinessProfileDetailInformation = x.BuinessProfileDetails.Where(z=> z.IsActive==  true).Select(z=> new CreateUserBusinessProfileDetailRequest
                            {
                                Address = z.Address,
                                FK_LanguageId = z.FkLanguageId,
                                SellerName = z.SellerName,
                                Id = z.Id,
                                Title = z.Title,
                                FK_UserBusinessProfileId = z.FkUserBusinessProfileId,
                                Description = z.Description,
                            }).ToList(),
                            AttachementList = x.Attachement.Where(y => y.IsActive == true).Select(y => new CreateAttachementsRequest
                            {
                                Id = y.Id,
                                FK_BusinessProfileId = y.FkBusinessProfileId,
                                Url = y.Url,
                            }).ToList()
                        })
                        .FirstOrDefault() ?? new CreateAppUserBusinessProfileRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetEditUserBusinessProfileRequest GetEditBusinessProfileApp(Guid id, long LanguageId)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.UserBusinessProfile
                        .Include(x => x.BuinessProfileDetails)
                        .Include(x => x.Attachement)
                        .Include(x => x.FkCategory).ThenInclude(x => (x as Categories).CategoriesDetails)
                        .Include(x => x.FkSubCategory).ThenInclude(x => (x as Categories).CategoriesDetails)
                        .Include(x => x.FkAppUser).ThenInclude(x => (x as AppUsers).AppUserProfiles)
                        .Include(x => x.FkCity).ThenInclude(x => (x as Cities).Citydetails)
                        .Include(x => x.FkCity).ThenInclude(x => (x as Cities).FkCountryNavigation).ThenInclude(x => (x as Countries).CountryDetails)
                        .Where(x => x.Id.Equals(id))
                        .Select(x => new GetEditUserBusinessProfileRequest
                        {
                            Id = x.Id,
                            FK_SubCategoryId = new General<Guid?> { Id = x.FkSubCategoryId, Name = x.FkSubCategory.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name },
                            ContactNumber = new PhoneNumberModel { CountryCode = x.PhoneNumberCountryCode, PhoneNumber = x.ContactNumber },
                            WhatsAppNumber = new PhoneNumberModel { CountryCode = x.WhatsAppCountryCode, PhoneNumber = x.WhatsAppNumber },
                            EmailId = x.EmailId,
                            FK_AppUserId = new General<Guid?> { Id = x.FkAppUserId, Name = x.FkAppUser.AppUserProfiles.FirstOrDefault(y => y.IsEnabled == true).Name },
                            FK_StatusId = x.FkStatusId,
                            FK_CategoryId = new General<Guid> { Id = x.FkCategoryId, Name = x.FkCategory.CategoriesDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name },
                            FK_CityId = new General<Guid> { Id = x.FkCityId, Name = x.FkCity.Citydetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name },
                            FK_CountryId = new General<Guid?> { Id = x.FkCity.FkCountry, Name = x.FkCity.FkCountryNavigation.CountryDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name },
                            Website = x.Website,
                            IsDeliveryAvailable = Convert.ToString(x.IsDeliveryAvailable),
                            IsSelfPickup = Convert.ToString(x.IsSelfPickup),
                            LogoIcon = new FileUrlResponce
                            {
                                URL = x.LogoIcon,
                            },
                            UserBusinessProfileDetailInformation = x.BuinessProfileDetails.Where(z => z.IsActive == true).Select(z => new CreateUserBusinessProfileDetailRequest
                            {
                                Address = z.Address,
                                FK_LanguageId = z.FkLanguageId,
                                SellerName = z.SellerName,
                                Id = z.Id,
                                Title = z.Title,
                                FK_UserBusinessProfileId = z.FkUserBusinessProfileId,
                                Description = z.Description,
                            }).ToList(),
                            AttachementList = x.Attachement.Where(y => y.IsActive == true).Select(y => new CreateAttachementsRequest
                            {
                                Id = y.Id,
                                FK_BusinessProfileId = y.FkBusinessProfileId,
                                Url = y.Url,
                                IsVideo =y.IsVideo,
                                ThubnilUrl =y.ThumbnilUrl
                            }).ToList()
                        }).FirstOrDefault() ?? new GetEditUserBusinessProfileRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ControllBusinessProfileActivation(Guid BusinessProfileId, bool activation, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.UserBusinessProfile.FirstOrDefault(x => x.Id.Equals(BusinessProfileId));

                    if (category == null) throw new Exception("BusinessProfileId Doesn't Exists");

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

        public async Task<bool> ControllBusinessProfileStatus(Guid BusinessProfileId, int StatusId, Guid userId, string Comments)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.UserBusinessProfile.Include(x=> x.BuinessProfileDetails)
                        .Include(x=> x.FkAppUser).ThenInclude(x=> (x as AppUsers).UserDeviceInformations)
                        .Include(x=> x.FkStatus)
                        .FirstOrDefault(x => x.Id.Equals(BusinessProfileId));

                    if (category == null) throw new Exception("BusinessProfileId Doesn't Exists");

                    category.FkStatusId = StatusId;
                    category.UpdatedBy = userId.ToString();
                    category.UpdatedDate = DateTime.UtcNow;

                    db.Entry(category).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    var stringNotify = NotificationsStrings.GetStringNotification(category.FkStatusId, category.BuinessProfileDetails.FirstOrDefault(x => x.FkLanguageId == (int)ELanguage.English)?.Title, 2);
                    var Body = stringNotify;
                    var Body1 = stringNotify;
                    var devicetoken = category.FkAppUser.UserDeviceInformations.OrderByDescending(x => x.CreatedOn).FirstOrDefault(x => x.IsEnabled == true)?.DeviceToken;
                    await db.AdvertisementNotifications.AddAsync(new AdvertisementNotifications
                    {
                        Id = SystemGlobal.GetId(),
                        FkServiceId = category.Id,
                        ReceiverId = category.FkAppUserId,
                        IsAdminNotify = false,
                        BodyText = Body,
                        DeviceToken = devicetoken,
                        IsSeen = false,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        CreatedBy = userId.ToString(),
                    });
                    await db.SaveChangesAsync();

                    if (!String.IsNullOrEmpty(Comments))
                    {
                        await db.Comments.AddAsync(new Comments
                        {
                            Id = SystemGlobal.GetId(),
                            Comments1 = Comments,
                            FkServiceId = category.Id,
                            CreatedBy = userId.ToString(),
                            CreatedDate = DateTime.UtcNow,
                            CreatedOn = DateTime.UtcNow,
                            IsActive = true
                        });
                        await db.SaveChangesAsync();
                    }

                    var newObj = new
                    {
                        ServiceId = category.Id,
                        UserId = category.FkAppUserId,
                        isAdvertisement = false,
                        Type = (int)ENotificationType.ServiceDetail
                    };

                    if (StatusId == (int)EStatuses.DisApproved)
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

        public async Task<bool> ControllBusinessProfileStatusApp(Guid BusinessProfileId, int StatusId, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.UserBusinessProfile.Include(x => x.BuinessProfileDetails)
                        .Include(x => x.FkAppUser).ThenInclude(x => (x as AppUsers).UserDeviceInformations)
                        .Include(x => x.FkStatus)
                        .FirstOrDefault(x => x.Id.Equals(BusinessProfileId));

                    if (category == null) throw new Exception("BusinessProfileId Doesn't Exists");

                    category.FkStatusId = StatusId;
                    category.UpdatedBy = userId.ToString();
                    category.UpdatedDate = DateTime.UtcNow;

                    db.Entry(category).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    //var stringNotify = NotificationsStrings.GetStringNotification(category.FkStatusId, category.BuinessProfileDetails.FirstOrDefault(x => x.FkLanguageId == (int)ELanguage.English)?.Title, 2);
                    //var Body = stringNotify;
                    //var devicetoken = category.FkAppUser.UserDeviceInformations.OrderByDescending(x => x.CreatedOn).FirstOrDefault(x => x.IsEnabled == true)?.DeviceToken;
                    //await db.AdvertisementNotifications.AddAsync(new AdvertisementNotifications
                    //{
                    //    Id = SystemGlobal.GetId(),
                    //    FkAdvertisementId = category.Id,
                    //    IsAdminNotify = false,
                    //    BodyText = Body,
                    //    DeviceToken = devicetoken,
                    //    IsSeen = false,
                    //    CreatedDate = DateTime.UtcNow,
                    //    IsActive = true,
                    //    CreatedBy = userId.ToString(),
                    //});
                    //await db.SaveChangesAsync();

                    //object newObj = new
                    //{
                    //    ServiceId = category.Id,
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

        public GetBusinessProfile GetBusinessProfileServiceByUserId(Guid appUserId , long LanguageId)
        {
            try
            {
                GetBusinessProfile response = new GetBusinessProfile();

                using (var db = new HilalDbContext())
                {
                    response = db.UserBusinessProfile
                        .Include(x => x.BuinessProfileDetails)
                        .Include(x => x.FkCategory).ThenInclude(x => (x as Categories).CategoriesDetails)
                        .Include(x => x.FkSubCategory).ThenInclude(x => (x as Categories).CategoriesDetails)
                        .Include(x => x.FkAppUser).ThenInclude(x => (x as AppUsers).AppUserProfiles)
                        .Include(x => x.FkCity).ThenInclude(x => (x as Cities).Citydetails)
                        .Include(x => x.ChatThreads)
                        .Include(x => x.Comments)
                        .Where(x => x.FkAppUserId == appUserId)
                        .OrderByDescending(x=> x.CreatedOn)
                        .Select(x => new GetBusinessProfile
                        {
                            Id = x.Id,
                            Address = x.BuinessProfileDetails.FirstOrDefault(y => y.FkLanguageId == LanguageId).Address,
                            Description = x.BuinessProfileDetails.FirstOrDefault(y => y.FkLanguageId == LanguageId).Description,
                            Title = x.BuinessProfileDetails.FirstOrDefault(y => y.FkLanguageId == LanguageId).Title,
                            SellerName = x.BuinessProfileDetails.FirstOrDefault(y => y.FkLanguageId == LanguageId).SellerName,
                            ContactNumber = x.ContactNumber,
                            EmailId = x.EmailId,
                            LogoIcon = x.LogoIcon,
                            Website = x.Website,
                            Comments = x.Comments.OrderByDescending(y => y.CreatedOn).FirstOrDefault(y => y.IsActive == true).Comments1,
                            WhatsAppNumber = x.WhatsAppNumber,
                            IsSelfPickup = x.IsSelfPickup,
                            SellerId = x.FkAppUserId,
                            UserName = x.FkAppUser.AppUserProfiles.FirstOrDefault(y=> y.IsEnabled == true).Name,
                            FK_CountryId = new General<Guid?> { Id = x.FkCity.FkCountry, Name = x.FkCity.FkCountryNavigation.CountryDetails.FirstOrDefault(y => y.FkLanguageId == LanguageId && x.IsActive == true).Name },
                            IsDeliveryAvailable = x.IsDeliveryAvailable,
                            FK_SubCategoryId = new General<Guid?> { Id = x.FkSubCategoryId, Name = x.FkSubCategory.CategoriesDetails.FirstOrDefault(y => y.FkLanguageId == LanguageId).Name },
                            FK_CategoryId = new General<Guid> { Id = x.FkCategoryId, Name = x.FkCategory.CategoriesDetails.FirstOrDefault(y => y.FkLanguageId == LanguageId).Name },
                            FK_AppUserId = new General<Guid> { Id = x.FkAppUserId, Name = x.FkAppUser.AppUserProfiles.FirstOrDefault(y => y.IsEnabled == true).Name },
                            FK_CityId = new General<Guid> { Id = x.FkCityId, Name = x.FkCity.Citydetails.FirstOrDefault(y => y.FkLanguageId == LanguageId).Name },
                            FK_StatusId = x.FkStatusId,
                            CreatedDate = x.CreatedOn,
                            ThreadId = x.ChatThreads.FirstOrDefault(x => x.IsActive == true).Id,
                            AttachementList = x.Attachement.Where(y => y.IsActive == true).Select(y => new GetAttachementsResponseList
                            {
                                Url = y.Url
                            }).ToList(),
                        }).FirstOrDefault();
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
