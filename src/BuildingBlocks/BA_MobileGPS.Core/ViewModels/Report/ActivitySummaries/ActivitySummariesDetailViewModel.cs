using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;

using Prism.Navigation;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ActivitySummariesDetailViewModel : ReportBase<ActivitySummariesRequest, ActivitySummariesService, ActivitySummariesModel>
    {
        private ActivitySummariesModel selectedActivitySummaries;
        public ActivitySummariesModel SelectedActivitySummaries { get => selectedActivitySummaries; set => SetProperty(ref selectedActivitySummaries, value); }

        public ActivitySummariesDetailViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = MobileResource.ActivitySummariesReport_Label_TitlePageDetail;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(ParameterKey.ReportActivitySummariesSelected, out ActivitySummariesModel ActivitySummariesDetail))
            {
                SelectedActivitySummaries = ActivitySummariesDetail;
            }
        }
    }
}