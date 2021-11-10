using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service.IService.Support;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class SelectSupportPageViewModel : ViewModelBase
    {
        #region Contructor

        public ICommand CommandPushSelectSupportPage { get; private set; }
        public ICommand CommandPushMessageSuportPage { get; private set; }
        private ISupportCategoryService _iSupportCategoryService;
        private readonly IDisplayMessage _displayMessage;

        public SelectSupportPageViewModel(INavigationService navigationService, ISupportCategoryService iSupportCategoryService, IDisplayMessage displayMessage) : base(navigationService)
        {
            Title = "Hỗ trợ khách hàng";
            CommandPushSelectSupportPage = new DelegateCommand(PushSelectSupportPage);
            CommandPushMessageSuportPage = new DelegateCommand(PushMessageSuportPage);
            _iSupportCategoryService = iSupportCategoryService;
            _displayMessage = displayMessage;
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
                    data = obj;
                    if (parameters.ContainsKey(ParameterKey.VehicleRoute) && parameters.GetValue<Vehicle>(ParameterKey.VehicleRoute) is Vehicle vehicle && !string.IsNullOrEmpty(vehicle.VehiclePlate))
                    {
                        Vehicle = vehicle;
                    }
                    else if (parameters.ContainsKey("ListVehicleSupport") && parameters.GetValue<List<VehicleOnline>>("ListVehicleSupport") is List<VehicleOnline> listvehicle)
                    {
                        
                        ListVehicle = listvehicle;
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

        private bool IsSupportDisconnectView = true;
        public NavigationParameters item;
        private SupportCategoryRespone data = new SupportCategoryRespone();
        private Vehicle data1 = new Vehicle();
        private Vehicle _vehicle = new Vehicle();
        public Vehicle Vehicle { get => _vehicle; set => SetProperty(ref _vehicle, value); }
        public List<MessageSupportRespone> ListSupportContent { get; set; }
        private MessageSupportRespone query = new MessageSupportRespone();
        private List<VehicleOnline> _listvehicle = new List<VehicleOnline>();
        public List<VehicleOnline> ListVehicle { get => _listvehicle; set => SetProperty(ref _listvehicle, value); }
        private VehicleOnline _vehicleOnline = new VehicleOnline();
        public VehicleOnline vehicleOnline { get => _vehicleOnline; set => SetProperty(ref _vehicleOnline, value); }
        #endregion Property

        #region PrivateMethod

        private void PushSelectSupportPage()
        {         
           
             item = new NavigationParameters
            {
                { "Support", data },
                { ParameterKey.VehicleRoute, Vehicle },
                {"ListVehicleSupport", ListVehicle}

            };
            if (data.IsChangePlate == false)
            {
                SafeExecute(async () =>
                {
                 await NavigationService.NavigateAsync("SupportErrorsSignalPage", item);
                });
                // Kiểm tra page đổi biển
            }
            else /*if (data.IsChangePlate == true)*/
            {
                SafeExecute(async () =>
                {
                    if (!string.IsNullOrEmpty(Vehicle.VehiclePlate))
                    {
                        vehicleOnline = StaticSettings.ListVehilceOnline.FirstOrDefault(x => x.VehicleId == Vehicle.VehicleId);
                    }
                    else
                    {
                        vehicleOnline = ListVehicle[0];
                    }

                    if (vehicleOnline != null)
                    {
                        ListSupportContent = await _iSupportCategoryService.GetMessagesSupport(data.ID);
                        if (ListSupportContent != null && ListSupportContent.Count > 0)
                        {
                            // Kiểm tra xe mất tín hiệu                          
                            if (StateVehicleExtension.IsLostGPS(vehicleOnline.GPSTime, vehicleOnline.VehicleTime) == true || StateVehicleExtension.IsLostGSM(vehicleOnline.VehicleTime) == true)
                            {
                                query = ListSupportContent.Where(s => s.OrderNo == 0).FirstOrDefault();
                                IsSupportDisconnectView = true;
                                item.Add("BoolPage", IsSupportDisconnectView);
                                item.Add("Support1", query);
                                await NavigationService.NavigateAsync("SupportFeePage", item);
                            }
                            else //nếu messageId==1,2,3,128 thì là xe dừng dịch vụ hoac dang no phi chuyen vao trang no phi                         
                            if (StateVehicleExtension.IsVehicleDebtMoney(vehicleOnline.MessageId, vehicleOnline.DataExt) == true || StateVehicleExtension.IsVehicleStopService(vehicleOnline.MessageId) == true)
                            {
                                IsSupportDisconnectView = false;
                                var query = ListSupportContent.Where(s => s.OrderNo == 1).FirstOrDefault();
                                item.Add("BoolPage", IsSupportDisconnectView);
                                item.Add("Support1", query);
                                await NavigationService.NavigateAsync("SupportFeePage", item);
                            }
                            else
                            // Nếu xe còn phí chuyển đến trang đổi biển
                            {
                                await NavigationService.NavigateAsync("ChangeLicensePlate", item);
                            }
                        }
                        else
                        {
                            var parameters = new NavigationParameters
                                   {
                              { "NoData", true },
                                   };
                            await NavigationService.NavigateAsync("SupportFeePage", parameters);
                        }
                    }
                    else
                    {
                        var parameters = new NavigationParameters
                                   {
                              { "NoData", true },
                                   };
                        await NavigationService.NavigateAsync("SupportFeePage", parameters);
                    }
                });             
            }          
        }

        private void PushMessageSuportPage()
        {
            item = new NavigationParameters
            {
                { "Support", data },
                { ParameterKey.VehicleRoute, Vehicle },
                {"ListVehicleSupport", ListVehicle}              
            };
            //Kiểm tra xe mất tín hiệu không đúng hiển thị xe đổi biển
            if (data.IsChangePlate == false)
            {
                SafeExecute(async () =>
                {                  
                    await NavigationService.NavigateAsync("MessageSuportPage", item);
                });
               
            }
            else
            {
                SafeExecute(async () =>
                {
                    await NavigationService.NavigateAsync("MessageSuportPage", item);
                });                
            }
        }


        #endregion PrivateMethod
    }
}