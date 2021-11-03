using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity.Expense
{
    public class ExpenseDetailsRespone
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string FK_ExpenseCategoryID { get; set; }
        public int FK_CompanyID { get; set; }
        public int FK_VehicleID { get; set; }
        public DateTime ExpenseDate { get; set; }
        public decimal ExpenseCost { get; set; }
        public string Note { get; set; }
        public string LandmarkName { get; set; }
        public string OtherAddress { get; set; }
        public string Photo { get; set; }
    }
}
