using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resource
{
    public partial class MobileResource
    {
        public static string Settings_Label_Title => Get(MobileResourceNames.Settings_Label_Title, "Cài đặt", "Settings");
        public static string Settings_Label_Enable_Cluster => Get(MobileResourceNames.Settings_Label_Enable_Cluster, "Gom phương tiện trên trang giám sát", "Enable Cluster on tracking page");
        public static string Settings_Label_Show_Notification => Get(MobileResourceNames.Settings_Label_Show_Notification, "Nhận cảnh báo notification", "Receive notification alerts");

        public static string Settings_Label_SettingGenaral => Get(MobileResourceNames.Settings_Label_SettingGenaral, "Cài đặt chung", "Genaral");
        public static string Settings_Label_SettingAdvanced => Get(MobileResourceNames.Settings_Label_SettingAdvanced, "Cài đặt nâng cao", "Advanced");

        public static string Settings_Label_SettingMylocationMap => Get(MobileResourceNames.Settings_Label_SettingMylocationMap, "Cài đặt vị trí mặc định", "Setting location map");

        public static string Settings_Label_SettingMylocationMap2 => Get(MobileResourceNames.Settings_Label_SettingMylocationMap2, "Thiết lập vị trí mặc định", "Setting location map");

        public static string Settings_Label_SettingThelocation => Get(MobileResourceNames.Settings_Label_SettingThelocation, "THIẾT LẬP VỊ TRÍ", "SETTING THE LOCATION");
    }
}