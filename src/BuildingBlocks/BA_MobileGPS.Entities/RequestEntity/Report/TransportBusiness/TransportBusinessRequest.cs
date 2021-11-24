using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.RequestEntity.Report.TransportBusiness
{
    public class TransportBusinessRequest 
    {
        public int CompanyID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string VehicleIDs { get; set; }
        public string FromPositionIds { get; set; }
        public string ToPositionIds { get; set; }
        public double MaxKm { get; set; }
        public double MinKm { get; set; }
        public bool ISChecked { get; set; }
    }
}
