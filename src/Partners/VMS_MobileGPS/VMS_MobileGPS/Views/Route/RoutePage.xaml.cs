using System;
using VMS_MobileGPS.Views.Route;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VMS_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoutePage : ContentPage
    {
        private bool _loadedPage = false;
        private bool _stopTimer = false;

        public RoutePage()
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
                Content = new RouteView();

                _loadedPage = true;
            }

            _stopTimer = false;
        }
    }
}