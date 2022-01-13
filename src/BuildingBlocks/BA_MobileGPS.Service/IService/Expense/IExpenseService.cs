using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity.Expense;
using BA_MobileGPS.Entities.ResponeEntity.Expense;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
   public interface  IExpenseService
    {
        Task<bool> GetExpense(ImportExpenseRequest request);
        Task<List<ListExpenseCategoryByCompanyRespone>> GetExpenseCategory(int FK_CompanyID, int languageID);
        Task<List<ExpenseRespone>> GetListExpense(ExpenseRequest request);
        Task<bool> Deletemultiple(DeleteExpenseRequest request);
    }
}
