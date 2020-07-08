using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Models;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Navigation;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using VMS_MobileGPS.Constant;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace VMS_MobileGPS.ViewModels
{
    public class MessageOnlineDetailViewModel : ViewModelBase
    {
        private readonly IMessageService messageService;

        private string receiver = string.Empty;
        public string Receiver { get => receiver; set => SetProperty(ref receiver, value.ToUpper()); }

        private MessageSOS message;
        public MessageSOS Message { get => message; set => SetProperty(ref message, value); }

        private IList<MessageSOS> messages = new ObservableCollection<MessageSOS>();
        public IList<MessageSOS> Messages { get => messages; set => SetProperty(ref messages, value); }

        public bool ReceiverVisible => Messages.Count <= 0;

        public ICommand SendMessageCommand { get; private set; }
        public ICommand SendIfFailCommand { get; private set; }

        public MessageOnlineDetailViewModel(INavigationService navigationService,
            IMessageService messageService)
            : base(navigationService)
        {
            this.messageService = messageService;

            SendMessageCommand = new Command(SendMessage);
            SendIfFailCommand = new Command<MessageSOS>(SendIfFail);

            Message = new MessageSOS();
            Message.MaxWords = GlobalResourcesVMS.Current.TotalByteSms;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            GetPackageDataByte();

            if (parameters.TryGetValue(ParameterKey.Message, out MessageSOS message))
            {
                Title = message.Receiver;

                Receiver = message.Receiver;

                GetPackageDataByte();

                Messages = messageService.GetListMessage(Receiver).OrderBy(m => m.CreatedDate).ToObservableCollection();
            }
            else
            {
                Title = "Tin nhắn mới";
            }

            RaisePropertyChanged(nameof(ReceiverVisible));
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehiclePlate)
            {
                Receiver = vehiclePlate.VehiclePlate;

                GetPackageDataByte();
            }
        }

        private bool ValidateReceiver()
        {
            if (!ReceiverVisible)
                return true;

            if (string.IsNullOrWhiteSpace(Receiver))
            {
                PageDialog.DisplayAlertAsync("Người nhận không được trống", "", "Đóng");
                return false;
            }

            return true;
        }

        private void SendMessage()
        {
            SafeExecute(async () =>
            {
                if (GlobalResourcesVMS.Current.TotalByteSms <= 0)
                {
                    var action = await PageDialog.DisplayAlertAsync("Thông báo", string.Format("Quý khách vui lòng kích hoạt gói cước để thực hiện gửi tin nhắn. Liên hệ kinh doanh hoặc tổng đài {0} để được trợ giúp", MobileSettingHelper.HotlineGps), "Liên hệ tổng đài", "Bỏ qua");
                    if (action)
                    {
                        if (!string.IsNullOrEmpty(MobileSettingHelper.HotlineGps))
                        {
                            PhoneDialer.Open(MobileSettingHelper.HotlineGps);
                        }
                    }

                    return;
                }

                if (string.IsNullOrWhiteSpace(Message.Content) || !ValidateReceiver())
                    return;

                Message.Receiver = Receiver ?? "".ToUpper();
                Message.CreatedDate = DateTime.Now.ToUniversalTime();
                Message.IsOnline = true;

                if (ReceiverVisible)
                {
                    if (messageService.GetListMessage(Receiver) is List<MessageSOS> messages && messages.Count > 0)
                    {
                        Messages.AddRange(messages);
                    }
                }

                messageService.SaveMessage(Message, out MessageSOS messageInserted);
                Messages.Add(messageInserted);

                if (!ReceiverVisible)
                    Title = Receiver;

                RaisePropertyChanged(nameof(ReceiverVisible));

                message.Id = messageInserted.Id;
                SendIfFail(Message);

                Message = new MessageSOS();
                Message.MaxWords = GlobalResourcesVMS.Current.TotalByteSms;

                if (GetControl<ScrollView>("ScrollView") is ScrollView scrollView)
                {
                    TryExecute(() =>
                    {
                        scrollView.ScrollToAsync(((StackLayout)scrollView.Content).Children.Last(), ScrollToPosition.End, false);
                    });
                }
            });
        }

        private void SendIfFail(MessageSOS args)
        {
            if (args.IsSend || args.IsSending)
                return;

            RunOnBackground(async () =>
            {
                args.Content = StringHelper.ConvertToVn(args.Content);
                args.IsSending = true;
                return await messageService.SendMessage(args);
            },
            (result) =>
            {
                if (result)
                {
                    GetPackageDataByte();

                    if (Messages.FirstOrDefault(m => m.Id == args.Id) is MessageSOS old)
                    {
                        old.IsSend = true;
                        old.IsSending = false;
                        messageService.UpdateMessage(old);
                    }
                }
                else
                {
                    if (Messages.FirstOrDefault(m => m.Id == args.Id) is MessageSOS old)
                    {
                        old.IsSending = false;
                        messageService.UpdateMessage(old);
                    }
                    PageDialog.DisplayAlertAsync("Gửi tin nhắn không thành công bạn vui lòng kiểm tra lại", "", "Đồng ý");
                    LoggerHelper.WriteLog(Receiver, "Gửi tin nhắn không thành công bạn vui lòng kiểm tra lại");
                }
            });
        }

        private void GetPackageDataByte()
        {
            if (!string.IsNullOrEmpty(Receiver))
            {
                RunOnBackground(async () =>
                {
                    return await messageService.GetPackageDataByte(new BA_MobileGPS.Entities.SmsPackageRequest()
                    {
                        XNCode = UserInfo.XNCode.ToString(),
                        VehiclePlate = Receiver
                    });
                }, (result) =>
                {
                    if (result != null)
                    {
                        if (result.Success)
                        {
                            GlobalResourcesVMS.Current.TotalByteSms = result.Data.LeftData;
                            Message.MaxWords = result.Data.LeftData;
                        }
                        else
                        {
                            GlobalResourcesVMS.Current.TotalByteSms = 0;
                            Message.MaxWords = 0;
                        }
                    }
                });
            }
        }
    }
}