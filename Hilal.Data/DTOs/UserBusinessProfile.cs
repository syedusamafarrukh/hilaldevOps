using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class UserBusinessProfile
    {
        public UserBusinessProfile()
        {
            AdvertisementNotifications = new HashSet<AdvertisementNotifications>();
            Attachement = new HashSet<Attachement>();
            BuinessProfileDetails = new HashSet<BuinessProfileDetails>();
            ChatThreads = new HashSet<ChatThreads>();
            Comments = new HashSet<Comments>();
            UserBookmarks = new HashSet<UserBookmarks>();
        }

        public Guid Id { get; set; }
        public Guid FkCategoryId { get; set; }
        public Guid? FkSubCategoryId { get; set; }
        public Guid FkAppUserId { get; set; }
        public string LogoIcon { get; set; }
        public string ContactNumber { get; set; }
        public string WhatsAppNumber { get; set; }
        public string EmailId { get; set; }
        public string Website { get; set; }
        public Guid FkCityId { get; set; }
        public int FkStatusId { get; set; }
        public bool IsDeliveryAvailable { get; set; }
        public bool IsSelfPickup { get; set; }
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

        public virtual AppUsers FkAppUser { get; set; }
        public virtual Categories FkCategory { get; set; }
        public virtual Cities FkCity { get; set; }
        public virtual AdvertisementStatus FkStatus { get; set; }
        public virtual Categories FkSubCategory { get; set; }
        public virtual ICollection<AdvertisementNotifications> AdvertisementNotifications { get; set; }
        public virtual ICollection<Attachement> Attachement { get; set; }
        public virtual ICollection<BuinessProfileDetails> BuinessProfileDetails { get; set; }
        public virtual ICollection<ChatThreads> ChatThreads { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<UserBookmarks> UserBookmarks { get; set; }
    }
}
