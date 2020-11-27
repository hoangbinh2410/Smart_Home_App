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
                    FontFamily = "FontAwesomeSolid";
                    break;

                case FontAttributes.Bold:
                    FontFamily = "FontAwesomeSolid"; 
                    break;

                case FontAttributes.Italic:
                    FontFamily = "FontAwesomeBrands";
                    break;

                default:
                    FontFamily = "FontAwesomeSolid";
                    break;
            }
        }
    }
}