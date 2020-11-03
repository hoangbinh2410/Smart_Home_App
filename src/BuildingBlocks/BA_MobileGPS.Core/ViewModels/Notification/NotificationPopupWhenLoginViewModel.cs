using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class NotificationPopupWhenLoginViewModel : ViewModelBase
    {
        private readonly INotificationService notificationService;
        public ICommand SaveCacheCommand { get; set; }

        public NotificationPopupWhenLoginViewModel(INavigationService navigationService,
            INotificationService notificationService) : base(navigationService)
        {
            this.notificationService = notificationService;
            SaveCacheCommand = new Command(SaveCache);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.ContainsKey(ParameterKey.NotificationKey) && parameters.GetValue<NotificationWhenLoginRespone>(ParameterKey.NotificationKey) is NotificationWhenLoginRespone notice)
            {
                GetListNoticeDetail(notice);
            }
            else if (parameters.ContainsKey(ParameterKey.NotificationForm) && parameters.GetValue<string>(ParameterKey.NotificationForm) is string body)
            {
                IsClosePage = true;
                IsSaveCache = false;
                Name = Settings.ReceivedNotificationTitle;
                Content = Content = "<meta name=\"viewport\" content=\"width=device-width,initial-scale=1.0,maximum-scale=1\" />" + body;
            }
        }

        public int pk_NoticeContentID { get; set; }

        private string name;
        public string Name { get => name; set => SetProperty(ref name, value); }

        private string content;
        public string Content { get => content; set => SetProperty(ref content, value); }

        private bool isClosePage;
        public bool IsClosePage { get => isClosePage; set => SetProperty(ref isClosePage, value); }

        private bool isSaveCache;
        public bool IsSaveCache { get => isSaveCache; set => SetProperty(ref isSaveCache, value); }

        /// <summary>Lấy thông tin body của thông báo</summary>
        /// <param name="notice">The notice.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/26/2020   created
        /// </Modified>
        private void GetListNoticeDetail(NotificationWhenLoginRespone notice)
        {
            RunOnBackground(async () =>
            {
                return await notificationService.GetNotificationBody(notice.PK_NoticeContentID, Settings.CurrentLanguage);
            },
                 (items) =>
                 {
                     if (items != null && items.Success && items.Data != null)
                     {
                         if (notice.IsAlwayShow)
                         {
                             IsClosePage = true;
                             IsSaveCache = false;
                         }
                         else
                         {
                             IsClosePage = false;
                             IsSaveCache = true;
                         }
                         pk_NoticeContentID = notice.PK_NoticeContentID;
                         Name = notice.Title;
                         Content = Content = "<meta name=\"viewport\" content=\"width=device-width,initial-scale=1.0,maximum-scale=1\" />" + items.Data.Body;
                     }
                 });
        }

        /// <summary>Lưu cache</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/26/2020   created
        /// </Modified>
        private void SaveCache()
        {
            SafeExecute(async () =>
            {
                Settings.NoticeIdWhenLogin = pk_NoticeContentID;

                await NavigationService.GoBackAsync(null,useModalNavigation: true,true);
            });
        }
    }
}