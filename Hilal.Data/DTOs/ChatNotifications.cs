using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class ChatNotifications
    {
        public Guid Id { get; set; }
        public Guid FkSenderId { get; set; }
        public Guid FkReceiverId { get; set; }
        public Guid FkChatThreadId { get; set; }
        public string BodyText { get; set; }
        public bool IsSeen { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public virtual ChatThreads FkChatThread { get; set; }
        public virtual AppUsers FkReceiver { get; set; }
        public virtual AppUsers FkSender { get; set; }
    }
}
