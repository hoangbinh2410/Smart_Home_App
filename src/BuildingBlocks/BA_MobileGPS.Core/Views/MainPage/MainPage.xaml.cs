using BA_MobileGPS.Core.Helpers;
using Prism;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace BA_MobileGPS.Core.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);


            var home = PrismApplicationBase.Current.Container.Resolve<ContentView>("HomeTab"); //Online
            ViewModelLocator.SetAutowirePartialView(home, MainContentPage);
            Switcher.Children.Add(home);// Trang online

            var listVehicleTab = PrismApplicationBase.Current.Container.Resolve<ContentView>("ListVehicleTab"); //Phương tiện
            ViewModelLocator.SetAutowirePartialView(listVehicleTab, MainContentPage);
            Switcher.Children.Add(listVehicleTab);

            var online = PrismApplicationBase.Current.Container.Resolve<ContentView>("OnlineTab"); //Online
            ViewModelLocator.SetAutowirePartialView(online, MainContentPage);
            Switcher.Children.Add(online);

            var routeTab = PrismApplicationBase.Current.Container.Resolve<ContentView>("RouteTab"); //RouteTab
            ViewModelLocator.SetAutowirePartialView(routeTab, MainContentPage);
            Switcher.Children.Add(routeTab);

            var accountTab = PrismApplicationBase.Current.Container.Resolve<ContentView>("AccountTab"); //Account
            ViewModelLocator.SetAutowirePartialView(accountTab, MainContentPage);
            Switcher.Children.Add(accountTab);

         

            eventAggregator = PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
            InitAnimation();
            this.eventAggregator.GetEvent<ShowTabItemEvent>().Subscribe(ShowTabItem);
            Switcher.SelectedIndex = 2;
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
            //TabHost.IsVisible = check;
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
    }
}