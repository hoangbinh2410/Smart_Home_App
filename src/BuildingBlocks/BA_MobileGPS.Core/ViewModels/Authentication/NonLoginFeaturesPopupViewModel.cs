using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;
namespace BA_MobileGPS.Core.ViewModels
{
    public class NonLoginFeaturesPopupViewModel : ViewModelBase
    {
        public NonLoginFeaturesPopupViewModel(INavigationService navigationService) : base(navigationService)
        {
            ClosePopupCommand = new DelegateCommand(ClosePopup);
            NavigateCommand = new DelegateCommand<ItemTappedEventArgs>(Navigate);
            items = new ObservableCollection<LoginPopupItem>();
            InitItems();
        }
        
        private void InitItems()
        {
            var list = new List<LoginPopupItem>();
            // Ra khơi
            list.Add(new LoginPopupItem
            {
                Title = MobileResource.Login_Popup_Starting_Page,
                Icon = "ic_FishingNet.png",
                Url = "NavigationPage/OfflinePage",
                ItemType = LoginPopupItemType.OfflinePage
            });
            // Hướng dẫn sử dụng
            list.Add(new LoginPopupItem
            {
                Title = MobileResource.Login_Popup_Manual,
                Icon = "ic_Manual.png",
                Url = MobileSettingHelper.WebGps,
                ItemType = LoginPopupItemType.Manual,
            });
            // Thông tin bảo hành
            list.Add(new LoginPopupItem
            {
                Title = MobileResource.Login_Popup_Guarantee,
                Icon = "ic_Guarantee.png",
                Url = "NavigationPage/HelperPage",
                ItemType = LoginPopupItemType.Guarantee,
            });
            // Đăng kí tư vấn
            list.Add(new LoginPopupItem
            {
                Title = MobileResource.Login_Popup_RegisterSupport,
                Icon = "ic_MiniLogo.png",  
                Url = MobileSettingHelper.WebGps,
                ItemType = LoginPopupItemType.RegisterSupport,
            });
            // Trải nghiệm BAGPS
            list.Add(new LoginPopupItem
            {
                Title = MobileResource.Login_Popup_BAGPSExperience,
                Icon = "ic_sharecircle.png",       
                Url = MobileSettingHelper.LinkShareApp,
                ItemType = LoginPopupItemType.BAGPSExperience,           
            });

            Items = list.ToObservableCollection();
        }

        private async void Navigate(ItemTappedEventArgs args)
        {
            try
            {
                if (!(args.ItemData is LoginPopupItem item) || string.IsNullOrEmpty(item.Url))
                    return;              
                else
                {
                   var a =  await NavigationService.NavigateAsync(item.Url, null, useModalNavigation: true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private async void ClosePopup()
        {
            await Task.Delay(200);
            await PopupNavigation.Instance.PopAsync();
        }
        public ICommand NavigateCommand { get; }
        public ICommand ClosePopupCommand { get; }

        private ObservableCollection<LoginPopupItem> items;
        public ObservableCollection<LoginPopupItem> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

      
    }

    public class LoginPopupItem
    {
        public string Title { get; set; }

        public string Icon { get; set; }

        public string Url { get; set; }

        public LoginPopupItemType ItemType { get; set; }

    }

    public enum LoginPopupItemType
    {
        OfflinePage,
        Manual,
        Guarantee,
        RegisterSupport,
        BAGPSExperience,
    }


}
