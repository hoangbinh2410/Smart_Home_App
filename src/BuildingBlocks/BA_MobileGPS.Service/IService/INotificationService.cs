using BA_MobileGPS.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface INotificationService
    {
        Task<BaseResponse<NotificationTypeEnum>> GetNotification(Guid userID);

        Task<ResponseBaseV2<List<NotificationRespone>>> GetListNotification(Guid userID, int pageSize, int pageIndex);

        Task<ResponseBaseV2<NotificationBody>> GetNotificationBody(int id);

        Task<ResponseBaseV2<NoticeDetailRespone>> GetNotificationDetail(int id);

        Task<ResponseBaseV2<bool>> UpdateIsReadNotification(UpdateIsReadRequest updateIsReadRequest);

        Task<ResponseBaseV2<bool>> DeleteNotificationByUser(NoticeDeletedByUserRequest noticeDeletedByUserRequest);

        Task<ResponseBaseV2<bool>> DeleteRangeNotificationByUser(NoticeDeletedRangeByUserRequest noticeDeletedByUserRequest);

        Task<ResponseBaseV2<NoticeDetailRespone>> GetNotificationWhenLogin(AppType appType);

        Task<ResponseBaseV2<NoticeDetailRespone>> GetNotificationAfterLogin(Guid userId);

        Task<BaseResponse<bool>> SendFeedbackNotification(InsertFeedbackNoticeRequest insertRequest);
    }
}