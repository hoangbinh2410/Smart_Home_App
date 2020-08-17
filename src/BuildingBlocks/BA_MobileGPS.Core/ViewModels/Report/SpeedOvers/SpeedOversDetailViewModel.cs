using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;

using Prism.Navigation;

namespace BA_MobileGPS.Core.ViewModels
{
    public class SpeedOversDetailViewModel : ReportBase<SpeedOversRequest, SpeedOversService, SpeedOversModel>
    {
        private SpeedOversModel selectedSpeedOvers;
        public SpeedOversModel SelectedSpeedOvers { get => selectedSpeedOvers; set => SetProperty(ref selectedSpeedOvers, value); }

        public SpeedOversDetailViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = MobileResource.SpeedOversReport_Label_TitlePageDetail;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(ParameterKey.ReportSpeedOversSelected, out SpeedOversModel SpeedOversDetail))
            {
                SelectedSpeedOvers = SpeedOversDetail;
            }
        }
    }
}