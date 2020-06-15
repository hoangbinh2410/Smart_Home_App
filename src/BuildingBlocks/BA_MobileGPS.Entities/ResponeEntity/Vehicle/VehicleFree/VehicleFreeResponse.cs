using System;

namespace BA_MobileGPS.Entities
{
    public class VehicleFreeResponse
    {
        public int OrderNumber { get; set; }

        public int PK_VehicleID { get; set; }

        public string VehiclePlate { get; set; }

        public int FK_CompanyID { get; set; }

        public int FK_VehicleGroupID { get; set; }

        public DateTime ExpireDate { get; set; }

        public int MessageIdBAP { get; set; }

        public DateTime RenewedToDate { get; set; }

        public int CountExpireDate { get; set; }
    }
}