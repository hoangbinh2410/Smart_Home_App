using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resource
{
    public partial class MobileResource
    {
        public static string Boundary_Label_Title => Get(MobileResourceNames.Boundary_Label_Title, "Danh sách điểm ranh giới", "List of boundary points");

        public static string Boundary_Label_All => Get(MobileResourceNames.Boundary_Label_All, "Tất cả", "All");

        public static string Boundary_Label_Showarea => Get(MobileResourceNames.Boundary_Label_Showarea, "Hiển thị vùng bao", "Show bounding area");

        public static string Boundary_Label_Showname => Get(MobileResourceNames.Boundary_Label_Showname, "Hiển thị tên điểm", "Show display name");
    }
}