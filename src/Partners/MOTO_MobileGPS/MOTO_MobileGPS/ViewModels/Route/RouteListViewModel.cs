using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;

using Prism.Navigation;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using BA_MobileGPS.Core.ViewModels;

namespace MOTO_MobileGPS.ViewModels
{
    public class RouteListViewModel : ViewModelBase
    {
        private readonly IGeocodeService geocodeService;

        private IList<VehicleRoute> listRoute;
        public IList<VehicleRoute> ListRoute { get => listRoute; set => SetProperty(ref listRoute, value); }

        public ICommand GetAddressCommand { get; }

        public RouteListViewModel(INavigationService navigationService, IGeocodeService geocodeService) : base(navigationService)
        {
            this.geocodeService = geocodeService;

            GetAddressCommand = new Command<VehicleRoute>(GetAddress);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters != null)
            {
                if (parameters.TryGetValue(ParameterKey.VehicleRoute, out List<VehicleRoute> listRoute))
                {
                    ListRoute = listRoute.GroupBy(x => x.Time)
                                  .Select(g => g.First())
                                  .ToList();
                }
            }
        }

        private void GetAddress(VehicleRoute route)
        {
            if (!string.IsNullOrWhiteSpace(route.Address))
                return;

            Task.Run(async () =>
            {
                return await geocodeService.GetAddressByLatLng(route.Latitude.ToString(), route.Longitude.ToString());
            }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    route.Address = task.Result;
                }
            }));
        }
    }
}