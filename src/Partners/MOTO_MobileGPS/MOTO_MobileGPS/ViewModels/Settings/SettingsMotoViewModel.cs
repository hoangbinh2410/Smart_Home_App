using BA_MobileGPS.Core;

using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

using MOTO_MobileGPS.Constant;
using System.Reflection;
using BA_MobileGPS.Core.Resources;

namespace MOTO_MobileGPS.ViewModels
{
    public class SettingsMotoViewModel : ViewModelBase
    {
        private readonly IUserService userService;

        private readonly IMotoPropertiesService motoPropertiesService;

        private readonly IMotoConfigService motoConfigService;

        private bool isAllowTurnOnOffEngineViaSMS;
        public bool IsAllowTurnOnOffEngineViaSMS { get => isAllowTurnOnOffEngineViaSMS; set => SetProperty(ref isAllowTurnOnOffEngineViaSMS, value); }

        private bool isAllowCallDriveAlarm;
        public bool IsAllowCallDriveAlarm { get => isAllowCallDriveAlarm; set => SetProperty(ref isAllowCallDriveAlarm, value); }

        public string PhoneNumberStr { get; set; } = string.Empty;

        private string phoneNumber;
        public string PhoneNumber { get => phoneNumber; set => SetProperty(ref phoneNumber, value); }

        private int warningNumber;
        public int WarningNumber { get => warningNumber; set => SetProperty(ref warningNumber, value); }

        private bool isTurnOnSMSWarning;
        public bool IsTurnOnSMSWarning { get => isTurnOnSMSWarning; set => SetProperty(ref isTurnOnSMSWarning, value); }

        private Thickness marginSliderWarningNumber = new Thickness(0, 0, 0, 0);
        public Thickness MarginSliderWarningNumber { get => marginSliderWarningNumber; set => SetProperty(ref marginSliderWarningNumber, value); }

        public bool IsDestroy { get; set; } = false;

        public ICommand ToggleAllowTurnOnOffEngineViaSMSCommand { get; private set; }

        public ICommand ToggleAllowCallDriveAlarmCommand { get; private set; }

        public ICommand PushToPhoneNumberSMSCommand { get; private set; }

        public ICommand DragCompletedCommand { get; private set; }

        public ICommand TurnOnSMSWarningCommand { get; private set; }

        public ICommand TurnOffSMSWarningCommand { get; private set; }

        private MotoPropertiesViewModel selectItem;
        public MotoPropertiesViewModel SelectItem { get => selectItem; set => SetProperty(ref selectItem, value); }

        //private MotoDetailViewModel motoDetail;
        //public MotoDetailViewModel MotoDetail { get => motoDetail; set => SetProperty(ref motoDetail, value); }

        public SettingsMotoViewModel(INavigationService navigationService, IUserService userService, IMotoPropertiesService motoPropertiesService, IMotoConfigService motoConfigService) : base(navigationService)
        {
            this.userService = userService;

            this.motoPropertiesService = motoPropertiesService;

            this.motoConfigService = motoConfigService;

            ToggleAllowTurnOnOffEngineViaSMSCommand = new Command(ToggleAllowTurnOnOffEngineViaSMS);

            ToggleAllowCallDriveAlarmCommand = new Command(ToggleAllowCallDriveAlarm);

            PushToPhoneNumberSMSCommand = new Command(PushToPhoneNumberSMS);

            DragCompletedCommand = new Command(DragCompleted);

            TurnOnSMSWarningCommand = new Command(TurnOnSMSWarning);

            TurnOffSMSWarningCommand = new Command(TurnOffSMSWarning);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters?.GetValue<MotoPropertiesViewModel>(MotoParameterKey.MotoDetail) is MotoPropertiesViewModel moto)
            {
                SelectItem = moto;
                SelectItem.BatteryLowVoltageThreshold = SelectItem.BatteryLowVoltageThreshold == 0 ? 10000 : SelectItem.BatteryLowVoltageThreshold;
                MotoStaticSettings.PhoneNumberStr = string.Empty;
                WarningNumber = SelectItem.BatteryLowVoltageThreshold;
                IsAllowTurnOnOffEngineViaSMS = moto.AllowTurnOnOffEngineViaSMS;
                IsAllowCallDriveAlarm = moto.AllowCallDriveAlarm;
                IsTurnOnSMSWarning = moto.AllowCallDriveAlarm;
                MarginSliderWarningNumber = IsAllowCallDriveAlarm ? new Thickness(0, 0, 0, 0) : new Thickness(0, -35, 0, 0);
                if (MotoStaticSettings.PhoneNumberStr != string.Empty)
                {
                    PhoneNumber = MotoStaticSettings.PhoneNumberStr;
                }
                else
                {
                    PhoneNumber = MotoConfigHelper.JoinPhoneNumberEx(moto.PhoneNumber1) + MotoConfigHelper.JoinPhoneNumberEx(moto.PhoneNumber2) + MotoConfigHelper.JoinPhoneNumberEx(moto.PhoneNumber3);
                    if (PhoneNumber.Length > 0)
                        PhoneNumber = PhoneNumber.Substring(0, PhoneNumber.Length - 1);
                }
                IsDestroy = true;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(MotoParameterKey.KeyPhoneNumberWarningSMSPage) && parameters.GetValue<string>(MotoParameterKey.KeyPhoneNumberWarningSMSPage) is string keyPhoneNumberSMSPage)
            {
                PhoneNumber = keyPhoneNumberSMSPage;
            }
            IsDestroy = true;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            IsDestroy = false;
        }

