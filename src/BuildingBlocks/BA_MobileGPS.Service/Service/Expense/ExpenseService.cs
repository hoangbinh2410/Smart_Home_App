using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity.Expense;
using BA_MobileGPS.Entities.ResponeEntity.Expense;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.Service.Expense
{
   public class ExpenseService : IExpenseService
    {
        private readonly IRequestProvider _iRequestProvider;
        public ExpenseService(IRequestProvider iRequestProvider)
        {
            this._iRequestProvider = iRequestProvider;
        }

        public async Task<bool> GetExpense(ImportExpenseRequest request)
        {
            bool result = false;
            try
            {
                var respone = await _iRequestProvider.PostAsync<ImportExpenseRequest, BaseResponse<bool>>(ApiUri.POST_Import_Expense, request);
                if (respone != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }     
        public async Task<List<ListExpenseCategoryByCompanyRespone>> GetExpenseCategory(int FK_CompanyID, int languageID)
        {
            List<ListExpenseCategoryByCompanyRespone> result = new List<ListExpenseCategoryByCompanyRespone>();
            try
            {
                string uri = string.Format(ApiUri.GET_List_ExpensesCategory + "?companyid={0}&languageID={1}", FK_CompanyID, languageID);
                var respone = await _iRequestProvider.GetAsync<ResponseBaseV2<List<ListExpenseCategoryByCompanyRespone>>>(uri);
                if (respone != null && respone.Data != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }

        public async Task<List<ExpenseRespone>> GetListExpense(ExpenseRequest request)
        {
            List<ExpenseRespone> result = new List<ExpenseRespone>();
            try
            {
                var respone = await _iRequestProvider.PostAsync<ExpenseRequest, ResponseBaseV2<List<ExpenseRespone>>>(ApiUri.GET_List_Expenses, request);
                if (respone != null && respone.Data != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }
        public async Task<bool> Deletemultiple(DeleteExpenseRequest request)
        {
            bool result = false;
            try
            {
                var respone = await _iRequestProvider.PostAsync<DeleteExpenseRequest, ResponseBaseV2<bool>>(ApiUri.Delete_Multiple, request);
                if (respone != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }
    }
}
