using System;

namespace BA_MobileGPS.Entities
{
    public class VehicleDebtMoneyResponse
    {
        public int OrderNumber { get; set; }
        public int VehicleID { get; set; }
        public string VehiclePlate { get; set; }
        public int FK_CompanyID { get; set; }
        public int GroupVehicleID { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime RenewedToDate { get; set; }
        public int CountExpireDate { get; set; }
        public string Descriptions { get; set; }
        public float Money { get; set; }
        public int TotalCount { get; set; }
        public int TotalDebtMoney { get; set; }
        public int MessageIdBAP { get; set; }
        public string Contact { get; set; }
        public int SortOrder { get; set; }
    }
}