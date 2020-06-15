using System;

namespace BA_MobileGPS.Entities
{
    public class ServicePackHistoryRequest
    {
        public int XNCode { get; set; }

        public string VehiclePlate { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }
    }
}