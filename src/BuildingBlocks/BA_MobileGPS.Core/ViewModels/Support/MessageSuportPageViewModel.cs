using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity.Support;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service.IService.Support;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
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
            _iSupportCategoryService = iSupportCategoryService;
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
                    Item = obj;
                    if (parameters.ContainsKey(ParameterKey.VehicleRoute) && parameters.GetValue<Vehicle>(ParameterKey.VehicleRoute) is Vehicle vehicle && !string.IsNullOrEmpty(vehicle.VehiclePlate))
                    {
                        Vehicle = vehicle;
                        LicensePlateNow = Vehicle.VehiclePlate;
                    }
                    else if (parameters.ContainsKey("ListVehicleSupport") && parameters.GetValue<List<VehicleOnline>>("ListVehicleSupport") is List<VehicleOnline> listvehicle)
                    {
                        ListVehicle = listvehicle;
                        ListVehiclePlace(listvehicle);
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
            if (StaticSettings.User != null)
            {
                Phonenumber = StaticSettings.User.PhoneNumber;
                UserFullName = StaticSettings.User.FullName;
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

        #region

        private List<VehicleOnline> listVehicle = new List<VehicleOnline>();
        public List<VehicleOnline> ListVehicle
        { get { return listVehicle; } set { SetProperty(ref listVehicle, value); } }

        public Vehicle vehicle = new Vehicle();

        public Vehicle Vehicle
        { get { return vehicle; } set { SetProperty(ref vehicle, value); } }

        private SupportCategoryRespone item = new SupportCategoryRespone();

        public SupportCategoryRespone Item
        { get { return item; } set { SetProperty(ref item, value); } }

        public List<Errorlist> Errorlist;
        public List<Vehiclelist> _vehiclelist = new List<Vehiclelist>();
        public List<Vehiclelist> Vehiclelist
        { get { return _vehiclelist; } set { SetProperty(ref _vehiclelist, value); } }
        public ContactInfo ContactInfo;

        public SupportBapRequest RequestSupport = new SupportBapRequest();

        public SupportBapRespone ResponeSupport;
        private string licensePlateNow = string.Empty;

        public string LicensePlateNow
        { get { return licensePlateNow; } set { SetProperty(ref licensePlateNow, value); } }

        private string feedback = string.Empty;

        public string Feedack
        { get { return feedback; } set { SetProperty(ref feedback, value); } }

        private string phonenumber;

        public string Phonenumber
        { get { return phonenumber; } set { SetProperty(ref phonenumber, value); } }

        private string userFullName;

        public string UserFullName
        { get { return userFullName; } set { SetProperty(ref userFullName, value); } }

        private bool InotificationView = false;
        private LoginResponse userInfo;

        public LoginResponse UserInfo
        { get { if (StaticSettings.User != null) { UserInfo = StaticSettings.User; } return userInfo; } set => SetProperty(ref userInfo, value); }

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

        public void ListVehiclePlace(List<VehicleOnline> listVehicle)
        {
            foreach (var item in listVehicle)
            {
                if (string.IsNullOrEmpty(LicensePlateNow))
                {
                    LicensePlateNow = item.VehiclePlate;
                }
                else
                {
                    LicensePlateNow = LicensePlateNow + $", {item.VehiclePlate}";
                }
            }
        }

        public void PushNotificationSupportPage()
        {
            SafeExecute(async () =>
            {
                if (!string.IsNullOrEmpty(UserFullName))
                {
                    if (!string.IsNullOrEmpty(Phonenumber))
                    {
                        if (!string.IsNullOrEmpty(Feedack))
                        {
                            if (!string.IsNullOrEmpty(Vehicle.VehiclePlate))
                            {
                                ContactInfo = new ContactInfo()
                                {
                                    fullname = UserFullName,
                                    username = UserInfo.UserName,
                                    mobilestr = Phonenumber
                                };
                                Errorlist = new List<Errorlist>()
                            {
                                new Errorlist() { code = Item.Code }
                            };
                                Vehiclelist = new List<Vehiclelist>()
                            {
                                new Vehiclelist() {
                                    platestr = Vehicle.VehiclePlate,
                                    description = Feedack,
                                    errorlist = Errorlist }
                            };

                                RequestSupport = new SupportBapRequest()
                                {
                                    xncode = UserInfo.XNCode,
                                    contactinfo = ContactInfo,
                                    vehiclelist = Vehiclelist,
                                    description = Feedack
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
                                ContactInfo = new ContactInfo()
                                {
                                    fullname = UserFullName,
                                    username = UserInfo.UserName,
                                    mobilestr = Phonenumber
                                };
                                Errorlist = new List<Errorlist>()
                                    {
                                      new Errorlist() { code = Item.Code}
                                    };
                                foreach (var item in ListVehicle)
                                {
                                    Vehiclelist.Add(new Vehiclelist() { platestr = item.VehiclePlate, description = Feedack, errorlist = Errorlist });                                 
                                }

                                RequestSupport = new SupportBapRequest()
                                {
                                    xncode = UserInfo.XNCode,
                                    contactinfo = ContactInfo,
                                    vehiclelist = Vehiclelist,
                                    description = Feedack
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
                        }
                        else
                        {
                            _displayMessage.ShowMessageWarning("Vui lòng nhập phản hồi");
                        }
                    }
                    else
                    {
                        _displayMessage.ShowMessageWarning("Vui lòng nhập số điện thoại");
                    }
                }
                else
                {
                    _displayMessage.ShowMessageWarning("Vui lòng nhập họ tên");
                }
            });
        }

        #endregion PrivateMethod
    }
}