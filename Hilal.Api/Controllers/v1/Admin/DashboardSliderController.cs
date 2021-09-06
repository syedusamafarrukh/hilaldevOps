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
using Hilal.Service.Interface.v1.Admin;
using Hilal.DataViewModel.Request;

namespace Hilal.Api.Controllers.v1.Admin
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DashboardSliderController : ControllerBase
    {
        private readonly IDashboardSliderService DashboardSliderService;
        private readonly IConfiguration configuration;
        public DashboardSliderController(IDashboardSliderService DashboardSliderService, IConfiguration configuration)
        {
            this.DashboardSliderService = DashboardSliderService;
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
        [ProducesResponseType(typeof(Response<GetDashboardSliderResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetDashboardSliderResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AdminAuthorize(ERight.Breed)]
        [HttpPost]
        [Route("getDashboardSlider")]
        public IActionResult GetDashboardSlider(int languageId)
        {
            languageId = 2;
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                return StatusCode(StatusCodes.Status200OK, new Response<GetDashboardSliderResponse>() { IsError = false, Message = "", Data = DashboardSliderService.GetDashboardSlider(languageId) });
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
        [ProducesResponseType(typeof(Response<GetDashboardSliderResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetDashboardSliderResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        //[AdminAuthorize(ERight.DashboardSlider)]
        [HttpPost]
        [Route("GetDashboardSliderList")]
        public IActionResult GetDashboardSliderList()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                var languageId = Convert.ToInt32(RouteData.Values["Language"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<GetDashboardSliderResponse>() { IsError = false, Message = "", Data = DashboardSliderService.GetDashboardSlider(languageId) });
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
        [ProducesResponseType(typeof(Response<CreateDashboardSliderRequest>), 200)]
        [ProducesResponseType(typeof(Response<CreateDashboardSliderRequest>), 403)]
        [HttpGet]
        [AdminAuthorize(ERight.Breed)]
        [Route("geteditDashboardSlider/{DashboardSliderId}")]
        public IActionResult GetEditDashboardSlider(Guid DashboardSliderId)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, new Response<CreateDashboardSliderRequest>() { IsError = false, Message = "", Data = DashboardSliderService.GetEditDashboardSlider(DashboardSliderId) });
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
        [AdminAuthorize(ERight.Breed)]
        [Route("saveDashboardSlider")]
        public async Task<IActionResult> SaveDashboardSlider([FromForm] CreateDashboardSliderRequest DashboardSliderRequest, IFormFile image)
        {
            try
            {
                FileUrlResponce file = new FileUrlResponce();
                DashboardSliderRequest.SliderDetails = JsonConvert.DeserializeObject<List<CreateDashboardSliderDetailsRequest>>(Request.Form["SliderDetails"]);
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                if (image != null)
                {
                    file = await new SaveFiles().SendMyFileToS3(image, configuration.GetValue<string>("Amazon:Bucket"), "Category" + SystemGlobal.GetSubDirectoryName(), false, configuration.GetValue<string>("Amazon:AccessKey"), configuration.GetValue<string>("Amazon:AccessSecret"), configuration.GetValue<string>("Amazon:BaseUrl"));
                    DashboardSliderRequest.Url = file.URL;
                    DashboardSliderRequest.ThubnilUrl = file.ThumbnailUrl;

                }

                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await DashboardSliderService.SaveDashboardSlider(DashboardSliderRequest, userId) });
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
        [AdminAuthorize(ERight.Breed)]
        [Route("controllDashboardSlideractivation/{DashboardSliderId}/{activation:bool}")]
        public async Task<IActionResult> ControllDashboardSliderActivation(Guid DashboardSliderId, bool activation)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await DashboardSliderService.ControllDashboardSliderActivation(DashboardSliderId, activation, userId) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
