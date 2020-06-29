using BA_MobileGPS.Core.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Interfaces
{
    public class PopupServices : IPopupServices
    {
        Color errorTextColor = (Color)Application.Current.Resources["DarkErorrPopupTextColor"];
        Color errorBtnTextColor = (Color)Application.Current.Resources["DarkErorrPopupButtonTextColor"];
        Color errorBtnColor = (Color)Application.Current.Resources["DarkErorrPopupButtonBackgroundColor"];

        Color yesBtnTextColor = (Color)Application.Current.Resources["DarkPopupYesBtnTextColor"];
        Color yesBtnBackgroundColor = (Color)Application.Current.Resources["DarkPopupYesBtnBackgroundColor"];

        public async Task ShowConfirmIconPopup(string title, string content, string iconImageSource, Color iconColor, IconPosititon iconPosititon, Action<bool> callback = null)
        {
            await PopupNavigation.Instance.
                                  PushAsync(new BasePopup(title, content, iconPosititon, PopupType.YesNo, iconImageSource,
                                  null, null, null, iconColor, yesBtnBackgroundColor, yesBtnTextColor, callback));
        }

        public async Task ShowConfirmPopup(string title, string content, Action<bool> callback)
        {          
            await PopupNavigation.Instance.
                                   PushAsync(new BasePopup(title, content, IconPosititon.None, PopupType.YesNo,
                                   null, null, null, null, null, yesBtnBackgroundColor, yesBtnTextColor, callback));
        }

        public async Task ShowErrorIconPopup(string title, string content, string iconImageSource, Color iconColor, IconPosititon iconPosititon)
        {          
            await PopupNavigation.Instance.
                                   PushAsync(new BasePopup(title, content, iconPosititon, PopupType.YesNo, iconImageSource,
                                   null, null, errorTextColor, iconColor, errorBtnColor, errorBtnTextColor));
        }

        public async Task ShowErrorPopup(string title, string content)
        {
            await PopupNavigation.Instance.
                                   PushAsync(new BasePopup(title, content, IconPosititon.None, PopupType.Yes,
                                   null, null, null, errorTextColor, null, errorBtnColor, errorBtnTextColor));
        }

        public async Task ShowNotificationIconPopup(string title, string content, string iconImageSource, Color iconColor, IconPosititon iconPosititon)
        {
            await PopupNavigation.Instance.
                                   PushAsync(new BasePopup(title, content, iconPosititon, PopupType.YesNo, iconImageSource,
                                   null, null, null, iconColor, yesBtnBackgroundColor, yesBtnTextColor));
        }

        public async Task ShowNotificatonPopup(string title, string content)
        {
            await PopupNavigation.Instance.
                                  PushAsync(new BasePopup(title, content, IconPosititon.None, PopupType.Yes,null,null,null,null,null,
                                  yesBtnBackgroundColor,yesBtnTextColor));
        }
    }
}