using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string VehicleLookUp_Label_Title => Get(MobileResourceNames.VehicleLookUp_Label_Title, "Lựa chọn phương tiện", "Select vehicle");
        public static string VehicleLookUp_Label_NotFound => Get(MobileResourceNames.ListVehicle_Label_NotFound, "Không có phương tiện nào", "Not found any vehicle");
        public static string VehicleLookUp_Label_NotFoundVehicleGroup => Get(MobileResourceNames.ListVehicle_Label_NotFound, "Không có nhóm phương tiện nào", "Not found any vehicle groups");
        public static string VehicleLookUp_Label_ChooseVehicleGroup => Get(MobileResourceNames.ListVehicle_Label_ChooseVehicleGroup, "Chọn nhóm phương tiện", "Choose vehicle group");
        public static string VehicleLookUp_Label_ChooseCompany => Get(MobileResourceNames.ListVehicle_Label_ChooseCompany, "Chọn công ty", "Choose company");
        public static string VehicleLookUp_Label_FilterData => Get(MobileResourceNames.ListVehicle_Label_FilterData, "Lọc dữ liệu", "Filter data");
        public static string VehicleLookUp_Label_Search => Get(MobileResourceNames.ListVehicle_Label_Search, "Tìm kiếm phương tiện", "Search vehicle");
    }
}