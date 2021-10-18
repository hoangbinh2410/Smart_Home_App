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
        #region Contructor
        public ICommand PushSupportFeePageCommand { get; private set; }
        public ICommand PushSupportDisconnectPageCommand { get; private set; }
        public SupportClientPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PushSupportFeePageCommand = new DelegateCommand(PushSupportFeePage);

            PushSupportDisconnectPageCommand = new DelegateCommand(PushSupportDisconnectPage);
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

        public void PushSupportFeePage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("NavigationPage/SupportFeePage");
            });
        }
        public void PushSupportDisconnectPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("NavigationPage/SupportDisconnectPage");
            });
        }
        #endregion PrivateMethod
    }
}
