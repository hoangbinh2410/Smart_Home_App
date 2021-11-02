using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity.Expense
{
    public class ExpenseRespone
    {
        public DateTime ExpenseDate { get; set; }
        public decimal Total { get; set; }
        public List<ExpenseDetailsRespone> Expenses { get; set; }
    }
}
