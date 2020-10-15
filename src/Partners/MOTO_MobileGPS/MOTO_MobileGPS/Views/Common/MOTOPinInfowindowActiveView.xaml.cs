
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MOTO_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MOTOPinInfowindowActiveView : Label
    {
        public MOTOPinInfowindowActiveView(string text = "")
        {
            InitializeComponent();

            Text = text;
        }
    }
}