using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class Advertisement
    {
        public Advertisement()
        {
            AdvertisementCommission = new HashSet<AdvertisementCommission>();
            AdvertisementDetails = new HashSet<AdvertisementDetails>();
            AdvertisementNotifications = new HashSet<AdvertisementNotifications>();
            Attachement = new HashSet<Attachement>();
            ChatThreads = new HashSet<ChatThreads>();
            Comments = new HashSet<Comments>();
            FeaturedAdvertisements = new HashSet<FeaturedAdvertisements>();
            UserBookmarks = new HashSet<UserBookmarks>();
        }

        public Guid Id { get; set; }
        public Guid FkCategoryId { get; set; }
        public Guid? FkSubCategoryId { get; set; }
        public Guid FkAppUserId { get; set; }
        public Guid? FkBreedId { get; set; }
        public Guid? FkAgeId { get; set; }
        public Guid? FkGenderId { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? MinimumPrice { get; set; }
        public decimal? CommissionAmount { get; set; }
        public Guid FkCityId { get; set; }
        public int FkStatusId { get; set; }
        public string Video { get; set; }
        public string VideoThunbnil { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string WhatsAppNumber { get; set; }
        public bool IsDeliveryAvailable { get; set; }
        public bool IsSelfPickup { get; set; }
        public bool IsFeatured { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeletedBy { get; set; }
        public string WhatsAppCountryCode { get; set; }
        public string PhoneNumberCountryCode { get; set; }
        public string RefId { get; set; }
        public string WaterMarkImage { get; set; }

        public virtual Age FkAge { get; set; }
        public virtual AppUsers FkAppUser { get; set; }
        public virtual Breed FkBreed { get; set; }
        public virtual Categories FkCategory { get; set; }
        public virtual Cities FkCity { get; set; }
        public virtual HilalGenders FkGender { get; set; }
        public virtual AdvertisementStatus FkStatus { get; set; }
        public virtual Categories FkSubCategory { get; set; }
        public virtual ICollection<AdvertisementCommission> AdvertisementCommission { get; set; }
        public virtual ICollection<AdvertisementDetails> AdvertisementDetails { get; set; }
        public virtual ICollection<AdvertisementNotifications> AdvertisementNotifications { get; set; }
        public virtual ICollection<Attachement> Attachement { get; set; }
        public virtual ICollection<ChatThreads> ChatThreads { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<FeaturedAdvertisements> FeaturedAdvertisements { get; set; }
        public virtual ICollection<UserBookmarks> UserBookmarks { get; set; }
    }
}
