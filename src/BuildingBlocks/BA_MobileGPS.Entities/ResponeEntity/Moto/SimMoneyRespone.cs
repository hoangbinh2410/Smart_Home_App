using System;

namespace BA_MobileGPS.Entities
{
    public class SimMoneyRespone
    {
        public long FK_VehicleID { set; get; }

        public float Money { set; get; }

        public string SMSMoneyResult { set; get; }

        public DateTime UpdateDate { set; get; }
    }
}