using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class ServicePackageHistoryService : IServicePackageHistoryService
    {
        public ServicePackHistoryRequest Request { get; set; }

        private readonly IRequestProvider _requestProvider;

        public ServicePackageHistoryService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public Task<int> GetCount()
        {
            throw new NotImplementedException();
        }

        public async Task<List<ServicePackHistory>> GetData()
        {
            try
            {
                var temp = await _requestProvider.PostAsync<ServicePackHistoryRequest, ResponseBase<List<ServicePackHistory>>>(ApiUri.GET_HISTORY_PACKAGE, Request);
                if (temp != null)
                {
                    return temp.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return default;
        }

        public Task<List<ServicePackHistory>> GetMoreData()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseBase<ServicePackageInfo>> GetCurrentServicePack(object request)
        {
            try
            {
                return await _requestProvider.PostAsync<object, ResponseBase<ServicePackageInfo>>(ApiUri.GET_CURRENT_PACKAGE, request);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return default;
        }

        public async Task<ResponseBase<List<ShipPackage>>> GetShipPackages()
        {
            try
            {
                return await _requestProvider.GetAsync<ResponseBase<List<ShipPackage>>>(ApiUri.GET_SHIP_PACKAGE);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return default;
        }

        public Task<ValidatedReportRespone> ValidateDateTimeReport(Guid UserId, DateTime FromDate, DateTime ToDate)
        {
            throw new NotImplementedException();
        }

        //public Task<BaseResponse<ServicePackageInfo>> GetCurrentServicePack(dynamic request)
        //{
        //    throw new NotImplementedException();
        //}
    }
}