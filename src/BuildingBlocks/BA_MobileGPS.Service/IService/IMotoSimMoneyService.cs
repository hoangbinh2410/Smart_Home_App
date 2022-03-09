using BA_MobileGPS.Entities;

using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IMotoSimMoneyService
    {
        Task<ResponseBase<SimMoneyRespone>> GetSimMoney(long vehicleID);
    }
}