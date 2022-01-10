using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class DetailVehiclePopupViewModel : ViewModelBase
    {
        public ICommand FavoritesCommand { get; }
        public ICommand NavigativeCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand MoreMenuCommand { get; }

        public ICommand SelectMenuCommand { get; }

        public DetailVehiclePopupViewModel(INavigationService navigationService) : base(navigationService)
        {
            CloseCommand = new DelegateCommand(Close);
            NavigativeCommand = new DelegateCommand<object>(Navigative);
            SelectMenuCommand = new DelegateCommand<object>(SelectMenu);
            FavoritesCommand = new DelegateCommand(FavoritesVehcile);
            MoreMenuCommand = new DelegateCommand(MoreMenu);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.TryGetValue("vehicleItem", out VehicleOnlineViewModel obj))
                {
                    VehicleName = obj.PrivateCode;
                    IsFavorites = obj.IsFavorite;
                }
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

        private string vehicleName;

        public string VehicleName
        {
            get { return vehicleName; }
            set { SetProperty(ref vehicleName, value); }
        }

        private bool isFavorites;

        public bool IsFavorites
        {
            get { return isFavorites; }
            set { SetProperty(ref isFavorites, value); }
        }

        private bool isShowMoreMenu = false;

        public bool IsShowMoreMenu
        {
            get { return isShowMoreMenu; }
            set { SetProperty(ref isShowMoreMenu, value); }
        }

        private bool isFistLoad = false;

        public void Close()
        {
            NavigationService.GoBackAsync();
        }

        private void MoreMenu()
        {
            SafeExecute(() =>
            {
                IsShowMoreMenu = !IsShowMoreMenu;
                if (IsShowMoreMenu && !isFistLoad)
                {
                    isFistLoad = true;
                    GetListMenu();
                    InitMenuMore();
                    InitMenuFeatures();
                }
            });
        }

        private void InitMenuMore()
        {
            var list = new List<MenuPageItem>();
            list.Add(new MenuPageItem
            {
                Title = MobileResource.Online_Label_TitlePage,
                Icon = "ic_mornitoring.png",
                IsEnable = true,
                MenuType = MenuKeyType.Online
            });
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
                Icon = "ic_helpcustomer2.png",
                IsEnable = App.AppType == AppType.BinhAnh || App.AppType == AppType.CNN ? true : false,
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

        private void SelectMenu(object obj)
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

        private void FavoritesVehcile()
        {
            SafeExecute(async () =>
            {
                IsFavorites = !IsFavorites;
                await NavigationService.GoBackAsync(useModalNavigation: true, animated: true, parameters: new NavigationParameters
                        {
                            { "FavoriteVehicle",  IsFavorites}
                        });
            });
        }
    }
}