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

        private bool _isVisibleFeedback;
        public bool IsVisibleFeedback
        {
            get => _isVisibleFeedback;
            set => SetProperty(ref _isVisibleFeedback, value);
        }
        private bool _isVisibleSuccess;
        public bool IsVisibleSuccess
        {
            get => _isVisibleSuccess;
            set => SetProperty(ref _isVisibleSuccess, value);
        }
        #endregion Property

        #region Contructor
    
        public ICommand SendFeedbackCommand { get; private set; }
        public ICommand BackPageCommand { get; private set; }
        public FeedbackErrorsSignalPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = MobileResource.SupportClient_Label_Title;
            SendFeedbackCommand = new DelegateCommand(SendFeedbackClicked);
            BackPageCommand = new DelegateCommand(BackPageClicked);
            IsVisibleFeedback = true;
            IsVisibleSuccess = false;
        }

        #endregion Contructor

        #region PrivateMethod
        private void SendFeedbackClicked()
        {
            IsVisibleFeedback = false;
            IsVisibleSuccess = true;
        }
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
