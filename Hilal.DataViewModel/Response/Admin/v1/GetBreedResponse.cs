using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.Admin.v1
{
    public class GetBreedResponse : ListGeneralModel
    {
        [JsonProperty(PropertyName = "itemList")]
        public List<GetBreed> ItemList { get; set; } = new List<GetBreed>();
    }

    public class GetBreed
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public General<Guid?> FK_CategoryId { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public List<General<Guid?>> FK_SubCategoryId { get; set; }

        [JsonProperty(PropertyName = "image")]
        public FileUrlResponce Image { get; set; } = new FileUrlResponce();

        [JsonProperty(PropertyName = "priority")]
        public int? Priority { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }
    }

    public class GetBreedList
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "image")]
        public FileUrlResponce Image { get; set; } = new FileUrlResponce();

        [JsonProperty(PropertyName = "priority")]
        public int? Priority { get; set; }
    }
}
