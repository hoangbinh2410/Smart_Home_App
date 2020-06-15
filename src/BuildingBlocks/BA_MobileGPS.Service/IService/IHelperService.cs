using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;

using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IHelperService
    {
        Task<HeplerModel> GetHelper(HelperRequest request);
    }
}