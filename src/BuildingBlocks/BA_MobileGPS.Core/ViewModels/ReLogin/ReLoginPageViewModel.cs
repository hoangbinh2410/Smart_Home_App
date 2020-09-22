using Prism.Navigation;

using System.Threading.Tasks;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ReLoginPageViewModel : ViewModelBase
    {
        public ReLoginPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Device.BeginInvokeOnMainThread(async () =>
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    await Task.Delay(700);
                }
                else
                {
                    await Task.Delay(500);
                }
                await NavigationService.NavigateAsync("/LoginPage");
            });
        }
    }
}