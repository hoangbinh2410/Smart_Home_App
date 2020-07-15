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