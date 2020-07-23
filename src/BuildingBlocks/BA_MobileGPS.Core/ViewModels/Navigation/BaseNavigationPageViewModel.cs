using Prism.Navigation;

namespace BA_MobileGPS.Core.ViewModels
{
    public class BaseNavigationPageViewModel : ViewModelBase
    {
        public BaseNavigationPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}