using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinInfowindowTimeView : Grid
    {
        public PinInfowindowTimeView(string text = "")
        {
            InitializeComponent();
            txt.Text = text;
        }
    }
}