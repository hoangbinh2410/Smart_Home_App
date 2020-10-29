using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Utilities;
using Prism;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListVehiclePage : ContentPage
    {
        private bool _loadedPage = false;
        private bool _stopTimer = false;
        private ListVehiclePageViewModel vm;

        private readonly IEventAggregator eventAggregator;

        public ListVehiclePage()
        {
            try
            {
                InitializeComponent();

                // Initialize the View Model Object
                vm = (ListVehiclePageViewModel)BindingContext;
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }

            eventAggregator = PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<DestroyEvent>().Subscribe(Destroy);
        }

        private void Destroy()
        {
            eventAggregator.GetEvent<DestroyEvent>().Unsubscribe(Destroy);
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
                Content = new ListVehicleView();

                _loadedPage = true;
            }

            _stopTimer = false;
        }
    }
}