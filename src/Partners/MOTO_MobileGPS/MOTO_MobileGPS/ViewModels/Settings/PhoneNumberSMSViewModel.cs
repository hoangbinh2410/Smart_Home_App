using BA_MobileGPS.Core;

using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;

using MOTO_MobileGPS.Constant;
using BA_MobileGPS.Core.Resources;

namespace MOTO_MobileGPS.ViewModels
{
    public class PhoneNumberSMSViewModel : ViewModelBase
    {
        private readonly IMotoConfigService motoConfigService;

        public DelegateCommand CloseFormCommand { get; private set; }

        public DelegateCommand SendPhoneNumberCommand { get; private set; }

        public string VehiclePlate { get; set; }

        public string PhoneNumberStr { get; set; }

        public PhoneNumberSMSViewModel(INavigationService navigationService, IMotoConfigService motoConfigService)
            : base(navigationService)
        {
            this.motoConfigService = motoConfigService;

            AddValidations();

            CloseFormCommand = new DelegateCommand(CloseForm);

            SendPhoneNumberCommand = new DelegateCommand(SendPhoneNumber);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters?.GetValue<MotoPropertiesRequest>(MotoParameterKey.KeyPhoneNumberSMSPage) is MotoPropertiesRequest moto)
            {
                if (moto.PhoneNumberStr.Length > 0)
                {
                    PhoneNumberStr = moto.PhoneNumberStr;
                    string[] item = moto.PhoneNumberStr.Split(',');
                    if (item.Length > 0)
                    {
                        for (int i = 0; i < item.Length; i++)
                        {
                            GetValuePhoneInpunt(item[i].Trim());
                        }
                    }
                }
                VehiclePlate = moto.VehiclePlate;
            }
        }

        private void GetValuePhoneInpunt(string item)
        {
            if (string.IsNullOrEmpty(PhoneNumber.Value))
            {
                PhoneNumber.Value = MotoConfigHelper.ReFixPhoneNumber(item);
            }
            else if (string.IsNullOrEmpty(PhoneNumber2.Value))
            {
                PhoneNumber2.Value = MotoConfigHelper.ReFixPhoneNumber(item);
            }
            else
            {
                PhoneNumber3.Value = MotoConfigHelper.ReFixPhoneNumber(item);
            }
        }

        private void CloseForm()
        {
            SafeExecute(async () =>
            {
                await NavigationService.GoBackAsync(useModalNavigation: false);
            });
        }

        private void SendPhoneNumber()
        {
            var p = MotoConfigHelper.JoinPhoneNumberEx(PhoneNumber.Value) + MotoConfigHelper.JoinPhoneNumberEx(PhoneNumber2.Value) + MotoConfigHelper.JoinPhoneNumberEx(PhoneNumber3.Value);
            if (p.Length > 0)
            {
                p = p.Substring(0, p.Length - 1);
                AddValidations2(p);
            }

            FixValidatePhoneNumber();

            if (!Validate())
            {
                ReFixValidatePhoneNumber();
                return;
            }

            var item = MotoConfigHelper.JoinPhoneNumber(PhoneNumber.Value) + MotoConfigHelper.JoinPhoneNumber(PhoneNumber2.Value) + MotoConfigHelper.JoinPhoneNumber(PhoneNumber3.Value);
            if (item.Length > 0)
            {
                item = MotoConfigHelper.ConvertPhoneNumber(item.Substring(0, item.Length - 1));
            }

            ReFixValidatePhoneNumber();

            var item2 = MotoConfigHelper.ConvertPhoneNumberEx(item);

            if (PhoneNumberStr != item2)
            {
                SendConfig(MotoParameterKey.ConfigThreePhoneNumberSMS, item);
            }
            else
            {
                NavigationService.GoBackAsync(parameters: new NavigationParameters
                            {
                                { MotoParameterKey.KeyPhoneNumberWarningSMSPage, p }
                            });
            }


        }

        private void FixValidatePhoneNumber()
        {

            PhoneNumber.Value = MotoConfigHelper.FixPhoneNumber(PhoneNumber.Value);
            PhoneNumber2.Value = MotoConfigHelper.FixPhoneNumber(PhoneNumber2.Value);
            PhoneNumber3.Value = MotoConfigHelper.FixPhoneNumber(PhoneNumber3.Value);
        }

        private void ReFixValidatePhoneNumber()
        {
            PhoneNumber.Value = MotoConfigHelper.ReFixPhoneNumber(PhoneNumber.Value);
            PhoneNumber2.Value = MotoConfigHelper.ReFixPhoneNumber(PhoneNumber2.Value);
            PhoneNumber3.Value = MotoConfigHelper.ReFixPhoneNumber(PhoneNumber3.Value);
        }

