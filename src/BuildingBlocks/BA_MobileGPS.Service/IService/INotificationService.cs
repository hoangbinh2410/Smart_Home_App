using BA_MobileGPS.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface INotificationService
    {
        Task<ResponseBase<NotificationTypeEnum>> GetNotification(Guid userID);

        Task<ResponseBase<List<NotificationRespone>>> GetListNotification(Guid userID, int pageSize, int pageIndex);

        Task<ResponseBase<NotificationBody>> GetNotificationBody(int id);

        Task<ResponseBase<NoticeDetailRespone>> GetNotificationDetail(int id);

        Task<ResponseBase<bool>> UpdateIsReadNotification(UpdateIsReadRequest updateIsReadRequest);

        Task<ResponseBase<bool>> DeleteNotificationByUser(NoticeDeletedByUserRequest noticeDeletedByUserRequest);

        Task<ResponseBase<bool>> DeleteRangeNotificationByUser(NoticeDeletedRangeByUserRequest noticeDeletedByUserRequest);

        Task<ResponseBase<NoticeDetailRespone>> GetNotificationWhenLogin(AppType appType);

        Task<ResponseBase<NoticeDetailRespone>> GetNotificationAfterLogin(Guid userId);

        Task<bool> SendFeedbackNotification(InsertFeedbackNoticeRequest insertRequest);
    }
}