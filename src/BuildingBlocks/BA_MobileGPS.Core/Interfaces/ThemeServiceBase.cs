using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Styles;
using BA_MobileGPS.Utilities.Enums;
using MonkeyCache;
using MonkeyCache.FileStore;
using System;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.DependencyServices
{
    public class ThemeServiceBase : IThemeService
    {     
        public void UpdateTheme(OSAppTheme oSAppTheme)
        {
            Application.Current.UserAppTheme = oSAppTheme;
        }
    }
}