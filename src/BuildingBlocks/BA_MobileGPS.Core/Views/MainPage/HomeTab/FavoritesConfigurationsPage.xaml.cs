using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class FavoritesConfigurationsPage : ContentPage
    {
        private FavoritesConfigurationsPageViewModel viewModel = null;

        public FavoritesConfigurationsPage()
        {
            InitializeComponent();
            main.Title = MobileResource.Favorites_Label_TilePage;
        }
        protected override void OnAppearing()
        {          
            base.OnAppearing();
            searchBarAndroid.Placeholder = MobileResource.Common_Message_SearchText;
            searchBarIOS.Placeholder = MobileResource.Common_Message_SearchText;
            btnAlert.Text = MobileResource.Common_Button_Save;            
        }

        private void ListView_ItemDragging(object sender, ItemDraggingEventArgs e)
        {
            try
            {
                if (e.Action == DragAction.Drop)
                {
                    // Refresh lại danh sách
                    listView.RefreshListViewItem();

                    var items = listView.DataSource.Items as List<HomeMenuItemViewModel>;
                    var index = e.NewIndex;
                    var olds = viewModel.MenuItems.ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        private void ListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            viewModel = BindingContext as FavoritesConfigurationsPageViewModel;
            try
            {
                var item = e.ItemData as HomeMenuItemViewModel;

                if (item.IsFavorited)
                {
                    item.IsFavorited = false;
                    item.GroupName = viewModel.GetOriginGroupName(item.PK_MenuItemID);

                    if (viewModel.FavoriteMenuItems.ToList().Any(x => x.PK_MenuItemID == item.PK_MenuItemID))
                    {
                        viewModel.FavoriteMenuItems.Remove(item);
                    }
                }
                else
                {
                    item.IsFavorited = true;
                    item.GroupName = MobileResource.Menu_Label_Favorite;

                    if (!viewModel.FavoriteMenuItems.ToList().Any(x => x.PK_MenuItemID == item.PK_MenuItemID))
                    {
                        viewModel.FavoriteMenuItems.Add(item);
                    }
                }

                viewModel.ReSort();
            }
            catch (System.Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }
    }
}