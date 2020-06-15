using System;

namespace BA_MobileGPS.Entities
{
    public class StopParkingVehicleRequest : ReportBaseModel
    {
        public string VehicleIDs { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int TotalTimeStop { get; set; }

        public int MinutesOfManchineOn { get; set; }
    }
}