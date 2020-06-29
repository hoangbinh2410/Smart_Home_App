using BA_MobileGPS.Core.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Interfaces
{
    public class PopupServices : IPopupServices
    {
        Color errorTextColor;
        Color errorBtnTextColor;
        Color errorBtnColor;
        Color yesBtnTextColor;
        Color yesBtnBackgroundColor;
        public PopupServices()
        {
            if (Application.Current.RequestedTheme == OSAppTheme.Light || Application.Current.RequestedTheme == OSAppTheme.Unspecified)
            {
                errorTextColor = (Color)Application.Current.Resources["LightErorrPopupTextColor"];
                errorBtnTextColor = (Color)Application.Current.Resources["LightErorrPopupButtonTextColor"];
                errorBtnColor = (Color)Application.Current.Resources["LightErorrPopupButtonBackgroundColor"];
                yesBtnTextColor = (Color)Application.Current.Resources["LightPopupYesBtnTextColor"];
                yesBtnBackgroundColor = (Color)Application.Current.Resources["LightPopupYesBtnBackgroundColor"];
            }
            else
            {
                errorTextColor = (Color)Application.Current.Resources["DarkErorrPopupTextColor"];
                errorBtnTextColor = (Color)Application.Current.Resources["DarkErorrPopupButtonTextColor"];
                errorBtnColor = (Color)Application.Current.Resources["DarkErorrPopupButtonBackgroundColor"];
                yesBtnTextColor = (Color)Application.Current.Resources["DarkPopupYesBtnTextColor"];
                yesBtnBackgroundColor = (Color)Application.Current.Resources["DarkPopupYesBtnBackgroundColor"];
            }
        }
       

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