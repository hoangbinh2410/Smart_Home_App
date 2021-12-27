using BA_MobileGPS.Core.Models;
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
            lblFeatures.Text = MobileResource.Common_Label_Features;
            lblReport.Text = MobileResource.Common_Label_Report;
            lblCamera.Text = MobileResource.Camera_Label_MenuTitle;
            viewmore.Text=MobileResource.Online_Lable_ViewMore;
            close.Text=MobileResource.Common_Label_Close;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitMenuPageItems();
        }

        private void InitMenuPageItems()
        {
            var list = new List<MenuPageItem>();

            list.Add(new MenuPageItem
            {
                Title = MobileResource.Online_Label_TitlePage,
                Icon = "ic_mornitoring.png",
                MenuType = MenuKeyType.Online,
                IsEnable = true,
            });
            list.Add(new MenuPageItem
            {
                Title = MobileResource.Route_Label_Title,
                Icon = "ic_route.png",
                MenuType = MenuKeyType.Route,
                IsEnable = CheckPermision((int)PermissionKeyNames.ViewModuleRoute)
            });
            list.Add(new MenuPageItem
            {
                Title = MobileResource.DetailVehicle_Label_TilePage,
                Icon = "ic_guarantee.png",
                MenuType = MenuKeyType.VehicleDetail,
                IsEnable = true,
            });
            list.Add(new MenuPageItem
            {
                Title = MobileResource.Camera_Label_Video,
                Icon = "ic_videolive.png",
                MenuType = MenuKeyType.Video,
                IsEnable = CheckPermision((int)PermissionKeyNames.TrackingVideosView),
            });
            list.Add(new MenuPageItem
            {
                Title = MobileResource.Image_Lable_Image,
                Icon = "ic_cameraonline.png",
                MenuType = MenuKeyType.Images,
                IsEnable = CheckPermision((int)PermissionKeyNames.TrackingOnlineByImagesView),
            });
            list.Add(new MenuPageItem
            {
                Title = MobileResource.Camera_Title_Retreaming,
                Icon = "ic_videolive.png",
                MenuType = MenuKeyType.VideoPlayback,
                IsEnable = CheckPermision((int)PermissionKeyNames.TrackingVideosView),
            });

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