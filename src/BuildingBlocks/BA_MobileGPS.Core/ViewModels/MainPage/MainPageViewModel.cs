using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.Utilities;
using BA_MobileGPS.Utilities;
using Newtonsoft.Json;
using Plugin.Toasts;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Contructor

        private readonly IVehicleOnlineService vehicleOnlineService;
        private readonly IAlertService alertService;
        private readonly IVehicleDebtMoneyService vehicleDebtMoneyService;
        private readonly IAppDeviceService appDeviceService;
        private readonly IAppVersionService appVersionService;
        private readonly INotificationService notificationService;
        private readonly IIdentityHubService identityHubService;
        private readonly IVehicleOnlineHubService vehicleOnlineHubService;
        private readonly IAlertHubService alertHubService;
        private readonly IPingServerService pingServerService;
        private readonly IMapper _mapper;
        private Timer timer;
        private Timer timerSyncData;

        public MainPageViewModel(INavigationService navigationService, IVehicleOnlineService vehicleOnlineService,
            IAlertService alertService,
            IVehicleDebtMoneyService vehicleDebtMoneyService,
            IAppVersionService appVersionService,
            IAppDeviceService appDeviceService,
            INotificationService notificationService,
            IIdentityHubService identityHubService,
            IVehicleOnlineHubService vehicleOnlineHubService,
            IPingServerService pingServerService,
            IAlertHubService alertHubService, IMapper mapper)
            : base(navigationService)
        {
            this.vehicleOnlineService = vehicleOnlineService;
            this.alertService = alertService;
            this.appVersionService = appVersionService;
            this.vehicleDebtMoneyService = vehicleDebtMoneyService;
            this.appDeviceService = appDeviceService;
            this.notificationService = notificationService;
            this.identityHubService = identityHubService;
            this.vehicleOnlineHubService = vehicleOnlineHubService;
            this.alertHubService = alertHubService;
            this.pingServerService = pingServerService;
            this._mapper = mapper;

            StaticSettings.TimeServer = UserInfo.TimeServer.AddSeconds(1);
            SetTimeServer();
            StartTimmerSynData();
            EventAggregator.GetEvent<OnResumeEvent>().Subscribe(OnResumePage);
            EventAggregator.GetEvent<OnSleepEvent>().Subscribe(OnSleepPage);
            EventAggregator.GetEvent<SelectedCompanyEvent>().Subscribe(SelectedCompanyChanged);
            EventAggregator.GetEvent<OneSignalOpendEvent>().Subscribe(OneSignalOpend);
        }

        #endregion Contructor

        #region Lifecycle

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
            TryExecute(() =>
            {
                InitVehilceOnline();
                Device.StartTimer(TimeSpan.FromMilliseconds(700), () =>
                {
                    TryExecute(async () =>
                    {
                        //await ConnectSignalR();
                        //GetCountVehicleDebtMoney();
                        InsertOrUpdateAppDevice();
                        // GetNoticePopup();
                        PushPageFileBase();
                        // Lấy danh sách cảnh báo
                        //GetCountAlert();
                    });

                    return false;
                });
            });
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (IsLoaded)
            {
                base.OnDestroy();
                timer.Stop();
                timer.Dispose();
                timerSyncData.Stop();
                timerSyncData.Dispose();
                EventAggregator.GetEvent<OnResumeEvent>().Unsubscribe(OnResumePage);
                EventAggregator.GetEvent<OnSleepEvent>().Unsubscribe(OnSleepPage);
                EventAggregator.GetEvent<SelectedCompanyEvent>().Unsubscribe(SelectedCompanyChanged);
                EventAggregator.GetEvent<OneSignalOpendEvent>().Unsubscribe(OneSignalOpend);
                DisconnectSignalR();
                IsLoaded = false;
            }
        }

        private void InitVehilceOnline()
        {
            if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 0)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    EventAggregator.GetEvent<OnReloadVehicleOnline>().Publish(false);
                    await ConnectSignalROnline();
                    //Join vào nhóm signalR để nhận dữ liệu online
                    JoinGroupSignalRCar(StaticSettings.ListVehilceOnline.Select(x => x.VehicleId.ToString()).ToList());
                });
            }
            else
            {
                GetListVehicleOnline();
            }
        }

        private void OnResumePage(bool args)
        {
            SafeExecute(async () =>
            {
                if (IsConnected)
                {
                    await ConnectSignalROnline();
                    await ConnectSignalR();
                    if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 0)
                    {
                        if (DateTime.Now.Subtract(StaticSettings.TimeSleep).TotalMinutes >= MobileSettingHelper.TimeSleep)
                        {
                            GetListVehicleOnlineResume(true);
                        }
                        else
                        {
                            GetListVehicleOnlineResume();
                        }
                    }
                    GetTimeServer();
                    //kiểm tra xem có thông báo nào không
                    //GetNofitication();
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
                    });
                }
            });
        }

        private void OnSleepPage(bool obj)
        {
            StaticSettings.TimeSleep = DateTime.Now;
            DisconnectSignalR();
        }

        #endregion Lifecycle

        #region Property

        private bool IsLoaded { get; set; }

        #endregion Property

        #region PrivateMethod

        public override void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            base.OnConnectivityChanged(sender, e);

            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                OnResumePage(true);
            }
            else
            {
                StaticSettings.TimeSleep = DateTime.Now;
            }
        }

        private void SetTimeServer()
        {
            timer = new Timer
            {
                Interval = 1000
            };
            timer.Elapsed += UpdateTimeServer;

            timer.Start();
        }

        private void StartTimmerSynData()
        {
            timerSyncData = new Timer
            {
                Interval = CompanyConfigurationHelper.TimmerVehicleSync
            };
            timerSyncData.Elapsed += TimerSyncData;

            timerSyncData.Start();
        }

        private void UpdateTimeServer(object sender, ElapsedEventArgs e)
        {
            StaticSettings.TimeServer = StaticSettings.TimeServer.AddSeconds(1);
        }

        private void TimerSyncData(object sender, ElapsedEventArgs e)
        {
            SyncVehicleOnline();
        }

        private async Task ConnectSignalR()
        {
            //Hub logout
            await identityHubService.Connect();
            identityHubService.onReceivePushLogoutToAllUserInCompany += onReceivePushLogoutToAllUserInCompany;
            identityHubService.onReceivePushLogoutToUser += onReceivePushLogoutToUser;

            // Khởi tạo alertlR
            await alertHubService.Connect();
            alertHubService.onReceiveAlertSignalR += OnReceiveAlertSignalR;
        }

        private async Task ConnectSignalROnline()
        {
            // Khởi tạo signalR
            await vehicleOnlineHubService.Connect();
            vehicleOnlineHubService.onReceiveSendCarSignalR -= OnReceiveSendCarSignalR;
            vehicleOnlineHubService.onReceiveSendCarSignalR += OnReceiveSendCarSignalR;
        }

        private async void DisconnectSignalR()
        {
            identityHubService.onReceivePushLogoutToAllUserInCompany -= onReceivePushLogoutToAllUserInCompany;
            identityHubService.onReceivePushLogoutToUser -= onReceivePushLogoutToUser;
            await identityHubService.Disconnect();

            //thoát khỏi nhóm nhận xe
            if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 0)
            {
                LeaveGroupSignalRCar(StaticSettings.ListVehilceOnline.Select(x => x.VehicleId.ToString()).ToList());
            }

            vehicleOnlineHubService.onReceiveSendCarSignalR -= OnReceiveSendCarSignalR;

            await vehicleOnlineHubService.Disconnect();

            alertHubService.onReceiveAlertSignalR -= OnReceiveAlertSignalR;

            await alertHubService.Disconnect();
        }

        private void JoinGroupSignalRCar(List<string> lstGroup)
        {
            try
            {
                //Thoát khỏi nhóm nhận thông tin xe
                vehicleOnlineHubService.JoinGroupReceivedVehicleID(string.Join(",", lstGroup));
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void LeaveGroupSignalRCar(List<string> lstGroup)
        {
            try
            {
                //Thoát khỏi nhóm nhận thông tin xe
                vehicleOnlineHubService.LeaveGroupReceivedVehicleID(string.Join(",", lstGroup));
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void onReceivePushLogoutToUser(object sender, string message)
        {
            TryExecute(() =>
            {
                if (!string.IsNullOrEmpty(message))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //Chỗ này sử lý logic khi server trả về trạng thái là thay đổi mật khẩu cần logout ra ngoài
                        Logout();
                        DisplayMessage.ShowMessageInfo("Phiên làm việc đã hết hạn bạn vui lòng đăng nhập lại");
                    });
                }
            });
        }

        private void onReceivePushLogoutToAllUserInCompany(object sender, string message)
        {
            TryExecute(() =>
            {
                if (!string.IsNullOrEmpty(message) && message == UserInfo.CompanyId.ToString())
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //Chỗ này sử lý logic khi server trả về trạng thái là thay đổi mật khẩu cần logout ra ngoài
                        Logout();
                        DisplayMessage.ShowMessageInfo("Phiên làm việc đã hết hạn bạn vui lòng đăng nhập lại");
                    });
                }
            });
        }

        private void OnReceiveSendCarSignalR(object sender, string e)
        {
            var carInfo = JsonConvert.DeserializeObject<VehicleOnlineMessage>(e);
            if (carInfo != null)
            {
                SendDataCar(carInfo);
            }
        }

        private void SendDataCar(VehicleOnlineMessage carInfo)
        {
            if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 0)
            {
                var vehicle = StaticSettings.ListVehilceOnline.FirstOrDefault(x => x.VehicleId == carInfo.VehicleId);
                if (vehicle != null && !StateVehicleExtension.IsVehicleDebtMoney(vehicle.MessageId, vehicle.DataExt) && vehicle.VehicleTime < carInfo.VehicleTime)
                {
                    vehicle.Update(carInfo);
                    vehicle.IconImage = IconCodeHelper.GetMarkerResource(vehicle);
                    vehicle.StatusEngineer = StateVehicleExtension.EngineState(vehicle);
                    if (!StateVehicleExtension.IsLostGPS(vehicle.GPSTime, vehicle.VehicleTime) && !StateVehicleExtension.IsLostGSM(vehicle.VehicleTime))
                    {
                        vehicle.SortOrder = 1;
                    }
                    else
                    {
                        vehicle.SortOrder = 0;
                    }

                    EventAggregator.GetEvent<ReceiveSendCarEvent>().Publish(vehicle);
                }
            }
        }

        private void OnReceiveAlertSignalR(object sender, string e)
        {
            var alert = JsonConvert.DeserializeObject<AlertSignalRModel>(e);

            if (alert != null)
            {
                if (MobileUserSettingHelper.ShowNotification)
                {
                    DependencyService.Get<IToastNotificator>().Notify(new NotificationOptions()
                    {
                        Description = alert.WarningContent,
                        Title = MobileResource.Alert_Label_TilePage,
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
                }

                EventAggregator.GetEvent<RecieveAlertEvent>().Publish(alert);

                GlobalResources.Current.TotalAlert++;
            }
        }

        private void GetListVehicleOnlineResume(bool isRelogin = false)
        {
            var userID = UserInfo.UserId;
            var companyID = UserInfo.CompanyId;
            var xnCode = UserInfo.XNCode;
            var userType = UserInfo.UserType;
            var companyType = UserInfo.CompanyType;

            if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
            {
                userID = Settings.CurrentCompany.UserId;
                companyID = Settings.CurrentCompany.FK_CompanyID;
                xnCode = Settings.CurrentCompany.XNCode;
                userType = Settings.CurrentCompany.UserType;
                companyType = Settings.CurrentCompany.CompanyType;
            }
            int vehicleGroup = 0;

            RunOnBackground(async () =>
            {
                return await vehicleOnlineService.GetListVehicleOnline(userID, vehicleGroup, companyID, xnCode, userType, companyType);
            }, (result) =>
            {
                if (StaticSettings.IsUnauthorized)
                {
                    if (result != null && result.Count > 0)
                    {
                        result.ForEach(x =>
                        {
                            x.IconImage = IconCodeHelper.GetMarkerResource(x);
                            x.StatusEngineer = StateVehicleExtension.EngineState(x);

                            if (!StateVehicleExtension.IsLostGPS(x.GPSTime, x.VehicleTime) && !StateVehicleExtension.IsLostGSM(x.VehicleTime))
                            {
                                x.SortOrder = 1;
                            }
                            else
                            {
                                x.SortOrder = 0;
                            }
                        });

                        StaticSettings.ListVehilceOnline = result;

                        //Join vào nhóm signalR để nhận dữ liệu online
                        JoinGroupSignalRCar(result.Select(x => x.VehicleId.ToString()).ToList());

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (isRelogin)
                            {
                                EventAggregator.GetEvent<OnReloadVehicleOnline>().Publish(false);
                            }
                            else
                            {
                                EventAggregator.GetEvent<OnReloadVehicleOnline>().Publish(true);
                            }
                        });
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //Chỗ này sử lý logic khi server trả về trạng thái là thay đổi mật khẩu cần logout ra ngoài
                        Logout();
                        DisplayMessage.ShowMessageInfo("Phiên làm việc đã hết hạn bạn vui lòng đăng nhập lại");
                    });
                }
            });
        }

        private void GetListVehicleOnline()
        {
            var userID = UserInfo.UserId;
            var companyID = UserInfo.CompanyId;
            var xnCode = UserInfo.XNCode;
            var userType = UserInfo.UserType;
            var companyType = UserInfo.CompanyType;

            if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
            {
                userID = Settings.CurrentCompany.UserId;
                companyID = Settings.CurrentCompany.FK_CompanyID;
                xnCode = Settings.CurrentCompany.XNCode;
                userType = Settings.CurrentCompany.UserType;
                companyType = Settings.CurrentCompany.CompanyType;
            }
            int vehicleGroup = 0;

            RunOnBackground(async () =>
            {
                return await vehicleOnlineService.GetListVehicleOnline(userID, vehicleGroup, companyID, xnCode, userType, companyType);
            }, (result) =>
            {
                if (result != null && result.Count > 0)
                {
                    result.ForEach(x =>
                    {
                        x.IconImage = IconCodeHelper.GetMarkerResource(x);
                        x.StatusEngineer = StateVehicleExtension.EngineState(x);

                        if (!StateVehicleExtension.IsLostGPS(x.GPSTime, x.VehicleTime) && !StateVehicleExtension.IsLostGSM(x.VehicleTime))
                        {
                            x.SortOrder = 1;
                        }
                        else
                        {
                            x.SortOrder = 0;
                        }
                    });

                    StaticSettings.ListVehilceOnline = result;

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        EventAggregator.GetEvent<OnReloadVehicleOnline>().Publish(false);

                        await ConnectSignalROnline();
                        //Join vào nhóm signalR để nhận dữ liệu online
                        JoinGroupSignalRCar(result.Select(x => x.VehicleId.ToString()).ToList());
                    });
                }
                else
                {
                    StaticSettings.ListVehilceOnline = new List<VehicleOnline>();
                }
            });
        }

        private void SyncVehicleOnline()
        {
            // Nếu cho phép đồng bộ thì mới cần như này, không thì chỉ 1 mình SignalR cân hết.
            if (CompanyConfigurationHelper.EnableLongPoolRequest)
            {
                if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 0)
                {
                    // Danh sách cần syn về api để lấy dữ liệu thay đổi
                    List<long> synVehicles = null;

                    // Namth: Lấy xe thời gian hiện tại trừ thời gian của xe =< 2 và nhỏ hơn 5
                    if (CompanyConfigurationHelper.SynOnlineLevel == SynOnlineLevelTypes.Level1)
                    {
                        synVehicles = StateVehicleExtension.GetVehicleSyncData(StaticSettings.ListVehilceOnline);
                    }

                    // trungtq: Lấy tất theo phiên cho lành (trường hợp xấu nhất, ăn theo cấu hình.
                    else if (CompanyConfigurationHelper.SynOnlineLevel == SynOnlineLevelTypes.Level2)
                    {
                        synVehicles = StaticSettings.ListVehilceOnline.Select(item => item.VehicleId).ToList();
                    }

                    if (synVehicles != null && synVehicles.Count > 0)
                    {
                        TryExecute(async () =>
                        {
                            if (!vehicleOnlineHubService.IsConnectedOrConnecting())
                            {
                                await ConnectSignalROnline();
                            }
                        });

                        var vehicelIDs = string.Join(",", synVehicles);
                        var userID = UserInfo.UserId;
                        var companyID = UserInfo.CompanyId;
                        if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
                        {
                            userID = Settings.CurrentCompany.UserId;
                            companyID = Settings.CurrentCompany.FK_CompanyID;
                        }
                        var request = new VehicleOnlineRequest()
                        {
                            CompanyID = companyID,
                            LastSync = StaticSettings.LastSyncTime,
                            UserId = userID,
                            VehicelIDs = vehicelIDs,
                            XnCode = UserInfo.XNCode
                        };
                        RunOnBackground(async () =>
                        {
                            return await vehicleOnlineService.GetListVehicleOnlineSync(request);
                        }, (result) =>
                        {
                            if (result != null && result.Count > 0)
                            {
                                // trungtq: Lấy theo giờ Server cho chuẩn
                                StaticSettings.LastSyncTime = StaticSettings.TimeServer;

                                Parallel.For(0, result.Count, action =>
                                {
                                    SendDataCar(result[action]);
                                });
                            }
                            else
                            {
                                var lst = StateVehicleExtension.GetVehicleLostGPSAndLostGSM();
                                if (lst != null && lst.Count > 0)
                                {
                                    Parallel.For(0, lst.Count, action =>
                                    {
                                        var vehicle = _mapper.MapProperties<VehicleOnlineMessage>(lst[action]);
                                        SendDataCar(vehicle);
                                    });
                                }
                            }
                        });
                    }
                }
            }
        }

        private void SelectedCompanyChanged(int companyID)
        {
            if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 0)
            {
                //Join vào nhóm signalR để nhận dữ liệu online
                JoinGroupSignalRCar(StaticSettings.ListVehilceOnline.Select(x => x.VehicleId.ToString()).ToList());
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                GetCountAlert();
            });
        }

        private void OneSignalOpend(bool obj)
        {
            PushPageFileBase();
        }

        private void GetCountAlert()
        {
            var userID = UserInfo.UserId;
            if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
            {
                userID = Settings.CurrentCompany.UserId;
            }

            RunOnBackground(async () =>
            {
                return await alertService.GetCountAlert(userID);
            }, (result) =>
            {
                GlobalResources.Current.TotalAlert = result;
            });
        }

        private void GetTimeServer()
        {
            RunOnBackground(async () =>
            {
                return await pingServerService.GetTimeServer();
            }, (result) =>
            {
                if (result != null && result.Data != null)
                {
                    StaticSettings.TimeServer = result.Data;
                }
                else
                {
                    if (StaticSettings.TimeServer < DateTime.Now)
                    {
                        StaticSettings.TimeServer = DateTime.Now;
                    }
                }
            });
        }

        private void GetCountVehicleDebtMoney()
        {
            if (MobileSettingHelper.IsUseVehicleDebtMoney)
            {
                RunOnBackground(async () =>
                {
                    return await vehicleDebtMoneyService.GetCountVehicleDebtMoney(UserInfo.UserId);
                }, async (result) =>
                {
                    if (result > 0)
                    {
                        if (string.IsNullOrEmpty(Settings.ReceivedNotificationType))
                        {
                            // gọi sang trang danh sách nợ phí
                            _ = await NavigationService.NavigateAsync("NavigationPage/VehicleDebtMoneyPage", null, useModalNavigation: true, true);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Lưu lại thông tin thiết bị xuống server
        /// </summary>
        private void InsertOrUpdateAppDevice()
        {
            var request = new AppDeviceRequest()
            {
                FK_AppID = (int)App.AppType,
                FK_UserID = UserInfo.UserId,
                DeviceName = DeviceInfo.Model,
                AppVersion = appVersionService.GetAppVersion(),
                Idiom = DeviceInfo.Idiom.ToString(),
                Manufacturer = DeviceInfo.Manufacturer,
                Platform = DeviceInfo.Platform.ToString(),
                OSVersion = DeviceInfo.VersionString,
                DeviceType = DeviceInfo.DeviceType.ToString(),
                TokenID = Settings.CurrentFirebaseToken
            };

            RunOnBackground(async () =>
            {
                return await appDeviceService.InsertOrUpdateAppDevice(request);
            },
            (result) =>
            {
                if (result != null && result.Success && result.Data)
                {
                }
            });
        }

        private void GetNofitication()
        {
            RunOnBackground(async () =>
            {
                return await notificationService.GetNotification(UserInfo.UserId);
            }, (result) =>
            {
                if (result != null && result.Success)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        switch (result.Data)
                        {
                            case NotificationTypeEnum.None:

                                break;

                            case NotificationTypeEnum.ChangePassword:
                                DisplayMessage.ShowMessageInfo("Phiên làm việc đã hết hạn bạn vui lòng đăng nhập lại");
                                Logout();
                                break;
                        }
                    });
                }
                else
                {
                }
            });
        }

        /// <summary>Gọi thông tin popup sau khi đăng nhập</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/26/2020   created
        /// </Modified>
        private void GetNoticePopup()
        {
            RunOnBackground(async () =>
            {
                return await notificationService.GetNotificationAfterLogin(StaticSettings.User.UserId);
            }, (items) =>
            {
                if (items != null && items.Data != null)
                {
                    if (items.Data.IsAlwayShow) // true luôn luôn hiển thị
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await NavigationService.NavigateAsync("NotificationPopupAfterLogin", parameters: new NavigationParameters
                                 {
                                 { ParameterKey.NotificationKey, items.Data }
                                });
                        });
                    }
                    else
                    {
                        if (Settings.NoticeIdAfterLogin != items.Data.PK_NoticeContentID)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await NavigationService.NavigateAsync("NotificationPopupAfterLogin", parameters: new NavigationParameters
                                     {
                                 { ParameterKey.NotificationKey, items.Data }
                                    });
                            });
                        }
                    }
                }
            });
        }

        private void PushPageFileBase()
        {
            //nếu người dùng click vào mở thông báo firebase thì vào trang thông báo luôn
            if (!string.IsNullOrEmpty(Settings.ReceivedNotificationType))
            {
                //NẾU Firebase là tung điều thì mở lên cuốc được tung điều đó
                if (Settings.ReceivedNotificationType == (((int)FormOfNoticeTypeEnum.Notice).ToString()))
                {
                    Settings.ReceivedNotificationType = string.Empty;
                    if (!string.IsNullOrEmpty(Settings.ReceivedNotificationValue))
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await NavigationService.NavigateAsync("BaseNavigationPage/NotificationDetailPage", parameters: new NavigationParameters
                             {
                                 { ParameterKey.NotificationKey, int.Parse(Settings.ReceivedNotificationValue) }
                            }, useModalNavigation: true, true);
                        });
                    }
                }
            }
        }

        #endregion PrivateMethod
    }
}