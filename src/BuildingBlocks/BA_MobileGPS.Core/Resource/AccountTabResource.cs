using BA_MobileGPS.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Core.Resource
{
    public partial class MobileResource
    {
        public static string AccountTab_Label_Notification => Get(MobileResourceNames.AccountTab_Label_Notification, "Thông báo", "Notification");
        public static string AccountTab_Label_ChangePassword => Get(MobileResourceNames.AccountTab_Label_ChangePassword, "Đổi mật khẩu", "Change password");
        public static string AccountTab_Label_CustomerSupport => Get(MobileResourceNames.AccountTab_Label_CustomerSupport, "Hỗ trợ Khách hàng", "Customer support");
        public static string AccountTab_Label_DeviceManual => Get(MobileResourceNames.AccountTab_Label_DeviceManual, "Hướng dẫn sử dụng thiết bị", "Device manual");
        public static string AccountTab_Label_BAGPS_Introduce => Get(MobileResourceNames.AccountTab_Label_BAGPS_Introduce, "Giới thiệu BA GPS", "BA GPS introduction");
        public static string AccountTab_Label_Share => Get(MobileResourceNames.AccountTab_Label_Share, "Chia sẻ", "Share");
        public static string AccountTab_Label_Rating => Get(MobileResourceNames.AccountTab_Label_Rating, "Đánh giá", "Rating");
        public static string AccountTab_Label_Setting => Get(MobileResourceNames.AccountTab_Label_Setting, "Cài đặt", "Settings");
        public static string AccountTab_Label_Logout => Get(MobileResourceNames.AccountTab_Label_Logout, "Đăng xuất", "Log out");
        public static string AccountTab_Label_MessageWarningLogout => Get(MobileResourceNames.AccountTab_Label_MessageWarningLogout, "Bạn có chắc chắn đăng xuất khỏi thiết bị?", "Are you sure to log out?");

        public static string AccountTab_Label_MyInformation => Get(MobileResourceNames.AccountTab_Label_MyInformation, "Thông tin của tôi", "Personal Information");
    }
}
