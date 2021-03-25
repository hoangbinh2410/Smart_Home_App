using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Prism.Navigation.TabbedPages;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class HomeViewModel : TabbedPageChildVMBase
    {
        private readonly IHomeService homeService;
        private readonly IMapper mapper;
        private List<HomeMenuItemViewModel> MenuReponse = new List<HomeMenuItemViewModel>();

        #region Constructor

        public HomeViewModel(INavigationService navigationService,
            IHomeService homeService, IMapper mapper)
            : base(navigationService)
        {
            this.homeService = homeService;
            this.mapper = mapper;

            TapMenuCommand = new DelegateCommand<object>(OnTappedMenu);
            listfeatures = new ObservableCollection<ItemSupport>();
            favouriteMenuItems = new ObservableCollection<ItemSupport>();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            // Lấy danh sách menu
            GetListMenu();
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue("IsFavoriteChange", out bool isChanged))
            {
                if (isChanged)
                {
                    GenMenu();
                }
            }
        }

        #endregion Constructor

        #region Icommand

        public ICommand NavigateToFavoriteCommand => new Command(() =>
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/FavoritesConfigurationsPage", null, useModalNavigation: true, true);
            });
        });

        public ICommand PushWebCommand => new Command(() =>
        {
            if (!string.IsNullOrEmpty(MobileSettingHelper.LinkAdvertising))
            {
                SafeExecute(async () => await Launcher.OpenAsync(new Uri(MobileSettingHelper.LinkAdvertising)));
            }
        });

        public ICommand TapMenuCommand { get; set; }

        #endregion Icommand

        #region Private method

        private void GetListMenu()
        {
            RunOnBackground(async () =>
            {
                return await homeService.GetHomeMenuAsync((int)App.AppType, Settings.CurrentLanguage);
            }, (result) =>
            {
                if (result != null && result.Count > 0)
                {
                    MenuReponse = mapper.MapListProperties<HomeMenuItemViewModel>(result.ToList());

                    GenMenu();
                }
            });
        }

        private void GenMenu()
        {
            // Lấy danh sách menu ưa thích theo danh sách chuối id
            string menuFavoriteIds = MobileUserSettingHelper.MenuFavorite;
            var menus =
                from m1 in MenuReponse
                join config in UserInfo.Permissions.Distinct() // Lấy theo permission menu
                on m1.PermissionViewID equals config
                join m2 in MenuReponse
                on m1.MenuItemParentID equals m2.PK_MenuItemID
                into gj
                from sub in gj.DefaultIfEmpty()
                where m1.MenuItemParentID != 0
                select new HomeMenuItemViewModel()
                {
                    FK_LanguageTypeID = m1.FK_LanguageTypeID,
                    IconMobile = m1.IconMobile,
                    GroupName = sub?.NameByCulture ?? m1.NameByCulture,
                    MenuKey = m1.MenuKey,
                    NameByCulture = m1.NameByCulture,
                    PK_MenuItemID = m1.PK_MenuItemID,
                    PermissionViewID = m1.PermissionViewID,
                    SortOrder = m1.SortOrder,
                    MenuItemParentID = m1.MenuItemParentID,
                    LanguageCode = m1.LanguageCode,
                };
            StaticSettings.ListMenuOriginGroup = mapper.MapListProperties<HomeMenuItem>(menus.ToList());

            if (!string.IsNullOrEmpty(menuFavoriteIds))
            {
                var favoritesIdLst = menuFavoriteIds.Split(',').Select(m => int.Parse(m));
                menus =
                    from m in menus
                    join fv in favoritesIdLst
                    on m.PK_MenuItemID equals fv
                    into gj
                    from fv_sub in gj.DefaultIfEmpty()
                    select new HomeMenuItemViewModel()
                    {
                        FK_LanguageTypeID = m.FK_LanguageTypeID,
                        IconMobile = m.IconMobile,
                        GroupName = !(fv_sub == 0) ? MobileResource.Menu_Label_Favorite : m.GroupName,
                        MenuKey = m.MenuKey,
                        NameByCulture = m.NameByCulture,
                        PK_MenuItemID = m.PK_MenuItemID,
                        PermissionViewID = m.PermissionViewID,
                        SortOrder = m.SortOrder,
                        MenuItemParentID = m.MenuItemParentID,
                        LanguageCode = m.LanguageCode,
                        IsFavorited = !(fv_sub == 0),
                    };
            }

            var result =
                from m in menus
                orderby m.IsFavorited descending, m.SortOrder, m.GroupName descending
                select m;
            var favourites = result.Where(s => s.IsFavorited).ToList();
            GenerateFavouriteMenu(favourites);
            var notFavorites = result.Where(s => !s.IsFavorited).ToList();

            GenerateListFeatures(notFavorites);
            HasFavorite = FavouriteMenuItems.Count != 0;
            StaticSettings.ListMenu = mapper.MapListProperties<HomeMenuItem>(result.ToList());
        }

        private void GenerateFavouriteMenu(List<HomeMenuItemViewModel> input)
        {
            FavouriteMenuItems.Clear();
            var list = new List<ItemSupport>();
            for (int i = 0; i < input.Count / 3.0; i++)
            {
                var temp = new ItemSupport();
                temp.FeaturesItem = input.Skip(i * 3).Take(3).ToList();
                list.Add(temp);
            }
            FavouriteMenuItems = list.ToObservableCollection();
        }

        private void GenerateListFeatures(List<HomeMenuItemViewModel> input)
        {
            var list = new List<ItemSupport>();
            // 6 Item per indicator view in Sflistview
            for (int i = 0; i < input.Count / 6.0; i++)
            {
                var temp = new ItemSupport();
                temp.FeaturesItem = input.Skip(i * 6).Take(6).ToList();
                list.Add(temp);
            }
            AllListfeatures = list.ToObservableCollection();
        }

        public async void OnTappedMenu(object obj)
        {
            if (!(obj is HomeMenuItemViewModel seletedMenu) || seletedMenu.MenuKey == null)
            {
                return;
            }
            var temp = seletedMenu;
            switch (temp.MenuKey)
            {
                case "ListVehiclePage":
                    if (App.AppType == AppType.VMS)
                    {
                        await NavigationService.SelectTabAsync("ListVehiclePageVMS");
                    }
                    else
                    {
                        await NavigationService.SelectTabAsync("ListVehiclePage");
                    }
                    break;

                case "OnlinePage":
                    if (App.AppType == AppType.Moto)
                    {
                        if (MobileUserSettingHelper.EnableShowCluster)
                        {
                            await NavigationService.SelectTabAsync("OnlinePageMoto");
                        }
                        else
                        {
                            await NavigationService.SelectTabAsync("OnlinePageNoClusterMoto");
                        }
                    }
                    else if (App.AppType == AppType.VMS)
                    {
                        if (MobileUserSettingHelper.EnableShowCluster)
                        {
                            await NavigationService.SelectTabAsync("OnlinePageVMS");
                        }
                        else
                        {
                            await NavigationService.SelectTabAsync("OnlinePageNoClusterVMS");
                        }
                    }
                    else
                    {
                        if (MobileUserSettingHelper.EnableShowCluster)
                        {
                            await NavigationService.SelectTabAsync("OnlinePage");
                        }
                        else
                        {
                            await NavigationService.SelectTabAsync("OnlinePageNoCluster");
                        }
                    }
                    break;

                case "RoutePage":
                    if (App.AppType == AppType.VMS)
                    {
                        await NavigationService.SelectTabAsync("RoutePageVMS");
                    }
                    else
                    {
                        await NavigationService.SelectTabAsync("RoutePage");
                    }
                    break;

                case "MessagesOnlinePage":
                    SafeExecute(async () =>
                    {
                        using (new HUDService(MobileResource.Common_Message_Processing))
                        {
                            _ = await NavigationService.NavigateAsync("NavigationPage/" + seletedMenu.MenuKey, null, useModalNavigation: true, true);
                        }
                    });
                    break;

                case "CameraManagingPage":
                    SafeExecute(async () =>
                    {
                        var photoPermission = await PermissionHelper.CheckPhotoPermissions();
                        var storagePermission = await PermissionHelper.CheckStoragePermissions();
                        if (photoPermission && storagePermission)
                        {
                            using (new HUDService(MobileResource.Common_Message_Processing))
                            {
                                var a = await NavigationService.NavigateAsync("NavigationPage/" + seletedMenu.MenuKey, null, useModalNavigation: true, true);
                            }
                        }
                    });

                    break;

                case "CameraRestream":
                    SafeExecute(async () =>
                    {
                        using (new HUDService(MobileResource.Common_Message_Processing))
                        {
                            _ = await NavigationService.NavigateAsync("BaseNavigationPage/" + seletedMenu.MenuKey, null, useModalNavigation: true, true);
                        }
                    });
                    break;

                default:
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        SafeExecute(async () =>
                        {//await NavigationService.NavigateAsync("NotificationPopup", useModalNavigation: true);
                            using (new HUDService(MobileResource.Common_Message_Processing))
                            {
                                var a = await NavigationService.NavigateAsync("NavigationPage/" + seletedMenu.MenuKey, null, useModalNavigation: true, true);
                            }
                        });
                    });
                    break;
            }
        }

        #endregion Private method

        #region Property Binding

        private ObservableCollection<ItemSupport> listfeatures;

        public ObservableCollection<ItemSupport> AllListfeatures
        {
            get => listfeatures;

            set
            {
                SetProperty(ref listfeatures, value);
            }
        }

        private ObservableCollection<ItemSupport> favouriteMenuItems;

        public ObservableCollection<ItemSupport> FavouriteMenuItems
        {
            get => favouriteMenuItems;
            set
            {
                SetProperty(ref favouriteMenuItems, value);
                RaisePropertyChanged();
            }
        }

        public bool hasFavorite;

        public bool HasFavorite
        {
            get => hasFavorite;
            set
            {
                SetProperty(ref hasFavorite, value);
                RaisePropertyChanged();
            }
        }

        #endregion Property Binding
    }

    public class ItemSupport
    {
        public List<HomeMenuItemViewModel> FeaturesItem { get; set; }
    }
}