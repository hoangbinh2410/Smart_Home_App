using Newtonsoft.Json;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class PlaybackStartRespone
    {
        public int Channel { get; set; }
        public string Link { get; set; }
        public object StreamingRequests { get; set; }

        [JsonProperty("PlaybackRequests")]
        public List<PlaybackUserRequest> PlaybackRequests { get; set; }

        public object UploadRequests { get; set; }
    }

    public class PlaybackUserRequest
    {
        public int Source { get; set; }
        public string User { get; set; }
    }
}