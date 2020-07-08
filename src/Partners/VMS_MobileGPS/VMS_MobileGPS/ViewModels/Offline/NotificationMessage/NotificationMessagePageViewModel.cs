using BA_MobileGPS.Models;
using BA_MobileGPS.Service;

using Prism.Navigation;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace VMS_MobileGPS.ViewModels
{
    public class NotificationMessageViewModel : VMSBaseViewModel
    {
        private readonly IMessageService messageService;

        private IList<MessageSOS> listMessage = new ObservableCollection<MessageSOS>();
        public IList<MessageSOS> ListMessage { get => listMessage; set => SetProperty(ref listMessage, value); }

        public ICommand ViewDetailCommand { get; private set; }

        public NotificationMessageViewModel(INavigationService navigationService,
            IMessageService messageService)
            : base(navigationService)
        {
            this.messageService = messageService;

            ViewDetailCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ViewDetail);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var messages = messageService.GetListMessage().FindAll(m => string.IsNullOrWhiteSpace(m.Receiver));

            if (messages != null && messages.ToList().Count > 0)
            {
                ListMessage = messages.OrderByDescending(m => m.CreatedDate).ToObservableCollection();
            }
        }

        private void ViewDetail(Syncfusion.ListView.XForms.ItemTappedEventArgs args)
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("PopupMessagePage", parameters: new NavigationParameters
                {
                    { "TitlePopup", "Tin nhắn" },
                    { "ContentPopup", ((MessageSOS)args.ItemData).Content },
                    { "TitleButton", "Đóng" }
                });

                //Update sự kiện đã đọc
                var message = messageService.GetMessage(Convert.ToInt64(((MessageSOS)(args.ItemData)).Id));

                message.IsRead = true;

                messageService.UpdateMessage(message);

                if (ListMessage.FirstOrDefault(m => m.Id == message.Id) is MessageSOS old)
                {
                    old.IsRead = true;
                }
            });
        }

        public void DeleteMessage(MessageSOS message)
        {
            SafeExecute(() =>
            {
                messageService.DeleteMessage(message.Id);
                ListMessage.Remove(message);
            });
        }
    }
}