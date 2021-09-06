using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class Breed
    {
        public Breed()
        {
            Advertisement = new HashSet<Advertisement>();
            BreedCategories = new HashSet<BreedCategories>();
            BreedDetails = new HashSet<BreedDetails>();
            InverseFkSelfBreed = new HashSet<Breed>();
        }

        public Guid Id { get; set; }
        public Guid? FkCategoryId { get; set; }
        public Guid? FkSubCategoryId { get; set; }
        public Guid? FkSelfBreedId { get; set; }
        public string ImageUrl { get; set; }
        public string ImageThumbnailUrl { get; set; }
        public int? Priority { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeletedBy { get; set; }

        public virtual Categories FkCategory { get; set; }
        public virtual Breed FkSelfBreed { get; set; }
        public virtual Categories FkSubCategory { get; set; }
        public virtual ICollection<Advertisement> Advertisement { get; set; }
        public virtual ICollection<BreedCategories> BreedCategories { get; set; }
        public virtual ICollection<BreedDetails> BreedDetails { get; set; }
        public virtual ICollection<Breed> InverseFkSelfBreed { get; set; }
    }
}
