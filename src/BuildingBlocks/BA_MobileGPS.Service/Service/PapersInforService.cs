﻿using BA_MobileGPS.Entities;
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
            PaperInsuranceInsertRequest result = new PaperInsuranceInsertRequest();
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
            PaperRegistrationInsertRequest result = new PaperRegistrationInsertRequest();
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

        public async Task<PaperCabSignInsertRequest> GetLastPaperSignByVehicleId(int companyID, long vehicleId)
        {
            PaperCabSignInsertRequest result = new PaperCabSignInsertRequest();
            try
            {
                string url = $"{ApiUri.GET_LAST_PAPER_SIGN}?companyId={companyID}&vehicleId={vehicleId}";
                var response = await _IRequestProvider.GetAsync<ResponseBaseV2<PaperCabSignInsertRequest>>(url);
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
                string url = $"{ApiUri.POST_PAPER_INSURANCE}";
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
                string url = $"{ApiUri.POST_PAPER_REGISTRATION}";
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

        public async Task<PapersIdResponse> InsertSignPaper(PaperCabSignInsertRequest data)
        {
            PapersIdResponse result = new PapersIdResponse();
            try
            {
                string url = $"{ApiUri.POST_PAPER_SIGN}";
                var response = await _IRequestProvider.PostAsync<PaperCabSignInsertRequest, PapersInforInsertResponse>(url, data);
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

        
    }
}
