using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ChangeLicensePlateViewModel : ViewModelBase
    {
        #region Contructor

        private readonly IPageDialogService _pageDialog;
        private readonly IDisplayMessage _displayMessage;
        public ICommand BackPageCommand { get; private set; }
        public ICommand PushNotificationSupportPageCommand { get; private set; }

        public ChangeLicensePlateViewModel(INavigationService navigationService, IPageDialogService pageDialog, IDisplayMessage displayMessage) : base(navigationService)
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

        private LoginResponse userInfo;

        public LoginResponse UserInfo
        {
            get { if (StaticSettings.User != null) { userInfo = StaticSettings.User; } return userInfo; }
            set => SetProperty(ref userInfo, value);
        }
        private string licensePlateNow = string.Empty;
        public string LicensePlateNow { get { return licensePlateNow; } set { SetProperty(ref licensePlateNow, value); } }
        private string licensePlatenew = string.Empty;
        public string LicensePlateNew { get { return licensePlatenew; } set { SetProperty(ref licensePlatenew, value); } }
        private bool InotificationView = false;
        public bool INotificationView { get { return InotificationView; } set { SetProperty(ref InotificationView, value); } }

        #endregion Property

        #region PrivateMethod

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
                if (LicensePlateNew != "")
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