using System;

namespace BA_MobileGPS.Entities
{
    public class ServicePackHistoryRequest :ReportBaseModel
    {
        public int XNCode { get; set; }

        public string VehiclePlate { get; set; }
    }
}