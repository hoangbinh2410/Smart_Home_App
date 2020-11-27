using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace MOTO_MobileGPS.ViewModels
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
                menuhelp.Add(new HeplerMenuModel()  //đăng nhập
                {
                    Icon = "ic_login_tutorial.png",
                    Title = MobileResource.Helper_Label_Using2 + MobileResource.Login_Button_Login.ToLower(),
                    Link = "https://www.youtube.com/watch?v=v-RgTxws1V0"
                });
                foreach (var item in menu)
                {
                    if ((int)PermissionKeyNames.ViewModuleOnline == item.PK_MenuItemID) // Giám sát
                    {
                        menuhelp.Add(new HeplerMenuModel()
                        {
                            Icon = item.IconMobile,
                            Title = MobileResource.Helper_Label_Using2 + item.NameByCulture.ToLower(),
                            Link = "https://www.youtube.com/watch?v=Gi90YeXezfE"
                        });
                    }
                    else if ((int)PermissionKeyNames.ViewModuleRoute == item.PK_MenuItemID) // hải trình lộ trình
                    {
                        menuhelp.Add(new HeplerMenuModel()
                        {
                            Icon = item.IconMobile,
                            Title = MobileResource.Helper_Label_Using2 + item.NameByCulture.ToLower(),
                            Link = "https://www.youtube.com/watch?v=85UfWyzFcpI&t=14s"
                        });
                    }
                }

                menuhelp.Add(new HeplerMenuModel() // cảnh báo
                {
                    Icon = "ic_anglebell.png",
                    Title = MobileResource.Helper_Label_Using2 + MobileResource.Common_Message_Warning.ToLower(),
                    Link = "https://www.youtube.com/watch?v=MYuiYmc6ncc&t=4s"
                });
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