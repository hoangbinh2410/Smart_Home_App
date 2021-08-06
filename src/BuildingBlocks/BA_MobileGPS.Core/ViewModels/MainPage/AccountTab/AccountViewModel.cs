using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace BA_MobileGPS.Core.ViewModels
{
    public class AccountViewModel : TabbedPageChildVMBase
    {
        private string appVersion;
        public string AppVersion { get => appVersion; set => SetProperty(ref appVersion, value); }

        private bool isShowPhoneNumber;
        public bool IsShowPhoneNumber { get => isShowPhoneNumber; set => SetProperty(ref isShowPhoneNumber, value); }

        private ObservableCollection<MenuItem> menuItems = new ObservableCollection<MenuItem>();
        public ObservableCollection<MenuItem> MenuItems { get => menuItems; set => SetProperty(ref menuItems, value); }

        public ICommand NavigateCommand { get; private set; }

        public AccountViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            NavigateCommand = new DelegateCommand<ItemTappedEventArgs>(Navigate);
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
            AppVersion = VersionTracking.CurrentVersion;
            IsShowPhoneNumber = MobileUserSettingHelper.IsShowPhoneNumber;
            InitMenuItems();
        }

        public override void Initialize(INavigationParameters parameters)
        {
        }

        private void InitMenuItems()
        {
            var list = new List<MenuItem>();

            // Thông báo
            list.Add(new MenuItem
            {
                Title = MobileResource.AccountTab_Label_Notification,
                Icon = "ic_mail.png",
                UseModalNavigation = true,
                Url = "NavigationPage/NotificationPage",
                MenuType = MenuType.Notification,
                IsEnable = true,
                IconColor = (Color)App.Current.Resources["PrimaryColor"]
            });
            // Đổi mật khẩu
            list.Add(new MenuItem
            {
                Title = MobileResource.AccountTab_Label_ChangePassword,
                Icon = "ic_changepassword.png",
                UseModalNavigation = true,
                Url = "BaseNavigationPage/ChangePasswordPage",
                MenuType = MenuType.ChangePassword,
                IsEnable = !CheckPermision((int)PermissionKeyNames.ChangePassword),
                IconColor = Color.FromHex("#795548")
            });
            // Phản hồi thông tin khách hàng
            list.Add(new MenuItem
            {
                Title = MobileResource.Issue_Label_TilePage,
                Icon = "ic_customersupport.png",
                UseModalNavigation = true,
                Url = "NavigationPage/ListIssuePage",
                MenuType = MenuType.DeviceManual,
                IsEnable = true,
                IconColor = Color.FromHex("#FF9900")
            });
            // Hướng dẫn sử dụng
            list.Add(new MenuItem
            {
                Title = MobileResource.AccountTab_Label_DeviceManual,
                Icon = "ic_devicemanual.png",
                UseModalNavigation = true,
                Url = "NavigationPage/HelperPage",
                MenuType = MenuType.DeviceManual,
                IsEnable = MobileSettingHelper.UseHelper,
                IconColor = Color.FromHex("#FF9900")
            });
            // Giới thiệu BAGPS
            list.Add(new MenuItem
            {
                Title = MobileResource.AccountTab_Label_BAGPS_Introduce,
                Icon = "ic_shortlogo.png",
                UseModalNavigation = true,
                Url = MobileSettingHelper.WebGps,
                MenuType = MenuType.BAGPSIntro,
                IsEnable = MobileSettingHelper.IsUseBAGPSIntroduce,
                IconColor = Color.FromHex("#0A46B1")
            });
            // Chia sẻ
            list.Add(new MenuItem
            {
                Title = MobileResource.AccountTab_Label_Share,
                Icon = "ic_sharecircle.png",
                UseModalNavigation = true,
                Url = MobileSettingHelper.LinkShareApp,
                MenuType = MenuType.Share,
                IsEnable = true,
                IconColor = Color.FromHex("#8BC34A")
            });
            // Đánh giá
            list.Add(new MenuItem
            {
                Title = MobileResource.AccountTab_Label_Rating,
                Icon = "ic_ratingstar.png",
                UseModalNavigation = true,
                Url = Device.RuntimePlatform == Device.Android ? MobileSettingHelper.LinkCHPlay : MobileSettingHelper.LinkAppStore,
                MenuType = MenuType.Rating,
                IsEnable = true,
                IconColor = Color.FromHex("#F9CF26")
            });
            // Cài đặt
            list.Add(new MenuItem
            {
                Title = MobileResource.AccountTab_Label_Setting,
                Icon = "ic_settings.png",
                UseModalNavigation = true,
                Url = "NavigationPage/SettingsPage",
                MenuType = MenuType.Setting,
                IsEnable = true,
                IconColor = Color.FromHex("#673AB7")
            });
            // Nâng cấp phiên bản
            list.Add(new MenuItem
            {
                Title = string.Format(MobileResource.AccountTab_Label_Upgrade, Settings.AppVersionDB),
                Icon = "ic_upgrade.png",
                UseModalNavigation = true,
                Url = Settings.AppLinkDownload,
                MenuType = MenuType.UpgradeVersion,
                IsEnable = Settings.AppVersionDB != AppVersion ? true : false,
                IconColor = (Color)App.Current.Resources["PrimaryColor"]
            });
            // Đăng xuất
            list.Add(new MenuItem
            {
                Title = MobileResource.AccountTab_Label_Logout,
                Icon = "ic_logout.png",
                UseModalNavigation = false,
                Url = "/LoginPage",
                MenuType = MenuType.Logout,
                IsEnable = true,
                IconColor = Color.FromHex("#E63C2B")
            });

            MenuItems = list.Where(x => x.IsEnable == true).ToObservableCollection();
        }

        public async void Navigate(ItemTappedEventArgs args)
        {
            try
            {
                if (!(args.ItemData is MenuItem item) || string.IsNullOrEmpty(item.Url))
                    return;

                if (item.MenuType == MenuType.Logout)
                {
                    var action = await PageDialog.DisplayAlertAsync(MobileResource.Common_Message_Warning, MobileResource.AccountTab_Label_MessageWarningLogout,
                        MobileResource.Common_Button_Yes, MobileResource.Common_Button_No);
                    if (action)
                    {
                        Logout();
                    }
                }
                else
                {
                    switch (item.MenuType)
                    {
                        case MenuType.ChangePassword:
                            await NavigationService.NavigateAsync(item.Url, null, useModalNavigation: item.UseModalNavigation, item.UseModalNavigation);
                            break;

                        case MenuType.CustomerSupport:
                            await Launcher.OpenAsync(new Uri(item.Url));
                            break;

                        case MenuType.BAGPSIntro:
                            await Launcher.OpenAsync(new Uri(item.Url));
                            break;

                        case MenuType.Share:
                            await Launcher.OpenAsync(new Uri(item.Url));
                            break;

                        case MenuType.Rating:
                            await Launcher.OpenAsync(new Uri(item.Url));
                            break;

                        case MenuType.UpgradeVersion:
                            await Launcher.OpenAsync(new Uri(item.Url));
                            break;

                        default:
                            await NavigationService.NavigateAsync(item.Url, null, useModalNavigation: item.UseModalNavigation, item.UseModalNavigation);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public ICommand PushToProfileCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (IsBusy)
                    {
                        return;
                    }
                    IsBusy = true;
                    try
                    {
                        await NavigationService.NavigateAsync("BaseNavigationPage/UserInfoPage", null, useModalNavigation: true, true);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                });
            }
        }
    }

    public enum MenuType
    {
        Notification,
        ChangePassword,
        CustomerSupport,
        DeviceManual,
        BAGPSIntro,
        Share,
        Rating,
        Setting,
        UpgradeVersion,
        Logout,
        Route,
        VehicleDetail,
        Images,
        Video
    }

    public class MenuItem
    {
        public string Title { get; set; }

        public string Icon { get; set; }

        public string Url { get; set; }

        public bool UseModalNavigation { get; set; }

        public MenuType MenuType { get; set; }

        public Color IconColor { get; set; }

        public bool IsEnable { get; set; }
    }
}