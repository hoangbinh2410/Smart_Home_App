using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class MyVideoTab : ContentPage
    {
        public MyVideoTab()
        {
            InitializeComponent();
            title.Text = MobileResource.Camera_ChildTab_BACloud;
            entrySearch.Placeholder = MobileResource.Online_Label_SeachVehicle2;
        }
    }
}