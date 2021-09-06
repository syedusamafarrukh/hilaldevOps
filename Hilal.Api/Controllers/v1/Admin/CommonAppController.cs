using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hilal.Api.Models.Authorizations;
using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Enum.Admin.v1;
using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Request.App.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using Hilal.DataViewModel.Response.App.v1;
using Hilal.Service.Interface.v1.Admin;
using Hilal.Service.Interface.v1.App;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hilal.Api.Controllers.v1.Admin
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CommonAppController : ControllerBase
    {
        private readonly  IAdvertisementService advertisementService;
        private readonly IAppAccountService appAccountService;
        private readonly IBlogService blogService;
        private readonly IBusinessProfileService businessProfileService;  
        public CommonAppController(IAdvertisementService advertisementService, IAppAccountService appAccountService, IBlogService blogService, IBusinessProfileService businessProfileService)
        {
            this.advertisementService = advertisementService;
            this.appAccountService = appAccountService;
            this.blogService = blogService;
            this.businessProfileService = businessProfileService;
        }


        /// <summary>
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="500">If Server Error</response>  
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<GetAppAdvertisementResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetAppAdvertisementResponse>), 403)]
        [AdminAuthorize(ERight.Category)]
        [HttpPost]
        [Route("GetAdvertisementServicePList")]
        public IActionResult GetAdvertisementServicePList(GetAdvertisementRequest advertisementRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                advertisementRequest.LanguageId = 2;
                Guid? userId = (Guid?) null;
                return StatusCode(StatusCodes.Status200OK, new Response<GetAppAdvertisementResponse>() { IsError = false, Message = "", Data = advertisementService.GetAdvertisementServicePList(advertisementRequest, userId,1) });

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
        [ProducesResponseType(typeof(Response<GetCategoryResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetCategoryResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AdminAuthorize(ERight.Category)]
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
                page.LanguageId =  2; 
                Guid? userId = (Guid?) null;
                return StatusCode(StatusCodes.Status200OK, new Response<GetUserBusinessProfileResponse>() { IsError = false, Message = "", Data = businessProfileService.GetBusinessProfileService(page,userId) });
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
        [ProducesResponseType(typeof(Response<GetAdminNotificationResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetAdminNotificationResponse>), 403)]
       // [AdminAuthorize(ERight.Advertisement)]
        [HttpPost]
        [Route("GetAdminNotifications")]
        public IActionResult GetAdminNotifications(ListGeneralModel page)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
              //  advertisementRequest.LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<GetAdminNotificationResponse>() { IsError = false, Message = "", Data = advertisementService.GetAdminNotifications(page) });

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
        [ProducesResponseType(typeof(Response<GetAppUserResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetAppUserResponse>), 403)]
      //  [AdminAuthorize(ERight.AppUsers)]
        [HttpPost]
        [Route("GetAppUser")]
        public IActionResult GetAppUsers(ListGeneralModel page)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                return StatusCode(StatusCodes.Status200OK, new Response<GetAppUserResponse>() { IsError = false, Message = "", Data = appAccountService.GetAppUsers(page) });

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
        [ProducesResponseType(typeof(Response<CreateAdvertisementRequest>), 200)]
        [ProducesResponseType(typeof(Response<CreateAdvertisementRequest>), 403)]
        //[AppAuthorize(true)]
        [HttpGet]
        [Route("GetEditAdvertisement/{AdvertisementId}")]
        public IActionResult GetEditAdvertisement(Guid AdvertisementId)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, new Response<CreateAdvertisementRequest>() { IsError = false, Message = "", Data = advertisementService.GetEditAdvertisement(AdvertisementId) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [ProducesResponseType(typeof(Response<bool>), 403)]
        [HttpPost]
        [AdminAuthorize(ERight.Category)]
        [Route("ControllAdvertisementStatus")]
        public async Task<IActionResult> ControllAdvertisementStatus(AddControlStatusRequest addControlStatusRequest)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await advertisementService.ControllAdvertisementStatus(addControlStatusRequest.AdvertisementId, addControlStatusRequest.StatusId, userId , addControlStatusRequest.comments) });
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
        [AdminAuthorize(ERight.Category)]
        [HttpPost]
        [Route("ControllBusinessProfileStatus")]
        public async Task<IActionResult> ControllBusinessProfileStatus(ServiceControlStatusRequest serviceControlStatusRequest)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await businessProfileService.ControllBusinessProfileStatus(serviceControlStatusRequest.BusinessProfileId, serviceControlStatusRequest.StatusId, userId, serviceControlStatusRequest.comments) });
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
        [ProducesResponseType(typeof(Response<GetBlogResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetBlogResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AppAuthorize(false)]
        [HttpPost]
        [Route("GetBlogList")]
        public IActionResult GetBlogList(ListGeneralModel page)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                return StatusCode(StatusCodes.Status200OK, new Response<GetBlogResponse>() { IsError = false, Message = "", Data = blogService.GetBlog(page) });
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
        [ProducesResponseType(typeof(Response<GetAdvertisemetDetail>), 200)]
        [ProducesResponseType(typeof(Response<GetAdvertisemetDetail>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AdminAuthorize(ERight.Category)]
        [HttpGet]
        [Route("GetAdvertisementDetail")]
        public IActionResult GetAdvertisementDetail(Guid Id)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                //var LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                Guid? userId = Guid.Parse(RouteData.Values["userId"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<GetAdvertisemetDetail>() { IsError = false, Message = "", Data = advertisementService.GetAdvertisementDetail(Id, 2, userId) });
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
        [AdminAuthorize(ERight.Category)]
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
                var LanguageId = 2;
                Guid? userId = Guid.Parse(RouteData.Values["userId"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<GetServicesDetail>() { IsError = false, Message = "", Data = businessProfileService.GetBusinessServicesDetail(BusinessProfileId, LanguageId, userId) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
