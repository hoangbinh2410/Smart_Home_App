using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BA_MobileGPS.Entities.Enums
{
    public enum MenuSupportEnum
    {
        [Description("MTH")]
        ErrorSinglePage = 0,

        [Description("CAM.LOI")]
        ErrorCameraPage = 1,

        [Description("DOIBS")]
        ChangePlatePage = 2
    }
}
