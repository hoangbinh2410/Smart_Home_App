using BA_MobileGPS.Core.Events;
using Prism.Navigation;
using System;

namespace BA_MobileGPS.Core.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            EventAggregator.GetEvent<TabItemSwitchEvent>().Subscribe(TabItemSwitch);
        }

        private void TabItemSwitch(Tuple<int, object> obj)
        {
            SelectedIndex = obj.Item1;
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