﻿using BA_MobileGPS.Core.Events;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using Prism;
using Prism.Common;
using Prism.Events;
using Prism.Ioc;
using Prism.Navigation;
using System;
using System.Collections.Generic;
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
        private readonly IEventAggregator eventAggregator;
        private IList<TabbedPageChildrenEnum> ListPage { get; set; } = new List<TabbedPageChildrenEnum>();
        private bool IsLoadedPage { get; set; }

        public MainPage()
        {
            eventAggregator = PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
            InitializeComponent();
            if (Device.RuntimePlatform == Device.Android)
            {
                On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
                On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);
            }

            ListPage.Add(TabbedPageChildrenEnum.HomeTab);
            var home = new ContentPage()
            {
                IconImageSource = "ic_home.png",
                Title = MobileResource.Menu_TabItem_Home
            };
            Children.Add(home);
            var firstSelectedPage = home;

            if (CheckPermision((int)PermissionKeyNames.VehicleView))
            {
                ListPage.Add(TabbedPageChildrenEnum.ListVehicleTab);
                var listVehicleTab = new ContentPage()
                {
                    IconImageSource = "ic_vehicle.png",
                    Title = MobileResource.Menu_TabItem_Vehicle
                };
                Children.Add(listVehicleTab);
            }

            if (CheckPermision((int)PermissionKeyNames.ViewModuleOnline))
            {
                var online = new ContentPage()
                {
                    IconImageSource = "ic_mornitoring.png",
                    Title = MobileResource.Menu_TabItem_Monitoring
                };
                //cấu hình cty này dùng Cluster thì mới mở forms Cluster
                if (MobileUserSettingHelper.EnableShowCluster)
                {
                    ListPage.Add(TabbedPageChildrenEnum.OnlineTab);
                }
                else
                {
                    ListPage.Add(TabbedPageChildrenEnum.OnlineTabNoCluster);
                }
                Children.Add(online);
                firstSelectedPage = online;
            }

            if (CheckPermision((int)PermissionKeyNames.ViewModuleRoute))
            {
                ListPage.Add(TabbedPageChildrenEnum.RouteTab);
                var routeTab = new ContentPage()
                {
                    IconImageSource = "ic_route.png",
                    Title = App.AppType == AppType.VMS ? MobileResource.Menu_TabItem_Voyage : MobileResource.Menu_TabItem_Route
                };
                Children.Add(routeTab);
            }

            ListPage.Add(TabbedPageChildrenEnum.AccountTab);
            var accountTab = new ContentPage()
            {
                IconImageSource = "ic_account.png",
                Title = MobileResource.Menu_TabItem_Account
            };
            Children.Add(accountTab);
            CurrentPage = firstSelectedPage;
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            var currenView = ((MainPageViewModel)BindingContext).currentChildView;
            if (IsLoadedPage)
            {
                using (new HUDService())
                {
                    var newPage = (ContentPage)CurrentPage;
                    var parameters = new NavigationParameters();
                    if (newPage?.Content == null) // => Load view
                    {
                        var currentIndex = GetIndex(CurrentPage);
                        var pageEnum = ListPage[currentIndex];
                        var viewResolve = PrismApplicationBase.Current.Container.Resolve<ContentView>(pageEnum.ToString());
                        newPage.Content = viewResolve;
                    }

                    //Raise Nanvigation while tab change
                    if (currenView != null)
                    {
                        PageUtilities.OnNavigatedFrom(currenView, parameters);
                    }

                    PageUtilities.OnNavigatedTo(newPage.Content, parameters);
                    ((MainPageViewModel)BindingContext).currentChildView = newPage.Content;
                }
            }
            else
            {
                IsLoadedPage = true;
            }
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