using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class BACloudTab : ContentPage
    {
        public BACloudTab()
        {
            InitializeComponent();
            entrySearch.Placeholder = MobileResource.Online_Label_SeachVehicle2;
        }
    }
}