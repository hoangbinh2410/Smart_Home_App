using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities
{
   public class ImportExpenseRequest
    {
        public Guid? Id { get; set; }
        public Guid FK_ExpenseCategoryID { get; set; }
        public int FK_CompanyID { get; set; }
        public long FK_VehicleID { get; set; }
        public DateTime ExpenseDate { get; set; }
        public decimal ExpenseCost { get; set; }
        public string Note { get; set; }    
        public int? FK_LandmarkID { get; set; }
        public string OtherAddress { get; set; }
        public string Photo { get; set; }
        public Guid User { get; set; }
        public bool IsChangePhoto { get; set; }

    }
}
