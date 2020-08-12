using BA_MobileGPS.Core.Styles;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Themes
{
    public interface IThemeServices
    {
        bool ChangeTheme(Theme themes);
    }
    public class ThemeServices : IThemeServices
    {
        public bool ChangeTheme(Theme themes)
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new Converters());
            Application.Current.Resources.MergedDictionaries.Add(new Fonts());
            Application.Current.Resources.MergedDictionaries.Add(new Text());

            switch (themes)
            {
                case Theme.Dark:
                    var dark = PrismApplicationBase.Current.Container.Resolve<ResourceDictionary>(Theme.Dark.ToString());
                    Application.Current.Resources.MergedDictionaries.Add(dark);
                    Application.Current.Resources.MergedDictionaries.Add(new Styles.Styles());
                    Settings.CurrentTheme = Theme.Dark.ToString();
                    return true;

                case Theme.Light:
                    var light = PrismApplicationBase.Current.Container.Resolve<ResourceDictionary>(Theme.Light.ToString());
                    Application.Current.Resources.MergedDictionaries.Add(light);
                    Application.Current.Resources.MergedDictionaries.Add(new Styles.Styles());
                    Settings.CurrentTheme = Theme.Light.ToString();
                    return true;

                case Theme.Custom:
                    var custom = PrismApplicationBase.Current.Container.Resolve<ResourceDictionary>(Theme.Custom.ToString());
                    Application.Current.Resources.MergedDictionaries.Add(custom);
                    Application.Current.Resources.MergedDictionaries.Add(new Styles.Styles());
                    Settings.CurrentTheme = Theme.Custom.ToString();
                    return true;

                default:
                    return false;
            }
        }
    }
    public enum Theme
    {
        Dark,
        Light,
        Custom
    }
}