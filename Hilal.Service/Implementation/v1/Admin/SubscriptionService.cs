using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Hilal.Common;
using Hilal.Data.Context;
using Hilal.Data.DTOs;
using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using Hilal.Service.Interface.v1.Admin;
using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Enum;
using Hilal.DataViewModel.Request.App.v1;

namespace Hilal.Service.Implementation.v1.Admin
{
    public class SubscriptionService : ISubscriptionService
    {
        public GetSubscriptionResponse GetSubscription(ListGeneralModel Subscription)
        {
            try
            {
                GetSubscriptionResponse response = new GetSubscriptionResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.Subsciption
                        .Where(x => x.IsActive == true)
                        .Select(x => new GetSubscription
                        {
                            Id = x.Id,
                            ValidityDays = x.ValidityDays,
                            ValidityPosts = x.ValidityPosts,
                            IsDisplayed = x.IsDisplayed,
                            DisplayValidityDays = x.SubscriptionDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == Subscription.LanguageId).DisplayValidityDays,
                            Name = x.SubscriptionDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == Subscription.LanguageId).Name,
                            Amount = x.Amount, 
                        })
                        .AsQueryable();

                    if (!string.IsNullOrEmpty(Subscription.Search))
                    {
                        var date = new DateTime();
                        var sdate = DateTime.TryParse(Subscription.Search, out date);
                        int totalCases = -1;
                        var isNumber = Int32.TryParse(Subscription.Search, out totalCases);

                        query = query.Where(
                        x => x.Name.ToLower().Contains(Subscription.Search.ToLower())
                        || x.DisplayValidityDays.ToLower().Contains(Subscription.Search.ToLower())
                    );
                    }

                    var orderedQuery = query.OrderByDescending(x => x.Name);
                    switch (Subscription.SortIndex)
                    {
                        case 0:
                            orderedQuery = Subscription.SortBy == "desc" ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                            break;
                        case 2:
                            orderedQuery = Subscription.SortBy == "desc" ? query.OrderByDescending(x => x.DisplayValidityDays) : query.OrderBy(x => x.DisplayValidityDays);
                            break; 
                        case 3:
                            orderedQuery = Subscription.SortBy == "desc" ? query.OrderByDescending(x => x.Amount) : query.OrderBy(x => x.Amount);
                            break;
                    }


