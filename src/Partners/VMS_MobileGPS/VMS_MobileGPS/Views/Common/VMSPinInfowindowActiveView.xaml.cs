using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VMS_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VMSPinInfowindowActiveView : Label
    {
        public VMSPinInfowindowActiveView(string text = "")
        {
            InitializeComponent();

            Text = text;
        }
    }
}