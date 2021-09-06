using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class Countries
    {
        public Countries()
        {
            Cities = new HashSet<Cities>();
            CountryDetails = new HashSet<CountryDetails>();
        }

        public Guid Id { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeletedBy { get; set; }

        public virtual ICollection<Cities> Cities { get; set; }
        public virtual ICollection<CountryDetails> CountryDetails { get; set; }
    }
}
