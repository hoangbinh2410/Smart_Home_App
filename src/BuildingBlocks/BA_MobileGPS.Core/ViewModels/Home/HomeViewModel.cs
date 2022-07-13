using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Service.IService;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        public ICommand ClickFan { get; private set; }
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
            ClickFan = new DelegateCommand(Turnfan);
            Timeupdate = new Timer(10000);
            Timeupdate.Elapsed += new ElapsedEventHandler(ReLoadStatus);
            Timeupdate.AutoReset = true;
            Timeupdate.Enabled = true;
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
                    TurnAir = obj;
                }
            }
            if (parameters != null)
            {
                if (parameters.ContainsKey("TurnLamp") && parameters.GetValue<Boolean>("TurnLamp") is Boolean obj)
                {
                    TurnLamp = obj;
                }
            }
            Getstatus();
               
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
            
        }

        #endregion Lifecycle
        private static Timer Timeupdate;
        public List<Light> lamp = new List<Light>();
        //Nhiệt độ
        private string temple;
        public string Temple { get { return temple; } set { SetProperty(ref temple, value); } }//Nhiệt độ
        private string rain;
        public string Rain { get { return rain; } set { SetProperty(ref rain, value); } }
        //ĐỘ ẩm
        private string humidity;
        public string Humidity { get { return humidity; } set { SetProperty(ref humidity, value); } }

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
        public bool TurnLamp { get { return turnLamp; } set { SetProperty(ref turnLamp, value); } }// đèn
        private bool turnFan = false;
        public bool TurnFan { get { return turnFan; } set { SetProperty(ref turnFan, value); } }
        private void ClickHeaterPage()
        {
            TryExecute(async () =>
            {
                var respone = await _controllService.ControlHome(5);
                if (respone)
                {
                  TurnHeater = true;
                }
                else
                {
                    TurnHeater = false;
                }               
            });
        }
        private void ClickLampPage()
        {
            TryExecute(async () =>
            {
                var parameters = new NavigationParameters
            {
                { "Lamp", lamp }
        
            };
                await NavigationService.NavigateAsync("NavigationPage/TurnLampView", parameters, useModalNavigation: true, true);
            });
        }
        private void TurnAir1()
        {
            TryExecute(async () =>
            {
                await NavigationService.NavigateAsync("TurnHeaterView", null, useModalNavigation: true, true);
            });
        }
        private void TurnMainWindow()
        {
            TryExecute(async () =>
            {
                var respone = await _controllService.ControlHome(1);
                if (respone)
                {
                    TurnMaindoor = true;
                }
                else
                {
                    TurnMaindoor = false;
                }
            });
        }
        private void Turnfan()
        {
            TryExecute(async () =>
            {
                var respone = await _controllService.ControlHome(11);
                if (respone)
                {
                    TurnFan = true;
                }
                else
                {
                    TurnFan = false;
                }
            });
        }

        private void TurnCurtain()
        {
            TryExecute(async () =>
            {
                var respone = await _controllService.ControlHome(4);
                if (respone)
                {
                    TurnCurtains = true;
                }
                else
                {
                    TurnCurtains = false;
                }
            });
        }
        private void TurnWindow1()
        {
            TryExecute(async () =>
            {               
                var respone = await _controllService.ControlHome(3);
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
            TryExecute(async () =>
            {
                var respone = await _controllService.ControlHome(2);
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
                StastusSmartHomeResponse result = new StastusSmartHomeResponse();
                var respone = await _statusService.Getall();
                if(respone!= null)
                {
                    result = respone;
                }  
                foreach( var item in result.switches)
                {
                    switch (item.id)
                    {
                        case 1:
                            TurnMaindoor = item.data;
                            break;
                        case 2:
                            TurnGara = item.data;
                            break;
                        case 3:
                            TurnWindow = item.data;
                            break;
                        case 4:
                            TurnCurtains = item.data;
                            break;
                        case 5:
                            TurnHeater = item.data;
                            break;
                        case 11:
                            TurnWindow = item.data;
                            break;
                        case 12:
                            TurnFan = item.data;
                            break;
                    }          
                };
                foreach (var item in result.sensors)
                {
                    if(item.code_name == "TEMP_SENSOR")
                    {
                        Temple = item.data;
                    }else if(item.code_name == "HUM_SENSOR")
                    {
                        Humidity = item.data;
                    }
                    else
                    {
                        Rain = item.data;
                    }
                }
                lamp = result.lights;

            });
        }
        private void ReLoadStatus(object sender ,ElapsedEventArgs e)
        {
                Getstatus();        
        }
    }
}
