using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request
{
    public class Raw
    {
        public Guid? Id { get; set; }
        public string Filter { get; set; }
    }

    public class PaginationViewModel
    {
        public string Search { get; set; }
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 0;
    }

    public class DetailResponceModel<T>
    {
        public int TotalCount { get; set; }
        public List<T> DtlList { get; set; }
    }

    public class ItemGuidViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

}
