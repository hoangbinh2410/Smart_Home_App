using BA_MobileGPS.Core.DependencyServices;
using BA_MobileGPS.Core.Events;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Core.Styles;
using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities.Enums;
using Prism.Commands;
using Prism.Events;
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
        private readonly IEventAggregator _eventAggregator;
        public HomePageViewModel(INavigationService navigationService,IPopupServices popupServices, IEventAggregator eventAggregator) : base(navigationService)
        {
            _eventAggregator = eventAggregator;
            _popupServices = popupServices;
            _listfeatures = new ObservableCollection<ItemSupport>();
            FavouriteMenuItems = new ObservableCollection<HomeMenuItem>();
            ItemsTappedCommand = new DelegateCommand<object>(ItemsTapped);
            GenerateMenu();
            OpenDiscoreryBoxCommand = new DelegateCommand(OpenDiscoreryBox);
            HotLineTapCommand = new DelegateCommand(HotLineTap);
            HobbiesIconTapCommand = new DelegateCommand(HobbiesIconTap);
        }

        private void ItemsTapped(object obj)
        {
            var item = (HomeMenuItem)((Syncfusion.ListView.XForms.ItemTappedEventArgs)obj).ItemData;
            if (StaticSettings.ListMenu.Contains(item))
            {
                _eventAggregator.GetEvent<TabItemSwitchEvent>().Publish(item);
            }
            else
            {
                //Navigation services
            }
          

            
        }

        private void HotLineTap()
        {
            var mes = "Special care is necessary to ensure that colors will be usable on each platform. Because each platform has different defaults";
            _popupServices.ShowNotificatonPopup("Thông báo", "Cần cập nhật phiên bản mới.....");
            _popupServices.ShowNotificationIconPopup("Xác nhận", mes, "vehicle_MenuIcon.png", Color.Green, IconPosititon.Left);
            _popupServices.ShowNotificationIconPopup("Xác nhận", mes, "vehicle_MenuIcon.png", Color.Green, IconPosititon.Right);
            _popupServices.ShowErrorPopup("Xác nhận", mes);
            _popupServices.ShowErrorIconPopup("Xác nhận", mes, "vehicle_MenuIcon.png", Color.Green, IconPosititon.Left);
            _popupServices.ShowErrorIconPopup("Xác nhận",mes, "vehicle_MenuIcon.png", Color.Green, IconPosititon.Right);
            _popupServices.ShowConfirmIconPopup("Xác nhận", mes, "vehicle_MenuIcon.png", Color.Green,IconPosititon.Left);
            _popupServices.ShowConfirmIconPopup("Xác nhận", mes, "vehicle_MenuIcon.png", Color.Green, IconPosititon.Right);
            _popupServices.ShowConfirmPopup("Xác nhận", mes);
        }

        private void HobbiesIconTap()
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

        public ICommand ItemsTappedCommand { get; }
        public ICommand OpenDiscoreryBoxCommand { get; }
        public ICommand HotLineTapCommand { get; }
        public ICommand HobbiesIconTapCommand { get; }

        private void GenerateMenu()
        {
            GenerateFavourites(StaticSettings.ListMenuOriginGroup);
            GenerateListFeatures(StaticSettings.ListMenuOriginGroup);
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
            PopupNavigation.Instance.PushAsync(new DiscoveryPopup(StaticSettings.ListMenuOriginGroup));
        }
    }

    public class ItemSupport
    {
        public List<HomeMenuItem> FeaturesItem { get; set; }
    }
}