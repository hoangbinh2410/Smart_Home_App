using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Styles;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;

namespace GSHT_MobileGPS.Core.Themes
{
    public interface IThemeGSHTServices
    {
        bool ChangeTheme(ThemeGSHT themes);
    }

    public class ThemeServices : IThemeGSHTServices
    {
        public bool ChangeTheme(ThemeGSHT themes)
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new Converters());
            Application.Current.Resources.MergedDictionaries.Add(new Fonts());
            switch (themes)
            {
                case ThemeGSHT.Theme1:
                    var th1 = PrismApplicationBase.Current.Container.Resolve<ResourceDictionary>(ThemeGSHT.Theme1.ToString());
                    Application.Current.Resources.MergedDictionaries.Add(th1);
                    Application.Current.Resources.MergedDictionaries.Add(new BA_MobileGPS.Core.Styles.Styles());
                    Settings.CurrentTheme = (int)ThemeGSHT.Theme1;
                    return true;

                case ThemeGSHT.Theme2:
                    var th2 = PrismApplicationBase.Current.Container.Resolve<ResourceDictionary>(ThemeGSHT.Theme2.ToString());
                    Application.Current.Resources.MergedDictionaries.Add(th2);
                    Application.Current.Resources.MergedDictionaries.Add(new BA_MobileGPS.Core.Styles.Styles());
                    Settings.CurrentTheme = (int)ThemeGSHT.Theme2;
                    return true;

                case ThemeGSHT.Theme3:
                    var th3 = PrismApplicationBase.Current.Container.Resolve<ResourceDictionary>(ThemeGSHT.Theme3.ToString());
                    Application.Current.Resources.MergedDictionaries.Add(th3);
                    Application.Current.Resources.MergedDictionaries.Add(new BA_MobileGPS.Core.Styles.Styles());
                    Settings.CurrentTheme = (int)ThemeGSHT.Theme3;
                    return true;

                default:
                    return false;
            }
        }
    }

    public enum ThemeGSHT
    {
        Theme1,
        Theme2,
        Theme3
    }
}