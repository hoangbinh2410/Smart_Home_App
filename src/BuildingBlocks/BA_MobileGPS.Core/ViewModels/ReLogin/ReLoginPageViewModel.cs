using Prism.Navigation;

using System.Threading.Tasks;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ReLoginPageViewModel : ViewModelBaseLogin
    {
        public ReLoginPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}