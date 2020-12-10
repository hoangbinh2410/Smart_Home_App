using BA_MobileGPS.Core.iOS.DependencyServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using ui = UIKit.UIApplication;

[assembly: Dependency(typeof(IconSwitchService))]

namespace BA_MobileGPS.Core.iOS.DependencyServices
{
    public class IconSwitchService : IIconSwitchService
    {
        public async Task SwitchAppIcon(string iconName)
        {
            if (ui.SharedApplication.SupportsAlternateIcons)
            {
                await ui.SharedApplication.SetAlternateIconNameAsync(iconName);
            }
        }
    }
}