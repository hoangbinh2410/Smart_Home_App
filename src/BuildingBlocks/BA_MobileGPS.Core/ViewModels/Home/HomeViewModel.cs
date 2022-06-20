using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels.Home
{
   public class HomeViewModel : ViewModelBase
    {
        public ICommand ClickHeater { get; private set; }
        public HomeViewModel(INavigationService navigationService) : base(navigationService)
        {
            ClickHeater = new DelegateCommand(ClickHeaterPage);
        } 
        private void ClickHeaterPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("TurnHeaterView", null, useModalNavigation: true, true);
            });
        }
    }
}
