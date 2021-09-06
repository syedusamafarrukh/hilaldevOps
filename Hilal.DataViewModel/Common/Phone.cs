using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Common
{
    public class Phone
    {
        [JsonProperty(PropertyName = "countryCode")]
        public string CountryCode { get; set; } = "";

        [JsonProperty(PropertyName = "phoneNumber")]
        public string PhoneNumber { get; set; } = "";
    }
}
