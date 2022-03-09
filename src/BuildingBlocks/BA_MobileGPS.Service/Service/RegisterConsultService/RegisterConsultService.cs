using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class RegisterConsultService : IRegisterConsultService
    {
        private readonly IRequestProvider _IRequestProvider;

        public RegisterConsultService(IRequestProvider requestProvider)
        {
            this._IRequestProvider = requestProvider;
        }

        /// <summary>
        /// đẩy dữ liệu đăng ký xuống db
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  2/25/2019   created
        /// </Modified>
        public async Task<int> RegisterConsultRequest(RegisterConsultRequest input)
        {
            return await _IRequestProvider.PostAsync<RegisterConsultRequest, int>(ApiUri.REGISTERCONSULT, input);
        }

        public async Task<List<ProvincesRegisterConsult>> GetDataTransportType(string currentLanguage)
        {
            var respone = new List<ProvincesRegisterConsult>();
            try
            {
                var temp = await _IRequestProvider.GetAsync<ResponseBase<List<ProvincesRegisterConsult>>>($"{ApiUri.GET_LISTTRANSPORTTYPES}?culture={currentLanguage}");
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

        public async Task<List<GisProvince_RegisterConsult>> GetDataProvince(string currentLanguage)
        {
            var respone = new List<GisProvince_RegisterConsult>();
            try
            {
                var temp = await _IRequestProvider.GetAsync<ResponseBase<List<GisProvince_RegisterConsult>>>($"{ApiUri.GET_LISTPROVINCES}?culture={currentLanguage}");
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
    }
}