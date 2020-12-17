using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string Online_Label_TitlePage => Get(MobileResourceNames.Online_Label_TitlePage, "Giám sát", "Tracking vehicle");
        public static string Online_Label_Determining => Get(MobileResourceNames.Online_Label_Determining, "Đang xác định địa chỉ.....", "Determining the address.....");
        public static string Online_Label_SeachVehicle => Get(MobileResourceNames.Online_Label_SeachVehicle, "Tìm kiếm biển số phương tiện", "Search vehicle");
        public static string Online_Label_SeachVehicle2 => Get(MobileResourceNames.Online_Label_SeachVehicle2, "Tìm kiếm phương tiện", "Search vehicle");

        public static string Online_Label_SeachVehicle3 => Get(MobileResourceNames.Online_Label_SeachVehicle3, "Tìm xe", "Search vehicle");

        public static string Online_Label_StatusCarAll => Get(MobileResourceNames.Online_Label_StatusCarAll, "Tất cả", "All");

        public static string Online_Label_StatusCarDebtMoney => Get(MobileResourceNames.Online_Label_StatusCarDebtMoney, "Nợ phí", "Car DebtMoney");
        public static string Online_Label_StatusCarMoving => Get(MobileResourceNames.Online_Label_StatusCarMoving, "Di chuyển", "Moving");

        public static string Online_Label_StatusCarStoping => Get(MobileResourceNames.Online_Label_StatusCarStoping, "Dừng đỗ", "Stoping");

        public static string Online_Label_StatusCarEngineOff => Get(MobileResourceNames.Online_Label_StatusCarEngineOff, "Tắt máy", "Engine OFF");

        public static string Online_Label_StatusCarEngineOn => Get(MobileResourceNames.Online_Label_StatusCarEngineOn, "Bật máy", "Engine ON");

        public static string Online_Label_StatusCarOverVelocity => Get(MobileResourceNames.Online_Label_StatusCarOverVelocity, "Quá tốc độ", "OverVelocity");

        public static string Online_Label_StatusCarLostGPS => Get(MobileResourceNames.Online_Label_StatusCarLostGPS, "Mất GPS", "Lost GPS");

        public static string Online_Label_StatusCarLostGSM => Get(MobileResourceNames.Online_Label_StatusCarLostGSM, "Mất GSM", "Lost GSM");
        public static string Online_Label_StatusCarSatelliteError => Get(MobileResourceNames.Online_Label_StatusCarSatelliteError, "Lỗi vệ tinh", "Satellite Error");

        public static string Online_Label_TotalCar => Get(MobileResourceNames.Online_Label_TotalCar, "Tổng xe :", "Total car :");
        public static string Online_Label_TotalVehicle => Get(MobileResourceNames.Online_Label_TotalCar, "Tổng phương tiện :", "Total vehicle :");

        public static string Online_Button_Detail => Get(MobileResourceNames.Online_Button_Detail, "CHI TIẾT", "DETAILS");
        public static string Online_Button_Router => Get(MobileResourceNames.Online_Button_Router, "LỘ TRÌNH", "ROUTER");
        public static string Online_Button_ShowHideBorder => Get(MobileResourceNames.Online_Button_ShowHideBorder, "ẨN/HIỆN ĐƯỜNG BAO", "HIDE BORDER");

        public static string Online_Label_VehicleInfo => Get(MobileResourceNames.Online_Label_VehicleInfo, "Thông tin phương tiện", "Vehicle info");
        public static string Online_Label_Last => Get(MobileResourceNames.Online_Label_Last, "cuối cùng", "");
        public static string Online_Label_AtTime => Get(MobileResourceNames.Online_Label_Last, "tại thời điểm", "at");
        public static string Online_Label_TotalKm => Get(MobileResourceNames.Online_Label_TotalKm, "Hải Lý", "NM");
        public static string Online_Label_Velocity => Get(MobileResourceNames.Online_Label_Velocity, "Vận Tốc", "Velocity");
        public static string Online_Label_Engine => Get(MobileResourceNames.Online_Label_Engine, "ĐỘNG CƠ", "Engine");
        public static string Online_Label_AirConditioning => Get(MobileResourceNames.Online_Label_AirConditioning, "ĐIỀU HÒA", "Air Conditioning");
        public static string Online_Label_Cardoor => Get(MobileResourceNames.Online_Label_Cardoor, "CỬA XE", "CAR DOOR");

        public static string Online_Label_Location => Get(MobileResourceNames.Online_Label_Location, "Vị trí", "Location");

        public static string Online_Message_CarDebtMoney => Get(MobileResourceNames.Online_Message_CarDebtMoney, "Xe đang nợ phí", "Car DebtMoney");

        public static string Online_Message_CarStopService => Get(MobileResourceNames.Online_Message_CarStopService, "Xe đã dừng dịch vụ", "Car has stopped service");

        public static string Online_Message_CarStopServiceFrom(string date) => Get(MobileResourceNames.Online_Message_CarStopService, "Xe đã dừng dịch vụ từ ngày " + date, "Car has stopped service from " + date);

        public static string Online_Lable_UpdateAdminUserSetting => Get(MobileResourceNames.Online_Lable_UpdateAdminUserSetting, "CÀI ĐẶT VỊ TRÍ", "Setup location");
        public static string Online_Action_UpdateAdminUserSetting => Get(MobileResourceNames.Online_Action_UpdateAdminUserSetting, "Cài đặt vị trí mặc định sẽ giúp bạn lưu lại vị trí của bạn khi vào trang giám sát phương tiện", "The default location setting will help you save your location when entering the vehicle monitoring page");

        public static string Online_Lable_SetupBoundary => Get(MobileResourceNames.Online_Lable_SetupBoundary, "CÀI ĐẶT VÙNG", "Setup Boundary");

        public static string Online_Button_ShowBorder => Get(MobileResourceNames.Online_Button_ShowBorder, "HIỆN ĐƯỜNG BAO", "SHOW BORDER");
        public static string Online_Button_HideBorder => Get(MobileResourceNames.Online_Button_HideBorder, "ẨN ĐƯỜNG BAO", "HIDE BORDER");

        public static string Online_Button_Option => Get(MobileResourceNames.Online_Button_Option, "Cài đặt", "Option");
        public static string Online_CheckBox_Distance => Get(MobileResourceNames.Online_CheckBox_Distance, "Khoảng cách", "Distance");
    }
}