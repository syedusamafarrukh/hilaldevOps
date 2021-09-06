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

namespace Hilal.Api.Controllers.v1.Admin
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AgeController : ControllerBase
    {
        private readonly IAgeService AgeService;
        private readonly IConfiguration configuration;
        public AgeController(IAgeService AgeService, IConfiguration configuration)
        {
            this.AgeService = AgeService;
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
        [ProducesResponseType(typeof(Response<GetAgeResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetAgeResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AdminAuthorize(ERight.Age)]
        [HttpPost]
        [Route("getAge")]
        public IActionResult GetAge(GetAgeRequest page)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                return StatusCode(StatusCodes.Status200OK, new Response<GetAgeResponse>() { IsError = false, Message = "", Data = AgeService.GetAge(page) });
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
        [ProducesResponseType(typeof(Response<CreateAgeRequest>), 200)]
        [ProducesResponseType(typeof(Response<CreateAgeRequest>), 403)]
        [AdminAuthorize(ERight.Age)]
        [HttpGet]
        [Route("geteditAge/{AgeId}")]
        public IActionResult GetEditAge(Guid AgeId)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<CreateAgeRequest>() { IsError = false, Message = "", Data = AgeService.GetEditAge(AgeId, userId) });
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
        [AdminAuthorize(ERight.Age)]
        [Route("saveAge")]
        public async Task<IActionResult> SaveAge([FromForm] CreateAgeRequest AgeRequest)
        {
            try
            {
                FileUrlResponce file = new FileUrlResponce();
                AgeRequest.AgeInformations = JsonConvert.DeserializeObject<List<GenericDetailRequest>>(Request.Form["AgeInformations"]);
                AgeRequest.subcategoryList = JsonConvert.DeserializeObject<List<General<Guid>>>(Request.Form["subcategoryList"]);

                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await AgeService.SaveAge(AgeRequest, userId) });
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
        [AdminAuthorize(ERight.Age)]
        [HttpGet]
        [Route("controllAgeactivation/{AgeId}/{activation:bool}")]
        public async Task<IActionResult> ControllAgeActivation(Guid AgeId, bool activation)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await AgeService.ControllAgeActivation(AgeId, activation, userId) });
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
        //[ProducesResponseType(200)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(403)]
        //[ProducesResponseType(500)]
        //[ProducesResponseType(typeof(Response<GetAgeResponse>), 200)]
        //[ProducesResponseType(typeof(Response<GetAgeResponse>), 403)]
        //[ProducesResponseType(typeof(string[]), 400)]
        //[AdminAuthorize(ERight.Age)]
        //[HttpPost]
        //[Route("GetAgeList")]
        //public IActionResult GetAgeList(GetAgeRequest page)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
        //        }

        //        return StatusCode(StatusCodes.Status200OK, new Response<GetAgeResponse>() { IsError = false, Message = "", Data = AgeService.GetAgeList(page) });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
