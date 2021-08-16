using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string Notification_Label_TilePage => Get(MobileResourceNames.Notification_Label_TilePage, "THÔNG BÁO", "Notification");

        public static string Notification_Label_DeleteNotice => Get(MobileResourceNames.Notification_Label_DeleteNotice, "Bạn chắc chắn xóa thông báo?", "Are you sure you delete notifications?");

        public static string Notification_Label_DeleteAllNotice => Get(MobileResourceNames.Notification_Label_DeleteAllNotice, "Bạn chắc chắn xóa tất cả thông báo?", "Are you sure you delete all notifications?");

        public static string Notification_Label_DeleteAllNoticeAction => Get(MobileResourceNames.Notification_Label_DeleteAllNoticeAction, "Xóa tất cả thông báo", "Clear all notifications");

        public static string Notification_Label_DeleteNoticeNotSuccess => Get(MobileResourceNames.Notification_Label_DeleteNoticeNotSuccess, "Xóa thông báo không thành công", "Delete notification failed");
    }
}