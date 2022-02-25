using System;

namespace BA_MobileGPS.Entities
{
    public class VehicleOnlineRequest
    {
        public Guid UserID { get; set; }

        public int CompanyID { get; set; }

        public int XnCode { get; set; }

        public UserType UserType { get; set; }

        public CompanyType CompanyType { get; set; }
    }

    public class VehicleOnlineSyncRequest
    {
        public Guid UserID { get; set; }

        public int CompanyID { get; set; }

        public int XnCode { get; set; }

        public string VehicelIDs { get; set; }

        public DateTime LastTime { get; set; }
    }
}