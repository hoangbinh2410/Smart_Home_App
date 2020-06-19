﻿using BA_MobileGPS.Core.ViewModels.Base;
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

namespace BA_MobileGPS.Core.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private IHomeService _homeServices;
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
            var menuFavoriteIds = "122,161,152,153,154";
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