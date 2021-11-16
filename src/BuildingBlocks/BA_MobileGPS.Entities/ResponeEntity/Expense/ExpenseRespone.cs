using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity.Expense
{
    [Serializable]
    public class ExpenseRespone:BaseModel
    {
        public DateTime ExpenseDate { get; set; }
        public decimal Total { get; set; }
        public List<ExpenseDetailsRespone> Expenses { get; set; }
    }
}
