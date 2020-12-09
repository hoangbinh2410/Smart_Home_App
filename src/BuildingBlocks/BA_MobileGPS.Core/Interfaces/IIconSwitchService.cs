using System.Threading.Tasks;

namespace BA_MobileGPS.Core
{
    public interface IIconSwitchService
    {
        Task SwitchAppIcon(string iconName);
    }
}