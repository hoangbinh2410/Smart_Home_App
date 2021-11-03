using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.RequestEntity.Expense
{
    public class ExpenseRequest
    {
        public int CompanyID { get; set; }
        public int VehicleID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }       
    }
}
