using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class HilalGenders
    {
        public HilalGenders()
        {
            Advertisement = new HashSet<Advertisement>();
            GenderDetails = new HashSet<GenderDetails>();
        }

        public Guid Id { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }

        public virtual ICollection<Advertisement> Advertisement { get; set; }
        public virtual ICollection<GenderDetails> GenderDetails { get; set; }
    }
}
