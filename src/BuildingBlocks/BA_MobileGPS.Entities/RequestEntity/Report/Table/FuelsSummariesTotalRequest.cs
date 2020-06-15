using System;

namespace BA_MobileGPS.Entities
{
    public class FuelsSummariesTotalRequest : ReportBaseModel
    {
        public string VehicleIDs { get; set; }

        public string VehiclePlate { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}