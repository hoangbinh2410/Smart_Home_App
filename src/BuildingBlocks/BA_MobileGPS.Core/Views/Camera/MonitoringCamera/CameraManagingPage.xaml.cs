using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.Views.Camera.MonitoringCamera;
using Prism.Mvvm;
using Syncfusion.ListView.XForms.Control.Helpers;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Markup;

using Prism.Ioc;
using Prism.Events;
using Prism.Navigation;
using System;

namespace BA_MobileGPS.Core.Views
{
    public partial class CameraManagingPage : ContentPage, IDestructible
    {
        private IEventAggregator eventAggregator { get; } = Prism.PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
        public CameraManagingPage()
        {
            try
            {                
                InitializeComponent();
                eventAggregator.GetEvent<SwitchToFullScreenEvent>().Subscribe(SwitchToFullScreen);
                eventAggregator.GetEvent<SwitchToNormalScreenEvent>().Subscribe(SwitchToNormal);
            }
            catch (System.Exception ex)
            {

                throw;
            }                       
        }

        private void SwitchToNormal()
        {
            Grid.SetRow(playbackControl, 3);
        }

        private void SwitchToFullScreen(CameraEnum obj)
        {
            Grid.SetRow(playbackControl, 2);
        }

        protected override void OnAppearing()
        {

            entrySearch.Placeholder = MobileResource.Route_Label_SearchFishing;
            var cam = new Template4Camera();
            ViewModelLocator.SetAutowirePartialView(cam, this);
            cameraPanel.Children.Add(new Template4Camera());
            base.OnAppearing();
        }

        public void Destroy()
        {
            eventAggregator.GetEvent<SwitchToFullScreenEvent>().Unsubscribe(SwitchToFullScreen);
            eventAggregator.GetEvent<SwitchToNormalScreenEvent>().Unsubscribe(SwitchToNormal);
        }
    }
}
