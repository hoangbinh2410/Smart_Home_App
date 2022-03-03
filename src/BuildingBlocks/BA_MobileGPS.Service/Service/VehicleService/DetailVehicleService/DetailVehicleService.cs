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
        public async Task<VehicleOnlineDetailViewModel> GetVehicleDetail(DetailVehicleRequest input)
        {
            var respone = new VehicleOnlineDetailViewModel();
            try
            {
                var temp = await _IRequestProvider.PostAsync<DetailVehicleRequest, ResponseBase<VehicleOnlineDetailViewModel>>(ApiUri.GET_VEHICLEDETAIL, input);
                if (temp != null && temp.Data != null)
                {
                    respone = temp.Data;
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
                var temp = await _IRequestProvider.GetAsync<ResponseBase<ShipDetailRespone>>(URL);
                if (temp != null&& temp.Data!=null)
                {
                    respone = temp.Data;
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