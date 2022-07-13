using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Service.IService;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class TurnLampViewModel : ViewModelBase
    {
       
        private readonly IControlSmartHomeService _controllService;
        private ObservableCollection<MobileUserSetting> mobileUserSettings;
        public ObservableCollection<MobileUserSetting> MobileUserSettings { get => mobileUserSettings; set => SetProperty(ref mobileUserSettings, value); }
        public ICommand TurnLamp { get; }

        public TurnLampViewModel(INavigationService navigationService,
            IControlSmartHomeService controllService) : base(navigationService)
        {
            Title = "Hệ thống đèn";
            TurnLamp = new DelegateCommand(ClickSave);
            _controllService = controllService;
           
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
                if (parameters.ContainsKey("Lamp") && parameters.GetValue<List<Light>>("Lamp") is List<Light> obj)
                {
                    Lamp = obj;
                   
                }
                CheckValueLamp(Lamp);
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
        }

        #endregion Lifecycle

        private int valueLamp;
        public int ValueLamp
        { get { return valueLamp; } set { SetProperty(ref valueLamp, value); } }
        private bool turnLamp1 = false;
        public bool TurnLamp1
        { get { return turnLamp1; } set { SetProperty(ref turnLamp1, value); } }
        private bool turnLamp2 = false;
        public bool TurnLamp2
        { get { return turnLamp2; } set { SetProperty(ref turnLamp2, value); } }
        private bool turnLamp3 = false;
        public bool TurnLamp3
        { get { return turnLamp3; } set { SetProperty(ref turnLamp3, value); } }
        public List<Light> Lamp = new List<Light>();

        private void ClickSave()
        {
            SafeExecute(async () =>
            {
                Data1 Value = new Data1 { power = TurnLamp3, config = ValueLamp };
                foreach (var item in Lamp)
                {
                    switch (item.id)
                    {
                        case 8:
                            item.data = new Data1 { power = TurnLamp2 };
                            break;

                        case 9:
                            item.data.power = TurnLamp3;
                            item.data.config = ValueLamp;
                            break;

                        case 10:
                            item.data = new Data1 { power = TurnLamp1 };
                            break;
                    }
                    List<Light> Request = Lamp;
                    var result = await _controllService.ControlLight(Request);
                    if (result)
                    {
                        var parameters = new NavigationParameters
            {
                { "TurnLamp", true }
            };
                        await NavigationService.GoBackAsync(parameters, useModalNavigation: true, true);
                    }
                    else
                    {
                       
                    }
                }
            });
        }

        private void CheckValueLamp(List<Light> list)
        {
            foreach (var item in list)
            {
                switch (item.id)
                {
                    case 8:
                        TurnLamp2 = item.data.power;
                        break;

                    case 9:
                        TurnLamp3 = item.data.power;
                        ValueLamp = item.data.config;
                        break;

                    case 10:
                        TurnLamp1 = item.data.power;
                        break;
                }
            };
        }
    }
}