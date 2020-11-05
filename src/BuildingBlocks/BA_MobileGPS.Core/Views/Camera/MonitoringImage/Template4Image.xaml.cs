using BA_MobileGPS.Core.Controls;
using BA_MobileGPS.Utilities;
using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views.Camera.MonitoringImage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Template4Image : ContentView
    {
        public Template4Image()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                Color unFavorites = (Color)Application.Current.Resources["WhiteColor"];
                Color favorites = (Color)Application.Current.Resources["YellowColor"];
                var temp = (StackLayout)sender;

                var colorFavorites = temp.Children.Where(x => x is IconView).FirstOrDefault() as IconView;
                if (colorFavorites != null)
                {
                    if (colorFavorites.Foreground == unFavorites)
                    {
                        colorFavorites.Foreground = favorites;
                    }
                    else
                    {
                        colorFavorites.Foreground = unFavorites;
                    }
                }

                var vehiclePlate = temp.Children.Where(x => x is Label).FirstOrDefault() as Label;
                if (!string.IsNullOrEmpty(vehiclePlate.Text))
                {
                    FavoritesVehicleImageHelper.UpdateFavoritesVehicleImage(vehiclePlate.Text);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void chkFa_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
        }
    }
}