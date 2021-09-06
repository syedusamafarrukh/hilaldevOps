using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Response.App.v1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.App.v1
{
    public class CreateAdvertisementRequest
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid? Id { get; set; }
        [JsonProperty(PropertyName = "FK_CategoryId")]
        public Guid FK_CategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_SubCategoryId")]
        public Guid? FK_SubCategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_AppUserId")]
        public Guid? FK_AppUserId { get; set; }
        [JsonProperty(PropertyName = "FK_BreedId")]
        public Guid? FK_BreedId { get; set; }
        [JsonProperty(PropertyName = "FK_AgeId")]
        public Guid? FK_AgeId { get; set; }
        [JsonProperty(PropertyName = "FK_GenderId")]
        public Guid? FK_GenderId { get; set; }
        [JsonProperty(PropertyName = "FK_CityId")]
        public Guid FK_CityId { get; set; }
        [JsonProperty(PropertyName = "countryId")]
        public Guid? countryId { get; set; }
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
        [JsonProperty(PropertyName = "VideoThumbnil")]
        public string VideoThumbnil { get; set; }
        [JsonProperty(PropertyName = "PhoneNumber")]
        public PhoneNumberModel PhoneNumber { get; set; }
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "WhatsAppNumber")]
        public PhoneNumberModel WhatsAppNumber { get; set; }
        [JsonProperty(PropertyName = "IsDeliveryAvailable")]
        public string IsDeliveryAvailable { get; set; }
        [JsonProperty(PropertyName = "IsSelfPickup")]
        public string IsSelfPickup { get; set; }
        [JsonProperty(PropertyName = "WaterMarkImage")]
        public string WaterMarkImage { get; set; }
        [JsonProperty(PropertyName = "AttachementList")]
        public List<CreateAttachementsRequest> AttachementList { get; set; }
        [JsonProperty(PropertyName = "AdvertisementDetailsInformation")]
        public List<CreateAdvertisementDetails> AdvertisementDetailsInformation { get; set; }

    }

    public class getEditAdvertisementRequest
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid? Id { get; set; }
        [JsonProperty(PropertyName = "FK_CategoryId")]
        public General<Guid> FK_CategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_SubCategoryId")]
        public General<Guid?> FK_SubCategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_AppUserId")]
        public General<Guid?> FK_AppUserId { get; set; }
        [JsonProperty(PropertyName = "FK_BreedId")]
        public General<Guid?> FK_BreedId { get; set; }
        [JsonProperty(PropertyName = "FK_AgeId")]
        public General<Guid?> FK_AgeId { get; set; }
        [JsonProperty(PropertyName = "FK_GenderId")]
        public General<Guid?> FK_GenderId { get; set; }
        [JsonProperty(PropertyName = "FK_CityId")]
        public General<Guid> FK_CityId { get; set; }
        [JsonProperty(PropertyName = "FK_CountryId")]
        public General<Guid?> FK_CountryId { get; set; }
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
        public PhoneNumberModel PhoneNumber { get; set; }
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "WhatsAppNumber")]
        public PhoneNumberModel WhatsAppNumber { get; set; }
        [JsonProperty(PropertyName = "IsDeliveryAvailable")]
        public string IsDeliveryAvailable { get; set; }
        [JsonProperty(PropertyName = "IsSelfPickup")]
        public string IsSelfPickup { get; set; }
        [JsonProperty(PropertyName = "AttachementList")]
        public List<CreateAttachementsRequest> AttachementList { get; set; }
        [JsonProperty(PropertyName = "AdvertisementDetailsInformation")]
        public List<CreateAdvertisementDetails> AdvertisementDetailsInformation { get; set; }

    }

    public class CreateAdvertisementDetails
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid? Id { get; set; }
        [JsonProperty(PropertyName = "FK_LanguageId")]
        public long FK_LanguageId { get; set; }
        [JsonProperty(PropertyName = "FK_AdvertisementId")]
        public Guid? FK_AdvertisementId { get; set; }
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
    }

    public class GetAdvertisementRequest : GeneralGetList
    {
        //[JsonProperty(PropertyName = "Id")]
        //public Guid? Id { get; set; } = new Guid("00000000-0000-0000-0000-000000000000");
        [JsonProperty(PropertyName = "FK_CategoryId")]
        public Guid? FK_CategoryId { get; set; } = new Guid("00000000-0000-0000-0000-000000000000");
        [JsonProperty(PropertyName = "FK_SubCategoryId")]
        public Guid? FK_SubCategoryId { get; set; } = new Guid("00000000-0000-0000-0000-000000000000");
        [JsonProperty(PropertyName = "FK_AppUserId")]
        public Guid? FK_AppUserId { get; set; } = new Guid("00000000-0000-0000-0000-000000000000");
        [JsonProperty(PropertyName = "FK_StatusId")]
        public int? FK_StatusId { get; set; } = 0;
        [JsonProperty(PropertyName = "FK_BreedId")]
        public Guid? FK_BreedId { get; set; } = new Guid("00000000-0000-0000-0000-000000000000");
        [JsonProperty(PropertyName = "FK_AgeId")]
        public Guid? FK_AgeId { get; set; } = new Guid("00000000-0000-0000-0000-000000000000");
        [JsonProperty(PropertyName = "FK_GenderId")]
        public Guid? FK_GenderId { get; set; } = new Guid("00000000-0000-0000-0000-000000000000");
        [JsonProperty(PropertyName = "FK_CityId")]
        public Guid? FK_CityId { get; set; } = new Guid("00000000-0000-0000-0000-000000000000");
        [JsonProperty(PropertyName = "FK_CountryId")]
        public Guid? FK_CountryId { get; set; } = new Guid("00000000-0000-0000-0000-000000000000");
    }
}
