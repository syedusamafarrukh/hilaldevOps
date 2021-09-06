using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class ApprovelSettings
    {
        public int Id { get; set; }
        public int ApprovelType { get; set; }
        public int CategoryType { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? StartDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeletedBy { get; set; }
    }
}
