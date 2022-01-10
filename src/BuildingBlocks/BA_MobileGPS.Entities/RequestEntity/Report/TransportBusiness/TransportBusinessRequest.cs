using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.RequestEntity.Report.TransportBusiness
{
    public class TransportBusinessRequest : ReportBaseModel
    {
        public string FromPositionIds { get; set; }
        public string ToPositionIds { get; set; }
        public double MaxKm { get; set; }
        public double MinKm { get; set; }
        public bool ISChecked { get; set; }
    }
}
