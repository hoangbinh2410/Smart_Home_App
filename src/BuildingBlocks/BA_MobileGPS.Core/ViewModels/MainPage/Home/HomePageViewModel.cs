using BA_MobileGPS.Core.DependencyServices;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Styles;
using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities.Enums;
using Prism.Commands;
using Prism.Ioc;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private readonly IPopupServices _popupServices;
        public HomePageViewModel(INavigationService navigationService,IPopupServices popupServices) : base(navigationService)
        {
            _popupServices = popupServices;
            _listfeatures = new ObservableCollection<ItemSupport>();
            FavouriteMenuItems = new ObservableCollection<HomeMenuItem>();
            FavouriteItemsTappedCommand = new DelegateCommand<object>(FavouriteItemsTapped);
            GenerateMenu();
            OpenDiscoreryBoxCommand = new DelegateCommand(OpenDiscoreryBox);
            HotLineTapCommand = new DelegateCommand(HotLineTap);
        }

        private void FavouriteItemsTapped(object obj)
        {
            var themeService = Prism.PrismApplicationBase.Current.Container.Resolve<IThemeService>();

            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            var dark = mergedDictionaries.FirstOrDefault(x => x.GetType() == new DarkTheme().GetType());

            if (dark == null)
            {
                themeService.UpdateTheme(ThemeMode.Dark);
            }
            else themeService.UpdateTheme(ThemeMode.Light);
        }

        private void HotLineTap()
        {          
            _popupServices.ShowNotificatonPopup("Thông báo", "Cần cập nhật phiên bản mới.....");
            _popupServices.ShowNotificationIconPopup("Xác nhận", "Special care is necessary to ensure that colors will be usable on each platform. Because each platform has different defaults",
    "vehicle_MenuIcon.png", Color.Green, IconPosititon.Left);
            _popupServices.ShowNotificationIconPopup("Xác nhận", "Special care is necessary to ensure that colors will be usable on each platform. Because each platform has different defaults",
"vehicle_MenuIcon.png", Color.Green, IconPosititon.Right);
            _popupServices.ShowErrorPopup("Xác nhận", "Special care is necessary to ensure that colors will be usable on each platform. Because each platform has different defaults");
            _popupServices.ShowErrorIconPopup("Xác nhận", "Special care is necessary to ensure that colors will be usable on each platform. Because each platform has different defaults",
    "vehicle_MenuIcon.png", Color.Green, IconPosititon.Left);
            _popupServices.ShowErrorIconPopup("Xác nhận", "Special care is necessary to ensure that colors will be usable on each platform. Because each platform has different defaults",
    "vehicle_MenuIcon.png", Color.Green, IconPosititon.Right);
            _popupServices.ShowConfirmIconPopup("Xác nhận", "Special care is necessary to ensure that colors will be usable on each platform. Because each platform has different defaults",
                "vehicle_MenuIcon.png", Color.Green,IconPosititon.Left);
            _popupServices.ShowConfirmIconPopup("Xác nhận", "Special care is necessary to ensure that colors will be usable on each platform. Because each platform has different defaults",
               "vehicle_MenuIcon.png", Color.Green, IconPosititon.Right);
            _popupServices.ShowConfirmPopup("Xác nhận", "Special care is necessary to ensure that colors will be usable on each platform. Because each platform has different defaults");

        }

        private ObservableCollection<HomeMenuItem> _favouriteMenuItems;

        public ObservableCollection<HomeMenuItem> FavouriteMenuItems
        {
            get => _favouriteMenuItems;
            set
            {
                SetProperty(ref _favouriteMenuItems, value);
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<ItemSupport> _listfeatures;

        public ObservableCollection<ItemSupport> AllListfeatures
        {
            get => _listfeatures;
            set
            {
                SetProperty(ref _listfeatures, value);
                RaisePropertyChanged();
            }
        }

        public ICommand FavouriteItemsTappedCommand { get; }
        public ICommand OpenDiscoreryBoxCommand { get; }
        public ICommand HotLineTapCommand { get; }

        private void GenerateMenu()
        {
            GenerateFavourites(StaticSettings.ListMenu);
            GenerateListFeatures(StaticSettings.ListMenu);
        }

        private void GenerateFavourites(List<HomeMenuItem> input)
        {
            var menuFavoriteIds = "153,154";
            if (!string.IsNullOrEmpty(menuFavoriteIds))
            {
                var favoritesIdLst = menuFavoriteIds.Split(',').Select(m => int.Parse(m));
                FavouriteMenuItems = input.Where(x => favoritesIdLst.Contains(x.PK_MenuItemID)).ToObservableCollection();
            }
        }

        private void GenerateListFeatures(List<HomeMenuItem> input)
        {
            for (int i = 0; i < input.Count / 6.0; i++)
            {
                var temp = new ItemSupport();
                temp.FeaturesItem = input.Skip(i * 6).Take(6).ToList();

                AllListfeatures.Add(temp);
            }
        }

        private void OpenDiscoreryBox()
        {
            PopupNavigation.Instance.PushAsync(new DiscoveryPopup(StaticSettings.ListMenu));
        }
    }

    public class ItemSupport
    {
        public List<HomeMenuItem> FeaturesItem { get; set; }
    }
}