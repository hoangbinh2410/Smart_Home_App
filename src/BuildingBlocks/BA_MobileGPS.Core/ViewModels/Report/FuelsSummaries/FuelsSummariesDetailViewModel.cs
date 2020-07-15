using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Reflection;

namespace BA_MobileGPS.Core.ViewModels
{
    public class FuelsSummariesDetailViewModel : ReportBase<FuelsSummariesRequest, FuelsSummariesService, FuelsSummariesModel>
    {
        private FuelsSummariesModel selectedFuelsSummaries;
        public FuelsSummariesModel SelectedFuelsSummaries { get => selectedFuelsSummaries; set => SetProperty(ref selectedFuelsSummaries, value); }

        public FuelsSummariesDetailViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = MobileResource.FuelsSummariesReport_Label_TitlePageDetail;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(ParameterKey.ReportFuelsSummariesSelected, out FuelsSummariesModel FuelsSummariesDetail))
            {
                SelectedFuelsSummaries = FuelsSummariesDetail;
            }
        }

    }
}