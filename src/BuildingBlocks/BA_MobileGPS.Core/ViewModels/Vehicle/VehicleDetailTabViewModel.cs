using Prism.Navigation;

namespace BA_MobileGPS.Core.ViewModels
{
    public class VehicleDetailTabViewModel : ViewModelBase
    {
        public VehicleDetailTabViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }
    }
}