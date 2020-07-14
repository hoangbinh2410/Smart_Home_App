using AutoMapper;
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
using Prism.Navigation;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using BA_MobileGPS.Core.Events;

namespace BA_MobileGPS.Core.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly IVehicleOnlineService vehicleOnlineService;
        private readonly IAlertService alertService;
        private readonly IHomeService homeService;
        private readonly ISignalRServices signalRServices;
        private readonly IVehicleDebtMoneyService vehicleDebtMoneyService;
        private readonly IAppDeviceService appDeviceService;
        private readonly IAppVersionService appVersionService;
        private readonly INotificationService notificationService;
        private readonly IMapper mapper;
        private Timer timer;
        private List<HomeMenuItemViewModel> MenuReponse = new List<HomeMenuItemViewModel>();

        public bool hasFavorite;
        public bool HasFavorite { get => hasFavorite; set => SetProperty(ref hasFavorite, value); }
        public ICommand TapMenuCommand { get; set; }

        public HomeViewModel(INavigationService navigationService,
            IHomeService homeService, IVehicleOnlineService vehicleOnlineService,
            ISignalRServices signalRServices, IAlertService alertService,
            IVehicleDebtMoneyService vehicleDebtMoneyService, IAppVersionService appVersionService, IAppDeviceService appDeviceService, INotificationService notificationService, IMapper mapper)
            : base(navigationService)
        {
            this.vehicleOnlineService = vehicleOnlineService;
            this.alertService = alertService;
            this.signalRServices = signalRServices;
            this.appVersionService = appVersionService;
            this.vehicleDebtMoneyService = vehicleDebtMoneyService;
            this.appDeviceService = appDeviceService;
            this.homeService = homeService;
            this.notificationService = notificationService;
            this.mapper = mapper;

            TapMenuCommand = new DelegateCommand<object>(OnTappedMenu);

            StaticSettings.TimeServer = UserInfo.TimeServer.AddSeconds(1);

            SetTimeServer();

            EventAggregator.GetEvent<SelectedCompanyEvent>().Subscribe(SelectedCompanyChanged);
            EventAggregator.GetEvent<OnResumeEvent>().Subscribe(OnResumePage);
            EventAggregator.GetEvent<OnSleepEvent>().Subscribe(OnSleepPage);
            EventAggregator.GetEvent<OneSignalOpendEvent>().Subscribe(OneSignalOpend);

            _listfeatures = new ObservableCollection<ItemSupport>();
            _favouriteMenuItems = new ObservableCollection<HomeMenuItemViewModel>();

        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            TryExecute(async () =>
            {
                // Lấy danh sách menu
                GetListMenu();

                // Lấy danh sách cảnh báo
                GetCountAlert();

                PushPageFileBase();

                InsertOrUpdateAppDevice();

                await InitSignalR();

                GetNoticePopup();
            });
        }

        public override void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            base.OnConnectivityChanged(sender, e);

            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                OnResumePage(true);
            }
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue("IsFavoriteChange", out bool isChanged))
            {
                if (isChanged)
                {
                    GenMenu();
                }
            }

            if (parameters.TryGetValue("IsClosedPopupAfterLogin", out bool isClosedPopupAfterLogin))
            {
                if (isClosedPopupAfterLogin)
                {
                    StartOnlinePage();
                }
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

        private void GetListMenu()
        {
            if (!IsConnected)
                return;
            TryExecute(() =>
            {
                RunOnBackground(async () =>
                {
                    return await homeService.GetHomeMenuAsync((int)App.AppType, Settings.CurrentLanguage);
                }, (result) =>
                {
                    if (result != null && result.Count > 0)
                    {
                        MenuReponse = mapper.Map<List<HomeMenuItemViewModel>>(result);

                        GenMenu();
                    }
                });
            });
        }

        private void GenMenu()
        {
            // Lấy danh sách menu ưa thích theo danh sách chuối id
            string menuFavoriteIds = MobileUserSettingHelper.MenuFavorite;
            var menus =
                from m1 in MenuReponse
                join config in UserInfo.Permissions.Distinct() // Lấy theo permission menu
                on m1.PermissionViewID equals config
                join m2 in MenuReponse
                on m1.MenuItemParentID equals m2.PK_MenuItemID
                into gj
                from sub in gj.DefaultIfEmpty()
                where m1.MenuItemParentID != 0
                select new HomeMenuItemViewModel()
                {
                    FK_LanguageTypeID = m1.FK_LanguageTypeID,
                    IconMobile = m1.IconMobile,
                    GroupName = sub?.NameByCulture ?? m1.NameByCulture,
                    MenuKey = m1.MenuKey,
                    NameByCulture = m1.NameByCulture,
                    PK_MenuItemID = m1.PK_MenuItemID,
                    PermissionViewID = m1.PermissionViewID,
                    SortOrder = m1.SortOrder,
                    MenuItemParentID = m1.MenuItemParentID,
                    LanguageCode = m1.LanguageCode,
                };

            GenerateListFeatures(menus.ToList());
            StaticSettings.ListMenuOriginGroup = mapper.Map<List<HomeMenuItem>>(menus);

            if (!string.IsNullOrEmpty(menuFavoriteIds))
            {
                HasFavorite = true;
                var favoritesIdLst = menuFavoriteIds.Split(',').Select(m => int.Parse(m));

                menus =
                    from m in menus
                    join fv in favoritesIdLst
                    on m.PK_MenuItemID equals fv
                    into gj
                    from fv_sub in gj.DefaultIfEmpty()
                    select new HomeMenuItemViewModel()
                    {
                        FK_LanguageTypeID = m.FK_LanguageTypeID,
                        IconMobile = m.IconMobile,
                        GroupName = !(fv_sub == 0) ? MobileResource.Menu_Label_Favorite : m.GroupName,
                        MenuKey = m.MenuKey,
                        NameByCulture = m.NameByCulture,
                        PK_MenuItemID = m.PK_MenuItemID,
                        PermissionViewID = m.PermissionViewID,
                        SortOrder = m.SortOrder,
                        MenuItemParentID = m.MenuItemParentID,
                        LanguageCode = m.LanguageCode,
                        IsFavorited = !(fv_sub == 0),
                    };
            }
            else
            {
                HasFavorite = false;
            }

            var result =
                from m in menus
                orderby m.IsFavorited descending, m.SortOrder, m.GroupName descending
                select m;
            FavouriteMenuItems = result.Where(s => s.IsFavorited).ToObservableCollection();

            StaticSettings.ListMenu = mapper.Map<List<HomeMenuItem>>(result);

        }

        private void GenerateListFeatures(List<HomeMenuItemViewModel> input)
        {
            AllListfeatures.Clear();
            // 6 Item per indicator view in Sflistview
            for (int i = 0; i < input.Count / 6.0; i++)
            {
                var temp = new ItemSupport();
                temp.FeaturesItem = input.Skip(i * 6).Take(6).ToList();
                AllListfeatures.Add(temp);
            }
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

        private void GetListVehicleOnline(bool isNavigate = true)
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
                                GetCountVehicleDebtMoney(isNavigate);
                            }
                            else
                            {
                                if (isNavigate)
                                {
                                    StartOnlinePage();
                                }
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

        private void GetCountVehicleDebtMoney(bool isNavigate = true)
        {
            RunOnBackground(async () =>
            {
                return await vehicleDebtMoneyService.GetCountVehicleDebtMoney(UserInfo.UserId);
            }, async (result) =>
            {
                if (result > 0)
                {
                    if (string.IsNullOrEmpty(Settings.ReceivedNotificationType) && isNavigate)
                    {
                        // gọi sang trang danh sách nợ phí
                        _ = await NavigationService.NavigateAsync("VehicleDebtMoneyPage", null, useModalNavigation: false);
                    }
                }
                else
                {
                    if (isNavigate)
                    {
                        StartOnlinePage();
                    }
                }
            });
        }

        private void StartOnlinePage()
        {
            SafeExecute(async () =>
            {
                if (MobileSettingHelper.IsStartOnlinePage)
                {
                    //cấu hình cty này dùng Cluster thì mới mở forms Cluster
                    if (MobileUserSettingHelper.EnableShowCluster || StaticSettings.ListVehilceOnline.Count >= MobileSettingHelper.CountVehicleUsingCluster)
                    {
                        _ = await NavigationService.NavigateAsync("OnlinePage");
                    }
                    else
                    {
                        _ = await NavigationService.NavigateAsync("OnlinePageNoCluster");
                    }
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

        public ICommand NavigateToFavoriteCommand => new Command(() =>
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/FavoritesConfigurationsPage", null, useModalNavigation: true);
            });
        });

        public void OnTappedMenu(object obj)
        {
            var args = (Syncfusion.ListView.XForms.ItemTappedEventArgs)obj;
            var temp = (HomeMenuItemViewModel)args.ItemData;
            switch (temp.MenuKey)
            {
                case "ListVehiclePage":
                    EventAggregator.GetEvent<TabItemSwitchEvent>().Publish(new Tuple<int, object>(1, ""));
                    break;
                case "OnlinePage":
                    EventAggregator.GetEvent<TabItemSwitchEvent>().Publish(new Tuple<int, object>(2, ""));
                    break;
                case "RoutePage":
                    EventAggregator.GetEvent<TabItemSwitchEvent>().Publish(new Tuple<int, object>(3, ""));
                    break;
                default:
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            if (IsBusy)
                                return;
                            IsBusy = true;

                            if (!(args.ItemData is HomeMenuItemViewModel seletedMenu) || seletedMenu.MenuKey == null)
                            {
                                return;
                            }
                            //await NavigationService.NavigateAsync("NotificationPopup", useModalNavigation: true);
                            using (new HUDService(MobileResource.Common_Message_Processing))
                            {
                                _ = await NavigationService.NavigateAsync(seletedMenu.MenuKey);

                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                        }
                        finally
                        {
                            IsBusy = false;
                        }
                    });
                    break;
            }
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
                        // Lấy danh sách xe lưu vào biến static
                        GetListVehicleOnline(false);
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
                            // Lấy danh sách xe lưu vào biến static
                            GetListVehicleOnline(false);
                        }
                        else
                        {
                            // Lấy danh sách xe lưu vào biến static
                            GetListVehicleOnline();
                        }
                    }
                }
                else
                {
                    // Lấy danh sách xe lưu vào biến static
                    GetListVehicleOnline();
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

        private ObservableCollection<ItemSupport> _listfeatures;

        public ObservableCollection<ItemSupport> AllListfeatures
        {
            get => _listfeatures;

            set
            {
                SetProperty(ref _listfeatures, value);
            }
        }
        private ObservableCollection<HomeMenuItemViewModel> _favouriteMenuItems;

        public ObservableCollection<HomeMenuItemViewModel> FavouriteMenuItems
        {
            get => _favouriteMenuItems;
            set
            {
                SetProperty(ref _favouriteMenuItems, value);
                RaisePropertyChanged();
            }
        }
    }

    public class ItemSupport
    {
        public List<HomeMenuItemViewModel> FeaturesItem { get; set; }
    }
}

