using BA_MobileGPS.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string PaperInfor_Page_Title => Get(MobileResourceNames.PaperInfor_Page_Title, 
            "Nhập thông tin giấy tờ", "Enter papers will exprire");
        public static string PaperInfor_Msg_Expired => Get(MobileResourceNames.PaperInfor_Msg_Expired,
         "Giấy tờ đã quá hạn, vui lòng tạo đăng ký mới!", "This paper was expired, please add new paper");
        public static string PaperInfor_Msg_NearExpire => Get(MobileResourceNames.PaperInfor_Msg_NearExpire,
         "Giấy tờ sắp tới ngày hết hạn, vui lòng tạo đăng ký mới!", "This paper's going to expire, please add new paper");

    }
}
