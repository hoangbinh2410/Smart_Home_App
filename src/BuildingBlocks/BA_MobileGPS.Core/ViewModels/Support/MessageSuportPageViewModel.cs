using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity.Support;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service.IService.Support;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
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
        private ISupportCategoryService _iSupportCategoryService;
        public MessageSuportPageViewModel(INavigationService navigationService, IPageDialogService pageDialog, IDisplayMessage displayMessage, ISupportCategoryService iSupportCategoryService) : base(navigationService)
        {
            BackPageCommand = new DelegateCommand(BackPage);
            Title = "Hỗ trợ khách hàng";
            PushNotificationSupportPageCommand = new DelegateCommand(PushNotificationSupportPage);
            _pageDialog = pageDialog;
            _displayMessage = displayMessage;
            _iSupportCategoryService = iSupportCategoryService ;
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            if (parameters != null)
            {
                if (parameters.ContainsKey("Support") && parameters.GetValue<SupportCategoryRespone>("Support") is SupportCategoryRespone obj)
                {
                    item = obj;
                    if (parameters.ContainsKey(ParameterKey.VehicleRoute) && parameters.GetValue<Vehicle>(ParameterKey.VehicleRoute) is Vehicle vehicle)
                    {
                        Vehicle = vehicle;
                    }
                    else
                    {
                        _displayMessage.ShowMessageInfo(MobileResource.Common_Message_SelectCompany);
                    }
                }
                else
                {
                    _displayMessage.ShowMessageInfo(MobileResource.Common_Message_SelectCompany);
                }
            }
            else
            {
                _displayMessage.ShowMessageInfo(MobileResource.Common_Message_SelectCompany);
            }           
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);       
            LicensePlateNow = Vehicle.VehiclePlate;
            NamePage = item.Name;
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
        public Vehicle Vehicle ;
        public SupportCategoryRespone item ;

        public List<Errorlist> Errorlist;

        public List<Vehiclelist> Vehiclelist;
        public ContactInfo ContactInfo;

        public SupportBapRequest RequestSupport = new SupportBapRequest();

        public SupportBapRespone ResponeSupport;
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
                if (Feedack != "")
                {
                    ContactInfo = new ContactInfo()
                    {
                        Fullname = UserInfo.FullName,
                        Username = UserInfo.UserName,
                        Mobilestr = UserInfo.PhoneNumber
                    };
                    Errorlist = new List<Errorlist>()
                    {
                        new Errorlist(){ Code = item.Code }
                    };
                    Vehiclelist = new List<Vehiclelist>()
                    {
                        new Vehiclelist(){ Platestr = Vehicle.VehiclePlate,Description = Feedack, Errorlist = Errorlist }
                    };
                    
                    RequestSupport = new SupportBapRequest() {                       
                        Xncode = UserInfo.XNCode,
                        ContactInfo = ContactInfo,
                        Vehiclelist = Vehiclelist,
                        Description = Feedack
                    };
                  
                    ResponeSupport = await _iSupportCategoryService.Getfeedback(RequestSupport);
                    if (ResponeSupport.State == true && ResponeSupport != null)
                    {
                        INotificationView = true;
                    }
                    else
                    {
                        _displayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error);
                    }
                }
                else
                {                  
                    _displayMessage.ShowMessageWarning("Nhập đầy đủ thông tin");
                }
            });
        }

        #endregion PrivateMethod
    }
}