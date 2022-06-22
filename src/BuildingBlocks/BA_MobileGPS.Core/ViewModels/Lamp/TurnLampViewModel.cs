using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
   public class TurnLampViewModel : ViewModelBase
    {
        private ObservableCollection<MobileUserSetting> mobileUserSettings;
        public ObservableCollection<MobileUserSetting> MobileUserSettings { get => mobileUserSettings; set => SetProperty(ref mobileUserSettings, value); }
        public ICommand TurnLamp { get; }
        public TurnLampViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Hệ thống đèn";
            TurnLamp = new DelegateCommand(ClickSave);
            MobileUserSettings = new ObservableCollection<MobileUserSetting>
            {
                new MobileUserSetting
                {
                    Name = MobileUserConfigurationNames.EnableShowCluster.ToString(),
                    Display = "Đèn 1",
                    Value1 = false
                },
                new MobileUserSetting
                {
                    Name = MobileUserConfigurationNames.ShowNotification.ToString(),
                    Display = "Đèn2",
                    Value1 = false
                },
                  new MobileUserSetting
                {
                    Name = MobileUserConfigurationNames.UseViewAllCar.ToString(),
                    Display = "Đèn 2",
                    Value1 = false
                }
            };
        }

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);           
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

        private void ClickSave()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
            {
                { "TurnLamp", true }
            };
                await NavigationService.GoBackAsync(parameters, useModalNavigation: true, true);
            });
        }
    }
}