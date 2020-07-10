using Prism;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Events;
using BA_MobileGPS.Core;
using System.Runtime.Serialization.Json;
using BA_MobileGPS.Core.Helpers;

namespace VMS_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private enum States
        {
            ShowFilter,
            HideFilter,
            ShowStatus,
            HideStatus
        }

        private readonly IEventAggregator eventAggregator;

        private readonly BA_MobileGPS.Core.Animation _animations = new BA_MobileGPS.Core.Animation();

        public MainPage()
        {
            InitializeComponent();
            eventAggregator = PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();

            InitAnimation();

            this.eventAggregator.GetEvent<ShowTabItemEvent>().Subscribe(ShowTabItem);
        }

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
            if(check)
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