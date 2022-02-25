using BA_MobileGPS.Entities;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IVehicleOnlineService
    {
        Task<List<Company>> GetListCompanyAsync(Guid userId, int companyID);

        Task<List<Company>> GetListCompanyByBusinessUserAsync(Guid userId);

        Task<List<VehicleGroupModel>> GetListVehicleGroupAsync(Guid userId, int companyID);

      //  Task<List<Vehicle>> GetListVehicle(VehicleOnlineRequest request);

        Task<List<VehicleOnline>> GetListVehicleOnline(VehicleOnlineRequest request);

        Task<List<VehicleOnlineMessage>> GetListVehicleOnlineSync(VehicleOnlineSyncRequest vehiclerequest);

        Task<List<LandmarkResponse>> GetListBoundary();

        Task<List<LandmarkResponse>> GetListParacelIslands();
    }
}