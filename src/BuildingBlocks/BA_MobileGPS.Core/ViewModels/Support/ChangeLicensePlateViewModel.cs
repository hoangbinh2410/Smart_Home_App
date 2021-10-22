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
        #region Contructor
        public ICommand BackPageCommand { get; private set; }
        public ICommand PushNotificationSupportPageCommand { get; private set; }
        public ChangeLicensePlateViewModel(INavigationService navigationService) : base(navigationService)
        {
            BackPageCommand = new DelegateCommand(BackPage);
            Title = "Hỗ trợ khách hàng";
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
        #region Property
        private bool InotificationView = false;
        public bool INotificationView
        {
            get { return InotificationView; }
            set
            {
                SetProperty(ref InotificationView, value);
            }
        }           
        #endregion Property
        #region  PrivateMethod

        public void BackPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.GoBackToRootAsync(null);
            });
        }
        public void PushNotificationSupportPage()
        {
            SafeExecute(async () =>
            {
                INotificationView = true;               
            });
        }
        #endregion PrivateMethod
    }
}
