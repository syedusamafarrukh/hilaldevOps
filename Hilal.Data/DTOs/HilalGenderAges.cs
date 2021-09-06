using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class HilalGenderAges
    {
        public Guid Id { get; set; }
        public Guid FkGenderId { get; set; }
        public Guid FkAgeId { get; set; }
        public Guid FkCategoryId { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public bool? IsActive { get; set; }

        public virtual Age FkAge { get; set; }
        public virtual Categories FkCategory { get; set; }
        public virtual HilalGenders FkGender { get; set; }
    }
}