        /// <summary>Cấu hình cho phép bật/tắt động cơ từ xa</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  3/13/2020   created
        /// </Modified>
        private void ToggleAllowTurnOnOffEngineViaSMS()
        {
            if (IsDestroy)
            {
                if (IsAllowTurnOnOffEngineViaSMS != SelectItem.AllowTurnOnOffEngineViaSMS)
                    SendConfig(MotoParameterKey.AllowTurnOnOffEngineViaSMS, IsAllowTurnOnOffEngineViaSMS ? "1" : "0", MotoParameterKey.AllowTurnOnOffEngineViaSMS);
            }

        }

        /// <summary>Cấu hình cảnh báo khi xe di chuyển trái phép</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  3/13/2020   created
        /// </Modified>
        private void ToggleAllowCallDriveAlarm()
        {
            if (IsDestroy)
            {
                if (IsAllowCallDriveAlarm != SelectItem.AllowCallDriveAlarm)
                    SendConfig(MotoParameterKey.AllowCallDriveAlarm, IsAllowCallDriveAlarm ? "1" : "0", MotoParameterKey.AllowCallDriveAlarm);
            }

        }

        /// <summary>Cấu hình ngưỡng cảnh báo/summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  3/13/2020   created
        /// </Modified>
        private async void DragCompleted()
        {
            if (WarningNumber != SelectItem.BatteryLowVoltageThreshold)
            {
                var result = await Application.Current.MainPage.DisplayAlert(MobileResource.Moto_Label_Alter, MobileResource.Moto_Message_Alter_WarningNumber, MobileResource.Common_Button_OK, MobileResource.Common_Message_Skip);

                if (result)
                {
                    SendConfig(MotoParameterKey.BatteryLowVoltageThreshold, WarningNumber.ToString(), MotoParameterKey.BatteryLowVoltageThreshold);
                }
                else
                {
                    WarningNumber = SelectItem.BatteryLowVoltageThreshold;
                }
            }
        }

        private void TurnOnSMSWarning()
        {
            if (ValidatePhone())
            {
                SendActionHelper.SendSMS(MotoStaticSettings.MotoProperties.DevicePhoneNumber, MotoParameterKey.ValueTurnOnSMSWarning);
            }
        }

        private void TurnOffSMSWarning()
        {
            if (ValidatePhone())
            {
                SendActionHelper.SendSMS(MotoStaticSettings.MotoProperties.DevicePhoneNumber, MotoParameterKey.ValueTurnOffSMSWarning);
            }
        }

