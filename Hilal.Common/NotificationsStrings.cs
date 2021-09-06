using Hilal.DataViewModel.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.Common
{
    public static class NotificationsStrings
    {
        public static string GetStringNotification (int StatusId, string Title, int Type)
        {
            string sentNotifyString = "";

            if (StatusId == (int) EStatuses.Approved)
            {
                sentNotifyString = "Hurray! Your"+" "+ (Type == 1 ? "ad" : "Service") + " " + Title +" is approved and published now.";
            }
            else if (StatusId == (int)EStatuses.DisApproved)
            {
                sentNotifyString = "Unfortunately, your" + " " + (Type == 1 ? "ad" : "Service") + " " + Title + " is rejected.";
            }

            return sentNotifyString;
        } 
    }
}
