﻿using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity.Support;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service.IService.Support;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.Service.Support
{
    public class SupportCategoryService : ISupportCategoryService
    {
        private readonly IRequestProvider _iRequestProvider;

        public SupportCategoryService(IRequestProvider iRequestProvider)
        {
            this._iRequestProvider = iRequestProvider;
        }

        public async Task<SupportBapRespone> Getfeedback(SupportBapRequest request)
        {
            SupportBapRespone result = new SupportBapRespone();
            try
            {
                var respone = await _iRequestProvider.PostAsync<SupportBapRequest, BaseResponse<SupportBapRespone>>(ApiUri.POST_MessageSupport, request);
                if (respone != null && respone.Data != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }

        public async Task<List<SupportCategoryRespone>> GetListSupportCategory()
        {
            List<SupportCategoryRespone> result = new List<SupportCategoryRespone>();
            try
            {
                var respone = await _iRequestProvider.GetAsync<ResponseBaseV2<List<SupportCategoryRespone>>>(ApiUri.GET_List_SupportCategory);
                if (respone != null && respone.Data != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }
        public async Task<List<MessageSupportRespone>> GetMessagesSupport(Guid id)
        {
            List<MessageSupportRespone> result = new List<MessageSupportRespone>();
            try
            {
                string uri = string.Format(ApiUri.GET_List_SupportContent + "?id={0}", id);
                var respone = await _iRequestProvider.GetAsync<ResponseBaseV2<List<MessageSupportRespone>>>(uri);
                if (respone != null && respone.Data != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }

    }
}