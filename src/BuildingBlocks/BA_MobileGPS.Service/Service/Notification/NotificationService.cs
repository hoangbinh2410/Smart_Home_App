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

        public async Task<BaseResponse<List<NotificationRespone>>> GetListNotification(Guid userID, int pageSize, int pageIndex, string culture)
        {
            BaseResponse<List<NotificationRespone>> result = new BaseResponse<List<NotificationRespone>>();
            try
            {
                string url = $"{ApiUri.GET_LIST_NOTIFICATION}?UserId={userID}&PageSize={pageSize}&PageIndex={pageIndex}&Culture={culture}";
                var data = await requestProvider.GetAsync<BaseResponse<List<NotificationRespone>>>(url);

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

        public async Task<BaseResponse<NotificationBody>> GetNotificationBody(int id, string culture)
        {
            BaseResponse<NotificationBody> result = new BaseResponse<NotificationBody>();
            try
            {
                string url = $"{ApiUri.GET_NOTIFICATION_BODY}?FK_NoticeContentID={id}&Culture={culture}";
                var data = await requestProvider.GetAsync<BaseResponse<NotificationBody>>(url);

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

        public async Task<BaseResponse<NotificationDetailRespone>> GetNotificationDetail(int id, string culture)
        {
            BaseResponse<NotificationDetailRespone> result = new BaseResponse<NotificationDetailRespone>();
            try
            {
                string url = $"{ApiUri.GET_NOTIFICATION_DETAIL}?FK_NoticeContentID={id}&Culture={culture}";
                var data = await requestProvider.GetAsync<BaseResponse<NotificationDetailRespone>>(url);

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

        public async Task<BaseResponse<NotificationWhenLoginRespone>> GetNotificationWhenLogin(AppType appType)
        {
            BaseResponse<NotificationWhenLoginRespone> result = new BaseResponse<NotificationWhenLoginRespone>();
            try
            {
                int appID = (int)appType;

                var url = string.Format(ApiUri.GET_NOTIFICATION_WHEN_LOGIN + "?MobileAppTypesId={0}", appID);

                var data = await requestProvider.GetAsync<BaseResponse<NotificationWhenLoginRespone>>(url);

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

        public async Task<BaseResponse<NotificationAfterLoginRespone>> GetNotificationAfterLogin(Guid userId)
        {
            BaseResponse<NotificationAfterLoginRespone> result = new BaseResponse<NotificationAfterLoginRespone>();
            try
            {
                var url = string.Format(ApiUri.GET_NOTIFICATION_AFTER_LOGIN + "?UserId={0}", userId);

                var data = await requestProvider.GetAsync<BaseResponse<NotificationAfterLoginRespone>>(url);

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