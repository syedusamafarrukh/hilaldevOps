using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Request;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.App.v1
{
    public class GetUserBusinessProfileResponse : GeneralGetList
    {
        [JsonProperty(PropertyName = "ItemList")]
        public List<GetBusinessProfile> ItemList { get; set; }

    }

    public class GetBusinessProfile
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "FK_CategoryId")]
        public General<Guid> FK_CategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_SubCategoryId")]
        public General<Guid?> FK_SubCategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_AppUserId")]
        public General<Guid> FK_AppUserId { get; set; }
        [JsonProperty(PropertyName = "LogoIcon")]
        public string LogoIcon { get; set; }
        [JsonProperty(PropertyName = "ContactNumber")]
        public string ContactNumber { get; set; }
        [JsonProperty(PropertyName = "WhatsAppNumber")]
        public string WhatsAppNumber { get; set; }
        [JsonProperty(PropertyName = "EmailId")]
        public string EmailId { get; set; }
        [JsonProperty(PropertyName = "Website")]
        public string Website { get; set; }
        [JsonProperty(PropertyName = "FK_CityId")]
        public General<Guid> FK_CityId { get; set; }
        [JsonProperty(PropertyName = "FK_CountryId")]
        public General<Guid?> FK_CountryId { get; set; }
        [JsonProperty(PropertyName = "FK_StatusId")]
        public int FK_StatusId { get; set; }
        [JsonProperty(PropertyName = "IsDeliveryAvailable")]
        public bool IsDeliveryAvailable { get; set; }
        [JsonProperty(PropertyName = "IsSelfPickup")]
        public bool IsSelfPickup { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "UserName")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "SellerName")]
        public string SellerName { get; set; }
        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "Comments")]
        public string Comments { get; set; }
        [JsonProperty(PropertyName = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }
        [JsonProperty(PropertyName = "ThreadId")]
        public Guid? ThreadId { get; set; }
        [JsonProperty(PropertyName = "SellerId")]
        public Guid SellerId { get; set; }
        [JsonProperty(PropertyName = "AttachementList")]
        public List<GetAttachementsResponseList> AttachementList { get; set; }
    }

    public class GetUserBusinessProfileListResponse : GeneralGetList
    {
        [JsonProperty(PropertyName = "ItemList")]
        public List<GetServicesList> ItemList { get; set; }

    }

    public class GetServicesList
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "PhoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty(PropertyName = "WhatsappNumber")]
        public string WhatsappNumber { get; set; }
        [JsonProperty(PropertyName = "Category")]
        public string Category { get; set; }
        [JsonProperty(PropertyName = "SubCategory")]
        public string SubCategory { get; set; }
        [JsonProperty(PropertyName = "UserName")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "SellerName")]
        public string SellerName { get; set; }
        [JsonProperty(PropertyName = "Comments")]
        public string Comments { get; set; }
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "Country")]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "City")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "Website")]
        public string Website { get; set; }
        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty(PropertyName = "LogoIcon")]
        public string LogoIcon { get; set; }
        [JsonProperty(PropertyName = "ThreadId")]
        public Guid? ThreadId { get; set; }
        [JsonProperty(PropertyName = "SellerId")]
        public Guid SellerId { get; set; }
        [JsonProperty(PropertyName = "image")]
        public FileUrlResponce Image { get; set; } = new FileUrlResponce();
    }

    public class GetServicesDetail
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "Category")]
        public string Category { get; set; }
        [JsonProperty(PropertyName = "SubCategory")]
        public string SubCategory { get; set; }
        [JsonProperty(PropertyName = "Comments")]
        public string Comments { get; set; }
        [JsonProperty(PropertyName = "UserName")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "SellerName")]
        public string SellerName { get; set; }
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
        [JsonProperty(PropertyName = "LogoIcon")]
        public string LogoIcon { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "Website")]
        public string Website { get; set; }
        [JsonProperty(PropertyName = "IsDeliveryAvailable")]
        public bool IsDeliveryAvailable { get; set; }
        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty(PropertyName = "ThreadId")]
        public Guid? ThreadId { get; set; }
        [JsonProperty(PropertyName = "SellerId")]
        public Guid SellerId { get; set; }
        [JsonProperty(PropertyName = "Attachements")]
        public List<GetAttachementsResponseList> Attachements { get; set; } = new List<GetAttachementsResponseList>();
    }

    public class GetBusinessProfileSP
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "FK_CategoryId")]
        public Guid FK_CategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_SubCategoryId")]
        public Guid FK_SubCategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_StatusId")]
        public int FK_StatusId { get; set; }
        [JsonProperty(PropertyName = "IsDeliveryAvailable")]
        public bool IsDeliveryAvailable { get; set; }
        [JsonProperty(PropertyName = "IsSelfPickup")]
        public bool IsSelfPickup { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "Comments")]
        public string Comments { get; set; }
        [JsonProperty(PropertyName = "CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty(PropertyName = "ThreadId")]
        public Guid? ThreadId { get; set; }
        [JsonProperty(PropertyName = "SellerId")]
        public Guid SellerId { get; set; }
        [JsonProperty(PropertyName = "Url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "ThumbnilUrl")]
        public string ThumbnilUrl { get; set; }
        [JsonProperty(PropertyName = "IsVideo")]
        public bool IsVideo { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "PhoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty(PropertyName = "WhatsappNumber")]
        public string WhatsappNumber { get; set; }
        [JsonProperty(PropertyName = "Category")]
        public string Category { get; set; }
        [JsonProperty(PropertyName = "SubCategory")]
        public string SubCategory { get; set; }
        [JsonProperty(PropertyName = "UserName")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "SellerName")]
        public string SellerName { get; set; }
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "Country")]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "City")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "Website")]
        public string Website { get; set; }
        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "LogoIcon")]
        public string LogoIcon { get; set; }
        [JsonProperty(PropertyName = "FK_CityId")]
        public Guid FK_CityId { get; set; }
        [JsonProperty(PropertyName = "FK_CountryId")]
        public Guid FK_CountryId { get; set; }
    }
}
