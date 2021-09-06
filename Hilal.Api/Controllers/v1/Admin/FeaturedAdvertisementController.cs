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
using Hilal.DataViewModel.Response.App.v1;
using Hilal.DataViewModel.Request.App.v1;

namespace Hilal.Api.Controllers.v1.Admin
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class FeaturedAdvertisementController : ControllerBase
    {
        private readonly IFeaturedAdvertisementService featuredAdvertisementService;
        private readonly IConfiguration configuration;
        public FeaturedAdvertisementController(IFeaturedAdvertisementService featuredAdvertisementService, IConfiguration configuration)
        {
            this.featuredAdvertisementService = featuredAdvertisementService;
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
        [ProducesResponseType(typeof(Response<GetFeaturedAdvertisementResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetFeaturedAdvertisementResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AdminAuthorize(ERight.Category)]
        [HttpPost]
        [Route("GetFeaturedAdvertisement")]
        public IActionResult GetFeaturedAdvertisement(ListGeneralModel page)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                return StatusCode(StatusCodes.Status200OK, new Response<GetFeaturedAdvertisementResponse>() { IsError = false, Message = "", Data = featuredAdvertisementService.GetFeaturedAdvertisement(page) });
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
        [ProducesResponseType(typeof(Response<CreateFeaturedAdvertisementRequest>), 200)]
        [ProducesResponseType(typeof(Response<CreateFeaturedAdvertisementRequest>), 403)]
        [AdminAuthorize(ERight.Category)]
        [HttpGet]
        [Route("GetEditFeaturedAdvertisement/{FeaturedAdvertisementId}")]
        public IActionResult GetEditFeaturedAdvertisement(Guid FeaturedAdvertisementId)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<CreateFeaturedAdvertisementRequest>() { IsError = false, Message = "", Data = featuredAdvertisementService.GetEditFeaturedAdvertisement(FeaturedAdvertisementId) });
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
        [AdminAuthorize(ERight.Category)]
        [Route("SaveFeaturedAdvertisement")]
        public async Task<IActionResult> SaveFeaturedAdvertisement([FromForm] CreateFeaturedAdvertisementRequest FeaturedAdvertisementRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await featuredAdvertisementService.SaveFeaturedAdvertisement(FeaturedAdvertisementRequest, userId) });
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
        [HttpGet]
        [Route("ControllFeaturedAdvertisementActivation/{FeaturedAdvertisementId}/{activation:bool}")]
        public async Task<IActionResult> ControllFeaturedAdvertisementActivation(Guid FeaturedAdvertisementId, bool activation)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await featuredAdvertisementService.ControllFeaturedAdvertisementActivation(FeaturedAdvertisementId, activation, userId)});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