                    response.PageSize = Subscription.PageSize;
                    response.Page = Subscription.Page;
                    response.TotalRecords = orderedQuery.Count();
                    if (response.PageSize > 0)
                    {
                        response.ItemList = orderedQuery.Skip(response.Page).Take(Subscription.PageSize).ToList();
                    }
                    else
                    {
                        response.ItemList = orderedQuery.ToList();
                    }
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SaveSubscription(CreateSubscriptionRequest SubscriptionRequest, Guid userId)
        {
            try
            {
                bool response = false;

                List<SubscriptionDetails> informations = new List<SubscriptionDetails>();

                foreach (var information in SubscriptionRequest.SubscriptionInformations)
                {
                    informations.Add(new SubscriptionDetails
                    {
                        Id = SystemGlobal.GetId(),
                        FkLanguageId = information.LanguageId,
                        IsActive = true,
                        CreatedBy = userId.ToString(),
                        CreatedOn = DateTime.UtcNow,
                        CreatedDate = DateTime.UtcNow,
                        DisplayValidityDays = information.DisplayValidityDays,
                        Name = information.Name 

                    });
                }

                if (SubscriptionRequest.Id == null)
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                await db.Subsciption.AddAsync(new Subsciption
                                {
                                    Id = SystemGlobal.GetId(),
                                    SubscriptionDetails = informations,
                                    CreatedDate = DateTime.UtcNow,
                                    IsActive = true,
                                    CreatedBy = userId.ToString(),
                                    CreatedOn = DateTime.UtcNow,
                                    ValidityDays = SubscriptionRequest.ValidityDays,
                                    ValidityPosts = SubscriptionRequest.ValidityPosts,
                                    Amount = SubscriptionRequest.Amount,
                                    IsBlock =  false,
                                    IsDisplayed = true,
                                    StartDate = DateTime.UtcNow,
                                    NewAmount = SubscriptionRequest.Amount,
                                });

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
                }
                else
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                await db.SubscriptionDetails
                                    .Where(x => x.IsActive ==  true && x.FkSubscriptionId.Equals(SubscriptionRequest.Id))
                                    .ForEachAsync(x =>
                                    {
                                        x.IsActive = false;
                                        x.UpdatedBy = userId.ToString();
                                        x.UpdatedDate = DateTime.UtcNow;
                                    });

                                await db.SaveChangesAsync();

                                //add Newer

                                var category = db.Subsciption.FirstOrDefault(x => x.Id.Equals(SubscriptionRequest.Id));
                                
                                category.ValidityPosts = SubscriptionRequest.ValidityPosts;
                                category.ValidityDays = SubscriptionRequest.ValidityDays;
                                category.IsDisplayed  = SubscriptionRequest.IsDisplayed;
                                category.UpdatedBy = userId.ToString();
                                category.UpdatedDate = DateTime.UtcNow;

                                db.Entry(category).State = EntityState.Modified;
                                await db.SaveChangesAsync();

                                informations.ForEach(x => x.FkSubscriptionId = category.Id);
                                await db.SubscriptionDetails.AddRangeAsync(informations);

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
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CreateSubscriptionRequest GetEditSubscription(Guid id, Guid userId)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.Subsciption
                        .Where(x => x.Id.Equals(id))
                        .Select(x => new CreateSubscriptionRequest
                        {
                            Id = x.Id,
                            ValidityDays = x.ValidityDays,
                            ValidityPosts = x.ValidityPosts,
                            IsDisplayed = x.IsDisplayed,
                            Amount = x.Amount,
                            SubscriptionInformations = x.SubscriptionDetails.Where(y => y.IsActive == true).Select(y => new SubscriptionDetailRequest
                            {
                                Id = y.Id,
                                LanguageId = y.FkLanguageId,
                                DisplayValidityDays = y.DisplayValidityDays,
                                FKMasterId = y.FkSubscriptionId,
                                Name = y.Name
                            }).ToList()
                        })
                        .FirstOrDefault() ?? new CreateSubscriptionRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ControllSubscriptionActivation(Guid SubscriptionId, bool activation, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.Subsciption.FirstOrDefault(x => x.Id.Equals(SubscriptionId));

                    if (category == null) throw new Exception("SubscriptionId Doesn't Exists");

                    category.IsActive = activation;
                    category.UpdatedBy = userId.ToString();
                    category.UpdatedDate = DateTime.UtcNow;

                    db.Entry(category).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    response = true;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetSubscriptionList> GetSubscriptionList(int LanguageId)
        {
            try
            {
                List<GetSubscriptionList> response = new List<GetSubscriptionList>();

                using (var db = new HilalDbContext())
                {
                    response = db.Subsciption
                        .Where(x => x.IsActive == true && x.IsDisplayed == true)
                        .Select(x => new GetSubscriptionList
                        {
                            Id = x.Id,
                            ValidityDays = x.ValidityDays,
                            ValidateDate = x.CreatedDate.AddDays(x.ValidityDays),
                            DisplayValidityDays = x.SubscriptionDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).DisplayValidityDays,
                            Name = x.SubscriptionDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                            Amount = x.Amount,
                        }).ToList();
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> CreateOrderRequest(CreatePaymentOrderRequest createPaymentOrderRequest, Guid userId)
        {
            using (var db = new HilalDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var resultString = "";
                        var alreadySubscribe = db.AppUserSubscription.Where(x => x.IsActive == true && x.FkSubscribedPlanId == createPaymentOrderRequest.planId
                            && x.FkUserId == userId && DateTime.UtcNow <= x.CreatedOn.AddDays(x.FkSubscribedPlan.ValidityDays)).FirstOrDefault();

                        if (alreadySubscribe == null)
                        {
                            createPaymentOrderRequest.Amount = createPaymentOrderRequest.Amount * 100;
                            resultString = NetworkGlobal.SinglePaymentMethod(createPaymentOrderRequest);
                            if (!String.IsNullOrEmpty(resultString))
                            {
                                PaymentHistory paymentHistory = new PaymentHistory
                                {
                                    Id = SystemGlobal.GetId(),
                                    Address = createPaymentOrderRequest.Address,
                                    Amount = createPaymentOrderRequest.Amount,
                                    Email = createPaymentOrderRequest.EmailAddress,
                                    FirstName = createPaymentOrderRequest.firstName,
                                    LastName = createPaymentOrderRequest.lastName,
                                    IsSubscription = true,
                                    AppUserId = userId,
                                    IsActive = true,
                                    IsPaymentDone = false,
                                    CreatedDate = DateTime.UtcNow,
                                    CreatedBy = userId.ToString(),
                                };
                                db.PaymentHistory.Add(paymentHistory);
                                db.SaveChanges();

                                await db.PaymentHistory
                                        .Where(x => x.IsActive == true && x.IsSubscription == true && x.AppUserId.Equals(userId))
                                        .ForEachAsync(x =>
                                        {
                                            x.IsActive = false;
                                            x.UpdatedBy = userId.ToString();
                                            x.UpdatedDate = DateTime.UtcNow;
                                        });
                                await db.SaveChangesAsync();
                                trans.Commit();
                            }
                        }
                        else
                        {
                            resultString = "AlreadySubscribed";
                        }
                        return resultString;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<bool> createSubscrption(string orderRef, Guid planId, Guid userId)
        {
            using (var db = new HilalDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var checkStatus = NetworkGlobal.checkOrderStatus(orderRef);
                        bool res = false;
                        var alreadySubscribe = db.AppUserSubscription.Where(x => x.IsActive == true && x.FkSubscribedPlanId == planId
                            && x.FkUserId == userId && DateTime.UtcNow <= x.CreatedOn.AddDays(x.FkSubscribedPlan.ValidityDays)).FirstOrDefault();
                        if (checkStatus == "CAPTURED" || checkStatus == "SUCCESS")
                        {
                            var userModel = db.AppUsers.FirstOrDefault(x => x.Id == userId);

                            if (alreadySubscribe == null)
                            {
                                await db.AppUserSubscription.Where(x => x.IsActive == true && x.FkUserId.Equals(userId))
                                                            .ForEachAsync(x =>
                                                            {
                                                                x.IsActive = false;
                                                                x.UpdatedBy = userId.ToString();
                                                                x.UpdatedDate = DateTime.UtcNow;
                                                            });
                                await db.SaveChangesAsync();

                                await db.AppUserSubscription.AddAsync(new AppUserSubscription
                                {
                                    Id = SystemGlobal.GetId(),
                                    FkSubscribedPlanId = planId,
                                    FkUserId = userId,
                                    IsActive = true,
                                    StartDate = DateTime.UtcNow,
                                    CreatedDate = DateTime.UtcNow,
                                    CreatedOn = DateTime.UtcNow,
                                    CreatedBy = userId.ToString(),
                                });
                                await db.SaveChangesAsync();

                                if (userModel != null)
                                {
                                    userModel.IsSubscribed = true;
                                    await db.SaveChangesAsync();
                                }
                                res = true;
                            }
                        }
                        trans.Commit();
                        return res;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public bool checkSubscription(Guid userId)
        {
            using (var db = new HilalDbContext())
            {
                try
                {
                    bool res = false;
                    var alreadySubscribe = db.AppUserSubscription.Where(x => x.IsActive == true &&  x.FkUserId == userId && DateTime.UtcNow <= x.CreatedOn.AddDays(x.FkSubscribedPlan.ValidityDays)).FirstOrDefault();
                    var userModel = db.AppUsers.FirstOrDefault(x => x.Id == userId);
                    if (alreadySubscribe != null)
                    {
                        res = true;
                        userModel.IsSubscribed = true;
                    }
                    else
                    {
                        db.AppUserSubscription
                        .Where(x => x.IsActive == true && x.FkUserId == userId).ForEachAsync(x => x.IsActive = false);
                        db.SaveChangesAsync();
                        if (userModel.IsSubscribed == true)
                        {
                            userModel.IsSubscribed = false;
                            userModel.IsSubscribedExpired = true;
                        }
                    }
                    return res;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
