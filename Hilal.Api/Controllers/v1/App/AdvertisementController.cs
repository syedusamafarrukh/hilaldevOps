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
using Hilal.Api.Models;
using Microsoft.AspNetCore.Hosting;

namespace Hilal.Api.Controllers.v1.App
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementService advertisementService;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment _env;
        public AdvertisementController(IAdvertisementService advertisementService, IConfiguration configuration, IWebHostEnvironment _env)
        {
            this.advertisementService = advertisementService;
            this.configuration = configuration;
            this._env = _env;
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
        [ProducesResponseType(typeof(Response<GetAppAdvertisementResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetAppAdvertisementResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AppAuthorize(false)]
        [HttpPost]
        [Route("GetAdvertisement")]
        public async Task<IActionResult> GetAdvertisement(GetAdvertisementRequest page)
        {
            try
            {
                //NetworkGlobal.SinglePaymentMethod();
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                page.LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                Guid? userId = Guid.Parse(RouteData.Values["userId"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<GetAppAdvertisementResponse>() { IsError = false, Message = "", Data = await advertisementService.GetAdvertisementService(page, userId,2) });
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
        [ProducesResponseType(typeof(Response<GetAppAdvertisementResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetAppAdvertisementResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AppAuthorize(false)]
        [HttpPost]
        [Route("GetFiltersAdvertisements")]
        public IActionResult GetFiltersAdvertisements(GetFiltersResquest page)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                page.LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<GetAppAdvertisementResponse>() { IsError = false, Message = "", Data = advertisementService.GetFiltersAdvertisements(page) });
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
        [AppAuthorize(false)]
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
                var LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                Guid? userId = Guid.Parse(RouteData.Values["userId"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<GetAdvertisemetDetail>() { IsError = false, Message = "", Data = advertisementService.GetAdvertisementDetail(Id, LanguageId, userId) });
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
        //[ProducesResponseType(typeof(Response<CreateAdvertisementRequest>), 200)]
        //[ProducesResponseType(typeof(Response<CreateAdvertisementRequest>), 403)]
        //[AppAuthorize(true)]
        //[HttpGet]
        //[Route("GetEditAdvertisement/{AdvertisementId}")]
        //public IActionResult GetEditAdvertisement(Guid AdvertisementId)
        //{
        //    try
        //    {
        //        return StatusCode(StatusCodes.Status200OK, new Response<CreateAdvertisementRequest>() { IsError = false, Message = "", Data = advertisementService.GetEditAdvertisement(AdvertisementId) });
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
        [ProducesResponseType(typeof(Response<getEditAdvertisementRequest>), 200)]
        [ProducesResponseType(typeof(Response<getEditAdvertisementRequest>), 403)]
        [AppAuthorize(true)]
        [HttpGet]
        [Route("GetEditAdvertisement/{AdvertisementId}")]
        public IActionResult GetEditAdvertisement(Guid AdvertisementId)
        {
            try
            {
                var LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<getEditAdvertisementRequest>() { IsError = false, Message = "", Data = advertisementService.GetEditAdvertisementforAPP(AdvertisementId, LanguageId)});
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
        [Route("SaveAdvertisement")]
        public async Task<IActionResult> SaveAdvertisement([FromForm] CreateAdvertisementRequest advertisementRequest, IFormFile Videos, IFormFile VideosThunbnil, List<IFormFile> Attachements)
        {
            try
            {
                FileUrlResponce file = new FileUrlResponce();
                FileUrlResponce fileee = new FileUrlResponce();
                FileUrlResponce filee = new FileUrlResponce();
                FileUrlResponce localFile = new FileUrlResponce();
                advertisementRequest.AttachementList = new List<CreateAttachementsRequest>();
                advertisementRequest.AdvertisementDetailsInformation = JsonConvert.DeserializeObject<List<CreateAdvertisementDetails>>(Request.Form["AdvertisementDetailsInformation"]);
                
                if (advertisementRequest.Id != null)
                {
                    if (Request.Form.Keys.Contains("AttachementList"))
                    {
                        advertisementRequest.AttachementList = JsonConvert.DeserializeObject<List<CreateAttachementsRequest>>(Request.Form["AttachementList"]);
                    }
                }
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                advertisementRequest.FK_AppUserId = userId;
                if (Videos != null)
                {
                    file = await new SaveFiles().SendMyFileToS3(Videos, configuration.GetValue<string>("Amazon:Bucket"), "Advertisement" + SystemGlobal.GetSubDirectoryName(), true, configuration.GetValue<string>("Amazon:AccessKey"), configuration.GetValue<string>("Amazon:AccessSecret"), configuration.GetValue<string>("Amazon:BaseUrl"));
                    advertisementRequest.Video = file.URL;

                    if (VideosThunbnil != null)
                    {
                        filee = await new SaveFiles().SendMyFileToS3(VideosThunbnil, configuration.GetValue<string>("Amazon:Bucket"), "Advertisement" + SystemGlobal.GetSubDirectoryName(), false, configuration.GetValue<string>("Amazon:AccessKey"), configuration.GetValue<string>("Amazon:AccessSecret"), configuration.GetValue<string>("Amazon:BaseUrl"));
                        advertisementRequest.VideoThumbnil = filee.ThumbnailUrl;
                        localFile = SaveFiles.SaveLocalImage(VideosThunbnil);
                        fileee = await Models.CommonFunctions.GetWaterMarkImage(localFile.URL,_env);
                        advertisementRequest.WaterMarkImage = fileee.URL;
                    }

                    CreateAttachementsRequest AttachementObject = new CreateAttachementsRequest
                    {
                        Id = 0,
                        Url = file.URL,
                        ThubnilUrl = advertisementRequest.VideoThumbnil,
                        //ThubnilUrl = !String.IsNullOrEmpty(thubnil) ? thubnil : file.ThumbnailUrl,
                        IsVideo = true,
                        WaterMarkImage = fileee.URL,
                    };
                    if (advertisementRequest.AttachementList == null)
                    {
                        advertisementRequest.AttachementList = new List<CreateAttachementsRequest>();
                    }
                    advertisementRequest.AttachementList.Add(AttachementObject);
                    if (System.IO.File.Exists(localFile.URL))
                    {
                        System.IO.File.Delete(localFile.URL);
                    }
                }

                if (Attachements != null && Attachements.Count > 0)
                {
                    foreach (var item in Attachements)
                    {
                        file = await new SaveFiles().SendMyFileToS3(item, configuration.GetValue<string>("Amazon:Bucket"), "Advertisement" + SystemGlobal.GetSubDirectoryName(), false, configuration.GetValue<string>("Amazon:AccessKey"), configuration.GetValue<string>("Amazon:AccessSecret"), configuration.GetValue<string>("Amazon:BaseUrl"));
                        localFile = SaveFiles.SaveLocalImage(item);
                        //throw new Exception(localFile.URL);
                        fileee = await Models.CommonFunctions.GetWaterMarkImage(localFile.URL, _env);
                        if (System.IO.File.Exists(localFile.URL))
                        {
                            System.IO.File.Delete(localFile.URL);
                        }
                        CreateAttachementsRequest AttachementObject = new CreateAttachementsRequest
                        {
                            Url = file.URL,
                            ThubnilUrl = file.ThumbnailUrl,
                            IsVideo = false,
                            WaterMarkImage = fileee.URL,
                        };
                        if (advertisementRequest.AttachementList == null)
                        {
                            advertisementRequest.AttachementList = new List<CreateAttachementsRequest>();
                        }
                        advertisementRequest.AttachementList.Add(AttachementObject);
                    }
                }


                return StatusCode(StatusCodes.Status200OK, new Response<Guid>() { IsError = false, Message = "", Data = await advertisementService.SaveAdvertisement(advertisementRequest, userId) });
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
        [Route("CreateAdvertisementCommission")]
        public async Task<IActionResult> CreateAdvertisementCommission([FromForm] CreateCommisionPaymentRequest CreateAdvertisementCommission)
        {
            try
            {
                FileUrlResponce file = new FileUrlResponce();

                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await advertisementService.CreateAdvertisementCommission(CreateAdvertisementCommission, userId) });
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
        [Route("ControllAdvertisementActivation/{AdvertisementId}/{activation:bool}")]
        public async Task<IActionResult> ControllAdvertisementActivation(Guid AdvertisementId, bool activation)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await advertisementService.ControllAdvertisementActivation(AdvertisementId, activation, userId) });
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
        [AppAuthorize(true)]
        [Route("ControllAdvertisementStatus/{AdvertisementId}/{StatusId:int}")]
        public async Task<IActionResult> ControllAdvertisementStatus(Guid AdvertisementId, int StatusId)
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await advertisementService.ControllAdvertisementStatusfromApp(AdvertisementId, StatusId, userId) });
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
        [ProducesResponseType(typeof(Response<GetAppAdvertisementResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetAppAdvertisementResponse>), 403)]
        [HttpGet]
        [Route("GetAdvertisementServicePList")]
        public async Task<IActionResult> GetAdvertisementServicePList(GetAdvertisementRequest advertisementRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                advertisementRequest.LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                Guid? userId = Guid.Parse(RouteData.Values["userId"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<GetAppAdvertisementResponse>() { IsError = false, Message = "", Data = await advertisementService.GetAdvertisementService(advertisementRequest, userId,2) });

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
        [Route("SaveBookmarksAdvertisement")]
        public async Task<IActionResult> SaveBookmarksAdvertisement(CreateBookMarksAdvertisementRequest advertisementRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                advertisementRequest.FK_AppUserId = userId;
                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await advertisementService.SaveBookMarksAdvertisement(advertisementRequest, userId) });
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
        [ProducesResponseType(typeof(Response<List<GetAdvertisementList>>), 200)]
        [ProducesResponseType(typeof(Response<List<GetAdvertisementList>>), 403)]
        [AppAuthorize(true)]
        [HttpGet]
        [Route("GetBookmarksAdvertisementByUserId")]
        public IActionResult GetBookmarksAdvertisementByUserId()
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                var LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<List<GetAdvertisementList>>() { IsError = false, Message = "", Data = advertisementService.GetBookmarksAdvertisementByUserId(LanguageId, userId) });
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
        [Route("AddUserSubscription")]
        public async Task<IActionResult> AddUserSubscription(CreateUserSubscription CreateUserSubscription)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }

                var userId = Guid.Parse(RouteData.Values["userId"].ToString());

                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = await advertisementService.AddUserSubscription(CreateUserSubscription, userId) });
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
        [ProducesResponseType(typeof(Response<GetSubscriptionList>), 200)]
        [ProducesResponseType(typeof(Response<GetSubscriptionList>), 403)]
        [AppAuthorize(true)]
        [HttpGet]
        [Route("GetUserSubscribedDetail")]
        public IActionResult GetUserSubscribedDetail()
        {
            try
            {
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                var LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<GetSubscriptionList>() { IsError = false, Message = "", Data = advertisementService.GetUserSubscribedDetail(userId, LanguageId) });
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
        [ProducesResponseType(typeof(Response<Task<string>>), 403)]
        [ProducesResponseType(typeof(Response<Task<string>>), 200)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AppAuthorize(true)]
        [HttpPost]
        [Route("CreateCommissionOrder")]
        public IActionResult CreateCommissionOrder(CreateAdCommisionRequest createPaymentOrderRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<Task<string>>() { IsError = false, Message = "", Data = advertisementService.CreateOrderRequest(createPaymentOrderRequest, userId) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
