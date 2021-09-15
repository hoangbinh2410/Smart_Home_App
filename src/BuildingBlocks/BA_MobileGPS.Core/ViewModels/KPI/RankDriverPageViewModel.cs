using Prism.Navigation;

namespace BA_MobileGPS.Core.ViewModels
{
    public class RankDriverPageViewModel : ViewModelBase
    {
        public RankDriverPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Điểm xếp hạng lái xe";
        }
    }
}