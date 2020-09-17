using BA_MobileGPS.Entities;

using System.Threading.Tasks;

namespace BA_MobileGPS
{
    public interface IDetailVehicleService
    {
        Task<VehicleDetailViewModel> GetVehicleDetail(DetailVehicleRequest input);

        Task<ShipDetailRespone> GetShipDetail(ShipDetailRequest input);
    }
}