using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using System.Linq;
using System.Reflection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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
            lbFavories.Text = MobileResource.Common_Label_HighLight;
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

                    if (viewModel.FavoriteMenuItems.ToList().Any(x => x.PK_MenuItemID == item.PK_MenuItemID))
                    {
                        viewModel.FavoriteMenuItems.Remove(item);
                    }
                }
                else
                {
                    item.IsFavorited = true;
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