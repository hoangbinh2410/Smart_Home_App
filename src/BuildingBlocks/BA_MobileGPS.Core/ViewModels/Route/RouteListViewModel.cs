using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;

using Prism.Navigation;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
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
                    var lst = listRoute.GroupBy(x => x.Time)
                                     .Select(g => g.First())
                                     .ToList();
                    foreach (var item in lst)
                    {
                        if (item.StateType !=null)
                        {
                            switch (item.StateType?.State)
                            {
                                case StateType.Normal:
                                    item.StateType.StateText= MobileResource.Online_Label_StatusCarMoving;
                                    break;

                                case StateType.Stop:
                                    item.StateType.StateText= MobileResource.Online_Label_StatusCarStoping;
                                    break;

                                case StateType.Loss:
                                    item.StateType.StateText= MobileResource.Online_Label_StatusCarLostGSM;
                                    break;

                                default:
                                    item.StateType.StateText= MobileResource.Online_Label_StatusCarMoving;
                                    break;
                            }
                        }

                    }
                    ListRoute=lst;
                }
            }
        }

        private void GetAddress(VehicleRoute route)
        {
            if (!string.IsNullOrWhiteSpace(route.Address))
                return;

            Task.Run(async () =>
            {
                return await geocodeService.GetAddressByLatLng(CurrentComanyID, route.Latitude.ToString(), route.Longitude.ToString());
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