using BA_MobileGPS.Core.Controls;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using Prism;
using Prism.Common;
using Prism.Events;
using Prism.Ioc;
using Prism.Navigation;
using System;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace BA_MobileGPS.Core.Views
{
    public partial class MainPage : TabbedPageEx
    {
        private readonly IEventAggregator eventAggregator;
        private Xamarin.Forms.Page currentChildPage;
        private bool isLoaded { get; set; } // Bỏ first auto-select của tabbed page

        public MainPage()
        {
            InitializeComponent();
            eventAggregator = PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
            this.eventAggregator.GetEvent<ShowHideTabEvent>().Subscribe(ShowHideTab);
            if (Device.RuntimePlatform == Device.Android)
            {
                On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
                On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);
                On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSmoothScrollEnabled(false);
            }
            else
            {
                On<iOS>().SetUseSafeArea(true);
            }

            ContentPage selected;

            var home = new Home()
            {
                IconImageSource = "ic_home.png",
                Title = MobileResource.Menu_TabItem_Home
            };
            Children.Add(home);
            selected = home;
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
                    selected = online;
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
            isLoaded = true;
            CurrentPage = selected;
        }

        private void ShowHideTab(bool obj)
        {
            if (obj)
            {
                this.IsHidden = false;
                isHideTabOnline = false;
            }
            else
            {
                this.IsHidden = true;
                isHideTabOnline = true;
            }
        }

        protected override void OnCurrentPageChanged()
        {
            if (isLoaded)
            {
                base.OnCurrentPageChanged();

                var parameters = new NavigationParameters();
                var newPage = (ContentPage)CurrentPage;

                if (currentChildPage != null)
                {
                    PageUtilities.OnNavigatedFrom(currentChildPage, parameters);
                    // Remove previous selected icon
                    if (currentChildPage.IconImageSource != null || !string.IsNullOrEmpty(currentChildPage.IconImageSource.ToString()))
                    {
                        var newPath = currentChildPage.IconImageSource.ToString().Replace("solid", string.Empty);
                        newPath = newPath.Replace("File:", string.Empty).Trim();
                        currentChildPage.IconImageSource = newPath;
                    }
                }

                PageUtilities.OnNavigatedTo(newPage, parameters);

                //Change current icon selected
                if (newPage.IconImageSource != null || !string.IsNullOrEmpty(newPage.IconImageSource.ToString()))
                {
                    var path = newPage.IconImageSource.ToString().Replace(".png", "solid.png");
                    path = path.Replace("File:", string.Empty).Trim();
                    newPage.IconImageSource = path;
                }

                currentChildPage = newPage;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //if (Device.RuntimePlatform == Device.iOS)
            //{
            //    var safe = On<iOS>().SafeAreaInsets();
            //    Padding = new Thickness(0, 0, 0, safe.Bottom);
            //}
        }

        public virtual bool CheckPermision(int PermissionKey)
        {
            return StaticSettings.User.Permissions.IndexOf(PermissionKey) != -1;
        }

        private bool bExit = false;
        private bool isHideTabOnline = false;

        protected override bool OnBackButtonPressed()
        {
            if (isHideTabOnline)
            {
                this.eventAggregator.GetEvent<BackButtonEvent>().Publish(true);
                return true;
            }
            else
            {
                if (!bExit)
                {
                    ShowAlertWhen2Back();
                    return bExit;
                }
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
                isHideTabOnline = false;
                bExit = false;
            }, 2000);
        }
    }
}