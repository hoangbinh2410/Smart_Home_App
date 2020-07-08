using Plugin.Permissions;
using Syncfusion.SfMaps.XForms;
using System;
using VMS_MobileGPS.Constant;
using VMS_MobileGPS.ViewModels;
using Xamarin.Forms;

namespace VMS_MobileGPS.Views
{
    public partial class OfflineMap : ContentPage
    {
        public OfflineMap()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
            {
                SfMap.EnableZooming = false;
            }
        }

        private void ImageryLayer_ZoomLevelChanging(object sender, ZoomLevelChangingEventArgs e)
        {
            if (e.CurrentLevel > SfMap.MaxZoom || e.CurrentLevel < SfMap.MinZoom)
            {
                e.Cancel = true;
            }
            else
                GlobalResourcesVMS.Current.OffMapZoomLevel = e.CurrentLevel;
        }
    }
}