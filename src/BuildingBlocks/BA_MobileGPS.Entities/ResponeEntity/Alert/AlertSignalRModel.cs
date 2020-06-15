using System;

namespace BA_MobileGPS.Entities
{
    public class AlertSignalRModel
    {
        public byte WarningType { set; get; }

        public long VehicleId { set; get; }

        public string VehiclePlate { set; get; }

        public DateTime TimeStart { set; get; }

        public double Latitude { set; get; }

        public double Longitude { set; get; }

        public string WarningContent { set; get; }

        public string VehicleNo { set; get; }
    }
}