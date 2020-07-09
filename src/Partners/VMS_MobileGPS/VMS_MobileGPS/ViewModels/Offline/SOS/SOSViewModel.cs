using BA_MobileGPS.Core;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using VMS_MobileGPS.Constant;
using VMS_MobileGPS.Service;

using Xamarin.Essentials;
using Xamarin.Forms.Extensions;

namespace VMS_MobileGPS.ViewModels
{
    public class SOSViewModel : VMSBaseViewModel
    {
        private readonly IAudioManager audioManager;
        private readonly ISOSHistoryService sOSHistoryService;

        private readonly IBluetoothService bluetoothService;

        public ICommand SendSOSCommand { get; private set; }
        public ICommand OffSOSCommand { get; private set; }
        public ICommand DeleteHistoryCommand { get; private set; }

        public SOSViewModel(INavigationService navigationService, IAudioManager audioManager,
            ISOSHistoryService sOSHistoryService)
            : base(navigationService)
        {
            this.sOSHistoryService = sOSHistoryService;
            this.audioManager = audioManager;

            bluetoothService = AppManager.BluetoothService;

            SendSOSCommand = new DelegateCommand(SendSOS);

            OffSOSCommand = new DelegateCommand(OffSOS);
            DeleteHistoryCommand = new DelegateCommand(DeleteAllSOSHistory);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();

            GETSOSHistory();
        }

        private SOSHistory sOSActive;

        public SOSHistory SOSActive
        {
            get { return sOSActive; }
            set => SetProperty(ref sOSActive, value);
        }

        private ObservableCollection<SOSHistory> listSOSHistory = new ObservableCollection<SOSHistory>();
        public ObservableCollection<SOSHistory> ListSOSHistory { get => listSOSHistory; set => SetProperty(ref listSOSHistory, value); }

        private void SendSOS()
        {
            SafeExecute(async () =>
            {
                if (AppManager.BluetoothService.State == Service.BleConnectionState.NO_CONNECTION)
                {
                    if (await PageDialog.DisplayAlertAsync("Cảnh báo", "Bạn chưa kết nối thiết bị. Bạn có muốn kết nối thiết bị?", "ĐỒNG Ý", "BỎ QUA"))
                    {
                        await NavigationService.NavigateAsync(PageNames.BluetoothPage.ToString());
                    }
                    return;
                }
                else
                {
                    var resultSOS = await bluetoothService.Send("SOS:ALARM");
                    if (!resultSOS.Data)
                        await PageDialog.DisplayAlertAsync("Lỗi", resultSOS.Message, "ĐỒNG Ý");
                    else
                    {
                        GlobalResourcesVMS.Current.DeviceManager.IsSendSOS = true;
                        var result = sOSHistoryService.Add(new SOSHistory()
                        {
                            CreatedDate = DateTimeOffset.Now,
                            IsOnSOS = true,
                            DevicePlate = GlobalResourcesVMS.Current.DeviceManager.DevicePlate
                        });
                        if (result != null)
                        {
                            SOSActive = result;

                            GETSOSHistory();
                        }
                        // Or use specified time
                        var duration = TimeSpan.FromSeconds(1);
                        Vibration.Vibrate(duration);
                        await audioManager.PlaySound("z_sos.mp3");
                        await PageDialog.DisplayAlertAsync("Thông báo", resultSOS.Message, "Gửi SOS thành công");
                    }
                }
            });
        }

        private void OffSOS()
        {
            SafeExecute(async () =>
            {
                if (AppManager.BluetoothService.State == Service.BleConnectionState.NO_CONNECTION)
                {
                    if (await PageDialog.DisplayAlertAsync("Bạn chưa kết nối thiết bị. Bạn có muốn kết nối thiết bị?", "Cảnh báo", "ĐỒNG Ý", "BỎ QUA"))
                    {
                        await NavigationService.NavigateAsync(PageNames.BluetoothPage.ToString());
                    }

                    return;
                }
                else
                {
                    var action = await PageDialog.DisplayAlertAsync("Bạn có thực sự muốn tắt SOS không?", "BA SAT", "Đồng ý", "Bỏ qua");
                    if (action)
                    {
                        var resultSOS = await bluetoothService.Send("SOS:OFF");
                        if (!resultSOS.Data)
                            await PageDialog.DisplayAlertAsync("Lỗi", resultSOS.Message, "ĐỒNG Ý");
                        else
                        {
                            GlobalResourcesVMS.Current.DeviceManager.IsSendSOS = false;
                            var model = sOSHistoryService.Get(r => r.Id == SOSActive.Id);
                            if (model != null)
                            {
                                model.IsOnSOS = false;
                                model.UpdateDate = DateTime.Now;
                                sOSHistoryService.Update(model);

                                GETSOSHistory();
                            }
                        }
                    }
                }
            });
        }

        private void GETSOSHistory()
        {
            // get db local
            var result = sOSHistoryService.All().ToList();
            if (result != null && result.Count > 0)
            {
                result.ForEach(x =>
                {
                    if (x.IsOnSOS)
                    {
                        SOSActive = x;
                        GlobalResourcesVMS.Current.DeviceManager.IsSendSOS = true;
                    }
                });
                ListSOSHistory = result.OrderByDescending(x => x.CreatedDate).ToObservableCollection();
            }
            else
            {
                ListSOSHistory = new ObservableCollection<SOSHistory>();
            }
        }

        private void DeleteAllSOSHistory()
        {
            SafeExecute(async () =>
            {
                var action = await PageDialog.DisplayAlertAsync("Thông báo", "Bạn có muốn xóa lịch sử các lần cảnh báo không?", "Đồng ý", "Bỏ qua");
                if (action)
                {
                    var result = sOSHistoryService.All().ToList();
                    if (result != null && result.Count > 0)
                    {
                        foreach (var item in result)
                        {
                            sOSHistoryService.Delete(item.Id);
                        }
                        GETSOSHistory();
                    }
                }
            });
        }
    }
}