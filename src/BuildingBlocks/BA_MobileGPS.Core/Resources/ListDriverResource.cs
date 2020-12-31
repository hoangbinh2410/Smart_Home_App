using BA_MobileGPS.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string ListDriver_Label_Delete => Get(MobileResourceNames.Notification_Label_TilePage, "THÔNG BÁO", "Notification");
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
        public static string ListDriver_Messenger_LicenseRank => Get(MobileResourceNames.ListDriver_Messenger_LicenseRank, "Bằng lái xe hạng ",
        "Driver license rank ");
        public static string ListDriver_Messenger_DuplicateData => Get(MobileResourceNames.ListDriver_Messenger_DuplicateData,
            "Thất bại,bị trùng CMND hoặc số bằng lái với lái xe khác",
        "The driver identity or license number are duplicated with other's");
        public static string ListDriver_Messenger_LicenseType => Get(MobileResourceNames.ListDriver_Messenger_LicenseType, "loại bằng lái",
        "driver license type ");
        public static string ListDriver_Item_SelectLicenseType => Get(MobileResourceNames.ListDriver_Item_SelectLicenseType, "Chọn loại bằng lái",
        "select driver license type ");

        public static string ListDriver_Messenger_Gender => Get(MobileResourceNames.ListDriver_Messenger_Gender, "giới tính",
     "gender");
        public static string ListDriver_Notify_DateGreater => Get(MobileResourceNames.ListDriver_Notify_DateGreater, "Ngày hết hạn phải lớn hơn ngày cấp",
     "The expried date must greater than the issue date");
        public static string ListDriver_Item_SelectGender => Get(MobileResourceNames.ListDriver_Item_SelectGender, "Chọn giới tính",
      "Select gender ");
        public static string ListDriver_Item_Male => Get(MobileResourceNames.ListDriver_Item_Male, "Nam", "Male");

        public static string ListDriver_Item_Female => Get(MobileResourceNames.ListDriver_Item_Female, "Nữ", "Female");

        public static string ListDriver_Item_Other => Get(MobileResourceNames.ListDriver_Item_Other, "Khác", "Other");

    }
}
