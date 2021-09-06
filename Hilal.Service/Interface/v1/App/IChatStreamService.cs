using Hilal.DataViewModel.Request.App.v1;
using Hilal.DataViewModel.Response.App;
using Hilal.DataViewModel.Response.App.v1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hilal.Service.Interface.v1.App
{
    public interface IChatStreamService
    {
        Task<Guid> SaveChatStream(CreateChatStream createChatStream, Guid userId, int LanguageId);
        GetChatStreamResponse GetChatStream(GetChatStreamRequest chatStreamRequest);
        GetChatThreadResponse GetChatThread(GetChatStreamRequest chatStreamRequest);
        GetNotificationsResponse getNotifications(GetNotificationRequest notificationRequest);
        DashBoardCountResponse getDashBoardCount(Guid UserId);
        WhtsappInfoViewModel getWhtsapInfo();
    }
}
