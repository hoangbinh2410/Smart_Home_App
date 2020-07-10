using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VMS_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VMSPinInfowindowView : Label
    {
        public VMSPinInfowindowView(string text = "")
        {
            InitializeComponent();

            Text = text;
        }
    }
}