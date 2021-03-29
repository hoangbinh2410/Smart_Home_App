using System;

namespace BA_MobileGPS.Entities.RequestEntity
{
    public class CameraImageRequest
    {
        public int XNCode { set; get; }

        public DateTime FromDate { set; get; }

        public DateTime ToDate { set; get; }

        public string VehiclePlate { set; get; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}