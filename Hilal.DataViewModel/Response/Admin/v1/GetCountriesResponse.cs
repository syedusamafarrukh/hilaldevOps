using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.Admin.v1
{
    public class GetCountriesResponse : ListGeneralModel
    {
        [JsonProperty(PropertyName = "itemList")]
        public List<GetCountries> ItemList { get; set; } = new List<GetCountries>();
    }

    public class GetCountries
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
    }
}
