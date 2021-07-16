using System;

namespace BA_MobileGPS.Entities
{
    public class ReportQCVN31SpeedRequest
    {
        public int XnCode { get; set; }

        public string VehiclePlate { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public bool OptionData { get; set; }

        public string Imei { get; set; }
    }
}