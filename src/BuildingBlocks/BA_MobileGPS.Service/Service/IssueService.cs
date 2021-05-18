using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Issues;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class IssueService : IIssueService
    {
        private readonly IRequestProvider _IRequestProvider;

        public IssueService(IRequestProvider IRequestProvider)
        {
            this._IRequestProvider = IRequestProvider;
        }

        public async Task<List<IssuesRespone>> GetIssueByCompanyID(int companyID)
        {
            List<IssuesRespone> result = new List<IssuesRespone>();
            try
            {
                string url = $"{ApiUri.GET_ISSUE_BYCOMPANYID}?companyID={companyID}";
                var response = await _IRequestProvider.GetAsync<ResponseBaseV2<List<IssuesRespone>>>(url);
                if (response != null && response.Data != null)
                {
                    result = response.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<IssuesDetailRespone> GetIssueByIssueCode(string issueCode)
        {
            IssuesDetailRespone result = new IssuesDetailRespone();
            try
            {
                string url = $"{ApiUri.GET_ISSUE_BYISSUECODE}?issueCode={issueCode}";
                var response = await _IRequestProvider.GetAsync<ResponseBaseV2<IssuesDetailRespone>>(url);
                if (response != null && response.Data != null)
                {
                    result = response.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return result;
        }
    }
}