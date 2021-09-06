using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.Admin.v1
{
    public class GetCitiesResponse : ListGeneralModel
    {
        [JsonProperty(PropertyName = "itemList")]
        public List<GetCities> ItemList { get; set; } = new List<GetCities>();
    }

    public class GetCities
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "FkCountry")]
        public Guid? FkCountry { get; set; }
    }

    public class GetCitiesList
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
    }
}
