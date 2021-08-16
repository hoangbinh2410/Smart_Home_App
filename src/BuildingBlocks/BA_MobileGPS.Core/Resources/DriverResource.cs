using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string ListDriver_Label_Title => Get(MobileResourceNames.ListDriver_Label_Title, "Danh sách lái xe", "Driver List");

        public static string ListDriver_Label_Search => Get(MobileResourceNames.ListDriver_Label_Search, "Tìm tên lái xe", "Find the driver's name");

        public static string ListDriver_Messenger_Delete => Get(MobileResourceNames.ListDriver_Messenger_Delete, "Bạn có chắc chắn muốn xóa lái xe?",
            "Do you want to delete this driver?");

        public static string ListDriver_Messenger_NotNull => Get(MobileResourceNames.ListDriver_Messenger_NotNull, "Vui lòng nhập ",
           "Please enter");

        public static string ListDriver_Messenger_NotSelect => Get(MobileResourceNames.ListDriver_Messenger_NotSelect, "Vui lòng chọn ",
         "Please select");

        public static string ListDriver_Title_Update => Get(MobileResourceNames.ListDriver_Title_Update, "Xem thông tin lái xe",
         "Detail Driver Informations");

        public static string ListDriver_Title_Insert => Get(MobileResourceNames.ListDriver_Title_Insert, "Nhập thông tin lái xe",
         "Add Driver Informations");

        public static string ListDriver_Title_Success => Get(MobileResourceNames.ListDriver_Title_Success, "Thêm lái xe thành công",
       "Add Successful");

        public static string AddDriver_Lable_Title => Get(MobileResourceNames.ListDriver_Title_Success, "Thêm lái xe",
         "Add driver");

        public static string ListDriver_Title_DeleteSuccess => Get(MobileResourceNames.ListDriver_Title_DeleteSuccess, "Xóa lái xe thành công",
         "Delete Successful");

        public static string ListDriver_Messenger_LicenseRank => Get(MobileResourceNames.ListDriver_Messenger_LicenseRank, "Bằng lái xe hạng ",
        "Driver license rank ");

        public static string ListDriver_Messenger_DuplicateData => Get(MobileResourceNames.ListDriver_Messenger_DuplicateData,
            "Thất bại,bị trùng CMND hoặc số bằng lái với lái xe khác",
        "The driver identity or license number are duplicated with other's");

        public static string ListDriver_Messenger_LicenseType => Get(MobileResourceNames.ListDriver_Messenger_LicenseType, "Loại bằng lái",
        "driver license type ");

        public static string ListDriver_Item_SelectLicenseType => Get(MobileResourceNames.ListDriver_Item_SelectLicenseType, "Chọn loại bằng lái",
        "select driver license type ");

        public static string ListDriver_Messenger_LicenseNumber => Get(MobileResourceNames.ListDriver_Messenger_LicenseNumber, "Số bằng lái",
        "driver license number ");

        public static string ListDriver_Messenger_LicenseDateRegister => Get(MobileResourceNames.ListDriver_Messenger_LicenseDateRegister, "Ngày cấp",
        "Date Register");

        public static string ListDriver_Messenger_Gender => Get(MobileResourceNames.ListDriver_Messenger_Gender, "giới tính",
     "gender");

        public static string ListDriver_Notify_DateGreater => Get(MobileResourceNames.ListDriver_Notify_DateGreater, "Ngày hết hạn phải lớn hơn ngày cấp",
     "The expried date must greater than the issue date");

        public static string ListDriver_Item_SelectGender => Get(MobileResourceNames.ListDriver_Item_SelectGender, "Chọn giới tính",
      "Select gender ");

        public static string ListDriver_Item_Male => Get(MobileResourceNames.ListDriver_Item_Male, "Nam", "Male");

        public static string ListDriver_Item_Female => Get(MobileResourceNames.ListDriver_Item_Female, "Nữ", "Female");

        public static string ListDriver_Item_Other => Get(MobileResourceNames.ListDriver_Item_Other, "Khác", "Other");

        public static string AddDriver_Lable_IdentityDriver => Get(MobileResourceNames.AddDriver_Lable_IdentityDriver, "Số CMND", "Identity Number");

        public static string ListDriver_Messenger_UpdateSuccess => Get(MobileResourceNames.ListDriver_Messenger_UpdateSuccess, "Cập nhật thành công",
        "Update successfully");

        public static string AddDriver_Lable_SelectImage => Get(MobileResourceNames.AddDriver_Lable_SelectImage, "Chọn ảnh lái xe", "Choose a driving photo");

        public static string AddDriver_Lable_AddContinue => Get(MobileResourceNames.AddDriver_Lable_AddContinue, "Tiếp tục thêm lái xe mới?", "How to get the most out of your car?");
    }
}