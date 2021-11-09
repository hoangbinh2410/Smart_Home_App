using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity.Expense
{
    public class ExpenseDetailsRespone:BaseModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string FK_ExpenseCategoryID { get; set; }
        public int FK_CompanyID { get; set; }
        public long FK_VehicleID { get; set; }
        public DateTime ExpenseDate { get; set; }
        public decimal expenseCost;
        public decimal ExpenseCost { get => expenseCost; set => SetProperty(ref expenseCost, value); }
        private string note;
        public string Note { get => note; set => SetProperty(ref note, value); }
        public string LandmarkName { get; set; }
        public string OtherAddress { get; set; }
        public string Photo { get; set; }
    }
}
