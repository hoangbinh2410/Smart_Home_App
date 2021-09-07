using BA_MobileGPS.Utilities;

using System.Globalization;

namespace BA_MobileGPS.Core
{
    public static class CultureHelper
    {
        public static void SetCulture()
        {
            if (Settings.CurrentLanguage == CultureCountry.English)
            {
                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
                CultureInfo.CurrentCulture = new CultureInfo("en-US");
                CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            }
            else if (Settings.CurrentLanguage == CultureCountry.Laos)
            {
                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("lo-LA");
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("lo-LA");
                CultureInfo.CurrentCulture = new CultureInfo("lo-LA");
                CultureInfo.CurrentUICulture = new CultureInfo("lo-LA");
            }
            else
            {
                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("vi-VN");
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("vi-VN");
                CultureInfo.CurrentCulture = new CultureInfo("vi-VN");
                CultureInfo.CurrentUICulture = new CultureInfo("vi-VN");
            }
        }
    }
}