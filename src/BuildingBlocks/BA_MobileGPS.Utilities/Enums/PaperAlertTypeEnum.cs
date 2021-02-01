using System.ComponentModel;

namespace BA_MobileGPS.Utilities.Enums
{

    public enum PaperAlertTypeEnum
    {
        [Description("Tất cả thời gian")]
        All,
        [Description("Chưa đến hạn cảnh báo")]
        UndueAlert,
        [Description("Đến hạn cảnh báo")]
        DueAlert,
        [Description("Quá hạn")]
        ExpireAlert
    }
}
