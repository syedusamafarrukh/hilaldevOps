using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Hilal.Api.Models.Authorizations;
using Hilal.Common;
using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Enum.Admin.v1;
using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using Hilal.Service.Interface.v1.App;
using Hilal.DataViewModel.Request.App.v1;
using Hilal.DataViewModel.Response.App.v1;
using Hilal.DataViewModel.Request;

namespace Hilal.Api.Controllers.v1.App
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BusinessProfileController : ControllerBase
    {
        private readonly IBusinessProfileService businessProfileService;
        private readonly IConfiguration configuration;
        public BusinessProfileController(IBusinessProfileService businessProfileService, IConfiguration configuration)
        {
            this.businessProfileService = businessProfileService;
            this.configuration = configuration;
        }

        /// <summary>
        /// SortBy = desc or asc
        /// case 0: Name case 1: Priority case 2: x.IsActive
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="400">If client made Validation Error</response>  
        /// <response code="403">If client made some mistake</response>  
        /// <response code="500">If Server Error</response>  
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<GetCategoryResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetCategoryResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AppAuthorize(false)]
        [HttpPost]
        [Route("getBusinessProfile")]
        public IActionResult GetBusinessProfile(GetBusinessProfileRequest page)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                page.LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                Guid? userId = Guid.Parse(RouteData.Values["userId"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<GetUserBusinessProfileResponse>() { IsError = false, Message = "", Data = businessProfileService.GetBusinessProfileService(page,userId) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// SortBy = desc or asc
        /// case 0: Name case 1: Priority case 2: x.IsActive
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="400">If client made Validation Error</response>  
        /// <response code="403">If client made some mistake</response>  
        /// <response code="500">If Server Error</response>  
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<GetUserBusinessProfileListResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetUserBusinessProfileListResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AppAuthorize(false)]
        [HttpPost]
        [Route("GetBusinessServicesTest")]
        public IActionResult GetBusinessServicesTest(GetBusinessProfileRequest page)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                page.LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                Guid? userId = Guid.Parse(RouteData.Values["userId"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<GetUserBusinessProfileListResponse>() { IsError = false, Message = "", Data = businessProfileService.GetBusinessServices(page,userId) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// SortBy = desc or asc
        /// case 0: Name case 1: Priority case 2: x.IsActive
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="400">If client made Validation Error</response>  
        /// <response code="403">If client made some mistake</response>  
        /// <response code="500">If Server Error</response>  
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<GetUserBusinessProfileListResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetUserBusinessProfileListResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AppAuthorize(false)]
        [HttpPost]
        [Route("GetBusinessServices")]
        public async Task<IActionResult> GetBusinessServices(GetBusinessProfileRequest page)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                page.LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                Guid? userId = Guid.Parse(RouteData.Values["userId"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<GetUserBusinessProfileListResponse>() { IsError = false, Message = "", Data = await businessProfileService.GetBusinessServicesSP(page, userId) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// SortBy = desc or asc
        /// case 0: Name case 1: Priority case 2: x.IsActive
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="400">If client made Validation Error</response>  
        /// <response code="403">If client made some mistake</response>  
        /// <response code="500">If Server Error</response>  
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<GetServicesDetail>), 200)]
        [ProducesResponseType(typeof(Response<GetServicesDetail>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AppAuthorize(false)]
        [HttpGet]
        [Route("GetBusinessServicesDetail/{BusinessProfileId}")]
        public IActionResult GetBusinessServicesDetail(Guid BusinessProfileId)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                var LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                Guid? userId = Guid.Parse(RouteData.Values["userId"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<GetServicesDetail>() { IsError = false, Message = "", Data = businessProfileService.GetBusinessServicesDetail(BusinessProfileId, LanguageId,userId) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///// <summary>
        ///// </summary>
        ///// <response code="200">If all working fine</response>
        ///// <response code="500">If Server Error</response>  
        //[ProducesResponseType(200)]
        //[ProducesResponseType(500)]
        //[ProducesResponseType(typeof(Response<CreateCategoryRequest>), 200)]
        //[ProducesResponseType(typeof(Response<CreateCategoryRequest>), 403)]
        //[AppAuthorize(true)]
        //[HttpGet]
        //[Route("geteditBusinessProfile/{BusinessProfileId}")]
        //public IActionResult geteditBusinessProfile(Guid BusinessProfileId)
        //{
        //    try
        //    {

        //        return StatusCode(StatusCodes.Status200OK, new Response<CreateAppUserBusinessProfileRequest>() { IsError = false, Message = "", Data = businessProfileService.GetEditBusinessProfile(BusinessProfileId) });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        
        /// <summary>
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="500">If Server Error</response>  
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<GetEditUserBusinessProfileRequest>), 200)]
        [ProducesResponseType(typeof(Response<GetEditUserBusinessProfileRequest>), 403)]
        [AppAuthorize(true)]
        [HttpGet]
        [Route("geteditBusinessProfile/{BusinessProfileId}")]
        public IActionResult geteditBusinessProfile(Guid BusinessProfileId)
        {
            try
            {
                var LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<GetEditUserBusinessProfileRequest>() { IsError = false, Message = "", Data = businessProfileService.GetEditBusinessProfileApp( BusinessProfileId, LanguageId )});
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
        [AppAuthorize(true)]
        [Route("saveBusinessProfile")]
        public async Task<IActionResult> SaveBusinessProfile([FromForm] CreateAppUserBusinessProfileRequest BusinessProfileRequest, IFormFile LogoIcon, List<IFormFile> Attachements)
        {
            try
            {
                FileUrlResponce file = new FileUrlResponce();
                BusinessProfileRequest.AttachementList = new List<CreateAttachementsRequest>();
                //BusinessProfileRequest.LogoIcon = JsonConvert.DeserializeObject<FileUrlResponce>(Request.Form["LogoIcon"]);
                BusinessProfileRequest.UserBusinessProfileDetailInformation = JsonConvert.DeserializeObject<List<CreateUserBusinessProfileDetailRequest>>(Request.Form["UserBusinessProfileDetailInformation"]);
                if (BusinessProfileRequest.Id != null)
                {
                    if (Request.Form.Keys.Contains("AttachementList"))
                    {
                        BusinessProfileRequest.AttachementList = JsonConvert.DeserializeObject<List<CreateAttachementsRequest>>(Request.Form["AttachementList"]);
                    }
                }
               // BusinessProfileRequest.AttachementList = new List<CreateAttachementsRequest>();
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                BusinessProfileRequest.FK_AppUserId = userId;
                if (LogoIcon != null && LogoIcon.Length > 0)
                {
                    file = await new SaveFiles().SendMyFileToS3(LogoIcon, configuration.GetValue<string>("Amazon:Bucket"), "Category" + SystemGlobal.GetSubDirectoryName(), false, configuration.GetValue<string>("Amazon:AccessKey"), configuration.GetValue<string>("Amazon:AccessSecret"), configuration.GetValue<string>("Amazon:BaseUrl"));

                    BusinessProfileRequest.LogoIcon = file;
                }

                if (Attachements != null && Attachements.Count > 0)
                {
                    foreach (var item in Attachements)
                    {
                        file = await new SaveFiles().SendMyFileToS3(item, configuration.GetValue<string>("Amazon:Bucket"), "Advertisement" + SystemGlobal.GetSubDirectoryName(), false, configuration.GetValue<string>("Amazon:AccessKey"), configuration.GetValue<string>("Amazon:AccessSecret"), configuration.GetValue<string>("Amazon:BaseUrl"));

                        CreateAttachementsRequest AttachementObject = new CreateAttachementsRequest
                        {
                            Url = file.URL,
                            ThubnilUrl = file.ThumbnailUrl,
                            IsVideo = false,
                        };
                        if (BusinessProfileRequest.AttachementList == null)
                        {
                            BusinessProfileRequest.AttachementList = new List<CreateAttachementsRequest>();
                        }
                        BusinessProfileRequest.AttachementList.Add(AttachementObject);
                    }
                }


                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await businessProfileService.SaveBusinessProfile(BusinessProfileRequest, userId) });
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
        [AppAuthorize(true)]
        [HttpGet]
        [Route("controllBusinessProfileactivation/{BusinessProfileId}/{activation:bool}")]
        public async Task<IActionResult> ControllBusinessProfileActivation(Guid BusinessProfileId, bool activation)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await businessProfileService.ControllBusinessProfileActivation(BusinessProfileId, activation, userId) });
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
        [AppAuthorize(true)]
        [HttpGet]
        [Route("ControllBusinessProfileStatus/{BusinessProfileId}/{StatusId:int}")]
        public async Task<IActionResult> ControllBusinessProfileStatus(Guid BusinessProfileId, int StatusId)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await businessProfileService.ControllBusinessProfileStatusApp(BusinessProfileId, StatusId, userId) });
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
        [ProducesResponseType(typeof(Response<CreateCategoryRequest>), 200)]
        [ProducesResponseType(typeof(Response<CreateCategoryRequest>), 403)]
        [AppAuthorize(true)]
        [HttpGet]
        [Route("GetBusinessProfileServiceByUserId")]
        public IActionResult GetBusinessProfileServiceByUserId()
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                var languageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<GetBusinessProfile>() { IsError = false, Message = "", Data = businessProfileService.GetBusinessProfileServiceByUserId(userId, languageId) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
