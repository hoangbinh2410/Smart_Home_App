using System;

namespace BA_MobileGPS.Entities
{
    public class DetailVehicleRequest
    {
        public string vehiclePlate { get; set; }
        public int vehicleID { get; set; }
        public Guid UserId { get; set; }
    }
}