using Hilal.DataViewModel.Response.App.v1;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.App.v1
{
    public class GetFiltersResquest : GeneralGetList
    {
        public List<Guid?> Categories { get; set; }
        public List<Guid?> SubCategories { get; set; }
        public List<Guid?> Breeds { get; set; }
        public List<Guid?> Ages { get; set; }
        public List<Guid?> Genders { get; set; }
        public int? FK_StatusId { get; set; }
    }
}
