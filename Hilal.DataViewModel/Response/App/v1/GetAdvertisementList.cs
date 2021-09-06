using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Request;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.App.v1
{
    public class GetAppAdvertisementResponse : GeneralGetList
    {
        [JsonProperty(PropertyName = "AdvertisementList")]
        public List<GetAdvertisementList> AdvertisementList { get; set; }
    }

    public class GetAdvertisementList 
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "SalePrice")]
        public decimal? SalePrice { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "PhoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty(PropertyName = "WhatsappNumber")]
        public string WhatsappNumber { get; set; }
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "Country")]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "City")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "CountryId")]
        public Guid? CountryId { get; set; }
        [JsonProperty(PropertyName = "CityId")]
        public Guid CityId { get; set; }
        [JsonProperty(PropertyName = "Age")]
        public string Age { get; set; }
        [JsonProperty(PropertyName = "Gender")]
        public string Gender { get; set; }
        [JsonProperty(PropertyName = "Category")]
        public string Category { get; set; }
        [JsonProperty(PropertyName = "SubCategory")]
        public string SubCategory { get; set; }
        [JsonProperty(PropertyName = "Breed")]
        public string Breed { get; set; }
        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty(PropertyName = "ThreadId")]
        public Guid? ThreadId { get; set; }
        [JsonProperty(PropertyName = "FK_StatusId")]
        public int FK_StatusId { get; set; }
        [JsonProperty(PropertyName = "IsBookMark")]
        public bool IsBookMark { get; set; }
        [JsonProperty(PropertyName = "IsVideo")]
        public bool IsVideo { get; set; }
        [JsonProperty(PropertyName = "image")]
        public FileUrlResponce Image { get; set; } = new FileUrlResponce();
        [JsonProperty(PropertyName = "SellerId")]
        public Guid SellerId { get; set; }
        [JsonProperty(PropertyName = "SellerName")]
        public string SellerName { get; set; }
        [JsonProperty(PropertyName = "UserName")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "Comments")]
        public string Comments { get; set; }
        [JsonProperty(PropertyName = "RefId")]
        public string RefId { get; set; }
    }

    public class GetAdvertisementRes
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "SalePrice")]
        public decimal? SalePrice { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "PhoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty(PropertyName = "WhatsappNumber")]
        public string WhatsappNumber { get; set; }
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "Country")]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "City")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "CountryId")]
        public Guid? CountryId { get; set; }
        [JsonProperty(PropertyName = "CityId")]
        public Guid CityId { get; set; }
        [JsonProperty(PropertyName = "Age")]
        public string Age { get; set; }
        [JsonProperty(PropertyName = "Gender")]
        public string Gender { get; set; }
        [JsonProperty(PropertyName = "Category")]
        public string Category { get; set; }
        [JsonProperty(PropertyName = "SubCategory")]
        public string SubCategory { get; set; }
        [JsonProperty(PropertyName = "Breed")]
        public string Breed { get; set; }
        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty(PropertyName = "ThreadId")]
        public Guid? ThreadId { get; set; }
        [JsonProperty(PropertyName = "FK_StatusId")]
        public int FK_StatusId { get; set; }
        [JsonProperty(PropertyName = "IsBookMark")]
        public bool IsBookMark { get; set; }
        [JsonProperty(PropertyName = "IsVideo")]
        public bool IsVideo { get; set; }
        [JsonProperty(PropertyName = "Url")]
        public string Url { get; set; } = "";
        [JsonProperty(PropertyName = "ThumbnilUrl")]
        public string ThumbnilUrl { get; set; } = "";
        [JsonProperty(PropertyName = "WaterMarkImage")]
        public string WaterMarkImage { get; set; } = "";
        [JsonProperty(PropertyName = "SellerId")]
        public Guid SellerId { get; set; }
        [JsonProperty(PropertyName = "SellerName")]
        public string SellerName { get; set; }
        [JsonProperty(PropertyName = "UserName")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "Comments")]
        public string Comments { get; set; }
        [JsonProperty(PropertyName = "RefId")]
        public string RefId { get; set; }
    }

    public class GetAdvertisemetDetail
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "SalePrice")]
        public decimal? SalePrice { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "PhoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty(PropertyName = "Category")]
        public string Category { get; set; }
        [JsonProperty(PropertyName = "SubCategory")]
        public string SubCategory { get; set; }
        [JsonProperty(PropertyName = "WhatsappNumber")]
        public string WhatsappNumber { get; set; }
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "Country")]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "City")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "CountryId")]
        public Guid? CountryId { get; set; }
        [JsonProperty(PropertyName = "CityId")]
        public Guid CityId { get; set; }
        [JsonProperty(PropertyName = "Age")]
        public string Age { get; set; }
        [JsonProperty(PropertyName = "Gender")]
        public string Gender { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "SellerName")]
        public string SellerName { get; set; }
        [JsonProperty(PropertyName = "UserName")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "CommissionAmount")]
        public decimal? CommissionAmount { get; set; }
        [JsonProperty(PropertyName = "Father")]
        public string Father { get; set; }
        [JsonProperty(PropertyName = "Mother")]
        public string Mother { get; set; }
        [JsonProperty(PropertyName = "Breed")]
        public string Breed { get; set; }
        [JsonProperty(PropertyName = "IsDeliveryAvailable")]
        public bool IsDeliveryAvailable { get; set; }
        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty(PropertyName = "FK_StatusId")]
        public int FK_StatusId { get; set; }
        [JsonProperty(PropertyName = "IsBookMark")]
        public bool IsBookMark { get; set; }
        [JsonProperty(PropertyName = "ThreadId")]
        public Guid? ThreadId { get; set; }
        [JsonProperty(PropertyName = "Attachements")]
        public List<GetAttachementsResponseList> Attachements { get; set; } = new List<GetAttachementsResponseList>();
        [JsonProperty(PropertyName = "SellerId")]
        public Guid SellerId { get; set; }
        [JsonProperty(PropertyName = "RefId")]
        public string RefId { get; set; }
    }
}
