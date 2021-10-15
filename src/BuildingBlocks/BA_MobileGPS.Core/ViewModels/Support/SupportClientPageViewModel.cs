using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels.Support
{
    public class SupportClientPageViewModel : ViewModelBase
    {
        #region Property

        #endregion Property

        #region Contructor

        public ICommand SupportErrorsSignalCommand { get; private set; }
        public ICommand SupportChangePlateCommand { get; private set; }
        public ICommand SupportErrorsCameraCommand { get; private set; }
        public SupportClientPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            SupportErrorsSignalCommand = new DelegateCommand(SupportErrorsSignalClicked);
            SupportChangePlateCommand = new DelegateCommand(SupportChangePlateClicked);
            SupportErrorsCameraCommand = new DelegateCommand(SupportErrorsCameraClicked);
        }

        #endregion Contructor

        #region PrivateMethod

        private void SupportErrorsSignalClicked()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("SupportErrorsSignalPage");
            });
        }
        private void SupportChangePlateClicked()
        {

        }
        private void SupportErrorsCameraClicked()
        {

        }

        #endregion PrivateMethod
    }
}
