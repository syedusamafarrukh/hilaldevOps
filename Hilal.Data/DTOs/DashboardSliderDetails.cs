using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class DashboardSliderDetails
    {
        public Guid Id { get; set; }
        public long FkLanguageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid FkDashboardSliderId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeletedBy { get; set; }

        public virtual DashboardSlider FkDashboardSlider { get; set; }
        public virtual Languages FkLanguage { get; set; }
    }
}
