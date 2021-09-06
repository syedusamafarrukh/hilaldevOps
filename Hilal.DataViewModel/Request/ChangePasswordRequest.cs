using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hilal.DataViewModel.Request
{
    public class ChangePasswordRequest
    {
        [Required]
        [JsonProperty(PropertyName = "oldPassword")]
        public string OldPassword { get; set; } = "";

        [Required]
        [JsonProperty(PropertyName = "newPassword")]
        public string NewPassword { get; set; } = "";
    }
}
