using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
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
            LicensePlateNow = parameters.GetValue<string>(ParameterKey.VehicleRoute);
            NamePage = parameters.GetValue<string>("Support");
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

        private string licensePlateNow = string.Empty;
        public string LicensePlateNow { get { return licensePlateNow; } set { SetProperty(ref licensePlateNow, value); } }
        private string namepage = String.Empty;
        public string NamePage { get { return namepage; } set { SetProperty(ref namepage, value); } }
        private string feedback = string.Empty;
        public string Feedack { get { return feedback; } set { SetProperty(ref feedback, value); } }
        private string phonenumber;
        public string Phonenumber { get { return phonenumber; } set { SetProperty(ref phonenumber, value); } }
        private bool InotificationView = false;
        private LoginResponse userInfo;
        public LoginResponse UserInfo {get { if (StaticSettings.User != null) { userInfo = StaticSettings.User; } return userInfo; }set => SetProperty(ref userInfo, value);}

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
                if (Feedack != "" && Phonenumber != null && Phonenumber.Length <= 10)
                {
                    INotificationView = true;
                }
                else
                {                  
                    _displayMessage.ShowMessageInfo(MobileResource.Common_Message_SelectCompany);
                }
            });
        }

        #endregion PrivateMethod
    }
}