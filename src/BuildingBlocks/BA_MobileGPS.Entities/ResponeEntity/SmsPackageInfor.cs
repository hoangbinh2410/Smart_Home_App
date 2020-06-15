using System;

namespace BA_MobileGPS.Entities
{
    public class SmsPackageInfor
    {
        public string VehicleName { get; set; }

        public string CustomerID { get; set; }

        public string BundleName { get; set; }

        public int BundleID { get; set; }

        public DateTime ExpiredTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public int LeftData { get; set; }
    }
}