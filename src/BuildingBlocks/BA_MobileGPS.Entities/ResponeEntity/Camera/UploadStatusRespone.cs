using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class UploadStatusRespone
    {
        public int Channel { get; set; }
        public string VehicleName { get; set; }
        public int CustomerID { get; set; }
        public string CurrentFile { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime RequestedTime { get; set; }
        public string RequestedUser { get; set; }
        public int TotalCount { get; set; }
        public int FinishCount { get; set; }
        public int ErrorCount { get; set; }
        public int RetryCount { get; set; }
        public int State { get; set; }
        public DateTime UpdatedTime { get; set; }
        public List<UploadFiles> UploadedFiles { get; set; }
    }

    public class UploadFiles
    {
        public string Link { get; set; }
        public int State { get; set; }
        public DateTime Time { get; set; }
    }
}