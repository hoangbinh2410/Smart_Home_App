using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity.Expense
{
    [Serializable]
    public class ExpenseDetailsRespone:BaseModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string FK_ExpenseCategoryID { get; set; }
        public int FK_CompanyID { get; set; }
        public long FK_VehicleID { get; set; }
        public DateTime ExpenseDate { get; set; }
        public decimal ExpenseCost;
        public decimal ExpenseCostRef { get => ExpenseCost; set => SetProperty(ref ExpenseCost, value); }
        public string Note;
        public string NoteRef { get => Note; set => SetProperty(ref Note, value); }
        public string LandmarkName { get; set; }
        public string OtherAddress { get; set; }
        public string Photo { get; set; }
    }
}
