using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;

using Prism.Commands;
using Prism.Navigation;

using System.Windows.Input;

namespace VMS_MobileGPS.ViewModels
{
    public class LocationDergeeInputViewModel : VMSBaseViewModel
    {
        private PositionDergee latitude;
        public PositionDergee Latitude { get => latitude; set => SetProperty(ref latitude, value); }

        private PositionDergee longitude;
        public PositionDergee Longitude { get => longitude; set => SetProperty(ref longitude, value); }

        public ICommand AcceptCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public LocationDergeeInputViewModel(INavigationService navigationService) : base(navigationService)
        {
            Latitude = new PositionDergee();
            Longitude = new PositionDergee();

            AcceptCommand = new DelegateCommand(Accept);
            CancelCommand = new DelegateCommand(Cancel);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters == null)
                return;

            if (parameters.ContainsKey(ParameterKey.PageMode))
            {
                ViewMode = parameters.GetValue<PageMode>(ParameterKey.PageMode);
            }

            if (parameters.TryGetValue(ParameterKey.Position, out Position position))
            {
                GeoHelper.LatitudeToDergeeMinSec(position.Latitude, out PositionDergee latitude);
                Latitude = latitude;

                GeoHelper.LongitudeToDergeeMinSec(position.Longitude, out PositionDergee longitude);
                Longitude = longitude;
            }
        }

        private async void Accept()
        {
            var latitude = GeoHelper.DergeeMinSecToLatitude(Latitude.Dergee, Latitude.Min, Latitude.Sec);
            var longitude = GeoHelper.DergeeMinSecToLatitude(Longitude.Dergee, Longitude.Min, Longitude.Sec);

            await NavigationService.GoBackAsync(useModalNavigation: true, parameters: new NavigationParameters
            {
                { ParameterKey.Position, new Position(latitude, longitude) }
            });
        }

        private async void Cancel()
        {
            await NavigationService.GoBackAsync(useModalNavigation: true);
        }
    }
}