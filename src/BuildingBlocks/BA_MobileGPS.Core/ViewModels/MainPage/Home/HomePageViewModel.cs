using BA_MobileGPS.Core.DependencyServices;
using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Syncfusion.Data.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Ioc;
using Xamarin.Forms;
using BA_MobileGPS.Core.Styles;
using System;
using BA_MobileGPS.Utilities.Enums;

namespace BA_MobileGPS.Core.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public HomePageViewModel(INavigationService navigationService) : base(navigationService)
        {
     
            _listfeatures = new ObservableCollection<ItemSupport>();
            FavouriteMenuItems = new ObservableCollection<HomeMenuItem>();
            FavouriteItemsTappedCommand = new DelegateCommand<object>(FavouriteItemsTapped);
            NumberOfColumns = 2;
            GenerateMenu();
        }


        private void FavouriteItemsTapped(object obj)
        {
            var themeService = Prism.PrismApplicationBase.Current.Container.Resolve<IThemeService>();


            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            var dark = mergedDictionaries.FirstOrDefault(x =>x.GetType() == new DarkTheme().GetType());

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

        private int _numberOfColumns;

       

        public int NumberOfColumns
        {
            get { return _numberOfColumns; }
            set { SetProperty(ref _numberOfColumns, value);}
        }

        public ICommand FavouriteItemsTappedCommand { get; }

       
        private  void GenerateMenu()
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
            for (int i = 0; i < input.Count/6.0 ; i++)
            {
                var temp = new ItemSupport();
                temp.FeaturesItem = input.Skip(i * 6).Take(6).ToList();

                AllListfeatures.Add(temp);
            }
        }

      
    }
    public class ItemSupport
    {
        public List<HomeMenuItem> FeaturesItem { get; set; }
    }
}