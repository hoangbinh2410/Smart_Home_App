using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class RegisterConsultPageViewModel : ViewModelBase
    {
        private readonly IRegisterConsultService registerConsultService;

        public RegisterConsultPageViewModel(INavigationService navigationService,
            IRegisterConsultService registerConsultService) : base(navigationService)
        {
            try
            {
                this.registerConsultService = registerConsultService;

                Title = MobileResource.RegisterConsult_Label_TilePage;

                // khai báo action - excute action
                RegiserConsultCommand = new DelegateCommand(ExcuteRegiserConsult);
                CallPopupTransportTypeCommand = new DelegateCommand(ExcuteCallPopupTransportType);
                CallComboboxProvinceCommand = new DelegateCommand(ExcuteCallComboboxProvince);
                CallComboboxCountryCodeCommand = new DelegateCommand(ExcuteCallComboboxCountryCode);
                CallHotLineCommand = new DelegateCommand(() => CallHotline());

                // khởi tạo các property
                _fullName = new ValidatableObject<string>();
                _countryCode = new ValidatableObject<string>
                {
                    Value = CountryCodeConstant.VietNam
                };
                _phoneNumber = new ValidatableObject<string>();
                _contentConsult = new ValidatableObject<string>();
                _isVisibleProvinces = true;
                _transportTypeItem = new ComboboxResponse();
                ListTransportTypes = new ObservableCollection<ComboboxRequest>();
                _provinceItem = new ComboboxResponse();
                ListProvinces = new ObservableCollection<ComboboxRequest>();
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            // sự kiện trả về
            EventAggregator.GetEvent<SelectComboboxEvent>().Subscribe(UpdateCombobox);
            EventAggregator.GetEvent<SelectCountryCodeEvent>().Subscribe(UpdateCountryCode);
            EventAggregator.GetEvent<SelectCancelPopupMessage>().Subscribe(UpdatePopupMessage);
        }

        public override void OnDestroy()
        {
            EventAggregator.GetEvent<SelectComboboxEvent>().Unsubscribe(UpdateCombobox);
            EventAggregator.GetEvent<SelectCountryCodeEvent>().Unsubscribe(UpdateCountryCode);
            EventAggregator.GetEvent<SelectCancelPopupMessage>().Unsubscribe(UpdatePopupMessage);
        }

        #region Property

        private ValidatableObject<string> _fullName;

        public ValidatableObject<string> FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }

        private ValidatableObject<string> _countryCode;

        public ValidatableObject<string> CountryCode
        {
            get { return _countryCode; }
            set { SetProperty(ref _countryCode, value); }
        }

        private ValidatableObject<string> _phoneNumber;

        public ValidatableObject<string> PhoneNumber
        {
            get { return _phoneNumber; }
            set { SetProperty(ref _phoneNumber, value); }
        }

        private ValidatableObject<string> _contentConsult;

        public ValidatableObject<string> ContentConsult
        {
            get { return _contentConsult; }
            set { SetProperty(ref _contentConsult, value); }
        }

        public ObservableCollection<ComboboxRequest> ListProvinces { get; set; }

        private ComboboxResponse _provinceItem;

        public ComboboxResponse ProvinceItem
        {
            get { return _provinceItem; }
            set { SetProperty(ref _provinceItem, value); }
        }

        private bool _isVisibleProvinces;

        public bool IsVisibleProvinces
        {
            get { return _isVisibleProvinces; }
            set { SetProperty(ref _isVisibleProvinces, value); }
        }

        public ObservableCollection<ComboboxRequest> ListTransportTypes { get; set; }
        private ComboboxResponse _transportTypeItem;

        public ComboboxResponse TransportTypeItem
        {
            get { return _transportTypeItem; }
            set { SetProperty(ref _transportTypeItem, value); }
        }

        private string _hotline = MobileSettingHelper.HotlineTeleSaleGps;

        public string Hotline
        {
            get
            {
                return _hotline;
            }
            set
            {
                _hotline = value;
                RaisePropertyChanged(() => Hotline);
            }
        }

        #endregion Property

        #region Command

        public DelegateCommand RegiserConsultCommand { get; private set; }

        public DelegateCommand CallHotLineCommand { get; private set; }

        public DelegateCommand CallPopupTransportTypeCommand { get; private set; }

        public DelegateCommand CallComboboxProvinceCommand { get; private set; }

        public DelegateCommand CallComboboxCountryCodeCommand { get; private set; }

        #endregion Command

        #region excute command

        /// <summary>
        /// xử lý lưu db khi kích vào nút đăng kí
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  10/03/2019   created
        /// </Modified>
        private async void ExcuteRegiserConsult()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            try
            {
                if (IsConnected)
                {
                    if (Validate())
                    {
                        using (new HUDService())
                        {
                            var input = new RegisterConsultRequest()
                            {
                                Fullname = FullName.Value.Trim(),
                                ContentVdvisory = ContentConsult.Value,
                                PhoneNumber = string.Format("{0}{1}", CountryCode.Value, PhoneNumber.Value),
                                FK_TransportTypeID = TransportTypeItem.Key,
                                FK_ProvinceID = ProvinceItem.Key,
                                SourceRegistry = (int)ClientType.Mobile,
                            };

                            // xử lý với trường hợp là mã việt nam
                            if (CountryCodeConstant.VietNam.Equals(CountryCode.Value))
                            {
                                if (PhoneNumber.Value.StartsWith("0"))
                                {
                                    input.PhoneNumber = string.Format("{0}{1}", CountryCode.Value, PhoneNumber.Value.Substring(1));
                                }
                            }
                            input.PhoneNumber = input.PhoneNumber.Replace("+", string.Empty);
                            var response = await registerConsultService.RegisterConsultRequest(input);
                            switch (response)
                            {
                                case (int)RegisterConsultEnum.Success:
                                    Device.BeginInvokeOnMainThread(async () =>
                                    {
                                        var p = new NavigationParameters
                                        {
                                            { "TitlePopup", MobileResource.Common_Label_BAGPS },
                                            { "ContentPopup", MobileResource.RegisterConsult_Message_SuccessRegister },
                                            { "TitleButton", MobileResource.RegisterConsult_Button_ClosePopup }
                                        };
                                        await NavigationService.NavigateAsync("PopupMessagePage", p);
                                    });

                                    break;

                                case (int)RegisterConsultEnum.Existed:
                                    DisplayMessage.ShowMessageInfo(MobileResource.RegisterConsult_Message_ErrorExistsPhone, 5000);
                                    break;

                                case (int)RegisterConsultEnum.False:
                                    DisplayMessage.ShowMessageInfo(MobileResource.RegisterConsult_Message_ErrorRegister, 5000);
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// gọi combobox loại hình đăng kí
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  10/03/2019   created
        /// </Modified>
        private async void ExcuteCallPopupTransportType()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            try
            {
                if (ListTransportTypes.Count <= 0)
                {
                    using (new HUDService())
                    {
                        var response = await registerConsultService.GetDataTransportType(Settings.CurrentLanguage);
                        if (response != null && response.Count > 0)
                        {
                            foreach (var item in response)
                            {
                                ListTransportTypes.Add(new ComboboxRequest() { Key = item.FK_TransportTypeID, Value = item.Value });
                            }
                        }
                    }
                }
                var p = new NavigationParameters
                {
                    { "dataCombobox", ListTransportTypes.ToList() },
                    { "ComboboxType", ComboboxType.First },
                    { "Title", MobileResource.RegisterConsult_Label_TransportType }
                };
                await NavigationService.NavigateAsync("BaseNavigationPage/ComboboxPage", p, useModalNavigation: true);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// gọi combobox tỉnh thành
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  10/03/2019   created
        /// </Modified>
        private async void ExcuteCallComboboxProvince()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            try
            {
                if (ListProvinces.Count <= 0)
                {
                    using (new HUDService())
                    {
                        var response = await registerConsultService.GetDataProvince(Settings.CurrentLanguage);
                        if (response != null && response.Count > 0)
                        {
                            foreach (var item in response)
                            {
                                ListProvinces.Add(new ComboboxRequest() { Key = item.PK_ProvinceID, Value = item.Name });
                            }
                        }
                    }
                }
                var p = new NavigationParameters
                {
                    { "dataCombobox", ListProvinces.ToList() },
                    { "ComboboxType", ComboboxType.Second },
                    { "Title", MobileResource.RegisterConsult_Label_Provinces }
                };
                await NavigationService.NavigateAsync("BaseNavigationPage/ComboboxPage", p, useModalNavigation: true);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// gọi combobox lấy mã quốc gia
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  10/03/2019   created
        /// </Modified>
        private async void ExcuteCallComboboxCountryCode()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            await NavigationService.NavigateAsync("BaseNavigationPage/PhoneCountryCodePage", useModalNavigation: true);
            IsBusy = false;
        }

        /// <summary>
        /// update khi nhận kết quả trả về từ combobox tỉnh thành và loại hình vận tải
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  10/03/2019   created
        /// </Modified>
        public override void UpdateCombobox(ComboboxResponse param)
        {
            if (param != null)
            {
                var dataResponse = param;
                if (dataResponse.ComboboxType == (Int16)ComboboxType.First)
                {
                    TransportTypeItem = dataResponse;
                }
                else
                {
                    ProvinceItem = dataResponse;
                }
            }
        }

        /// <summary>
        /// update khi nhận kết quả trả về từ combobox mã quốc gia
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  10/03/2019   created
        /// </Modified>
        private void UpdateCountryCode(CountryCode param)
        {
            if (param != null)
            {
                CountryCode.Value = param.DialCode;
                IsVisibleProvinces = param.DialCode.Equals(CountryCodeConstant.VietNam);
            }
        }

        /// <summary>
        /// update khi nhận kết quả trả về từ popup message
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  10/03/2019   created
        /// </Modified>
        private async void UpdatePopupMessage()
        {
            await NavigationService.GoBackAsync(useModalNavigation: true);
        }

        /// <summary>
        /// Hàm thực hiện gọi tới số điện thoại hotline
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  10/03/2019   created
        /// </Modified>
        private void CallHotline()
        {
            if (!string.IsNullOrEmpty(Hotline))
            {
                PhoneDialer.Open(MobileSettingHelper.HotlineGps);
            }
        }

        // hàm thực hiện thêm và xử lý với các validation
        private void AddValidations()
        {
            _fullName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = MobileResource.Common_Property_NullOrEmpty(MobileResource.RegisterConsult_Label_UserName.Replace("(*)", "")) });
            _fullName.Validations.Add(new MaxLengthRule<string> { ValidationMessage = MobileResource.RegisterConsult_Message_Validate_MaxLength_UserName, MaxLenght = 50 });
            _fullName.Validations.Add(new ExpressionDangerousCharsRule<string> { Expression = MobileSettingHelper.ConfigDangerousCharTextBox, ValidationMessage = MobileResource.Common_Property_DangerousCharShow(MobileResource.RegisterConsult_Label_UserName, MobileSettingHelper.ConfigDangerousCharTextBox) });
            _countryCode.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = MobileResource.RegisterConsult_Message_Validate_IsNull_CountryCode });
            _contentConsult.Validations.Add(new ExpressionDangerousCharsRule<string> { Expression = MobileSettingHelper.ConfigDangerousCharTextBox, ValidationMessage = MobileResource.Common_Property_DangerousCharShow(MobileResource.RegisterConsult_Label_Content, MobileSettingHelper.ConfigDangerousCharTextBox) });
        }

        private void AddValidationsPhone()
        {
            _phoneNumber.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = MobileResource.Common_Property_NullOrEmpty(MobileResource.RegisterConsult_Label_Phone.Replace("(*)", "")) });
            _phoneNumber.Validations.Add(new PhoneNumberRule<string> { ValidationMessage = MobileResource.RegisterConsult_Message_Validate_Rule_Phone, CountryCode = CountryCode.Value });
            _phoneNumber.Validations.Add(new ExpressionDangerousCharsRule<string> { Expression = MobileSettingHelper.ConfigDangerousCharTextBox, ValidationMessage = MobileResource.Common_Property_DangerousCharShow(MobileResource.RegisterConsult_Label_Phone, MobileSettingHelper.ConfigDangerousCharTextBox) });
        }

        private bool Validate()
        {
            AddValidations();
            if (_fullName.Validate() && _contentConsult.Validate() && _countryCode.Validate())
            {
                // check là đầu số việt nam thì check tiếp ko thì mặc định là lưu
                if (CountryCodeConstant.VietNam.Equals(CountryCode.Value))
                {
                    AddValidationsPhone();
                    return _phoneNumber.Validate();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        //public async Task ShowMessageSuccess(string message,
        //   string title,
        //   string buttonText,
        //   Action afterHideCallback)
        //{
        //    await _dialogService.DisplayAlertAsync(title, message, buttonText);
        //    afterHideCallback?.Invoke();
        //}

        #endregion excute command
    }
}
