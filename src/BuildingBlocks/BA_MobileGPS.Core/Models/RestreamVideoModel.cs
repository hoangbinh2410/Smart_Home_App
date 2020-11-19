using BA_MobileGPS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Core.Models
{
    public class RestreamVideoModel
    {
        public string VideoName { get; set; }
        public DateTime VideoStartTime { get; set; }
        public string VideoImageSource { get; set; }
        public TimeSpan VideoTime { get; set; }
        public StreamStart Data { get; set; }
        public DateTime VideoEndTime { get; set; }
        public int EventType { get; set; }
    }
}
