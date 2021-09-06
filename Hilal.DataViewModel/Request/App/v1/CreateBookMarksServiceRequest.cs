using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.App.v1
{
    public class CreateBookMarksServiceRequest
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid? Id { get; set; }
        [JsonProperty(PropertyName = "FK_AppUserId")]
        public Guid? FK_AppUserId { get; set; }
        [JsonProperty(PropertyName = "FK_ServiceId")]
        public Guid? FK_ServiceId { get; set; }
    }
}
