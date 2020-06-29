using BA_MobileGPS.Core.Events;
using BA_MobileGPS.Core.ViewModels.Base;
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
            OpenPopup = false;
        }

        private void HotlineTap()
        {
            OpenPopup = true;
        }

        public ICommand HotlineTapCommand { get; }

        private bool _openPopup;
        public bool OpenPopup
        {
            get { return _openPopup; }
            set { SetProperty(ref _openPopup, value);
                RaisePropertyChanged();
            }
        }
    }
}
