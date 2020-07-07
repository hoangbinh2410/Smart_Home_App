using System.ComponentModel;

namespace BA_MobileGPS.Entities
{
    public enum NoticeTypeEnum
    {
        [Description("Thông báo tương tác")]
        Interactive = 1,

        [Description("Thông báo cấp")]
        Level = 2,

        [Description("Thông báo bảo trì")]
        Maintenance = 3,

        [Description("Thông báo tính năng mới")]
        NewFution = 4,

        [Description("Chức mừng Noel")]
        Noel = 5,

        [Description("Chúc tết")]
        NewYear = 6,

        [Description("Chúc mừng sinh nhật")]
        Birthday = 7,

        [Description("Thông báo lâu không đăng nhập")]
        NoLogin = 8
    }
}