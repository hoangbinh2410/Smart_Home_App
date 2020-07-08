using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Delegates.Shinny;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Models;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Plugin.Toasts;
using Prism.Events;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using Shiny.Locations;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using VMS_MobileGPS.Constant;
using VMS_MobileGPS.Events;
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
        private readonly IDBVersionService dBVersionService;
        private readonly IAppVersionService appVersionService;
        private readonly IGpsListener _gpsListener;
        private readonly IGpsManager _gpsManager;

        public ICommand NavigateToCommand { get; private set; }
        private Timer timer;
        private Timer timerGPSTracking;

        public OfflinePageViewModel(INavigationService navigationService,
            IEventAggregator eventAggregator,
            IDBVersionService dBVersionService,
            IAppVersionService appVersionService,
            IMessageService messageService,
            IGpsListener gpsListener,
            IGpsManager gpsManager)
            : base(navigationService)
        {
            this.messageService = messageService;
            this.eventAggregator = eventAggregator;
            this.dBVersionService = dBVersionService;
            this.appVersionService = appVersionService;
            this._gpsListener = gpsListener;
            this._gpsManager = gpsManager;

            _gpsListener.OnReadingReceived += OnReadingReceived;

            NavigateToCommand = new Command<PageNames>(NavigateTo);
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            //GetMobileVersion();
            AppManager.BluetoothService.OnReceiveNotificationBLE -= BluetoothService_OnReceiveNotificationBLE;
            AppManager.BluetoothService.OnReceiveNotificationBLE += BluetoothService_OnReceiveNotificationBLE;
            eventAggregator.GetEvent<StartShinnyEvent>().Subscribe(StartShinyLocation);
            eventAggregator.GetEvent<StopShinyEvent>().Subscribe(StopShinnyLocation);
            GlobalResourcesVMS.Current.PermissionManager = new PermissionManager();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            GetCountMessage();
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();

            if (Device.RuntimePlatform == Device.iOS)
            {
                StartTimmerStatusBluetooth();
            }
            SafeExecute(async () =>
            {
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
            AppManager.BluetoothService.OnReceiveNotificationBLE -= BluetoothService_OnReceiveNotificationBLE;
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

        public string AppVersion => appVersionService.GetAppVersion();

        #endregion Property

        #region PrivateMethod

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
                timerGPSTracking.Elapsed += TimerGPSTracking_Elapsed; ;

                timerGPSTracking.Start();
            }
            GetMyLocation();
        }

        private void TimerGPSTracking_Elapsed(object sender, ElapsedEventArgs e)
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

        private void BluetoothService_OnReceiveNotificationBLE(object sender, string Msg)
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
                //else if (PageNames.SOSPage == args || PageNames.MessagesPage == args)
                //{
                //    if (GlobalResourcesVMS.Current.DeviceManager.State == Service.BleConnectionState.NO_CONNECTION)
                //    {
                //        if (!await PageDialog.DisplayAlertAsync("Bạn chưa kết nối thiết bị. Bạn có muốn kết nối thiết bị?", "Cảnh báo", "ĐỒNG Ý", "BỎ QUA"))
                //        {
                //            return;
                //        }

                //        result = await NavigationService.NavigateAsync(PageNames.BluetoothPage.ToString());
                //    }
                //    else
                //    {
                //        result = await NavigationService.NavigateAsync(args.ToString());
                //    }
                //}
                else if (PageNames.OffMap == args)
                {
                    result = await NavigationService.NavigateAsync(args.ToString());
                }
                else
                {
                    result = await NavigationService.NavigateAsync(args.ToString());
                }

                if (!result.Success)
                {
                }
            });
        }

        private void StartTimmerStatusBluetooth()
        {
            timer = new Timer
            {
                Interval = 1000
            };
            timer.Elapsed += Timer_Elapsed;

            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (AppManager.BluetoothService.State == BleConnectionState.NO_CONNECTION)
            {
                GlobalResourcesVMS.Current.DeviceManager.State = BleConnectionState.NO_CONNECTION;
                GlobalResourcesVMS.Current.DeviceManager.DeviceName = string.Empty;
                GlobalResourcesVMS.Current.DeviceManager.DevicePlate = string.Empty;
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
                        Xamarin.Forms.DependencyService.Get<IToastNotificator>().Notify(new NotificationOptions()
                        {
                            Title = "Tin nhắn đến",
                            Description = message.Content,
                            IsClickable = true,
                            WindowsOptions = new WindowsOptions() { LogoUri = "ic_notification.png" },
                            ClearFromHistory = false,
                            AllowTapInNotificationCenter = false,
                            AndroidOptions = new AndroidOptions()
                            {
                                HexColor = "#F99D1C",
                                ForceOpenAppOnNotificationTap = true,
                            }
                        });
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

        private void GetMobileVersion()
        {
            if (!IsConnected)
            {
                return;
            }
            RunOnBackground(async () =>
            {
                return await dBVersionService.GetMobileVersion(Device.RuntimePlatform.ToString(), (int)App.AppType);
            },
            (versionDB) =>
            {
                if (versionDB != null && !string.IsNullOrEmpty(versionDB.VersionName) && !string.IsNullOrEmpty(versionDB.LinkDownload))
                {
                    // Nếu giá trị bị null hoặc giá trị đường link thay đổi => cập nhật lại
                    if (string.IsNullOrEmpty(Settings.AppLinkDownload) || !versionDB.LinkDownload.Equals(Settings.AppLinkDownload, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Settings.AppLinkDownload = versionDB.LinkDownload;
                    }

                    if (!versionDB.VersionName.Equals(AppVersion, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bool flags = false;

                        if (!flags)
                        {
                            flags = true;

                            string title = "Cập nhập phiên bản mới";
                            string message = !string.IsNullOrEmpty(versionDB.Description) ? versionDB.Description : "Cập nhập phiên bản mới";
                            string accept = MobileResource.Common_Button_Update;
                            string cancel = MobileResource.Common_Button_No;

                            // Nếu yêu cầu cài lại app
                            if (versionDB.IsMustUpdate)
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await NavigationService.NavigateAsync("UpdateVersion");
                                });
                            }
                            else
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    var install = await PageDialog.DisplayAlertAsync(title, message, accept, cancel);

                                    if (install == true) // if it's equal to Update
                                    {
                                        await Launcher.OpenAsync(new Uri(versionDB.LinkDownload));
                                    }
                                    else // if it's equal to Skip
                                    {
                                        return; // just return to the page and do nothing.
                                    }
                                });
                            }
                            flags = true;
                        }
                    }
                }
            });
        }

        #endregion PrivateMethod
    }
}