using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response
{
    public class AppLoginResponse
    {
        [JsonProperty(PropertyName = "accessToken")]
        public string AccessToken { get; set; } = "";

        [JsonProperty(PropertyName = "isAccountVerified")]
        public bool IsAccountVerified { get; set; } = false;
        [JsonProperty(PropertyName = "PhoneNumber")]
        public PhoneNumberModel PhoneNumber { get; set; }
        [JsonProperty(PropertyName = "UserId")]
        public Guid UserId { get; set; }
        [JsonProperty(PropertyName = "PlanId")]
        public Guid? PlanId { get; set; }
        [JsonProperty(PropertyName = "IsSubscribed")]
        public bool IsSubscribed { get; set; } = false;
    }
}
