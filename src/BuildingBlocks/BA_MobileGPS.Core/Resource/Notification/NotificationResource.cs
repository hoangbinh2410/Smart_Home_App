using BA_MobileGPS.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Core.Resource
{
    public partial class MobileResource
    {
        public static string Notification_Label_TilePage => Get(MobileResourceNames.Notification_Label_TilePage, "THÔNG BÁO", "Notification");

        public static string Notification_Label_DeleteAllNotice => Get(MobileResourceNames.Notification_Label_DeleteAllNotice, "Bạn chắc chắn xóa tất cả thông báo?", "Are you sure you delete all notifications?");

        public static string Notification_Label_DeleteAllNoticeAction => Get(MobileResourceNames.Notification_Label_DeleteAllNoticeAction, "Xóa tất cả thông báo", "Clear all notifications");

        public static string Notification_Label_DeleteNoticeNotSuccess => Get(MobileResourceNames.Notification_Label_DeleteNoticeNotSuccess, "Xóa thông báo không thành công", "Delete notification failed");
    }
}
