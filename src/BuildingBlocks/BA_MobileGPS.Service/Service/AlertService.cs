using BA_MobileGPS.Entities;
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
        public async Task<List<AlertTypeModel>> GetAlertTypeAsync(Guid userId, string cultureName = "en")
        {
            List<AlertTypeModel> result = new List<AlertTypeModel>();
            try
            {
                string url = $"{ApiUri.GET_ALERT_TYPE}?cultureName={cultureName}&userId={userId}";
                var response = await _IRequestProvider.GetAsync<List<AlertTypeModel>>(url);
                if (result != null)
                {
                    result = response;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<int> GetCountAlert(Guid PK_UserID)
        {
            int result = 0;
            try
            {
                string url = string.Format(ApiUri.GET_COUNT_ALERT_ONLINE + "/?userID={0}", PK_UserID);

                var response = await _IRequestProvider.GetAsync<int>(url);

                if (response > 0)
                {
                    result = response;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return result;
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
        public async Task<AlertOnlineViewModel> GetListAlertOnlineAsync(AlertGetRequest request)
        {
            AlertOnlineViewModel result = new AlertOnlineViewModel();
            try
            {
                string url = $"{ApiUri.GET_ALERT_ONLINE}";
                var data = await _IRequestProvider.PostAsync<AlertGetRequest, AlertOnlineViewModel>(url, request);
                if (result != null)
                {
                    result = data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return result;
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
            bool result = false;
            try
            {
                string url = $"{ApiUri.POST_ALERT_HANDLE}";
                result = await _IRequestProvider.PostAsync<StatusAlertRequestModel, bool>(url, rqModel);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return result;
        }
    }
}