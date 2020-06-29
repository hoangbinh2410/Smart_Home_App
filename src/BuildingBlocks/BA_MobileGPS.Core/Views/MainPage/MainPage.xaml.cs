using BA_MobileGPS.Core.Events;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using Prism.Events;
using Prism.Ioc;
using Rg.Plugins.Popup.Services;
using Sharpnado.Presentation.Forms.CustomViews.Tabs;
using Shiny;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            GenerateTabItem();

            TabHost.SelectedIndex = 0;
            var _eventAggregator = Prism.PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<TabItemSwitchEvent>().Subscribe(TabItemSwitch);
        }

    

        private void TabItemSwitch(HomeMenuItem selectItem)
        {
            for (int i = 0; i < TabHost.Tabs.Count; i++)
            {
                var label = ((BottomTabItem)TabHost.Tabs[i]).Label.Trim().ToLower();
                if (string.Equals(selectItem.NameByCulture.Trim().ToLower(),label)
                    && TabHost.Tabs[i].IsVisible)
                {
                    TabHost.SelectedIndex = i;
                    break;
                }
            }
        }

        private void GenerateTabItem()
        {
            var source = StaticSettings.ListMenuOriginGroup;

            foreach (var temp in source)
            {
                foreach (BottomTabItem tab in TabHost.Tabs)
                {
                    // Label được hard code ở view
                    if (string.Equals(temp.NameByCulture.Trim().ToLower(), tab.Label.Trim().ToLower()))
                    {
                        tab.IsVisible = true;
                        //Lưu trữ visible tabitem vào setting
                        StaticSettings.ListMenu.Add(temp);
                        break;
                    }
                }
            }

        }



    }
}
