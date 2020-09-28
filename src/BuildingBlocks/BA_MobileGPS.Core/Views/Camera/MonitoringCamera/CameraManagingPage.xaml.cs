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
            cameraPanel.Children.Add(new Template4Camera());
            base.OnAppearing();
        }





    }
}
