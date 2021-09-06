using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class ServiceControlStatusRequest
    {
        [JsonProperty(PropertyName = "BusinessProfileId")]
        public Guid BusinessProfileId { get; set; }
        [JsonProperty(PropertyName = "StatusId")]
        public int StatusId { get; set; }
        [JsonProperty(PropertyName = "comments")]
        public string comments { get; set; } = "";
    }
}
