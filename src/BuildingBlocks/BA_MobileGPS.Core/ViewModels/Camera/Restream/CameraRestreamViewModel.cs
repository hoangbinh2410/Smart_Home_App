using BA_MobileGPS.Core.Interfaces;
using Prism.Navigation;

namespace BA_MobileGPS.Core.ViewModels
{
    public class CameraRestreamViewModel : ViewModelBase
    {
        private readonly IScreenOrientServices _screenOrientServices;

        public CameraRestreamViewModel(INavigationService navigationService, IScreenOrientServices screenOrientServices) : base(navigationService)
        {
            _screenOrientServices = screenOrientServices;
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _screenOrientServices.ForcePortrait();
        }
    }
}