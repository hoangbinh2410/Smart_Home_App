using System;

namespace BA_MobileGPS.Entities
{
    public class AlertGetRequest
    {
        public Guid UserId { get; set; }

        public string CultureName { get; set; }

        public string ListAlertTypeIDs { get; set; }

        public string ListVehicleIDs { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }
    }
}