using Prism.Events;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Ioc;
using System;
using Prism.Navigation;
using System.Collections.Generic;

namespace BA_MobileGPS.Core.Views.Camera.MonitoringCamera
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Template4Camera : ContentView, IDestructible
    {
        private IEventAggregator eventAggregator { get; } = Prism.PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
        public Template4Camera()
        {
            InitializeComponent();
            eventAggregator.GetEvent<HideVideoViewEvent>().Subscribe(HideVideoView);
            eventAggregator.GetEvent<ShowVideoViewEvent>().Subscribe(ShowVideoView);
        }

        private void ShowVideoView(List<CameraEnum> obj)
        {
            foreach (var item in obj)
            {
                switch (item)
                {
                    case CameraEnum.FirstCamera:
                        frCam1.IsEnabled = true;
                        topLeftCam.IsVisible = true;
                        break;
                    case CameraEnum.SecondCamera:
                        frCam1.IsEnabled = true;
                        bottomLeftCam.IsVisible = true;
                        break;
                    case CameraEnum.ThirdCamera:
                        frCam1.IsEnabled = true;
                        topRightCam.IsVisible = true;
                        break;
                    case CameraEnum.FourthCamera:
                        frCam1.IsEnabled = true;
                        bottomRightCam.IsVisible = true;
                        break;
                }
            }
        }

        private void HideVideoView(List<CameraEnum> obj)
        {
            foreach (var item in obj)
            {
                switch (item)
                {
                    case CameraEnum.FirstCamera:                 
                        frCam1.IsEnabled = false;
                        topLeftCam.IsVisible = false;
                        break;
                    case CameraEnum.SecondCamera:                
                        frCam2.IsEnabled = false;
                        bottomLeftCam.IsVisible = false;
                        break;
                    case CameraEnum.ThirdCamera:                   
                        frCam3.IsEnabled = false;
                        topRightCam.IsVisible = false;
                        break;
                    case CameraEnum.FourthCamera:                       
                        frCam4.IsEnabled = false;
                        bottomRightCam.IsVisible = false;
                        break;
                }
            }
        }

        public void Destroy()
        {
            eventAggregator.GetEvent<HideVideoViewEvent>().Unsubscribe(HideVideoView);
            eventAggregator.GetEvent<ShowVideoViewEvent>().Unsubscribe(ShowVideoView);
        }
    }

    public class ShowVideoViewEvent : PubSubEvent<List<CameraEnum>>
    {

    }
    public class HideVideoViewEvent : PubSubEvent<List<CameraEnum>>
    {

    }
}