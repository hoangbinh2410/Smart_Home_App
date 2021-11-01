using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.Utilities;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ListMenuPopupPageViewModel : ViewModelBase
    {
        private readonly IHomeService homeService;
        private readonly IMapper mapper;
        public ICommand TapMenuCommand { get; set; }
        public ICommand NavigativeCommand { get; set; }

        public ICommand CloseCommand { get; }

        public ListMenuPopupPageViewModel(INavigationService navigationService,
            IHomeService homeService, IMapper mapper) : base(navigationService)
        {
            this.homeService = homeService;
            this.mapper = mapper;
            TapMenuCommand = new DelegateCommand<object>(OnTappedMenu);
            NavigativeCommand = new DelegateCommand<object>(Navigative);
            CloseCommand = new DelegateCommand(Close);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            // Lấy danh sách menu
            GetListMenu();
            InitMenuItems();
            InitMenuFeatures();
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.TryGetValue("vehicleItem", out VehicleOnline obj))
                {
                    CarActive = obj;
                }
            }
        }

        #region Property

        private VehicleOnline carActive;

        public VehicleOnline CarActive
        {
            get { return carActive; }
            set
            {
                carActive = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<MenuPageItem> listfeatures;

        public ObservableCollection<MenuPageItem> AllListfeatures
        {
            get => listfeatures;

            set
            {
                SetProperty(ref listfeatures, value);
            }
        }

        private ObservableCollection<ItemMenu> listCamera;

        public ObservableCollection<ItemMenu> AllListCamera
        {
            get => listCamera;

            set
            {
                SetProperty(ref listCamera, value);
            }
        }

        private ObservableCollection<ItemMenu> listReport;

        public ObservableCollection<ItemMenu> AllListReport
        {
            get => listReport;

            set
            {
                SetProperty(ref listReport, value);
            }
        }

        private ObservableCollection<MenuPageItem> menuItems = new ObservableCollection<MenuPageItem>();

        public ObservableCollection<MenuPageItem> MenuItems
        {
            get
            {
                return menuItems;
            }
            set
            {
                SetProperty(ref menuItems, value);
                RaisePropertyChanged();
            }
        }

        #endregion Property

        #region Private method

        private void InitMenuItems()
        {
            var list = new List<MenuPageItem>();

            list.Add(new MenuPageItem
            {
                Title = MobileResource.Route_Label_Title,
                Icon = "ic_route.png",
                IsEnable = CheckPermision((int)PermissionKeyNames.ViewModuleRoute),
                MenuType = MenuKeyType.Route
            });
            list.Add(new MenuPageItem
            {
                Title = MobileResource.DetailVehicle_Label_TilePage,
                Icon = "ic_guarantee.png",
                IsEnable = true,
                MenuType = MenuKeyType.VehicleDetail
            });
            list.Add(new MenuPageItem
            {
                Title = "Hỗ trợ khách hàng",
                Icon = "ic_helpcustomer.png",
                IsEnable = true,
                MenuType = MenuKeyType.HelpCustomer
            });
            MenuItems = list.Where(x => x.IsEnable == true).ToObservableCollection();
        }

        private void InitMenuFeatures()
        {
            var list = new List<MenuPageItem>();

            list.Add(new MenuPageItem
            {
                Title = MobileResource.Camera_Lable_ExportVideo,
                Icon = "ic_exportvideo.png",
                IsEnable = CheckPermision((int)PermissionKeyNames.UploadVideoStream),
                MenuType = MenuKeyType.ExportVideo
            });
            list.Add(new MenuPageItem
            {
                Title = "Hỗ trợ khách hàng",
                Icon = "ic_helpcustomer.png",
                IsEnable = true,
                MenuType = MenuKeyType.HelpCustomer
            });

            //list.Add(new MenuPageItem
            //{
            //    Title = "SOS",
            //    Icon = "ic_mail.png",
            //    IsEnable = true,
            //    MenuType = MenuKeyType.SOS
            //});
            AllListfeatures = list.Where(x => x.IsEnable == true).ToObservableCollection();
        }

        private void GetListMenu()
        {
            TryExecute(() =>
            {
                var result = StaticSettings.ListMenu;
                if (result != null)
                {
                    var lstCamera = result.Where(s => s.MenuItemParentID == 2022).ToList();
                    AllListCamera = GenerateListFeatures(lstCamera).ToObservableCollection();

                    var lstReport = result.Where(s => s.MenuItemParentID == 2023).ToList();
                    AllListReport = GenerateListFeatures(lstReport).ToObservableCollection();
                }
            });
        }

        private List<ItemMenu> GenerateListFeatures(List<HomeMenuItem> input)
        {
            var list = new List<ItemMenu>();
            for (int i = 0; i < input.Count / 3.0; i++)
            {
                var temp = new ItemMenu();
                temp.FeaturesItem = input.Skip(i * 3).Take(3).ToList();
                list.Add(temp);
            }
            return list;
        }

        public void OnTappedMenu(object obj)
        {
            if (obj == null)
                return;
            if (!(obj is MenuPageItem seletedMenu))
            {
                return;
            }
            SafeExecute(async () =>
            {
                var param = seletedMenu.MenuType;
                await NavigationService.GoBackAsync(useModalNavigation: true, animated: true, parameters: new NavigationParameters
                        {
                            { "MenuPageItem",  param}
                        });
            });
        }

        private void Navigative(object obj)
        {
            if (!(obj is HomeMenuItem seletedMenu) || seletedMenu.MenuKey == null)
            {
                return;
            }
            SafeExecute(async () =>
            {
                var param = seletedMenu.MenuKey.ToString();
                await NavigationService.GoBackAsync(useModalNavigation: true, animated: true, parameters: new NavigationParameters
                        {
                            { "pagetoNavigation",  param}
                        });
            });
        }

        public void Close()
        {
            NavigationService.GoBackAsync();
        }

        #endregion Private method
    }

    public class ItemMenu
    {
        public List<HomeMenuItem> FeaturesItem { get; set; }
    }
}