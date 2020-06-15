using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IPlacesGeocode
    {
        Task<Geocode> GetGeocode(string input);

        Task<Geocode> GetAddressesForPosition(string lat, string lng);
    }
}