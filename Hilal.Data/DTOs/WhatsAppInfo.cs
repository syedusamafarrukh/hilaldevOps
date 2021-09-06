using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class WhatsAppInfo
    {
        public long Id { get; set; }
        public string WhatsappNumber { get; set; }
        public string WhatsappUrl { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
    }
}
