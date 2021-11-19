using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.RequestEntity.Report.TransportBusiness
{
    public class TransportBusinessRequest : ReportBaseModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string VehicleIDs { get; set; }
        public string SortColumn { get; set; }
        public string FromPositionIds { get; set; }
        public string ToPositionIds { get; set; }
        public string FromPositionIdHis { get; set; }
        public string ToPositionIdHis { get; set; }
        public double MaxKm { get; set; }
        public double MinKm { get; set; }
        public bool ISChecked { get; set; }
        public bool ISHikari { get; set; }
        public bool ISHistory { get; set; }
        public bool OnBen { get; set; }
        public bool ISStop { get; set; }
    }
}
