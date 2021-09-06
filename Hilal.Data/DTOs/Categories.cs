using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class Categories
    {
        public Categories()
        {
            AdvertisementFkCategory = new HashSet<Advertisement>();
            AdvertisementFkSubCategory = new HashSet<Advertisement>();
            AgeCategoriesFkCategory = new HashSet<AgeCategories>();
            AgeCategoriesFkSubCategory = new HashSet<AgeCategories>();
            AgeFkCategory = new HashSet<Age>();
            AgeFkSubCategory = new HashSet<Age>();
            BreedCategoriesFkCategory = new HashSet<BreedCategories>();
            BreedCategoriesFkSubCategory = new HashSet<BreedCategories>();
            BreedFkCategory = new HashSet<Breed>();
            BreedFkSubCategory = new HashSet<Breed>();
            CategoriesDetails = new HashSet<CategoriesDetails>();
            Commission = new HashSet<Commission>();
            InverseCategorySelf = new HashSet<Categories>();
            UserBusinessProfileFkCategory = new HashSet<UserBusinessProfile>();
            UserBusinessProfileFkSubCategory = new HashSet<UserBusinessProfile>();
        }

        public Guid Id { get; set; }
        public Guid? CategorySelfId { get; set; }
        public string ImageUrl { get; set; }
        public string ImageThumbnailUrl { get; set; }
        public int Priority { get; set; }
        public bool IsSubCategory { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeletedBy { get; set; }
        public int? FkCategoryType { get; set; }

        public virtual Categories CategorySelf { get; set; }
        public virtual ICollection<Advertisement> AdvertisementFkCategory { get; set; }
        public virtual ICollection<Advertisement> AdvertisementFkSubCategory { get; set; }
        public virtual ICollection<AgeCategories> AgeCategoriesFkCategory { get; set; }
        public virtual ICollection<AgeCategories> AgeCategoriesFkSubCategory { get; set; }
        public virtual ICollection<Age> AgeFkCategory { get; set; }
        public virtual ICollection<Age> AgeFkSubCategory { get; set; }
        public virtual ICollection<BreedCategories> BreedCategoriesFkCategory { get; set; }
        public virtual ICollection<BreedCategories> BreedCategoriesFkSubCategory { get; set; }
        public virtual ICollection<Breed> BreedFkCategory { get; set; }
        public virtual ICollection<Breed> BreedFkSubCategory { get; set; }
        public virtual ICollection<CategoriesDetails> CategoriesDetails { get; set; }
        public virtual ICollection<Commission> Commission { get; set; }
        public virtual ICollection<Categories> InverseCategorySelf { get; set; }
        public virtual ICollection<UserBusinessProfile> UserBusinessProfileFkCategory { get; set; }
        public virtual ICollection<UserBusinessProfile> UserBusinessProfileFkSubCategory { get; set; }
    }
}
