using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms.Extensions;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace BA_MobileGPS.Core.ViewModels
{
    public class LoginPreviewFeaturesPageViewModel : ViewModelBase
    {
        public LoginPreviewFeaturesPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            ClosePopupCommand = new DelegateCommand(ClosePopup);
            NavigateCommand = new DelegateCommand<ItemTappedEventArgs>(Navigate);
            items = new ObservableCollection<LoginPopupItem>();
            InitItems();
        }

        private void InitItems()
        {
            var list = new List<LoginPopupItem>();
            if (App.AppType == Entities.AppType.VMS)
            {
                // Ra khơi
                list.Add(new LoginPopupItem
                {
                    Title = MobileResource.Login_Popup_Starting_Page,
                    Icon = "ic_fishingnet.png",
                    Url = "/NavigationPage/OfflinePage",
                    ItemType = LoginPopupItemType.OfflinePage,
                    IsEnable = true
                });
            }
            // Facebook
            list.Add(new LoginPopupItem
            {
                Title = "Facebook",
                Icon = "ic_facebook.png",
                Url = MobileSettingHelper.LinkFacebook,
                ItemType = LoginPopupItemType.Facebook,
                IsEnable = true
            });
            // Zalo
            list.Add(new LoginPopupItem
            {
                Title = "Zalo",
                Icon = "ic_zalo.png",
                Url = MobileSettingHelper.LinkZalo,
                ItemType = LoginPopupItemType.Zalo,
                IsEnable = true
            });
            // Youtube
            list.Add(new LoginPopupItem
            {
                Title = "Youtube",
                Icon = "ic_youtube.png",
                Url = MobileSettingHelper.LinkYoutube,
                ItemType = LoginPopupItemType.Youtube,
                IsEnable = true
            });
            // Tiktok
            list.Add(new LoginPopupItem
            {
                Title = "Tiktok",
                Icon = "ic_tiktok.png",
                Url = MobileSettingHelper.LinkTiktok,
                ItemType = LoginPopupItemType.Tiktok,
                IsEnable = true
            });
            // Mạng lưới
            list.Add(new LoginPopupItem
            {
                Title = MobileResource.Login_Popup_Network,
                Icon = "ic_network.png",
                Url = MobileSettingHelper.Network,
                ItemType = LoginPopupItemType.Network,
                IsEnable = MobileSettingHelper.IsUseNetwork
            });
            // Trải nghiệm BAGPS
            list.Add(new LoginPopupItem
            {
                Title = MobileResource.Login_Popup_BAGPSExperience,
                Icon = "ic_minilogo.png",
                Url = MobileSettingHelper.LinkYoutube,
                ItemType = LoginPopupItemType.BAGPSExperience,
                IsEnable = MobileSettingHelper.IsUseExperience
            });
            // Đăng kí tư vấn
            //list.Add(new LoginPopupItem
            //{
            //    Title = MobileResource.Login_Popup_RegisterSupport,
            //    Icon = "ic_chatsupport.png",
            //    Url = "BaseNavigationPage/RegisterConsultPage",
            //    ItemType = LoginPopupItemType.RegisterSupport,
            //    IsEnable = MobileSettingHelper.IsUseRegisterSupport
            //});
            Items = list.Where(x => x.IsEnable == true).ToObservableCollection();
        }

        private async void Navigate(ItemTappedEventArgs args)
        {
            try
            {
                if (!(args.ItemData is LoginPopupItem item) || string.IsNullOrEmpty(item.Url))
                    return;
                else
                {
                    await NavigationService.GoBackAsync(useModalNavigation: true, animated: true, parameters: new NavigationParameters
                        {
                            { "popupitem",  item}
                        });
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private async void ClosePopup()
        {
            //await PopupNavigation.Instance.PopAsync();
            var a = await NavigationService.GoBackAsync();
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

        public bool IsEnable { get; set; }
    }

    public enum LoginPopupItemType
    {
        OfflinePage,
        Manual,
        Guarantee,
        Network,
        RegisterSupport,
        BAGPSExperience,
        Facebook,
        Zalo,
        Tiktok,
        Youtube
    }
}