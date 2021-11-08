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

        private bool ISupportDisconnectView = true;
        public NavigationParameters item;
        private SupportCategoryRespone data = new SupportCategoryRespone();
        private Vehicle data1 = new Vehicle();
        private Vehicle _vehicle = new Vehicle();
        public Vehicle Vehicle { get => _vehicle; set => SetProperty(ref _vehicle, value); }
        public List<MessageSupportRespone> ListSupportContent { get; set; }
        private MessageSupportRespone query = new MessageSupportRespone();

        #endregion Property

        #region PrivateMethod

        private void PushSelectSupportPage()
        {
            var qry = StaticSettings.ListVehilceOnline.FirstOrDefault(x => x.VehicleId == Vehicle.VehicleId);
             item = new NavigationParameters
            {
                { "Support", data },
                { ParameterKey.VehicleRoute, Vehicle },
                
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
                    if (qry != null)
                    {
                        ListSupportContent = await _iSupportCategoryService.GetMessagesSupport(data.ID);
                        if (ListSupportContent != null && ListSupportContent.Count > 0)
                        {
                            // Kiểm tra xe mất tín hiệu                          
                            if (StateVehicleExtension.IsLostGPS(qry.GPSTime, qry.VehicleTime) == true || StateVehicleExtension.IsLostGSM(qry.VehicleTime) == true)
                            {
                                query = ListSupportContent.Where(s => s.OrderNo == 0).FirstOrDefault();
                                ISupportDisconnectView = true;
                                item.Add("BoolPage", ISupportDisconnectView);
                                item.Add("Support1", query);
                                await NavigationService.NavigateAsync("SupportFeePage", item);
                            }
                            else //nếu messageId==1,2,3,128 thì là xe dừng dịch vụ hoac dang no phi chuyen vao trang no phi                         
                            if (StateVehicleExtension.IsVehicleDebtMoney(qry.MessageId, qry.DataExt) == true || StateVehicleExtension.IsVehicleStopService(qry.MessageId) == true)
                            {
                                ISupportDisconnectView = false;
                                var query = ListSupportContent.Where(s => s.OrderNo == 1).FirstOrDefault();
                                item.Add("BoolPage", ISupportDisconnectView);
                                item.Add("Support1", query);
                                await NavigationService.NavigateAsync("SupportFeePage", item);
                            }
                            else
                            // Nếu xe còn phí chuyển đến trang đổi biển
                            {
                                //var parameter = new NavigationParameters { { ParameterKey.VehicleRoute, Vehicle.VehiclePlate } };
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
            };
            if (data.IsChangePlate == false)
            {
                SafeExecute(async () =>
                {
                    await NavigationService.NavigateAsync("MessageSuportPage", item);
                });
                // Kiểm tra page đổi biển
            }
            else /*if (data.Code == nameof(SupportCode.PLATE))*/
            {
                SafeExecute(async () =>
                {
                    await NavigationService.NavigateAsync("MessageSuportPage", item);
                });
                // Kiểm tra Page Camera
            }
        }

        #endregion PrivateMethod
    }
}