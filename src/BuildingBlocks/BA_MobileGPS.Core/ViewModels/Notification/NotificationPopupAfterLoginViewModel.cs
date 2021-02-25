using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using Prism.Navigation;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class NotificationPopupAfterLoginViewModel : ViewModelBase
    {
        private readonly INotificationService notificationService;

        public ICommand SaveCacheCommand { get; set; }

        public ICommand FeedBackCommand { get; set; }

        public ICommand SendFeedbackCommand { get; set; }

        public ICommand CloseFeedbackCommand { get; set; }

        public NotificationPopupAfterLoginViewModel(INavigationService navigationService,
            INotificationService notificationService) : base(navigationService)
        {
            this.notificationService = notificationService;

            AddValidations();

            SaveCacheCommand = new Command(SaveCache);

            FeedBackCommand = new Command(OpenFeedback);

            SendFeedbackCommand = new Command(SendFeedback);

            CloseFeedbackCommand = new Command(CloseFeedback);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.ContainsKey(ParameterKey.NotificationKey) && parameters.GetValue<NoticeDetailRespone>(ParameterKey.NotificationKey) is NoticeDetailRespone notice)
            {
                GetNoticePopupAfterLogin(notice);
            }
        }

        public int pk_NoticeContentID { get; set; }

        private string name;
        public string Name { get => name; set => SetProperty(ref name, value); }

        private string content;
        public string Content { get => content; set => SetProperty(ref content, value); }

        private bool isShowFeedback;
        public bool IsShowFeedback { get => isShowFeedback; set => SetProperty(ref isShowFeedback, value); }

        private bool isFormFeedback;
        public bool IsFormFeedback { get => isFormFeedback; set => SetProperty(ref isFormFeedback, value); }

        private bool isFormContent;
        public bool IsFormContent { get => isFormContent; set => SetProperty(ref isFormContent, value); }

        private ValidatableObject<string> contentFeedback = new ValidatableObject<string>();

        public ValidatableObject<string> ContentFeedback { get => contentFeedback; set => SetProperty(ref contentFeedback, value); }

        /// <summary>Lấy thông tin body của thông báo</summary>
        /// <param name="notice">The notice.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/26/2020   created
        /// </Modified>
        private void GetNoticePopupAfterLogin(NoticeDetailRespone notice)
        {
            SafeExecute(() =>
            {
                RunOnBackground(async () =>
                {
                    return await notificationService.GetNotificationBody(notice.Id);
                },
                 (items) =>
                 {
                     if (items != null && items?.Data != null)
                     {
                         IsFormFeedback = false;
                         IsFormContent = true;
                         if (notice.IsFeedback)
                         {
                             IsShowFeedback = true;
                         }
                         else
                         {
                             IsShowFeedback = false;
                         }
                         pk_NoticeContentID = notice.Id;
                         Name = notice.Title;
                         Content = Content = "<meta name=\"viewport\" content=\"width=device-width,initial-scale=1.0,maximum-scale=1\" />" + items.Data.Body;
                     }
                 });
            });
        }

        /// <summary>Lưu cache và update</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/26/2020   created
        /// </Modified>
        private void SaveCache()
        {
            SafeExecute(async () =>
            {
                Settings.NoticeIdAfterLogin = pk_NoticeContentID;

                UpdateIsReadNotification(pk_NoticeContentID);

                await NavigationService.GoBackAsync(parameters: new NavigationParameters
                {
                    { "IsClosedPopupAfterLogin", true }
                });
            });
        }

        /// <summary>Mở trang phản hồi</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/26/2020   created
        /// </Modified>
        private void OpenFeedback()
        {
            SafeExecute(() =>
            {
                IsFormFeedback = true;
                IsFormContent = false;
            });
        }

        /// <summary>Đóng trang phản hồi</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/26/2020   created
        /// </Modified>
        private void CloseFeedback()
        {
            SafeExecute(() =>
            {
                IsFormFeedback = false;
                IsFormContent = true;
            });
        }

        /// <summary>Updates thông tin người đọc</summary>
        /// <param name="PK_NoticeContentID">The pk notice content identifier.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/26/2020   created
        /// </Modified>
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

        /// <summary>Gửi thông tin phản hồi</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  3/3/2020   created
        /// </Modified>
        private void SendFeedback()
        {
            SafeExecute(() =>
            {
                if (!Validate())
                {
                    return;
                }

                RunOnBackground(async () =>
                {
                    return await notificationService.SendFeedbackNotification(new InsertFeedbackNoticeRequest()
                    {
                        fk_CompanyID = CurrentComanyID,
                        fk_NoticeID = pk_NoticeContentID,
                        culture = Settings.CurrentLanguage,
                        body = ContentFeedback.Value,
                        userId = UserInfo.UserId
                    });
                },
               (items) =>
               {
                   if (items != null && items.Data)
                   {
                       if (items.Data)
                       {
                           Settings.NoticeIdAfterLogin = pk_NoticeContentID;

                           UpdateIsReadNotification(pk_NoticeContentID);

                           PageDialog.DisplayAlertAsync(MobileResource.Feedback_Label_Thanks, MobileResource.Feedback_Notice_SendSuccess,
                                 MobileResource.Common_Button_Close);

                           NavigationService.GoBackAsync(parameters: new NavigationParameters
                            {
                                { "IsClosedPopupAfterLogin", true }
                            });
                       }
                       else
                       {
                           DisplayMessage.ShowMessageError(MobileResource.Feedback_Notice_SendFail);
                       }
                   }
               });
            });
        }

        /// <summary>Thêm trường hợp lỗi từ ngữ (ngôn ngữ không phù hợp)</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  3/3/2020   created
        /// </Modified>
        private void AddValidations()
        {
            ContentFeedback.Validations.Add(new IsNotContains<string>
            {
                ValidationMessage = MobileResource.Feedback_Message_FilterWord,
                Keywords = HostConfigurationHelper.FilterWord.Split(',').Where(k => !string.IsNullOrWhiteSpace(k)).ToArray()
            });
        }

        /// <summary>Bắt lỗi</summary>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  3/3/2020   created
        /// </Modified>
        private bool Validate()
        {
            if (!contentFeedback.Validate())
            {
                return false;
            }

            if (string.IsNullOrEmpty(ContentFeedback.Value))
            {
                DisplayMessage.ShowMessageError(MobileResource.Feedback_Notice_NoEntry);
                return false;
            }
            else
            {
                if (ContentFeedback.Value.Trim().Length == 0)
                {
                    DisplayMessage.ShowMessageError(MobileResource.Feedback_Notice_NoEntry);
                    ContentFeedback.Value = string.Empty;
                    return false;
                }
            }
            return true;
        }
    }
}