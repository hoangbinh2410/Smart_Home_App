using BA_MobileGPS.Core.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Interfaces
{
    public interface IPopupServices
    {
        Task ShowErrorPopup(string title, string content);

        Task ShowErrorIconPopup(string title, string content, string iconImageSource, Color iconColor, IconPosititon iconPosititon);

        Task ShowNotificatonPopup(string title, string content);

        Task ShowNotificationIconPopup(string title, string content, string iconImageSource, Color iconColor, IconPosititon iconPosititon);

        Task ShowConfirmPopup(string title, string content, Action<bool> callback = null);

        Task ShowConfirmIconPopup(string title, string content, string iconImageSource, Color iconColor, IconPosititon iconPosititon, Action<bool> callback = null);
    }
}