        private void SendConfig(string configID, string configValue)
        {
            SafeExecute(() =>
            {
                RunOnBackground(async () =>
                {
                    return await motoConfigService.SendConfigMoto(new MotoConfigRequest()
                    {
                        XNCode = StaticSettings.User.XNCode,
                        VehiclePlate = VehiclePlate,
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

                              MotoStaticSettings.PhoneNumberStr = MotoConfigHelper.ConvertPhoneNumberEx(configValue);
                              MotoStaticSettings.MotoProperties.PhoneNumber1 = PhoneNumber.Value;
                              MotoStaticSettings.MotoProperties.PhoneNumber2 = PhoneNumber2.Value;
                              MotoStaticSettings.MotoProperties.PhoneNumber3 = PhoneNumber3.Value;

                              NavigationService.GoBackAsync(parameters: new NavigationParameters
                                {
                                    { MotoParameterKey.KeyPhoneNumberWarningSMSPage, MotoStaticSettings.PhoneNumberStr }
                                });

                              break;

                          case MotoConfigResult.FAIL:

                              DisplayMessage.ShowMessageError(MobileResource.SendConfig_Label_TurnOnOff_Fail);

                              break;

                          case MotoConfigResult.OK:

                              MotoStaticSettings.PhoneNumberStr = MotoConfigHelper.ConvertPhoneNumberEx(configValue);
                              MotoStaticSettings.MotoProperties.PhoneNumber1 = PhoneNumber.Value;
                              MotoStaticSettings.MotoProperties.PhoneNumber2 = PhoneNumber2.Value;
                              MotoStaticSettings.MotoProperties.PhoneNumber3 = PhoneNumber3.Value;

                              NavigationService.GoBackAsync(parameters: new NavigationParameters
                                {
                                    { MotoParameterKey.KeyPhoneNumberWarningSMSPage, MotoStaticSettings.PhoneNumberStr }
                                });

                              break;

                          case MotoConfigResult.NOTONLINE:

                              DisplayMessage.ShowMessageWarning(MobileResource.SendConfig_Label_TurnOnOff_NotOnline);

                              break;

                          case MotoConfigResult.NOTSUPPORT:

                              DisplayMessage.ShowMessageWarning(MobileResource.SendConfig_Label_TurnOnOff_NotSupport);

                              break;
                      }
                  }
              }, showLoading: true);
            });
        }

        private void AddValidations()
        {
            _phoneNumber.Validations.Add(new PhoneNumberRule<string> { ValidationMessage = MobileResource.RegisterConsult_Message_Validate_Rule_Phone, CountryCode = CountryCodeConstant.VietNam });

            _phoneNumber2.Validations.Add(new PhoneNumberRule<string> { ValidationMessage = MobileResource.RegisterConsult_Message_Validate_Rule_Phone, CountryCode = CountryCodeConstant.VietNam });

            _phoneNumber3.Validations.Add(new PhoneNumberRule<string> { ValidationMessage = MobileResource.RegisterConsult_Message_Validate_Rule_Phone, CountryCode = CountryCodeConstant.VietNam });
        }

        private void AddValidations2(string value)
        {
            _phoneNumber.Validations.Clear();
            _phoneNumber.Validations.Add(new PhoneNumberRule<string> { ValidationMessage = MobileResource.RegisterConsult_Message_Validate_Rule_Phone, CountryCode = CountryCodeConstant.VietNam });
            _phoneNumber.Validations.Add(new PhoneNumberDoubleRule<string> { ValidationMessage = MobileResource.RegisterConsult_Message_Validate_Rule_Double_Phone, PhoneStr = value });

            _phoneNumber2.Validations.Clear();
            _phoneNumber2.Validations.Add(new PhoneNumberRule<string> { ValidationMessage = MobileResource.RegisterConsult_Message_Validate_Rule_Phone, CountryCode = CountryCodeConstant.VietNam });
            _phoneNumber2.Validations.Add(new PhoneNumberDoubleRule<string> { ValidationMessage = MobileResource.RegisterConsult_Message_Validate_Rule_Double_Phone, PhoneStr = value });

            _phoneNumber3.Validations.Clear();
            _phoneNumber3.Validations.Add(new PhoneNumberRule<string> { ValidationMessage = MobileResource.RegisterConsult_Message_Validate_Rule_Phone, CountryCode = CountryCodeConstant.VietNam });
            _phoneNumber3.Validations.Add(new PhoneNumberDoubleRule<string> { ValidationMessage = MobileResource.RegisterConsult_Message_Validate_Rule_Double_Phone, PhoneStr = value });
        }

        private bool Validate()
        {
            if (_phoneNumber.Validate() && _phoneNumber2.Validate() && _phoneNumber3.Validate())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Property

        private ValidatableObject<string> _phoneNumber = new ValidatableObject<string>();

        public ValidatableObject<string> PhoneNumber
        {
            get { return _phoneNumber; }
            set { SetProperty(ref _phoneNumber, value); }
        }

        private ValidatableObject<string> _phoneNumber2 = new ValidatableObject<string>();

        public ValidatableObject<string> PhoneNumber2
        {
            get { return _phoneNumber2; }
            set { SetProperty(ref _phoneNumber2, value); }
        }

        private ValidatableObject<string> _phoneNumber3 = new ValidatableObject<string>();

        public ValidatableObject<string> PhoneNumber3
        {
            get { return _phoneNumber3; }
            set { SetProperty(ref _phoneNumber3, value); }
        }

        #endregion Property
    }
}