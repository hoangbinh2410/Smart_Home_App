using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinInfowindowView : Label
    {
        public PinInfowindowView(string text = "")
        {
            InitializeComponent();

            Text = text;
        }
    }
}