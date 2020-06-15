using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IPlacesAutocomplete
    {
        Task<Predictions> GetAutocomplete(string input);
    }
}