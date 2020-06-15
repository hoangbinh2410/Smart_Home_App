using System;

namespace BA_MobileGPS.Entities
{
    public class ServicePackageInfo
    {
        public int XnCode { get; set; }

        public string VehiclePlate { get; set; }

        public int VTPackageID { get; set; }

        public string VTPackageName { get; set; }

        public int SMSPackageID { get; set; }

        public string SMSPackageName { get; set; }

        public int SmsData { get; set; }

        public int Blocks => SmsData / 10;

        public DateTime? ExpiredTime { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}