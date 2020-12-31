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
        // public static string ListDriver_Messenger_Delete => Get(MobileResourceNames.Notification_Label_TilePage, "THÔNG BÁO", "Notification");
        //  public static string ListDriver_Label_Delete => Get(MobileResourceNames.Notification_Label_TilePage, "THÔNG BÁO", "Notification");

    }
}
