using BA_MobileGPS.Entities;

using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IMotoPropertiesService
    {
        Task<ResponseBase<MotoPropertiesRespone>> GetMotoProperties(int xnCode, string vehiclePlate);
    }
}