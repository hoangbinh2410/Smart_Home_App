using Newtonsoft.Json;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class PlaybackStartRespone
    {
        public int Channel { get; set; }
        public string Link { get; set; }

        [JsonProperty("StreamingRequests")]
        public List<StreamUserRequest> StreamingRequests { get; set; }

        [JsonProperty("PlaybackRequests")]
        public List<PlaybackUserRequest> PlaybackRequests { get; set; }

        [JsonProperty("UploadRequests")]
        public List<UploadUserRequest> UploadRequests { get; set; }
    }

    public class PlaybackUserRequest
    {
        public int Source { get; set; }
        public string User { get; set; }
    }
}