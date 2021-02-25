using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using Prism.Navigation;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class NotificationDetailPageViewModel : ViewModelBase
    {
        private readonly INotificationService notificationService;
        public ICommand DowloadFileCommand { get; set; }

        public NotificationDetailPageViewModel(INavigationService navigationService, INotificationService notificationService) : base(navigationService)
        {
            this.notificationService = notificationService;
            DowloadFileCommand = new Command<string>(DowloadFile);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.ContainsKey(ParameterKey.NotificationKey) && parameters.TryGetValue(ParameterKey.NotificationKey, out NotificationRespone notice))
            {
                Title = notice.Title;

                if (notice.ListFileAttachs != null && notice.ListFileAttachs.Count > 0)
                {
                    FileAttach = notice.ListFileAttachs.ToObservableCollection();
                }

                GetListNoticeBody(notice);
            }
            else if (parameters.ContainsKey(ParameterKey.NotificationKey) && parameters.TryGetValue(ParameterKey.NotificationKey, out int PK_NoticeContentID))
            {
                GetListNoticeDetail(PK_NoticeContentID);
            }
        }

        public string content;
        public string Content { get => content; set => SetProperty(ref content, value); }

        public ObservableCollection<string> fileAttach;
        public ObservableCollection<string> FileAttach { get => fileAttach; set => SetProperty(ref fileAttach, value); }

        private void GetListNoticeBody(NotificationRespone notice)
        {
            RunOnBackground(async () =>
            {
                return await notificationService.GetNotificationBody(notice.Id);
            },
                  (items) =>
                  {
                      if (items != null  && items.Data != null)
                      {
                          if (!string.IsNullOrEmpty(items.Data.Body))
                          {
                              Content = "<meta name=\"viewport\" content=\"width=device-width,initial-scale=1.0,maximum-scale=1\" />" + items.Data.Body;
                          }
                          else
                          {
                              Content = string.Empty;

                              Device.BeginInvokeOnMainThread(async () =>
                              {
                                  await Launcher.OpenAsync(new Uri(notice.Linkview));

                                  await NavigationService.GoBackAsync();
                              });
                          }
                      }
                  });
        }

        private void GetListNoticeDetail(int PK_NoticeContentID)
        {
            RunOnBackground(async () =>
            {
                return await notificationService.GetNotificationDetail(PK_NoticeContentID);
            },
                  (items) =>
                  {
                      if (items != null && items.Data != null)
                      {
                          var notice = items.Data;
                          Title = notice.Title;

                          if (notice.ListFileAttachs != null && notice.ListFileAttachs.Count > 0)
                          {
                              FileAttach = notice.ListFileAttachs.ToObservableCollection();
                          }

                          if (!string.IsNullOrEmpty(notice.Body))
                          {
                              Content = "<meta name=\"viewport\" content=\"width=device-width,initial-scale=1.0,maximum-scale=1\" />" + notice.Body;
                          }
                          else
                          {
                              Content = string.Empty;

                              Device.BeginInvokeOnMainThread(async () =>
                              {
                                  await Launcher.OpenAsync(new Uri(items.Data.Linkview));

                                  await NavigationService.GoBackAsync();
                              });
                          }

                          UpdateIsReadNotification(notice.Id);
                      }
                  });
        }

        private async void DowloadFile(string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                await Launcher.OpenAsync(new Uri(obj));
            }
            else
            {
                DisplayMessage.ShowMessageInfo("Không có tệp nào");
            }
        }

        public void UpdateIsReadNotification(int PK_NoticeContentID)
        {
            RunOnBackground(async () =>
            {
                return await notificationService.UpdateIsReadNotification(new UpdateIsReadRequest()
                {
                    fk_NoticeContentID = PK_NoticeContentID,
                    userId = UserInfo.UserId
                });
            },
                 (items) =>
                 {
                     if (items != null && items.Data)
                     {
                     }
                 });
        }
    }
}