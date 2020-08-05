using BA_MobileGPS.Core.Controls;
using BA_MobileGPS.Core.ViewModels;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraDetail : ContentPage
    {
        public CameraDetail()
        {

            InitializeComponent();

            Map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(Settings.Latitude, Settings.Longitude), 6d);

        }


    }
}