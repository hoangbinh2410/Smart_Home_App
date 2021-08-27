using System.ComponentModel;

namespace BA_MobileGPS.Entities
{
    public enum IconColor
    {
        [Description("grey")]
        GREY = 1,

        [Description("blue")]
        BLUE = 2,

        [Description("orange")]
        ORANGE = 3,

        [Description("red")]
        RED = 4,

        [Description("stop")]
        STOP_LONG = 5,

        [Description("lost_gps")]
        LOST_GPS = 6,

        [Description("warn")]
        WARNING = 7,

        [Description("blue_grey")]
        BLUEGREY = 8,

        [Description("warnturnoff")]
        WARNINGTURNOFF = 9,

    }
}