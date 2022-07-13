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
        
        private readonly IControlSmartHomeService _controllService;
        public ICommand TurnHeater { get; }
        public ICommand TurnoffHeater { get; }
        public TurnHeaterViewModel(INavigationService navigationService,
            IControlSmartHomeService controllService
        ) : base(navigationService)
        {            
            TurnHeater = new DelegateCommand(ClickTurnHeater);
            TurnoffHeater = new DelegateCommand(ClickTurnoffHeater);
            _controllService = controllService;
            
        }
        private int temple;
        public int Temple { get { return temple; } set { SetProperty(ref temple, value); } }
        private void ClickTurnHeater()
        {
            TryExecute(async () =>
            {
                AirControll data = new AirControll { data = temple };
                var result = await _controllService.ControlAir(data);
                var respone = await _controllService.ControlHome(9);             
                if (respone)
                {
                    var parameters = new NavigationParameters
            {
                { "TurnHeater", true }
            };
                    await NavigationService.GoBackAsync(parameters, useModalNavigation: true, true);
                }
                else
                {
                  
                }
                
            });
        }
        private void ClickTurnoffHeater()
        {
            TryExecute(async () =>
            {
                var respone = await _controllService.ControlHome(9);
                if (!respone)
                {
                    var parameters = new NavigationParameters
            {
                { "TurnHeater", false }
            };
                    await NavigationService.GoBackAsync(parameters, useModalNavigation: true, true);
                }
                else
                {

                }
            });
        }
    }
}
