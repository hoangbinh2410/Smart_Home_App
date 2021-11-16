using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.RequestEntity.Report.Station
{
    public class StationDetailsRequest : ReportBaseModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string VehicleIDs { get; set; }
        public int LandmarkId { get; set; }
        public int NumberOfMinute { get; set; }

    }
}
