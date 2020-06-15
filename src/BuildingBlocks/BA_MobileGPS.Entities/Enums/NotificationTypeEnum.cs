using System.ComponentModel;

namespace BA_MobileGPS.Entities
{
    public enum NotificationTypeEnum
    {
        [Description("Không có gì")]
        None = 0,

        [Description("Thay đổi mật khẩu")]
        ChangePassword = 1
    }
}