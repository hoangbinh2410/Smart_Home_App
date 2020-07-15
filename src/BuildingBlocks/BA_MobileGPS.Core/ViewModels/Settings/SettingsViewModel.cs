﻿using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Navigation;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IUserService userService;

        private ObservableCollection<MobileUserSetting> mobileUserSettings;
        public ObservableCollection<MobileUserSetting> MobileUserSettings { get => mobileUserSettings; set => SetProperty(ref mobileUserSettings, value); }

        public ICommand SaveSettingsCommand { get; private set; }
        public ICommand PushToSettingMapCommand { get; private set; }
        public ICommand PushToSettingReceiveAlertCommand { get; private set; }

        public SettingsViewModel(INavigationService navigationService, IUserService userService) : base(navigationService)
        {
            this.userService = userService;

            SaveSettingsCommand = new Command(SaveSettings);
            PushToSettingMapCommand = new Command(PushToSettingMap);
            PushToSettingReceiveAlertCommand = new Command(PushToSettingReceiveAlert);
        }

        public override void OnPageAppearingFirstTime()
        {
            MobileUserSettings = new ObservableCollection<MobileUserSetting>
            {
                new MobileUserSetting
                {
                    Name = MobileUserConfigurationNames.EnableShowCluster.ToString(),
                    Display = MobileResource.Settings_Label_Enable_Cluster,
                    Value = MobileUserSettingHelper.EnableShowCluster.ToString()
                },
                new MobileUserSetting
                {
                    Name = MobileUserConfigurationNames.ShowNotification.ToString(),
                    Display = MobileResource.Settings_Label_Show_Notification,
                    Value = MobileUserSettingHelper.ShowNotification.ToString()
                }
            };

            Init();
        }

        private void Init()
        {
            foreach (var item in MobileUserSettings)
            {
                item.PropertyChanged += Item_PropertyChanged;
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(MobileUserSetting.Value)))
            {
                (sender as MobileUserSetting).IsChanged = true;
            }
        }

        public override void OnDestroy()
        {
            if (MobileUserSettings.Any(s => s.IsChanged))
            {
                SaveSettings();
            }
        }

        private void SaveSettings()
        {
            foreach (var item in MobileUserSettings.ToList().FindAll(s => s.IsChanged))
            {
                MobileUserSettingHelper.Set(item.Name, item.Value);
            }

            DependencyService.Get<IHUDProvider>().DisplayProgress("");

            Task.Run(async () =>
            {
                return await userService.SetUserSettings(new UserSettingsRequest
                {
                    UserID = UserInfo.UserId,
                    ListSettings = MobileUserSettings.ToList().FindAll(s => s.IsChanged),
                    ExecutedByUser = UserInfo.UserId
                });
            }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                DependencyService.Get<IHUDProvider>().Dismiss();

                if (task.Status == TaskStatus.RanToCompletion)
                {
                    if (task.Result)
                    {
                    }
                }
            }));
        }

        private void PushToSettingMap()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("MyLocationSettingPage", useModalNavigation: false);
            });
        }

        private void PushToSettingReceiveAlert()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("AlertConfigSettingPage", useModalNavigation: false);
            });
        }
    }
}