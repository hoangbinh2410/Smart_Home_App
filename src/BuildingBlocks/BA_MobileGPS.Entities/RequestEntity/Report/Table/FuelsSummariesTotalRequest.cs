using System;

namespace BA_MobileGPS.Entities
{
    public class FuelsSummariesTotalRequest : ReportBaseModel
    {        
        public string VehiclePlate { get; set; }        
        public string PrivateCode { set; get; }
    }
}