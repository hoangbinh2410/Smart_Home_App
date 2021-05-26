using System.ComponentModel;

namespace BA_MobileGPS.Utilities.Enums
{
    // Source: https://developer.android.com/reference/android/telephony/TelephonyManager
    public enum NetworkDataType : int
    {
        [Description("1xRTT")]
        NETWORK_TYPE_1xRTT = 7,
        [Description("CDMA")]
        NETWORK_TYPE_CDMA = 4,
        [Description("EDGE")]
        NETWORK_TYPE_EDGE = 2,
        [Description("eHRPD")]
        NETWORK_TYPE_EHRPD = 14,
        [Description("EVDO revision 0")]
        NETWORK_TYPE_EVDO_0 = 5,
        [Description("EVDO revision A")]
        NETWORK_TYPE_EVDO_A = 6,
        [Description("EVDO revision B")]
        NETWORK_TYPE_EVDO_B = 12,
        [Description("GPRS")]
        NETWORK_TYPE_GPRS = 1,
        [Description("GSM")]
        NETWORK_TYPE_GSM = 16,
        [Description("HSDPA")]
        NETWORK_TYPE_HSDPA = 8,
        [Description("HSPA")]
        NETWORK_TYPE_HSPA = 10,
        [Description("HSPA+")]
        NETWORK_TYPE_HSPAP = 15,
        [Description("HSUPA")]
        NETWORK_TYPE_HSUPA = 9,
        [Description("iDen")]
        NETWORK_TYPE_IDEN = 11,
        [Description("IWLAN")]
        NETWORK_TYPE_IWLAN = 18,
        [Description("4G/LTE")]
        NETWORK_TYPE_LTE = 13,
        [Description("5G")]
        NETWORK_TYPE_NR = 20,
        [Description("TD_SCDMA")]
        NETWORK_TYPE_TD_SCDMA = 17,
        [Description("UMTS")]
        NETWORK_TYPE_UMTS = 3,
        [Description("Unknown")]
        NETWORK_TYPE_UNKNOWN = 0
    };
}