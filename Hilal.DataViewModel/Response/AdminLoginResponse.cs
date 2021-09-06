using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response
{
    public class AdminLoginResponse
    {
        [JsonProperty(PropertyName = "accessToken")]
        public string AccessToken { get; set; } = "";
        [JsonProperty(PropertyName = "RightsId")]
        public List<General<Guid>> RightsId { get; set; } = new List<General<Guid>>();
    }
}
