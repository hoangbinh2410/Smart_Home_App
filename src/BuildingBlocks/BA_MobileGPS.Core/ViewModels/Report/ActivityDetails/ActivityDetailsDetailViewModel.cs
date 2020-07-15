using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;

using Prism.Navigation;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ActivityDetailsDetailViewModel : ReportBase<ActivityDetailsRequest, ActivityDetailsService, ActivityDetailsModel>
    {
        private ActivityDetailsModel selectedDetails;
        public ActivityDetailsModel SelectedDetails { get => selectedDetails; set => SetProperty(ref selectedDetails, value); }

        public ActivityDetailsDetailViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = MobileResource.DetailsReport_Label_TitlePageDetail;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(ParameterKey.ReportDetailsSelected, out ActivityDetailsModel DetailsDetail))
            {
                SelectedDetails = DetailsDetail;
            }
        }
    }
}