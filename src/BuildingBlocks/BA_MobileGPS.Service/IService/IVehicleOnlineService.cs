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

        Task<List<Vehicle>> GetListVehicle(Guid userID, string groupIDs, int companyID, VehicleLookUpType type);

        Task<List<VehicleOnline>> GetListVehicleOnline(Guid userId, int groupId);

        Task<List<LandmarkResponse>> GetListBoundary();
    }
}