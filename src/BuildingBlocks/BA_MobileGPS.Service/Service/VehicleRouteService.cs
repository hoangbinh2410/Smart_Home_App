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

                var data = await requestProvider.PostAsync<RouteHistoryRequest, string>(url, request);

                if (data != null)
                {
                    var msg = JsonConvert.DeserializeObject<MsgRequest>(data);
                    var decoded = Message.DecodeMessage(msg.Param);

                    if (result.FromByteArray32(decoded))
                    {
                        return result;
                    }
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
                var data = await requestProvider.PostAsync<ValidateUserConfigGetHistoryRouteRequest, ResponseBase<ValidateUserConfigGetHistoryRouteResponse>>(ApiUri.GET_VALIDATE_USER_CONFIG_ROUTE_HISTORY, request);
                if (data != null && data.Data != null)
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
    }
}