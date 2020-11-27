using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RouteListPage : ContentPage
    {
        public RouteListPage()
        {
            InitializeComponent();
            this.Title = MobileResource.Route_Label_ListTitle;
        }

        protected override void OnAppearing()
        {
            lblTime.Text = MobileResource.Route_Label_Time;
            lblVgps.Text = MobileResource.Route_Label_Vgps;
            lblStatus.Text = MobileResource.Route_Label_Status;
            base.OnAppearing();
        }
    }
}