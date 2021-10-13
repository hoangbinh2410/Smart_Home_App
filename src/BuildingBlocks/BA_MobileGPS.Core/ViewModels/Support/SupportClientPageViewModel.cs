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
    public class SupportClientPageViewModel : ViewModelBase
    {
        public ICommand PushSupportFeePageCommand { get; private set; }
        public ICommand PushSupportDisconnectPageCommand { get; private set; }
        public SupportClientPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PushSupportFeePageCommand = new DelegateCommand(PushSupportFeePage);

            PushSupportDisconnectPageCommand = new DelegateCommand(PushSupportDisconnectPage);
        }
        public void PushSupportFeePage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("SupportFeePage");
            });
        }
        public void PushSupportDisconnectPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("SupportDisconnectPage");
            });
        }
    }
}
