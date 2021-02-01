using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.Service
{
    public class PapersInforService : IPapersInforService
    {
        private readonly IRequestProvider _IRequestProvider;

        public PapersInforService(IRequestProvider IRequestProvider)
        {
            _IRequestProvider = IRequestProvider;
        }

        public async Task<PaperInsuranceInsertRequest> GetLastPaperInsuranceByVehicleId(int companyID, long vehicleId)
        {
            PaperInsuranceInsertRequest result = null;
            try
            {
                string url = $"{ApiUri.GET_LAST_PAPER_INSURANCE}?companyId={companyID}&vehicleId={vehicleId}";
                var response = await _IRequestProvider.GetAsync<ResponseBaseV2<PaperInsuranceInsertRequest>>(url);
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

        public async Task<PaperRegistrationInsertRequest> GetLastPaperRegistrationByVehicleId(int companyID, long vehicleId)
        {
            PaperRegistrationInsertRequest result = null;
            try
            {
                string url = $"{ApiUri.GET_LAST_PAPER_REGISTRATION}?companyId={companyID}&vehicleId={vehicleId}";
                var response = await _IRequestProvider.GetAsync<ResponseBaseV2<PaperRegistrationInsertRequest>>(url);
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

        public async Task<PaperCabSignInforRequest> GetLastPaperSignByVehicleId(int companyID, long vehicleId)
        {
            PaperCabSignInforRequest result = null;
            try
            {
                string url = $"{ApiUri.GET_LAST_PAPER_SIGN}?companyId={companyID}&vehicleId={vehicleId}";
                var response = await _IRequestProvider.GetAsync<ResponseBaseV2<PaperCabSignInforRequest>>(url);
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

        public async Task<List<PaperCategory>> GetPaperCategories()
        {
            List<PaperCategory> result = new List<PaperCategory>();
            try
            {
                string url = $"{ApiUri.GET_LIST_PAPER_CATEGORY}";
                var response = await _IRequestProvider.GetAsync<PaperCategoriesResponse>(url);
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

        public async Task<List<InsuranceCategory>> GetInsuranceCategories(int companyID)
        {
            List<InsuranceCategory> result = new List<InsuranceCategory>();
            try
            {
                string url = $"{ApiUri.GET_LIST_INSURANCE_CATEGORY}?companyId={companyID}";
                var response = await _IRequestProvider.GetAsync<InsuranceCategoriesResponse>(url);
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

        public async Task<PapersIdResponse> InsertInsurancePaper(PaperInsuranceInsertRequest data)
        {
            PapersIdResponse result = new PapersIdResponse();
            try
            {
                string url = $"{ApiUri.POST_INSERT_PAPER_INSURANCE}";
                var response = await _IRequestProvider.PostAsync<PaperInsuranceInsertRequest, PapersInforInsertResponse>(url, data);
                if (response != null && response.Data != null)
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

        public async Task<PapersIdResponse> InsertRegistrationPaper(PaperRegistrationInsertRequest data)
        {
            PapersIdResponse result = new PapersIdResponse();
            try
            {
                string url = $"{ApiUri.POST_INSERT_PAPER_REGISTRATION}";
                var response = await _IRequestProvider.PostAsync<PaperRegistrationInsertRequest, PapersInforInsertResponse>(url, data);
                if (response != null && response.Data != null)
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

        public async Task<PapersIdResponse> InsertSignPaper(PaperCabSignInforRequest data)
        {
            PapersIdResponse result = new PapersIdResponse();
            try
            {
                string url = $"{ApiUri.POST_INSERT_PAPER_SIGN}";
                var response = await _IRequestProvider.PostAsync<PaperCabSignInforRequest, PapersInforInsertResponse>(url, data);
                if (response != null && response.Data != null)
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

        public async Task<PapersIdResponse> UpdateRegistrationPaper(PaperRegistrationInsertRequest data)
        {

            PapersIdResponse result = new PapersIdResponse();
            try
            {
                string url = $"{ApiUri.POST_UPDATE_PAPER_REGISTRATION}";
                var response = await _IRequestProvider.PostAsync<PaperRegistrationInsertRequest, PapersInforInsertResponse>(url, data);
                if (response != null && response.Data != null)
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

        public async Task<PapersIdResponse> UpdateInsurancePaper(PaperInsuranceInsertRequest data)
        {

            PapersIdResponse result = new PapersIdResponse();
            try
            {
                string url = $"{ApiUri.POST_UPDATE_PAPER_INSURANCE}";
                var response = await _IRequestProvider.PostAsync<PaperInsuranceInsertRequest, PapersInforInsertResponse>(url, data);
                if (response != null && response.Data != null)
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

        public async Task<PapersIdResponse> UpdateSignPaper(PaperCabSignInforRequest data)
        {

            PapersIdResponse result = new PapersIdResponse();
            try
            {
                string url = $"{ApiUri.POST_UPDATE_PAPER_SIGN}";
                var response = await _IRequestProvider.PostAsync<PaperCabSignInforRequest, PapersInforInsertResponse>(url, data);
                if (response != null && response.Data != null)
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

        public async Task<List<PaperItemInfor>> GetListPaper(int companyId, int OrderBy = 0, int sortOrder = 0)
        {
            List<PaperItemInfor> result = new List<PaperItemInfor>();
            try
            {
                string url = string.Empty;
                if (OrderBy ==0 && sortOrder==0)
                {
                    url = $"{ApiUri.GET_LIST_ALL_PAPER}?companyId={companyId}";
                }
                else url = $"{ApiUri.GET_LIST_ALL_PAPER}?companyId={companyId}&OrderBy={OrderBy}&SortOder={sortOrder}";

                var response = await _IRequestProvider.GetAsync<ListPaperResponse>(url);
                if (response?.Data != null && response.Data.Count > 0)
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
    }
}
