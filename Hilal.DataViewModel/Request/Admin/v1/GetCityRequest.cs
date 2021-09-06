using Hilal.DataViewModel.Response.Admin.v1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class GetCityRequest : ListGeneralModel
    {
        [JsonProperty(PropertyName = "countryId")]
        public Guid? countryId { get; set; }
    }

}
