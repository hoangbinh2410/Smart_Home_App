using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;

using Prism.Navigation;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ReportDetailTemperaturePageViewModel : ViewModelBase
    {
        public ReportDetailTemperaturePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = MobileResource.ReportTemperature_Label_TitlePage;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters?.GetValue<TemperatureVehicleResponse>(ParameterKey.ReportTemperatureSelected) is TemperatureVehicleResponse temperatureDetail)
            {
                temperatureDetail.ShowDateDetailSTR = DateTimeHelper.DisplayDateTimeVer2(temperatureDetail.StartTime, temperatureDetail.EndTime, Settings.CurrentLanguage);
                SelectTemperatureItem = temperatureDetail;
            }
        }

        #region property

        private TemperatureVehicleResponse _selectTemperature;

        public TemperatureVehicleResponse SelectTemperatureItem
        {
            get { return _selectTemperature; }
            set { SetProperty(ref _selectTemperature, value); }
        }

        #endregion property
    }
}