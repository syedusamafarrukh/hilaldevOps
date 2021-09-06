using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Request;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.App.v1
{
    public class GetAdvertisementResponse
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "FK_CategoryId")]
        public General<Guid> FK_CategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_SubCategoryId")]
        public General<Guid?> FK_SubCategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_AppUserId")]
        public General<Guid> FK_AppUserId { get; set; }
        [JsonProperty(PropertyName = "FK_BreedId")]
        public General<Guid> FK_BreedId { get; set; }
        [JsonProperty(PropertyName = "FK_AgeId")]
        public General<Guid> FK_AgeId { get; set; }
        [JsonProperty(PropertyName = "FK_GenderId")]
        public General<Guid> FK_GenderId { get; set; }
        [JsonProperty(PropertyName = "FK_CityId")]
        public General<Guid> FK_CityId { get; set; }
        [JsonProperty(PropertyName = "SalePrice")]
        public decimal? SalePrice { get; set; }
        [JsonProperty(PropertyName = "MinimumPrice")]
        public decimal? MinimumPrice { get; set; }
        [JsonProperty(PropertyName = "CommissionAmount")]
        public decimal? CommissionAmount { get; set; }
        [JsonProperty(PropertyName = "FK_StatusId")]
        public int FK_StatusId { get; set; }
        [JsonProperty(PropertyName = "Video")]
        public string Video { get; set; }
        [JsonProperty(PropertyName = "PhoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "WhatsAppNumber")]
        public string WhatsAppNumber { get; set; }
        [JsonProperty(PropertyName = "IsDeliveryAvailable")]
        public bool IsDeliveryAvailable { get; set; }
        [JsonProperty(PropertyName = "IsSelfPickup")]
        public bool IsSelfPickup { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "SellerName")]
        public string SellerName { get; set; }
        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "Father")]
        public string Father { get; set; }
        [JsonProperty(PropertyName = "Mother")]
        public string Mother { get; set; }
        [JsonProperty(PropertyName = "AttachementList")]
        public List<CreateAttachementsRequest> AttachementList { get; set; }

    }
}
