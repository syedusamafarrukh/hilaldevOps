using Hilal.DataViewModel.Response.App.v1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.App.v1
{
    public class GetNotificationRequest : GeneralGetList
    {
        [JsonProperty(PropertyName = "UserId")]
        public Guid? UserId { get; set; }
    }
}
