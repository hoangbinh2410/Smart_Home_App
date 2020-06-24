using BA_MobileGPS.Core.Events;
using BA_MobileGPS.Core.ViewModels.Base;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BA_MobileGPS.Core.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        public MainPageViewModel(INavigationService navigationService,IEventAggregator eventAggregator) : base(navigationService)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<TabItemSwitchEvent>().Subscribe(TabItemSwitch);
        }

        private void TabItemSwitch(int obj)
        {

        }
    }
}
