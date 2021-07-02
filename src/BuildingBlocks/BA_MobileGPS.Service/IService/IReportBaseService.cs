using BA_MobileGPS.Entities;
using System;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IReportBaseService<TRequest, TResponse> where TRequest : class, new() where TResponse : class, new()
    {
        TRequest Request { get; set; }

        Task<TResponse> GetData();

        Task<TResponse> GetMoreData();

        Task<int> GetCount();

        Task<ValidatedReportRespone> ValidateDateTimeReport(Guid UserId,
          DateTime FromDate, DateTime ToDate);
    }
}