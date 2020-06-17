using BA_MobileGPS.Entities;

using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class HomePageViewModel : BindableBase
    {
        public HomePageViewModel()
        {
            SetFavourites();
            SetFeatureSource();
            FavouriteItemsTappedCommand = new DelegateCommand<object>(FavouriteItemsTapped);
            NumberOfColumns = 2;
        }

        private void FavouriteItemsTapped(object obj)
        {
        }

        private IList<HomeMenuItemViewModel> _favouriteMenuItems;

        public IList<HomeMenuItemViewModel> FavouriteMenuItems
        {
            get => _favouriteMenuItems;
            set => SetProperty(ref _favouriteMenuItems, value);
        }

        private List<ItemSupport> _listfeatures;

        public List<ItemSupport> AllListfeatures
        {
            get => _listfeatures;
            set => SetProperty(ref _listfeatures, value);
        }

        private int _numberOfColumns;

        public int NumberOfColumns
        {
            get { return _numberOfColumns; }
            set { SetProperty(ref _numberOfColumns, value); }
        }

        public ICommand FavouriteItemsTappedCommand { get; }

        private void SetFavourites()
        {
            _favouriteMenuItems = new List<HomeMenuItemViewModel>()
            {
                new HomeMenuItemViewModel()
                {
                    IconMobile = "monitoringGreen_Icon.png",
                    NameByCulture = "Giám sát"
                },
                new HomeMenuItemViewModel()
                {
                    IconMobile = "voyage_Icon.png",
                    NameByCulture = "Hải trình"
                },
                new HomeMenuItemViewModel()
                {
                    IconMobile = "vehicles_Icon.png",
                    NameByCulture = "Phương tiện"
                },
                new HomeMenuItemViewModel()
                {
                    IconMobile = "servicesFee_Icon.png",
                    NameByCulture = "Phí dịch vụ"
                },
                new HomeMenuItemViewModel()
                {
                    IconMobile = "monitoringGreen_Icon.png",
                    NameByCulture = "Giám sát 2"
                },
                new HomeMenuItemViewModel()
                {
                    IconMobile = "voyage_Icon.png",
                    NameByCulture = "Hải trình 2"
                },
            };
        }

        private void SetFeatureSource()
        {
            _listfeatures = new List<ItemSupport>()
           {
               new ItemSupport()
               {
                   FeaturesItem = new List<HomeMenuItemViewModel>(){
                   new HomeMenuItemViewModel()
                {
                    IconMobile = "monitoringGreen_Icon.png",
                    NameByCulture = "Giám sát"
                },
                new HomeMenuItemViewModel()
                {
                    IconMobile = "voyage_Icon.png",
                    NameByCulture = "Hải trình"
                },
                new HomeMenuItemViewModel()
                {
                    IconMobile = "vehicles_Icon.png",
                    NameByCulture = "Phương tiện"
                },
                new HomeMenuItemViewModel()
                {
                    IconMobile = "servicesFee_Icon.png",
                    NameByCulture = "Phí dịch vụ"
                },
                new HomeMenuItemViewModel()
                {
                    IconMobile = "monitoringGreen_Icon.png",
                    NameByCulture = "Giám sát 2"
                },
                new HomeMenuItemViewModel()
                {
                    IconMobile = "voyage_Icon.png",
                    NameByCulture = "Hải trình 2"
                },
                   }
               },
               new ItemSupport()
               {
                   FeaturesItem = new List<HomeMenuItemViewModel>(){
                   new HomeMenuItemViewModel()
                {
                    IconMobile = "monitoringGreen_Icon.png",
                    NameByCulture = "Giám sát 1"
                },
                new HomeMenuItemViewModel()
                {
                    IconMobile = "voyage_Icon.png",
                    NameByCulture = "Hải trình 1"
                },
                new HomeMenuItemViewModel()
                {
                    IconMobile = "vehicles_Icon.png",
                    NameByCulture = "Phương tiện 1"
                },
                new HomeMenuItemViewModel()
                {
                    IconMobile = "servicesFee_Icon.png",
                    NameByCulture = "Phí dịch vụ 1"
                },
                new HomeMenuItemViewModel()
                {
                    IconMobile = "monitoringGreen_Icon.png",
                    NameByCulture = "Giám sát 21"
                },
                new HomeMenuItemViewModel()
                {
                    IconMobile = "voyage_Icon.png",
                    NameByCulture = "Hải trình 21"
                },
                   }
               }
           };
   
        }
    }
    public class ItemSupport
    {
        public List<HomeMenuItemViewModel> FeaturesItem { get; set; }
    }
}