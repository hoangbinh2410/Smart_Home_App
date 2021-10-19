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
    public class SupportDisconnectPageViewModel : ViewModelBase
    {
        #region Contructor
        public ICommand BackPageCommand { get; private set; }
        public SupportDisconnectPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            BackPageCommand = new DelegateCommand(BackPage);
            Title = "Hỗ trợ khách hàng";
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

        #endregion Property
        #region  PrivateMethod
        public void BackPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.GoBackAsync(); ;
            });
        }
        #endregion PrivateMethod
    }
}
