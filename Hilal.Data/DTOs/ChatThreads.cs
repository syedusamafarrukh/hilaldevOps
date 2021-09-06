using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class ChatThreads
    {
        public ChatThreads()
        {
            ChatMessages = new HashSet<ChatMessages>();
            ChatNotifications = new HashSet<ChatNotifications>();
        }

        public Guid Id { get; set; }
        public Guid FkSellerId { get; set; }
        public Guid FkUserId { get; set; }
        public Guid? FkAdvertisementId { get; set; }
        public Guid? FkServiceId { get; set; }
        public bool SellerDeleted { get; set; }
        public bool UserDeleted { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Deleted1Date { get; set; }
        public string Deleted1By { get; set; }
        public DateTime? Deleted2Date { get; set; }
        public string Deleted2By { get; set; }

        public virtual Advertisement FkAdvertisement { get; set; }
        public virtual AppUsers FkSeller { get; set; }
        public virtual UserBusinessProfile FkService { get; set; }
        public virtual AppUsers FkUser { get; set; }
        public virtual ICollection<ChatMessages> ChatMessages { get; set; }
        public virtual ICollection<ChatNotifications> ChatNotifications { get; set; }
    }
}
