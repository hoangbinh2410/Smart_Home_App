using BA_MobileGPS.Entities.Enums;
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
        #region Property

        private List<SupportCategoryRespone> menuItems = new List<SupportCategoryRespone>();

        public List<SupportCategoryRespone> MenuItems
        {
            get { return menuItems; }
            set { SetProperty(ref menuItems, value); }
        }

        #endregion Property

        #region Contructor

        public ICommand NavigateCommand { get; }
        private ISupportCategoryService _iSupportCategoryService;

        public SupportClientPageViewModel(INavigationService navigationService, ISupportCategoryService iSupportCategoryService)
            : base(navigationService)
        {
            NavigateCommand = new DelegateCommand<ItemTappedEventArgs>(NavigateClicked);
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

        private void NavigateClicked(ItemTappedEventArgs item)
        {
            SupportCategoryRespone data = (SupportCategoryRespone)item.ItemData;
            var parameters = new NavigationParameters
            {
                { "Support", data }
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