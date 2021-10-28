using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service.IService.Support;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.ListView.XForms;
using System.Collections.Generic;
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
                    VehiclePlate = vehicle.VehiclePlate;
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
            PushSupportMesage = new DelegateCommand(PushFeedbackErrorsPage);
            SupportSelect = new DelegateCommand(PushSupportErrorsPage);
            _displayMessage = displayMessage;
        }

        #endregion Contructor

        #region Property

        public string vehiclePlate = string.Empty;

        public string VehiclePlate
        {
            get { return vehiclePlate; }
            set { SetProperty(ref vehiclePlate, value); }
        }

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
            if (VehiclePlate == "")
            {
                _displayMessage.ShowMessageInfo("Vui lòng chọn phương tiện xử lý");
            }
            else
            {
                data = (SupportCategoryRespone)item.ItemData;
                var parameters = new NavigationParameters
                {
                    { "Support", data },
                    { ParameterKey.VehicleRoute, Vehicle }
                };
                // Kiểm tra  page mất tín hiệu , // Kiểm tra Page Camera
                if (data.IsChangePlate == false)
                {
                    SafeExecute(async () =>
                    {
                        await NavigationService.NavigateAsync("SelectSupportPage", parameters);
                    });  
                }
                // Kiểm tra page đổi biển
                else
                {
                    SafeExecute(async () =>
                    {
                        await NavigationService.NavigateAsync("SelectSupportPage", parameters);
                    });
                }
                //else if (data.Code == nameof(SupportCode.CMR))
                //{
                //    SafeExecute(async () =>
                //    {
                //    //ISelect = true;
                //    await NavigationService.NavigateAsync("SelectSupportPage", parameters);
                //    });
                //}
                //// không đúng thì hiển thị thông báo
                //else
                //{
                //    _displayMessage.ShowMessageInfo(MobileResource.Common_Message_SelectCompany);
                //}
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