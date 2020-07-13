using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Com.OneSignal;
using Prism;
using Prism.Ioc;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using BA_MobileGPS.Core.Views;

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
            language = new LanguageRespone()
            {
                CodeName = CultureCountry.Vietnamese,
                Icon = "flag_vn.png",
                Description = "Tiếng Việt",
                PK_LanguageID = 1
            };
            InitValidations();
        }
        #endregion

        #region Init
        public override void Initialize(INavigationParameters parameters)
        {
            GetInfomation();
        }

        void GetInfomation()
        {
            GetMobileSetting();
            GetVersionDBLogin();
            GetMobileVersion();

            if (!string.IsNullOrEmpty(Settings.CurrentLanguage))
            {
                GetLanguageType();
            }

            if (!string.IsNullOrEmpty(Settings.UserName) && !string.IsNullOrEmpty(Settings.Password))
            {
                UserName.Value = Settings.UserName;
                Password.Value = Settings.Password;
                if (Settings.Rememberme)
                {
                    rememberme = true;
                }
            }
            else
            {
                UserName.Value = string.Empty;
                Password.Value = string.Empty;
                rememberme = false;
            }
            PingServerStatus();
        }

        /// <summary>Kiểm tra xem server sống hay chết</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/26/2020   created
        /// </Modified>
        void PingServerStatus()
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
                    result.ForEach(item =>
                    {
                        if (!MobileSettingHelper.DicMobileConfigurations.ContainsKey(item.Name))
                        {
                            MobileSettingHelper.DicMobileConfigurations.Add(item.Name, item.Value);
                        }
                    });

                    //EmailSupport = MobileSettingHelper.EmailSupport;
                    //Hotline = MobileSettingHelper.HotlineGps;


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
                                    else //bỏ qua
                                    {
                                        // lưu biến vào đây
                                        Settings.TempVersionName = versionDB.VersionName;
                                        return;
                                    }
                                });
                            }
                        }
                    }
                    else
                    {
                        // lưu version DB hiện tại 
                        Settings.TempVersionName = versionDB.VersionName;
                        Settings.AppVersionDB = versionDB.VersionName;
                    }
                }
            });
        }

        public override void OnDestroy()
        {
            EventAggregator.GetEvent<SelectLanguageTypeEvent>().Unsubscribe(UpdateLanguage);
        }
        #endregion

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
        #endregion

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

        //private string hotline;
        //public string Hotline { get => hotline; set => SetProperty(ref hotline, value); }

        //private string emailSupport;
        //public string EmailSupport { get => emailSupport; set => SetProperty(ref emailSupport, value); }

       

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
            SafeExecute(() =>
            {
                var popupServices = PrismApplicationBase.Current.Container.Resolve<IPopupServices>();
                var title = MobileResource.Login_ForgotPassword_PopupTitle;
                var content = MobileResource.Login_ForgotPassword_PopupContent;
                popupServices.ShowNotificationIconPopup(title, content, "ic_Lock.png", Color.Blue, Views.IconPosititon.Left);
            });
        });

        public ICommand OpenLoginFragmentCommand => new DelegateCommand(() =>
        {
            PopupNavigation.Instance.PushAsync(new NonLoginFeaturesPopup());
        });
        //public ICommand PushToRegisterConsultCommand => new DelegateCommand(() =>
        //{
        //    SafeExecute(async () => await NavigationService.NavigateAsync("BaseNavigationPage/RegisterConsultPage", null, useModalNavigation: true));
        //    //SafeExecute(async () => await NavigationService.NavigateAsync("MenuNavigationPage/VerifyCodeOtpPage", null, useModalNavigation: true));
        //});

        //public ICommand PushToExperienceBACommand => new DelegateCommand(() =>
        //{
        //    SafeExecute(async () => await Launcher.OpenAsync(new Uri(MobileSettingHelper.LinkExperience)));
        //});

        public ICommand CallHotLineCommand => new DelegateCommand(() =>
        {
            if (!string.IsNullOrEmpty(MobileSettingHelper.HotlineGps))
            {
                PhoneDialer.Open(MobileSettingHelper.HotlineGps);
            }
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
                                UserName = UserName.Value,
                                Password = Password.Value,
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

                                        DisplayMessage.ShowMessageInfo(MobileResource.Login_Message_AccountPasswordIncorrect);

                                        //DisplayMessage.ShowMessageInfo("Tài khoản của bạn không được phép đăng nhập hệ thống");
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
        #endregion

        #region PrivateMethod
        private bool Validate()
        {
            return UserName.Validate() && Password.Validate();
        }

        private void InitValidations()
        {
            UserName = new ValidatableObject<string>();
            Password = new ValidatableObject<string>();

            UserName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = MobileResource.Common_Property_NullOrEmpty(MobileResource.Login_Textbox_UserName) });
            Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = MobileResource.Common_Property_NullOrEmpty(MobileResource.Login_Textbox_Password) });
        }

        private void GetLanguageType()
        {
            TryExecute(() =>
            {
                //lấy hết resource theo ngôn ngữ
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
                StaticSettings.Token = user.AccessToken;
                StaticSettings.User = user;
                Settings.UserName = UserName.Value;
                Settings.Password = Password.Value;
                //Neu co su thay doi ngon ngu thi update lai ngon nghu
                if (MobileUserSettingHelper.DefautLanguage != Language.PK_LanguageID)
                {
                    //Update lại userlanguage
                    var ischanglanguage = await languageTypeService.UpdateLanguageByUser(new RequestUpdateLanguage()
                    {
                        FK_LanguageID = Language.PK_LanguageID,
                        FK_UserID = user.UserId
                    });
                }

                OneSignal.Current.SendTag("UserID", user.UserId.ToString().ToUpper());

                //nếu nhớ mật khẩu thì lưu lại thông tin username và password
                if (Rememberme)
                {
                    Settings.Rememberme = true;
                }
                else
                {
                    Settings.Rememberme = false;
                }
                CultureInfo.CurrentCulture = new CultureInfo(Language.CodeName);

                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(Language.CodeName);
                //nếu cần xác thực OTP thì mở trang xác thực OTP
                //if (user.IsNeededOtp)
                //{
                //    await NavigationService.NavigateAsync("MenuNavigationPage/VerifyCodeOtpPage", null, useModalNavigation: true);
                //}
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
