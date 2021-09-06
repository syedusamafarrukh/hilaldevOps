using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class Commission
    {
        public Commission()
        {
            CommissionDetails = new HashSet<CommissionDetails>();
        }

        public Guid Id { get; set; }
        public Guid? FkCategoryId { get; set; }
        public double Percentage { get; set; }
        public double StartRange { get; set; }
        public double EndRange { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeletedBy { get; set; }

        public virtual Categories FkCategory { get; set; }
        public virtual ICollection<CommissionDetails> CommissionDetails { get; set; }
    }
}
