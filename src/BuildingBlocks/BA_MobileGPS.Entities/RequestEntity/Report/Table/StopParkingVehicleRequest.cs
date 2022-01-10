using System;

namespace BA_MobileGPS.Entities
{
    public class StopParkingVehicleRequest : ReportBaseModel
    {      
        public int TotalTimeStop { get; set; }

        public int MinutesOfManchineOn { get; set; }
    }
}