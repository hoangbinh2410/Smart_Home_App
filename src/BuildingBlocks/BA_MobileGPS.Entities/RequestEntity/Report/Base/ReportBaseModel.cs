using System;

namespace BA_MobileGPS.Entities
{
    public class ReportBaseModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int CompanyID { get; set; }
        public string VehicleIDs { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool IsAddress { get; set; }
    }
}