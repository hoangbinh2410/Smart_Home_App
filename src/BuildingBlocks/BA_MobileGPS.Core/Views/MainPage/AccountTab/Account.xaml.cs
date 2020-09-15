using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class Account : ContentView
    {
        public Account()
        {
            InitializeComponent();
            lblMyInformation.Text = MobileResource.AccountTab_Label_MyInformation;
        }
    }
}