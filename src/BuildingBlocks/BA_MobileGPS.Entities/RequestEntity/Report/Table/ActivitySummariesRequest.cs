using System;

namespace BA_MobileGPS.Entities
{
    public class ActivitySummariesRequest : ReportBaseModel
    {
        public string VehicleIDs { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}