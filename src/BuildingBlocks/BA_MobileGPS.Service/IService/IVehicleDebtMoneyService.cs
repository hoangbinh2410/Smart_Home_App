using BA_MobileGPS.Entities;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IVehicleDebtMoneyService
    {
        Task<List<VehicleDebtMoneyResponse>> LoadAllVehicleDebtMoney(Guid UserID);

        Task<List<VehicleFreeResponse>> LoadAllVehicleFree(Guid UserID);

        Task<int> GetCountVehicleDebtMoney(Guid UserID);
    }
}