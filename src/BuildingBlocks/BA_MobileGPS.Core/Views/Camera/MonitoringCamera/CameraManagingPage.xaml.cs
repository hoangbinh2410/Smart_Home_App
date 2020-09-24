using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.Views.Camera.MonitoringCamera;
using Prism.Mvvm;
using Syncfusion.ListView.XForms.Control.Helpers;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Markup;

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
            var cam = new Template4Camera();
            ViewModelLocator.SetAutowirePartialView(cam, this);
            cameraPanel.Children.Add(new Template4Camera());
            base.OnAppearing();
        }

    }
}
