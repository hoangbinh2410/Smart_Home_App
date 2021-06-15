using BA_MobileGPS.Entities.ResponeEntity.Issues;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IIssueService
    {
        Task<List<IssuesRespone>> GetIssueByCompanyID(int companyID);

        Task<IssuesDetailRespone> GetIssueByIssueCode(string issueCode);

        Task<List<IssuesRespone>> GetIssueByUserID(Guid userID);
    }
}