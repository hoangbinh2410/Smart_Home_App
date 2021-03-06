using System;
using VMS_MobileGPS.Views.ListVehicle;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VMS_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListVehiclePage : ContentPage
    {
        private bool _loadedPage = false;
        private bool _stopTimer = false;

        public ListVehiclePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (_loadedPage) return;

            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                SetContent();
                return false;
            });
        }

        protected override void OnDisappearing()
        {
            _stopTimer = true;
        }

        private void SetContent()
        {
            if (!_stopTimer)
            {

                var view = new ListVehicleView();
                Content = view;

                _loadedPage = true;
            }

            _stopTimer = false;
        }
    }
}