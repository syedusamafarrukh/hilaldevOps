using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class AdvertisementNotifications
    {
        public Guid Id { get; set; }
        public Guid? FkAdvertisementId { get; set; }
        public Guid? FkServiceId { get; set; }
        public Guid? ReceiverId { get; set; }
        public Guid? AdminReceiverId { get; set; }
        public string BodyText { get; set; }
        public string DeviceToken { get; set; }
        public bool IsSeen { get; set; }
        public bool IsAdminNotify { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual AdminUsers AdminReceiver { get; set; }
        public virtual Advertisement FkAdvertisement { get; set; }
        public virtual UserBusinessProfile FkService { get; set; }
        public virtual AppUsers Receiver { get; set; }
    }
}
