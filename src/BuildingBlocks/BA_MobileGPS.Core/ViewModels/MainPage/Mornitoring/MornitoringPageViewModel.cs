using BA_MobileGPS.Core.Events;
using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Core.Views;
using Microsoft.AppCenter;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class MornitoringPageViewModel : ViewModelBase
    {
        public IEventAggregator _eventAggregator;
        public MornitoringPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator) : base(navigationService)
        {
            _eventAggregator = eventAggregator;
            HotlineTapCommand = new DelegateCommand(HotlineTap);
            _bottomGroupMargin = new Thickness(15, 30);
            _eventAggregator.GetEvent<DetailVehiclePopupCloseEvent>().Subscribe(DetailVehiclePopupClose);
   
        }

        private void HotlineTap()
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                PopupNavigation.Instance.PopAllAsync();
            }
            PopupNavigation.Instance.PushAsync(new DetailVehiclePopup());
            BottomGroupMargin = new Thickness(15, 30,15,130);
        }

        public ICommand HotlineTapCommand { get; }
        private Thickness _bottomGroupMargin;
        public Thickness BottomGroupMargin
        {
            get { return _bottomGroupMargin; }
            set { SetProperty(ref _bottomGroupMargin, value);
                RaisePropertyChanged();
            }
        }

        private void DetailVehiclePopupClose()
        {
            BottomGroupMargin = new Thickness(15, 30);
        }

    }
}
