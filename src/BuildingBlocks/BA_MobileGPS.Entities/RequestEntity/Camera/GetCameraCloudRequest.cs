using System;

namespace BA_MobileGPS.Entities
{
    public class GetCameraCloudRequest : CameraBaseRequest
    {
        public int Customer { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Vehicle { get; set; }

        public int Channel { get; set; }
    }
}