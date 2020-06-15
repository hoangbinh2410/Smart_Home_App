using System;

namespace BA_MobileGPS.Entities
{
    public class FuelsSummariesRequest : ReportBaseModel
    {
        public string VehicleIDs { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}