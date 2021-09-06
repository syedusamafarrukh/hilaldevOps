using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Hilal.Common
{
    public class SystemGlobal
    {
        public static Guid GetId()
        {
            return Guid.NewGuid();
        }

        public static int Get4digitOTP()
        {
            return new Random().Next(1000, 9999);
        }

        public static string GetSubDirectoryName()
        {
            return DateTime.UtcNow.ToString("/yyyy/MMM/dd");
        }

        public decimal DiffrenceInMunites(DateTime startTime, DateTime endTime)
        {
            return Convert.ToDecimal(endTime.Subtract(startTime).TotalMinutes);
        }
    }
}
