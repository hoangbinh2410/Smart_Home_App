using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.Reflection;
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
        
        public async Task<List<PaperCategory>> GetPaperCategories()
        {
            List<PaperCategory> result = new List<PaperCategory>();
            try
            {
                string url = $"{ApiUri.GET_LIST_PAPER_CATEGORY}";
                var response = await _IRequestProvider.GetAsync<PaperCategoriesResponse>(url);
                if (response != null && response.Data.Count > 0)
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
                string url = $"{ApiUri.GET_LIST_INSURANCE_CATEGORY}?companyID={companyID}";
                var response = await _IRequestProvider.GetAsync<InsuranceCategoriesResponse>(url);
                if (response != null && response.Data.Count > 0)
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
                var response = await _IRequestProvider.PutAsync<PaperRegistrationInsertRequest, PapersInforInsertResponse>(url, data);
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
                var response = await _IRequestProvider.PutAsync<PaperInsuranceInsertRequest, PapersInforInsertResponse>(url, data);
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
                var response = await _IRequestProvider.PutAsync<PaperCabSignInforRequest, PapersInforInsertResponse>(url, data);
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
                if (OrderBy == 0 && sortOrder == 0)
                {
                    url = $"{ApiUri.GET_LIST_ALL_PAPER}?FK_CompanyID={companyId}";
                }
                else url = $"{ApiUri.GET_LIST_ALL_PAPER}?FK_CompanyID={companyId}&OrderBy={OrderBy}&SortOrder={sortOrder}";

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

        public async Task<List<PaperItemHistoryModel>> GetListPaperHistory(int companyId, int pageSize = 0, int pageIndex = 0, int orderBy = 0, int sortOrder = 0)
        {
            List<PaperItemHistoryModel> result = new List<PaperItemHistoryModel>();
            try
            {
                var url = $"{ApiUri.GET_LIST_ALL_PAPER_HISTORY}?FK_CompanyID={companyId}&PageSize={pageSize}&PageIndex={pageIndex}&OrderBy={orderBy}&SortOrder={sortOrder}";

                var response = await _IRequestProvider.GetAsync<ResponseBase<List<PaperItemHistoryModel>>>(url);
                if (response != null &&response?.Data != null && response.Data.Count > 0)
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

        public async Task<DateTime?> GetLastPaperDateByVehicle(int companyId, long vehicleId, PaperCategoryTypeEnum paperType)
        {
            DateTime? result = null;
            try
            {
               // var type = (int)paperType;
                var url = $"{ApiUri.GET_LAST_PAPER_DATE_BY_VEHICLE}?FK_VehicleID={vehicleId}&FK_CompanyID={companyId}&PaperCategoryType={paperType}";

                var response = await _IRequestProvider.GetAsync<ResponseBase<DateTime?>>(url);
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

        public async Task<PaperInfoDetailResponse> GetLastPaperByVehicleId(int companyID, PaperCategoryTypeEnum paperType, long vehicleId)
        {
            PaperInfoDetailResponse result = null;
            try
            {
                string url = $"{ApiUri.GET_LAST_PAPER_PaperCategory}?FK_VehicleID={vehicleId}&PaperCategoryType={paperType}&FK_CompanyID={companyID}";
                var response = await _IRequestProvider.GetAsync<ResponseBase<PaperInfoDetailResponse>>(url);
                if (response.Data != null && response != null)
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