using Newtonsoft.Json;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class CameraStartRespone
    {
        public int Channel { get; set; }
        public string Link { get; set; }

        [JsonProperty("StreamRequests")]
        public List<StreamUserRequest> StreamUserRequests { get; set; }
    }

    public class StreamUserRequest
    {
        public int Source { get; set; }
        public string User { get; set; }
    }
}