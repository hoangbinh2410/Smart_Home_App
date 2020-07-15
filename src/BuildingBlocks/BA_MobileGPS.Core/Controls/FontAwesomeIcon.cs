using Xamarin.Forms;

namespace BA_MobileGPS.Core.Controls
{
    public class FontAwesomeIcon : Label
    {
        public FontAwesomeIcon()
        {
            FontSize = 20;
            switch (FontAttributes)
            {
                case FontAttributes.None:
                    FontFamily = (OnPlatform<string>)App.Current.Resources["FontAwesomeSolid"]; //iOS is happy with this, Android needs a renderer to add ".ttf"
                    break;

                case FontAttributes.Bold:
                    FontFamily = (OnPlatform<string>)App.Current.Resources["FontAwesomeBrands"]; //iOS is happy with this, Android needs a renderer to add ".ttf"
                    break;

                case FontAttributes.Italic:
                    FontFamily = (OnPlatform<string>)App.Current.Resources["FontAwesomeSolid"]; //iOS is happy with this, Android needs a renderer to add ".ttf"
                    break;

                default:
                    FontFamily = (OnPlatform<string>)App.Current.Resources["FontAwesomeSolid"]; //iOS is happy with this, Android needs a renderer to add ".ttf"
                    break;
            }
        }
    }
}