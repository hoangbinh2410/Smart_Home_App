using BA_MobileGPS.Core.ViewModels.Base;
using Prism;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoutePage : ContentPage
    {
        private readonly IEventAggregator eventAggregator;
        private bool _loadedPage = false;
        private bool _stopTimer = false;

        public RoutePage()
        {
            InitializeComponent();

            eventAggregator = PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();

            eventAggregator.GetEvent<DestroyEvent>().Subscribe(Destroy);
        }

        protected override void OnAppearing()
        {
            if (_loadedPage) return;

            Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
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
                ViewModelLocator.SetAutowireViewModel(this, true);
                Content = new RouteView();

                _loadedPage = true;
            }

            _stopTimer = false;
        }

        private void Destroy()
        {
            eventAggregator.GetEvent<DestroyEvent>().Unsubscribe(Destroy);
        }
    }
}