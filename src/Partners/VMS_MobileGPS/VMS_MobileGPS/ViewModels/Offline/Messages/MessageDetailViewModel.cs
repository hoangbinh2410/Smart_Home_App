using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Models;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using VMS_MobileGPS.Constant;
using VMS_MobileGPS.Events;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace VMS_MobileGPS.ViewModels
{
    public class MessageDetailViewModel : VMSBaseViewModel
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IMessageService messageService;

        private string receiver = string.Empty;
        public string Receiver { get => receiver; set => SetProperty(ref receiver, value); }

        private MessageSOS message;
        public MessageSOS Message { get => message; set => SetProperty(ref message, value); }

        private IList<MessageSOS> messages = new ObservableCollection<MessageSOS>();
        public IList<MessageSOS> Messages { get => messages; set => SetProperty(ref messages, value); }

        public bool ReceiverVisible => Messages.Count <= 0;

        public ICommand SendMessageCommand { get; private set; }

        public MessageDetailViewModel(INavigationService navigationService, IEventAggregator eventAggregator,
        IMessageService messageService)
            : base(navigationService)
        {
            this.eventAggregator = eventAggregator;
            this.messageService = messageService;

            SendMessageCommand = new Command(SendMessage);

            eventAggregator.GetEvent<SendMessageEvent>().Subscribe(OnSendMessageSuccess);

            Message = new MessageSOS();
            Message.MaxWords = GlobalResourcesVMS.Current.TotalByteSms;
            GlobalResourcesVMS.Current.PropertyChanged -= Current_PropertyChanged;
            GlobalResourcesVMS.Current.PropertyChanged += Current_PropertyChanged;
        }

        private void Current_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TotalByteSms")
            {
                Message.MaxWords = GlobalResourcesVMS.Current.TotalByteSms;
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(ParameterKey.Message, out MessageSOS message))
            {
                Title = message.Receiver;

                Receiver = message.Receiver;

                Messages = messageService.GetListMessage(Receiver).OrderBy(m => m.CreatedDate).ToObservableCollection();
            }
            else
            {
                Title = "Tin nhắn mới";
            }

            GetByteSMS();

            RaisePropertyChanged(nameof(ReceiverVisible));
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            SetFocus();
        }

        public override void OnDestroy()
        {
            eventAggregator.GetEvent<SendMessageEvent>().Unsubscribe(OnSendMessageSuccess);
        }

        private async void SetFocus()
        {
            if (ReceiverVisible)
            {
                if (GetControl<Entry>("txtPhoneNumber") is Entry entry)
                {
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        await Task.Delay(600);
                        entry.Focus();
                    }
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        await Task.Delay(300);
                        entry.Focus();
                    }
                }
            }
        }

        private void OnSendMessageSuccess(MessageSOS message)
        {
            if (Messages.FirstOrDefault(m => m.Id == message.Id) is MessageSOS old)
            {
                old.IsSend = true;
            }
        }

        private bool ValidateReceiver()
        {
            if (!ReceiverVisible)
                return true;

            if (string.IsNullOrWhiteSpace(Receiver))
            {
                PageDialog.DisplayAlertAsync("Thông báo", "Số điện thoại nhận không được trống", "Đóng");
                return false;
            }
            if (!StringHelper.ValidPhoneNumer(Receiver, MobileSettingHelper.LengthAndPrefixNumberPhone))
            {
                PageDialog.DisplayAlertAsync("Thông báo", "Số điện thoại không hợp lệ", "Đóng");
                return false;
            }

            return true;
        }

        private void SendMessage()
        {
            SafeExecute(async () =>
            {
                try
                {
                    if (AppManager.BluetoothService.State == Service.BleConnectionState.NO_CONNECTION)
                    {
                        if (await PageDialog.DisplayAlertAsync("Cảnh báo", "Bạn chưa kết nối thiết bị. Bạn có muốn kết nối hay không?", "Có", "Không"))
                        {
                            await NavigationService.NavigateAsync(PageNames.BluetoothPage.ToString());
                        }

                        return;
                    }
                    if (GlobalResourcesVMS.Current.TotalByteSms <= 0)
                    {
                        var action = await PageDialog.DisplayAlertAsync("Thông báo", "Gói cước của bạn chưa được thiết lập hoặc đã hết dung lượng. Vui lòng liên hệ với kinh doanh hoặc tổng đài 19006464 để được hỗ trợ", "Liên hệ tổng đài", "Bỏ qua");
                        if (action)
                        {
                            PhoneDialer.Open("19006464");
                        }
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(Message.Content) || !ValidateReceiver())
                        return;

                    Message.Receiver = Receiver;
                    Message.CreatedDate = DateTime.Now.ToUniversalTime();

                    if (ReceiverVisible)
                    {
                        if (messageService.GetListMessage(Receiver) is List<MessageSOS> messages && messages.Count > 0)
                        {
                            Messages.AddRange(messages);
                        }
                    }

                    var result = messageService.SaveMessage(Message, out MessageSOS messageInserted);
                    if (result && messageInserted != null)
                    {
                        Messages.Add(messageInserted);

                        var messageSend = StringHelper.ConvertToVn(Message.Content);

                        string str = string.Format("SEND_SMS:{0},{1}({2})", Receiver, messageInserted.Id, messageSend);

                        Debug.WriteLine(str);
                        LoggerHelper.WriteLog(GlobalResourcesVMS.Current.DeviceManager.DevicePlate, str);
                        var ret = await AppManager.BluetoothService.Send(str);

                        if (!ret.Data)
                        {
                            await PageDialog.DisplayAlertAsync("Thông báo", ret.Message, "OK");
                            return;
                        }

                        RaisePropertyChanged(nameof(ReceiverVisible));

                        if (!ReceiverVisible)
                            Title = Receiver;

                        if (GetControl<ScrollView>("ScrollView") is ScrollView scrollView)
                        {
                            TryExecute(() =>
                            {
                                scrollView.ScrollToAsync(((StackLayout)scrollView.Content).Children.Last(), ScrollToPosition.End, false);
                            });
                        }

                        Message = new MessageSOS();
                        Message.MaxWords = GlobalResourcesVMS.Current.TotalByteSms;
                    }
                    else
                    {
                        LoggerHelper.WriteLog(GlobalResourcesVMS.Current.DeviceManager.DevicePlate, "Gửi tin nhắn không thành công bạn vui lòng kiểm tra lại");
                        await PageDialog.DisplayAlertAsync("Thông báo", "Gửi tin nhắn không thành công bạn vui lòng kiểm tra lại", "Đồng ý");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex.Message);
                }
            });
        }

        private async void GetByteSMS()
        {
            await AppManager.BluetoothService.Send("GCFG,310");
        }
    }
}