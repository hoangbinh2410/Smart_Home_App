using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class DetailVehicleService : IDetailVehicleService
    {
        private readonly IRequestProvider _IRequestProvider;

        public DetailVehicleService(IRequestProvider requestProvider)
        {
            this._IRequestProvider = requestProvider;
        }

        /// <summary>
        /// Load tất cả các thông tin của xe lên
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  3/15/2019   created
        /// </Modified>
        public async Task<VehicleOnlineDetailViewModel> LoadAllInforVehicle(DetailVehicleRequest input)
        {
            var respone = new VehicleOnlineDetailViewModel();
            try
            {
               // var URL = string.Format(ApiUri.GET_VEHICLEDETAIL + "/?UserId={0}&vehiclePlate={1}&vehicleID={2}", input.UserId, input.vehiclePlate, input.vehicleID);
                var URL = string.Format(ApiUri.GET_VEHICLEDETAIL + "/?xnCode={0}&vehiclePlate={1}&companyId={2}", input.XnCode, input.VehiclePlate, input.CompanyId);
                var temp = await _IRequestProvider.GetAsync<VehicleOnlineDetailViewModel>(URL);
                if (temp != null)
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

        /// <summary>
        /// Load tất cả các thông tin của xe lên
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// namth  19/07/2019   created
        /// </Modified>
        public async Task<ShipDetailRespone> GetShipDetail(ShipDetailRequest input)
        {
            var respone = new ShipDetailRespone();
            try
            {
                var URL = string.Format(ApiUri.GET_SHIPDETAIL + "/?UserId={0}&vehiclePlate={1}", input.UserId, input.vehiclePlate);
                var temp = await _IRequestProvider.GetAsync<ShipDetailRespone>(URL);
                if (temp != null)
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