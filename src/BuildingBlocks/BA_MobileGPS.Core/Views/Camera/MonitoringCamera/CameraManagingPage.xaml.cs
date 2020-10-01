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
                eventAggregator.GetEvent<SetCameraLayoutEvent>().Subscribe(SetCameraLayout);
            }
            catch (System.Exception ex)
            {

                throw;
            }                       
        }

        private void SetCameraLayout(int obj)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                cameraPanel.Children.Clear();
                if (obj == 1)
                {
                    var cam = new Template1Camera();
                    ViewModelLocator.SetAutowirePartialView(cam, this);
                    cameraPanel.Children.Add(cam);
                }
                else if (obj == 2)
                {
                    var cam = new Template2Camera();
                    ViewModelLocator.SetAutowirePartialView(cam, this);
                    cameraPanel.Children.Add(cam);
                }
                else if(obj == 4)
                {
                    var cam = new Template4Camera();
                    ViewModelLocator.SetAutowirePartialView(cam, this);
                    cameraPanel.Children.Add(cam);
                }
                else if (obj == 0)
                {
                    var a = new Label();
                    a.Text = " KHÔNG CÓ CAMERA";
                    a.FontSize = 20;
                    a.HorizontalOptions = LayoutOptions.Center;
                    a.VerticalOptions = LayoutOptions.Center;
                    a.TextColor = Color.Red;
                    cameraPanel.Children.Add(a);
                }
            });
        
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
            base.OnAppearing();
        }

        public void Destroy()
        {
            eventAggregator.GetEvent<SwitchToFullScreenEvent>().Unsubscribe(SwitchToFullScreen);
            eventAggregator.GetEvent<SwitchToNormalScreenEvent>().Unsubscribe(SwitchToNormal);
            eventAggregator.GetEvent<SetCameraLayoutEvent>().Unsubscribe(SetCameraLayout);
        }
    }

    public class SetCameraLayoutEvent : PubSubEvent<int>
    {

    }
}
