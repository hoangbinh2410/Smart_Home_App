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

        public async Task<BaseResponse<NotificationTypeEnum>> GetNotification(Guid userID)
        {
            BaseResponse<NotificationTypeEnum> result = new BaseResponse<NotificationTypeEnum>();
            try
            {
                string url = $"{ApiUri.GET_NOTIFICATION}?userId={userID}";
                var data = await requestProvider.GetAsync<BaseResponse<NotificationTypeEnum>>(url);

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

        public async Task<ResponseBaseV2<List<NotificationRespone>>> GetListNotification(Guid userID, int pageSize, int pageIndex)
        {
            ResponseBaseV2<List<NotificationRespone>> result = new ResponseBaseV2<List<NotificationRespone>>();
            try
            {
                string url = $"{ApiUri.GET_LIST_NOTIFICATION}?UserId={userID}&PageSize={pageSize}&PageIndex={pageIndex}";
                var data = await requestProvider.GetAsync<ResponseBaseV2<List<NotificationRespone>>>(url);

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

        public async Task<ResponseBaseV2<NotificationBody>> GetNotificationBody(int id)
        {
            ResponseBaseV2<NotificationBody> result = new ResponseBaseV2<NotificationBody>();
            try
            {
                string url = $"{ApiUri.GET_NOTIFICATION_BODY}?noticeId={id}";
                var data = await requestProvider.GetAsync<ResponseBaseV2<NotificationBody>>(url);

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

        public async Task<ResponseBaseV2<NoticeDetailRespone>> GetNotificationDetail(int id)
        {
            ResponseBaseV2<NoticeDetailRespone> result = new ResponseBaseV2<NoticeDetailRespone>();
            try
            {
                string url = $"{ApiUri.GET_NOTIFICATION_DETAIL}?noticeId={id}";
                var data = await requestProvider.GetAsync<ResponseBaseV2<NoticeDetailRespone>>(url);

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

        public async Task<BaseResponse<bool>> UpdateIsReadNotification(UpdateIsReadRequest updateIsReadRequest)
        {
            BaseResponse<bool> result = new BaseResponse<bool>();
            try
            {
                var data = await requestProvider.PostAsync<UpdateIsReadRequest, BaseResponse<bool>>(ApiUri.POST_UPDATEISREAD_NOTIFICATION, updateIsReadRequest);

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

        public async Task<BaseResponse<bool>> DeleteNotificationByUser(NoticeDeletedByUserRequest noticeDeletedByUserRequest)
        {
            BaseResponse<bool> result = new BaseResponse<bool>();
            try
            {
                var data = await requestProvider.PostAsync<NoticeDeletedByUserRequest, BaseResponse<bool>>(ApiUri.POST_DELETE_NOTIFICATION_BYUSER, noticeDeletedByUserRequest);

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

        public async Task<BaseResponse<bool>> DeleteRangeNotificationByUser(NoticeDeletedRangeByUserRequest noticeDeletedByUserRequest)
        {
            BaseResponse<bool> result = new BaseResponse<bool>();
            try
            {
                var data = await requestProvider.PostAsync<NoticeDeletedRangeByUserRequest, BaseResponse<bool>>(ApiUri.POST_DELETERANGE_NOTIFICATION_BYUSER, noticeDeletedByUserRequest);

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

        public async Task<ResponseBaseV2<NoticeDetailRespone>> GetNotificationWhenLogin(AppType appType)
        {
            ResponseBaseV2<NoticeDetailRespone> result = new ResponseBaseV2<NoticeDetailRespone>();
            try
            {
                int appID = (int)appType;

                var url = string.Format(ApiUri.GET_NOTIFICATION_WHEN_LOGIN + "?appID={0}", appID);

                var data = await requestProvider.GetAsync<ResponseBaseV2<NoticeDetailRespone>>(url);

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

        public async Task<ResponseBaseV2<NoticeDetailRespone>> GetNotificationAfterLogin(Guid userId)
        {
            ResponseBaseV2<NoticeDetailRespone> result = new ResponseBaseV2<NoticeDetailRespone>();
            try
            {
                var url = string.Format(ApiUri.GET_NOTIFICATION_AFTER_LOGIN + "?userId={0}", userId);

                var data = await requestProvider.GetAsync<ResponseBaseV2<NoticeDetailRespone>>(url);

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

        public async Task<BaseResponse<bool>> SendFeedbackNotification(InsertFeedbackNoticeRequest insertRequest)
        {
            BaseResponse<bool> result = new BaseResponse<bool>();
            try
            {
                var data = await requestProvider.PostAsync<InsertFeedbackNoticeRequest, BaseResponse<bool>>(ApiUri.POST_INSERT_FEEDBACK_NOTIFICATION_BYUSER, insertRequest);

                if (data != null && data.Data)
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
    }
}