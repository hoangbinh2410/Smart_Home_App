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
        }
    }
}