using System.ComponentModel;

namespace BA_MobileGPS.Entities
{
    public enum FormOfNoticeTypeEnum
    {
        [Description("Hiển thị popup sau khi đăng nhập")]
        NoticeAfterLogin = 1,

        [Description("Hiển thị ngoài màn hình đăng nhập")]
        NoticeWhenLogin = 2,

        [Description("Hiển thị thông báo Notify")]
        Notice = 3
    }
}