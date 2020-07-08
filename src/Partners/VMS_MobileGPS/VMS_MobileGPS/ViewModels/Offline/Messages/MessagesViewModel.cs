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
    public class MessagesViewModel : VMSBaseViewModel
    {
        private readonly IMessageService messageService;

        private IList<MessageByUser> listMessageByUser = new ObservableCollection<MessageByUser>();
        public IList<MessageByUser> ListMessageByUser { get => listMessageByUser; set => SetProperty(ref listMessageByUser, value); }

        public ICommand WriteNewMessageCommand { get; private set; }
        public ICommand ViewDetailCommand { get; private set; }

        public MessagesViewModel(INavigationService navigationService, IMessageService messageService)
            : base(navigationService)
        {
            this.messageService = messageService;

            WriteNewMessageCommand = new Command(WriteNewMessage);
            ViewDetailCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ViewDetail);
        }

        private void ViewDetail(Syncfusion.ListView.XForms.ItemTappedEventArgs args)
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync(PageNames.MessageDetailPage.ToString(), parameters: new NavigationParameters
                {
                    { ParameterKey.Message, ((MessageByUser)args.ItemData).LastMessage }
                });
            });
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var messages = messageService.GetListMessage().FindAll(m => !string.IsNullOrWhiteSpace(m.Receiver) && m.IsOnline == false);

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
                await NavigationService.NavigateAsync(PageNames.MessageDetailPage.ToString());
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