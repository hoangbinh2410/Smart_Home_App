using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Reflection;

namespace BA_MobileGPS.Core.ViewModels
{
    public class PourFuelDetailViewModel : ReportBase<FuelReportRequest, PourFuelService, FuelVehicleModel>
    {
        private FuelVehicleModel selectedPourFuel;
        public FuelVehicleModel SelectedPourFuel { get => selectedPourFuel; set => SetProperty(ref selectedPourFuel, value); }

        public PourFuelDetailViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = MobileResource.PourFuelReport_Label_TileDetailPage;

            try
            {
                //// khải báo phần button và sự kiện
                ChartFuelCommand = new DelegateCommand(ExecuteChartFuel);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(ParameterKey.ReportPourFuelSelected, out FuelVehicleModel PourFuelDetail))
            {
                SelectedPourFuel = PourFuelDetail;
            }
        }

        #region Command

        public DelegateCommand ChartFuelCommand { get; private set; }

        #endregion Command

        private async void ExecuteChartFuel()
        {
            try
            {
                var p = new NavigationParameters
                {
                    { ParameterKey.ReportPourFuelSelected, SelectedPourFuel }
                };
                await NavigationService.NavigateAsync("NavigationPage/ChartFuelReportPage", p, useModalNavigation: true);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }
    }
}