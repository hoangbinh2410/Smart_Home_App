using System;

namespace BA_MobileGPS.Entities
{
    public class CameraPlaybackInfoRequest
    {
        public int CompanyID { get; set; }
        public int XnCode { get; set; }
        public string VehiclePlate { get; set; }

        public int? Channel { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}