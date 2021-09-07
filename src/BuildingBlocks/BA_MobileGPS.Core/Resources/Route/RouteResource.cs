using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string Route_Label_Title => Get(MobileResourceNames.Route_Label_Title, "Lộ trình", "Route");

        public static string Route_Label_TitleVMS => Get(MobileResourceNames.Route_Label_Title, "Hải trình", "Voyage");
        public static string Route_Label_ListTitle => Get(MobileResourceNames.Route_Label_ListTitle, "Lộ trình", "Route");
        public static string Route_Label_Time => Get(MobileResourceNames.Route_Label_Time, "Thời gian", "Time");
        public static string Route_Label_Vgps => Get(MobileResourceNames.Route_Label_Vgps, "Vgps", "Vgps");
        public static string Route_Label_Status => Get(MobileResourceNames.Route_Label_Status, "Trạng thái", "Status");
        public static string Route_Label_SearchVehicle => Get(MobileResourceNames.Route_Label_SearchVehicle, "Tìm kiếm xe", "Search vehicle");

        public static string Route_Label_SearchFishing => Get(MobileResourceNames.Route_Label_SearchFishing, "Tìm kiếm phương tiện", "Search for fishing boat");
        public static string Route_Label_VehicleEmpty => Get(MobileResourceNames.Route_Label_VehicleEmpty, "Bạn chưa chọn xe", "Vehicle is empty");
        public static string Route_Label_RouteNotFound => Get(MobileResourceNames.Route_Label_RouteNotFound, "Không tìm thấy lộ trình", "Route not found");
        public static string Route_Label_RouteNotFoundVMS => Get(MobileResourceNames.Route_Label_RouteNotFoundVMS, "Không tìm thấy hải trình", "Voyage not found");
        public static string Route_Label_RouteNotExist => Get(MobileResourceNames.Route_Label_RouteNotExist, "Chưa có lộ trình", "No route yet");
        public static string Route_Label_RouteNotExistVMS => Get(MobileResourceNames.Route_Label_RouteNotExistVMS, "Chưa có hải trình", "Voyage not exist");
        public static string Route_Label_More => Get(MobileResourceNames.Route_Label_More, "Khác", "More");

        public static string Route_Label_Coordinates => Get(MobileResourceNames.Route_Label_Coordinates, "Tọa độ:", "Coordinates:");

        public static string Route_Label_Move => Get(MobileResourceNames.Route_Label_Move, "Di chuyển", "Move");

        public static string Route_Label_StopParking => Get(MobileResourceNames.Route_Label_StopParking, "Dừng đỗ", "Stop Parking");

        public static string Route_Label_LostSignal => Get(MobileResourceNames.Route_Label_LostSignal, "Mất tín hiệu", "Lost Signal");
        public static string Route_Label_StartDateMustSmallerThanEndDate => Get(MobileResourceNames.Route_Label_StartDateMustSmallerThanEndDate, "Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc", "Start time must be smaller than end time");

        public static string Route_Label_TotalTimeLimit => Get(MobileResourceNames.Route_Label_TotalTimeLimit,
            "Bạn không được phép xem quá {0} ngày", "You don't have permission to view more than {0} days");

        public static string Route_Label_EndDateLimit => Get(MobileResourceNames.Route_Label_EndDateLimit, "Bạn không được phép xem quá ngày hiện tại", "End date is greater than now");

        public static string Route_Label_AccountIsExpired => Get(MobileResourceNames.Route_Label_AccountIsExpired,
            "Quí khách đã hết hạn sử dụng do chưa đóng phí. Vui lòng đóng phí để có thể tiếp tục theo dõi", "Your account is expired. Please deposit money into account to continue");

        public static string Route_Label_FromDateToDateLimit => Get(MobileResourceNames.Route_Label_FromDateToDateLimit,
            "Bạn chỉ được phép xem từ ngày {0} đến ngày {1}", "You are only allowed to view from {0}to {1}");

        public static string Route_Label_ToDateFromDateLimit => Get(MobileResourceNames.Route_Label_ToDateFromDateLimit,
            "Bạn chỉ được phép xem từ ngày {0} và trước ngày {1}", "You are only allowed to view from {0} and before {1}");

        public static string Route_Label_FromDateLimit => Get(MobileResourceNames.Route_Label_FromDateLimit,
            "Bạn chỉ được phép xem từ ngày {0}", "You are only allowed to view from {0}");

        public static string Route_Label_ToDateLimit=> Get(MobileResourceNames.Route_Label_ToDateLimit,
            "Bạn không được phép xem quá ngày {0}", "You are not allowed to view after {0}");

        public static string Route_Label_OverDateLimit => Get(MobileResourceNames.Route_Label_OverDateLimit,
            "Bạn đang xem quá ngày giới hạn", "You are viewing past the limit date");

        public static string Route_Label_DistanceTitle => Get(MobileResourceNames.Route_Label_DistanceTitle, "Đo khoảng cách", "Measure the distance");

        public static string Route_Label_FeeVMS => Get(MobileResourceNames.Route_Label_FeeVMS, "Phí phương tiện", "Fee");

        public static string Route_Label_Circle => Get(MobileResourceNames.Route_Label_Circle, "Vòng bao", "Circle");
    }
}