﻿using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resource
{
    public partial class MobileResource
    {
        public static string Settings_Label_Title => Get(MobileResourceNames.Settings_Label_Title, "Cài đặt", "Settings");
        public static string Settings_Label_Enable_Cluster => Get(MobileResourceNames.Settings_Label_Enable_Cluster, "Gom phương tiện trên trang giám sát", "Keep track of all vehicles");
        public static string Settings_Label_Show_Notification => Get(MobileResourceNames.Settings_Label_Show_Notification, "Nhận thông báo", "Notifications");

        public static string Settings_Label_SettingGenaral => Get(MobileResourceNames.Settings_Label_SettingGenaral, "Cài đặt chung", "Genaral");
        public static string Settings_Label_SettingAdvanced => Get(MobileResourceNames.Settings_Label_SettingAdvanced, "Cài đặt nâng cao", "Advanced");

        public static string Settings_Label_SettingMylocationMap => Get(MobileResourceNames.Settings_Label_SettingMylocationMap, "Cài đặt vị trí mặc định", "Location Setting");

        public static string Settings_Label_SettingMylocationMap2 => Get(MobileResourceNames.Settings_Label_SettingMylocationMap2, "Thiết lập vị trí mặc định", "Location Setting");

        public static string Settings_Label_SettingThelocation => Get(MobileResourceNames.Settings_Label_SettingThelocation, "Thiết lập vị trí", "Setting the location");

        public static string Settings_Label_SettingReceiveAlert => Get(MobileResourceNames.Settings_Label_SettingReceiveAlert, "Cài đặt nhận cảnh báo", "Alerts Setting");

        public static string Settings_Label_ListAlert => Get(MobileResourceNames.Settings_Label_ListAlert, "Danh sách cảnh báo", "List Alert");

        public static string Settings_Label_ListVehicleAlert => Get(MobileResourceNames.Settings_Label_ListVehicleAlert, "Danh sách xe nhận cảnh báo", "List Vehicle Receive Alert");
    }
}