using System;

namespace BA_MobileGPS.Entities
{
    public class GetCameraCloudRequest : CameraBaseRequest
    {
        public int Customer { get; set; }

        public DateTime Time { get; set; }

        public string Vehicle { get; set; }
    }
}