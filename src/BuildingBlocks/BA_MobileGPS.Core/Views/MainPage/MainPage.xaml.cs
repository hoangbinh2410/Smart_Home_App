﻿using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using Prism;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Sharpnado.Presentation.Forms.CustomViews.Tabs;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace BA_MobileGPS.Core.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();                      
            var home = PrismApplicationBase.Current.Container.Resolve<ContentView>("HomeTab"); //Home
            ViewModelLocator.SetAutowirePartialView(home, MainContentPage);
            Switcher.Children.Add(home);// Trang home
            tabitem.Tabs.Add(new BottomTabItem() { IconImageSource = "ic_home.png", Label = MobileResource.Menu_TabItem_Home });

            if (CheckPermision((int)PermissionKeyNames.VehicleView))
            {
                var listVehicleTab = PrismApplicationBase.Current.Container.Resolve<ContentView>("ListVehicleTab"); //Phương tiện
                ViewModelLocator.SetAutowirePartialView(listVehicleTab, MainContentPage);
                Switcher.Children.Add(listVehicleTab);
                tabitem.Tabs.Add(new BottomTabItem() { IconImageSource = "ic_ship.png", Label = MobileResource.Menu_TabItem_Vehicle });
            }
            tabitem.SelectedTabIndexChanged += Tabitem_SelectedTabIndexChanged;
            if (CheckPermision((int)PermissionKeyNames.ViewModuleOnline))
            {
                var online = PrismApplicationBase.Current.Container.Resolve<ContentView>("OnlineTab"); //Online
                ViewModelLocator.SetAutowirePartialView(online, MainContentPage);
                Switcher.Children.Add(online);
                tabitem.Tabs.Add(new BottomTabItem() { IconImageSource = "ic_mornitoring.png", Label = MobileResource.Menu_TabItem_Monitoring });
                Switcher.SelectedIndex = Switcher.Children.Count - 1;
            }
            else
            {
                Switcher.SelectedIndex = 1;
                Switcher.SelectedIndex = 0;
            }

            if (CheckPermision((int)PermissionKeyNames.ViewModuleRoute))
            {
                var routeTab = PrismApplicationBase.Current.Container.Resolve<ContentView>("RouteTab"); //RouteTab
                ViewModelLocator.SetAutowirePartialView(routeTab, MainContentPage);
                Switcher.Children.Add(routeTab);
                tabitem.Tabs.Add(new BottomTabItem() { IconImageSource = "ic_voyage.png", Label = MobileResource.Menu_TabItem_Voyage });
            }

            var accountTab = PrismApplicationBase.Current.Container.Resolve<ContentView>("AccountTab"); //Account
            ViewModelLocator.SetAutowirePartialView(accountTab, MainContentPage);
            Switcher.Children.Add(accountTab);
            tabitem.Tabs.Add(new BottomTabItem() { IconImageSource = "ic_account.png", Label = MobileResource.Menu_TabItem_Account });

            eventAggregator = PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
            InitAnimation();
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
                if (((BottomTabItem)tabitem.Tabs[previousIndex]).IconImageSource !=null || !string.IsNullOrEmpty(((BottomTabItem)tabitem.Tabs[previousIndex]).IconImageSource.ToString()))
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
                previousIndex = index;
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
                                                            new ViewTransition(TabHost, AnimationType.TranslationY, 0, 300, delay: 300), // Active and visible
                                                new ViewTransition(TabHost, AnimationType.Opacity, 1, 0), // Active and visible
                                                          });

                _animations.Add(States.HideFilter, new[] {
                                                            new ViewTransition(TabHost, AnimationType.TranslationY, 300),
                                                            new ViewTransition(TabHost, AnimationType.Opacity, 0),
                                                          });

                await _animations.Go(States.ShowFilter, false);
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
                ShowBoxInfo();
            }
            else
            {
                HideBoxInfo();
            }
        }

        /// <summary>
        /// ẩn tab
        /// </summary>
        public async void HideBoxInfo()
        {
            await _animations.Go(States.HideFilter, true);
        }

        /// <summary>
        /// Hiển thị tab
        /// </summary>
        private async void ShowBoxInfo()
        {
            await _animations.Go(States.ShowFilter, true);
        }

        public virtual bool CheckPermision(int PermissionKey)
        {
            return StaticSettings.User.Permissions.IndexOf(PermissionKey) != -1;
        }
    }
}