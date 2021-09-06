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
    public class CitiesController : ControllerBase
    {
        private readonly ICitiesService CitiesService;
        private readonly IConfiguration configuration;
        public CitiesController(ICitiesService CitiesService, IConfiguration configuration)
        {
            this.CitiesService = CitiesService;
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
        [ProducesResponseType(typeof(Response<GetCitiesResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetCitiesResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AdminAuthorize(ERight.Cities)]
        [HttpPost]
        [Route("getCities")]
        public IActionResult GetCities(GetCityRequest page)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                return StatusCode(StatusCodes.Status200OK, new Response<GetCitiesResponse>() { IsError = false, Message = "", Data = CitiesService.GetCities(page) });
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
        [ProducesResponseType(typeof(Response<CreateCitiesRequest>), 200)]
        [ProducesResponseType(typeof(Response<CreateCitiesRequest>), 403)]
        [AdminAuthorize(ERight.Cities)]
        [HttpGet]
        [Route("geteditCities/{CitiesId}")]
        public IActionResult GetEditCities(Guid CitiesId)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<CreateCitiesRequest>() { IsError = false, Message = "", Data = CitiesService.GetEditCities(CitiesId, userId) });
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
        [AdminAuthorize(ERight.Cities)]
        [Route("saveCities")]
        public async Task<IActionResult> SaveCities([FromForm] CreateCitiesRequest CitiesRequest)
        {
            try
            {
                FileUrlResponce file = new FileUrlResponce();
                CitiesRequest.CitiesInformations = JsonConvert.DeserializeObject<List<GenericDetailRequest>>(Request.Form["CitiesInformations"]);

                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await CitiesService.SaveCities(CitiesRequest, userId) });
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
        [AdminAuthorize(ERight.Cities)]
        [Route("SaveCitiesbyExcel")]
        public async Task<IActionResult> SaveCitiesbyExcel([FromForm] CreateCitiesRequest CitiesRequest, IFormFile file)
        {
            try
            {
                List<CreateCitiesRequest> cityList = new List<CreateCitiesRequest>();
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                cityList = ExcelWorkSheet.ImportExcelGetCities(file, CitiesRequest.FkCountry);

                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                foreach (var item in cityList)
                {
                   var booll = await CitiesService.SaveCities(item, userId);
                }
                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = true });
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
        [AdminAuthorize(ERight.Cities)]
        [HttpGet]
        [Route("controllCitiesactivation/{CitiesId}/{activation:bool}")]
        public async Task<IActionResult> ControllCitiesActivation(Guid CitiesId, bool activation)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await CitiesService.ControllCitiesActivation(CitiesId, activation, userId) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
