using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.Admin.v1
{
    public class GetSubscriptionResponse : ListGeneralModel
    {
        [JsonProperty(PropertyName = "itemList")]
        public List<GetSubscription> ItemList { get; set; } = new List<GetSubscription>();
    }

    public class GetSubscription
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "ValidityDays")]
        public int ValidityDays { get; set; }
        [JsonProperty(PropertyName = "Amount")]
        public decimal Amount { get; set; }
        [JsonProperty(PropertyName = "ValidityPosts")]
        public int? ValidityPosts { get; set; }
        [JsonProperty(PropertyName = "IsDisplayed")]
        public bool? IsDisplayed { get; set; }
        [JsonProperty(PropertyName = "DisplayValidityDays")]
        public string DisplayValidityDays { get; set; }
    }

    public class GetSubscriptionList
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "ValidityDays")]
        public int ValidityDays { get; set; }
        [JsonProperty(PropertyName = "Amount")]
        public decimal Amount { get; set; }
        [JsonProperty(PropertyName = "DisplayValidityDays")]
        public string DisplayValidityDays { get; set; }
        [JsonProperty(PropertyName = "ValidateDate")]
        public DateTime ValidateDate { get; set; }
    }
}
