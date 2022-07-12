using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Service.IService;
using Prism.Commands;
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
        private readonly IDisplayMessage _displayMessage;
        private readonly IControlSmartHomeService _controllService;
        public ICommand TurnHeater { get; }
        public ICommand TurnoffHeater { get; }
        public TurnHeaterViewModel(INavigationService navigationService,
            IControlSmartHomeService controllService, 
            IDisplayMessage displayMessage) : base(navigationService)
        {            
            TurnHeater = new DelegateCommand(ClickTurnHeater);
            TurnoffHeater = new DelegateCommand(ClickTurnoffHeater);
            _controllService = controllService;
            _displayMessage = displayMessage;
        }
        private int temple;
        public int Temple { get { return temple; } set { SetProperty(ref temple, value); } }
        private void ClickTurnHeater()
        {
            SafeExecute(async () =>
            {
                AirControll data = new AirControll { data = temple };
                var result = await _controllService.ControlAir(data);
                if (result)
                {
                    var parameters = new NavigationParameters
            {
                { "TurnHeater", true }
            };
                    await NavigationService.GoBackAsync(parameters, useModalNavigation: true, true);
                }
                else
                {
                    _displayMessage.ShowMessageWarning("Lỗi");
                }
                
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
