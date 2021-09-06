using Hilal.DataViewModel.Request;
using Hilal.DataViewModel.Response;
using Hilal.DataViewModel.Response.Admin.v1;
using Hilal.DataViewModel.Response.App.v1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hilal.Service.Interface.v1.App
{
    public interface IAppAccountService
    {
        GetEditProfileRequest GetEditProfile(Guid userId, int LanguagesId);
        GetAppUserResponse GetAppUsers(ListGeneralModel page);
        Task<bool> SaveProfile(ProfileRequest profileRequest);
        List<AppUsersViewModel> GetAppUsersList();
        Task<bool> ChangePassword(ChangePasswordRequest changePassword, Guid userId);
        bool IsUserExists(string userinfo);
        Task<bool> Signup(SignUpRequest signUpRequest);
        Task<Guid> VerifyOTP(string userinfo, int otp);
        Task<Tuple<AppLoginResponse, bool, bool, bool, bool>> AppLogin(AppLoginRequest request);
        Task<Tuple<bool>> SaveDeviceInformation(SaveDeviceInformationRequest request);
        Task<bool> Logout(Guid userId, string deviceToken, bool logoutFromAll);
        Task<bool> ForgotPassword(string userinfo);
        Task<bool> ResetPassword(string newPassword, Guid userId);
        bool IsTokenValid(Guid userId, string deviceToken);
        bool IsAccountVerified(Guid userId);
    }
}
