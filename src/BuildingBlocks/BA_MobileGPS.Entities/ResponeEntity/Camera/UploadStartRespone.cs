using Newtonsoft.Json;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class UploadStartRespone
    {
        public int Channel { get; set; }

        [JsonProperty("UploadRequests")]
        public List<UploadUserRequest> UploadUserRequests { get; set; }
    }

    public class UploadUserRequest
    {
        public int Source { get; set; }
        public string User { get; set; }

        public string Session { get; set; }
    }
}