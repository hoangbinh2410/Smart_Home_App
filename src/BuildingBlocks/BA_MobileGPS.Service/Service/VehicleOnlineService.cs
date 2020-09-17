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
                var data = await requestProvider.GetHandleOutputAsync<List<Company>>(url);
                if (data != null)
                {
                    result = data;
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
                var data = await requestProvider.GetHandleOutputAsync<List<Company>>(url);
                if (data != null)
                {
                    result = data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<List<Vehicle>> GetListVehicle(Guid userId, string groupIDs, int companyID, VehicleLookUpType type)
        {
            List<Vehicle> result = new List<Vehicle>();
            try
            {
                var url = $"{ApiUri.GET_VEHICLE_LIST}?userID={userId}&vehicleGroupIDs={groupIDs}&companyID={companyID}&type={type}";
                var data = await requestProvider.GetHandleOutputAsync<List<Vehicle>>(url);
                if (data != null)
                {
                    result = data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<List<VehicleOnline>> GetListVehicleOnline(Guid userId, int groupId, int companyID, int xnCode, UserType userType, CompanyType companyType)
        {
            List<VehicleOnline> result = new List<VehicleOnline>();
            try
            {
                string url = $"{ApiUri.GET_VEHICLEONLINE}?userId={userId}&groupID={groupId}&companyID={companyID}&xnCode={xnCode}&userType={(int)userType}&companyType={(int)companyType}";
                var data = await requestProvider.GetAsync<ResponseBaseV2<List<VehicleOnline>>>(url);
                if (data != null)
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

        public async Task<List<VehicleGroupModel>> GetListVehicleGroupAsync(Guid userId, int companyID)
        {
            List<VehicleGroupModel> result = new List<VehicleGroupModel>();
            try
            {
                string url = $"{ApiUri.GET_VEHICLE_GROUP}?userid={userId}&companyid={companyID}";

                var data = await requestProvider.GetHandleOutputAsync<List<VehicleGroupModel>>(url);
                if (data != null)
                {
                    result = data;
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
            try
            {
                string url = $"{ApiUri.GET_LIST_POLYGON}?FK_LandmarkCatalogueID=220";

                var data = await requestProvider.GetHandleOutputAsync<List<LandmarkResponse>>(url);
                if (data != null)
                {
                    return data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return new List<LandmarkResponse>();
        }

        public async Task<List<VehicleOnlineMessage>> GetListVehicleOnlineSync(VehicleOnlineRequest vehiclerequest)
        {
            List<VehicleOnlineMessage> result = new List<VehicleOnlineMessage>();
            try
            {
                string url = ApiUri.GET_VEHICLEONLINESYNC;
                result = await requestProvider.PostAsync<VehicleOnlineRequest, List<VehicleOnlineMessage>>(url, vehiclerequest);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }
    }
}