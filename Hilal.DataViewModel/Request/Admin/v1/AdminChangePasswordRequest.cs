using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class AdminChangePasswordRequest
    {
        [Required]
        [JsonProperty(PropertyName = "userId")]
        public Guid UserId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "oldPassword")]
        public string OldPassword { get; set; } = "";

        [Required]
        [JsonProperty(PropertyName = "newPassword")]
        public string NewPassword { get; set; } = "";
    }
}
