using Hilal.Common;
using Hilal.Data.Context;
using Hilal.Data.DTOs;
using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Enum;
using Hilal.DataViewModel.Request.App.v1;
using Hilal.DataViewModel.Response.App;
using Hilal.DataViewModel.Response.App.v1;
using Hilal.Service.Interface.v1.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Hilal.Service.Implementation.v1.App
{
    public class ChatStreamService : IChatStreamService
    {
        private readonly IConfiguration configuration;

        public ChatStreamService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<Guid> SaveChatStream(CreateChatStream createChatStream, Guid userId , int LanguageId)
        {
            try
            {
                bool response = false;
                var chatmessagesObject = new List<ChatMessages>();
                chatmessagesObject.Add(new ChatMessages
                {
                    Id = SystemGlobal.GetId(),
                    FkSenderId = createChatStream.FK_SenderId,
                    IsImage = false,
                    IsText = true,
                    IsVideo = false,
                    IsRead = false,
                    MessageText = createChatStream.MessageText,
                    SellerDeleted = false,
                    UserDeleted = false,
                    IsActive = true,
                    CreatedBy = userId.ToString(),
                    CreatedDate = DateTime.UtcNow
                });
                var chatNotificationsObject = new List<ChatNotifications>();
                chatNotificationsObject.Add(new ChatNotifications
                {
                    Id = SystemGlobal.GetId(),
                    FkSenderId = createChatStream.FK_SenderId,
                    FkReceiverId = createChatStream.FK_ReceiverId,
                    BodyText = "You Got a New Message",
                    IsSeen = false,
                    IsActive = true,
                    CreatedBy = userId.ToString(),
                    CreatedDate = DateTime.UtcNow
                });
                var id = SystemGlobal.GetId();
                using (var db = new HilalDbContext())
                {
                    using (var trans = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var advertisementObject = new Advertisement();
                            var serviceObject = new UserBusinessProfile();
                            Guid sellerId = new Guid();
                            Guid ASId = new Guid();
                            
                            var threadObject = db.ChatThreads.FirstOrDefault(x => x.Id == createChatStream.ThreadId);
                            if (createChatStream.FK_AdvertisementId != null)
                            {
                                advertisementObject = db.Advertisement.Include(x => x.FkAppUser).FirstOrDefault(x => x.Id == createChatStream.FK_AdvertisementId);
                                if (advertisementObject != null)
                                {
                                    sellerId = advertisementObject.FkAppUserId;
                                    ASId = advertisementObject.Id; 
                                }
                            }
                            else
                            {
                                serviceObject = db.UserBusinessProfile.Include(x => x.FkAppUser).FirstOrDefault(x => x.Id == createChatStream.FK_ServiceId);
                                if (serviceObject != null)
                                {
                                    ASId = serviceObject.Id;
                                    sellerId = serviceObject.FkAppUserId; 
                                }
                            }

                            var userReceverObject = db.AppUsers.Include(x=> x.AppUserProfiles).Include(x=> x.UserDeviceInformations).FirstOrDefault(x => x.Id == createChatStream.FK_ReceiverId);
                            var userSenderObject = db.AppUsers.Include(x => x.AppUserProfiles).Include(x => x.UserDeviceInformations).FirstOrDefault(x => x.Id == createChatStream.FK_SenderId);

                            var DeviceToken = userReceverObject.UserDeviceInformations.FirstOrDefault(x => x.IsEnabled == true)?.DeviceToken;
                            if (threadObject == null)
                            {
                                await db.ChatThreads.AddAsync(new ChatThreads
                                {
                                    Id = id,
                                    ChatMessages = chatmessagesObject,
                                    ChatNotifications = chatNotificationsObject,
                                    FkAdvertisementId = createChatStream.FK_AdvertisementId,
                                    FkServiceId = createChatStream.FK_ServiceId,
                                    FkSellerId = sellerId,
                                    FkUserId = sellerId == createChatStream.FK_SenderId ? createChatStream.FK_ReceiverId : createChatStream.FK_SenderId,
                                    SellerDeleted = false,
                                    UserDeleted = false,
                                    CreatedDate = DateTime.UtcNow,
                                    IsActive = true,
                                    CreatedBy = userId.ToString(),
                                });
                                await db.SaveChangesAsync();
                            }
                            else
                            {
                                id = threadObject.Id;
                                db.Entry(threadObject).State = EntityState.Modified;
                                await db.SaveChangesAsync();

                                chatmessagesObject.ForEach(x => x.FkChatThreadsId = threadObject.Id);
                                await db.ChatMessages.AddRangeAsync(chatmessagesObject);

                                chatNotificationsObject.ForEach(x => x.FkChatThreadId = threadObject.Id);
                                await db.ChatNotifications.AddRangeAsync(chatNotificationsObject);
                                await db.SaveChangesAsync();
                            }
                            if (createChatStream.FK_AdvertisementId != null)
                            {
                                var newObj = new
                                {
                                    FK_AdvertisementId = ASId,
                                    MessageText = createChatStream.MessageText,
                                    FkSellerId = sellerId,
                                    ThreadId = id,
                                    FkSenderId = createChatStream.FK_SenderId,
                                    FkSenderName = userSenderObject.AppUserProfiles.FirstOrDefault(x => x.IsEnabled == true)?.Name,
                                    FkReceiverId = createChatStream.FK_ReceiverId,
                                    FkReceiverName = userReceverObject.AppUserProfiles.FirstOrDefault(x => x.IsEnabled == true)?.Name,
                                    isAdvertisement = true,
                                    Type = (int) ENotificationType.AdvertisementChat
                                };
                                if (DeviceToken != null)
                                {
                                    FCMNotification.Sentnotify(configuration.GetValue<string>("FCM:ServerKey"), configuration.GetValue<string>("FCM:SenderId"), DeviceToken , createChatStream.MessageText, "You Got A New Message", "", newObj,newObj.Type);
                                }
                            }
                            else
                            {
                                var newObj = new
                                {
                                    FK_ServiceId = ASId,
                                    MessageText = createChatStream.MessageText,
                                    FkSellerId = sellerId,
                                    ThreadId = id,
                                    FkSenderId = createChatStream.FK_SenderId,
                                    FkSenderName = userSenderObject.AppUserProfiles.FirstOrDefault(x => x.IsEnabled == true)?.Name,
                                    FkReceiverId = createChatStream.FK_ReceiverId,
                                    FkReceiverName = userReceverObject.AppUserProfiles.FirstOrDefault(x => x.IsEnabled == true)?.Name,
                                    isAdvertisement = false,
                                    Type = (int) ENotificationType.ServiceChat
                                };
                                //advertisementObject.AdvertisementDetails.FirstOrDefault(x => x.FkLanguageId == LanguageId).Title
                                if (DeviceToken != null)
                                {
                                    FCMNotification.Sentnotify(configuration.GetValue<string>("FCM:ServerKey"), configuration.GetValue<string>("FCM:SenderId"), DeviceToken ,  createChatStream.MessageText, "You Got A New Message", "", newObj, newObj.Type);

                                }
                            }
                            await db.SaveChangesAsync();
                            trans.Commit();
                            response = true;
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                throw ex;
                            }
                        }
                }

                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetChatStreamResponse GetChatStream(GetChatStreamRequest chatStreamRequest)
        {
            try
            {
                GetChatStreamResponse response = new GetChatStreamResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.ChatMessages
                        .Include(x=> x.FkChatThreads)
                        .Include(x=> x.FkSender)
                        .ThenInclude(x => (x as AppUsers).AppUserProfiles)
                        .OrderBy(x=> x.CreatedDate)
                        .Where(x => x.IsActive == true && x.FkChatThreadsId == chatStreamRequest.ThreadId)
                        .AsQueryable();

                    

                    var orderQuery = query.Select(x => new ChatStreamViewModel
                    {
                        FK_AdvertisementId = x.FkChatThreads.FkAdvertisementId,
                        FK_ServiceId = x.FkChatThreads.FkServiceId,
                        ThreadId = x.FkChatThreadsId,
                        IsRead = x.IsRead,
                        MessageText = x.MessageText,
                        CreatedOn = x.CreatedDate,
                        FK_SenderId = x.FkSenderId,
                        FK_SenderName = x.FkSender.AppUserProfiles.FirstOrDefault(x=> x.IsEnabled == true).Name,
                        FK_SenderDp = new FileUrlResponce { URL = x.FkSender.AppUserProfiles.FirstOrDefault(x => x.IsEnabled == true).ImageUrl, ThumbnailUrl = x.FkSender.AppUserProfiles.FirstOrDefault(x => x.IsEnabled == true).ImageThumbnailUrl },
                    }).AsQueryable();

                    var UserSenderId = query.FirstOrDefault(x => x.IsActive == true);

                    if (UserSenderId != null)
                    {
                        var userObject = db.AppUsers.Include(x => x.AppUserProfiles).FirstOrDefault(x => x.Id == (UserSenderId.FkChatThreads.FkSellerId == chatStreamRequest.UserId ? UserSenderId.FkChatThreads.FkUserId : UserSenderId.FkChatThreads.FkSellerId) );
                        if (userObject != null)
                        {
                            response.UserId = userObject.Id;
                            response.UserName = userObject.AppUserProfiles.FirstOrDefault(x => x.IsEnabled == true).Name;
                            response.UserDP = new FileUrlResponce { URL = userObject.AppUserProfiles.FirstOrDefault(x => x.IsEnabled == true).ImageUrl, ThumbnailUrl = userObject.AppUserProfiles.FirstOrDefault(x => x.IsEnabled == true).ImageThumbnailUrl };
                        }

                    }

                    response.Skip = chatStreamRequest.Skip;
                    response.Take = chatStreamRequest.Take;
                    response.TotalRecords = orderQuery.Count();
                    if (response.Take > 0)
                    {
                        response.ChatList = orderQuery.Skip(response.Skip).Take(response.Take).ToList();
                    }
                    else
                    {
                        response.ChatList = orderQuery.ToList();
                    }
                    var unReadChatList = db.ChatMessages.Where(x => x.IsActive == true && x.FkSenderId != chatStreamRequest.UserId && x.IsRead == false && x.FkChatThreadsId == chatStreamRequest.ThreadId).ToList();
                    foreach (var item in unReadChatList)
                    {
                        item.IsRead = true;
                        db.SaveChanges();

                    }
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public GetChatThreadResponse GetChatThread(GetChatStreamRequest chatStreamRequest)
        {
            try
            {
                GetChatThreadResponse response = new GetChatThreadResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.ChatThreads
                        .Include(x=> x.ChatMessages)
                        .Include(x=> x.FkSeller)
                        .ThenInclude(x=> (x as AppUsers).AppUserProfiles)
                        .Include(x=> x.FkUser)
                        .ThenInclude(x=> (x as AppUsers).AppUserProfiles)
                        .OrderByDescending(x=> x.CreatedDate)
                        .Where(x => x.IsActive == true && (chatStreamRequest.UserId == null ? true:( x.FkUserId == chatStreamRequest.UserId || x.FkSellerId == chatStreamRequest.UserId)))
                        .AsQueryable();

                    var orderQuery = query.Select(x => new ThreadViewModel
                    {
                        FK_AdvertisementId = x.FkAdvertisementId,
                        FK_ServiceId = x.FkServiceId,
                        Title = x.FkAdvertisementId == null ? x.FkSellerId == null ? "" : x.FkService.BuinessProfileDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == chatStreamRequest.LanguageId).Title == null ? "" : x.FkService.BuinessProfileDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == chatStreamRequest.LanguageId).Title : x.FkAdvertisement.AdvertisementDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == chatStreamRequest.LanguageId).Title == null ? "" : x.FkAdvertisement.AdvertisementDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == chatStreamRequest.LanguageId).Title,
                        ThreadId = x.Id,
                        MessageText = x.ChatMessages.OrderByDescending(x=> x.CreatedDate).FirstOrDefault().MessageText,
                        UnReadCount = x.ChatMessages.Where(x=> x.IsRead == false && x.FkSenderId != chatStreamRequest.UserId).Count(),
                        CreatedOn = x.CreatedDate,
                        FK_SenderId = x.FkSellerId == chatStreamRequest.UserId ? x.FkUserId : x.FkSellerId,
                        FK_SenderName = x.FkSellerId == chatStreamRequest.UserId ? x.FkUser.AppUserProfiles.FirstOrDefault(x => x.IsEnabled == true).Name : x.FkSeller.AppUserProfiles.FirstOrDefault(x=> x.IsEnabled == true).Name,
                        FK_SenderDp = new FileUrlResponce { URL = x.FkSellerId == chatStreamRequest.UserId ? x.FkUser.AppUserProfiles.FirstOrDefault(x => x.IsEnabled == true).ImageUrl : x.FkSeller.AppUserProfiles.FirstOrDefault(x => x.IsEnabled == true).ImageUrl , ThumbnailUrl = x.FkSellerId == chatStreamRequest.UserId ? x.FkUser.AppUserProfiles.FirstOrDefault(x => x.IsEnabled == true).ImageThumbnailUrl : x.FkSeller.AppUserProfiles.FirstOrDefault(x => x.IsEnabled == true).ImageThumbnailUrl },
                    }).AsQueryable();

                    if (!string.IsNullOrEmpty(chatStreamRequest.Search))
                    {
                        var date = new DateTime();
                        var sdate = DateTime.TryParse(chatStreamRequest.Search, out date);
                        int totalCases = -1;
                        var isNumber = Int32.TryParse(chatStreamRequest.Search, out totalCases);

                        orderQuery = orderQuery.Where(
                        x => x.MessageText.ToLower().Contains(chatStreamRequest.Search.ToLower())
                        || x.FK_SenderName.ToLower().Contains(chatStreamRequest.Search.ToLower())
                    );
                    }

                    response.Skip = chatStreamRequest.Skip;
                    response.Take = chatStreamRequest.Take;
                    response.TotalRecords = orderQuery.Count();
                    if (response.Take > 0)
                    {
                        response.ThreadList = orderQuery.Skip(response.Skip).Take(response.Take).ToList();
                    }
                    else
                    {
                        response.ThreadList = orderQuery.ToList();
                    }
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetNotificationsResponse getNotifications(GetNotificationRequest notificationRequest)
        {
            try
            {
                GetNotificationsResponse response = new GetNotificationsResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.AdvertisementNotifications
                        .Include(x => x.FkAdvertisement).ThenInclude(x=> (x as Advertisement).AdvertisementDetails)
                        .Include(x => x.FkService).ThenInclude(x=> (x as UserBusinessProfile).BuinessProfileDetails)
                        .Include(x => x.Receiver).ThenInclude(x => (x as AppUsers).AppUserProfiles)
                        .Include(x => x.AdminReceiver)
                        .OrderByDescending(x => x.CreatedDate)
                        .Where(x => x.ReceiverId == notificationRequest.UserId && x.IsActive == true && x.IsAdminNotify == false)
                        .AsQueryable();



                    var orderQuery = query.Select(x => new GetNotificationsViewModels
                    {
                        Title = x.FkAdvertisementId == null ? x.FkService.BuinessProfileDetails.FirstOrDefault(x=> x.FkLanguageId == notificationRequest.LanguageId).Title : x.FkAdvertisement.AdvertisementDetails.FirstOrDefault(x => x.FkLanguageId == notificationRequest.LanguageId).Title,
                        BodyText = x.BodyText,
                        IsSeen = x.IsSeen,
                        AdvertisementId = x.FkAdvertisementId,
                        ReceiverId = x.ReceiverId,
                        ServiceId = x.FkServiceId,
                        CreatedDate = x.CreatedDate
                    }).AsQueryable();

                    response.Skip = notificationRequest.Skip;
                    response.Take = notificationRequest.Take;
                    response.UnReadCount = orderQuery.Where(x => x.IsSeen == false).Count();
                    response.TotalRecords = orderQuery.Count();
                    if (response.Take > 0)
                    {
                        response.ItemList = orderQuery.Skip(response.Skip).Take(response.Take).ToList();
                        db.AdvertisementNotifications.OrderByDescending(x => x.CreatedDate).Where(x => x.ReceiverId == notificationRequest.UserId && x.IsActive == true && x.IsAdminNotify == false)
                            .Skip(response.Skip).Take(response.Take)
                                    .ForEachAsync(x =>
                                    {
                                        x.IsSeen = true;
                                    });
                        db.SaveChanges();
                    }
                    else
                    {
                        response.ItemList = orderQuery.ToList();
                        db.AdvertisementNotifications.OrderByDescending(x => x.CreatedDate).Where(x => x.ReceiverId == notificationRequest.UserId && x.IsActive == true && x.IsAdminNotify == false)
                                    .ForEachAsync(x =>
                                    {
                                        x.IsSeen = true;
                                    });
                        db.SaveChanges();
                    }
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DashBoardCountResponse getDashBoardCount(Guid UserId)
        {
            DashBoardCountResponse response = new DashBoardCountResponse();
            using (var db = new HilalDbContext())
            {
                try
                {
                    response.NotificationCount = db.AdvertisementNotifications.Where(x => x.IsActive == true && x.ReceiverId == UserId && x.IsSeen == false).Count();
                    response.ChatCount = db.ChatMessages.Include(x=> x.FkChatThreads)
                        .Where(x => x.IsActive == true && x.IsRead == false && x.FkSenderId != UserId /*(x.FkChatThreads.FkSellerId == UserId || x.FkChatThreads.FkUserId == UserId)*/).Count();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


         return response;
        }

        public WhtsappInfoViewModel getWhtsapInfo()
        {
            using (var db = new HilalDbContext())
            {
                try
                {
                    return db.WhatsAppInfo.Where(x=> x.IsActive == true).Select(x => new WhtsappInfoViewModel
                    {
                        Email = x.Email,
                        WhatsappNumber = x.WhatsappNumber,
                        WhatsappUrl = x.WhatsappUrl
                    }).FirstOrDefault();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
    }
}
