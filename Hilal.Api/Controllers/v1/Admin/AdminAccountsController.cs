using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hilal.Api.Models.Authorizations;
using Hilal.Common;
using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Enum.Admin.v1;
using Hilal.DataViewModel.Request;
using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Response;
using Hilal.DataViewModel.Response.Admin.v1;
using Hilal.Service.Interface.v1.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Hilal.Api.Controllers.v1.Admin
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AdminAccountsController : ControllerBase
    {

        private readonly IAccountService accountService;
        private readonly IConfiguration configuration;
        public AdminAccountsController(IAccountService accountService, IConfiguration configuration)
        {
            this.accountService = accountService;
            this.configuration = configuration;
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
        [ProducesResponseType(typeof(Response<AdminLoginResponse>), 200)]
        [ProducesResponseType(typeof(Response<AdminLoginResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [Route("adminlogin")]
        public IActionResult AdminLogin(AdminLoginRequest login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                var res = accountService.AdminLogin(login);

                if (res.Item3)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response<AdminLoginResponse>() { IsError = true, Message = Error.AccountBlocked, Data = new AdminLoginResponse() });
                }

                if (res.Item2)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response<AdminLoginResponse>() { IsError = false, Message = "", Data = res.Item1 });
                }

                return StatusCode(StatusCodes.Status403Forbidden, new Response<AdminLoginResponse>() { IsError = true, Message = Error.LoginFailed, Data = new AdminLoginResponse() });
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
        [AdminAuthorize(ERight.UserManagement)]
        [HttpGet]
        [Route("addrole/{name}")]
        public async Task<IActionResult> AddRole(string name)
        {
            try
            {
                //var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                var userId = Guid.NewGuid();

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await accountService.AddRole(name, userId) });
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
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [ProducesResponseType(typeof(Response<bool>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AdminAuthorize(ERight.UserManagement)]
        [HttpPost]
        [Route("updaterole")]
        public async Task<IActionResult> UpdateRole(UpdateRoleRequest roleRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await accountService.UpdateRole(roleRequest, userId) });
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
        [AdminAuthorize(ERight.UserManagement)]
        [HttpGet]
        [Route("controllroleactivation/{roleId}/{activation:bool}")]
        public async Task<IActionResult> ControllRoleActivation(Guid roleId, bool activation)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await accountService.ControllRoleActivation(roleId, activation, userId) });
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
        [ProducesResponseType(typeof(Response<List<General<Guid>>>), 200)]
        [ProducesResponseType(typeof(Response<List<General<Guid>>>), 403)]
        [AdminAuthorize(ERight.UserManagement)]
        [HttpGet]
        [Route("getroles")]
        public IActionResult GetRoles()
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, new Response<List<General<Guid>>>() { IsError = false, Message = "", Data = accountService.GetRoles() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// roleId will be 00000000-0000-0000-0000-000000000000 in case of get all rights
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="500">If Server Error</response>  
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<List<General<Guid>>>), 200)]
        [ProducesResponseType(typeof(Response<List<General<Guid>>>), 403)]
        [AdminAuthorize(ERight.UserManagement)]
        [HttpGet]
        [Route("getrights/{roleId}")]
        public IActionResult GetRights(Guid roleId)
        {
            try
            {
                if (roleId.Equals(Guid.Empty))
                    return StatusCode(StatusCodes.Status200OK, new Response<List<General<Guid>>>() { IsError = false, Message = "", Data = accountService.GetRights() });

                return StatusCode(StatusCodes.Status200OK, new Response<List<General<Guid>>>() { IsError = false, Message = "", Data = accountService.GetRights(roleId) });
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
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [ProducesResponseType(typeof(Response<bool>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AdminAuthorize(ERight.UserManagement)]
        [HttpPost]
        [Route("assignrights")]
        public async Task<IActionResult> AssignRights(AssignRight assignRight)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
               // var userId = Guid.NewGuid();

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await accountService.AssignRights(assignRight, userId) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// SortBy = desc or asc
        /// SortIndex =  case 0: Name case 1: CountryCode case 2: PhoneNumber case 3: DateOfBirth case 4: Designation case 5: Email case 6: GenderName case 7: RoleName
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="400">If client made Validation Error</response>  
        /// <response code="403">If client made some mistake</response>  
        /// <response code="500">If Server Error</response>  
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<GetUsersResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetUsersResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AdminAuthorize(ERight.UserManagement)]
        [HttpPost]
        [Route("getusers")]
        public IActionResult GetUsers(ListGeneralModel page)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                return StatusCode(StatusCodes.Status200OK, new Response<GetUsersResponse>() { IsError = false, Message = "", Data = accountService.GetUsers(page) });
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
        [AdminAuthorize(ERight.UserManagement)]
        [Route("saveuser")]
        public async Task<IActionResult> SaveUser([FromForm] CreateUserRequest createUser, IFormFile profilePicture)
        {
            try
            {
                FileUrlResponce file = new FileUrlResponce();

                //createUser.ProfileImage = JsonConvert.DeserializeObject<FileUrlResponce>(Request.Form["profileImage"]);

                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
               // var userId = Guid.NewGuid();

                if (profilePicture != null && profilePicture.Length > 0)
                {
                    file = await new SaveFiles().SendMyFileToS3(profilePicture, configuration.GetValue<string>("Amazon:Bucket"), "AdminProfilePicture" + SystemGlobal.GetSubDirectoryName(), false, configuration.GetValue<string>("Amazon:AccessKey"), configuration.GetValue<string>("Amazon:AccessSecret"), configuration.GetValue<string>("Amazon:BaseUrl"));

                    createUser.ProfileImage = file;
                }

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await accountService.SaveUser(createUser, userId) });
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
        [ProducesResponseType(typeof(Response<CreateUserRequest>), 200)]
        [ProducesResponseType(typeof(Response<CreateUserRequest>), 403)]
        //[AdminAuthorize(ERight.UserManagement)]
        [HttpGet]
        [Route("getedituser/{userId}")]
        public IActionResult GetEditUser(Guid userId)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, new Response<CreateUserRequest>() { IsError = false, Message = "", Data = accountService.GetEditUser(userId) });
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
        [AdminAuthorize(ERight.UserManagement)]
        [HttpGet]
        [Route("controlluseractivation/{adminUserId}/{activation:bool}/{isRemove:bool}")]
        public async Task<IActionResult> ControllUserActivation(Guid adminUserId, bool activation, bool isRemove)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await accountService.ControllUserActivation(adminUserId, activation, isRemove, userId) });
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
        [AdminAuthorize(ERight.UserManagement)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [ProducesResponseType(typeof(Response<bool>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [Route("changepassword")]
        public async Task<IActionResult> ChangePassword(AdminChangePasswordRequest changePassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await accountService.ChangePassword(changePassword, userId) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
