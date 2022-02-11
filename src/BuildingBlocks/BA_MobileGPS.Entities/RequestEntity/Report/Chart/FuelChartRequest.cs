using System;

namespace BA_MobileGPS.Entities
{
    public class FuelChartRequest : ReportBaseModel
    {
        public Guid UserID { get; set; }      

        public long VehicleID { get; set; }

        public string VehiclePlate { get; set; }
    }
}