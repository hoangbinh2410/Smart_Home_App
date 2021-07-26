using System.Threading.Tasks;

namespace BA_MobileGPS.Core.Interfaces
{
    public interface IAlert
    {
        Task<string> Display(string title, string message, string firstButton, string secondButton, string cancel);
    }
}