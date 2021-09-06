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
    public class BlogController : ControllerBase
    {
        private readonly IBlogService blogService;
        private readonly IConfiguration configuration;
        public BlogController(IBlogService blogService, IConfiguration configuration)
        {
            this.blogService = blogService;
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
        [ProducesResponseType(typeof(Response<GetBlogResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetBlogResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AdminAuthorize(ERight.Cities)]
        [HttpPost]
        [Route("GetBlog")]
        public IActionResult GetBlog(ListGeneralModel page)
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
        /// </summary>
        /// <response code="200">If all working fine</response>
        /// <response code="500">If Server Error</response>  
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<CreateBlogRequest>), 200)]
        [ProducesResponseType(typeof(Response<CreateBlogRequest>), 403)]
        [AdminAuthorize(ERight.Cities)]
        [HttpGet]
        [Route("GetEditBlog/{id}")]
        public IActionResult GetEditBlog(Guid id)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<CreateBlogRequest>() { IsError = false, Message = "", Data = blogService.GetEditBlog(id, userId)});
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
        [Route("SaveBlog")]
        public async Task<IActionResult> SaveBlog([FromForm] CreateBlogRequest BlogRequest, IFormFile image)
        {
            try
            {
                FileUrlResponce file = new FileUrlResponce();
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                if (image != null && image.Length > 0)
                {
                    file = await new SaveFiles().SendMyFileToS3(image, configuration.GetValue<string>("Amazon:Bucket"), "Category" + SystemGlobal.GetSubDirectoryName(), false, configuration.GetValue<string>("Amazon:AccessKey"), configuration.GetValue<string>("Amazon:AccessSecret"), configuration.GetValue<string>("Amazon:BaseUrl"));

                    BlogRequest.HeaderImage = file.URL;
                    BlogRequest.ThumbnilUrl = file.ThumbnailUrl;

                }

                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await blogService.SaveBlog(BlogRequest, userId)});
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
        [Route("ControllBlogActivation/{BlogId}/{activation:bool}")]
        public async Task<IActionResult> ControllBlogActivation(Guid BlogId, bool activation)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await blogService.ControllBlogActivation(BlogId, activation, userId) });
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
        [Route("ControllBlogPublish/{BlogId}/{activation:bool}")]
        public async Task<IActionResult> ControllBlogPublish(Guid BlogId, bool activation)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await blogService.ControllBlogActivation(BlogId, activation, userId) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
