using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using Rg.Plugins.Popup.Pages;
using System.Collections.Generic;
using System.Linq;

namespace BA_MobileGPS.Core.Views
{
    public partial class DetailVehiclePopup : PopupPage
    {
        public DetailVehiclePopup()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitMenuItems();
        }

        private void InitMenuItems()
        {
            var list = new List<MenuItem>();

            list.Add(new MenuItem
            {
                Title = MobileResource.Online_Label_TitlePage,
                Icon = "ic_mornitoring.png",
                IsEnable = true,
            });
            list.Add(new MenuItem
            {
                Title = MobileResource.Route_Label_Title,
                Icon = "ic_route.png",
                IsEnable = CheckPermision((int)PermissionKeyNames.ViewModuleRoute)
            });
            list.Add(new MenuItem
            {
                Title = MobileResource.DetailVehicle_Label_TilePage,
                Icon = "ic_guarantee.png",
                IsEnable = true,
            });
            list.Add(new MenuItem
            {
                Title = MobileResource.Camera_Label_Video,
                Icon = "ic_videolive.png",
                IsEnable = CheckPermision((int)PermissionKeyNames.TrackingVideosView),
            });
            list.Add(new MenuItem
            {
                Title = MobileResource.Image_Lable_Image,
                Icon = "ic_cameraonline.png",
                IsEnable = CheckPermision((int)PermissionKeyNames.TrackingOnlineByImagesView),
            });
            list.Add(new MenuItem
            {
                Title = MobileResource.Camera_Lable_ExportVideo,
                Icon = "ic_videolive.png",
                IsEnable = CheckPermision((int)PermissionKeyNames.TrackingVideosView),
            });
            if (!CheckPermision((int)PermissionKeyNames.TrackingVideosView))
            {
                list.Add(new MenuItem
                {
                    Title = MobileResource.DetailVehicle_Label_Fuel,
                    Icon = "ic_fuel.png",
                    IsEnable = CheckPermision((int)PermissionKeyNames.ShowFuelChartOnline),
                });
            }

            var lstresource = list.Where(x => x.IsEnable == true).ToList();
            if (lstresource.Count <= 3)
            {
                lsvMenu.HeightRequest = 120;
            }
            else
            {
                lsvMenu.HeightRequest = 200;
            }

            if (lstresource.Count == 4)
            {
                lsvMenu.ColumnCount = 2;
            }
            else
            {
                lsvMenu.ColumnCount = 3;
            }
            lsvMenu.ItemsSource = lstresource;
        }

        public virtual bool CheckPermision(int PermissionKey)
        {
            return StaticSettings.User.Permissions.IndexOf(PermissionKey) != -1;
        }

        private void PopupPage_BackgroundClicked(object sender, System.EventArgs e)
        {
            ((DetailVehiclePopupViewModel)BindingContext).Close();
        }
    }
}