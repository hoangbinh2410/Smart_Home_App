using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Infrastructure.Repository;
using BA_MobileGPS.Entities.RealmEntity;
using BA_MobileGPS.Service.Utilities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class DBVersionService : RealmBaseService<DBLocalVersionRealm, DatabaseVersionsResponse>, IDBVersionService
    {
        private readonly IRequestProvider RequestProvider;

        public DBVersionService(IRequestProvider requestProvider, IBaseRepository baseRepository, IMapper mapper) : base(baseRepository, mapper)
        {
            RequestProvider = requestProvider;
        }

        public async Task<MobileVersionModel> GetMobileVersion(string operatingSystem, int appID)
        {
            MobileVersionModel result = new MobileVersionModel();
            try
            {
                string uri = string.Format(ApiUri.GET_MOBILEVERSION + "?os={0}&appID={1}", operatingSystem, appID);

                var data = await RequestProvider.GetAsync<MobileVersionModel>(uri);

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

        public async Task<List<DatabaseVersionsResponse>> GetVersionDataBase(int appID)
        {
            var result = new List<DatabaseVersionsResponse>();
            try
            {
                string uri = string.Format(ApiUri.GET_DATABASEVERSION + "?appID={0}", appID);

                var data = await RequestProvider.GetAsync<List<DatabaseVersionsResponse>>(uri);

                if (data != null && data.Count > 0)
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