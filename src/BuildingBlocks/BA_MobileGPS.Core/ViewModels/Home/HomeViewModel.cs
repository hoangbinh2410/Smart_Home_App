using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Service.IService;
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
        private readonly IGetStatusService _statusService;
        private readonly IControlSmartHomeService _controllService;
        public ICommand ClickHeater { get; private set; }
        public ICommand ClickLamp { get; private set; }
        public ICommand ClickAir { get; private set; }
        public ICommand ClickMainWindow { get; private set; }
        public ICommand ClickCurtains { get; private set; }
        public ICommand ClickWindow { get; private set; }
        public ICommand ClickGara { get; private set; }
        public HomeViewModel(INavigationService navigationService,
            IGetStatusService statusService,
            IControlSmartHomeService controllService) : base(navigationService)
        {
            _statusService = statusService;
            _controllService = controllService;
            ClickHeater = new DelegateCommand(ClickHeaterPage);
            ClickLamp = new DelegateCommand(ClickLampPage);
            ClickAir = new DelegateCommand(TurnAir1);
            ClickMainWindow = new DelegateCommand(TurnMainWindow);
            ClickCurtains = new DelegateCommand(TurnCurtain);
            ClickWindow = new DelegateCommand(TurnWindow1);
            ClickGara = new DelegateCommand(Turngara);
            EventAggregator.GetEvent<OnReloadVehicleOnline>().Subscribe(ReLoadStatus);
        }

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
                 
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.ContainsKey("TurnHeater") && parameters.GetValue<Boolean>("TurnHeater") is Boolean obj)
                {
                    TurnHeater = obj;
                }
            }
            if (parameters != null)
            {
                if (parameters.ContainsKey("TurnLamp") && parameters.GetValue<Boolean>("TurnLamp") is Boolean obj)
                {
                    TurnLamp = obj;
                }
            }
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnDestroy()
        {
            EventAggregator.GetEvent<OnReloadVehicleOnline>().Unsubscribe(ReLoadStatus) ;
        }

        #endregion Lifecycle

        //Nhiệt độ
        private int temple;
        public int Temple { get { return temple; } set { SetProperty(ref temple, value); } }
        //ĐỘ ẩm
        private int humidity;
        public int Humidity { get { return humidity; } set { SetProperty(ref humidity, value); } }

        // bật cửa chính
        private bool turnWindow = false;
        public bool TurnWindow { get { return turnWindow; } set { SetProperty(ref turnWindow, value); } }

        // bật cửa chính
        private bool turnGara = false;
        public bool TurnGara { get { return turnGara; } set { SetProperty(ref turnGara, value); } }
        // Bật rèm
        private bool turnCurtains = false;
        public bool TurnCurtains { get { return turnCurtains; } set { SetProperty(ref turnCurtains, value); } }
        //Cửa chính
        private bool turnMaindoor = false;
        public bool TurnMaindoor { get { return turnMaindoor; } set { SetProperty(ref turnMaindoor, value); } }
        // Điều hòa
        private bool turnAir = false;
        public bool TurnAir { get { return turnAir; } set { SetProperty(ref turnAir, value); } }
        //nóng lạnh
        private bool turnHeater = false;
        public bool TurnHeater { get { return turnHeater; } set { SetProperty(ref turnHeater, value); } }
        // đèn
        private bool turnLamp = false;
        public bool TurnLamp { get { return turnLamp; } set { SetProperty(ref turnLamp, value); } }
        private void ClickHeaterPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("TurnHeaterView", null, useModalNavigation: true, true);
            });
        }
        private void ClickLampPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("NavigationPage/TurnLampView", null, useModalNavigation: true, true);
            });
        }
        private void TurnAir1()
        {
            SafeExecute(async () =>
            {
                var respone = await _controllService.ControlHome(1);
                if (respone)
                {
                    TurnGara = true;
                }
                else
                {
                    TurnGara = false;
                }
            });
        }
        private void TurnMainWindow()
        {
            SafeExecute(async () =>
            {
                var respone = await _controllService.ControlHome(1);
                if (respone)
                {
                    TurnGara = true;
                }
                else
                {
                    TurnGara = false;
                }
            });
        }

        private void TurnCurtain()
        {
            SafeExecute(async () =>
            {
                var respone = await _controllService.ControlHome(1);
                if (respone)
                {
                    TurnGara = true;
                }
                else
                {
                    TurnGara = false;
                }
            });
        }
        private void TurnWindow1()
        {
            SafeExecute(async () =>
            {               
                var respone = await _controllService.ControlHome(1);
                if (respone)
                {
                    TurnWindow = true;
                }
                else
                {
                    TurnWindow = false;
                }
            });
        }
        private void Turngara()
        {
            SafeExecute(async () =>
            {
                var respone = await _controllService.ControlHome(1);
                if (respone)
                {
                    TurnGara = true;
                }
                else
                {
                    TurnGara = false;
                }
            });
        }
        private void Getstatus()
        {
            SafeExecute(async () =>
            {
                List<StastusSmartHomeResponse> result = new List<StastusSmartHomeResponse>();
                var respone = await _statusService.Getall();
                if(respone!= null)
                {
                    result = respone;
                }                       
            });
        }
        private void ReLoadStatus(bool arg)
        {
            if (arg)
            {
                Getstatus();
            }
        }
    }
}
