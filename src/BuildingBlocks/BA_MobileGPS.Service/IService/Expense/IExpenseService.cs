using BA_MobileGPS.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
   public interface  IExpenseService
    {
        Task<ImportExpenseRespone> GetExpense(ImportExpenseRequest request);
        Task<List<ListExpenseCategoryByCompanyRespone>> GetExpenseCategory(int FK_CompanyID);
    }
}
