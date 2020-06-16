using System;

namespace BA_MobileGPS.Entities
{
    public class MotoPropertiesRespone
    {
        public MotoPropertiesViewModel GetXMPropertiesResult { set; get; }
    }

    public class MotoPropertiesViewModel
    {
        public long IMEI { set; get; }

        public string XNCode { set; get; }

        public string VehiclePlate { set; get; }

        public DateTime FirstConnectTime { set; get; }

        public DateTime LastConnectTime { set; get; }

        public bool AllowLowPowerAlarmViaSMS { set; get; }

        public string PhoneNumber1 { set; get; }

        public string PhoneNumber2 { set; get; }

        public string PhoneNumber3 { set; get; }

        public bool AllowDriveAlarm { set; get; }

        public bool AllowAutoTurnOnLightWhenNight { set; get; }

        public bool AllowPowerDisconnectAlarmViaSMS { set; get; }

        public bool AllowTurnOnOffEngineViaSMS { set; get; }

        public int BatteryLowVoltageThreshold { set; get; }

        public bool AllowCallDriveAlarm { set; get; }

        public string DevicePhoneNumber { get; set; }
    }
}