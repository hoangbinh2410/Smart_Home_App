using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Newtonsoft.Json;
using Plugin.Toasts;
using Prism.Commands;
using Prism.Mvvm;
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
        private readonly IVehicleOnlineService vehicleOnlineService;
        private readonly IAlertService alertService;
        private readonly ISignalRServices signalRServices;
        private readonly IAppDeviceService appDeviceService;
        private readonly IAppVersionService appVersionService;
        private readonly IVehicleDebtMoneyService vehicleDebtMoneyService;
        private readonly INotificationService notificationService;

        private bool isVisibleTabItem;
        public bool IsVisibleTabItem { get => isVisibleTabItem; set => SetProperty(ref isVisibleTabItem, value); }

        private Timer timer;
        public MainPageViewModel(INavigationService navigationService, IVehicleOnlineService vehicleOnlineService,
            IAlertService alertService, ISignalRServices signalRServices, IAppVersionService appVersionService, IAppDeviceService appDeviceService,
        IVehicleDebtMoneyService vehicleDebtMoneyService, INotificationService notificationService) : base(navigationService)
        {
            this.vehicleOnlineService = vehicleOnlineService;
            this.alertService = alertService;
            this.signalRServices = signalRServices;
            this.appVersionService = appVersionService;
            this.appDeviceService = appDeviceService;
            this.vehicleDebtMoneyService = vehicleDebtMoneyService;
            this.notificationService = notificationService;

            EventAggregator.GetEvent<OnResumeEvent>().Subscribe(OnResumePage);
            EventAggregator.GetEvent<OnSleepEvent>().Subscribe(OnSleepPage);
            EventAggregator.GetEvent<ShowTabItemEvent>().Subscribe(ShowTabItem);

            IsVisibleTabItem = true;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            TryExecute(async () =>
            {
                // Lấy danh sách menu
                //GetListMenu();

                //GetListVehicleOnline();

                // Lấy danh sách cảnh báo
                GetCountAlert();

                PushPageFileBase();

                InsertOrUpdateAppDevice();

                await InitSignalR();

                GetNoticePopup();
            });
          
        }

        public override void OnDestroy()
        {
            timer.Stop();
            timer.Dispose();

            EventAggregator.GetEvent<SelectedCompanyEvent>().Unsubscribe(SelectedCompanyChanged);
            EventAggregator.GetEvent<OnResumeEvent>().Unsubscribe(OnResumePage);
            EventAggregator.GetEvent<OnSleepEvent>().Unsubscribe(OnSleepPage);
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

        private void OneSignalOpend(bool obj)
        {
            PushPageFileBase();
        }

        private void SetTimeServer()
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
            StaticSettings.TimeServer = StaticSettings.TimeServer.AddSeconds(1);
        }


        private void GetListVehicleOnline()
        {
            TryExecute(() =>
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

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            //Nếu app có dùng trang danh sách nợ phí thì mới gọi xuống lấy danh sách nợ phí
                            if (MobileSettingHelper.IsUseVehicleDebtMoney)
                            {
                                // hàm get danh sách xe nợ phí
                                GetCountVehicleDebtMoney();
                            }
                            else
                            {
                                //if (isNavigate)
                                //{
                                //    StartOnlinePage();
                                //}
                            }
                        });
                    }
                    else
                    {
                        StaticSettings.ListVehilceOnline = new List<VehicleOnline>();
                    }
                });
            });
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

        private void JoinGroupSignalRCar(List<string> lstGroup)
        {
            try
            {
                //Thoát khỏi nhóm nhận thông tin xe
                signalRServices.JoinGroupReceivedVehicleID(string.Join(",", lstGroup));
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
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
                        _ = await NavigationService.NavigateAsync("VehicleDebtMoneyPage", null, useModalNavigation: false);
                    }
                }
                else
                {
                    //if (isNavigate)
                    //{
                    //    StartOnlinePage();
                    //}
                }
            });
        }

        private async Task InitSignalR()
        {
            // Khởi tạo signalR
            await signalRServices.Connect();

            signalRServices.onReceiveSendCarSignalR += OnReceiveSendCarSignalR;

            signalRServices.onReceiveAlertSignalR += OnReceiveAlertSignalR;

            signalRServices.onReceiveNotificationSignalR += OnReceiveNotificationSignalR;

            signalRServices.onReceiveLogoutAllUserInCompany += onReceiveLogoutAllUserInCompany;

            //Join vào nhóm signalR để nhận dữ liệu online
            JoinGroupSignalRCompany(UserInfo.CompanyId.ToString());
        }

        private async Task ConnectSignalR()
        {
            // Khởi tạo signalR
            await signalRServices.Connect();

            signalRServices.onReceiveSendCarSignalR += OnReceiveSendCarSignalR;

            signalRServices.onReceiveAlertSignalR += OnReceiveAlertSignalR;

            signalRServices.onReceiveNotificationSignalR += OnReceiveNotificationSignalR;

            signalRServices.onReceiveLogoutAllUserInCompany += onReceiveLogoutAllUserInCompany;

            //Join vào nhóm signalR để nhận dữ liệu online
            JoinGroupSignalRCompany(UserInfo.CompanyId.ToString());
        }

        private void OnReceiveNotificationSignalR(object sender, int message)
        {
            TryExecute(() =>
            {
                if (message == (int)NotificationTypeEnum.ChangePassword)
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

        private void onReceiveLogoutAllUserInCompany(object sender, string message)
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

        private void DisconnectSignalR()
        {
            //thoát khỏi nhóm nhận xe
            if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 0)
            {
                LeaveGroupSignalRCar(StaticSettings.ListVehilceOnline.Select(x => x.VehicleId.ToString()).ToList());
            }

            signalRServices.onReceiveSendCarSignalR -= OnReceiveSendCarSignalR;
            signalRServices.onReceiveAlertSignalR -= OnReceiveAlertSignalR;
            signalRServices.onReceiveNotificationSignalR -= OnReceiveNotificationSignalR;
            signalRServices.onReceiveLogoutAllUserInCompany -= onReceiveLogoutAllUserInCompany;

            //Join vào nhóm signalR để nhận dữ liệu online
            LeaveGroupCompany(UserInfo.CompanyId.ToString());

            signalRServices.Disconnect();
        }

        private void JoinGroupSignalRCompany(string companyID)
        {
            try
            {
                //Thoát khỏi nhóm nhận thông tin xe
                signalRServices.JoinGroupCompany(companyID);
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
                signalRServices.LeaveGroupReceivedVehicleID(string.Join(",", lstGroup));
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void LeaveGroupCompany(string companyID)
        {
            try
            {
                //Thoát khỏi nhóm nhận thông tin xe
                signalRServices.LeaveGroupCompany(companyID);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
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

        private void ShowTabItem(bool check)
        {
            IsVisibleTabItem = check;
        }
    }
}
