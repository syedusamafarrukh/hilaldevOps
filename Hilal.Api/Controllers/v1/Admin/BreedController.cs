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
    public class BreedController : ControllerBase
    {
        private readonly IBreedService BreedService;
        private readonly IConfiguration configuration;
        public BreedController(IBreedService BreedService, IConfiguration configuration)
        {
            this.BreedService = BreedService;
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
        [ProducesResponseType(typeof(Response<GetBreedResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetBreedResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AdminAuthorize(ERight.Breed)]
        [HttpPost]
        [Route("getBreed")]
        public IActionResult GetBreed(GetBreedRequest page)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                return StatusCode(StatusCodes.Status200OK, new Response<GetBreedResponse>() { IsError = false, Message = "", Data = BreedService.GetBreed(page) });
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
        [ProducesResponseType(typeof(Response<CreateBreedRequest>), 200)]
        [ProducesResponseType(typeof(Response<CreateBreedRequest>), 403)]
        [AdminAuthorize(ERight.Breed)]
        [HttpGet]
        [Route("geteditBreed/{BreedId}")]
        public IActionResult GetEditBreed(Guid BreedId)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<CreateBreedRequest>() { IsError = false, Message = "", Data = BreedService.GetEditBreed(BreedId, userId) });
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
        [Route("saveBreed")]
        public async Task<IActionResult> SaveBreed([FromForm] CreateBreedRequest BreedRequest, IFormFile image)
        {
            try
            {
                FileUrlResponce file = new FileUrlResponce();
              //  BreedRequest.Image = JsonConvert.DeserializeObject<FileUrlResponce>(Request.Form["image"]);
                BreedRequest.BreedInformations = JsonConvert.DeserializeObject<List<GenericDetailRequest>>(Request.Form["BreedInformations"]);
                BreedRequest.subcategoryList = JsonConvert.DeserializeObject<List<General<Guid>>>(Request.Form["subcategoryList"]);

                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                if (image != null && image.Length > 0)
                {
                    file = await new SaveFiles().SendMyFileToS3(image, configuration.GetValue<string>("Amazon:Bucket"), "Category" + SystemGlobal.GetSubDirectoryName(), false, configuration.GetValue<string>("Amazon:AccessKey"), configuration.GetValue<string>("Amazon:AccessSecret"), configuration.GetValue<string>("Amazon:BaseUrl"));

                    BreedRequest.Image = file;
                }
                

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await BreedService.SaveBreed(BreedRequest, userId) });
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
        [AdminAuthorize(ERight.Breed)]
        [HttpGet]
        [Route("controllBreedactivation/{BreedId}/{activation:bool}")]
        public async Task<IActionResult> ControllBreedActivation(Guid BreedId, bool activation)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await BreedService.ControllBreedActivation(BreedId, activation, userId) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///// <summary>
        ///// SortBy = desc or asc
        ///// case 0: Name case 1: Priority case 2: x.IsActive
        ///// </summary>
        ///// <response code="200">If all working fine</response>
        ///// <response code="400">If client made Validation Error</response>  
        ///// <response code="403">If client made some mistake</response>  
        ///// <response code="500">If Server Error</response>  
        //[ProducesResponseType(200)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(403)]
        //[ProducesResponseType(500)]
        //[ProducesResponseType(typeof(Response<GetBreedResponse>), 200)]
        //[ProducesResponseType(typeof(Response<GetBreedResponse>), 403)]
        //[ProducesResponseType(typeof(string[]), 400)]
        //[AdminAuthorize(ERight.Breed)]
        //[HttpPost]
        //[Route("getBreedList")]
        //public IActionResult getBreedList(GetBreedRequest page)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
        //        }

        //        return StatusCode(StatusCodes.Status200OK, new Response<GetBreedResponse>() { IsError = false, Message = "", Data = BreedService.GetBreedList(page) });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
