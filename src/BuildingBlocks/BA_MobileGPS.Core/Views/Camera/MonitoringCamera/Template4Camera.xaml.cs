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
            eventAggregator.GetEvent<SwitchToFullScreenEvent>().Subscribe(FullScreen);
            eventAggregator.GetEvent<SwitchToNormalScreenEvent>().Subscribe(SwitchToNormal);
        }    

        public void Destroy()
        {
            eventAggregator.GetEvent<SwitchToFullScreenEvent>().Unsubscribe(FullScreen);
            eventAggregator.GetEvent<SwitchToNormalScreenEvent>().Unsubscribe(SwitchToNormal);
        }

      

        private void FullScreen(CameraEnum obj)
        {
            parentPanel.ColumnDefinitions.Clear();
            parentPanel.RowDefinitions.Clear();
            switch (obj)
            {
                case CameraEnum.CAM1:                    
                    FullScreenCam1();
                    break;
                case CameraEnum.CAM2:
                    FullScreenCam2();
                    break;
                case CameraEnum.CAM3:
                    FullScreenCam3();
                    break;
                case CameraEnum.CAM4:
                    FullScreenCam4();
                    break;
            }
        }

        private void FullScreenCam1()
        {         
            parentPanel.ColumnDefinitions.Add(new ColumnDefinition());
            parentPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0) });
            
            parentPanel.RowDefinitions.Add(new RowDefinition());
            parentPanel.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0) });
        }
        private void FullScreenCam2()
        {

            parentPanel.ColumnDefinitions.Add(new ColumnDefinition());
            parentPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0) });
                      
            parentPanel.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0) });
            parentPanel.RowDefinitions.Add(new RowDefinition());
        }
        private void FullScreenCam3()
        {           
            parentPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0) });
            parentPanel.ColumnDefinitions.Add(new ColumnDefinition());

            parentPanel.RowDefinitions.Add(new RowDefinition());
            parentPanel.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0) });
        }

        private void FullScreenCam4()
        {           
            parentPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0) });
            parentPanel.ColumnDefinitions.Add(new ColumnDefinition());
            
            parentPanel.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0) });
            parentPanel.RowDefinitions.Add(new RowDefinition());
        }
        private void SwitchToNormal()
        {
            NormalParentPanel();
        }

        private void NormalParentPanel()
        {
            parentPanel.ColumnDefinitions.Clear();
            parentPanel.RowDefinitions.Clear();

            parentPanel.ColumnDefinitions.Add(new ColumnDefinition());
            parentPanel.ColumnDefinitions.Add(new ColumnDefinition());

            parentPanel.RowDefinitions.Add(new RowDefinition());
            parentPanel.RowDefinitions.Add(new RowDefinition());  
        }
    }

 
}