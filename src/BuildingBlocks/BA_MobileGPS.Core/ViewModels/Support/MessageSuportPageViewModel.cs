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
    public class MessageSuportPageViewModel : ViewModelBase
    {
        #region Contructor
        public ICommand BackPageCommand { get; private set; }
        public ICommand PushNotificationSupportPageCommand { get; private set; }
        public MessageSuportPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            BackPageCommand = new DelegateCommand(BackPage);
            PushNotificationSupportPageCommand = new DelegateCommand(PushNotificationSupportPage);
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
        #endregion PrivateMethod
    }
}

