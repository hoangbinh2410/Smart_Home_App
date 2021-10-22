using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BA_MobileGPS.Entities.Enums
{
    public enum SupportPageCode
    {
        [Description("Hiển thị trang hỗ trợ mất tín hiệu")]
        ErrorSignalPage = 0,

        [Description("Hiển thị trang hỗ trợ thay đổi biển số")]
        ChangePlateNumberPage = 1,

        [Description("Hiển thị trang hỗ trợ lỗi camera")]
        ErrorCameraPage = 2
    }
}
