using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string Alert_Label_TilePage => Get(MobileResourceNames.Alert_Label_TilePage, "Danh sách cảnh báo", "Alerts");

        public static string Alert_Label_TileHandlePage => Get(MobileResourceNames.Alert_Label_TileHandlePage, "Xử lý cảnh báo", "Alert Handle");

        public static string Alert_Label_NotFound => Get(MobileResourceNames.Alert_Label_NotFound, "Không có cảnh báo nào", "Not found alerts");

        public static string Alert_Label_Handler => Get(MobileResourceNames.Alert_Label_Handler, "Người xử lý", "Handler");

        public static string Alert_Label_Time => Get(MobileResourceNames.Alert_Label_Time, "Thời gian", "Time");

        public static string Alert_Label_AlertType => Get(MobileResourceNames.Alert_Label_AlertType, "Loại cảnh báo", "Types of Alerts");

        public static string Alert_Label_SearchVehicle => Get(MobileResourceNames.Alert_Label_SearchVehicle, "Phương tiện", "Vehicle");

        public static string Alert_Label_AlertRead => Get(MobileResourceNames.Alert_Label_AlertRead, "Đã đọc, đóng cảnh báo", "Read and close");

        public static string Alert_Label_AlertProcessed => Get(MobileResourceNames.Alert_Label_AlertProcessed, "Đã xử lý, đóng cảnh báo", "Processed and close");

        public static string Alert_Label_AlertHandling => Get(MobileResourceNames.Alert_Label_AlertHandling, "Xử lý", "Content");

        public static string Alert_Label_AlertDetail => Get(MobileResourceNames.Alert_Label_AlertDetail, "Nội dung", "Detail");

        public static string Alert_Message_RequiredContent => Get(MobileResourceNames.Alert_Message_RequiredContent, "Nội dung xử lý không được để trống", "Content is required");

        public static string Alert_Message_RequireHandleMinLenght => Get(MobileResourceNames.Alert_Message_RequireHandleMinLenght, "Nội dung xử lý không nhỏ hơn {0} ký tự", "Handle content don't less than {0} characters");

        public static string Alert_Message_Alert_Success => Get(MobileResourceNames.Alert_Message_Alert_Success, "Xử lý cảnh báo thành công", "Handle alert successfully");

        public static string Alert_Message_Alert_Fail => Get(MobileResourceNames.Alert_Message_Alert_Fail, "Xử lý cảnh báo chưa thành công", "Handle alert was unsuccessfully");

        public static string Alert_Message_AlertLongStop => Get(MobileResourceNames.Alert_Message_AlertLongStop,
            "Xe {0} Đã vi phạm dừng đỗ quá lâu, ngưỡng cảnh báo 90 phút kết thúc tại thời điểm {1} được {2} phút",
            "Xe {0} Đã vi phạm dừng đỗ quá lâu, ngưỡng cảnh báo 90 phút kết thúc tại thời điểm {1} được {2} phút");

        public static string Alert_Message_AlertLostGPS => Get(MobileResourceNames.Alert_Message_AlertLostGPS,
            "Xe - {0} mat tin hieu GSM luc {1}",
            "Xe - {0} mat tin hieu GSM luc {1}");

        public static string Alert_Message_AlertLostGSM => Get(MobileResourceNames.Alert_Message_AlertLostGSM,
            "Xe - {0} mat tin hieu GSM luc {1}",
            "Xe - {0} mat tin hieu GSM luc {1}");
    }
}