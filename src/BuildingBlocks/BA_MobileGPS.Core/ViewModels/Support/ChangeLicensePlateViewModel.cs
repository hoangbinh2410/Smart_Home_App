using BA_MobileGPS.Core.Resources;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ChangeLicensePlateViewModel : ViewModelBase
    {
        public ICommand BackPageCommand { get; private set; }
        public ICommand PushNotificationSupportPageCommand { get; private set; }
        public ChangeLicensePlateViewModel(INavigationService navigationService) : base(navigationService)
        {
            BackPageCommand = new DelegateCommand(BackPage);
            PushNotificationSupportPageCommand = new DelegateCommand(PushNotificationSupportPage);
        }
        public void BackPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.GoBackAsync(null, true, false); ;
            });
        }
        public void PushNotificationSupportPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("NotificationSupportPage", null, true, false);
            });
        }
    }
}
