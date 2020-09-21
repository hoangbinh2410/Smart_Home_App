using Prism.Events;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Ioc;
using System;
using Prism.Navigation;

namespace BA_MobileGPS.Core.Views.Camera.MonitoringCamera
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Template4Camera : ContentView, IDestructible
    {
        private  IEventAggregator eventAggregator { get; } = Prism.PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
        public Template4Camera()
        {
            InitializeComponent();
            //eventAggregator.GetEvent<CameraLoadedEvent>

        }


        private void SetVideoViewSize()
        {


        }

        public void Destroy()
        {
            //throw new NotImplementedException();
        }
    }

    public class CameraLoadedEvent : PubSubEvent
    {

    }
}