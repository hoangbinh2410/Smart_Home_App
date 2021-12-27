using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class CameraStartRequest : CameraBaseRequest
    {
        public int CustomerID { get; set; }

        public string VehicleName { get; set; }

        public int Channel { get; set; }

        public int Duration { get; set; }
    }

    public class CameraStartMultipleRequest : CameraBaseRequest
    {
        public int CustomerID { get; set; }

        public List<string> VehicleNames { get; set; }
    }
}