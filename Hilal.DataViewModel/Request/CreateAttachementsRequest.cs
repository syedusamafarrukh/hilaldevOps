using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request
{
    public class CreateAttachementsRequest
    {
        [JsonProperty(PropertyName = "Id")]
        public int? Id { get; set; }
        [JsonProperty(PropertyName = "FK_BusinessProfileId")]
        public Guid? FK_BusinessProfileId { get; set; }
        [JsonProperty(PropertyName = "FK_AdvertisementId")]
        public Guid? FK_AdvertisementId { get; set; } 
        [JsonProperty(PropertyName = "Url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "ThubnilUrl")]
        public string ThubnilUrl { get; set; }
        [JsonProperty(PropertyName = "WaterMarkImage")]
        public string WaterMarkImage { get; set; }
        [JsonProperty(PropertyName = "IsVideo")]
        public bool? IsVideo { get; set; }
    }

    public class GetAttachementsResponse
    {
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "FK_BusinessProfileId")]
        public General<Guid?> FK_BusinessProfileId { get; set; }
        [JsonProperty(PropertyName = "FK_AdvertisementId")]
        public General<Guid?> FK_AdvertisementId { get; set; }
        [JsonProperty(PropertyName = "Url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "ThubnilUrl")]
        public string ThubnilUrl { get; set; }
        [JsonProperty(PropertyName = "IsVideo")]
        public bool IsVideo { get; set; }
    }

    public class GetAttachementsResponseList
    {
        [JsonProperty(PropertyName = "Url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "ThubnilUrl")]
        public string ThubnilUrl { get; set; } 
        [JsonProperty(PropertyName = "ThumbnailUrl")]
        public string ThumbnailUrl { get; set; }
        [JsonProperty(PropertyName = "WaterMarkImage")]
        public string WaterMarkImage { get; set; }
        [JsonProperty(PropertyName = "IsVideo")]
        public bool IsVideo { get; set; }
    }
}
