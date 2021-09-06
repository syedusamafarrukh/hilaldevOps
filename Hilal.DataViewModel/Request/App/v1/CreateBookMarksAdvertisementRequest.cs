using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.App.v1
{
    public class CreateBookMarksAdvertisementRequest
    {
        [JsonProperty(PropertyName = "FK_AppUserId")]
        public Guid? FK_AppUserId { get; set; }
        [JsonProperty(PropertyName = "FK_AdvertisementId")]
        public Guid FK_AdvertisementId { get; set; }
    }
}
