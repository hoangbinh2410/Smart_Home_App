using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class VehicleOnlineService : IVehicleOnlineService
    {
        private readonly IRequestProvider requestProvider;

        public VehicleOnlineService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<List<Company>> GetListCompanyAsync(Guid userId, int companyID)
        {
            List<Company> result = new List<Company>();
            try
            {
                string url = $"{ApiUri.GET_VEHICLE_COMPANY}?userId={userId}&companyID={companyID}";
                var data = await requestProvider.GetHandleOutputAsync<ResponseBase<List<Company>>>(url);
                if (data != null && data.Data.Count>0)
                {
                    result = data.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<List<Company>> GetListCompanyByBusinessUserAsync(Guid userId)
        {
            List<Company> result = new List<Company>();
            try
            {
                string url = $"{ApiUri.GET_VEHICLE_COMPANY_BY_BUSINESSUSER}?userId={userId}";
                var data = await requestProvider.GetHandleOutputAsync<ResponseBase<List<Company>>>(url);
                if (data != null&& data.Data.Count>0)
                {
                    result = data.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }
        public async Task<List<VehicleOnline>> GetListVehicleOnline(VehicleOnlineRequest request)
        {
            List<VehicleOnline> result = new List<VehicleOnline>();
            try
            {
                var temp = await requestProvider.PostAsync<VehicleOnlineRequest, ResponseBase<List<VehicleOnline>>>(ApiUri.GET_VEHICLEONLINE, request);
                if (temp != null && temp.Data.Count>0)
                {
                   result = temp.Data;
                }              
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<List<VehicleGroupModel>> GetListVehicleGroupAsync(Guid userId, int companyID)
        {
            List<VehicleGroupModel> result = new List<VehicleGroupModel>();
            try
            {
                string url = $"{ApiUri.GET_VEHICLE_GROUP}?userid={userId}&companyid={companyID}";

                var data = await requestProvider.GetHandleOutputAsync<ResponseBase<List<VehicleGroupModel>>>(url);
                if (data != null && data.Data.Count>0)
                {
                    result = data.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<List<LandmarkResponse>> GetListBoundary()
        {
            List<LandmarkResponse> result = new List<LandmarkResponse>();
            try
            {
                string url = $"{ApiUri.GET_LIST_POLYGON}?FK_LandmarkCatalogueID=220";

                var data = await requestProvider.GetHandleOutputAsync<ResponseBase<List<LandmarkResponse>>>(url);
                if (data != null && data.Data.Count>0)
                {
                    result = data.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<List<LandmarkResponse>> GetListParacelIslands()
        {
            List<LandmarkResponse> result = new List<LandmarkResponse>();
            try
            {
                string url = $"{ApiUri.GET_LIST_POLYGONPARACELISLANDS}?FK_LandmarkCatalogueID=220";

                var data = await requestProvider.GetHandleOutputAsync<ResponseBase<List<LandmarkResponse>>>(url);
                if (data != null&& data.Data.Count>0)
                {
                    result = data.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<List<VehicleOnlineMessage>> GetListVehicleOnlineSync(VehicleOnlineSyncRequest vehiclerequest)
        {
            List<VehicleOnlineMessage> result = new List<VehicleOnlineMessage>();
            try
            {               
               var respone = await requestProvider.PostAsync<VehicleOnlineSyncRequest, ResponseBase<List<VehicleOnlineMessage>>>(ApiUri.GET_VEHICLEONLINESYNC, vehiclerequest);
               if(respone != null && respone.Data.Count > 0)
                {
                    result = respone.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }
    }
}