using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinInfowindowActiveView : Label
    {
        public PinInfowindowActiveView(string text = "")
        {
            InitializeComponent();

            Text = text;
            if (Device.RuntimePlatform == Device.iOS)
            {
                WidthRequest = (text.Trim().Length * 10) + 10;
            }
            else
            {
                WidthRequest = -1;
            }
        }
    }
}