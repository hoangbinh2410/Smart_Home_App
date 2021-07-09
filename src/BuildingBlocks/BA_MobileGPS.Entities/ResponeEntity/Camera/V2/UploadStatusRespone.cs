using System;

namespace BA_MobileGPS.Entities
{
    public class UploadStatusRespone
    {
        public int CustomerID { get; set; }
        public string VehicleName { get; set; }
        public int Channel { get; set; }
        public string CurrentFile { get; set; }
        public int ErrorCount { get; set; }
        public int FinishCount { get; set; }
        public int TotalCount { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}