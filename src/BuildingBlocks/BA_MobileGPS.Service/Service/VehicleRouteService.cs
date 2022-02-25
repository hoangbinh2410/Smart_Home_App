using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using Newtonsoft.Json;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class VehicleRouteService : IVehicleRouteService
    {
        private readonly IRequestProvider requestProvider;

        public VehicleRouteService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<RouteHistoryResponse> GetHistoryRoute(RouteHistoryRequest request)
        {
            RouteHistoryResponse result = new RouteHistoryResponse();

            try
            {
                string url = $"{ApiUri.GET_VEHICLE_ROUTE_HISTORY}";

                var data = await requestProvider.PostAsync<RouteHistoryRequest,ResponseBase<RouteHistoryResponse>>(url, request);

                if (data != null&& data.Data != null)
                {
                   result = data.Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<ValidateUserConfigGetHistoryRouteResponse> ValidateUserConfigGetHistoryRoute(ValidateUserConfigGetHistoryRouteRequest request)
        {
            ValidateUserConfigGetHistoryRouteResponse result = new ValidateUserConfigGetHistoryRouteResponse();

            try
            {
                string url = $"{ApiUri.GET_VALIDATE_USER_CONFIG_ROUTE_HISTORY}?UserId={request.UserId}&CompanyId={request.CompanyId}&VehiclePlate={request.VehiclePlate}" +
                    $"&FromDate={request.FromDate.ToString("yyyy-MM-dd HH:mm:ss")}&ToDate={request.ToDate.ToString("yyyy-MM-dd HH:mm:ss")}&appID={request.AppID}";

                result = await requestProvider.GetAsync<ValidateUserConfigGetHistoryRouteResponse>(url);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }

            return result;
        }
    }
}