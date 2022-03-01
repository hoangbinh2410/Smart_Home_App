using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class DriverInforService : IDriverInforService
    {
        private readonly IRequestProvider _IRequestProvider;

        public DriverInforService(IRequestProvider IRequestProvider)
        {
            _IRequestProvider = IRequestProvider;
        }
        public async Task<int> AddDriverInfor(DriverInfor driver)
        {
            int result = -1;
            try
            {
                string url = $"{ApiUri.POST_ADD_DRIVER}";
                var response = await _IRequestProvider.PostAsync<DriverInfor, ResponseBase<int>>(url, driver);
                if (response != null )
                {
                    result = response.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<int> DeleteDriverInfor(DriverDeleteRequest driver)
        {
            int result = -1;
            try
            {
                string url = $"{ApiUri.POST_DELETE_DRIVER}";
                var response = await _IRequestProvider.PostAsync<DriverDeleteRequest, ResponseBase<int>>(url, driver);
                if (response != null)
                {
                    result = response.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }



        public async Task<List<DriverInfor>> GetListDriverByCompanyId(int companyId, int pageSize = 20, int pageIndex = 1,
            DriverOrderByEnum orderBy = DriverOrderByEnum.ASC, DriverSortOderEnum sortOrder = DriverSortOderEnum.DisplayName)
        {
            List<DriverInfor> result = new List<DriverInfor>();
            try
            {
                string url = $"{ApiUri.GET_LIST_DRIVER}?companyId={companyId}";
                var response = await _IRequestProvider.GetAsync<ResponseBase<List<DriverInfor>>>(url);
                if (response?.Data != null)
                {
                    result = response.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<int> UpdateDriverInfor(DriverInfor driver)
        {
            int result = -1;
            try
            {
                string url = $"{ApiUri.POST_UPDATE_DRIVER}";
                var response = await _IRequestProvider.PostAsync<DriverInfor, ResponseBase<DriverInfor>>(url, driver);
                if (response != null && response.Data != null)
                {
                    result = response.Data.PK_EmployeeID;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }
    }
}
