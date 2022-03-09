using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    /// <summary>
    /// Xử lý cảnh báo
    /// </summary>
    /// Name     Date         Comments
    /// TruongPV  1/7/2019   created
    /// </Modified>
    /// <seealso cref="BA_SealDienTu.Service.IAlertService" />
    public class AlertService : IAlertService
    {
        private readonly IRequestProvider _IRequestProvider;

        public AlertService(IRequestProvider IRequestProvider)
        {
            this._IRequestProvider = IRequestProvider;
        }

        /// <summary>
        /// Gets the alert type asynchronous.
        /// </summary>
        /// <param name="companyID">The company identifier.</param>
        /// <returns></returns>
        /// Name     Date         Comments
        /// TruongPV  1/7/2019   created
        /// </Modified>
        public async Task<List<AlertTypeModel>> GetAlertTypeAsync(int CompanyID)
        {
            List<AlertTypeModel> result = new List<AlertTypeModel>();
            try
            {
                string url = $"{ApiUri.GET_ALERT_TYPE}?CompanyID={CompanyID}";
                var response = await _IRequestProvider.GetAsync<ResponseBase<List<AlertTypeModel>>>(url);
                if (response != null && response.Data != null)
                {
                    result = response.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<int> GetCountAlert(GetCountAlertByUserIDRequest request)
        {
            int response = 0;
            try
            {
                string url = $"{ApiUri.GET_COUNT_ALERT_ONLINE}";
                var result = await _IRequestProvider.PostAsync<GetCountAlertByUserIDRequest, ResponseBase<int>>(url, request);
                if (result != null && result.Data > 0)
                {
                    response = result.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return response;
        }

        /// <summary>
        /// Gets the list alert online asynchronous.
        /// </summary>
        /// <param name="pageCount">The page count.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="listAlertTypeIDs">The list alert type i ds.</param>
        /// <param name="listVehicleIDs">The list vehicle i ds.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        /// Name     Date         Comments
        /// TruongPV  1/7/2019   created
        /// </Modified>
        public async Task<List<AlertOnlineDetailModel>> GetListAlertOnlineAsync(AlertGetRequest request)
        {
            List<AlertOnlineDetailModel> respone = new List<AlertOnlineDetailModel>();
            try
            {
                string url = $"{ApiUri.GET_ALERT_ONLINE}";
                var result = await _IRequestProvider.PostAsync<AlertGetRequest, ResponseBase<List<AlertOnlineDetailModel>>>(url, request);
                if (result != null && result.Data != null)
                {
                    respone = result.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return respone;
        }

        /// <summary>
        /// Handles the alert asynchronous.
        /// </summary>
        /// <param name="rqModel">The rq model.</param>
        /// <returns></returns>
        /// Name     Date         Comments
        /// TruongPV  1/7/2019   created
        /// </Modified>
        public async Task<bool> HandleAlertAsync(StatusAlertRequestModel rqModel)
        {
            bool respone = false;
            try
            {
                string url = $"{ApiUri.POST_ALERT_HANDLE}";
                var result = await _IRequestProvider.PostAsync<StatusAlertRequestModel, ResponseBase<bool>>(url, rqModel);
                if (result != null && result.Data)
                {
                    respone = result.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return respone;
        }

        public async Task<List<AlertCompanyConfigRespone>> GetAlertCompanyConfig(int companyID)
        {
            var result = new List<AlertCompanyConfigRespone>();
            //try
            //{
            //    var url = string.Format(ApiUri.GET_LIST_ALERT_COMPANY_CONFIG_BY_COMPANYID + "?companyID={0}", companyID);

            //    var data = await _IRequestProvider.GetAsync<BaseResponse<List<AlertCompanyConfigRespone>>>(url);

            //    if (data != null)
            //    {
            //        result = data.Data;
            //    }
            //}
            //catch (Exception e)
            //{
            //    Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            //}
            return result;
        }

        public async Task<AlertUserConfigurationsRespone> GetAlertUserConfigurations(Guid userId)
        {
            var result = new AlertUserConfigurationsRespone();
            try
            {
                var url = string.Format(ApiUri.GET_ALERT_USER_CONFIGURATIONS + "?UserId={0}", userId);

                var data = await _IRequestProvider.GetAsync<ResponseBase<AlertUserConfigurationsRespone>>(url);

                if (data != null)
                {
                    result = data.Data;
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }

        public async Task<ResponseBase<bool>> SendAlertUserConfig(AlertUserConfigurationsRequest request)
        {
            ResponseBase<bool> result = new ResponseBase<bool>();
            try
            {
                var data = await _IRequestProvider.PostAsync<AlertUserConfigurationsRequest, ResponseBase<bool>>(ApiUri.SEND_ALERT_USER_CONFIG, request);

                if (data != null && data.Data)
                {
                    result = data;
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }

        public async Task<AlertMaskModel> GetAlertMaskDetail(Guid id)
        {
            var result = new AlertMaskModel();
            try
            {
                var url = string.Format(ApiUri.GET_ALERT_MASK_DETAIL + "?id={0}", id);

                var data = await _IRequestProvider.GetAsync<ResponseBase<AlertMaskModel>>(url);

                if (data != null)
                {
                    result = data.Data;
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }
    }
}