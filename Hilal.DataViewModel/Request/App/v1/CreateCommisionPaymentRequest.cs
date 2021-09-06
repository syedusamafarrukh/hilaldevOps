using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.App.v1
{
    public class CreateCommisionPaymentRequest
    {
        [JsonProperty(PropertyName = "AdvertisementId")]
        public Guid AdvertisementId { get; set; }
        [JsonProperty(PropertyName = "OrderRefId")]
        public string OrderRefId { get; set; }
    }
}
