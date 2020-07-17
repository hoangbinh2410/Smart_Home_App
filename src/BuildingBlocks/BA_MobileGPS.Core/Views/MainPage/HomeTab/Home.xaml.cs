using BA_MobileGPS.Entities;
using Syncfusion.ListView.XForms;
using Syncfusion.ListView.XForms.Control.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class Home : ContentView
    {
        public Home()
        {
            InitializeComponent();
        }

        private void SfListView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsSource")
            {                
                var listViewTempalte = (SfListView)sender;

                var itemSource = (List<HomeMenuItemViewModel>)listViewTempalte.ItemsSource;
                if (itemSource != null)
                {
                    var itemCount = itemSource.Count;
                    if (itemCount <= 3)
                    {
                        listViewTempalte.LayoutManager = new GridLayout() { SpanCount = 1 };
                        listViewTempalte.ItemSpacing = new Thickness(5, 0);
                        listViewTempalte.WidthRequest = itemCount * 100 + 10;
                        listViewTempalte.HeightRequest = 130;
                    }
                    else if(itemCount == 4)
                    {
                        listViewTempalte.LayoutManager = new GridLayout() { SpanCount = 2 };
                        listViewTempalte.ItemSpacing = new Thickness(20, 0);
                        listViewTempalte.WidthRequest = 290;
                        listViewTempalte.HeightRequest = 260;
                    }
                    else
                    {
                        listViewTempalte.LayoutManager = new GridLayout() { SpanCount = 2 };
                        listViewTempalte.ItemSpacing = new Thickness(5, 0);
                        listViewTempalte.WidthRequest = 350;
                        listViewTempalte.HeightRequest = 260;
                    }                  
                }
            }
        }

        private void Favourite_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsSource")
            {
                var listViewTempalte = (SfListView)sender;

                var itemSource = (List<HomeMenuItemViewModel>)listViewTempalte.ItemsSource;
                if (itemSource != null)
                {
                    var itemCount = itemSource.Count;
                    if (itemCount < 3)
                    {
                        listViewTempalte.WidthRequest = itemCount * 100 + 10;
                    }
                    else
                    {
                        listViewTempalte.WidthRequest = 350;
                    }                  
                }
            }
        }
    }
}
