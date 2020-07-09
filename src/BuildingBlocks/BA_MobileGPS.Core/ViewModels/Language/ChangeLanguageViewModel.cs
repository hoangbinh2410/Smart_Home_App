using Prism.Navigation;

using System.Threading.Tasks;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ChangeLanguageViewModel : ViewModelBase
    {
        public ChangeLanguageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Device.BeginInvokeOnMainThread(async () =>
            {
                //DisplayMessage.ShowMessageInfo("Chang language");
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