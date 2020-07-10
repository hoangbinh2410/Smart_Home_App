using System.ComponentModel;

namespace VMS_MobileGPS.Models
{
    public enum StateDeviceNameEnums
    {
        [Description("SRV")]
        SRV,

        [Description("KN")]
        KN,

        [Description("GSM")]
        GSM,

        [Description("GPS")]
        GPS,

        [Description("SAT")]
        SAT,

        [Description("RTC")]
        RTC,

        [Description("FLASH")]
        FLASH,

        [Description("FRAM")]
        FRAM,

        [Description("BLE")]
        BLE,

        [Description("SOS")]
        SOS,

        [Description("VEX")]
        VEX,

        [Description("VSOLAR")]
        VSOLAR,

        [Description("VBAT")]
        VBAT,

        [Description("VSYS")]
        VSYS,

        [Description("V33")]
        V33,

        [Description("VGSM")]
        VGSM,

        [Description("VSUP")]
        VSUP,

        [Description("MODE")]
        MODE,

        [Description("AUX")]
        AUX
    }
}