using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class CameraCloudRespone
    {
        public int Customer { get; set; }
        public string Vehicle { get; set; }
        public DateTime Time { get; set; }
        [JsonProperty("Data")]
        public List<DataVideoCloud> Data { get; set; }
    }

    public class DataVideoCloud
    {
        public string Link { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Channel { get; set; }
    }
}