using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resource
{
    public partial class MobileResource
    {
        public static string CompanyLookUp_Label_Title => Get(MobileResourceNames.CompanyLookUp_Label_Title, "Chọn công ty", "Choose company");
        public static string CompanyLookUp_Label_NotFound => Get(MobileResourceNames.CompanyLookUp_Label_NotFound, "Không có công ty nào", "Not found any company");
        public static string CompanyLookUp_Label_Search => Get(MobileResourceNames.CompanyLookUp_Label_Search, "Tìm kiếm", "Search");
    }
}