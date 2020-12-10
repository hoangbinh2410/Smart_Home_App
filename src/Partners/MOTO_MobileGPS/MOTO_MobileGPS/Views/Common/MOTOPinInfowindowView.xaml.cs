using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MOTO_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MOTOPinInfowindowView : Label
    {
        public MOTOPinInfowindowView(string text = "")
        {
            InitializeComponent();

            Text = text;
            if (Device.RuntimePlatform == Device.iOS)
            {
                var lenght = text.Trim().Length;
                if (lenght >= 10)
                {
                    WidthRequest = (text.Trim().Length * 8.5);
                }
                else if (lenght <= 5)
                {
                    WidthRequest = (text.Trim().Length * 15);
                }
                else
                {
                    WidthRequest = (text.Trim().Length * 9);
                }
            }
            else
            {
                WidthRequest = -1;
            }
        }
    }
}