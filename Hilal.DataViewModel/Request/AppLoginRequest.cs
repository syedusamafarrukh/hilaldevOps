using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hilal.DataViewModel.Request
{
    public class AppLoginRequest
    {
        [Required]
        [JsonProperty(PropertyName = "UserInfo")]
        public string UserInfo { get; set; }

        [Required]
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "deviceToken")]
        public string DeviceToken { get; set; } = Guid.Empty.ToString();

        [Required]
        [JsonProperty(PropertyName = "deviceModel")]
        public string DeviceModel { get; set; }

        [Required]
        [JsonProperty(PropertyName = "os")]
        public string OS { get; set; }

        [Required]
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        [Required]
        [JsonProperty(PropertyName = "deviceType")]
        public int DeviceType { get; set; }
    }

    public class SaveDeviceInformationRequest
    {
        [Required]
        [JsonProperty(PropertyName = "UserId")]
        public Guid UserId { get; set; }

        [JsonProperty(PropertyName = "deviceToken")]
        public string DeviceToken { get; set; } = Guid.Empty.ToString();

        [Required]
        [JsonProperty(PropertyName = "deviceModel")]
        public string DeviceModel { get; set; }

        [Required]
        [JsonProperty(PropertyName = "os")]
        public string OS { get; set; }

        [Required]
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        [Required]
        [JsonProperty(PropertyName = "deviceType")]
        public int DeviceType { get; set; }
    }
}
