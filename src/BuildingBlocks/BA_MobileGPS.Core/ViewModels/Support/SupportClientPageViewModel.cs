using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Enums;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service.IService.Support;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.ListView.XForms;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class SupportClientPageViewModel : ViewModelBase
    {
        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetListSupportCategory();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
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
            ISelect = false;
        }

        public override void OnDestroy()
        {
        }

        #endregion Lifecycle

        #region Contructor

        private readonly IDisplayMessage _displayMessage;
        public ICommand NavigateCommand { get; }
        public ICommand SelectVehicleAllCommand { get; }
        public ICommand HideSelect { get; }
        public ICommand SupportSelect { get; }
        public ICommand PushSupportMesage { get; }

        private ISupportCategoryService _iSupportCategoryService;

        public SupportClientPageViewModel(INavigationService navigationService, ISupportCategoryService iSupportCategoryService, IDisplayMessage displayMessage)
            : base(navigationService)
        {
            NavigateCommand = new DelegateCommand<ItemTappedEventArgs>(NavigateClicked);
            SelectVehicleAllCommand = new DelegateCommand(SelectVehicleAll);
            _iSupportCategoryService = iSupportCategoryService;
            HideSelect = new DelegateCommand(ClickHideSelect);
            PushSupportMesage = new DelegateCommand(PushFeedbackErrorsPage);
            SupportSelect = new DelegateCommand(PushSupportErrorsPage);
            _displayMessage = displayMessage;
        }

        #endregion Contructor

        #region Property

        private List<SupportCategoryRespone> menuItems = new List<SupportCategoryRespone>();

        public List<SupportCategoryRespone> MenuItems
        {
            get { return menuItems; }
            set { SetProperty(ref menuItems, value); }
        }

        private Vehicle _vehicle = new Vehicle();

        public Vehicle Vehicle
        {
            get => _vehicle;
            set => SetProperty(ref _vehicle, value);
        }

        private bool iSelect = false;

        public bool ISelect
        {
            get => iSelect;
            set => SetProperty(ref iSelect, value);
        }

        private bool ISupportDisconnectView = true;
        private SupportCategoryRespone data = new SupportCategoryRespone();
        private string DataPlate { get; set; }

        public List<MessageSupportRespone> ListSupportContent { get; set; }

        private MessageSupportRespone _SupportContent = new MessageSupportRespone();

        public MessageSupportRespone SupportContent
        {
            get { return _SupportContent; }
            set { SetProperty(ref _SupportContent, value); }
        }

        private MessageSupportRespone query = new MessageSupportRespone();

        #endregion Property

        #region PrivateMethod

        private void GetListSupportCategory()
        {
            SafeExecute(async () =>
            {
                if (IsConnected)
                {
                    using (new HUDService(MobileResource.Common_Message_Processing))
                    {
                        MenuItems = await _iSupportCategoryService.GetListSupportCategory();
                    }
                }
                else
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
                }
            });
        }

        public void NavigateClicked(ItemTappedEventArgs item)
        {
            if (item == null || item.ItemData == null)
            {
                return;
            }
            data = (SupportCategoryRespone)item.ItemData;
            var parameters = new NavigationParameters
            {
                { "Support", data },
                { ParameterKey.VehicleRoute, Vehicle },
                {"BoolPage",ISupportDisconnectView }
            };
            var qry = StaticSettings.ListVehilceOnline.FirstOrDefault(x => x.VehicleId == Vehicle.VehicleId);
          
            // Kiểm tra  page mất tín hiệu
             if ( data.Code == "MTH")
            {
                SafeExecute(async () =>
                {
                    ISelect = true;
                });
                // Kiểm tra page đổi biển
            } else if(data.Code == "PLATE")
            {
                SafeExecute(async () =>
                {
                    if (qry != null)
                    {
                        ListSupportContent = await _iSupportCategoryService.GetMessagesSupport(data.ID);
                        if (ListSupportContent != null && ListSupportContent.Count > 0)
                        {
                            // Kiểm tra xe mất tín hiệu
                            //if (StateVehicleExtension.IsVehicleDebtMoney(qry.MessageId, qry.DataExt) == true || StateVehicleExtension.IsVehicleStopService(qry.MessageId) == true)
                            //{
                            //    ISupportDisconnectView = false;
                            //    var query = ListSupportContent.Where(s => s.OrderNo == 1).FirstOrDefault();
                            //    parameters.Add("Support1", query);
                            //    await NavigationService.NavigateAsync("SupportFeePage", parameters);
                            //}
                            if (StateVehicleExtension.IsLostGPS(qry.GPSTime, qry.VehicleTime) == true || StateVehicleExtension.IsLostGSM(qry.VehicleTime) == true)
                            {
                                query = ListSupportContent.Where(s => s.OrderNo == 0).FirstOrDefault();
                                ISupportDisconnectView = true;
                                parameters.Add("Support1", query);
                                await NavigationService.NavigateAsync("SupportFeePage", parameters);
                            }
                            else //nếu messageId==1,2,3,128 thì là xe dừng dịch vụ hoac dang no phi chuyen vao trang no phi
                                 // if (StateVehicleExtension.IsLostGPS(qry.GPSTime, qry.VehicleTime) == true || StateVehicleExtension.IsLostGSM(qry.VehicleTime) == true)
                                 //{
                                 //    query = ListSupportContent.Where(s => s.OrderNo == 0).FirstOrDefault();
                                 //    ISupportDisconnectView = true;
                                 //    parameters.Add("Support1", query);
                                 //    await NavigationService.NavigateAsync("SupportFeePage", parameters);
                                 //}
                            if (StateVehicleExtension.IsVehicleDebtMoney(qry.MessageId, qry.DataExt) == true || StateVehicleExtension.IsVehicleStopService(qry.MessageId) == true)
                            {
                                ISupportDisconnectView = false;
                                var query = ListSupportContent.Where(s => s.OrderNo == 1).FirstOrDefault();
                                parameters.Add("Support1", query);
                                await NavigationService.NavigateAsync("SupportFeePage", parameters);
                            }
                            else
                            // Nếu xe còn phí chuyển đến trang đổi biển
                            {
                                var parameter = new NavigationParameters { { ParameterKey.VehicleRoute, Vehicle.VehiclePlate } };
                                await NavigationService.NavigateAsync("ChangeLicensePlate", parameter);
                            }
                        }
                        else
                        {
                            var parameter = new NavigationParameters
                                   {
                              { "NoData", true },
                                   };
                            await NavigationService.NavigateAsync("SupportFeePage", parameter);
                        }
                    }
                    else
                    {
                        var parameter = new NavigationParameters
                                   {
                              { "NoData", true },
                                   };
                        await NavigationService.NavigateAsync("SupportFeePage", parameter);
                    }
                });
                // Kiểm tra Page Camera
            } else if(data.Code == "CMR")
            {
                SafeExecute(async () =>
                {
                    ISelect = true;
                });
            }
             // không đúng thì hiển thị thông báo
            else
            {
                _displayMessage.ShowMessageInfo(MobileResource.Common_Message_SelectCompany);
            }             
        }
        private void SelectVehicleAll()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleLookUp", animated: true, useModalNavigation: true, parameters: new NavigationParameters
                        {
                            { ParameterKey.VehicleLookUpType, VehicleLookUpType.VehicleList },
                            {  ParameterKey.VehicleGroupsSelected, VehicleGroups},
                            {  ParameterKey.VehicleStatusSelected, ListVehicleStatus}
                        });
            });
        }

        private void ClickHideSelect()
        {
            ISelect = false;
        }

        private async void PushFeedbackErrorsPage()
        {
            var parameters = new NavigationParameters
            {
                { "Support", data },
                { ParameterKey.VehicleRoute, Vehicle },
            };
            await NavigationService.NavigateAsync("FeedbackErrorsSignalPage", parameters);
        }

        private async void PushSupportErrorsPage()
        {
            var parameters = new NavigationParameters
            {
                { "Support", data },
                { ParameterKey.VehicleRoute, Vehicle },
            };
            await NavigationService.NavigateAsync("SupportErrorsSignalPage", parameters);
        }

        #endregion PrivateMethod
    }
}