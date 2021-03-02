using System;

namespace BA_MobileGPS.Entities.RequestEntity
{
    public class GetCountAlertByUserIDRequest
    {
        public Guid UserID { get; set; }

        public int CompanyID { get; set; }

        public string ListVehicleIDs { get; set; }
    }
}