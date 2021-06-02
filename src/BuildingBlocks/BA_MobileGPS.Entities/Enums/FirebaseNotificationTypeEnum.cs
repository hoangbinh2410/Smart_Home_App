using System.ComponentModel;

namespace BA_MobileGPS.Entities
{
    public enum FirebaseNotificationTypeEnum
    {
        [Description("None")]
        None = 0,

        [Description("Alert")]
        Alert = 1,

        [Description("Notice")]
        Notice = 2,

        [Description("Issue")]
        Issue = 3,

        [Description("AlertMask")]
        AlertMask = 4,
    }
}