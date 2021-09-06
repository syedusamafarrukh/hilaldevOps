using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Hilal.Common;
using Hilal.Data.Context;
using Hilal.Data.DTOs;
using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Request;
using Hilal.DataViewModel.Response;
using Hilal.Service.Interface.v1.App;
using Hilal.DataViewModel.Response.App.v1;
using Hilal.DataViewModel.Response.Admin.v1;

namespace Hilal.Service.Implementation.v1.App
{
    public class AppAccountService : IAppAccountService
    {
        private readonly IConfiguration configuration;
        private Encryption ea = new Encryption();
        private SentSMS sentSMS = new SentSMS();
        public AppAccountService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        #region Profile
        public GetEditProfileRequest GetEditProfile(Guid userId, int LanguagesId)
        {
            try
            {
                
                using (var db = new HilalDbContext())
                {
                    return db.AppUserProfiles.Include(x=> x.City).ThenInclude(x=> (x as Cities).Citydetails)
                        .Include(x => x.City).ThenInclude(x => (x as Cities).FkCountryNavigation).ThenInclude(x=> (x as Countries).CountryDetails)
                        .Where(x => x.AppUserId.Equals(userId) && x.IsEnabled)
                        .Select(x => new GetEditProfileRequest
                        {
                            Id = x.AppUserId,
                            FullName = x.Name,
                            Gender = x.Gender.Name,
                            GenderId = x.GenderId,
                            Nationality = x.Nationality,
                            ImageUrl = x.ImageUrl,
                            cityId = new General<Guid?> { Id = x.CityId, Name = x.City.Citydetails.FirstOrDefault(y=> y.IsActive == true && y.FkLanguageId == LanguagesId).Name },
                            CountryId = new General<Guid?> { Id = x.City.FkCountry, Name = x.City.FkCountryNavigation.CountryDetails.FirstOrDefault(y=> y.IsActive == true && y.FkLanguageId == LanguagesId).Name },
                            PhoneNumber= new PhoneNumberModel { CountryCode = x.PhoneCountryCode, PhoneNumber = x.PhoneNumber},
                            passwordCount = ea.Decrypt(x.AppUser.Password, configuration.GetValue<string>("EncryptionKey")).LongCount(),
                            ImageThumbnailUrl = x.ImageThumbnailUrl,
                            DateOfBirth = x.DateOfBirth,
                            Password = ea.Decrypt(x.AppUser.Password , configuration.GetValue<string>("EncryptionKey")),
                        })
                        .FirstOrDefault() ?? new GetEditProfileRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetAppUserResponse GetAppUsers(ListGeneralModel page)
        {
            try
            {
                using (var db = new HilalDbContext())
                { 
                    GetAppUserResponse response = new GetAppUserResponse();
                    var query = db.AppUserProfiles.Include(x=> x.AppUser).OrderByDescending(x=> x.CreatedOn)
                        .Where(x => x.IsEnabled == true).Select(x => new GetAppUsers
                        {
                            Id = x.Id,
                            AppUserId = x.AppUserId,
                            FullName = x.Name,
                            city = x.City.Citydetails.FirstOrDefault(y=> y.IsActive==true && y.FkLanguageId == page.LanguageId).Name,
                            Country = x.City.FkCountryNavigation.CountryDetails.FirstOrDefault(y=> y.IsActive==true && y.FkLanguageId == page.LanguageId).Name,
                            DateOfBirth = x.DateOfBirth,
                            Email = x.Email,
                            Gender = x.Gender.Name,
                            Nationality = x.Nationality,
                            Password = x.AppUser.Password,
                            ImageUrl = x.ImageUrl,
                            ImageThumbnailUrl = x.ImageThumbnailUrl,
                            PhoneNumber = x.PhoneCountryCode + x.PhoneNumber,
                        }).AsQueryable();

                    if (!string.IsNullOrEmpty(page.Search))
                    {
                        var date = new DateTime();
                        var sdate = DateTime.TryParse(page.Search, out date);
                        int totalCases = -1;
                        var isNumber = Int32.TryParse(page.Search, out totalCases);

                        query = query.Where(
                        x => x.FullName.ToLower().Contains(page.Search.ToLower())
                        || x.Email.ToLower().Contains(page.Search.ToLower())
                        || x.PhoneNumber.ToLower().Contains(page.Search.ToLower())
                        || x.Nationality.ToLower().Contains(page.Search.ToLower()));
                    }

                    response.Page = page.Page;
                    response.PageSize = page.PageSize;
                    response.TotalRecords = query.Count();

                    if (response.PageSize > 0)
                    {
                        response.appUsersList = query.Skip(response.Page).Take(response.PageSize).ToList();
                    }
                    else
                    {
                        response.appUsersList = query.ToList();
                    }
                    return response;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<bool> SaveProfile(ProfileRequest profileRequest)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    using (var trans = db.Database.BeginTransaction())
                    {
                        try
                        {
                            await db.AppUserProfiles.Where(x => x.IsEnabled && x.AppUserId.Equals(profileRequest.Id)).ForEachAsync(x => { x.IsEnabled = false; x.UpdatedBy = profileRequest.Id.ToString(); x.UpdatedOn = DateTime.UtcNow; });
                            await db.SaveChangesAsync();

                            var user = db.AppUsers.FirstOrDefault(x => x.Id.Equals(profileRequest.Id));

                            if (!string.IsNullOrEmpty(profileRequest.Password))
                            {
                                user.Password = new Encryption().Encrypt(profileRequest.Password, configuration.GetValue<string>("EncryptionKey"));
                                user.IsVerified = true;
                                db.Entry(user).State = EntityState.Modified;
                                await db.SaveChangesAsync();
                            }

                            db.AppUserProfiles.Add(new AppUserProfiles
                            {
                                Id = SystemGlobal.GetId(),
                                AppUserId = profileRequest.Id,
                                CityId = profileRequest.cityId,
                                DateOfBirth = profileRequest.DateOfBirth,
                                GenderId = profileRequest.GenderId,
                                ImageUrl = profileRequest.ImageUrl == null ? "" : profileRequest.ImageUrl,
                                ImageThumbnailUrl = profileRequest.ImageThumbnailUrl== null ? "" : profileRequest.ImageThumbnailUrl,
                                Name = profileRequest.FullName,
                                Nationality = profileRequest.Nationality,
                                Email = user.Email,
                                PhoneCountryCode = user.CountryCode,
                                PhoneNumber = user.PhoneNumber,
                                IsEnabled = true,
                                CreatedBy = profileRequest.Id.ToString(),
                                CreatedOn = DateTime.UtcNow,
                                CreatedOnDate = DateTime.UtcNow
                            });
                            await db.SaveChangesAsync();

                            trans.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         
        public List<AppUsersViewModel> GetAppUsersList()
        {
            using (var db = new HilalDbContext())
            {
                return db.AppUsers.Include(x=> x.AppUserProfiles).Where(x => x.IsEnabled == true).Select(x => new AppUsersViewModel
                {
                    Name = x.AppUserProfiles.FirstOrDefault( y => y.IsEnabled == true).Name,
                    Id = x.Id,
                    OTP = x.Otp,
                    PhoneNumber =x.CountryCode + x.PhoneNumber,
                    
                }).ToList();
            }
        }

        public async Task<bool> ChangePassword(ChangePasswordRequest changePassword, Guid userId)
        {
            try
            {
                string encryptionKey = configuration.GetValue<string>("EncryptionKey");
                bool response = false;

                var encryptedPassword = new Encryption().Encrypt(changePassword.OldPassword, encryptionKey);

                using (var db = new HilalDbContext())
                {
                    var user = db.AppUsers.FirstOrDefault(x => x.Id.Equals(userId) && x.Password.Equals(encryptedPassword));
                    if (user != null)
                    {
                        user.Password = new Encryption().Encrypt(changePassword.NewPassword, encryptionKey);
                        user.UpdatedOn = DateTime.UtcNow;
                        user.UpdatedBy = userId.ToString();

                        db.Entry(user).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        response = true;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Login/Signup
        public bool IsUserExists(string userinfo)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.AppUsers.Any(x => x.IsVerified == true && (userinfo.Contains("@") ? x.Email.ToLower().Equals(userinfo.ToLower()) : (x.CountryCode + x.PhoneNumber).Equals(userinfo)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Signup(SignUpRequest signUpRequest)
        {
            try
            {
                int verificationCode = SystemGlobal.Get4digitOTP();

                if (!IsUserExists(signUpRequest.PhoneNumber.CountryCode + signUpRequest.PhoneNumber.PhoneNumber))
                {
                    using (var db = new HilalDbContext())
                    {
                        db.AppUsers.Add(new AppUsers
                        {
                            Id = SystemGlobal.GetId(),
                            Email = signUpRequest.Email == null ? "" : signUpRequest.Email.ToLower(),
                            CountryCode = signUpRequest.PhoneNumber.CountryCode,
                            PhoneNumber = signUpRequest.PhoneNumber.PhoneNumber,
                            Otp = verificationCode.ToString(),
                            IsVerified = false,
                            IsBlocked = false,
                            Password = "",
                            IsEnabled = true,
                            CreatedBy = "signup",
                            CreatedOn = DateTime.UtcNow,                            
                            CreatedOnDate = DateTime.UtcNow,
                            IsSubscribed = false
                        });
                        await db.SaveChangesAsync();
                    }
                }
                else
                {
                    using (var db = new HilalDbContext())
                    {
                        var user = db.AppUsers.FirstOrDefault(x => (x.CountryCode + x.PhoneNumber).Equals(signUpRequest.PhoneNumber.CountryCode + signUpRequest.PhoneNumber.PhoneNumber.ToLower()));

                        if (user.IsVerified) return false;
                        //user.IsVerified = true;
                        user.Otp = verificationCode.ToString();
                        user.UpdatedBy = "signup";
                        user.UpdatedOn = DateTime.UtcNow;

                        db.Entry(user).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }

                await SentOTP(signUpRequest.Email, signUpRequest.PhoneNumber.CountryCode + signUpRequest.PhoneNumber.PhoneNumber, verificationCode);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Guid> VerifyOTP(string userinfo, int otp)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    var user = db.AppUsers.FirstOrDefault(x => (userinfo.Contains("@") ? x.Email.ToLower().Equals(userinfo.ToLower()) : (x.CountryCode + x.PhoneNumber).Equals(userinfo.ToLower())) && x.Otp.Equals(otp.ToString()));

                    if (user == null) return Guid.Empty;

                    user.Otp = "";
                    user.UpdatedBy = "signup Verifyied";
                    user.UpdatedOn = DateTime.UtcNow;

                    db.Entry(user).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return user.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Tuple<AppLoginResponse, bool, bool, bool, bool>> AppLogin(AppLoginRequest request)
        {
            string encryptionKey = configuration.GetValue<string>("EncryptionKey"), tokenKey = configuration.GetValue<string>("Tokens:Key");

            try
            {
                var encryptedPassword = new Encryption().Encrypt(request.Password, encryptionKey);
                var response = new AppLoginResponse();
                bool isLogin = false,
                    isWrongPassword = false,
                    isBlock = false,
                    isVerified = false;

                using (var db = new HilalDbContext())
                {
                    var user = db.AppUsers
                        .Include(x => x.AppUserProfiles).Include(x=> x.AppUserSubscription).FirstOrDefault(x => (request.UserInfo.Contains("@") ? x.Email.ToLower().Equals(request.UserInfo.ToLower()) : (x.CountryCode + x.PhoneNumber).Equals(request.UserInfo)) && x.Password.Equals(encryptedPassword));

                    if (user == null) return Tuple.Create(response, isLogin , isBlock, isVerified, isWrongPassword = true);
                    isBlock = user.IsBlocked;
                    if (isBlock) return Tuple.Create(response, isLogin, isBlock, isVerified, isWrongPassword = true);
                    isVerified = user.IsVerified;
                    if (!isVerified) return Tuple.Create(response, isLogin, isBlock, isVerified, isWrongPassword);

                    var authToken = new Encryption().GetToken(new AuthToken { UserId = user.Id, DeviceToken = request.DeviceToken }, user.Id, tokenKey);

                    var res = AddMobileInfo(user.Id, request.DeviceToken, request.DeviceType, request.DeviceModel, request.OS, request.Version);

                    if (!res) return Tuple.Create(response, isLogin, isBlock, isVerified, isWrongPassword);

                    response = new AppLoginResponse
                    {
                        AccessToken = authToken,
                        IsAccountVerified = user.IsVerified,
                        PhoneNumber = new PhoneNumberModel { CountryCode = user.CountryCode , PhoneNumber = user.PhoneNumber },
                        UserId = user.Id,
                        IsSubscribed = user.IsSubscribed,
                        PlanId = user.AppUserSubscription.FirstOrDefault(x=> x.IsActive == true)?.FkSubscribedPlanId
                    };
                    isLogin = true;

                    await db.GuestAppUserDeviceInformations
                        .Where(x => x.DeviceToken.Equals(request.DeviceToken) && x.DeviceTypeId == request.DeviceType && x.IsEnabled == true)
                        .ForEachAsync(x => { x.IsEnabled = false; x.DeletedBy = user.Id.ToString(); x.DeletedOn = DateTime.UtcNow; });
                    await db.SaveChangesAsync();

                    return Tuple.Create(response, isLogin, isBlock, isVerified, isWrongPassword);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public async Task<Tuple<bool>> SaveDeviceInformation(SaveDeviceInformationRequest request)
        {
            string encryptionKey = configuration.GetValue<string>("EncryptionKey"), tokenKey = configuration.GetValue<string>("Tokens:Key");

            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var user = db.AppUsers.Include(x => x.AppUserProfiles).FirstOrDefault(x => x.Id == request.UserId);

                    if (user == null) return Tuple.Create(response);
                    var res = AddMobileInfo(user.Id, request.DeviceToken, request.DeviceType, request.DeviceModel, request.OS, request.Version);

                    if (!res) return Tuple.Create(response);

                    await db.GuestAppUserDeviceInformations
                        .Where(x => x.DeviceToken.Equals(request.DeviceToken) && x.DeviceTypeId == request.DeviceType && x.IsEnabled == true)
                        .ForEachAsync(x => { x.IsEnabled = false; x.DeletedBy = user.Id.ToString(); x.DeletedOn = DateTime.UtcNow; });
                    await db.SaveChangesAsync();
                    response = true;
                    return Tuple.Create(response);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Logout(Guid userId, string deviceToken, bool logoutFromAll)
        {
            try
            {
                bool res = false;
                using (var db = new HilalDbContext())
                {
                    using (var comite = db.Database.BeginTransaction())
                    {
                        try
                        {
                            if (logoutFromAll)
                            {
                                db.UserDeviceInformations.Where(x => x.AppUserId.Equals(userId) && x.IsEnabled).ToList().ForEach(x => { x.IsEnabled = false; x.UpdatedOn = DateTime.UtcNow; x.UpdatedBy = userId.ToString(); });
                                db.SaveChanges();
                                comite.Commit();
                                res = true;
                            }
                            else
                            {
                                var model = db.UserDeviceInformations.FirstOrDefault(x => x.AppUserId.Equals(userId) && x.DeviceToken.Equals(deviceToken));
                                if (model != null)
                                {
                                    model.IsEnabled = false;
                                    model.UpdatedOn = DateTime.UtcNow;
                                    model.UpdatedBy = userId.ToString();

                                    db.Entry(model).State = EntityState.Modified;
                                    await db.SaveChangesAsync();
                                    comite.Commit();
                                    res = true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            comite.Rollback();
                            throw ex;
                        }
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ForgotPassword(string userinfo)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var user = db.AppUsers.FirstOrDefault(x => (userinfo.Contains("@") ? x.Email.ToLower().Equals(userinfo.ToLower()) : (x.CountryCode + x.PhoneNumber).Equals(userinfo)));

                    if (user != null)
                    {
                        int verificationCode = SystemGlobal.Get4digitOTP();

                        user.Otp = verificationCode.ToString();

                        db.Entry(user).State = EntityState.Modified;
                        await db.SaveChangesAsync();


                        response = await SentOTP(user.Email, userinfo, verificationCode);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ResetPassword(string newPassword, Guid userId)
        {
            try
            {
                string encryptionKey = configuration.GetValue<string>("EncryptionKey");
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var user = db.AppUsers.FirstOrDefault(x => x.Id.Equals(userId));
                    if (user != null)
                    {
                        user.Password = new Encryption().Encrypt(newPassword, encryptionKey);
                        user.UpdatedOn = DateTime.UtcNow;
                        user.UpdatedBy = userId.ToString();

                        db.Entry(user).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        response = true;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool AddMobileInfo(Guid userId, string deviceToken, int deviceType, string name, string versionName, string version)
        {
            bool result = false;
            try
            {
                using (var db = new HilalDbContext())
                {
                    using (var trans = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.UserDeviceInformations.Where(x => x.AppUserId.Equals(userId) && x.IsEnabled).ToList().ForEach(x => { x.IsEnabled = false; x.UpdatedBy = userId.ToString(); x.UpdatedOn = DateTime.UtcNow; });
                            db.SaveChanges();

                            db.UserDeviceInformations.Add(new UserDeviceInformations
                            {
                                Id = SystemGlobal.GetId(),
                                Name = name,
                                Version = version,
                                VersionName = versionName,
                                DeviceTypeId = deviceType,
                                DeviceToken = deviceToken,
                                AppUserId = userId,
                                IsEnabled = true,
                                CreatedBy = userId.ToString(),
                                CreatedOn = DateTime.UtcNow,
                                CreatedOnDate = DateTime.UtcNow,
                            });
                            db.SaveChanges();
                            trans.Commit();
                            result = true;
                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                            result = false;
                        }
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> SentOTP(string email, string phoneNumber, int verificationCode)
        {
            try
            {
                //await Task.Run(() => { Email.SendEmail("Waseet Password Recovery", "Enter this " + verificationCode + "for reset Password.</a>", configuration.GetValue<string>("Email"), configuration.GetValue<string>("Password"), email); });
                //await Task.Run(() => { new SentSMS().SendSmsToUser("Your OPT is : " + verificationCode, phoneNumber, configuration.GetValue<string>("Twilio:AccountSID"), configuration.GetValue<string>("Twilio:AuthToken"), configuration.GetValue<string>("Twilio:FromNumber"));});
                await sentSMS.SendSMS(phoneNumber, "Your OPT is : " + verificationCode);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("OTP Sending Failed");
            }
        }
        #endregion

        #region Authorization
        public bool IsTokenValid(Guid userId, string deviceToken)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    var model = db.UserDeviceInformations.FirstOrDefault(x => x.AppUserId.Equals(userId) && x.DeviceToken.Equals(deviceToken) && x.IsEnabled == true);
                    return model == null ? false : true;
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public bool IsAccountVerified(Guid userId)
        {
            try
            {
                bool res = false;

                using (var db = new HilalDbContext())
                {
                    var model = db.AppUsers.FirstOrDefault(x => x.Id.Equals(userId));

                    if (model != null)
                    {
                        res = model.IsVerified;
                    }
                }

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
