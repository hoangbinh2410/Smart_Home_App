using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.Views.Camera.MonitoringCamera;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class CameraManagingPage : ContentPage
    {
        public CameraManagingPage()
        {
            try
            {                
                InitializeComponent();                
            }
            catch (System.Exception ex)
            {

                throw;
            }                       
        }
        protected override void OnAppearing()
        {
            entrySearch.Placeholder = MobileResource.Route_Label_SearchFishing;
            cameraPanel.Children.Add(new Template4Camera());
            base.OnAppearing();
        }





    }
}
