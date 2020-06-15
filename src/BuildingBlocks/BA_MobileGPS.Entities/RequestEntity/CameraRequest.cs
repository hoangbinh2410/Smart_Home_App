using System;

namespace BA_MobileGPS.Entities.RequestEntity
{
    public class CameraImageRequest
    {
        public int CompanyID { set; get; }

        public int XNCode { set; get; }

        public DateTime DayBefore { set; get; }

        public short DayOffset { set; get; }

        public TimeSpan TimeFrom { set; get; }

        public TimeSpan TimeEnd { set; get; }

        public string VehiclePlate { set; get; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}