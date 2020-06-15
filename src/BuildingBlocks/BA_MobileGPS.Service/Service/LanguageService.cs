using AutoMapper;

using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Infrastructure.Repository;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class LanguageService : RealmBaseService<LanguageRealm, LanguageRespone>, ILanguageService
    {
        private readonly IRequestProvider requestProvider;

        public LanguageService(IRequestProvider requestProvider, IBaseRepository baseRepository, IMapper mapper) : base(baseRepository, mapper)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<List<LanguageRespone>> GetAllLanguageType()
        {
            List<LanguageRespone> result = new List<LanguageRespone>();
            try
            {
                var data = await requestProvider.GetAsync<List<LanguageRespone>>(ApiUri.GET_LANGUAGETYPE);

                if (data != null)
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

        public async Task<bool> UpdateLanguageByUser(RequestUpdateLanguage model)
        {
            bool result = false;
            try
            {
                var data = await requestProvider.PostAsync<RequestUpdateLanguage, bool>(ApiUri.POST_UPDATELANGUAGEUSER, model);

                if (data)
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
    }
}