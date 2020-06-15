using BA_MobileGPS.Entities;

using System.Threading.Tasks;

namespace BA_MobileGPS
{
    public interface IDetailVehicleService
    {
        Task<VehicleOnlineDetailViewModel> LoadAllInforVehicle(DetailVehicleRequest input);

        Task<ShipDetailRespone> GetShipDetail(ShipDetailRequest input);
    }
}