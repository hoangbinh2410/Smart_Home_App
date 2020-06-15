using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resource
{
    public partial class MobileResource
    {
        public static string ListVehicle_Label_Title => Get(MobileResourceNames.ListVehicle_Label_Title, "Danh sách Phương tiện", "List vehicle");
        public static string ListVehicle_Label_Title2 => Get(MobileResourceNames.ListVehicle_Label_Title2, "Danh sách phương tiện", "List vehicle");
        public static string ListVehicle_Label_NotFound => Get(MobileResourceNames.ListVehicle_Label_NotFound, "Không có xe nào", "Not found any vehicle");
        public static string ListVehicle_Label_NotFoundVehicleGroup => Get(MobileResourceNames.ListVehicle_Label_NotFound, "Không có nhóm phương tiện nào", "Not found any vehicle groups");
        public static string ListVehicle_Label_ChooseVehicleGroup => Get(MobileResourceNames.ListVehicle_Label_ChooseVehicleGroup, "Chọn nhóm phương tiện", "Choose vehicle group");
        public static string ListVehicle_Label_ChooseCompany => Get(MobileResourceNames.ListVehicle_Label_ChooseCompany, "Chọn công ty", "Choose company");
        public static string ListVehicle_Label_FilterData => Get(MobileResourceNames.ListVehicle_Label_FilterData, "Lọc dữ liệu", "Filter data");
        public static string ListVehicle_Label_Search => Get(MobileResourceNames.ListVehicle_Label_Search, "Tìm kiếm phương tiện", "Search vehicle");
        public static string ListVehicle_Label_Vehicle_Expired => Get(MobileResourceNames.ListVehicle_Label_Vehicle_Expired, "Phương tiện đã hết hạn sử dụng phí", "Vehicle expired");
        public static string ListVehicle_Label_Vehicle_Unpaid => Get(MobileResourceNames.ListVehicle_Label_Vehicle_Unpaid, "Phương tiện bị khoá do chưa thanh toán phí", "Vehicle is locked due to unpaid fee");
        public static string ListVehicle_Label_Vehicle_Stop_Service => Get(MobileResourceNames.ListVehicle_Label_Vehicle_Stop_Service, "Phương tiện dừng dịch vụ", "Car stop service");
        public static string ListVehicle_Label_Vehicle_Expiration_Date => Get(MobileResourceNames.ListVehicle_Label_Vehicle_Expiration_Date, "Ngày hết hạn:", "Expiration date:");
        public static string ListVehicle_Label_Vehicle_Expired_Date => Get(MobileResourceNames.ListVehicle_Label_Vehicle_Expired_Date, "Hạn thu phí:", "Date of payment:");
        public static string ListVehicle_Label_Vehicle_Stop_Date => Get(MobileResourceNames.ListVehicle_Label_Vehicle_Stop_Date, "Ngày dừng:", "Stop date:");
        public static string ListVehicle_Label_Vehicle_Own_Fee => Get(MobileResourceNames.ListVehicle_Label_Vehicle_Own_Fee, "Xe đang nợ phí dịch vụ", "Vehicles are owed service charges");
        public static string ListVehicle_Label_SelectCommand => Get(MobileResourceNames.ListVehicle_Label_SelectCommand, "Chọn chức năng", "Select");
        public static string ListVehicle_Label_SortBy => Get(MobileResourceNames.ListVehicle_Label_SortBy, "Sắp xếp theo", "Sort by");
        public static string ListVehicle_Label_ByVehicle => Get(MobileResourceNames.ListVehicle_Label_ByVehicle, "Phương tiện", "Vehicle");
        public static string ListVehicle_Label_ByVehiclePlate => Get(MobileResourceNames.ListVehicle_Label_ByVehiclePlate, "Biển kiểm soát", "Vehicle plate");
        public static string ListVehicle_Label_ByTime => Get(MobileResourceNames.ListVehicle_Label_ByTime, "Thời gian", "Time");
        public static string ListVehicle_Label_ByState => Get(MobileResourceNames.ListVehicle_Label_ByState, "Trạng thái", "State");
        public static string ListVehicle_Label_Default => Get(MobileResourceNames.ListVehicle_Label_Default, "Mặc định", "Default");
        public static string ListVehicle_Label_VehicleNormal => Get(MobileResourceNames.ListVehicle_Label_VehicleNormal, "Phương tiện dừng đỗ, nổ máy hoặc xe chạy với vận tốc bình thường", "Parking stops, on engine or the vehicle runs at normal speed");
        public static string ListVehicle_Label_VehicleNormal2 => Get(MobileResourceNames.ListVehicle_Label_VehicleNormal2, "Phương tiện dừng đỗ, nổ máy hoặc chạy với vận tốc bình thường", "Parking stops, on engine or the vehicle runs at normal speed");
        public static string ListVehicle_Label_VehicleStop => Get(MobileResourceNames.ListVehicle_Label_VehicleStop, "Phương tiện dừng đỗ, tắt máy hoặc xe nợ phí", "Parking stops, off engine or vehicle debt charges");
        public static string ListVehicle_Label_VehicleStop2 => Get(MobileResourceNames.ListVehicle_Label_VehicleStop2, "Phương tiện dừng đỗ, tắt máy hoặc nợ phí", "Parking stops, off engine or vehicle debt charges");
        public static string ListVehicle_Label_VehicleSpeeding => Get(MobileResourceNames.ListVehicle_Label_VehicleSpeeding, "Phương tiện chạy quá tốc độ cho phép", "Speeding vehicle");
        public static string ListVehicle_Label_VehicleSpeeding2 => Get(MobileResourceNames.ListVehicle_Label_VehicleSpeeding2, "Phương tiện chạy quá tốc độ cho phép", "Speeding vehicle");
        public static string ListVehicle_Label_VehicleLostGPS => Get(MobileResourceNames.ListVehicle_Label_VehicleLostGPS, "Phương tiện mất tín hiệu GPS lớn hơn 5 phút và nhỏ hơn 150 phút", "Vehicles lose GPS signals larger than 5 minutes and less than 150 minutes");
        public static string ListVehicle_Label_VehicleLostGPS2 => Get(MobileResourceNames.ListVehicle_Label_VehicleLostGPS2, "Phương tiện mất tín hiệu GPS lớn hơn 5 phút và nhỏ hơn 150 phút", "Vehicles lose GPS signals larger than 5 minutes and less than 150 minutes");
        public static string ListVehicle_Label_VehicleLostGSM => Get(MobileResourceNames.ListVehicle_Label_VehicleLostGSM, "Phương tiện mất tín hiệu liên lạc, hết tiền hoặc mất GPS lớn hơn 150 phút", "The car loses the contact signal, runs out of money or loses GPS for more than 150 minutes");
        public static string ListVehicle_Label_VehicleLostGSM2 => Get(MobileResourceNames.ListVehicle_Label_VehicleLostGSM2, "Phương tiện mất tín hiệu liên lạc, hết tiền hoặc mất GPS lớn hơn 150 phút", "The car loses the contact signal, runs out of money or loses GPS for more than 150 minutes");
    }
}