using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class NotificationPopupViewModel : ViewModelBase
    {
        private readonly INotificationService notificationService;
        public ICommand ViewMoreCommand { get; set; }

        public NotificationPopupViewModel(INavigationService navigationService, INotificationService notificationService) : base(navigationService)
        {
            this.notificationService = notificationService;
            ViewMoreCommand = new Command(OnViewMore);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.ContainsKey(ParameterKey.NotificationKey) && parameters.GetValue<NotificationRespone>(ParameterKey.NotificationKey) is NotificationRespone notice)
            {
                Title = notice.Title;
                GetListNoticeDetail(notice.PK_NoticeContentID);
            }
        }

        public string content;
        public string Content { get => content; set => SetProperty(ref content, value); }

        private void GetListNoticeDetail(int ID)
        {
            RunOnBackground(async () =>
            {
                return await notificationService.GetNotificationBody(ID, Settings.CurrentLanguage);
            },
                  (items) =>
                  {
                      if (items != null && items.Success && items.Data != null)
                      {
                          Content = Content = "<meta name=\"viewport\" content=\"width=device-width,initial-scale=1.0,maximum-scale=1\" />" + items.Data.Body;
                      }
                  });
        }

        private void OnViewMore(object obj)
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("NotificationPage", null);
            });
        }
    }
}