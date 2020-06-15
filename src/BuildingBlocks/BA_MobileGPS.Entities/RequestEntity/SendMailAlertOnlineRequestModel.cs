using System;

namespace BA_MobileGPS.Entities
{
    public class SendMailAlertOnlineRequestModel
    {
        public string Email { get; set; }

        public string ListAlertTypeIds { get; set; }

        public string ListVehicleIds { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public bool WasBonusReaded { get; set; } = true;
    }
}