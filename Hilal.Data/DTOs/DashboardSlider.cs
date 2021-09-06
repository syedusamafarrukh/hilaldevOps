using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class DashboardSlider
    {
        public DashboardSlider()
        {
            DashboardSliderDetails = new HashSet<DashboardSliderDetails>();
        }

        public Guid Id { get; set; }
        public bool? IsActive { get; set; }
        public string Url { get; set; }
        public string ThumbnilUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? StartDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeletedBy { get; set; }

        public virtual ICollection<DashboardSliderDetails> DashboardSliderDetails { get; set; }
    }
}
