using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;

using Prism.Navigation;

namespace BA_MobileGPS.Core.ViewModels
{
    public class StopParkingVehicleDetailViewModel : ReportBase<StopParkingVehicleRequest, StopsParkingVehicleService, StopParkingVehicleModel>
    {
        private StopParkingVehicleModel selectedStopParkingVehicle;
        public StopParkingVehicleModel SelectedStopParkingVehicle { get => selectedStopParkingVehicle; set => SetProperty(ref selectedStopParkingVehicle, value); }

        public StopParkingVehicleDetailViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = MobileResource.StopParkingReport_Label_TitlePageDetail;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(ParameterKey.ReportStopParkingVehicleSelected, out StopParkingVehicleModel StopParkingVehicleDetail))
            {
                SelectedStopParkingVehicle = StopParkingVehicleDetail;
            }
        }
    }
}