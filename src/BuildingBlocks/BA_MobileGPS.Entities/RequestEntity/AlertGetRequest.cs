using System;

namespace BA_MobileGPS.Entities
{
    public class AlertGetRequest
    {
        public Guid UserID { get; set; }

        public int CompanyID { get; set; }

        public string ListAlertTypeIDs { get; set; }

        public string ListVehicleIDs { get; set; }
    }
}