using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.App.v1
{
    public class CreateFeaturedAdvertisementRequest
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid? Id { get; set; }
        [JsonProperty(PropertyName = "FK_AdvertisementId")]
        public Guid FK_AdvertisementId { get; set; }
        [JsonProperty(PropertyName = "Priority")]
        public int Priority { get; set; }
    }
}
