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