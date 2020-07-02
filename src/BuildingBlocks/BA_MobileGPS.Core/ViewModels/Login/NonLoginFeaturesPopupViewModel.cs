using BA_MobileGPS.Core.ViewModels.Base;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class NonLoginFeaturesPopupViewModel : ViewModelBase
    {
        public NonLoginFeaturesPopupViewModel(INavigationService navigationService) : base(navigationService)
        {
            ClosePopupCommand = new DelegateCommand(ClosePopup);
        }

        private async void ClosePopup()
        {
            await Task.Delay(300);
            await PopupNavigation.Instance.PopAsync();
        }

        public ICommand ClosePopupCommand { get; }
    }
}
