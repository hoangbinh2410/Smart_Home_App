using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Account : ContentView
    {
        public Account()
        {
            InitializeComponent();
            lblMyInformation.Text = MobileResource.AccountTab_Label_MyInformation;
        }
    }
}