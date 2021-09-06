using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string ListPaper_Label_TilePage => Get(MobileResourceNames.ListPaper_Label_TilePage, "Danh sách giấy tờ", "List of documents");
        public static string ListPaper_Label_OptionPaper => Get(MobileResourceNames.ListPaper_Label_OptionPaper, "Theo loại giấy tờ", "By type of document");
        public static string ListPaper_Label_OptionVehicle => Get(MobileResourceNames.ListPaper_Label_OptionVehicle, "Theo phương tiện", "Follow the car");

        public static string ListPaper_Label_SelectPaperType => Get(MobileResourceNames.ListPaper_Label_SelectPaperType, "Chọn loại giấy tờ", "Select the type of document");

        public static string CabSignInfor_Label_SignNumber => Get(MobileResourceNames.CabSignInfor_Label_SignNumber, "Số phù hiệu", "Sign Number");
        public static string CabSignInfor_Label_RegisterDate => Get(MobileResourceNames.CabSignInfor_Label_RegisterDate, "Ngày đăng kí", "Register Date");

        public static string CabSignInfor_Label_DaysNumberForAlertAppear => Get(MobileResourceNames.CabSignInfor_Label_DaysNumberForAlertAppear, "Số ngày cảnh báo trước", "Number of days of advance warning");
        public static string CabSignInfor_Label_Insert => Get(MobileResourceNames.CabSignInfor_Label_Insert, "Tạo mới", "Create");
    }
}