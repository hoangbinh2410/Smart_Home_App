using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string Boundary_Label_TilePage => Get(MobileResourceNames.Boundary_Label_TilePage, "Cấu hình hiển thị điểm", "Score display configuration");

        public static string Boundary_Label_SelectPointCompany => Get(MobileResourceNames.Boundary_Label_SelectPointCompany, "Lựa chọn nhóm điểm công ty", "Select the company score group");

        public static string Boundary_Label_AllPoint => Get(MobileResourceNames.Boundary_Label_AllPoint, "Tất cả", "All point");

        public static string Boundary_Label_Boundary => Get(MobileResourceNames.Boundary_Label_Boundary, "Vùng bao", "Boundary");

        public static string Boundary_Label_PointName => Get(MobileResourceNames.Boundary_Label_PointName, "Tên điểm", "Point name");

        public static string Boundary_Label_PointAplication => Get(MobileResourceNames.Boundary_Label_PointAplication, "Nhóm điểm hệ thống", "System point group");
    }
}