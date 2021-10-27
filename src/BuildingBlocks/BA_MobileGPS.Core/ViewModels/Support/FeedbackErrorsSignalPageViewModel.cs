using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.IService.Support;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class FeedbackErrorsSignalPageViewModel : ViewModelBase
    {
        #region Property
        private readonly IUserService _userService;

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

        private SupportCategoryRespone _objSupport = new SupportCategoryRespone();
        public SupportCategoryRespone ObjSupport
        {
            get => _objSupport;
            set => SetProperty(ref _objSupport, value);
        }

        private Vehicle _vehicle = new Vehicle();
        public Vehicle Vehicle
        {
            get => _vehicle;
            set => SetProperty(ref _vehicle, value);
        }
        private LoginResponse userInfo;

        public LoginResponse UserInfo
        {
            get { if (StaticSettings.User != null) { userInfo = StaticSettings.User; } return userInfo; }
            set => SetProperty(ref userInfo, value);
        }
        private UserInfoRespone _user;

        public UserInfoRespone User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        private string feedback = string.Empty;
        public string Feedack { get { return feedback; } set { SetProperty(ref feedback, value); } }

        #endregion Property

        #region Contructor

        public ICommand SendFeedbackCommand { get; private set; }
        public ICommand BackPageCommand { get; private set; }
        private ISupportCategoryService _iSupportCategoryService;

        public FeedbackErrorsSignalPageViewModel(INavigationService navigationService, IUserService userService, ISupportCategoryService iSupportCategoryService)
            : base(navigationService)
        {
            Title = MobileResource.SupportClient_Label_Title;
            SendFeedbackCommand = new DelegateCommand(SendFeedbackClicked);
            BackPageCommand = new DelegateCommand(BackPageClicked);
            IsVisibleFeedback = true;
            IsVisibleSuccess = false;
            _userService = userService;
            _iSupportCategoryService = iSupportCategoryService;
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
                await NavigationService.GoBackToRootAsync(null);
            });
        }     
        #endregion PrivateMethod

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.ContainsKey("ObjSupport") && parameters.GetValue<SupportCategoryRespone>("ObjSupport") is SupportCategoryRespone objSupport)
                {
                    ObjSupport = objSupport;
                }
                if (parameters.ContainsKey(ParameterKey.VehicleRoute) && parameters.GetValue<Vehicle>(ParameterKey.VehicleRoute) is Vehicle vehicle)
                {
                    Vehicle = vehicle;
                }
            }
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
    }
}
