using BA_MobileGPS.Core.Resources;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class MessageSuportPageViewModel : ViewModelBase
    {
        #region Contructor
        private readonly IPageDialogService _pageDialog;
        private readonly IDisplayMessage _displayMessage;
        public ICommand BackPageCommand { get; private set; }
        public ICommand PushNotificationSupportPageCommand { get; private set; }
        public MessageSuportPageViewModel(INavigationService navigationService, IPageDialogService pageDialog, IDisplayMessage displayMessage) : base(navigationService)
        {
            BackPageCommand = new DelegateCommand(BackPage);
            Title = "Hỗ trợ khách hàng";
            PushNotificationSupportPageCommand = new DelegateCommand(PushNotificationSupportPage);
            _pageDialog = pageDialog;
            _displayMessage = displayMessage;
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
        #region 

        
        private string feedback = string.Empty;
        public string Feedack { get { return feedback; } set { SetProperty(ref feedback, value); } }
        private string phonenumber;
        public string Phonenumber { get { return phonenumber; } set { SetProperty(ref phonenumber, value); } }
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
                if(Feedack != null && Phonenumber != null && Phonenumber.Length==10 )
                {
                    INotificationView = true;
                }
                else
                {
                    //_pageDialog.DisplayAlertAsync(MobileResource.Common_Message_Warning, MobileResource.Online_Message_CarDebtMoney, MobileResource.Common_Label_Close);
                    _displayMessage.ShowMessageInfo(MobileResource.Common_Message_SelectCompany);
                }

            });
        }
        #endregion PrivateMethod
    }
}

