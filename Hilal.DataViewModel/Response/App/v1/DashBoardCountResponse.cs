using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.App.v1
{
    public class DashBoardCountResponse
    {
        [JsonProperty(PropertyName = "NotificationCount")]
        public int NotificationCount { get; set; }
        [JsonProperty(PropertyName = "ChatCount")]
        public int ChatCount { get; set; }
    }
}
