using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hilal.DataViewModel.Request
{
    public class AdminLoginRequest
    {
        [Required]
        [JsonProperty(PropertyName = "UserInfo")]
        public string UserInfo { get; set; }

        [Required]
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
    }
}
