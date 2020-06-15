using BA_MobileGPS.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface INotificationService
    {
        Task<BaseResponse<NotificationTypeEnum>> GetNotification(Guid userID);

        Task<BaseResponse<List<NotificationRespone>>> GetListNotification(Guid userID, int pageSize, int pageIndex, string culture);

        Task<BaseResponse<NotificationBody>> GetNotificationBody(int id, string culture);

        Task<BaseResponse<NotificationDetailRespone>> GetNotificationDetail(int id, string culture);

        Task<BaseResponse<bool>> UpdateIsReadNotification(UpdateIsReadRequest updateIsReadRequest);

        Task<BaseResponse<bool>> DeleteNotificationByUser(NoticeDeletedByUserRequest noticeDeletedByUserRequest);

        Task<BaseResponse<bool>> DeleteRangeNotificationByUser(NoticeDeletedRangeByUserRequest noticeDeletedByUserRequest);

        Task<BaseResponse<NotificationWhenLoginRespone>> GetNotificationWhenLogin(AppType appType);

        Task<BaseResponse<NotificationAfterLoginRespone>> GetNotificationAfterLogin(Guid userId);

        Task<BaseResponse<bool>> SendFeedbackNotification(InsertFeedbackNoticeRequest insertRequest);
    }
}