using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class TurnHeaterViewModel : ViewModelBase
    {
        public ICommand DateSelectedCommand { get; }
        public ICommand TurnHeater { get; }
        public ICommand TurnoffHeater { get; }
        public TurnHeaterViewModel(INavigationService navigationService) : base(navigationService)
        {
            DateSelectedCommand = new Command<DateChangedEventArgs>(DateSelected);
            TurnHeater = new Command(ClickTurnHeater);
            TurnoffHeater = new Command(ClickTurnoffHeater);
        }
        private void DateSelected(DateChangedEventArgs args)
        {

        }
        private void ClickTurnHeater()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
            {
                { "TurnHeater", true }            
            };
                await NavigationService.GoBackAsync(parameters, useModalNavigation: true, true);
            });
        }
        private void ClickTurnoffHeater()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
            {
                { "TurnHeater", false }
            };
                await NavigationService.GoBackAsync(parameters, useModalNavigation: true, true);
            });
        }
    }
}
