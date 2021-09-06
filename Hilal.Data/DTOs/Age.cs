using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class Age
    {
        public Age()
        {
            Advertisement = new HashSet<Advertisement>();
            AgeCategories = new HashSet<AgeCategories>();
            AgeDetails = new HashSet<AgeDetails>();
        }

        public Guid Id { get; set; }
        public Guid? FkCategoryId { get; set; }
        public Guid? FkSubCategoryId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeletedBy { get; set; }
        public int Priority { get; set; }

        public virtual Categories FkCategory { get; set; }
        public virtual Categories FkSubCategory { get; set; }
        public virtual ICollection<Advertisement> Advertisement { get; set; }
        public virtual ICollection<AgeCategories> AgeCategories { get; set; }
        public virtual ICollection<AgeDetails> AgeDetails { get; set; }
    }
}
