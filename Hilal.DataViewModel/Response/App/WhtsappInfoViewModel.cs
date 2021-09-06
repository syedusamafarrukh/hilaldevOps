using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.App
{
    public class WhtsappInfoViewModel
    {
        [JsonProperty(PropertyName = "WhatsappNumber")]
        public string WhatsappNumber { get; set; }
        [JsonProperty(PropertyName = "WhatsappUrl")]
        public string WhatsappUrl { get; set; }
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }
    }
}
