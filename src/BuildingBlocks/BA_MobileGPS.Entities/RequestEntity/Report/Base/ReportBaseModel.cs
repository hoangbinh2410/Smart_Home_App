using System;

namespace BA_MobileGPS.Entities
{
    public class ReportBaseModel
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int CompanyID { get; set; }
        public string ListVehicleID { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool IsAddress { get; set; }
    }
}