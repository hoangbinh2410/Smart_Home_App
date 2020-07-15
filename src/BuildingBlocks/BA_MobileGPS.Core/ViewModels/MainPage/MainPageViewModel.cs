using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Events;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Newtonsoft.Json;
using Plugin.Toasts;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Timer timer;

        public MainPageViewModel(INavigationService navigationService, IVehicleOnlineService vehicleOnlineService,
            IAlertService alertService,
            IVehicleDebtMoneyService vehicleDebtMoneyService,
            IAppVersionService appVersionService,
            IAppDeviceService appDeviceService,
            INotificationService notificationService,
            IIdentityHubService identityHubService, IVehicleOnlineHubService vehicleOnlineHubService)
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
            StaticSettings.TimeServer = UserInfo.TimeServer.AddSeconds(1);
            SetTimeServer();
            EventAggregator.GetEvent<TabItemSwitchEvent>().Subscribe(TabItemSwitch);
            EventAggregator.GetEvent<OnResumeEvent>().Subscribe(OnResumePage);
            EventAggregator.GetEvent<OnSleepEvent>().Subscribe(OnSleepPage);
            EventAggregator.GetEvent<SelectedCompanyEvent>().Subscribe(SelectedCompanyChanged);
            EventAggregator.GetEvent<OneSignalOpendEvent>().Subscribe(OneSignalOpend);
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            TryExecute(async () =>
            {
                // Lấy danh sách cảnh báo
                GetCountAlert();

                PushPageFileBase();

                InsertOrUpdateAppDevice();

                await ConnectSignalR();

                GetNoticePopup();

                GetCountVehicleDebtMoney();

                MenuTabConfig();
            });
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            timer.Stop();
            timer.Dispose();
            EventAggregator.GetEvent<TabItemSwitchEvent>().Unsubscribe(TabItemSwitch);
            EventAggregator.GetEvent<OnResumeEvent>().Unsubscribe(OnResumePage);
            EventAggregator.GetEvent<OnSleepEvent>().Unsubscribe(OnSleepPage);
            EventAggregator.GetEvent<SelectedCompanyEvent>().Unsubscribe(SelectedCompanyChanged);
            EventAggregator.GetEvent<OneSignalOpendEvent>().Unsubscribe(OneSignalOpend);
            DisconnectSignalR();
        }

        private async void OnResumePage(bool args)
        {
            if (IsConnected)
            {
                using (new HUDService(MobileResource.Common_Message_Processing))
                {
                    await ConnectSignalR();
                    if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 0)
                    {
                        //Join vào nhóm signalR để nhận dữ liệu online
                        GetListVehicleOnlineResume();
                    }
                    //kiểm tra xem có thông báo nào không
                    GetNofitication();
                }

                if (StaticSettings.TimeServer < DateTime.Now)
                {
                    StaticSettings.TimeServer = DateTime.Now;
                }
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
                });
            }
        }

        private void OnSleepPage(bool obj)
        {
            DisconnectSignalR();
        }

        #endregion Lifecycle

        #region Property

        private int selectedIndex;

        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                SetProperty(ref selectedIndex, value);
                RaisePropertyChanged();
            }
        }    

        private bool vehicleTabVisible;
        public bool VehicleTabVisible
        {
            get { return vehicleTabVisible; }
            set { SetProperty(ref vehicleTabVisible, value);
                RaisePropertyChanged();
            }
        }

        private bool onlineTabVisible;
        public bool OnlineTabVisible
        {
            get { return onlineTabVisible; }
            set { SetProperty(ref onlineTabVisible, value);
                RaisePropertyChanged();
            }
        }

        private bool routeTabVisible;
        public bool RouteTabVisible
        {
            get { return routeTabVisible; }
            set { SetProperty(ref routeTabVisible, value);
                RaisePropertyChanged();
            }
        }

        #endregion Property

        #region PrivateMethod

        private void TabItemSwitch(Tuple<ItemTabPageEnums, object> obj)
        {
            SelectedIndex = (int)obj.Item1;
        }

        public override void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            base.OnConnectivityChanged(sender, e);

            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                OnResumePage(true);
            }
        }
        private void MenuTabConfig()
        {
            VehicleTabVisible = CheckPermision((int)PermissionKeyNames.VehicleView);
            OnlineTabVisible = CheckPermision((int)PermissionKeyNames.ViewModuleOnline);
            RouteTabVisible = CheckPermision((int)PermissionKeyNames.ViewModuleRoute);
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

        private void UpdateTimeServer(object sender, ElapsedEventArgs e)
        {
            StaticSettings.TimeServer = StaticSettings.TimeServer.AddSeconds(1);
        }

        private async Task ConnectSignalR()
        {
            //Hub logout
            await identityHubService.Connect();
            identityHubService.onReceivePushLogoutToAllUserInCompany += onReceivePushLogoutToAllUserInCompany;
            identityHubService.onReceivePushLogoutToUser += onReceivePushLogoutToUser;

            // Khởi tạo signalR
            await vehicleOnlineHubService.Connect();

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
            Debug.Write("OnReceiveSendCarSignalR" + e);
            var carInfo = JsonConvert.DeserializeObject<VehicleOnlineMessage>(e);
            if (carInfo != null)
            {
                var vehicle = StaticSettings.ListVehilceOnline.FirstOrDefault(x => x.VehicleId == carInfo.VehicleId);
                if (vehicle != null && !StateVehicleExtension.IsVehicleDebtMoney(vehicle.MessageId, vehicle.DataExt))
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

        private void GetListVehicleOnlineResume()
        {
            var userID = UserInfo.UserId;
            if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
            {
                userID = Settings.CurrentCompany.UserId;
            }
            int vehicleGroup = 0;

            RunOnBackground(async () =>
            {
                return await vehicleOnlineService.GetListVehicleOnline(userID, vehicleGroup);
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

                    //Join vào nhóm signalR để nhận dữ liệu online
                    JoinGroupSignalRCar(result.Select(x => x.VehicleId.ToString()).ToList());
                }
                else
                {
                    StaticSettings.ListVehilceOnline = new List<VehicleOnline>();
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    EventAggregator.GetEvent<OnReloadVehicleOnline>().Publish(true);
                });
            });
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

        private void GetCountVehicleDebtMoney()
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
                        _ = await NavigationService.NavigateAsync("BaseNavigationPage/VehicleDebtMoneyPage", null, useModalNavigation: true);
                    }
                }
            });
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
                            }, useModalNavigation: true);
                        });
                    }
                }
            }
        }

        #endregion PrivateMethod
    }
}