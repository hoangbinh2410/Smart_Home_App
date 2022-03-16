using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IRequestProvider requestProvider;

        public NotificationService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<ResponseBase<NotificationTypeEnum>> GetNotification(Guid userID)
        {
            ResponseBase<NotificationTypeEnum> result = new ResponseBase<NotificationTypeEnum>();
            //try
            //{
            //    string url = $"{ApiUri.GET_NOTIFICATION}?userId={userID}";
            //    var data = await requestProvider.GetAsync<BaseResponse<NotificationTypeEnum>>(url);

            //    if (data != null)
            //    {
            //        result = data;
            //    }
            //}
            //catch (Exception e)
            //{
            //    Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            //}
            return result;
        }

        public async Task<ResponseBase<List<NotificationRespone>>> GetListNotification(Guid userID, int pageSize, int pageIndex)
        {
            ResponseBase<List<NotificationRespone>> result = new ResponseBase<List<NotificationRespone>>();
            try
            {
                string url = $"{ApiUri.GET_LIST_NOTIFICATION}?UserId={userID}&PageSize={pageSize}&PageIndex={pageIndex}";
                var data = await requestProvider.GetAsync<ResponseBase<List<NotificationRespone>>>(url);

                if (data != null)
                {
                    result = data;
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }

        public async Task<ResponseBase<NotificationBody>> GetNotificationBody(int id)
        {
            ResponseBase<NotificationBody> result = new ResponseBase<NotificationBody>();
            try
            {
                string url = $"{ApiUri.GET_NOTIFICATION_BODY}?Id={id}";
                var data = await requestProvider.GetAsync<ResponseBase<NotificationBody>>(url);

                if (data != null)
                {
                    result = data;
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }

        public async Task<ResponseBase<NoticeDetailRespone>> GetNotificationDetail(int id)
        {
            ResponseBase<NoticeDetailRespone> result = new ResponseBase<NoticeDetailRespone>();
            try
            {
                string url = $"{ApiUri.GET_NOTIFICATION_DETAIL}?Id={id}";
                var data = await requestProvider.GetAsync<ResponseBase<NoticeDetailRespone>>(url);

                if (data != null)
                {
                    result = data;
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }

        public async Task<ResponseBase<bool>> UpdateIsReadNotification(UpdateIsReadRequest updateIsReadRequest)
        {
            ResponseBase<bool> result = new ResponseBase<bool>();
            try
            {
                var data = await requestProvider.PostAsync<UpdateIsReadRequest, ResponseBase<bool>>(ApiUri.POST_UPDATEISREAD_NOTIFICATION, updateIsReadRequest);

                if (data != null)
                {
                    result = data;
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }

        public async Task<ResponseBase<bool>> DeleteNotificationByUser(NoticeDeletedByUserRequest noticeDeletedByUserRequest)
        {
            ResponseBase<bool> result = new ResponseBase<bool>();
            try
            {
                var data = await requestProvider.PostAsync<NoticeDeletedByUserRequest, ResponseBase<bool>>(ApiUri.POST_DELETE_NOTIFICATION_BYUSER, noticeDeletedByUserRequest);

                if (data != null)
                {
                    result = data;
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }

        public async Task<ResponseBase<bool>> DeleteRangeNotificationByUser(NoticeDeletedRangeByUserRequest noticeDeletedByUserRequest)
        {
            ResponseBase<bool> result = new ResponseBase<bool>();
            try
            {
                var data = await requestProvider.PostAsync<NoticeDeletedRangeByUserRequest, ResponseBase<bool>>(ApiUri.POST_DELETERANGE_NOTIFICATION_BYUSER, noticeDeletedByUserRequest);

                if (data != null)
                {
                    result = data;
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }

        public async Task<ResponseBase<NoticeDetailRespone>> GetNotificationWhenLogin(AppType appType)
        {
            ResponseBase<NoticeDetailRespone> result = new ResponseBase<NoticeDetailRespone>();
            //try
            //{
            //    int appID = (int)appType;

            //    var url = string.Format(ApiUri.GET_NOTIFICATION_WHEN_LOGIN + "?appID={0}", appID);

            //    var data = await requestProvider.GetAsync<ResponseBaseV2<NoticeDetailRespone>>(url);

            //    if (data != null)
            //    {
            //        result = data;
            //    }
            //}
            //catch (Exception e)
            //{
            //    Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            //}
            return result;
        }

        public async Task<ResponseBase<NoticeDetailRespone>> GetNotificationAfterLogin(Guid userId)
        {
            ResponseBase<NoticeDetailRespone> result = new ResponseBase<NoticeDetailRespone>();
            try
            {
                var url = string.Format(ApiUri.GET_NOTIFICATION_AFTER_LOGIN + "?userId={0}", userId);

                var data = await requestProvider.GetAsync<ResponseBase<NoticeDetailRespone>>(url);

                if (data != null)
                {
                    result = data;
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }

        public async Task<bool> SendFeedbackNotification(InsertFeedbackNoticeRequest insertRequest)
        {
            bool result = false;
            try
            {
                var data = await requestProvider.PostAsync<InsertFeedbackNoticeRequest, ResponseBase<bool>>(ApiUri.POST_INSERT_FEEDBACK_NOTIFICATION_BYUSER, insertRequest);

                if (data != null && data.Data)
                {
                    result = data.Data;
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }
    }
}