﻿using BA_MobileGPS.Entities;
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

        public async Task<ImportExpenseRespone> GetExpense(ImportExpenseRequest request)
        {
            ImportExpenseRespone result = new ImportExpenseRespone();
            try
            {
                var respone = await _iRequestProvider.PostAsync<ImportExpenseRequest, BaseResponse<ImportExpenseRespone>>(ApiUri.POST_Import_Expense, request);
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
        public async Task<List<ListExpenseCategoryByCompanyRespone>> GetExpenseCategory(int FK_CompanyID)
        {
            List<ListExpenseCategoryByCompanyRespone> result = new List<ListExpenseCategoryByCompanyRespone>();
            try
            {
                string uri = string.Format(ApiUri.GET_List_ExpensesCategory + "?companyid={0}", FK_CompanyID);
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
    }
}
