using System;

namespace BA_MobileGPS.Entities
{
    public class VehicleOnlineRequest
    {
        public Guid UserId { get; set; }
        public int XnCode { get; set; }
        public int CompanyID { get; set; }
        public string VehicelIDs { get; set; }
        public DateTime LastSync { get; set; }
    }
}