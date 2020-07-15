using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resource
{
    public partial class MobileResource
    {
        public static string AlertConfig_Label_Alter => Get(MobileResourceNames.AlertConfig_Label_Alter, "Thông báo", "Notification");

        public static string AlertConfig_Label_SendSuccess => Get(MobileResourceNames.AlertConfig_Label_SendSuccess, "Lưu cấu hình thành công", "Configuration saved successfully");

        public static string AlertConfig_Label_SendFail => Get(MobileResourceNames.AlertConfig_Label_SendFail, "Lưu cấu hình không thành công", "Configuration save failed");

        public static string AlertConfig_Label_Choose_Time => Get(MobileResourceNames.AlertConfig_Label_Choose_Time, "Chọn giờ", "Choose a time");

        public static string AlertConfig_Label_Warning_Alert => Get(MobileResourceNames.AlertConfig_Label_Warning_Alert, "Bạn phải lựa chọn ít nhất 1 cảnh báo để cấu hình", "You must select at least 1 alert to configure");

        public static string AlertConfig_Label_Warning_Vehicle => Get(MobileResourceNames.AlertConfig_Label_Warning_Vehicle, "Bạn phải lựa chọn ít nhất 1 xe để cấu hình", "You must select at least 1 vehicle to configure");

        public static string AlertConfig_Label_Warning_Time => Get(MobileResourceNames.AlertConfig_Label_Warning_Time, "Thời gian từ giờ không được nhỏ hơn thời gian đến giờ", "Time from now must not be less than time to time");

        public static string AlertConfig_Label_All_Warnings => Get(MobileResourceNames.AlertConfig_Label_All_Warnings, "Tất cả các cảnh báo", "All warnings");

        public static string AlertConfig_Label_All_Warnings_Description => Get(MobileResourceNames.AlertConfig_Label_All_Warnings_Description, "Bạn chọn tất cả các cảnh báo cùng cấu hình", "You select all alerts with the same configuration");

        public static string AlertConfig_Label_NextStep => Get(MobileResourceNames.AlertConfig_Label_NextStep, "Tiếp tục", "Next");

        public static string AlertConfig_Label_All_Vehicle => Get(MobileResourceNames.AlertConfig_Label_All_Vehicle, "Tất cả danh sách xe", "All list vehicle");

        public static string AlertConfig_Label_All_Vehicle_Description => Get(MobileResourceNames.AlertConfig_Label_All_Vehicle_Description, "Bạn chọn tất cả các xe có cảnh báo cùng cung giờ", "You choose all vehicles with the same hourly warning");

        public static string AlertConfig_Label_All_Vehicle_Not_Configured => Get(MobileResourceNames.AlertConfig_Label_All_Vehicle_Not_Configured, "Xe chưa được cấu hình", "Vehicle is not configured");

        public static string AlertConfig_Label_About_Time => Get(MobileResourceNames.AlertConfig_Label_About_Time, "Trong khoảng thời gian", "About time");

        public static string AlertConfig_Label_Start_Time => Get(MobileResourceNames.AlertConfig_Label_Start_Time, "Từ giờ", "Start time");

        public static string AlertConfig_Label_End_Time => Get(MobileResourceNames.AlertConfig_Label_End_Time, "Đến giờ", "End time");

        public static string AlertConfig_Label_Choose_Time_Zone => Get(MobileResourceNames.AlertConfig_Label_Choose_Time_Zone, "Chọn đúng múi giờ", "Choose the right time zone");

        public static string AlertConfig_Label_Choose_All_Time_Zone => Get(MobileResourceNames.AlertConfig_Label_Choose_All_Time_Zone, "Tất cả", "Choose all");

        public static string AlertConfig_Label_Save_Configuration => Get(MobileResourceNames.AlertConfig_Label_Save_Configuration, "Lưu cấu hình", "Save the configuration");
    }
}