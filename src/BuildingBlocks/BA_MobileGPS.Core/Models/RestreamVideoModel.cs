using System;

namespace BA_MobileGPS.Core.Models
{
    public class RestreamVideoModel
    {
        public string VideoName { get; set; }
        public DateTime VideoStartTime { get; set; }
        public string ImageUrl { get; set; }
        public TimeSpan VideoTime { get; set; }
        public DateTime VideoEndTime { get; set; }
        public int Channel { get; set; }
        public string Link { get; set; }
    }
}