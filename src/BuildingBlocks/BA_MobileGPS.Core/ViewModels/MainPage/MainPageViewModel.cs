using BA_MobileGPS.Core.Events;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BA_MobileGPS.Core.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {            
            EventAggregator.GetEvent<TabItemSwitchEvent>().Subscribe(TabItemSwitch);
        }

        private void TabItemSwitch(int obj)
        {
            SelectedIndex = obj;
        }

        private int selectedIndex;
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                SetProperty(ref selectedIndex, value);
                RaisePropertyChanged();
            }
        }
    }
}
