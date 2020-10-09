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
                Title = "Video",
                Icon = "ic_videolive.png",
                IsEnable = CheckPermision((int)PermissionKeyNames.TrackingVideosView),
            });
            list.Add(new MenuItem
            {
                Title = "Hình Ảnh",
                Icon = "ic_cameraonline.png",
                IsEnable = CheckPermision((int)PermissionKeyNames.TrackingOnlineByImagesView),
            });
            list.Add(new MenuItem
            {
                Title = "Nhiên liệu",
                Icon = "ic_fuel.png",
                IsEnable = CheckPermision((int)PermissionKeyNames.ReportFuelView),
            });
            var lstresource = list.Where(x => x.IsEnable == true).ToList();
            if (lstresource.Count <= 3)
            {
                lsvMenu.HeightRequest = 120;
            }
            else
            {
                lsvMenu.HeightRequest = 180;
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
    }
}