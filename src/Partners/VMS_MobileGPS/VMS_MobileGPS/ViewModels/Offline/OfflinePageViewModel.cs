﻿using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RealmEntity;
using BA_MobileGPS.Models;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using Shiny.Locations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using VMS_MobileGPS.Constant;
using VMS_MobileGPS.Delegates.Shinny;
using VMS_MobileGPS.Events;
using VMS_MobileGPS.Extensions;
using VMS_MobileGPS.Service;
using VMS_MobileGPS.Views.Popup;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace VMS_MobileGPS.ViewModels
{
    public class OfflinePageViewModel : VMSBaseViewModel
    {
        #region Contructor

        private readonly IEventAggregator eventAggregator;
        private readonly IMessageService messageService;
        private readonly IGpsListener _gpsListener;
        private readonly IGpsManager _gpsManager;
        private readonly IDisplayMessage _displayMessage;
        private readonly IVehicleOnlineService vehicleOnlineService;
        private readonly IRealmBaseService<BoundaryRealm, LandmarkResponse> baseRepository;

        public ICommand NavigateToCommand { get; private set; }
        public ICommand ConnectBleCommand { get; private set; }
        private Timer timer;
        private Timer timerGPSTracking;

        public OfflinePageViewModel(INavigationService navigationService,
            IEventAggregator eventAggregator,
            IMessageService messageService,
            IGpsListener gpsListener,
            IGpsManager gpsManager, IDisplayMessage displayMessage,
            IVehicleOnlineService vehicleOnlineService,
            IRealmBaseService<BoundaryRealm, LandmarkResponse> baseRepository)
            : base(navigationService)
        {
            this.messageService = messageService;
            this.eventAggregator = eventAggregator;
            this._gpsListener = gpsListener;
            this._gpsManager = gpsManager;
            this._displayMessage = displayMessage;
            this.vehicleOnlineService = vehicleOnlineService;
            this.baseRepository = baseRepository;

            _gpsListener.OnReadingReceived += OnReadingReceived;

            NavigateToCommand = new Command<PageNames>(NavigateTo);
            ConnectBleCommand = new DelegateCommand(PushBlutoothPage);

            stateDeviceMessage = "Chưa kết nối";
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            //GetMobileVersion();
            SetHeightBox();
            AppManager.BluetoothService.OnReceiveNotificationBLE -= OnReceiveNotificationBLE;
            AppManager.BluetoothService.OnReceiveNotificationBLE += OnReceiveNotificationBLE;
            eventAggregator.GetEvent<StartShinnyEvent>().Subscribe(StartShinyLocation);
            eventAggregator.GetEvent<StopShinyEvent>().Subscribe(StopShinnyLocation);
            GlobalResourcesVMS.Current.PermissionManager = new PermissionManager();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsConnectBLE = GlobalResourcesVMS.Current.DeviceManager.State == BleConnectionState.NO_CONNECTION ? false : true;
            if (IsConnectBLE && StateDevice.Count == 0)
            {
                StateDeviceMessage = "Đã kết nối";
            }
            else if (!IsConnectBLE)
            {
                StateDeviceMessage = "Chưa kết nối";
                StateDevice = new Dictionary<string, string>();
            }
            GetCountMessage();
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
            SafeExecute(async () =>
            {
                StartTimmerStateDevice();

                if (!Settings.IsLoadedMap)
                {
                    var result = await NavigationService.NavigateAsync("OffMap");
                    //Settings.IsLoadedMap = true;
                }

                var temp = GlobalResourcesVMS.Current.PermissionManager;
                if (!temp.IsCameraGranted || !temp.IsLocationGranted || !temp.IsPhotoGranted || !temp.IsStorageGranted)
                {
                    ShowPermistionPage();
                }
                GetListLandmark();
            });
        }

        public override void OnDestroy()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                timer.Stop();
                timer.Dispose();
            }
            else
            {
                StopTimerGPSTracking();
            }
            _gpsListener.OnReadingReceived -= OnReadingReceived;
            AppManager.BluetoothService.OnReceiveNotificationBLE -= OnReceiveNotificationBLE;
            eventAggregator.GetEvent<StartShinnyEvent>().Unsubscribe(StartShinyLocation);
            eventAggregator.GetEvent<StopShinyEvent>().Unsubscribe(StopShinnyLocation);
        }

        public override void OnResume()
        {
            base.OnResume();
            if (Device.RuntimePlatform == Device.Android)
            {
                var count = PopupNavigation.Instance.PopupStack.Count;
                if (count > 0)
                {
                    PopupNavigation.Instance.PopAllAsync();
                    GlobalResourcesVMS.Current.PermissionManager = new PermissionManager();
                    var temp = GlobalResourcesVMS.Current.PermissionManager;
                    if (!temp.IsCameraGranted || !temp.IsLocationGranted || !temp.IsPhotoGranted || !temp.IsStorageGranted)
                    {
                        ShowPermistionPage();
                    }
                }
            }
        }

        #endregion Lifecycle

        #region Property

        private bool isConnectBLE = GlobalResourcesVMS.Current.DeviceManager.State == BleConnectionState.NO_CONNECTION ? false : true;
        public bool IsConnectBLE { get => isConnectBLE; set => SetProperty(ref isConnectBLE, value); }

        private string stateDeviceMessage = string.Empty;
        public string StateDeviceMessage { get => stateDeviceMessage; set => SetProperty(ref stateDeviceMessage, value); }
        private Dictionary<string, string> StateDevice = new Dictionary<string, string>();

        private double heightbox = 300;
        public double HeightBox { get => heightbox; set => SetProperty(ref heightbox, value); }

        #endregion Property

        #region PrivateMethod

        private async void SendRequestStateDevice()
        {
            string str = string.Format("GCFG,{0}", "999");

            Debug.WriteLine(str);

            var ret = await AppManager.BluetoothService.Send(str);
        }

        private void StartShinyLocation()
        {
            TryExecute(async () =>
            {
                await PermissionHelper.CheckLocationAsync(() =>
                {
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        var platform = DeviceInfo.Version;
                        if (platform.Major >= 9)
                        {
                            ShinnyStart();

                            GetMyLocation();
                        }
                        else GPSTrack();
                    }
                    else
                    {
                        ShinnyStart();
                    }
                });
            });
        }

        private void ShinnyStart()
        {
            TryExecute(() =>
            {
                if (!_gpsManager.IsListening)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await _gpsManager.StartListener(new GpsRequest
                        {
                            UseBackground = false,
                            Priority = GpsPriority.Highest,
                            Interval = TimeSpan.FromSeconds(50),
                            ThrottledInterval = TimeSpan.FromSeconds(40) //Should be lower than Interval
                        });
                    });
                }
            });
        }

        private void GPSTrack()
        {
            if (timerGPSTracking == null)
            {
                timerGPSTracking = new Timer();
                timerGPSTracking.Interval = 50000;
                timerGPSTracking.Elapsed += GPSTrackingTimmer; ;

                timerGPSTracking.Start();
            }
            GetMyLocation();
        }

        private void GPSTrackingTimmer(object sender, ElapsedEventArgs e)
        {
            GetMyLocation();
        }

        private void GetMyLocation()
        {
            TryExecute(async () =>
            {
                var location = await Geolocation.GetLocationAsync();
                if (location != null)
                {
                    var data = new RecieveLocation()
                    {
                        GPSTime = DateTime.Now,
                        Lat = location.Latitude,
                        Lng = location.Longitude,
                        Velocity = 0
                    };
                    eventAggregator.GetEvent<RecieveLocationEvent>().Publish(data);
                }
            });
        }

        private void StopShinnyLocation()
        {
            TryExecute(async () =>
            {
                if (_gpsManager.IsListening)
                {
                    await _gpsManager.StopListener();
                }
                StopTimerGPSTracking();
            });
        }

        private void StopTimerGPSTracking()
        {
            if (timerGPSTracking != null)
            {
                timerGPSTracking.Stop();
                timerGPSTracking.Dispose();
            }
        }

        private void OnReadingReceived(object sender, GpsReadingEventArgs e)
        {
            if (GlobalResourcesVMS.Current.DeviceManager.State == Service.BleConnectionState.NO_CONNECTION)
            {
                int temp;
                try
                {
                    temp = Convert.ToInt32(e.Reading.Speed * 3.6);
                    if (temp < 7)
                        temp = 0;
                }
                catch (Exception)
                {
                    temp = 0;
                }
                var data = new RecieveLocation()
                {
                    GPSTime = DateTime.Now,
                    Lat = e.Reading.Position.Latitude,
                    Lng = e.Reading.Position.Longitude,
                    Velocity = temp
                };
                eventAggregator.GetEvent<RecieveLocationEvent>().Publish(data);
            }
        }

        private void OnReceiveNotificationBLE(object sender, string Msg)
        {
            TryExecute(() =>
            {
                Debug.WriteLine("MSG_Device:" + Msg);
                LoggerHelper.WriteLog(GlobalResourcesVMS.Current.DeviceManager.DevicePlate, Msg);
                if (string.IsNullOrEmpty(Msg))
                    return;

                if (Msg.StartsWith("Cau hinh 997: SMS_LOC"))
                {
                    Debug.WriteLine(Msg);
                    ReciveDataLocation(Msg);
                }
                if (Msg.StartsWith("Cau hinh 999:"))
                {
                    Debug.WriteLine(Msg);
                    ReciveDataStateDevice(Msg);
                }
                if (Msg.StartsWith("SEND_SMS"))
                {
                    Debug.WriteLine(Msg);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplaySendMessageResponse(Msg);
                    });
                }
                if (Msg.StartsWith("Cau hinh 22: GSM"))
                {
                    var result = Msg.Remove(0, Msg.IndexOf(':') + 1);

                    string gsm = result.Remove(0, result.IndexOf(':') + 1);

                    string imei = gsm.Substring(0, gsm.IndexOf(','));

                    Debug.WriteLine(Msg + ":" + imei);
                    Settings.LastImeiVMS = imei.Trim();
                }
                else if (Msg.StartsWith("RECEIVE_SMS"))
                {
                    Debug.WriteLine(Msg);
                    AddMessage(Msg);
                }
                else if (Msg.StartsWith("Cau hinh 310: SMS DATA"))
                {
                    var result = Msg.Remove(0, Msg.IndexOf(':') + 1).Trim();
                    string totalByte = result.Remove(0, result.IndexOf(':') + 1).Trim();
                    GlobalResourcesVMS.Current.TotalByteSms = int.Parse(totalByte);
                }
            });
        }

        private void ShowPermistionPage()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await PopupNavigation.Instance.PushAsync(new VMSPermissionGranted());
            });
        }

        private void NavigateTo(PageNames args)
        {
            SafeExecute(async () =>
            {
                INavigationResult result;

                if (PageNames.LoginPage == args)
                {
                    //Nếu cài app lần đầu tiên hoặc có sự thay đổi dữ liệu trên server thì sẽ vào trang cập nhật thông tin vào localDB
                    if (!Settings.IsFistInstallApp || Settings.IsChangeDataLocalDB)
                    {
                        result = await NavigationService.NavigateAsync("/InsertLocalDBPage");
                    }
                    else
                    {
                        result = await NavigationService.NavigateAsync("/LoginPage");
                    }
                }
                else if (PageNames.SOSPage == args || PageNames.MessagesPage == args)
                {
                    if (GlobalResourcesVMS.Current.DeviceManager.State == Service.BleConnectionState.NO_CONNECTION)
                    {
                        if (!await PageDialog.DisplayAlertAsync("Cảnh báo", "Bạn chưa kết nối thiết bị. Bạn có muốn kết nối thiết bị?", "ĐỒNG Ý", "BỎ QUA"))
                        {
                            return;
                        }
                        result = await NavigationService.NavigateAsync("NavigationPage/BluetoothPage", null, useModalNavigation: true, true);
                    }
                    else
                    {
                        result = await NavigationService.NavigateAsync(args.ToString());
                    }
                }
                else if (PageNames.OffMap == args)
                {
                    result = await NavigationService.NavigateAsync(args.ToString());
                }
                else
                {
                    result = await NavigationService.NavigateAsync(args.ToString());
                }
            });
        }

        private void PushBlutoothPage()
        {
            TryExecute(async () =>
            {
                IsConnectBLE = !IsConnectBLE;

                if (IsConnectBLE)
                {
                    _ = await NavigationService.NavigateAsync("NavigationPage/BluetoothPage", null, useModalNavigation: true, true);
                }
                else
                {
                    if (await PageDialog.DisplayAlertAsync("Cảnh báo", "Bạn có muốn ngắt kết nối thiết bị không?", "ĐỒNG Ý", "BỎ QUA"))
                    {
                        await AppManager.BluetoothService.Disconnect();
                        StateDevice = new Dictionary<string, string>();
                        StateDeviceMessage = "Chưa kết nối";
                    }
                    else
                    {
                        IsConnectBLE = true;
                    }
                }
            });
        }

        private void StartTimmerStateDevice()
        {
            timer = new Timer
            {
                Interval = 50000
            };
            timer.Elapsed += TimmerStateDevice;

            timer.Start();
        }

        private void TimmerStateDevice(object sender, ElapsedEventArgs e)
        {
            if (AppManager.BluetoothService.State == BleConnectionState.NO_CONNECTION)
            {
                GlobalResourcesVMS.Current.DeviceManager.State = BleConnectionState.NO_CONNECTION;
                GlobalResourcesVMS.Current.DeviceManager.DeviceName = string.Empty;
                GlobalResourcesVMS.Current.DeviceManager.DevicePlate = string.Empty;
            }
            else
            {
                SendRequestStateDevice();
            }
        }

        private void DisplaySendMessageResponse(string Msg)
        {
            var result = Msg.Remove(0, Msg.IndexOf(':') + 1);
            result = result.Remove(0, result.IndexOf(':') + 1);
            result = result.Remove(0, result.IndexOf(':') + 1);
            string status = result.Substring(0, result.IndexOf(':')).Trim();
            switch (status)
            {
                case "OK":
                    OnSendMessageSuccess(Msg);
                    break;

                case "INVALID":
                    //UserDialogs.Instance.Alert("SĐT không hợp lệ (độ dài < 3)", "", "Đồng ý");
                    break;

                case "BUSY":
                    //UserDialogs.Instance.Alert("Gửi tin nhắn không thành công bạn vui lòng kiểm tra lại", "", "Đồng ý");
                    break;

                case "SYNTAX":
                    //UserDialogs.Instance.Alert("Gửi tin nhắn không thành công bạn vui lòng kiểm tra lại", "", "Đồng ý");
                    break;

                case "SUCCESS":
                    OnSendMessageSuccess(Msg);
                    break;

                case "FAIL":
                    //UserDialogs.Instance.Alert("Gửi tin nhắn không thành công bạn vui lòng kiểm tra lại", "", "Đồng ý");
                    break;
            }

            var totalByte = result.Remove(0, result.IndexOf(':') + 1).Trim();

            Debug.WriteLine(totalByte);

            GlobalResourcesVMS.Current.TotalByteSms = int.Parse(totalByte);
        }

        private void OnSendMessageSuccess(string Msg)
        {
            TryExecute(() =>
            {
                var result = Msg.Remove(0, Msg.IndexOf(':') + 1);
                result = result.Remove(0, result.IndexOf(':') + 1);
                string id = result.Substring(0, result.IndexOf(':'));

                var message = messageService.GetMessage(Convert.ToInt64(id));
                if (message != null)
                {
                    message.IsSend = true;

                    messageService.UpdateMessage(message);
                }
                eventAggregator.GetEvent<SendMessageEvent>().Publish(message);
            });
        }

        private async void AddMessage(string Msg)
        {
            var result = Msg.Remove(0, Msg.IndexOf(':') + 1);

            string id = result.Substring(0, result.IndexOf(':'));

            string content = result.Remove(0, result.IndexOf(':') + 1);

            Debug.WriteLine(string.Format("RECEIVE_SMS:{0}", id));

            await AppManager.BluetoothService.Send(string.Format("RECEIVE_SMS:{0}", id));

            Device.BeginInvokeOnMainThread(() =>
            {
                DateTime messageTime = DateTime.Now;
                string messageContent = content;

                if (content.Length > 1)
                {
                    var time = content.Substring(1, content.IndexOf(']') - 1);
                    messageContent = content.Remove(0, content.IndexOf(']') + 1);

                    if (DateTime.TryParse(time, out DateTime dt))
                    {
                        messageTime = dt;
                    }
                }
                var model = messageService.FindListMessage(messageContent, messageTime).FirstOrDefault();
                //Nếu bản tin PNC gửi về có nội dung và thời gian giống nhau thì là trùng lặp  return luôn
                if (model != null)
                {
                    return;
                }
                else
                {
                    var message = new MessageSOS
                    {
                        Receiver = "",
                        Content = messageContent,
                        CreatedDate = messageTime.ToUniversalTime()
                    };
                    messageService.SaveMessage(message, out message);
                    eventAggregator.GetEvent<RecieveMessageEvent>().Publish(message);
                    TryExecute(() =>
                    {
                        _displayMessage.ShowMessageInfo(message.Content, 10000);
                    });

                    GlobalResourcesVMS.Current.TotalMessage = messageService.GetListMessage("").Count(m => m.IsRead == false);
                }
            });
        }

        private void ReciveDataLocation(string msg)
        {
            var data = msg.Remove(0, msg.IndexOf(':') + 1).Trim();
            var result = data.Remove(0, data.IndexOf(':') + 1).Trim();
            var lst = result.Split('+');
            if (lst != null && lst.Length == 4)
            {
                string time = lst[0].Trim();
                string lng = lst[1].Trim();
                string lat = lst[2].Trim();
                string velocity = lst[3].Trim();

                var date = time.Substring(0, time.IndexOf('-'));
                var hour = time.Remove(0, time.IndexOf('-') + 1);
                var datetime = DateTimeHelper.ToDateTime(date + " " + hour, '/');

                var datasend = new RecieveLocation()
                {
                    GPSTime = datetime,
                    Lat = Convert.ToDouble(lat, CultureInfo.InvariantCulture),
                    Lng = Convert.ToDouble(lng, CultureInfo.InvariantCulture),
                    Velocity = int.Parse(velocity)
                };
                eventAggregator.GetEvent<RecieveLocationEvent>().Publish(datasend);
            }
        }

        private void ReciveDataStateDevice(string msg)
        {
            var data = msg.Remove(0, msg.IndexOf(':') + 1).Trim();
            var result = data.Remove(0, data.IndexOf('-') + 1).Trim();
            var lst = result.Split(',');
            if (lst != null && lst.Length > 0)
            {
                for (int i = 0; i < lst.Length; i++)
                {
                    var key = lst[i].Substring(0, lst[i].IndexOf(':')).Trim().ToUpper();
                    var value = lst[i].Remove(0, lst[i].IndexOf(':') + 1).Trim().ToUpper();
                    StateDevice.Add(key, value);
                }

                StateDeviceMessage = StateDeviceExtension.StateDevice(StateDevice);
            }
        }

        private void GetCountMessage()
        {
            TryExecute(() =>
            {
                var listmess = messageService.GetListMessage("");
                if (listmess != null && listmess.Count > 0)
                {
                    GlobalResourcesVMS.Current.TotalMessage = listmess.Count(m => m.IsRead == false);
                }
            });
        }

        private void GetListLandmark()
        {
            if (!IsConnected)
            {
                return;
            }
            TryExecute(() =>
            {
                //lấy từ local DB
                var list = baseRepository.All()?.ToList();
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        baseRepository.Delete(item.Id);
                    }
                }
                RunOnBackground(async () =>
                {
                    return await vehicleOnlineService.GetListBoundary();
                },
                    (respones) =>
                    {
                        if (respones != null && respones.Count > 0)
                        {
                            foreach (var item in respones)
                            {
                                //thêm dữ liệu vào local database với bẳng là LanmarkReal
                                baseRepository.Add(item);
                            }
                        }
                    });
            });
        }

        private void SetHeightBox()
        {
            var widthdevice = DeviceDisplay.MainDisplayInfo.Width;

            // ảnh 48 chữ 12 hhoang cach 45

            HeightBox = (widthdevice / 2) - 105;
        }

        #endregion PrivateMethod
    }
}