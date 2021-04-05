using BA_MobileGPS.Entities;
using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class DetailVehiclePopupViewModel : ViewModelBase
    {
        public DetailVehiclePopupViewModel(INavigationService navigationService) : base(navigationService)
        {
            CloseCommand = new DelegateCommand(Close);
            NavigativeCommand = new DelegateCommand<object>(Navigative);
            FavoritesCommand = new DelegateCommand(FavoritesVehcile);
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

        public ICommand CloseCommand { get; }

        public void Close()
        {
            NavigationService.GoBackAsync();
        }

        public ICommand NavigativeCommand { get; }

        private void Navigative(object obj)
        {
            if (!(obj is MenuItem seletedMenu))
            {
                return;
            }
            SafeExecute(async () =>
            {
                var param = seletedMenu.Title.ToString();
                await NavigationService.GoBackAsync(useModalNavigation: true, animated: true, parameters: new NavigationParameters
                        {
                            { "pagetoNavigation",  param}
                        });
            });
        }


        public ICommand FavoritesCommand { get; }

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