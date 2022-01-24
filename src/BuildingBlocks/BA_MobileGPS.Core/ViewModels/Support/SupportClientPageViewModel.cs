using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service.IService.Support;
using BA_MobileGPS.Utilities;
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
                if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehicle)
                {
                    Vehicle = vehicle;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
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

        // public ICommand SelectVehicleAllCommand { get; }
        public ICommand HideSelect { get; }

        public ICommand SupportSelect { get; }
        public ICommand PushSupportMesage { get; }

        private ISupportCategoryService _iSupportCategoryService;

        public SupportClientPageViewModel(INavigationService navigationService, ISupportCategoryService iSupportCategoryService, IDisplayMessage displayMessage)
            : base(navigationService)
        {
            NavigateCommand = new DelegateCommand<ItemTappedEventArgs>(NavigateClicked);
            _iSupportCategoryService = iSupportCategoryService;
            _displayMessage = displayMessage;
            Title = MobileResource.SupportClient_Label_Title;
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
                        int languageId = Settings.CurrentLanguage == CultureCountry.Vietnamese ? 1 : 2;
                        MenuItems = await _iSupportCategoryService.GetListSupportCategory(languageId);
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
            else
            {
                data = (SupportCategoryRespone)item.ItemData;
                var parameters = new NavigationParameters
                {
                    { "Support", data }
                };
                SafeExecute(async () =>
                {
                    if (string.IsNullOrEmpty(Vehicle.VehiclePlate))
                    {
                        await NavigationService.NavigateAsync("ListVehicleSupportPage", parameters);
                    }
                    else
                    {
                        parameters.Add(ParameterKey.VehicleRoute, Vehicle);
                        await NavigationService.NavigateAsync("SelectSupportPage", parameters);
                    }
                });
            }
        }

        #endregion PrivateMethod
    }
}