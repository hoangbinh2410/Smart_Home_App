using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string Menu_Label_Favorite => Get(MobileResourceNames.Menu_Label_Favorite, "THIẾT LẬP ƯA THÍCH", "Settings favorites");

        public static string Menu_TabItem_Home => Get(MobileResourceNames.Menu_TabItem_Home, "Trang chủ", "Home");
        public static string Menu_TabItem_Vehicle => Get(MobileResourceNames.Menu_TabItem_Vehicle, "Phương tiện", "Vehicles");
        public static string Menu_TabItem_Monitoring => Get(MobileResourceNames.Menu_TabItem_Monitoring, "Giám sát", "Monitoring");
        public static string Menu_TabItem_Voyage => Get(MobileResourceNames.Menu_TabItem_Voyage, "Hải trình", "Voyages");
        public static string Menu_TabItem_Route => Get(MobileResourceNames.Menu_TabItem_Voyage, "Lộ trình", "Route");
        public static string Menu_TabItem_Account => Get(MobileResourceNames.Menu_TabItem_Account, "Tài khoản", "Account");
        public static string Home_Label_Highlight => Get(MobileResourceNames.Home_Label_Highlight, "NỔI BẬT", "HIGHLIGHTS");
        public static string Home_Label_Features => Get(MobileResourceNames.Home_Label_Features, "TIỆN ÍCH", "FEATURES");
    }
}