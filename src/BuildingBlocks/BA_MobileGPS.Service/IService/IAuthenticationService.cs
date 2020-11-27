using BA_MobileGPS.Entities;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> LoginStreamAsync(LoginRequest request);

        Task<bool> ChangePassword(ChangePassRequest request);
    }
}