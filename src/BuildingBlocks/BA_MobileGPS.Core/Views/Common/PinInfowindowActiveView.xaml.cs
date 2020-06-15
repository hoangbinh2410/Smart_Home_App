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
        }
    }
}