using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resource
{
    public partial class MobileResource
    {
        public static string VehicleGroupLookUp_Label_Title => Get(MobileResourceNames.VehicleGroupLookUp_Label_Title, "Chọn nhóm", "Choose Vehicle Group");
        public static string VehicleGroupLookUp_Label_NotFound => Get(MobileResourceNames.VehicleGroupLookUp_Label_NotFound, "Không có nhóm nào", "Not found any vehicle group");
        public static string VehicleGroupLookUp_Label_Search => Get(MobileResourceNames.VehicleGroupLookUp_Label_Search, "Tìm kiếm", "Search");
        public static string VehicleGroupLookUp_Label_Confirm => Get(MobileResourceNames.VehicleGroupLookUp_Label_Confirm, "Chọn", "Confirm");
    }
}