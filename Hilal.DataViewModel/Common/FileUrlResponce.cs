using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Common
{
    public class FileUrlResponce
    {
        [JsonProperty(PropertyName = "url")]
        public string URL { get; set; } = "";
        [JsonProperty(PropertyName = "thumbnailUrl")]
        public string ThumbnailUrl { get; set; } = "";
        [JsonProperty(PropertyName = "WaterMarkImage")]
        public string WaterMarkImage { get; set; } = "";
        [JsonProperty(PropertyName = "IsVideo")]
        public bool IsVideo { get; set; } = false;
    }
}
