using Prism.Mvvm;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RouteReportPage : ContentPage
    {
        private bool _loadedPage = false;
        private bool _stopTimer = false;

        public RouteReportPage()
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
                Content = new RouteReportView();

                _loadedPage = true;
            }

            _stopTimer = false;
        }
    }
}