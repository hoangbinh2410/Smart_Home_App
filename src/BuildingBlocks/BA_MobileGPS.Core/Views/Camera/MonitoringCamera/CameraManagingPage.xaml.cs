﻿using BA_MobileGPS.Core.Resources;
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
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using System.Collections.Generic;

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
            eventAggregator.GetEvent<DisposeTemplateView>().Publish();
            Device.BeginInvokeOnMainThread(() =>
            {              
                noDataImage.IsVisible = false;
                cameraPanel.Children.Clear();
                if (obj == 1)
                {
                    var cam = new Template1Camera();
                  
                    cameraPanel.Children.Add(cam);
                }
                else if (obj == 2)
                {
                    var cam = new Template2Camera();
                  
                    cameraPanel.Children.Add(cam);
                }
                else if(obj == 4)
                {
                    var cam = new Template4Camera();
                 
                    cameraPanel.Children.Add(cam);
                }
                else if (obj == 0)
                {
                    noDataImage.IsVisible = true;
                }
            });
        
        }

        private void SwitchToNormal()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Grid.SetRow(playbackControl, 3);
                if (Device.RuntimePlatform == Device.iOS)
                {
                    var safe = On<iOS>().SafeAreaInsets();
                    Padding = new Thickness(0, 0, 0, safe.Bottom);
                }
            });
          
        }

        private void SwitchToFullScreen(CameraEnum obj)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Grid.SetRow(playbackControl, 2);
                Padding = new Thickness(0, 0, 0, 0);
            });        
        }

        protected override void OnAppearing()
        {
            entrySearch.Placeholder = MobileResource.Route_Label_SearchFishing;      
            base.OnAppearing();
            if (Device.RuntimePlatform == Device.iOS)
            {
                var safe = On<iOS>().SafeAreaInsets();
                Padding = new Thickness(0, 0, 0, safe.Bottom);
            }
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

    public class SwitchToFullScreenEvent : PubSubEvent<CameraEnum>
    {

    }

    public class SwitchToNormalScreenEvent : PubSubEvent
    {

    }
    public class DisposeTemplateView : PubSubEvent
    {

    }
}