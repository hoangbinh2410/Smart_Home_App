using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
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