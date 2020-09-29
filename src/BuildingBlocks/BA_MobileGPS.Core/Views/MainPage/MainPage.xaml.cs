using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Events;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using Prism;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Sharpnado.Presentation.Forms.CustomViews.Tabs;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace BA_MobileGPS.Core.Views
{
    public partial class MainPage : ContentPage
    {
        private bool checkpermissiononline = false;
        public MainPage()
        {
            InitializeComponent();
            var home = new Home(); //Home                     
            ViewModelLocator.SetAutowirePartialView(home, MainContentPage);
            Switcher.Children.Add(home);// Trang home
            tabitem.Tabs.Add(new BottomTabItem() { IconImageSource = "ic_home.png", Label = MobileResource.Menu_TabItem_Home });
            if (CheckPermision((int)PermissionKeyNames.VehicleView))
            {
                var listVehicleTab = new ContentView(); //Home
                if (App.AppType == AppType.VMS || App.AppType == AppType.Moto)
                {
                    listVehicleTab = PrismApplicationBase.Current.Container.Resolve<ContentView>("ListVehicleTab"); //Phương tiện
                }
                else
                {
                    listVehicleTab = new ListVehiclePage();
                }
                ViewModelLocator.SetAutowirePartialView(listVehicleTab, MainContentPage);
                Switcher.Children.Add(listVehicleTab);
                tabitem.Tabs.Add(new BottomTabItem() { IconImageSource = "ic_vehicle.png", Label = MobileResource.Menu_TabItem_Vehicle });
            }
            tabitem.SelectedTabIndexChanged += Tabitem_SelectedTabIndexChanged;
            if (CheckPermision((int)PermissionKeyNames.ViewModuleOnline))
            {
                checkpermissiononline = true;
                var online = new ContentView(); //Home
                if (App.AppType == AppType.VMS || App.AppType == AppType.Moto)
                {
                    //cấu hình cty này dùng Cluster thì mới mở forms Cluster
                    if (MobileUserSettingHelper.EnableShowCluster)
                    {
                        online = PrismApplicationBase.Current.Container.Resolve<ContentView>("OnlineTab"); //Online                       
                    }
                    else
                    {
                        online = PrismApplicationBase.Current.Container.Resolve<ContentView>("OnlineTabNoCluster"); //Online
                    }
                }
                else
                {
                    //cấu hình cty này dùng Cluster thì mới mở forms Cluster
                    if (MobileUserSettingHelper.EnableShowCluster)
                    {
                        online = new OnlinePage();

                    }
                    else
                    {
                        online = new OnlinePageNoCluster();

                    }
                }
                ViewModelLocator.SetAutowirePartialView(online, MainContentPage);
                Switcher.Children.Add(online);
                tabitem.Tabs.Add(new BottomTabItem() { IconImageSource = "ic_mornitoring.png", Label = MobileResource.Menu_TabItem_Monitoring });
                Switcher.SelectedIndex = Switcher.Children.Count - 1;
            }
            if (CheckPermision((int)PermissionKeyNames.ViewModuleRoute))
            {
                var routeTab = new ContentView();
                if (App.AppType == AppType.VMS || App.AppType == AppType.Moto)
                {
                    routeTab = PrismApplicationBase.Current.Container.Resolve<ContentView>("RouteTab"); //RouteTab
                }
                else
                {
                    routeTab = new RoutePage();
                }

                ViewModelLocator.SetAutowirePartialView(routeTab, MainContentPage);
                Switcher.Children.Add(routeTab);
                tabitem.Tabs.Add(new BottomTabItem() { IconImageSource = "ic_route.png", Label = App.AppType == AppType.VMS ? MobileResource.Menu_TabItem_Voyage : MobileResource.Menu_TabItem_Route });
            }
            var accountTab = new Account(); //Account
            ViewModelLocator.SetAutowirePartialView(accountTab, MainContentPage);
            Switcher.Children.Add(accountTab);
            tabitem.Tabs.Add(new BottomTabItem() { IconImageSource = "ic_account.png", Label = MobileResource.Menu_TabItem_Account });
            InitAnimation();
            eventAggregator = PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
            this.eventAggregator.GetEvent<ShowTabItemEvent>().Subscribe(ShowTabItem);
            previousIndex = Switcher.SelectedIndex;
        }

        private int previousIndex { get; set; }

        private void Tabitem_SelectedTabIndexChanged(object sender, SelectedPositionChangedEventArgs e)
        {
            if (previousIndex != (int)e.SelectedPosition)
            {
                var index = (int)e.SelectedPosition;
                //Change icon at lostselected tabItem
                if (((BottomTabItem)tabitem.Tabs[previousIndex]).IconImageSource != null || !string.IsNullOrEmpty(((BottomTabItem)tabitem.Tabs[previousIndex]).IconImageSource.ToString()))
                {
                    var newPath = ((BottomTabItem)tabitem.Tabs[previousIndex]).IconImageSource.ToString().Replace("solid", string.Empty);
                    newPath = newPath.Replace("File:", string.Empty).Trim();
                    ((BottomTabItem)tabitem.Tabs[previousIndex]).IconImageSource = newPath;
                }
                //Change icon selected tabItem
                if (((BottomTabItem)tabitem.Tabs[index]).IconImageSource != null || !string.IsNullOrEmpty(((BottomTabItem)tabitem.Tabs[index]).IconImageSource.ToString()))
                {
                    var path = ((BottomTabItem)tabitem.Tabs[index]).IconImageSource.ToString().Replace(".png", "solid.png");
                    path = path.Replace("File:", string.Empty).Trim();
                    ((BottomTabItem)tabitem.Tabs[index]).IconImageSource = path;
                }
                if (this.eventAggregator != null)
                {
                    this.eventAggregator.GetEvent<TabSelectedChangedEvent>().Publish(index);
                }

                previousIndex = index;

            }

        }


        protected override void OnAppearing()
        {
            if (!checkpermissiononline)
            {
                //Switcher.SelectedIndex = 1;
                Switcher.SelectedIndex = 0;
            }

            base.OnAppearing();
            if (Device.RuntimePlatform == Device.iOS)
            {
                var safe = On<iOS>().SafeAreaInsets();
                Padding = new Thickness(0, 0, 0, safe.Bottom);
            }
        }

        private enum States
        {
            ShowFilter,
            HideFilter,
            ShowStatus,
            HideStatus
        }

        private readonly IEventAggregator eventAggregator;

        private readonly BA_MobileGPS.Core.Animation _animations = new BA_MobileGPS.Core.Animation();

        private async void InitAnimation()
        {
            try
            {
                if (_animations == null)
                {
                    return;
                }

                _animations.Add(States.ShowFilter, new[] {
                                                            new ViewTransition(TabHost, AnimationType.TranslationY, 0, 100, delay: 100), // Active and visible
                                                new ViewTransition(TabHost, AnimationType.Opacity, 1, 0), // Active and visible
                                                          });

                _animations.Add(States.HideFilter, new[] {
                                                            new ViewTransition(TabHost, AnimationType.TranslationY, 100),
                                                            new ViewTransition(TabHost, AnimationType.Opacity, 0),
                                                          });


                await _animations.Go(States.HideStatus, false);

            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError("InitAnimation", ex);
            }
        }

        private void ShowTabItem(bool check)
        {
            if (check)
            {
                ShowTab();
            }
            else
            {
                HideTab();
            }
        }

        /// <summary>
        /// ẩn tab
        /// </summary>
        public async void HideTab()
        {
            await _animations.Go(States.HideFilter, true);
        }

        /// <summary>
        /// Hiển thị tab
        /// </summary>
        private async void ShowTab()
        {
            await _animations.Go(States.ShowFilter, true);
        }

        public virtual bool CheckPermision(int PermissionKey)
        {
            return StaticSettings.User.Permissions.IndexOf(PermissionKey) != -1;
        }


        bool bExit = false;

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            if (!bExit)
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // Confirmation dialog "Are you sure you want to close?"
                    bExit = await DisplayAlert("", "Bạn có chắc chắn muốn thoát khỏi Ứng dụng không?", "Đồng ý", "Bỏ qua");
                    if (bExit)
                    {
                        // Toast notification... "Press back button again to close"
                        DependencyService.Get<IDisplayMessage>().ShowMessageInfo("Back thêm lần nữa để thoát ứng dụng");
                        this.OnBackButtonPressed();
                    }
                });
            else
                return false;

            return true;
        }
    }
}