using BA_MobileGPS.Entities;

using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IVehicleRouteService
    {
        Task<RouteHistoryResponse> GetHistoryRoute(RouteHistoryRequest request);
        
        Task<ValidateUserConfigGetHistoryRouteResponse> ValidateUserConfigGetHistoryRoute(ValidateUserConfigGetHistoryRouteRequest request);
    }
}