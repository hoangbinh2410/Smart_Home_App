using BA_MobileGPS.Core.Views.Camera.MonitoringImage;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class ImageManagingPage : ContentPage
    {
        public ImageManagingPage()
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
            ImagePanel.Children.Add(new Template4Image());
            base.OnAppearing();
        }

    }
}
