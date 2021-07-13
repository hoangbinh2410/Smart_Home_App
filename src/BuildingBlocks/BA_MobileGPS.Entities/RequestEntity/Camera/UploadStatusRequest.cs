using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class UploadStatusRequest : CameraBaseRequest
    {
        public int CustomerID { get; set; }

        public List<string> VehicleName { get; set; }
    }
}