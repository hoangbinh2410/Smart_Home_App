using BA_MobileGPS.Utilities.Enums;

using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class DistanceResponse
    {
        public string VehiclePlate { set; get; }

        public DateTime VehicleTime { get; set; }

        public int Velocity { set; get; }

        public string Distance { get; set; }
    }

}