using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class VehicleDebtMoneyService : IVehicleDebtMoneyService
    {
        private readonly IRequestProvider _IRequestProvider;

        public VehicleDebtMoneyService(IRequestProvider requestProvider)
        {
            this._IRequestProvider = requestProvider;
        }

        /// <summary>
        /// Load tất cả dữ liệu về phần xe nợ phí ra trong 1 lần
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// namth  3/15/2019   created
        /// </Modified>
        public async Task<List<VehicleDebtMoneyResponse>> LoadAllVehicleDebtMoney(Guid UserID)
        {
            var respone = new List<VehicleDebtMoneyResponse>();
            try
            {
                var temp = await _IRequestProvider.GetAsync<ResponseBase<List<VehicleDebtMoneyResponse>>>($"{ApiUri.GET_LISTVEHICLEDEBTMONEY}?userID={UserID}");
                if (temp != null && temp.Data.Count > 0)
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
        /// Load tất cả dữ liệu về phần xe nợ phí ra trong 1 lần
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// namth  3/15/2019   created
        /// </Modified>
        public async Task<List<VehicleFreeResponse>> LoadAllVehicleFree(Guid UserID)
        {
            var respone = new List<VehicleFreeResponse>();
            try
            {
                var temp = await _IRequestProvider.GetAsync<ResponseBase<List<VehicleFreeResponse>>>($"{ApiUri.GET_LISTVEHICLEFREE}?userID={UserID}");
                if (temp != null && temp.Data.Count > 0)
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
        /// Load tất cả dữ liệu về phần xe nợ phí ra trong 1 lần
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// namth  3/15/2019   created
        /// </Modified>
        public async Task<int> GetCountVehicleDebtMoney(Guid UserID)
        {
            var respone = 0;
            try
            {
                var temp = await _IRequestProvider.GetAsync<ResponseBase<int>>($"{ApiUri.GET_COUNTVEHICLEDEBTMONEY}?userID={UserID}");
                if (temp.Data > 0)
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