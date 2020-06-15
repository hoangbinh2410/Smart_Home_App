using System;

namespace BA_MobileGPS.Entities
{
    public class SpeedOversRequest : ReportBaseModel
    {
        public string VehicleIDs { get; set; }

        public int VelocityMax { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}