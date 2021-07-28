using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
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
        public ICommand PushToSettingThemeCommand { get; }

        public SettingsViewModel(INavigationService navigationService, IUserService userService) : base(navigationService)
        {
            this.userService = userService;

            SaveSettingsCommand = new Command(SaveSettings);
            PushToSettingMapCommand = new Command(PushToSettingMap);
            PushToSettingReceiveAlertCommand = new Command(PushToSettingReceiveAlert);
            PushToSettingThemeCommand = new DelegateCommand(PushToSettingTheme);
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
                },
                  new MobileUserSetting
                {
                    Name = MobileUserConfigurationNames.UseGPSDefaut.ToString(),
                    Display = MobileResource.Settings_Label_UseGPSDefault,
                    Value = MobileUserSettingHelper.UseGPSDefaut.ToString()
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
                        Logout();
                    }
                }
            }));
        }

        private void PushToSettingMap()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("MyLocationSettingPage", null, useModalNavigation: false, true);
            });
        }

        private void PushToSettingReceiveAlert()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("AlertConfigSettingPage", null, useModalNavigation: false, true);
            });
        }

        private void PushToSettingTheme()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("SettingThemePage", null, useModalNavigation: false, true);
            });
        }
    }
}