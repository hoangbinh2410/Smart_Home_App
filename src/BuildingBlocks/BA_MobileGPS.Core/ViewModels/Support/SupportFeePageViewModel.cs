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
        #region Contructor
        public ICommand BackPageCommand { get; private set; }
        public ICommand PushChangeLicensePlateCommand { get; private set; }
        public ICommand PushMessageSuportPageCommand { get; private set; }

        public SupportFeePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            BackPageCommand = new DelegateCommand(BackPage);
            PushChangeLicensePlateCommand = new DelegateCommand(PushChangeLicensePlate);
            PushMessageSuportPageCommand = new DelegateCommand(PushMessageSuportPage);
        }
        #endregion Contructor
        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnDestroy()
        {
        }

        #endregion Lifecycle
        //312312312312312
        #region Property

        #endregion Property
        #region  PrivateMethod
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
        #endregion PrivateMethod
    }
}
