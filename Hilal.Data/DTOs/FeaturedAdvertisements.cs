using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class FeaturedAdvertisements
    {
        public Guid Id { get; set; }
        public Guid FkAdvertisementId { get; set; }
        public int Priority { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeletedBy { get; set; }

        public virtual Advertisement FkAdvertisement { get; set; }
    }
}
