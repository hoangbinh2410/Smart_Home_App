﻿using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity.Support;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service.IService.Support;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
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
        private ISupportCategoryService _iSupportCategoryService;

        public ChangeLicensePlateViewModel(INavigationService navigationService, IPageDialogService pageDialog, IDisplayMessage displayMessage, ISupportCategoryService iSupportCategoryService) : base(navigationService)
        {
            BackPageCommand = new DelegateCommand(BackPage);
            Title = MobileResource.SupportClient_Label_Title;
            PushNotificationSupportPageCommand = new DelegateCommand(PushNotificationSupportPage);
            _pageDialog = pageDialog;
            _displayMessage = displayMessage;
            _iSupportCategoryService = iSupportCategoryService;
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
            if (parameters != null)
            {
                if (parameters.ContainsKey("Support") && parameters.GetValue<SupportCategoryRespone>("Support") is SupportCategoryRespone obj)
                {
                    Item = obj;
                    if (parameters.ContainsKey(ParameterKey.VehicleRoute) && parameters.GetValue<Vehicle>(ParameterKey.VehicleRoute) is Vehicle vehicle && !string.IsNullOrEmpty(vehicle.VehiclePlate))
                    {
                        LicensePlateNow = vehicle.VehiclePlate;
                    }
                    else if(parameters.ContainsKey("ListVehicleSupport") && parameters.GetValue<List<VehicleOnline>>("ListVehicleSupport") is List<VehicleOnline> listvehicle)
                    {
                        LicensePlateNow = listvehicle[0].VehiclePlate;
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

        private SupportCategoryRespone item = new SupportCategoryRespone();
        public SupportCategoryRespone Item { get { return item; } set { SetProperty(ref item, value); } }
        public List<Errorlist> Errorlist;

        public List<Vehiclelist> Vehiclelist;
        public ContactInfo ContactInfo;
        public SupportBapRespone ResponeSupport;
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
                if (!string.IsNullOrEmpty(LicensePlateNew))
                {
                    var ContactInfo = new ContactInfo()
                    {
                        fullname = UserInfo.FullName,
                        username = UserInfo.UserName,
                        mobilestr = UserInfo.PhoneNumber
                    };
                    var Errorlist = new List<Errorlist>()
                            {
                            new Errorlist()
                            { code = Item.Code }
                             };
                    var Vehiclelist = new List<Vehiclelist>()
                            {
                            new Vehiclelist()
                            {
                                platestr = LicensePlateNow , description = LicensePlateNew, errorlist = Errorlist
                            }
                            };

                    var RequestSupport = new SupportBapRequest()
                    {
                        xncode = UserInfo.XNCode,
                        contactinfo = ContactInfo,
                        vehiclelist = Vehiclelist,
                        description = string.Empty
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
                    _displayMessage.ShowMessageInfo(MobileResource.SupportClient_Notification_PleaseEnterNewLicensePlate);
                }
            });
        }

        #endregion PrivateMethod
    }
}