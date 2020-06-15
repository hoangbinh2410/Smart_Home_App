using System;

namespace BA_MobileGPS.Entities
{
    public class FuelChartRequest
    {
        public Guid UserID { get; set; }

        public int CompanyID { get; set; }

        public long VehicleID { get; set; }

        public string VehiclePlate { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}