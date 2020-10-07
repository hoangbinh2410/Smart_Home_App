using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.Views.Authentication;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Com.OneSignal;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        #region Constructor

        private readonly IAuthenticationService authenticationService;
        private readonly IDBVersionService dBVersionService;
        private readonly IResourceService resourceService;
        private readonly ILanguageService languageTypeService;
        private readonly IAppVersionService appVersionService;
        private readonly INotificationService notificationService;
        private readonly IPingServerService pingServerService;

        public LoginPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService,
            IAppVersionService appVersionService, IDBVersionService dBVersionService,
            IResourceService resourceService, ILanguageService languageTypeService,
            INotificationService notificationService, IPingServerService pingServerService)
            : base(navigationService)
        {
            this.authenticationService = authenticationService;
            this.appVersionService = appVersionService;
            this.resourceService = resourceService;
            this.languageTypeService = languageTypeService;
            this.dBVersionService = dBVersionService;
            this.notificationService = notificationService;
            this.pingServerService = pingServerService;

            EventAggregator.GetEvent<SelectLanguageTypeEvent>().Subscribe(UpdateLanguage);
            InitValidations();
            if (App.CurrentLanguage == CultureCountry.English)
            {
                language = new LanguageRespone()
                {
                    CodeName = CultureCountry.English,
                    Icon = "flag_us.png",
                    Description = "English",
                    PK_LanguageID = 2
                };
            }
            else
            {
                language = new LanguageRespone()
                {
                    CodeName = CultureCountry.Vietnamese,
                    Icon = "flag_vn.png",
                    Description = "Tiếng Việt",
                    PK_LanguageID = 1
                };
            }
        }

        #endregion Constructor

        #region Init

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                if (parameters.TryGetValue(ParameterKey.Logout, out bool isLogout))
                {
                    if (isLogout)
                    {
                        GetInfomation(true);
                    }
                }
                else
                {
                    GetInfomation();
                }
            }
            else
            {
                GetInfomation();
            }
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();

            //_ = MobileResource.Get("Login_UserNameProperty_NullOrEmpty");
        }

        private void GetInfomation(bool isLogout = false)
        {
            GetMobileSetting();
            if (!isLogout)
            {
                GetMobileVersion();
            }
            if (Settings.Rememberme)
            {
                UserName.Value = Settings.UserName;
                Password.Value = Settings.Password;
                Rememberme = true;
            }
            else
            {
                UserName.Value = string.Empty;
                Password.Value = string.Empty;
                Rememberme = false;
            }
            GetNoticePopup();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.TryGetValue("popupitem", out LoginPopupItem obj))
                {
                    NavigateLoginPreview(obj);
                }
            }
        }

        private void NavigateLoginPreview(LoginPopupItem item)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                switch (item.ItemType)
                {
                    case LoginPopupItemType.OfflinePage:
                        _ = await NavigationService.NavigateAsync(item.Url);
                        break;

                    case LoginPopupItemType.Network:
                        await Launcher.OpenAsync(new Uri(item.Url));
                        break;

                    case LoginPopupItemType.Manual:
                        _ = await NavigationService.NavigateAsync(item.Url);
                        break;

                    case LoginPopupItemType.Guarantee:
                        await Launcher.OpenAsync(new Uri(item.Url));
                        break;

                    case LoginPopupItemType.RegisterSupport:
                        _ = await NavigationService.NavigateAsync(item.Url, null, useModalNavigation: true);
                        break;

                    case LoginPopupItemType.BAGPSExperience:
                        await Launcher.OpenAsync(new Uri(item.Url));
                        break;

                    default:
                        _ = await NavigationService.NavigateAsync(item.Url, null, useModalNavigation: true);
                        break;
                }
            });
        }

        /// <summary>Kiểm tra xem server sống hay chết</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/26/2020   created
        /// </Modified>
        private void PingServerStatus()
        {
            RunOnBackground(async () =>
            {
                return await pingServerService.PingServerStatus();
            }, (items) =>
            {
                if (items != null && items.Data == true)
                {
                    GetNoticePopup();
                }
                else
                {
                    PushPageFileBase();
                }
            });
        }

        private Task GetMobileSetting()
        {
            return RunOnBackground(async () =>
            {
                return await resourceService.GetAllMobileConfigs(App.AppType);
            },
            (result) =>
            {
                if (result != null && result.Count > 0)
                {
                    MobileSettingHelper.SetData(result);
                }
            });
        }

        private void GetVersionDBLogin()
        {
            TryExecute(() =>
            {
                // get db local
                var dbLocal = dBVersionService.All();
                //nếu db local có dữ liệu thì mới gọi xuống server check cập nhật . không có thì phải vào trang insall cập nhật
                if (dbLocal == null || dbLocal.Count() <= 0)
                {
                    Settings.IsChangeDataLocalDB = true;
                    return;
                }

                RunOnBackground(async () =>
                {
                    return await dBVersionService.GetVersionDataBase((int)App.AppType);
                },
                (result) =>
                {
                    if (result != null && result.Count > 0)
                    {
                        foreach (var item in result)
                        {
                            if (Enum.TryParse<LocalDBNames>(item.TableName.ToUpper(), out var tableName))
                            {
                                var lastUpdateDBLocal = dbLocal.FirstOrDefault(x => x.TableName == item.TableName);
                                // khác version và có thời gian update lớn hơn thời gian lưu trong db thì mới cập nhật
                                if (lastUpdateDBLocal != null && lastUpdateDBLocal.UpdatedDate <= item.UpdatedDate && item.VersionDB != lastUpdateDBLocal.VersionDB)
                                {
                                    Settings.IsChangeDataLocalDB = true;
                                }
                            }
                        }
                    }
                });
            });
        }

        private void GetMobileVersion()
        {
            if (string.IsNullOrEmpty(Settings.TempVersionName)) // nếu lần đầu cài app
            {
                Settings.TempVersionName = AppVersion;
                Settings.AppVersionDB = AppVersion;
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
                        Settings.AppVersionDB = versionDB.VersionName;
                        string title = "Cập nhập phiên bản mới";
                        //string message = !string.IsNullOrEmpty(versionDB.Description) ? versionDB.Description : "Cập nhập phiên bản mới";
                        string accept = MobileResource.Common_Button_Update;
                        string later = MobileResource.Common_Button_Update_Later;
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
                            if (Settings.TempVersionName != versionDB.VersionName) // khác phiên bản hiện tại khi bỏ qua
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    var action = await PageDialog.DisplayActionSheetAsync(title, null, null, accept, later, cancel);

                                    if (action == accept) // cập nhật
                                    {
                                        await Launcher.OpenAsync(new Uri(versionDB.LinkDownload));
                                    }
                                    else if (action == later) // cập nhật sau
                                    {
                                        return;
                                    }
                                    else if (action == cancel)
                                    {
                                        Settings.TempVersionName = versionDB.VersionName;
                                        return;
                                    }
                                    else //bỏ qua
                                    {
                                        // lưu biến vào đây
                                        return;

                                    }
                                });
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(Settings.UserName) && !string.IsNullOrEmpty(Settings.Password))
                                {
                                    if (Settings.Rememberme)
                                    {
                                        LoginCommand.Execute(null);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // lưu version DB hiện tại
                        Settings.TempVersionName = versionDB.VersionName;
                        Settings.AppVersionDB = versionDB.VersionName;

                        if (!string.IsNullOrEmpty(Settings.UserName) && !string.IsNullOrEmpty(Settings.Password))
                        {
                            if (Settings.Rememberme)
                            {
                                LoginCommand.Execute(null);
                            }
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Settings.UserName) && !string.IsNullOrEmpty(Settings.Password))
                    {
                        if (Settings.Rememberme)
                        {
                            LoginCommand.Execute(null);
                        }
                    }
                }
            });
        }

        public override void OnDestroy()
        {
            EventAggregator.GetEvent<SelectLanguageTypeEvent>().Unsubscribe(UpdateLanguage);
        }

        #endregion Init

        #region Event subcribe

        public void UpdateLanguage(LanguageRespone param)
        {
            if (param == null)
                return;

            try
            {
                Language = param;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (Settings.CurrentLanguage != Language.CodeName)
                    {
                        Settings.CurrentLanguage = Language.CodeName;

                        App.CurrentLanguage = Language.CodeName;

                        //Update lại ngôn ngữ trên giao diện
                        MobileResource._DicMobileResource = null;

                        await NavigationService.NavigateAsync("/ChangeLanguage");
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_ErrorTryAgain);
            }
        }

        #endregion Event subcribe

        #region Property

        public string AppVersion => appVersionService.GetAppVersion();

        public ValidatableObject<string> UserName { get; set; }

        public ValidatableObject<string> Password { get; set; }

        private LanguageRespone language;

        public LanguageRespone Language
        {
            get => language;
            set
            {
                SetProperty(ref language, value);
                RaisePropertyChanged();
            }
        }

        private bool rememberme;

        public bool Rememberme
        {
            get => rememberme;
            set
            {
                Settings.Rememberme = value;
                SetProperty(ref rememberme, value);
            }
        }

        #endregion Property

        #region ICommand

        public ICommand PushtoLanguageCommand => new DelegateCommand(() =>
        {
            SafeExecute(async () => await NavigationService.NavigateAsync("BaseNavigationPage/LanguagePage", null, useModalNavigation: true));
        });

        public ICommand ForgotPasswordCommand => new DelegateCommand(() =>
        {
            PopupNavigation.Instance.PushAsync(new ForgotPasswordPopup());
        });

        public ICommand OpenLoginFragmentCommand => new DelegateCommand(() =>
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("LoginPreviewFeaturesPage");
            });

        });

        public ICommand OpenWebGPSCommand => new DelegateCommand(() =>
        {
            SafeExecute(async () => await Launcher.OpenAsync(new Uri(MobileSettingHelper.LinkBAGPS)));
        });

        [Obsolete]
        public ICommand SendEmailCommand => new DelegateCommand(() =>
        {
            try
            {
                if (!string.IsNullOrEmpty(MobileSettingHelper.EmailSupport))
                {
                    string shareurl = String.Empty;
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var email = Regex.Replace(MobileSettingHelper.EmailSupport, @"[^\u0000-\u00FF]", string.Empty);
                        shareurl = "mailto:" + email;
                    }
                    else
                    {
                        shareurl = "mailto:" + MobileSettingHelper.EmailSupport;
                    }
                    Device.OpenUri(new Uri(shareurl));
                }
            }
            catch
            {
                Device.OpenUri(new Uri("https://accounts.google.com/"));
            }
        });

        public ICommand LoginCommand => new DelegateCommand(() =>
        {
            SafeExecute(async () =>
            {
                if (IsConnected)
                {
                    bool isValid = Validate();
                    if (isValid)
                    {
                        using (new HUDService(MobileResource.Common_Message_Processing))
                        {
                            var request = new LoginRequest
                            {
                                UserName = UserName.Value.Trim(),
                                Password = Password.Value.Trim(),
                                AppType = App.AppType
                            };
                            // Lấy thông tin token
                            var user = await authenticationService.LoginStreamAsync(request);
                            if (user != null)
                            {
                                switch (user.Status)
                                {
                                    case LoginStatus.Success://Đăng nhập thành công
                                        if (!string.IsNullOrEmpty(user.AccessToken))
                                        {
                                            OnLoginSuccess(user);
                                        }
                                        else
                                        {
                                            DisplayMessage.ShowMessageInfo(MobileResource.Login_Message_AccountPasswordIncorrect);
                                            StaticSettings.User = null;
                                        }
                                        break;

                                    case LoginStatus.LoginFailed://Đăng nhập không thành công

                                        DisplayMessage.ShowMessageInfo(MobileResource.Login_Message_AccountPasswordIncorrect);

                                        StaticSettings.User = null;

                                        break;

                                    case LoginStatus.UpdateRequired:
                                        StaticSettings.User = null;

                                        break;

                                    case LoginStatus.Locked://Tài khoản đang bị khóa

                                        DisplayMessage.ShowMessageInfo(MobileResource.Login_Message_AccountLocked);

                                        StaticSettings.User = null;

                                        break;

                                    case LoginStatus.WrongAppType:

                                        DisplayMessage.ShowMessageInfo(MobileResource.Login_Message_AccountAllowedSystem);
                                        break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
                }
            });
        });

        #endregion ICommand

        #region PrivateMethod

        private bool Validate()
        {
            return UserName.Validate() && Password.Validate();
        }

        private void InitValidations()
        {
            UserName = new ValidatableObject<string>();
            Password = new ValidatableObject<string>();

            UserName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = MobileResource.Login_UserNameProperty_NullOrEmpty });
            Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = MobileResource.Login_PasswordProperty_NullOrEmpty });
        }

        private void GetLanguageType()
        {
            Task.Run(() =>
           {
               var lstlanguage = languageTypeService.Find(x => x.CodeName == Settings.CurrentLanguage)?.FirstOrDefault();

               if (lstlanguage != null)
               {
                   Language = lstlanguage;
               }
           });
        }

        private async void OnLoginSuccess(LoginResponse user)
        {
            try
            {
                //Nếu đăng nhập tài khoản khác thì xóa CurrentCompany đi
                if (!string.IsNullOrEmpty(Settings.UserName) && Settings.UserName != UserName.Value && Settings.CurrentCompany != null)
                {
                    Settings.CurrentCompany = null;
                }
                //nếu nhớ mật khẩu thì lưu lại thông tin username và password
                if (Rememberme)
                {
                    Settings.UserName = UserName.Value;
                    Settings.Password = Password.Value;
                    Settings.Rememberme = true;
                }
                StaticSettings.Token = user.AccessToken;
                StaticSettings.User = user;
                OneSignal.Current.SendTag("UserID", user.UserId.ToString().ToUpper());
                CultureInfo.CurrentCulture = new CultureInfo(Language.CodeName);
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(Language.CodeName);
                //nếu cần đổi mật khẩu thì mở trang đổi mật khẩu
                if (user.IsNeedChangePassword)
                {
                    await NavigationService.NavigateAsync("BaseNavigationPage/ChangePasswordPage", useModalNavigation: true);
                }
                else
                {
                    await NavigationService.NavigateAsync("/MainPage");
                }

            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>Gọi thông tin popup khi đăng nhập</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/26/2020   created
        /// </Modified>
        private void GetNoticePopup()
        {
            RunOnBackground(async () =>
            {
                return await notificationService.GetNotificationWhenLogin(App.AppType);
            }, (items) =>
            {
                if (items != null && items.Data != null)
                {
                    if (items.Data.IsAlwayShow) // true luôn luôn hiển thị
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await NavigationService.NavigateAsync("NotificationPopupWhenLogin", parameters: new NavigationParameters
                             {
                                 { ParameterKey.NotificationKey, items.Data }
                            });
                        });
                    }
                    else
                    {
                        if (Settings.NoticeIdWhenLogin != items.Data.PK_NoticeContentID)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await NavigationService.NavigateAsync("NotificationPopupWhenLogin", parameters: new NavigationParameters
                             {
                                 { ParameterKey.NotificationKey, items.Data }
                            });
                            });
                        }
                    }
                }
            });
        }

        /// <summary>Lấy thông tin từ firebase</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/26/2020   created
        /// </Modified>
        private void PushPageFileBase()
        {
            //nếu người dùng click vào mở thông báo firebase thì vào trang thông báo luôn
            if (!string.IsNullOrEmpty(Settings.ReceivedNotificationType))
            {
                if (Settings.ReceivedNotificationType == (((int)FormOfNoticeTypeEnum.NoticeWhenLogin).ToString()))
                {
                    Settings.ReceivedNotificationType = string.Empty;
                    if (!string.IsNullOrEmpty(Settings.ReceivedNotificationValue))
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await NavigationService.NavigateAsync("NotificationPopupWhenLogin", parameters: new NavigationParameters
                             {
                                 { ParameterKey.NotificationForm, Settings.ReceivedNotificationValue }
                            });
                        });
                    }
                }
            }
        }

        /// <summary>Kiểm tra mạng lấy lại thông tin khi có mạng</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ConnectivityChangedEventArgs"/> instance containing the event data.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/27/2020   created
        /// </Modified>
        public override void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            base.OnConnectivityChanged(sender, e);
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                GetInfomation();
            }
        }

        #endregion PrivateMethod
    }
}