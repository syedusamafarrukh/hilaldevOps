using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hilal.Api.Models.Authorizations;
using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Enum;
using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Request.App.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using Hilal.DataViewModel.Response.App.v1;
using Hilal.Service.Interface.v1.Admin;
using Hilal.Service.Interface.v1.App;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Hilal.Api.Controllers.v1.App
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AppCommonController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly IBreedService breedService;
        private readonly IGenderService genderService;
        private readonly ISubscriptionService subscriptionService;
        private readonly ICitiesService citiesService;
        private readonly ICountriesService countriesService;
        private readonly IFeaturedAdvertisementService featuredAdvertisementService;
        private readonly IDashboardSliderService dashboardSliderService;
        private readonly IAgeService ageService;
        private readonly IChatStreamService chatStreamService;
        public AppCommonController(ISubscriptionService subscriptionService,IFeaturedAdvertisementService featuredAdvertisementService, ICategoryService categoryService, IBreedService breedService, IAgeService ageService, IGenderService genderService, ICitiesService citiesService, IDashboardSliderService dashboardSliderService, ICountriesService countriesService, IChatStreamService chatStreamService)
        {
            this.categoryService = categoryService;
            this.ageService = ageService;
            this.breedService = breedService;
            this.breedService = breedService;
            this.genderService = genderService;
            this.citiesService = citiesService;
            this.countriesService = countriesService;
            this.subscriptionService = subscriptionService;
            this.dashboardSliderService = dashboardSliderService;
            this.featuredAdvertisementService = featuredAdvertisementService;
            this.chatStreamService = chatStreamService;
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<List<GetCategory>>), 200)]
        [ProducesResponseType(typeof(Response<List<GetCategory>>), 403)]
        [HttpGet]
        [AppAuthorize(false)]
        [Route("GetCategoriesList")]
        public IActionResult GetCategoriesList(int CategoryType)
        {
            try
            {
                int? languageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                GetCategoriesRequest page = new GetCategoriesRequest
                {
                    CategoryId = null,
                    LanguageId = languageId == null ? (int)ELanguage.English : languageId,
                    FkCategoryType = CategoryType,
                    SubCategoryList = false
                };
                return StatusCode(StatusCodes.Status200OK, new Response<List<GetCategory>>() { IsError = false, Message = "", Data = categoryService.GetCategoriesList(page) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<List<GetCategory>>), 200)]
        [ProducesResponseType(typeof(Response<List<GetCategory>>), 403)]
        [HttpGet]
        [AppAuthorize(false)]
        [Route("GetSubCategoriesList")]
        public IActionResult GetSubCategoriesList(Guid CategoryId)
        {
            try
            {
                int? languageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                GetCategoriesRequest page = new GetCategoriesRequest
                {
                    CategoryId = CategoryId,
                    LanguageId = languageId == null ? (int) ELanguage.English : languageId,
                    FkCategoryType = null,
                    SubCategoryList = true
                };
                return StatusCode(StatusCodes.Status200OK, new Response<List<GetCategory>>() { IsError = false, Message = "", Data = categoryService.GetCategoriesList(page) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<List<GetCategory>>), 200)]
        [ProducesResponseType(typeof(Response<List<GetCategory>>), 403)]
        [HttpGet]
        [AppAuthorize(false)]
        [Route("GetSubCategoriesListMulti")]
        public IActionResult GetSubCategoriesListMulti(List<Guid> CategoryId)
        {
            try
            {
                int? languageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                GetSubCategoriesRequest page = new GetSubCategoriesRequest
                {
                    CategoryId = CategoryId,
                    LanguageId = languageId == null ? (int)ELanguage.English : languageId,
                    FkCategoryType = null,
                };
                return StatusCode(StatusCodes.Status200OK, new Response<List<GetCategory>>() { IsError = false, Message = "", Data = categoryService.GetSubCategoriesByCategoryList(page) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<List<GetBreedList>>), 200)]
        [ProducesResponseType(typeof(Response<List<GetBreedList>>), 403)]
        [HttpGet]
        [AppAuthorize(false)]
        [Route("GetBreedList")]
        public IActionResult GetBreedList(Guid? CategoryId, Guid? SubCategoryId)
        {
            try
            {
                int? languageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                GetBreedRequest page = new GetBreedRequest
                {
                    FK_CategoryId = CategoryId,
                    FK_SubCategoryId = SubCategoryId,
                    LanguageId = languageId == null ? (int)ELanguage.English : languageId,
                };
                return StatusCode(StatusCodes.Status200OK, new Response<List<GetBreedList>>() { IsError = false, Message = "", Data = breedService.GetBreedList(page) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<List<GetBreedList>>), 200)]
        [ProducesResponseType(typeof(Response<List<GetBreedList>>), 403)]
        [HttpPost]
        [AppAuthorize(false)]
        [Route("GetBreedListMulti")]
        public IActionResult GetBreedListMulti(GetBreedByCategoryRequest page)
        {
            try
            {
                int? languageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                //GetBreedByCategoryRequest page = new GetBreedByCategoryRequest
                //{
                //    FK_CategoryId = CategoryId,
                //    FK_SubCategoryId = SubCategoryId,
                //    LanguageId = languageId == null ? (int)ELanguage.English : languageId,
                //};
                page.LanguageId = languageId;
                return StatusCode(StatusCodes.Status200OK, new Response<List<GetBreedList>>() { IsError = false, Message = "", Data = breedService.GetBreedByCategoryList(page) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<List<GetAgesLsit>>), 200)]
        [ProducesResponseType(typeof(Response<List<GetAgesLsit>>), 403)]
        [HttpPost]
        [AppAuthorize(false)]
        [Route("GetAgeListMulti")]
        public IActionResult GetAgeListMulti(GetAgeByCategoryRequest page)
        {
            try
            {
                int? languageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                //GetAgeByCategoryRequest page = new GetAgeByCategoryRequest
                //{
                //    FK_CategoryId = CategoryId,
                //    FK_SubCategoryId = SubCategoryId,
                //    LanguageId = languageId == null ? (int)ELanguage.English : languageId,
                //};
                page.LanguageId = languageId;
                return StatusCode(StatusCodes.Status200OK, new Response<List<GetAgesLsit>>() { IsError = false, Message = "", Data = ageService.GetAgeByCategories(page) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<List<GetAgesLsit>>), 200)]
        [ProducesResponseType(typeof(Response<List<GetAgesLsit>>), 403)]
        [HttpGet]
        [AppAuthorize(false)]
        [Route("GetAgeList")]
        public IActionResult GetAgeList(Guid? CategoryId, Guid? SubCategoryId)
        {
            try
            {
                int? languageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                GetAgeRequest page = new GetAgeRequest
                {
                    FK_CategoryId = CategoryId,
                    FK_SubCategoryId = SubCategoryId,
                    LanguageId = languageId == null ? (int)ELanguage.English : languageId,
                };
                return StatusCode(StatusCodes.Status200OK, new Response<List<GetAgesLsit>>() { IsError = false, Message = "", Data = ageService.GetAgeList(page) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<List<GetGenderList>>), 200)]
        [ProducesResponseType(typeof(Response<List<GetGenderList>>), 403)]
        [HttpGet]
        [AppAuthorize(false)]
        [Route("GetGenderList")]
        public IActionResult GetGenderList()
        {
            try
            {
                int? languageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                GetGenderRequest page = new GetGenderRequest
                {
                    LanguageId = languageId == null ? (int)ELanguage.English : languageId,
                };
                return StatusCode(StatusCodes.Status200OK, new Response<List<GetGenderList>>() { IsError = false, Message = "", Data = genderService.GetGenderList(page) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<List<GetGenderList>>), 200)]
        [ProducesResponseType(typeof(Response<List<GetGenderList>>), 403)]
        [HttpPost]
        [AppAuthorize(false)]
        [Route("GetGenderListMulti")]
        public IActionResult GetGenderListMulti(GetGenderByCategoryRequest page)
        {
            try
            {
                int? languageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                //GetGenderByCategoryRequest page = new GetGenderByCategoryRequest
                //{
                //    CategoryId = CategoryId,
                //    FK_AgeId = FK_AgeId,
                //    LanguageId = languageId == null ? (int)ELanguage.English : languageId,
                //};
                page.LanguageId = languageId;
                return StatusCode(StatusCodes.Status200OK, new Response<List<GetGenderList>>() { IsError = false, Message = "", Data = genderService.GetGenderByCategory(page) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<List<GetGenderList>>), 200)]
        [ProducesResponseType(typeof(Response<List<GetGenderList>>), 403)]
        [HttpGet]
        [AppAuthorize(false)]
        [Route("GetCitiesList")]
        public IActionResult GetCitiesList(Guid? countryId)
        {
            try
            {
                int? languageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<List<GetCitiesList>>() { IsError = false, Message = "", Data = citiesService.GetCitiesList(countryId, (languageId == null ? 2 : languageId.Value)) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<List<GetGenderList>>), 200)]
        [ProducesResponseType(typeof(Response<List<GetGenderList>>), 403)]
        [HttpGet]
        [AppAuthorize(false)]
        [Route("GetCountriesList")]
        public IActionResult GetCountriesList()
        {
            try
            {
                int? languageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<List<GetCountries>>() { IsError = false, Message = "", Data = countriesService.GetCountriesList((languageId == null ? 2 : languageId.Value)) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<List<GetGetDashboardSliderResponseList>>), 200)]
        [ProducesResponseType(typeof(Response<List<GetGetDashboardSliderResponseList>>), 403)]
        [HttpGet]
        [AppAuthorize(false)]
        [Route("GetDashboardList")]
        public IActionResult GetDashboardList()
        {
            try
            {
                int? languageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<List<GetGetDashboardSliderResponseList>>() { IsError = false, Message = "", Data = dashboardSliderService.GetDashboardSliderList((languageId == null ? 2 : languageId.Value)) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Response<List<GetSubscriptionList>>), 200)]
        [ProducesResponseType(typeof(Response<List<GetSubscriptionList>>), 403)]
        [HttpGet]
        [AppAuthorize(false)]
        [Route("GetSubscriptionList")]
        public IActionResult GetSubscriptionList()
        {
            try
            {
                int? languageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<List<GetSubscriptionList>>() { IsError = false, Message = "", Data = subscriptionService.GetSubscriptionList((languageId == null ? 2 : languageId.Value)) });
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
        [ProducesResponseType(typeof(Response<List<GetAdvertisementList>>), 200)]
        [ProducesResponseType(typeof(Response<List<GetAdvertisementList>>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AppAuthorize(false)]
        [HttpGet]
        [Route("GetFeaturedAdvertisementList")]
        public IActionResult GetFeaturedAdvertisementList()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                var LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<List<GetAdvertisementList>>() { IsError = false, Message = "", Data = featuredAdvertisementService.GetFeaturedAdvertisementList(LanguageId) });
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
        [ProducesResponseType(typeof(Response<GetNotificationsResponse>), 200)]
        [ProducesResponseType(typeof(Response<GetNotificationsResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AppAuthorize(true)]
        [HttpPost]
        [Route("getNotifications")]
        public IActionResult getNotifications(GetNotificationRequest page)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                page.LanguageId = Convert.ToInt32(RouteData.Values["Language"].ToString());
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                page.UserId = userId;
                return StatusCode(StatusCodes.Status200OK, new Response<GetNotificationsResponse>() { IsError = false, Message = "", Data = chatStreamService.getNotifications(page) });
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
        [ProducesResponseType(typeof(Response<DashBoardCountResponse>), 200)]
        [ProducesResponseType(typeof(Response<DashBoardCountResponse>), 403)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AppAuthorize(true)]
        [HttpGet]
        [Route("getDashBoardCount")]
        public IActionResult getDashBoardCount()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<DashBoardCountResponse>() { IsError = false, Message = "", Data = chatStreamService.getDashBoardCount(userId) });
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
        [Route("CreateSubscrptionOrder")]
        public IActionResult CreateSubscrptionOrder(CreatePaymentOrderRequest createPaymentOrderRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<Task<string>>() { IsError = false, Message = "", Data = subscriptionService.CreateOrderRequest(createPaymentOrderRequest , userId) });
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
        [ProducesResponseType(typeof(Response<Task<bool>>), 403)]
        [ProducesResponseType(typeof(Response<Task<bool>>), 200)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AppAuthorize(true)]
        [HttpGet]
        [Route("CheckSubscrptionPaymentStatus/{PlanId}/{OrderRef}")]
        public IActionResult CheckSubscrptionPaymentStatus(Guid PlanId , string OrderRef)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<Task<bool>>() { IsError = false, Message = "", Data = subscriptionService.createSubscrption(OrderRef,PlanId, userId) });
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
        [ProducesResponseType(typeof(Response<bool>), 403)]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [ProducesResponseType(typeof(string[]), 400)]
        [AppAuthorize(true)]
        [HttpGet]
        [Route("CheckSubscrption")]
        public IActionResult CheckSubscrption()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(v => v.Errors.Select(z => z.ErrorMessage)));
                }
                var userId = Guid.Parse(RouteData.Values["userId"].ToString());
                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { IsError = false, Message = "", Data = subscriptionService.checkSubscription(userId) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
