using BA_MobileGPS.Core.Resources;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class FeedbackErrorsSignalPageViewModel : ViewModelBase
    {
        #region Property

        #endregion Property

        #region Contructor

        public ICommand BackPageCommand { get; private set; }
        public FeedbackErrorsSignalPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = MobileResource.SupportClient_Label_Title;
            BackPageCommand = new DelegateCommand(BackPageClicked);
        }

        #endregion Contructor

        #region PrivateMethod
        private void BackPageClicked()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("SupportClientPage");
            });
        }
        #endregion PrivateMethod
    }
}
