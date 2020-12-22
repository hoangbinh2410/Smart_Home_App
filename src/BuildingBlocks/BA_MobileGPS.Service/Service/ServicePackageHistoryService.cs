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
                var temp = await _requestProvider.PostAsync<ServicePackHistoryRequest, BaseResponse<List<ServicePackHistory>>>(ApiUri.GET_HISTORY_PACKAGE, Request);
                if (temp != null)
                {
                    if (temp.Success)
                    {
                        return temp.Data;
                    }
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

        public async Task<BaseResponse<ServicePackageInfo>> GetCurrentServicePack(object request)
        {
            try
            {
                return await _requestProvider.PostAsync<object, BaseResponse<ServicePackageInfo>>(ApiUri.GET_CURRENT_PACKAGE, request);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return default;
        }

        public async Task<BaseResponse<List<ShipPackage>>> GetShipPackages()
        {
            try
            {
                return await _requestProvider.GetAsync<BaseResponse<List<ShipPackage>>>(ApiUri.GET_SHIP_PACKAGE);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return default;
        }

        //public Task<BaseResponse<ServicePackageInfo>> GetCurrentServicePack(dynamic request)
        //{
        //    throw new NotImplementedException();
        //}
    }
}