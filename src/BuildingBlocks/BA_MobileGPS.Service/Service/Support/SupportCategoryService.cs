using BA_MobileGPS.Entities;
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
                var respone = await _iRequestProvider.PostAsync<SupportBapRequest, ResponseBase<SupportBapRespone>>(ApiUri.POST_MessageSupport, request);
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

        public async Task<List<SupportCategoryRespone>> GetListSupportCategory(int languageId)
        {
            List<SupportCategoryRespone> result = new List<SupportCategoryRespone>();
            var url = ApiUri.GET_List_SupportCategory + $"?languageID={languageId}";
            try
            {
                var respone = await _iRequestProvider.GetAsync<ResponseBase<List<SupportCategoryRespone>>>(url);
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
        public async Task<List<MessageSupportRespone>> GetMessagesSupport(Guid id, int languageId)
        {
            List<MessageSupportRespone> result = new List<MessageSupportRespone>();
            try
            {
                string uri = string.Format(ApiUri.GET_List_SupportContent + "?id={0}&languageID={1}",id,languageId);
                var respone = await _iRequestProvider.GetAsync<ResponseBase<List<MessageSupportRespone>>>(uri);
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
