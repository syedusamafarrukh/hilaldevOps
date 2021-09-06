using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.App.v1
{
    public class CreatePaymentOrderRequest
    {
        [JsonProperty(PropertyName = "planId")]
        public Guid planId { get; set; }
        [JsonProperty(PropertyName = "Amount")]
        public decimal Amount { get; set; }
        [JsonProperty(PropertyName = "EmailAddress")]
        public string EmailAddress { get; set; }
        [JsonProperty(PropertyName = "firstName")]
        public string firstName { get; set; }
        [JsonProperty(PropertyName = "lastName")]
        public string lastName { get; set; }
        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string city { get; set; }

    }

    public class CreateAdCommisionRequest
    {
        [JsonProperty(PropertyName = "AdvertisementId")]
        public Guid AdvertisementId { get; set; }
        [JsonProperty(PropertyName = "Amount")]
        public decimal Amount { get; set; }
        [JsonProperty(PropertyName = "EmailAddress")]
        public string EmailAddress { get; set; }
        [JsonProperty(PropertyName = "firstName")]
        public string firstName { get; set; }
        [JsonProperty(PropertyName = "lastName")]
        public string lastName { get; set; }
        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string city { get; set; }

    }
}
