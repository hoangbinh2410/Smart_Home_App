using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Models;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Navigation;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace VMS_MobileGPS.ViewModels
{
    public class MessagesOnlineViewModel : VMSBaseViewModel
    {
        private readonly IMessageService messageService;

        private IList<MessageByUser> listMessageByUser = new ObservableCollection<MessageByUser>();
        public IList<MessageByUser> ListMessageByUser { get => listMessageByUser; set => SetProperty(ref listMessageByUser, value); }

        public ICommand WriteNewMessageCommand { get; private set; }
        public ICommand ViewDetailCommand { get; private set; }

        public MessagesOnlineViewModel(INavigationService navigationService, IMessageService messageService)
            : base(navigationService)
        {
            this.messageService = messageService;

            WriteNewMessageCommand = new Command(WriteNewMessage);
            ViewDetailCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ViewDetail);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var messages = messageService.GetListMessage(m => m.IsOnline);

            if (messages != null && messages.ToList().Count > 0)
            {
                var result = new List<MessageByUser>();

                foreach (var mes in messages.GroupBy(mes => mes.Receiver))
                {
                    result.Add(new MessageByUser()
                    {
                        Title = mes.Key,
                        LastMessage = mes.OrderByDescending(m => m.CreatedDate).FirstOrDefault()
                    });
                }
                ListMessageByUser = result.OrderByDescending(m => m.LastMessage.CreatedDate).ToObservableCollection();
            }
        }

        private void WriteNewMessage()
        {
            SafeExecute(async () =>
            {
                var result = await NavigationService.NavigateAsync(PageNames.MessageOnlineDetailPage.ToString());

                if (!result.Success)
                {
                }
            });
        }

        private void ViewDetail(Syncfusion.ListView.XForms.ItemTappedEventArgs args)
        {
            SafeExecute(async () =>
            {
                var result = await NavigationService.NavigateAsync(PageNames.MessageOnlineDetailPage.ToString(), parameters: new NavigationParameters
                {
                    { ParameterKey.Message, ((MessageByUser)args.ItemData).LastMessage }
                });

                if (!result.Success)
                {
                }
            });
        }

        public void DeleteMessage(MessageByUser messageByUser)
        {
            SafeExecute(() =>
            {
                messageService.DeleteMessages(messageByUser.LastMessage.Receiver);
                ListMessageByUser.Remove(messageByUser);
            });
        }
    }
}