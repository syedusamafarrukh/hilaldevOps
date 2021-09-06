using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class BreedCategories
    {
        public Guid Id { get; set; }
        public Guid FkBreedId { get; set; }
        public Guid FkSubCategoryId { get; set; }
        public Guid FkCategoryId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Breed FkBreed { get; set; }
        public virtual Categories FkCategory { get; set; }
        public virtual Categories FkSubCategory { get; set; }
    }
}
