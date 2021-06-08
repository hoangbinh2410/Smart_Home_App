using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class MyVideoTab : ContentPage
    {
        public MyVideoTab()
        {
            InitializeComponent();
            entrySearch.Placeholder = MobileResource.Route_Label_SearchFishing;
        }
    }
}