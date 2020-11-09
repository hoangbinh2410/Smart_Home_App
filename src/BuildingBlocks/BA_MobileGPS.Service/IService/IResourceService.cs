using BA_MobileGPS.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IResourceService : IRealmBaseService<MobileResourceRealm, MobileResourceRespone>
    {
        Task<List<MobileResourceRespone>> GetAllResources(int appID, string Culture, long? LastTime);
    }
}