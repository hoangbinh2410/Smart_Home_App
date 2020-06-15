using System;

namespace BA_MobileGPS.Entities
{
    public class VehicleDebtBlockResponse
    {
        public int OrderNumber { get; set; }
        public int VehicleID { get; set; }
        public string VehiclePlate { get; set; }
        public int FK_CompanyID { get; set; }
        public int CountBlockLeft { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime RenewedToDate { get; set; }
        public int CountExpireDate { get; set; }
        public string Descriptions { get; set; }
        public int SortOrder { get; set; }
    }
}