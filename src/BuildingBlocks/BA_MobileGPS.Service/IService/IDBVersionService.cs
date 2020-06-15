using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RealmEntity;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IDBVersionService : IRealmBaseService<DBLocalVersionRealm, DatabaseVersionsResponse>
    {
        Task<MobileVersionModel> GetMobileVersion(string operatingSystem, int appID);

        Task<List<DatabaseVersionsResponse>> GetVersionDataBase(int appID);
    }
}