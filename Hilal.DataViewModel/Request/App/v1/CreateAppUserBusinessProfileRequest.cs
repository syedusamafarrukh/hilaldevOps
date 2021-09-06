using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Response.App.v1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.App.v1
{
    public class CreateAppUserBusinessProfileRequest
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid? Id { get; set; }
        [JsonProperty(PropertyName = "FK_CategoryId")]
        public Guid FK_CategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_SubCategoryId")]
        public Guid? FK_SubCategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_AppUserId")]
        public Guid? FK_AppUserId { get; set; }
        [JsonProperty(PropertyName = "LogoIcon")]
        public FileUrlResponce LogoIcon { get; set; } = new FileUrlResponce();
        [JsonProperty(PropertyName = "ContactNumber")]
        public PhoneNumberModel ContactNumber { get; set; }
        [JsonProperty(PropertyName = "WhatsAppNumber")]
        public PhoneNumberModel WhatsAppNumber { get; set; }
        [JsonProperty(PropertyName = "EmailId")]
        public string EmailId { get; set; }
        [JsonProperty(PropertyName = "Website")]
        public string Website { get; set; }
        [JsonProperty(PropertyName = "FK_CityId")]
        public Guid FK_CityId { get; set; }
        [JsonProperty(PropertyName = "FK_StatusId")]
        public int FK_StatusId { get; set; }
        [JsonProperty(PropertyName = "IsDeliveryAvailable")]
        public string IsDeliveryAvailable { get; set; }
        [JsonProperty(PropertyName = "IsSelfPickup")]
        public string IsSelfPickup { get; set; }
        [JsonProperty(PropertyName = "AttachementList")]
        public List<CreateAttachementsRequest> AttachementList { get; set; }
        [JsonProperty(PropertyName = "IsSelfPickup")]
        public List<CreateUserBusinessProfileDetailRequest> UserBusinessProfileDetailInformation { get; set; }

    }

    public class GetEditUserBusinessProfileRequest
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid? Id { get; set; }
        [JsonProperty(PropertyName = "FK_CategoryId")]
        public General<Guid> FK_CategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_SubCategoryId")]
        public General<Guid?> FK_SubCategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_AppUserId")]
        public General<Guid?> FK_AppUserId { get; set; }
        [JsonProperty(PropertyName = "LogoIcon")]
        public FileUrlResponce LogoIcon { get; set; } = new FileUrlResponce();
        [JsonProperty(PropertyName = "ContactNumber")]
        public PhoneNumberModel ContactNumber { get; set; }
        [JsonProperty(PropertyName = "WhatsAppNumber")]
        public PhoneNumberModel WhatsAppNumber { get; set; }
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
        public string IsDeliveryAvailable { get; set; }
        [JsonProperty(PropertyName = "IsSelfPickup")]
        public string IsSelfPickup { get; set; }
        [JsonProperty(PropertyName = "AttachementList")]
        public List<CreateAttachementsRequest> AttachementList { get; set; }
        [JsonProperty(PropertyName = "IsSelfPickup")]
        public List<CreateUserBusinessProfileDetailRequest> UserBusinessProfileDetailInformation { get; set; }

    }

    public class CreateUserBusinessProfileDetailRequest
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "FK_UserBusinessProfileId")]
        public Guid FK_UserBusinessProfileId { get; set; }
        [JsonProperty(PropertyName = "FK_LanguageId")]
        public long FK_LanguageId { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "SellerName")]
        public string SellerName { get; set; }
        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }
    }

    public class GetBusinessProfileRequest : GeneralGetList
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid? Id { get; set; }
        [JsonProperty(PropertyName = "FK_CategoryId")]
        public Guid? FK_CategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_SubCategoryId")]
        public Guid? FK_SubCategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_CityId")]
        public Guid? FK_CityId { get; set; }
        [JsonProperty(PropertyName = "FK_CountryId")]
        public Guid? FK_CountryId { get; set; }
        [JsonProperty(PropertyName = "FK_AppUserId")]
        public Guid? FK_AppUserId { get; set; }
        [JsonProperty(PropertyName = "FK_StatusId")]
        public int? FK_StatusId { get; set; }
        [JsonProperty(PropertyName = "SortIndex")]
        public int? SortIndex { get; set; }
        [JsonProperty(PropertyName = "SortBy")]
        public string SortBy { get; set; }
    }
}
