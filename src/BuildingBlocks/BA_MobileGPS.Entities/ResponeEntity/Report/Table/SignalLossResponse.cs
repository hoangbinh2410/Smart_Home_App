using System;

namespace BA_MobileGPS.Entities
{
    public class SignalLossResponse : ReportBaseResponse
    {
        public string PrivateCode { get; set; }

        public TimeSpan TotalTimes { get; set; }

        public string Status { get; set; }
    }
}