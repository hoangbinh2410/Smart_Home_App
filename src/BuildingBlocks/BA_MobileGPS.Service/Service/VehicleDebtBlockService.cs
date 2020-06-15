using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class VehicleDebtBlockService : IVehicleDebtBlockService
    {
        private readonly IRequestProvider requestProvider;

        public VehicleDebtBlockService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<List<VehicleDebtBlockResponse>> LoadAllVehicleDebtBlock(Guid UserID)
        {
            var respone = new List<VehicleDebtBlockResponse>();
            try
            {
                var temp = await requestProvider.GetAsync<List<VehicleDebtBlockResponse>>($"{ApiUri.GET_LISTVEHICLEDEBTMONEY}?userID={UserID}");
                if (temp != null && temp.Count > 0)
                {
                    respone = temp;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return respone;
        }
    }
}