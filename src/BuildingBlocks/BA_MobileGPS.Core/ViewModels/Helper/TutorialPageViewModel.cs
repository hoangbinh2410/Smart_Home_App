using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;

using Prism.Navigation;

using Syncfusion.Data.Extensions;

using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class TutorialPageViewModel : ViewModelBase
    {
        private readonly IGuideService guideService;

        public TutorialPageViewModel(INavigationService navigationService, IGuideService guideService)
            : base(navigationService)
        {
            this.guideService = guideService;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            if (parameters.ContainsKey(ParameterKey.HelperKey) && parameters.GetValue<GuideType>(ParameterKey.HelperKey) is GuideType type)
            {
                GetGuide((int)type);
            }
        }

        public override void OnPageAppearingFirstTime()
        {
        }

        private int position = -1;
        public int Position { get => position; set => SetProperty(ref position, value); }

        private ObservableCollection<string> pageCarouselData = new ObservableCollection<string>();
        public ObservableCollection<string> PageCarouselData { get => pageCarouselData; set => SetProperty(ref pageCarouselData, value); }

        private void GetGuide(int type)
        {
            Task.Run(async () =>
            {
                return await guideService.GetGuide(type);
            }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    PageCarouselData = task.Result.ToObservableCollection();
                }
            }));
        }
    }
}