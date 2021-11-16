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
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class LoginPageViewModel : ViewModelBaseLogin
    {
        #region Constructor

        private readonly IAuthenticationService authenticationService;
        private readonly IMobileSettingService mobileSettingService;

        public LoginPageViewModel(INavigationService navigationService,
            IAuthenticationService authenticationService,
            IMobileSettingService mobileSettingService)
            : base(navigationService)
        {
            this.authenticationService = authenticationService;
            this.mobileSettingService = mobileSettingService;
            InitValidations();
            isShowRegisterSupport = false;
        }

        #endregion Constructor

        #region Init

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(ParameterKey.Logout, out bool isLogout))
            {
                IsShowRegisterSupport = MobileSettingHelper.IsUseRegisterSupport;
                if (!isLogout)
                {
                    GetMobileVersion();
                }
            }
            else
            {
                GetMobileSetting();
                GetMobileVersion();
            }
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();

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
            AddLanguage();
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
                else if (parameters.TryGetValue(ParameterKey.ChangeLanguge, out LanguageRespone languageRespone))
                {
                    if (languageRespone != null)
                    {
                        UpdateLanguage(languageRespone);
                    }
                }
                else if (parameters.TryGetValue("LoginFailedPopup", out bool isforgot))
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (isforgot)
                        {
                            if (MobileSettingHelper.IsUseForgotpassword)
                            {
                                await NavigationService.NavigateAsync("NavigationPage/ForgotPasswordPage", null, useModalNavigation: true, true);
                            }
                            else
                            {
                                await PopupNavigation.Instance.PushAsync(new ForgotPasswordPopup());
                            }
                        }
                        else
                        {
                            var actioncall = await PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification,
                                string.Format(MobileResource.Login_Message_PleaseCall, MobileSettingHelper.HotlineGps),
                                MobileResource.Common_Label_Contact, MobileResource.Common_Message_Skip);
                            if (actioncall)
                            {
                                if (!string.IsNullOrEmpty(MobileSettingHelper.HotlineGps))
                                {
                                    PhoneDialer.Open(MobileSettingHelper.HotlineGps);
                                }
                            }
                        }
                    });
                }
            }
        }

        #endregion Init

        #region Property

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

        private bool isShowRegisterSupport;

        public bool IsShowRegisterSupport
        {
            get => isShowRegisterSupport;
            set
            {
                SetProperty(ref isShowRegisterSupport, value);
                RaisePropertyChanged();
            }
        }

        #endregion Property

        #region ICommand

        public ICommand PushtoLanguageCommand => new DelegateCommand(() =>
         {
             SafeExecute(async () =>
             {
                 await NavigationService.NavigateAsync("BaseNavigationPage/LanguagePage", null, useModalNavigation: true, true);
             });
         });

        public ICommand PushtoRegisterSupportCommand => new DelegateCommand(() =>
         {
             SafeExecute(async () =>
             {
                 _ = await NavigationService.NavigateAsync("BaseNavigationPage/RegisterConsultPage", null, useModalNavigation: true, true);
             });
         });

        public ICommand ForgotPasswordCommand => new DelegateCommand(() =>
         {
             SafeExecute(async () =>
             {
                 if (MobileSettingHelper.IsUseForgotpassword)
                 {
                     await NavigationService.NavigateAsync("NavigationPage/ForgotPasswordPage", null, useModalNavigation: true, true);
                 }
                 else
                 {
                     await PopupNavigation.Instance.PushAsync(new ForgotPasswordPopup());
                 }
             });
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
             Login();
         });

        #endregion ICommand

        #region PrivateMethod

        private void AddLanguage()
        {
            if (App.CurrentLanguage == CultureCountry.English)
            {
                Language = new LanguageRespone()
                {
                    CodeName = CultureCountry.English,
                    Icon = "flag_us.png",
                    Description = "English",
                    PK_LanguageID = 2
                };
            }
            else if (App.CurrentLanguage == CultureCountry.Laos)
            {
                Language = new LanguageRespone()
                {
                    CodeName = CultureCountry.Laos,
                    Icon = "flag_la.png",
                    Description = "ລາວ (ສ.ປ.ປ. ລາວ)",
                    PK_LanguageID = 3
                };
            }
            else
            {
                Language = new LanguageRespone()
                {
                    CodeName = CultureCountry.Vietnamese,
                    Icon = "flag_vn.png",
                    Description = "Tiếng Việt",
                    PK_LanguageID = 1
                };
            }
        }

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

        private void Login()
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
                                            StaticSettings.User = null;
                                            Device.BeginInvokeOnMainThread(async () =>
                                            {
                                                await PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification, MobileResource.Login_Message_AccountPasswordIncorrect, MobileResource.ForgotPassword_Label_TilePage, MobileResource.ForgotAccount_Label_TilePage);
                                            });
                                        }
                                        break;

                                    case LoginStatus.LoginFailed://Đăng nhập không thành công
                                        StaticSettings.User = null;
                                        OnLoginFailed();
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

                                    case LoginStatus.UserLoginOnlyWeb:

                                        DisplayMessage.ShowMessageInfo(MobileResource.Login_Message_LoginWebOnly);
                                        break;

                                    case LoginStatus.LicenseAppFailed:
                                        DisplayMessage.ShowMessageInfo(MobileResource.Login_Message_LoginLicenseAppFailed);
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
                        _ = await NavigationService.NavigateAsync(item.Url, null, useModalNavigation: true, true);
                        break;

                    case LoginPopupItemType.BAGPSExperience:
                        await Launcher.OpenAsync(new Uri(item.Url));
                        break;

                    case LoginPopupItemType.Facebook:
                    case LoginPopupItemType.Zalo:
                    case LoginPopupItemType.Youtube:
                    case LoginPopupItemType.Tiktok:
                        await Launcher.OpenAsync(new Uri(item.Url));
                        break;

                    default:
                        _ = await NavigationService.NavigateAsync(item.Url, null, useModalNavigation: true, true);
                        break;
                }
            });
        }

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

        private void GetMobileSetting()
        {
            RunOnBackground(async () =>
            {
                return await mobileSettingService.GetAllMobileConfigs(App.AppType);
            },
           (result) =>
           {
               if (result != null && result.Count > 0)
               {
                   MobileSettingHelper.SetData(result);
                   IsShowRegisterSupport = MobileSettingHelper.IsUseRegisterSupport;
               }
           });
        }

        private void GetMobileVersion()
        {
            RunOnBackground(async () =>
            {
                return await mobileSettingService.GetMobileVersion(Device.RuntimePlatform.ToString(), (int)App.AppType);
            },
            (versionDB) =>
            {
                var appVersion = VersionTracking.CurrentVersion;
                if (string.IsNullOrEmpty(Settings.TempVersionName)) // nếu lần đầu cài app
                {
                    Settings.TempVersionName = appVersion;
                    Settings.AppVersionDB = appVersion;
                }
                if (versionDB != null && !string.IsNullOrEmpty(versionDB.VersionName) && !string.IsNullOrEmpty(versionDB.LinkDownload))
                {
                    // Nếu giá trị bị null hoặc giá trị đường link thay đổi => cập nhật lại
                    if (string.IsNullOrEmpty(Settings.AppLinkDownload) || !versionDB.LinkDownload.Equals(Settings.AppLinkDownload, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Settings.AppLinkDownload = versionDB.LinkDownload;
                    }

                    if (!versionDB.VersionName.Equals(appVersion, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Settings.AppVersionDB = versionDB.VersionName;
                        string title = MobileResource.Login_Message_UpdateVersionNew;
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
                                GetNoticePopup();
                            }
                        }
                    }
                    else
                    {
                        // lưu version DB hiện tại
                        Settings.TempVersionName = versionDB.VersionName;
                        Settings.AppVersionDB = versionDB.VersionName;

                        GetNoticePopup();
                    }
                }
                else
                {
                    GetNoticePopup();
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
                    Settings.Rememberme = true;
                }

                Settings.UserName = UserName.Value;
                Settings.Password = Password.Value;

                StaticSettings.Token = user.AccessToken;
                StaticSettings.User = user;
                StaticSettings.SessionID = DeviceInfo.Model + "_" + DeviceInfo.Platform + "_" + Guid.NewGuid().ToString();
                OneSignal.Current.SendTag("UserID", user.UserId.ToString().ToUpper());
                OneSignal.Current.SendTag("UserName", user.UserName.ToString().ToUpper());
                CultureInfo.CurrentCulture = new CultureInfo(Language.CodeName);
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(Language.CodeName);
                //nếu cần đổi mật khẩu thì mở trang đổi mật khẩu
                if (user.IsNeedChangePassword)
                {
                    await NavigationService.NavigateAsync("BaseNavigationPage/ChangePasswordPage", null, useModalNavigation: true, true);
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

        private async void OnLoginFailed()
        {
            await NavigationService.NavigateAsync("LoginFailedPopup");
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
                GetMobileSetting();
            }
        }

        /// <summary>Gọi thông tin popup khi đăng nhập</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/26/2020   created
        /// </Modified>
        private void GetNoticePopup()
        {
            if (Settings.Rememberme)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Login();
                });
            }
            //RunOnBackground(async () =>
            //{
            //    return await notificationService.GetNotificationWhenLogin(App.AppType);
            //}, (items) =>
            //{
            //    if (items != null && items.Data != null && items.Data.Id > 0)
            //    {
            //        if (items.Data.IsAlwayShow) // true luôn luôn hiển thị
            //        {
            //            Device.BeginInvokeOnMainThread(async () =>
            //            {
            //                await NavigationService.NavigateAsync("NotificationPopupWhenLogin", parameters: new NavigationParameters
            //                 {
            //                     { ParameterKey.NotificationKey, items.Data }
            //                });
            //            });
            //        }
            //        else
            //        {
            //            if (Settings.NoticeIdWhenLogin != items?.Data.Id)
            //            {
            //                Device.BeginInvokeOnMainThread(async () =>
            //                {
            //                    await NavigationService.NavigateAsync("NotificationPopupWhenLogin", parameters: new NavigationParameters
            //                 {
            //                     { ParameterKey.NotificationKey, items.Data }
            //                });
            //                });
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (!string.IsNullOrEmpty(Settings.UserName) && !string.IsNullOrEmpty(Settings.Password))
            //        {
            //            if (Settings.Rememberme)
            //            {
            //                Device.BeginInvokeOnMainThread(() =>
            //                {
            //                    Login();
            //                });
            //            }
            //        }
            //    }
            //});
        }

        #endregion PrivateMethod
    }
}