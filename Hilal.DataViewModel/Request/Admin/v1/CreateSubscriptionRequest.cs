using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class CreateSubscriptionRequest
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }
        [JsonProperty(PropertyName = "ValidityDays")]
        public int ValidityDays { get; set; }
        [JsonProperty(PropertyName = "ValidityPosts")]
        public int? ValidityPosts { get; set; }
        [JsonProperty(PropertyName = "IsDisplayed")]
        public bool? IsDisplayed { get; set; }
        [JsonProperty(PropertyName = "Amount")]
        public decimal Amount { get; set; }
        [Required]
        [JsonProperty(PropertyName = "SubscriptionInformations")]
        public List<SubscriptionDetailRequest> SubscriptionInformations { get; set; }
    }

    public class SubscriptionDetailRequest
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "fkmasterId")]
        public Guid FKMasterId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "languageId")]
        public long LanguageId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "DisplayValidityDays")]
        public string DisplayValidityDays { get; set; }
    }
}
