using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RealmEntity;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IDBVersionService : IRealmBaseService<DBLocalVersionRealm, DatabaseVersionsResponse>
    {
        Task<List<DatabaseVersionsResponse>> GetVersionDataBase(int appID);
    }
}