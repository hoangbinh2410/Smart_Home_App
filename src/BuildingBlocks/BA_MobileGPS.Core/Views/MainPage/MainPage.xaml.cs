using BA_MobileGPS.Entities;
using Sharpnado.Presentation.Forms.CustomViews.Tabs;
using Shiny;
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
        }

        private void GenerateTabItem()
        {
            var source = StaticSettings.ListMenu;

            foreach (var temp in source)
            {
                foreach (BottomTabItem tab in TabHost.Tabs)
                {
                    if (temp.NameByCulture.Trim().ToLower().Contains(tab.Label.Trim().ToLower()))
                    {
                        tab.IsVisible = true;
                        break;
                    }

                }
            }
        }
    }
}
