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
using Hilal.DataViewModel.Response.App;

namespace Hilal.Api.Controllers.v1.App
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ChatStreamController : ControllerBase
    {
        private readonly IChatStreamService chatStreamService;
        private readonly IConfiguration configuration;
        public ChatStreamController(IChatStreamService chatStreamService, IConfiguration configuration)
        {
            this.chatStreamService = chatStreamService;
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
        [ProducesResponseType(typeof(Response<GetChatStreamResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetChatStreamResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AppAuthorize(true)]
        [HttpPost]
        [Route("GetChatStream")]
        public IActionResult GetChatStream(GetChatStreamRequest page)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                page.LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                page.UserId = userId;
                return StatusCode(StatusCodes.Status200OK, new Response<GetChatStreamResponse>() { IsError = false, Message = "", Data = chatStreamService.GetChatStream(page) });
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
        [ProducesResponseType(typeof(Response<GetChatThreadResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetChatThreadResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AppAuthorize(true)]
        [HttpPost]
        [Route("GetChatThread")]
        public IActionResult GetChatThread(GetChatStreamRequest page)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                page.LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<GetChatThreadResponse>() { IsError = false, Message = "", Data = chatStreamService.GetChatThread(page) });
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
        [ProducesResponseType(typeof(Response<Guid>), 200)]
        [ProducesResponseType(typeof(Response<Guid>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AppAuthorize(true)]
        [Route("SaveChatStream")]
        public async Task<IActionResult> SaveChatStream(CreateChatStream createChatStream)
        {
            try
            {
                FileUrlResponce file = new FileUrlResponce();
                
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                var LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<Guid>() { IsError = false, Message = "", Data = await chatStreamService.SaveChatStream(createChatStream, userId, LanguageId) });
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
        [ProducesResponseType(typeof(Response<WhtsappInfoViewModel>), 200)]
        [ProducesResponseType(typeof(Response<WhtsappInfoViewModel>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AppAuthorize(false)]
        [HttpGet]
        [Route("GetWhatsAppInfo")]
        public IActionResult GetWhatsAppInfo()
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                return StatusCode(StatusCodes.Status200OK, new Response<WhtsappInfoViewModel>() { IsError = false, Message = "", Data = chatStreamService.getWhtsapInfo() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
