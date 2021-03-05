using System;

namespace BA_MobileGPS.Entities
{
    public class AlertUserConfigurationsRequest
    {
        public int Id { get; set; }

        public int FK_CompanyID { get; set; }

        public Guid FK_UserID { get; set; }

        public string AlertTypeIDs { get; set; }

        public string VehicleIDs { get; set; }

        public string ReceiveTimes { get; set; }
    }
}