using BA_MobileGPS.Core.Styles;
using BA_MobileGPS.Utilities.Enums;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Extensions
{
    public static class ThemeExtensions
    {
        public static ResourceDictionary ToResourceDictionary(this ThemeMode mode, ResourceDictionary customColors = default)
        {
            switch (mode)
            {
                case ThemeMode.Dark:
                    return new DarkTheme();

                case ThemeMode.Light:
                    return new LightTheme();

                default:
                    return customColors;
            }
        }
    }
}