using BA_MobileGPS.Utilities.Enums;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.DependencyServices
{
    public interface IThemeService
    {
        ResourceDictionary CustomColors { get; set; }
        ThemeMode AppTheme { get; set; }

        void UpdateTheme(ThemeMode themeMode = ThemeMode.Auto);
    }
}