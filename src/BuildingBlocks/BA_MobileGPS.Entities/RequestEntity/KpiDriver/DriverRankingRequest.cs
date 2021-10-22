using System;

namespace BA_MobileGPS.Entities
{
    public class DriverRankingRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int CompanyID { get; set; }
        public string[] UserIDs { get; set; }
    }
}