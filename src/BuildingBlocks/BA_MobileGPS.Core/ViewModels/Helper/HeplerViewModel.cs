using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;

using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
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
                foreach (var item in menu)
                {
                    menuhelp.Add(new HeplerMenuModel()
                    {
                        Icon = "ic_list_black",
                        Title = MobileResource.Helper_Label_Using + item.NameByCulture.ToLower(),
                        Link = "https://www.youtube.com/channel/UC0vfDfFTKXXV_d7m86b1MhQ",
                        IsShow = true
                    });
                }
            }

            ListHelper = menuhelp.FindAll(h => h.IsShow);
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