using BA_MobileGPS.Core.Events;
using BA_MobileGPS.Core.Models;
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

        private void TabItemSwitch(Tuple<ItemTabPageEnums, object> obj)
        {
            SelectedIndex = (int)obj.Item1;
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
        public override void OnDestroy()
        {
            base.OnDestroy();
            EventAggregator.GetEvent<TabItemSwitchEvent>().Unsubscribe(TabItemSwitch);
        }
    }
}