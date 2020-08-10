using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using Prism.Navigation;

namespace BA_MobileGPS.Core.ViewModels
{
    public class SignalLossDetailViewModel : ReportBase<SignalLossRequest, SignalLossServices, SignalLossResponse>
    {
        private SignalLossResponse selectedSignalLoss;
        public SignalLossResponse SelectedSignalLoss { get => selectedSignalLoss; set => SetProperty(ref selectedSignalLoss, value); }

        public SignalLossDetailViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = MobileResource.ReportSignalLoss_Label_TitlePageDetail;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(ParameterKey.ReportSignalLossSelected, out SignalLossResponse SignalLossDetail))
            {
                SelectedSignalLoss = SignalLossDetail;
            }
        }
    }
}