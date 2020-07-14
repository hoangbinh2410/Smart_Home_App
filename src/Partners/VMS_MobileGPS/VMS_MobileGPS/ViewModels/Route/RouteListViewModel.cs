using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;

using Prism.Navigation;

using System.Collections.Generic;

namespace VMS_MobileGPS.ViewModels
{
    public class RouteListViewModel : ViewModelBase
    {
        private readonly IGeocodeService geocodeService;

        private IList<VehicleRoute> listRoute;
        public IList<VehicleRoute> ListRoute { get => listRoute; set => SetProperty(ref listRoute, value); }

        public RouteListViewModel(INavigationService navigationService, IGeocodeService geocodeService) : base(navigationService)
        {
            this.geocodeService = geocodeService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters != null)
            {
                if (parameters.TryGetValue(ParameterKey.VehicleRoute, out IList<VehicleRoute> listRoute))
                {
                    for (int i = 0; i < listRoute.Count; i++)
                    {
                        listRoute[i].Address = string.Join(", ", GeoHelper.LatitudeToDergeeMinSec(listRoute[i].Latitude), GeoHelper.LongitudeToDergeeMinSec(listRoute[i].Longitude));
                    }

                    ListRoute = listRoute;
                }
            }
        }
    }
}