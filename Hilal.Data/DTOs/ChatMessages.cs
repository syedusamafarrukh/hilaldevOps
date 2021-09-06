using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class ChatMessages
    {
        public Guid Id { get; set; }
        public Guid FkChatThreadsId { get; set; }
        public Guid FkSenderId { get; set; }
        public bool IsRead { get; set; }
        public string MessageText { get; set; }
        public string XmppMsgId { get; set; }
        public bool SellerDeleted { get; set; }
        public bool UserDeleted { get; set; }
        public bool IsText { get; set; }
        public bool IsVideo { get; set; }
        public bool IsImage { get; set; }
        public string MediaUrl { get; set; }
        public string ThumbnilUrl { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public virtual ChatThreads FkChatThreads { get; set; }
        public virtual AppUsers FkSender { get; set; }
    }
}
