﻿using BA_MobileGPS.Entities;
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
            //favouriteListView.Loaded += Favourite_SfListView_Loaded;
           // favouriteListView.PropertyChanged += FavouriteListView_PropertyChanged;
        }

        //private void FavouriteListView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "ItemSource")
        //    {

        //    }
        //}


        //private void Favourite_SfListView_Loaded(object sender, ListViewLoadedEventArgs e)
        //{
        //    CenterFavouriteListViewConfig();
        //}

        private void CenterFavouriteListViewConfig()
        {
            var visualContainer = favouriteListView.GetVisualContainer();
            var totalextent = (double)visualContainer.GetType().GetRuntimeProperties().FirstOrDefault(container => container.Name == "TotalExtent").GetValue(visualContainer);

            favouriteListView.WidthRequest = totalextent + 10;
        }

        private void FeaturesSfListView_Loaded(object sender, ListViewLoadedEventArgs e)
        {
            var listViewTempalte = (SfListView)sender;

            var itemSource = (List<HomeMenuItemViewModel>)listViewTempalte.ItemsSource;
            var itemCount = itemSource.Count;
            if (itemCount <= 3)
            {
                listViewTempalte.LayoutManager = new GridLayout() { SpanCount = 1 };
            }
            else listViewTempalte.LayoutManager = new GridLayout() { SpanCount = 2 };

            var visualContainer = listViewTempalte.GetVisualContainer();
            var totalextent = (double)visualContainer.GetType().GetRuntimeProperties().FirstOrDefault(container => container.Name == "TotalExtent").GetValue(visualContainer);
            listViewTempalte.WidthRequest = totalextent + 10;
        }

      
    }
}
