using AutoMapper;
using BA_MobileGPS.Core.Events;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly IHomeService homeService;
        private readonly IMapper mapper;
        private Timer timer;
        private List<HomeMenuItemViewModel> MenuReponse = new List<HomeMenuItemViewModel>();

        public bool hasFavorite;
        public bool HasFavorite { get => hasFavorite; set => SetProperty(ref hasFavorite, value); }
        public ICommand TapMenuCommand { get; set; }

        public HomeViewModel(INavigationService navigationService,
            IHomeService homeService, IMapper mapper)
            : base(navigationService)
        {
            this.homeService = homeService;
            this.mapper = mapper;

            TapMenuCommand = new DelegateCommand<object>(OnTappedMenu);
            _listfeatures = new ObservableCollection<ItemSupport>();
            _favouriteMenuItems = new ObservableCollection<HomeMenuItemViewModel>();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            // Lấy danh sách menu
            GetListMenu();
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

        private void GetListMenu()
        {
            if (!IsConnected)
                return;
            TryExecute(() =>
            {
                RunOnBackground(async () =>
                {
                    return await homeService.GetHomeMenuAsync((int)App.AppType, Settings.CurrentLanguage);
                }, (result) =>
                {
                    if (result != null && result.Count > 0)
                    {
                        MenuReponse = mapper.Map<List<HomeMenuItemViewModel>>(result);

                        GenMenu();
                    }
                });
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

            GenerateListFeatures(menus.ToList());
            StaticSettings.ListMenuOriginGroup = mapper.Map<List<HomeMenuItem>>(menus);

            if (!string.IsNullOrEmpty(menuFavoriteIds))
            {
                HasFavorite = true;
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
            else
            {
                HasFavorite = false;
            }

            var result =
                from m in menus
                orderby m.IsFavorited descending, m.SortOrder, m.GroupName descending
                select m;
            FavouriteMenuItems = result.Where(s => s.IsFavorited).ToObservableCollection();

            StaticSettings.ListMenu = mapper.Map<List<HomeMenuItem>>(result);
        }

        private void GenerateListFeatures(List<HomeMenuItemViewModel> input)
        {
            AllListfeatures.Clear();
            // 6 Item per indicator view in Sflistview
            for (int i = 0; i < input.Count / 6.0; i++)
            {
                var temp = new ItemSupport();
                temp.FeaturesItem = input.Skip(i * 6).Take(6).ToList();
                AllListfeatures.Add(temp);
            }
        }

        public ICommand NavigateToFavoriteCommand => new Command(() =>
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/FavoritesConfigurationsPage", null, useModalNavigation: true);
            });
        });

        public void OnTappedMenu(object obj)
        {
            var args = (Syncfusion.ListView.XForms.ItemTappedEventArgs)obj;
            var temp = (HomeMenuItemViewModel)args.ItemData;
            switch (temp.MenuKey)
            {
                case "ListVehiclePage":
                    EventAggregator.GetEvent<TabItemSwitchEvent>().Publish(new Tuple<ItemTabPageEnums, object>(ItemTabPageEnums.ListVehiclePage, ""));
                    break;

                case "OnlinePage":
                    EventAggregator.GetEvent<TabItemSwitchEvent>().Publish(new Tuple<ItemTabPageEnums, object>(ItemTabPageEnums.OnlinePage, ""));
                    break;

                case "RoutePage":
                    EventAggregator.GetEvent<TabItemSwitchEvent>().Publish(new Tuple<ItemTabPageEnums, object>(ItemTabPageEnums.RoutePage, ""));
                    break;

                default:
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            if (IsBusy)
                                return;
                            IsBusy = true;

                            if (!(args.ItemData is HomeMenuItemViewModel seletedMenu) || seletedMenu.MenuKey == null)
                            {
                                return;
                            }
                            //await NavigationService.NavigateAsync("NotificationPopup", useModalNavigation: true);
                            using (new HUDService(MobileResource.Common_Message_Processing))
                            {
                                _ = await NavigationService.NavigateAsync("BaseNavigationPage/" + seletedMenu.MenuKey, useModalNavigation: true);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                        }
                        finally
                        {
                            IsBusy = false;
                        }
                    });
                    break;
            }
        }

        private ObservableCollection<ItemSupport> _listfeatures;

        public ObservableCollection<ItemSupport> AllListfeatures
        {
            get => _listfeatures;

            set
            {
                SetProperty(ref _listfeatures, value);
            }
        }

        private ObservableCollection<HomeMenuItemViewModel> _favouriteMenuItems;

        public ObservableCollection<HomeMenuItemViewModel> FavouriteMenuItems
        {
            get => _favouriteMenuItems;
            set
            {
                SetProperty(ref _favouriteMenuItems, value);
                RaisePropertyChanged();
            }
        }
    }

    public class ItemSupport
    {
        public List<HomeMenuItemViewModel> FeaturesItem { get; set; }
    }
}