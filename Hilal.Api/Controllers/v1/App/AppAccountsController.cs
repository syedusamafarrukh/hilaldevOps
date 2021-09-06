using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hilal.Api.Models.Authorizations;
using Hilal.Common;
using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Request;
using Hilal.DataViewModel.Response;
using Hilal.DataViewModel.Response.App.v1;
using Hilal.Service.Interface.v1.App;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Hilal.Api.Controllers.v1.App
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AppAccountsController : ControllerBase
    {
        private readonly IAppAccountService accountService;
        private readonly IConfiguration configuration;

        public AppAccountsController(IConfiguration configuration, IAppAccountService accountService)
        {
            this.configuration = configuration;
            this.accountService = accountService;
        }

        /// <summary>
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="400">If client made Validation Error</response>  
        /// <response code="403">If client made some mistake</response>  
        /// <response code="500">If Server Error</response>  
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<AppLoginResponse>), 200)]
        [ProducesResponseType(typeof(Response<AppLoginResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [Route("applogin")]
        public async Task<IActionResult> AppLogin(AppLoginRequest login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                login.UserInfo = "+" + login.UserInfo;
                var res = await accountService.AppLogin(login);

                if (res.Item2)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response<AppLoginResponse>() { IsError = false, Message = "", Data = res.Item1 });
                }

                if (res.Item5)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response<AppLoginResponse>() { IsError = true, Message = Error.WrongPassword, Data = new AppLoginResponse() });
                }

                if (res.Item3)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response<AppLoginResponse>() { IsError = true, Message = Error.AccountBlocked, Data = new AppLoginResponse() });
                }

                if (!res.Item4)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response<AppLoginResponse>() { IsError = true, Message = Error.AccoutNotVerified, Data = new AppLoginResponse() });
                }

                return StatusCode(StatusCodes.Status403Forbidden, new Response<AppLoginResponse>() { IsError = true, Message = Error.LoginFailed, Data = new AppLoginResponse() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="400">If client made Validation Error</response>  
        /// <response code="403">If client made some mistake</response>  
        /// <response code="500">If Server Error</response>  
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [ProducesResponseType(typeof(Response<bool>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [Route("SaveDeviceInformation")]
        public async Task<IActionResult> SaveDeviceInformation(SaveDeviceInformationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                var res = await accountService.SaveDeviceInformation(request);

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = res.Item1 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="500">If Server Error</response>  
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [ProducesResponseType(typeof(Response<bool>), 403)]
        [HttpGet]
        [Route("isuserexists/{userinfo}")]
        public IActionResult IsUserExists(string userinfo)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = accountService.IsUserExists("+" + userinfo) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="500">If Server Error</response>  
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<List<AppUsersViewModel>>), 200)]
        [ProducesResponseType(typeof(Response<List<AppUsersViewModel>>), 403)]
        [HttpGet]
        [AppAuthorize(false)]
        [Route("GetAppUsersList")]
        public IActionResult GetAppUsersList()
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, new Response<List<AppUsersViewModel>>() { IsError = false, Message = "", Data = accountService.GetAppUsersList() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="400">If client made Validation Error</response>  
        /// <response code="403">If client made some mistake</response>  
        /// <response code="500">If Server Error</response>  
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [ProducesResponseType(typeof(Response<bool>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [Route("signup")]
        public async Task<IActionResult> Signup(SignUpRequest signUpRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                signUpRequest.PhoneNumber.CountryCode = "+" + signUpRequest.PhoneNumber.CountryCode;
                var res = await accountService.Signup(signUpRequest);

                if (res)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = true });
                }


                return StatusCode(StatusCodes.Status403Forbidden, new Response<bool>() { IsError = true, Message = Error.EmailAlreadyExists, Data = false });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// if return this 00000000-0000-0000-0000-000000000000 verification failed else passed 
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="500">If Server Error</response>  
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<Guid>), 200)]
        [ProducesResponseType(typeof(Response<Guid>), 403)]
        [HttpGet]
        [AppAuthorize(false)]
        [Route("verifyotp/{userinfo}/{otp:int}")]
        public async Task<IActionResult> VerifyOTP(string userinfo, int otp)
        {
            try
            {
                userinfo = "+" + userinfo;
                return StatusCode(StatusCodes.Status200OK, new Response<Guid>() { IsError = false, Message = "", Data = await accountService.VerifyOTP(userinfo, otp) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="500">If Server Error</response>  
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [ProducesResponseType(typeof(Response<bool>), 403)]
        [HttpGet]
        [AppAuthorize(false)]
        [Route("forgotpassword/{userinfo}")]
        public async Task<IActionResult> ForgotPassword(string userinfo)
        {
            try
            {
                userinfo = "+" + userinfo;
                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await accountService.ForgotPassword(userinfo) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="400">If client made Validation Error</response>  
        /// <response code="403">If client made some mistake</response>  
        /// <response code="500">If Server Error</response>  
        [HttpPost]
        [AppAuthorize(false)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [ProducesResponseType(typeof(Response<bool>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [Route("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                var res = await accountService.ResetPassword(resetPasswordRequest.NewPassword, resetPasswordRequest.Id);

                if (res)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = true });
                }


                return StatusCode(StatusCodes.Status403Forbidden, new Response<bool>() { IsError = true, Message = Error.ServerError, Data = false });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="400">If client made Validation Error</response>  
        /// <response code="403">If client made some mistake</response>  
        /// <response code="500">If Server Error</response>  
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [ProducesResponseType(typeof(Response<bool>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [Route("saveprofile")]
        public async Task<IActionResult> SaveProfile([FromForm] ProfileRequest profile, IFormFile profilePicture)
        {
            try
            {
                FileUrlResponce file = new FileUrlResponce();
                
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                if (profilePicture != null && profilePicture.Length > 0)
                {
                    profile.ImageUrl = "";
                    profile.ImageThumbnailUrl = "";
                    file = await new SaveFiles().SendMyFileToS3(profilePicture, configuration.GetValue<string>("Amazon:Bucket"), "ProfilePicture" + SystemGlobal.GetSubDirectoryName(), false, configuration.GetValue<string>("Amazon:AccessKey"), configuration.GetValue<string>("Amazon:AccessSecret"), configuration.GetValue<string>("Amazon:BaseUrl"));
                    profile.ImageUrl = file.URL;
                    profile.ImageThumbnailUrl = file.ThumbnailUrl;
                }

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await accountService.SaveProfile(profile) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="500">If Server Error</response>  
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<GetEditProfileRequest>), 200)]
        [ProducesResponseType(typeof(Response<GetEditProfileRequest>), 403)]
        [HttpGet]
        [AppAuthorize(true)]
        [Route("geteditprofile")]
        public IActionResult GetEditProfile()
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                var LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<GetEditProfileRequest>() { IsError = false, Message = "", Data = accountService.GetEditProfile(userId, LanguageId)});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="400">If client made Validation Error</response>  
        /// <response code="403">If client made some mistake</response>  
        /// <response code="500">If Server Error</response>  
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [ProducesResponseType(typeof(Response<bool>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [Route("changepassword")]
        [AppAuthorize(false)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest changePassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                var res = await accountService.ChangePassword(changePassword, userId);

                if (res)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = true });
                }


                return StatusCode(StatusCodes.Status403Forbidden, new Response<bool>() { IsError = true, Message = Error.UserDoesnotExists, Data = false });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="500">If Server Error</response>  
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [ProducesResponseType(typeof(Response<bool>), 403)]
        [HttpGet]
        [AppAuthorize(true)]
        [Route("logout/{logoutFromAll:bool}")]
        public async Task<IActionResult> Logout(bool logoutFromAll)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                var deviceToken = RouteData.Values["DeviceToken"].ToString();

                await accountService.Logout(userId, deviceToken, logoutFromAll);

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = true });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
