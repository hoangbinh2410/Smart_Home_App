using BA_MobileGPS.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string PaperInfor_Page_Title => Get(MobileResourceNames.PaperInfor_Page_Title, 
            "Nhập thông tin giấy tờ tới hạn", "Enter papers will exprire");

    }
}
