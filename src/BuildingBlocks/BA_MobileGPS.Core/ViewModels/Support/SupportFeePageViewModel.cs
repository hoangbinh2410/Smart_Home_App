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
    public class SupportFeePageViewModel : ViewModelBase
    {
        public ICommand BackPageCommand { get; private set; }
        public ICommand PushChangeLicensePlateCommand { get; private set; }
        public ICommand PushMessageSuportPageCommand { get; private set; }

        public SupportFeePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            BackPageCommand = new DelegateCommand(BackPage);
            PushChangeLicensePlateCommand = new DelegateCommand(PushChangeLicensePlate);
            PushMessageSuportPageCommand = new DelegateCommand(PushMessageSuportPage);
        }
        public void BackPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.GoBackAsync(null, true, false);
            });
        }
        public void PushChangeLicensePlate()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("ChangeLicensePlate", null, true, false);
            });
        }
        public void PushMessageSuportPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("MessageSuportPage", null, true, false);
            });
        }
    }
}
