using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Enums;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.Utilities;
using BA_MobileGPS.Utilities;
using Newtonsoft.Json;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
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
        private readonly IVehicleOnlineService vehicleOnlineService;
        private readonly IAlertService alertService;
        private readonly IVehicleDebtMoneyService vehicleDebtMoneyService;
        private readonly IAppDeviceService appDeviceService;
        private readonly INotificationService notificationService;
        private readonly IIdentityHubService identityHubService;
        private readonly IVehicleOnlineHubService vehicleOnlineHubService;
        private readonly IUserBahaviorHubService userBahaviorHubService;
        private readonly IPingServerService pingServerService;
        private readonly IMapper _mapper;
        private readonly IStreamCameraService streamCameraService;
        private readonly IUserService userService;
        private Timer timer;
        private Timer timerSyncData;
        private Timer timerSyncUploadStatus;
        private Timer timerPingStreamCamera;
        private System.Threading.CancellationTokenSource cts = new System.Threading.CancellationTokenSource();

        public MainPageViewModel(INavigationService navigationService, IVehicleOnlineService vehicleOnlineService,
            IAlertService alertService,
            IVehicleDebtMoneyService vehicleDebtMoneyService,
            IAppDeviceService appDeviceService,
            INotificationService notificationService,
            IIdentityHubService identityHubService,
            IVehicleOnlineHubService vehicleOnlineHubService,
            IPingServerService pingServerService,
            IUserBahaviorHubService userBahaviorHubService, IMapper mapper,
            IStreamCameraService streamCameraService, IUserService userService)
            : base(navigationService)
        {
            this.vehicleOnlineService = vehicleOnlineService;
            this.alertService = alertService;
            this.vehicleDebtMoneyService = vehicleDebtMoneyService;
            this.appDeviceService = appDeviceService;
            this.notificationService = notificationService;
            this.identityHubService = identityHubService;
            this.vehicleOnlineHubService = vehicleOnlineHubService;
            this.userBahaviorHubService = userBahaviorHubService;
            this.pingServerService = pingServerService;
            this.streamCameraService = streamCameraService;
            this.userService = userService;
            this._mapper = mapper;

            StaticSettings.TimeServer = UserInfo.TimeServer.AddSeconds(1);
            SetTimeServer();
            DisposeEventAggregator();
            EventAggregator.GetEvent<OnResumeEvent>().Subscribe(OnResumePage);
            EventAggregator.GetEvent<OnSleepEvent>().Subscribe(OnSleepPage);
            EventAggregator.GetEvent<SelectedCompanyEvent>().Subscribe(SelectedCompanyChanged);
            EventAggregator.GetEvent<OneSignalOpendEvent>().Subscribe(OneSignalOpend);
            EventAggregator.GetEvent<UploadVideoEvent>().Subscribe(UploadVideoRestream);
            EventAggregator.GetEvent<UserBehaviorEvent>().Subscribe(OnUserBehavior);
            EventAggregator.GetEvent<UserMessageEvent>().Subscribe(PushMessageToUser);

            InitSystemType();
        }

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
                        await ConnectSignalR();
                        PushPageFileBase();
                        InsertOrUpdateAppDevice();
                    });

                    return false;
                });
            });
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            timer.Stop();
            timer.Dispose();
            timerSyncData.Stop();
            timerSyncData.Dispose();
            if (timerSyncUploadStatus != null)
            {
                timerSyncUploadStatus.Stop();
                timerSyncUploadStatus.Dispose();
            }
            if (timerPingStreamCamera != null)
            {
                timerPingStreamCamera.Stop();
                timerPingStreamCamera.Dispose();
            }
            if (cts != null)
            {
                cts.Cancel();
                cts.Dispose();
            }
            DisposeEventAggregator();
            DisconnectSignalR();
        }

        private void DisposeEventAggregator()
        {
            EventAggregator.GetEvent<OnResumeEvent>().Unsubscribe(OnResumePage);
            EventAggregator.GetEvent<OnSleepEvent>().Unsubscribe(OnSleepPage);
            EventAggregator.GetEvent<SelectedCompanyEvent>().Unsubscribe(SelectedCompanyChanged);
            EventAggregator.GetEvent<OneSignalOpendEvent>().Unsubscribe(OneSignalOpend);
            EventAggregator.GetEvent<UploadVideoEvent>().Unsubscribe(UploadVideoRestream);
            EventAggregator.GetEvent<UserBehaviorEvent>().Unsubscribe(OnUserBehavior);
            EventAggregator.GetEvent<UserMessageEvent>().Unsubscribe(PushMessageToUser);
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
                if (CheckPermision((int)PermissionKeyNames.TrackingVideosView) || CheckPermision((int)PermissionKeyNames.TrackingOnlineByImagesView))
                {
                    GetVehicleIsCamera();
                }
                else
                {
                    GetListVehicleOnline();
                }
            }
        }

        private void OnResumePage(bool args)
        {
            SafeExecute(async () =>
            {
                var isconnectedinternet = NetworkHelper.ConnectedToInternet();
                if (IsConnected && isconnectedinternet)
                {
                    await ConnectSignalROnline();
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
                    Device.StartTimer(TimeSpan.FromMilliseconds(700), () =>
                    {
                        TryExecute(async () =>
                        {
                            await ConnectSignalR();
                        });

                        return false;
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (!isconnectedinternet)
                        {
                            await PopupNavigation.Instance.PushAsync(new NetworkPage());
                        }
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

        private void OnUserBehavior(UserBehaviorModel obj)
        {
            if (obj != null && MobileSettingHelper.UseUserBehavior)
            {
                userBahaviorHubService.SendUserBehavior(new UserBehaviorRequest()
                {
                    CompanyId = CurrentComanyID,
                    Id = UserInfo.UserId,
                    Fullname = UserInfo.FullName,
                    Username = UserInfo.UserName,
                    XNCode = UserInfo.XNCode,
                    SystemType = SystemType,
                    MenuKey = obj.Page,
                    Time = StaticSettings.TimeServer.Ticks,
                    TimeType = obj.Type,
                });
            }
        }

        private int SystemType { get; set; } = 51;

        private void InitSystemType()
        {
            switch (App.AppType)
            {
                case AppType.BinhAnh:
                    SystemType = 51;
                    break;

                case AppType.GisViet:
                    SystemType = 56;
                    break;

                case AppType.CNN:
                    SystemType = 52;
                    break;

                case AppType.Viview:
                    SystemType = 53;
                    break;

                case AppType.VMS:
                    SystemType = 55;
                    break;

                case AppType.Moto:
                    SystemType = 54;
                    break;
            }
        }

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
            identityHubService.onReceivePushLogoutToAllUserInCompany -= onReceivePushLogoutToAllUserInCompany;
            identityHubService.onReceivePushLogoutToAllUserInCompany += onReceivePushLogoutToAllUserInCompany;
            identityHubService.onReceivePushLogoutToUser -= onReceivePushLogoutToUser;
            identityHubService.onReceivePushLogoutToUser += onReceivePushLogoutToUser;
            identityHubService.onReceivePushMessageToUser -= onReceiveMessageToUser;
            identityHubService.onReceivePushMessageToUser += onReceiveMessageToUser;
            if (MobileSettingHelper.UseUserBehavior)
            {
                await userBahaviorHubService.Connect();
                userBahaviorHubService.SendUserBehavior(new UserBehaviorRequest()
                {
                    CompanyId = CurrentComanyID,
                    Id = UserInfo.UserId,
                    Fullname = UserInfo.FullName,
                    Username = UserInfo.UserName,
                    XNCode = UserInfo.XNCode,
                    SystemType = SystemType,
                    MenuKey = Entities.Enums.MenuKeyEnums.ModuleOnline,
                    Time = StaticSettings.TimeServer.Ticks,
                    TimeType = UserBehaviorType.Start,
                });
            }
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
            identityHubService.onReceivePushMessageToUser -= onReceiveMessageToUser;
            await identityHubService.Disconnect();

            //thoát khỏi nhóm nhận xe
            if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 0)
            {
                LeaveGroupSignalRCar(StaticSettings.ListVehilceOnline.Select(x => x.VehicleId.ToString()).ToList());
            }

            vehicleOnlineHubService.onReceiveSendCarSignalR -= OnReceiveSendCarSignalR;

            await vehicleOnlineHubService.Disconnect();

            //if (CheckPermision((int)PermissionKeyNames.AdminAlertView))
            //{
            //    alertHubService.onReceiveAlertSignalR -= OnReceiveAlertSignalR;

            //    await alertHubService.Disconnect();
            //}
            if (MobileSettingHelper.UseUserBehavior)
            {
                await userBahaviorHubService.Disconnect();
            }
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

        private void PushMessageToUser(UserMessageEventModel model)
        {
            try
            {
                GetUserInfoByUserName(model);
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

        private void onReceiveMessageToUser(object sender, string message)
        {
            TryExecute(() =>
            {
                if (!string.IsNullOrEmpty(message))
                {
                    EventAggregator.GetEvent<UserMessageCameraEvent>().Publish(message);
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
                                x.SortOrder = 2;
                            }
                            else
                            {
                                x.SortOrder = 1;
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
            try
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

                        StartTimmerSynData();

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            EventAggregator.GetEvent<OnReloadVehicleOnline>().Publish(false);

                            await ConnectSignalROnline();
                            //Join vào nhóm signalR để nhận dữ liệu online
                            JoinGroupSignalRCar(result.Select(x => x.VehicleId.ToString()).ToList());
                        });

                        // Lấy danh sách cảnh báo
                        GetCountAlert();

                        StartStreamMultiple();
                    }
                    else
                    {
                        StaticSettings.ListVehilceOnline = new List<VehicleOnline>();
                    }
                });
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteLog("GetListVehicleOnline", $"Start Error API :{ex.Message}");
            }
        }

        private void GetVehicleIsCamera()
        {
            if (StaticSettings.ListVehilceCamera != null && StaticSettings.ListVehilceCamera.Count > 0)
            {
                return;
            }
            else
            {
                RunOnBackground(async () =>
                {
                    return await streamCameraService.GetListVehicleHasCamera(UserInfo.XNCode);
                },
                (lst) =>
                {
                    if (lst != null && lst.Count > 0)
                    {
                        StaticSettings.ListVehilceCamera = lst;
                    }
                    GetListVehicleOnline();
                });
            }
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
            PushPageFileBase(true);
        }

        private void GetCountAlert()
        {
            //kiểm tra xem có quyền hay ko
            if (CheckPermision((int)PermissionKeyNames.AdminAlertView))
            {
                if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 0)
                {
                    var userID = UserInfo.UserId;
                    if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
                    {
                        userID = Settings.CurrentCompany.UserId;
                    }
                    var request = new GetCountAlertByUserIDRequest()
                    {
                        CompanyID = CurrentComanyID,
                        UserID = userID,
                        ListVehicleIDs = string.Join(",", StaticSettings.ListVehilceOnline.Select(x => x.VehicleId))
                    };
                    RunOnBackground(async () =>
                    {
                        return await alertService.GetCountAlert(request);
                    }, (result) =>
                    {
                        GlobalResources.Current.TotalAlert = result;
                    });
                }
            }
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

        private void GetUserInfoByUserName(UserMessageEventModel model)
        {
            RunOnBackground(async () =>
            {
                return await userService.GetUserInfomation(model.UserName);
            }, (result) =>
            {
                if (result != null && result.PK_UserID != Guid.Empty)
                {
                    //Thoát khỏi nhóm nhận thông tin xe
                    identityHubService.PushMessageToUser(result.PK_UserID.ToString().ToUpper(), model.Message);
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
                            var param = new NavigationParameters
                            {
                                {ParameterKey.IsLoginAnnouncement, true }
                            };
                            _ = await NavigationService.NavigateAsync("NavigationPage/VehicleDebtMoneyPage", param, useModalNavigation: true, true);
                            return;
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
                AppVersion = VersionTracking.CurrentVersion,
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
                if (items != null && items.Data != null && items.Data.Id > 0)
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
                        if (Settings.NoticeIdAfterLogin != items.Data.Id)
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

        private void PushPageFileBase(bool isOpen = false)
        {
            //nếu người dùng click vào mở thông báo firebase thì vào trang thông báo luôn
            if (!string.IsNullOrEmpty(Settings.ReceivedNotificationType))
            {
                //NẾU Firebase là tung điều thì mở lên cuốc được tung điều đó
                if (Settings.ReceivedNotificationType == (((int)FirebaseNotificationTypeEnum.Notice).ToString()))
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
                else if (Settings.ReceivedNotificationType == (((int)FirebaseNotificationTypeEnum.Issue).ToString()))
                {
                    Settings.ReceivedNotificationType = string.Empty;
                    if (!string.IsNullOrEmpty(Settings.ReceivedNotificationValue))
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await NavigationService.NavigateAsync("BaseNavigationPage/IssuesDetailPage", parameters: new NavigationParameters
                             {
                                 { ParameterKey.IssuesKey, Settings.ReceivedNotificationValue }
                            });
                        });
                    }
                }
                else if (Settings.ReceivedNotificationType == (((int)FirebaseNotificationTypeEnum.AlertMask).ToString()))
                {
                    Settings.ReceivedNotificationType = string.Empty;
                    if (!string.IsNullOrEmpty(Settings.ReceivedNotificationValue))
                    {
                        ShowMaskImage(Settings.ReceivedNotificationValue);
                    }
                }
                else if (!isOpen)
                {
                    GetCountVehicleDebtMoney();
                    GetNoticePopup();
                }
            }
            else if (!isOpen)
            {
                GetCountVehicleDebtMoney();
                GetNoticePopup();
            }
        }

        /// <summary>Xem chi tiết ảnh</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/6/2020   created
        /// </Modified>
        private async void ShowMaskImage(string id)
        {
            await NavigationService.NavigateAsync("NavigationPage/AlertMaskDetailPage", parameters: new NavigationParameters
                                     {
                                    { ParameterKey.AlertMask, new Guid(id) }
                                    }, useModalNavigation: true);
        }

        private void UploadVideoRestream(bool arg)
        {
            StartTimmerUploadVideo();
        }

        private async void CheckStatusUploadFile(object sender, ElapsedEventArgs e)
        {
            if (StaticSettings.ListVehilceCamera != null && StaticSettings.ListVehilceCamera.Count > 0)
            {
                List<UploadFiles> lstOldFileUploading = new List<UploadFiles>();
                var listVehicle = StaticSettings.ListVehilceCamera.Select(x => x.VehiclePlate).ToList();
                var respone = await streamCameraService.GetUploadingProgressInfor(new UploadStatusRequest()
                {
                    CustomerID = UserInfo.XNCode,
                    VehicleName = listVehicle,
                    Source = (int)CameraSourceType.App,
                    User = UserInfo.UserName,
                    SessionID = StaticSettings.SessionID
                });
                if (respone != null && respone.Count > 0)
                {
                    List<UploadFiles> lstVideoNew = new List<UploadFiles>();
                    foreach (var item in respone)
                    {
                        if (item.UploadFiles != null)
                        {
                            lstVideoNew.AddRange(item.UploadFiles);
                        }
                    }
                    if (StaticSettings.ListUploadFiles != null && StaticSettings.ListUploadFiles.Count > 0)
                    {
                        if (lstVideoNew.Count > 0)
                        {
                            var listvideoUploaded = lstVideoNew.Where(x => x.State != (int)VideoUploadStatus.Uploading
                           && x.State != (int)VideoUploadStatus.WaitingUpload).ToList();
                            List<UploadFiles> lstuploadold = new List<UploadFiles>();
                            lstuploadold.AddRange(StaticSettings.ListUploadFiles);
                            foreach (var itemold in lstuploadold)
                            {
                                var itemnew = listvideoUploaded.FirstOrDefault(x => x.State != itemold.State && x.Time == itemold.Time);
                                if (itemnew != null)
                                {
                                    if (StaticSettings.ListUploadFiles != null && StaticSettings.ListUploadFiles.Count > 0)
                                    {
                                        StaticSettings.ListUploadFiles.Remove(itemold);
                                    }
                                    if (itemnew.State == (int)VideoUploadStatus.Uploaded && !string.IsNullOrEmpty(itemnew.Link))
                                    {
                                        EventAggregator.GetEvent<UploadFinishVideoEvent>().Publish(true);

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            DisplayMessage.ShowMessageInfo(MobileResource.Camera_Alert_DownloadedVideo);
                                        });
                                    }
                                    else if (itemnew.State == (int)VideoUploadStatus.UploadErrorCancel
                                            || itemnew.State == (int)VideoUploadStatus.UploadErrorDevice
                                            || itemnew.State == (int)VideoUploadStatus.UploadErrorTimeout)
                                    {
                                        EventAggregator.GetEvent<UploadFinishVideoEvent>().Publish(false);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (timerSyncUploadStatus != null)
                        {
                            timerSyncUploadStatus.Stop();
                            timerSyncUploadStatus.Dispose();
                        }
                    }
                    var stateUpload = respone.Exists(x => x.State == (int)VideoUploadStatus.WaitingUpload
                    || x.State == (int)VideoUploadStatus.Uploading);
                    if (!stateUpload)
                    {
                        StaticSettings.ListUploadFiles = new List<UploadFiles>();
                        //nếu ko còn phiên nào chạy thì Hủy Timmer đi
                        if (timerSyncUploadStatus != null)
                        {
                            timerSyncUploadStatus.Stop();
                            timerSyncUploadStatus.Dispose();
                        }
                    }
                }
            }
        }

        private void StartTimmerUploadVideo()
        {
            if (timerSyncUploadStatus == null || !timerSyncUploadStatus.Enabled)
            {
                timerSyncUploadStatus = new Timer
                {
                    Interval = 3000
                };
                timerSyncUploadStatus.Elapsed += CheckStatusUploadFile;

                timerSyncUploadStatus.Start();
            }
        }

        private void StartStreamMultiple()
        {
            TryExecute(() =>
            {
                if (MobileUserSettingHelper.PingLiveStream && StaticSettings.ListVehilceCamera !=null
                    && StaticSettings.ListVehilceCamera.Count > 0
                    && StaticSettings.ListVehilceOnline != null
                    && StaticSettings.ListVehilceOnline.Count >0)
                {
                    var lstCamera = new List<string>();
                    var lstvehicle = StaticSettings.ListVehilceOnline;
                    foreach (var item in StaticSettings.ListVehilceCamera.Where(x => x.HasVideo).ToList())
                    {
                        var plate = item.VehiclePlate.Contains("_C") ? item.VehiclePlate.Replace("_C", "") : item.VehiclePlate;
                        var model = lstvehicle.FirstOrDefault(x => x.VehiclePlate.ToUpper() == plate.ToUpper());
                        if (model != null)
                        {
                            lstCamera.Add(item.VehiclePlate);
                        }
                        else
                        {
                            var model_c = lstvehicle.FirstOrDefault(x => x.VehiclePlate.ToUpper() == item.VehiclePlate.ToUpper());
                            if (model_c != null)
                            {
                                lstCamera.Add(item.VehiclePlate);
                            }
                        }
                    }
                    var request = new CameraStartMultipleRequest()
                    {
                        VehicleNames = lstCamera,
                        CustomerID = UserInfo.XNCode,
                        Source = (int)CameraSourceType.App,
                        User = UserInfo.UserName,
                        SessionID = StaticSettings.SessionID
                    };

                    RunOnBackground(async () =>
                    {
                        return await streamCameraService.DevicesStartMultiple(request);
                    },
                    (result) =>
                    {
                        if (result)
                        {
                            VehicleNames=lstCamera;
                            StartTimmerPingStream();
                        }
                    });
                }
            });
        }

        private List<string> VehicleNames { get; set; } = new List<string>();

        private void PingStreamMultiple(object sender, ElapsedEventArgs e)
        {
            if (VehicleNames !=null && VehicleNames.Count >0)
            {
                var request = new CameraStartMultipleRequest()
                {
                    VehicleNames = VehicleNames,
                    CustomerID = UserInfo.XNCode,
                    Source = (int)CameraSourceType.App,
                    User = UserInfo.UserName,
                    SessionID = StaticSettings.SessionID
                };

                RunOnBackground(async () =>
                {
                    return await streamCameraService.DevicesPingMultiple(request);
                },
                (result) =>
                {
                    if (result)
                    {
                    }
                });
            }
        }

        private void StartTimmerPingStream()
        {
            if (timerPingStreamCamera == null || !timerPingStreamCamera.Enabled)
            {
                timerPingStreamCamera = new Timer
                {
                    Interval = 60000
                };
                timerPingStreamCamera.Elapsed += PingStreamMultiple;

                timerPingStreamCamera.Start();
            }
        }
    }
}