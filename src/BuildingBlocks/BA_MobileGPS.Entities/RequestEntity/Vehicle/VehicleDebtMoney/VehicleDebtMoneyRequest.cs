using System;

namespace BA_MobileGPS.Entities
{
    public class VehicleDebtMoneyRequest
    {
        public int XNCode { get; set; }
        public Guid UserID { get; set; }
        public int PageNext { get; set; }
        public int PageSize { get; set; }
    }
}