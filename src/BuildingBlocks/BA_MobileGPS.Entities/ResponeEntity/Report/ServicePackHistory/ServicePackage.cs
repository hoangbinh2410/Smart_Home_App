using System;

namespace BA_MobileGPS.Entities
{
    public class ServicePackage
    {
        public int BundleID { get; set; }

        public string BundleName { get; set; }

        public int SmsData { get; set; }

        public int LeftData { get; set; }

        public int Blocks => LeftData / 10;

        public DateTime ExpiredTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}