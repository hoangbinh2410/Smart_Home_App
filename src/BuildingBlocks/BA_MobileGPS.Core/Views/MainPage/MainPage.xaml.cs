using BA_MobileGPS.Core.Controls;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using Prism;
using Prism.Common;
using Prism.Ioc;
using Prism.Navigation;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using TabbedPage = Xamarin.Forms.TabbedPage;

namespace BA_MobileGPS.Core.Views
{
    public partial class MainPage : TabbedPageEx
    {
        public MainPage()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.Android)
            {
                On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
                On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);
                On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSmoothScrollEnabled(false);
            }

            var home = new Home()
            {
                IconImageSource = "ic_home.png",
                Title = MobileResource.Menu_TabItem_Home
            };
            Children.Add(home);

            if (CheckPermision((int)PermissionKeyNames.VehicleView))
            {
                var listVehicleTab = PrismApplicationBase.Current.Container.Resolve<ContentPage>("ListVehiclePage"); //Online

                if (listVehicleTab != null)
                {
                    listVehicleTab.IconImageSource = "ic_vehicle.png";
                    listVehicleTab.Title = MobileResource.Menu_TabItem_Vehicle;
                    Children.Add(listVehicleTab);
                }
            }

            if (CheckPermision((int)PermissionKeyNames.ViewModuleOnline))
            {
                ContentPage online;
                //cấu hình cty này dùng Cluster thì mới mở forms Cluster
                if (MobileUserSettingHelper.EnableShowCluster)
                {
                    online = PrismApplicationBase.Current.Container.Resolve<ContentPage>("OnlinePage"); //Online
                }
                else
                {
                    online = PrismApplicationBase.Current.Container.Resolve<ContentPage>("OnlinePageNoCluster"); //Online
                }
                if (online != null)
                {
                    online.IconImageSource = "ic_mornitoring.png";
                    online.Title = MobileResource.Menu_TabItem_Monitoring;
                    Children.Add(online);
                }
            }

            if (CheckPermision((int)PermissionKeyNames.ViewModuleRoute))
            {
                var routeTab = PrismApplicationBase.Current.Container.Resolve<ContentPage>("RoutePage"); //Online

                if (routeTab != null)
                {
                    routeTab.IconImageSource = "ic_route.png";
                    routeTab.Title = App.AppType == AppType.VMS ? MobileResource.Menu_TabItem_Voyage : MobileResource.Menu_TabItem_Route;
                    Children.Add(routeTab);
                }
            }

            var accountTab = new Account()
            {
                IconImageSource = "ic_account.png",
                Title = MobileResource.Menu_TabItem_Account
            };
            Children.Add(accountTab);
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            var context = ((MainPageViewModel)BindingContext);
            var parameters = new NavigationParameters();
            var newPage = (ContentPage)CurrentPage;
            var previousPage = context.currentChildPage;
            if (previousPage != null)
            {
                PageUtilities.OnNavigatedFrom(previousPage, parameters);
            }

            PageUtilities.OnNavigatedTo(newPage, parameters);
            context.currentChildPage = newPage;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Device.RuntimePlatform == Device.iOS)
            {
                var safe = On<iOS>().SafeAreaInsets();
                Padding = new Thickness(0, 0, 0, safe.Bottom);
            }
            Task.Run(async() =>
            {
               await Task.Delay(8000);
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsHidden = true;
                });
               await Task.Delay(8000);
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsHidden = false;
                });
            });
        }

        public virtual bool CheckPermision(int PermissionKey)
        {
            return StaticSettings.User.Permissions.IndexOf(PermissionKey) != -1;
        }

        private bool bExit = false;

        protected override bool OnBackButtonPressed()
        {
            if (!bExit)
            {
                ShowAlertWhen2Back();
                return bExit;
            }
            StaticSettings.ClearStaticSettings();
            return false;
        }

        /// <summary>
        /// Thực hiện delay giá trị
        /// </summary>
        /// <param name="timers"></param>
        /// <param name="action"></param>
        /// <param name="timeDelay">đơn vị miliseconds</param>
        public Timer PostDelayed(Timer timers, Action action, int timeDelay)
        {
            timers.Interval = timeDelay;
            timers.AutoReset = false; //so that it only calls the method once
            timers.Elapsed += (s, args) =>
            {
                timers.Close();
                MainThread.BeginInvokeOnMainThread(() => { action?.Invoke(); });
            };
            timers.Start();
            return timers;
        }

        private void ShowAlertWhen2Back()
        {
            DependencyService.Get<IDisplayMessage>().ShowToast("Back thêm lần nữa để thoát ứng dụng");
            bExit = !bExit;
            PostDelayed(new Timer(), () =>
            {
                bExit = false;
            }, 2000);
        }
    }
}