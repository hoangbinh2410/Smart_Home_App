using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Core.ViewModels;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace VMS_MobileGPS.ViewModels
{
    public class HeplerViewModel : ViewModelBase
    {
        private List<HeplerMenuModel> listHelper;
        public List<HeplerMenuModel> ListHelper { get => listHelper; set => SetProperty(ref listHelper, value); }

        public ICommand ItemSelectedCommand { get; set; }

        [Obsolete]
        public HeplerViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            ItemSelectedCommand = new DelegateCommand<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemSelected);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            ShowHelpMenu();
        }

        private void ShowHelpMenu()
        {
            var menuhelp = new List<HeplerMenuModel>();

            var menu = StaticSettings.ListMenu;

            if (menu != null && menu.Count > 0)
            {
                menuhelp.Add(new HeplerMenuModel()
                {
                    Icon = "ic_fishingnet.png",
                    Title = MobileResource.Helper_Label_Using + MobileResource.Login_Popup_Starting_Page.ToLower(),
                    Link = "https://www.youtube.com/watch?v=yldaLkQ2Xto"
                });
                foreach (var item in menu)
                {

                    if ((int)PermissionKeyNames.ViewModuleOnline == item.PK_MenuItemID) // Giám sát
                    {
                        menuhelp.Add(new HeplerMenuModel()
                        {
                            Icon = item.IconMobile,
                            Title = MobileResource.Helper_Label_Using + item.NameByCulture.ToLower(),
                            Link = "https://www.youtube.com/watch?v=r4WLJnqU3KI"
                        });
                    }
                    else if ((int)PermissionKeyNames.ViewModuleRoute == item.PK_MenuItemID) // hải trình lộ trình
                    {
                        menuhelp.Add(new HeplerMenuModel()
                        {
                            Icon = item.IconMobile,
                            Title = MobileResource.Helper_Label_Using + item.NameByCulture.ToLower(),
                            Link = "https://www.youtube.com/watch?v=Fup8RiPFoL8"
                        });
                    }
                    else if ((int)PermissionKeyNames.ViewMessageOnline == item.PK_MenuItemID) // tin nhắn online
                    {
                        menuhelp.Add(new HeplerMenuModel()
                        {
                            Icon = item.IconMobile,
                            Title = MobileResource.Helper_Label_Using + item.NameByCulture.ToLower(),
                            Link = "https://www.youtube.com/watch?v=0caPEO42Rco"
                        });
                    }
                }
            }

            ListHelper = menuhelp;
        }

        [Obsolete]
        private void ItemSelected(Syncfusion.ListView.XForms.ItemTappedEventArgs args)
        {
            if (args.ItemData is HeplerMenuModel hepler)
            {
                Device.OpenUri(new Uri(hepler.Link));
            }
        }
    }
}