using BA_MobileGPS.Entities;

using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IMotoDetailService
    {
        Task<ResponseBase<MotoDetailRespone>> GetMotoDetail(int xnCode, string vehiclePlate);
    }
}