        /// <summary>Gửi lệnh cấu hình</summary>
        /// <param name="configID">The configuration identifier.</param>
        /// <param name="configValue">The configuration value.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  3/13/2020   created
        /// </Modified>
        private void SendConfig(string configID, string configValue, string key)
        {
            SafeExecute(() =>
            {
                RunOnBackground(async () =>
                {
                    return await motoConfigService.SendConfigMoto(new MotoConfigRequest()
                    {
                        XNCode = StaticSettings.User.XNCode,
                        VehiclePlate = MotoStaticSettings.MotoProperties.VehiclePlate,
                        ConfigID = configID,
                        ConfigValue = configValue
                    });
                },
              (items) =>
              {
                  if (items != null)
                  {
                      switch (items.SetConfigResult)
                      {
                          case MotoConfigResult.RUNNING:

                              ConfirmConfig(key);

                              break;

                          case MotoConfigResult.FAIL:

                              DisplayMessage.ShowMessageError(MobileResource.SendConfig_Label_TurnOnOff_Fail);
                              ReturnConfig(key);

                              break;

                          case MotoConfigResult.OK:

                              ConfirmConfig(key);

                              break;

                          case MotoConfigResult.NOTONLINE:

                              DisplayMessage.ShowMessageWarning(MobileResource.SendConfig_Label_TurnOnOff_NotOnline);
                              ReturnConfig(key);

                              break;
                          case MotoConfigResult.NOTSUPPORT:

                              DisplayMessage.ShowMessageWarning(MobileResource.SendConfig_Label_TurnOnOff_NotSupport);
                              ReturnConfig(key);

                              break;
                      }
                  }
              }, showLoading: true);
            });
        }

        private void ConfirmConfig(string key)
        {
            if (key == MotoParameterKey.AllowTurnOnOffEngineViaSMS)
            {
                MotoStaticSettings.MotoProperties.AllowTurnOnOffEngineViaSMS = IsAllowTurnOnOffEngineViaSMS;
            }
            else if (key == MotoParameterKey.AllowCallDriveAlarm)
            {
                MotoStaticSettings.MotoProperties.AllowCallDriveAlarm = IsAllowCallDriveAlarm;
                MarginSliderWarningNumber = IsAllowCallDriveAlarm ? new Thickness(0, 0, 0, 0) : new Thickness(0, -35, 0, 0);
                IsTurnOnSMSWarning = IsAllowCallDriveAlarm;
            }
            else if (key == MotoParameterKey.BatteryLowVoltageThreshold)
            {
                MotoStaticSettings.MotoProperties.BatteryLowVoltageThreshold = WarningNumber;
            }
        }

        private void ReturnConfig(string key)
        {
            if (key == MotoParameterKey.AllowTurnOnOffEngineViaSMS)
            {
                IsAllowTurnOnOffEngineViaSMS = !IsAllowTurnOnOffEngineViaSMS;
            }
            else if (key == MotoParameterKey.AllowCallDriveAlarm)
            {
                IsAllowCallDriveAlarm = !IsAllowCallDriveAlarm;
            }
            else if (key == MotoParameterKey.BatteryLowVoltageThreshold)
            {
                WarningNumber = SelectItem.BatteryLowVoltageThreshold;
            }
        }

        /// <summary>Kiểm tra có số điện thoại không</summary>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  3/18/2020   created
        /// </Modified>
        private bool ValidatePhone()
        {
            if (string.IsNullOrEmpty(MotoStaticSettings.MotoProperties.DevicePhoneNumber))
            {
                SafeExecute(async () =>
                {
                    var result = await Application.Current.MainPage.DisplayAlert(MobileResource.Moto_Label_Alter, MobileResource.Moto_Message_Alter, MobileResource.Moto_Label_Call_Operator, MobileResource.Common_Message_Skip);
                    if (result)
                    {
                        SendActionHelper.SendMakePhoneCall(MobileSettingHelper.HotlineGps);
                    }
                });
                return false;
            }
            return true;
        }

        private void PushToPhoneNumberSMS()
        {
            SafeExecute(async () =>
            {
                var p = new MotoPropertiesRequest
                {
                    PhoneNumberStr = PhoneNumber,
                    VehiclePlate = MotoStaticSettings.MotoProperties.VehiclePlate
                };
                await NavigationService.NavigateAsync("PhoneNumberSMSPage", new NavigationParameters
                {
                    { MotoParameterKey.KeyPhoneNumberSMSPage, p }
                }, useModalNavigation: false,false);
            });
        }
    }
}