using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class StreamDeviceRequest : CameraBaseRequest
    {
        public int ConditionType { get; set; }

        public List<string> ConditionValues { get; set; }
    }
}