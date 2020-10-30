using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using Prism.Common;
using Prism.Navigation;
using System;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using TabbedPage = Xamarin.Forms.TabbedPage;

namespace BA_MobileGPS.Core.Views
{
    public partial class MainPage : TabbedPage
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
                var listVehicleTab = new ListVehiclePage()
                {
                    IconImageSource = "ic_vehicle.png",
                    Title = MobileResource.Menu_TabItem_Vehicle
                };
                Children.Add(listVehicleTab);
            }

            if (CheckPermision((int)PermissionKeyNames.ViewModuleOnline))
            {
                var online = new ContentPage();
                //cấu hình cty này dùng Cluster thì mới mở forms Cluster
                if (MobileUserSettingHelper.EnableShowCluster)
                {
                    online = new OnlinePage()
                    {
                        IconImageSource = "ic_mornitoring.png",
                        Title = MobileResource.Menu_TabItem_Monitoring
                    };
                }
                else
                {
                    online = new OnlinePageNoCluster()
                    {
                        IconImageSource = "ic_mornitoring.png",
                        Title = MobileResource.Menu_TabItem_Monitoring
                    };
                }
                Children.Add(online);
            }

            if (CheckPermision((int)PermissionKeyNames.ViewModuleRoute))
            {
                var routeTab = new RoutePage()
                {
                    IconImageSource = "ic_route.png",
                    Title = App.AppType == AppType.VMS ? MobileResource.Menu_TabItem_Voyage : MobileResource.Menu_TabItem_Route
                };
                Children.Add(routeTab);
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