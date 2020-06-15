using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resource
{
    public partial class MobileResource
    {
        public static string Menu_Label_Home => Get(MobileResourceNames.Menu_Label_Home, "Trang chủ", "Home");
        public static string Menu_Label_Route => Get(MobileResourceNames.Menu_Label_Home, "Lộ trình", "Route");
        public static string Menu_Label_Settings => Get(MobileResourceNames.Menu_Label_Settings, "Cài đặt", "Settings");
        public static string Menu_Label_About => Get(MobileResourceNames.Menu_Label_About, "Giới thiệu GPS", "About GPS");
        public static string Menu_Label_Feedback => Get(MobileResourceNames.Menu_Label_Feedback, "Góp ý", "Feedback");
        public static string Menu_Label_ChangePassword => Get(MobileResourceNames.Menu_Label_ChangePassword, "Đổi mật khẩu", "Change password");
        public static string Menu_Label_Help => Get(MobileResourceNames.Menu_Label_Help, "Trợ giúp", "Support");
        public static string Menu_Label_Share => Get(MobileResourceNames.Menu_Label_Share, "Chia sẻ", "Share");
        public static string Menu_Label_Rating => Get(MobileResourceNames.Menu_Label_Rating, "Đánh giá", "Rating");
        public static string Menu_Label_Upgrade => Get(MobileResourceNames.Menu_Label_Upgrade, "Nâng cấp phiên bản {0}", "Upgrade Version {0}");
        public static string Menu_Label_Logout => Get(MobileResourceNames.Menu_Label_Logout, "Đăng xuất", "Log out");
        public static string Menu_Label_Log => Get(MobileResourceNames.Menu_Label_Log, "Log bug", "Log bug");
        public static string Menu_Label_MessageWarningLogout => Get(MobileResourceNames.Menu_Label_MessageWarningLogout, "Bạn có chắc chắn đăng xuất khỏi thiết bị?", "Are you sure to log out?");
        public static string Menu_Label_Favorite => Get(MobileResourceNames.Menu_Label_Favorite, "THIẾT LẬP ƯA THÍCH", "Settings favorites");
        public static string Menu_Label_Report => Get(MobileResourceNames.Menu_Label_Report, "Báo cáo", "Reports");
        public static string Menu_Lable_Message => Get(MobileResourceNames.Menu_Lable_Message, "Chọn vào biểu tượng ngôi sao để thiết lập chức năng ưa thích", "Choose star icon to setting up menu favorites");
        public static string Menu_Label_Version => Get(MobileResourceNames.Menu_Label_Version, "Phiên bản :", "Version :");

        public static string Menu_Label_Notification => Get(MobileResourceNames.Menu_Label_Notification, "Thông báo", "Notification");
    }
}