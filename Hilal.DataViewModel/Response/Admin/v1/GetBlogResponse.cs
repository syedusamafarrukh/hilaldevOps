using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.Admin.v1
{
    public class GetBlogResponse : ListGeneralModel
    {
        [JsonProperty(PropertyName = "itemList")]
        public List<BlogViewModel> ItemList { get; set; } = new List<BlogViewModel>();
    }

    public class BlogViewModel
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid? Id { get; set; }
        [JsonProperty(PropertyName = "HeaderImage")]
        public string HeaderImage { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "PostedDate")]
        public DateTime PostedDate { get; set; }
    }
}
