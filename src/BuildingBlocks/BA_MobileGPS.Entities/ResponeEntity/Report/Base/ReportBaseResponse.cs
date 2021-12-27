using System;

namespace BA_MobileGPS.Entities
{
    public class ReportBaseResponse
    {
        public int OrderNumber { get; set; }
        public long FK_VehicleID { get; set; }

        public string VehiclePlate { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int FK_CompanyID { get; set; }

        public float StartLatitude { get; set; }

        public float StartLongitude { get; set; }

        public float EndLatitude { get; set; }

        public float EndLongitude { get; set; }

        public string StartAddress { get; set; }

        public string EndAddress { get; set; }

        public bool IsVideoCam { get; set; }
    }

    public class ReportBasePaging
    {
        public int OrderNumber { get; set; }
    }
}