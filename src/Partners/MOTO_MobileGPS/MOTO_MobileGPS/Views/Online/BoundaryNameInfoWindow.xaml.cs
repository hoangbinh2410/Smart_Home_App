using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MOTO_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BoundaryNameInfoWindow : Label
    {
        public BoundaryNameInfoWindow(string text = "")
        {
            InitializeComponent();

            Text = text;
        }
    }
}