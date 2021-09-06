using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class AppUsers
    {
        public AppUsers()
        {
            Advertisement = new HashSet<Advertisement>();
            AdvertisementNotifications = new HashSet<AdvertisementNotifications>();
            AppUserProfiles = new HashSet<AppUserProfiles>();
            AppUserSubscription = new HashSet<AppUserSubscription>();
            ChatMessages = new HashSet<ChatMessages>();
            ChatNotificationsFkReceiver = new HashSet<ChatNotifications>();
            ChatNotificationsFkSender = new HashSet<ChatNotifications>();
            ChatThreadsFkSeller = new HashSet<ChatThreads>();
            ChatThreadsFkUser = new HashSet<ChatThreads>();
            PaymentHistory = new HashSet<PaymentHistory>();
            UserBookmarks = new HashSet<UserBookmarks>();
            UserBusinessProfile = new HashSet<UserBusinessProfile>();
            UserDeviceInformations = new HashSet<UserDeviceInformations>();
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsVerified { get; set; }
        public bool IsSubscribed { get; set; }
        public bool IsSubscribedExpired { get; set; }
        public bool IsAddAutoApprovel { get; set; }
        public bool IsServiceAutoApprovel { get; set; }
        public string Otp { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }

        public virtual ICollection<Advertisement> Advertisement { get; set; }
        public virtual ICollection<AdvertisementNotifications> AdvertisementNotifications { get; set; }
        public virtual ICollection<AppUserProfiles> AppUserProfiles { get; set; }
        public virtual ICollection<AppUserSubscription> AppUserSubscription { get; set; }
        public virtual ICollection<ChatMessages> ChatMessages { get; set; }
        public virtual ICollection<ChatNotifications> ChatNotificationsFkReceiver { get; set; }
        public virtual ICollection<ChatNotifications> ChatNotificationsFkSender { get; set; }
        public virtual ICollection<ChatThreads> ChatThreadsFkSeller { get; set; }
        public virtual ICollection<ChatThreads> ChatThreadsFkUser { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistory { get; set; }
        public virtual ICollection<UserBookmarks> UserBookmarks { get; set; }
        public virtual ICollection<UserBusinessProfile> UserBusinessProfile { get; set; }
        public virtual ICollection<UserDeviceInformations> UserDeviceInformations { get; set; }
    }
}
