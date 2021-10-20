using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Enums;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service.IService.Support;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Windows.Input;


namespace BA_MobileGPS.Core.ViewModels
{
    public class SupportClientPageViewModel : ViewModelBase
    {
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
        #endregion Property

        #region Contructor

        public ICommand NavigateCommand { get; }
        public ICommand ComboboxCommand { get; }
        private ISupportCategoryService _iSupportCategoryService;

        public SupportClientPageViewModel(INavigationService navigationService, ISupportCategoryService iSupportCategoryService)
            : base(navigationService)
        {
            NavigateCommand = new DelegateCommand<ItemTappedEventArgs>(NavigateClicked);
            ComboboxCommand = new DelegateCommand(ComboboxClicked);
            _iSupportCategoryService = iSupportCategoryService;
        }
        #endregion Contructor

        #region PrivateMethod

        private void GetListSupportCategory()
        {
            SafeExecute(async () =>
            {
                MenuItems = await _iSupportCategoryService.GetListSupportCategory();
            });
        }
        private void ComboboxClicked()
        {
            
        }

        private void NavigateClicked(ItemTappedEventArgs item)
        {
            if(item == null || item.ItemData == null)
            {
                return;
            }    
            SupportCategoryRespone data = (SupportCategoryRespone)item.ItemData;
            var parameters = new NavigationParameters
            {
                { "Support", data },
                { ParameterKey.VehicleRoute, Vehicle },
            };

            switch (data.Code)
            {
                case (int)SupportPageCode.ErrorSignalPage:
                    SafeExecute(async () =>
                    {
                        await NavigationService.NavigateAsync("SupportErrorsSignalPage", parameters);
                    });
                    break;

                case (int)SupportPageCode.ChangePlateNumberPage:
                    SafeExecute(async () =>
                    {
                        await NavigationService.NavigateAsync("SupportFeePage", parameters);
                    });
                    break;

                case (int)SupportPageCode.ErrorCameraPage:
                    SafeExecute(async () =>
                    {
                        await NavigationService.NavigateAsync("SupportErrorsSignalPage", parameters);
                    });
                    break;

                default:
                    break;
            }
        }

        #endregion PrivateMethod

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
        }

        public override void OnDestroy()
        {

        }

        #endregion Lifecycle
    }
}