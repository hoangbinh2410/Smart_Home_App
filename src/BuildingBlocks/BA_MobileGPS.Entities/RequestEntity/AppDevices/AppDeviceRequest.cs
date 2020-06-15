using System;

namespace BA_MobileGPS.Entities
{
    public class AppDeviceRequest
    {
        public int FK_AppID { get; set; }
        public Guid FK_UserID { get; set; }
        public string TokenID { get; set; }

        public string AppVersion { get; set; }

        public string DeviceName { get; set; }

        public string OSVersion { get; set; }

        public string Platform { get; set; }

        public string Idiom { get; set; }

        public string DeviceType { get; set; }

        public string Manufacturer { get; set; }
    }
}