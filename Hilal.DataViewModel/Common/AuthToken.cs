using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Common
{
    public class AuthToken
    {
        public Guid UserId { get; set; }
        public string DeviceToken { get; set; }
    }
}
