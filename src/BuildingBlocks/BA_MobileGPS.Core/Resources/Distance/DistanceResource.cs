using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string Distance_Label_TilePage => Get(MobileResourceNames.Distance_Label_TilePage, "Khoảng cách tới các tàu", "Distance to the vehicle");

        public static string Distance_Label_Vehicle => Get(MobileResourceNames.Distance_Label_Vehicle, "Phương tiện", "Vehicle");

        public static string Distance_Label_Time => Get(MobileResourceNames.Distance_Label_Time, "Thời điểm", "Time");

        public static string Distance_Label_Speed => Get(MobileResourceNames.Distance_Label_Speed, "Vận tốc", "Speed");

        public static string Distance_Label_Distance => Get(MobileResourceNames.Distance_Label_Distance, "Hải lý", "Distance");

        public static string Distance_Label_TitleDistance => Get(MobileResourceNames.Distance_Label_TitleDistance, "Khoảng cách từ phương tiện {0} đến các phương tiện khác", "Distance from the vehicle {0} to other vehicles");

        public static string Distance_Label_LostGPS => Get(MobileResourceNames.Distance_Label_LostGPS, "Mất GPS", "Lost GPS");

        public static string Distance_Message_NoData => Get(MobileResourceNames.Distance_Message_NoData, "Không có dữ liệu tìm kiếm", "Data not found");
    }
}