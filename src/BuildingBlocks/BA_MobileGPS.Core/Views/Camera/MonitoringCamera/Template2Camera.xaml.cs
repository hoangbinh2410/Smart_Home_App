using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Navigation;

namespace BA_MobileGPS.Core.Views.Camera.MonitoringCamera
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Template2Camera : ContentView
    {
        private IEventAggregator eventAggregator { get; } = Prism.PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();

        public Template2Camera()
        {
            InitializeComponent();
            eventAggregator.GetEvent<SwitchToNormalScreenEvent>().Subscribe(SwitchToNormal);
            eventAggregator.GetEvent<SwitchToFullScreenEvent>().Subscribe(FullScreen);
            eventAggregator.GetEvent<DisposeTemplateView>().Subscribe(ClearView);

        }

        public void ClearView()
        {
            try
            {
                eventAggregator.GetEvent<SwitchToFullScreenEvent>().Unsubscribe(FullScreen);
                eventAggregator.GetEvent<SwitchToNormalScreenEvent>().Unsubscribe(SwitchToNormal);
                eventAggregator.GetEvent<DisposeTemplateView>().Unsubscribe(ClearView);
            }
            catch (Exception ex)
            {

                
            }
         
        }
        private void SwitchToNormal()
        {
            parentPanel.RowDefinitions.Clear();
            parentPanel.RowDefinitions.Add(new RowDefinition());
            parentPanel.RowDefinitions.Add(new RowDefinition());
        }

        private void FullScreen(CameraEnum obj)
        {
            parentPanel.RowDefinitions.Clear();
            switch (obj)
            {
                case CameraEnum.CAM1:
                    FullScreenCam1();
                    break;
                case CameraEnum.CAM2:
                    FullScreenCam2();
                    break;
                default:
                    break;
            }
        }
        private void FullScreenCam1()
        {
            parentPanel.RowDefinitions.Add(new RowDefinition());
            parentPanel.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0) });
        }
        private void FullScreenCam2()
        {
            parentPanel.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0) });
            parentPanel.RowDefinitions.Add(new RowDefinition());
        }
    }
}