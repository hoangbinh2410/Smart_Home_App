using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;

using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class SendEngineControlViewModel : ReportBase<ActionOnOffMachineLogRequest, SendEngineControlService, ActionOnOffMachineLogViewModel>
    {
        private readonly ISendEngineControlService sendEngineControlService;

        public SendEngineControlViewModel(INavigationService navigationService, ISendEngineControlService sendEngineControlService)
            : base(navigationService)
        {
            this.sendEngineControlService = sendEngineControlService;
            OpenPopupOnCommand = new DelegateCommand(OpenPopupOn);
            OpenPopupOffCommand = new DelegateCommand(OpenPopupOff);
            SendCommand = new DelegateCommand(RunSend);
            CloseCommand = new DelegateCommand(ClosePopup);
            ExcuteSearchData();
        }

        public ICommand OpenPopupOnCommand { get; set; }

        public ICommand OpenPopupOffCommand { get; set; }

        public ICommand SendCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        private string popupHeaderTitle;
        public string PopupHeaderTitle { get => popupHeaderTitle; set => SetProperty(ref popupHeaderTitle, value); }

        private bool isDisplayPopupSendEngineControl;
        public bool IsDisplayPopupSendEngineControl { get => isDisplayPopupSendEngineControl; set => SetProperty(ref isDisplayPopupSendEngineControl, value); }

        private bool passwordPopup = false;
        public bool PasswordPopup { get => passwordPopup; set => SetProperty(ref passwordPopup, value); }

        private bool warningPopup = false;
        public bool WarningPopup { get => warningPopup; set => SetProperty(ref warningPopup, value); }

        private string password;
        public string Password { get => password; set => SetProperty(ref password, value); }

        private bool checkOnOff;
        public bool CheckOnOff { get => checkOnOff; set => SetProperty(ref checkOnOff, value); }

        // cấu hình không quá số ngày cho phép để tìm kiếm dữ liệu
        public override int AddDayMinfromDate { get; set; } = MobileSettingHelper.ViewReport;

        /// <summary>Mở Popup bật máy</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  1/8/2020   created
        /// </Modified>
        private void OpenPopupOn()
        {
            if (string.IsNullOrEmpty(VehicleSelect.VehiclePlate))
            {
                DisplayMessage.ShowMessageInfo(MobileResource.SendEngineControl_Warning_On, CountMinutesShowMessageReport);
            }
            else
            {
                if (CompanyConfigurationHelper.IsDisplayPopupSendEngineControl)
                {
                    PopupHeaderTitle = MobileResource.SendEngineControl_Popup_Placeholder_Password;
                    IsDisplayPopupSendEngineControl = true;
                    PasswordPopup = true;
                    WarningPopup = false;
                    CheckOnOff = true;
                    Password = string.Empty;
                }
                else
                {
                    TryExecute(async () =>
                    {
                        var result = await Application.Current.MainPage.DisplayAlert(MobileResource.SendEngineControl_Warning_Label, MobileResource.SendEngineControl_Message_TurnOn_Engine, MobileResource.SendEngineControl_Label_Confirm, MobileResource.Common_Message_Skip);
                        if (result)
                        {
                            //call hàm luôn
                            CheckOnOff = true;
                            CallEngineControl();
                        }
                    });
                }
            }
        }

        /// <summary>Mở Popup tắt máy</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  1/8/2020   created
        /// </Modified>
        private void OpenPopupOff()
        {
            if (string.IsNullOrEmpty(VehicleSelect.VehiclePlate))
            {
                DisplayMessage.ShowMessageInfo(MobileResource.SendEngineControl_Warning_Off, CountMinutesShowMessageReport);
            }
            else
            {
                if (CompanyConfigurationHelper.IsDisplayPopupSendEngineControl)
                {
                    //- Bật máy
                    //- V >= 5km / h
                    //- Now – VehicleTIme <= 60s
                    //Lấy xe online
                    if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 1)
                    {
                        var vehicle = StaticSettings.ListVehilceOnline.Where(x => x.VehiclePlate == VehicleSelect.VehiclePlate).FirstOrDefault();
                        if (vehicle != null)
                        {
                            if (StateVehicleExtension.IsMovingAndEngineON(vehicle))
                            {
                                PopupHeaderTitle = MobileResource.SendEngineControl_Popup_HeaderTitle;
                                IsDisplayPopupSendEngineControl = true;
                                PasswordPopup = false;
                                WarningPopup = true;
                            }
                            else
                            {
                                PopupHeaderTitle = MobileResource.SendEngineControl_Popup_Placeholder_Password;
                                IsDisplayPopupSendEngineControl = true;
                                PasswordPopup = true;
                                WarningPopup = false;
                                CheckOnOff = false;
                                Password = string.Empty;
                            }
                        }
                        else
                        {
                            DisplayMessage.ShowMessageWarning(MobileResource.SendEngineControl_Warning_VehilceNotOnline, CountMinutesShowMessageReport);
                        }
                    }
                    else
                    {
                        DisplayMessage.ShowMessageWarning(MobileResource.SendEngineControl_Warning_VehilceNotOnline, CountMinutesShowMessageReport);
                    }
                }
                else
                {
                    TryExecute(async () =>
                    {
                        var result = await Application.Current.MainPage.DisplayAlert(MobileResource.SendEngineControl_Warning_Label, MobileResource.SendEngineControl_Message_TurnOff_Engine, MobileResource.SendEngineControl_Label_Confirm, MobileResource.Common_Message_Skip);
                        if (result)
                        {
                            //call hàm luôn
                            CheckOnOff = false;
                            CallEngineControl();
                        }
                    });
                }
            }
        }

        /// <summary>Thực hiện bật tắt</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  1/8/2020   created
        /// </Modified>
        private void RunSend()
        {
            //Kiểm tra mật khẩu
            if (Password == Settings.Password)
            {
                //Gọi hàm bật tắt
                CallEngineControl();
            }
            else
            {
                DisplayMessage.ShowMessageWarning(MobileResource.SendEngineControl_Validate_PasswordInvalid, CountMinutesShowMessageReport);
            }
        }

        private void CallEngineControl()
        {
            TryExecute(async () =>
            {
                //Gọi hàm bật tắt
                var input = new SendEngineControlRequest()
                {
                    FK_UserID = StaticSettings.User.UserId,
                    Imei = VehicleSelect.Imei,
                    VehiclePlate = VehicleSelect.VehiclePlate,
                    Command = CheckOnOff ? "BATMAY#" : "TATMAY#",
                    XNCode = StaticSettings.User.XNCode
                };

                var response = await sendEngineControlService.SendEngineControl(input);

                if (response != null)
                {
                    IsDisplayPopupSendEngineControl = false;
                    PasswordPopup = false;
                    switch (response.Error.Code)
                    {
                        case ErrorSendEngineEnum.Success:
                            if (checkOnOff)
                            {
                                DisplayMessage.ShowMessageSuccess(MobileResource.SendEngineControl_Label_On_Success, CountMinutesShowMessageReport);
                            }
                            else
                            {
                                DisplayMessage.ShowMessageSuccess(MobileResource.SendEngineControl_Label_Off_Success, CountMinutesShowMessageReport);
                            }
                            break;

                        case ErrorSendEngineEnum.WrongKey:
                            DisplayMessage.ShowMessageWarning(MobileResource.SendEngineControl_Label_State_1, CountMinutesShowMessageReport);
                            break;

                        case ErrorSendEngineEnum.DeviceNotOnline:
                            DisplayMessage.ShowMessageWarning(MobileResource.SendEngineControl_Label_State_2, CountMinutesShowMessageReport);
                            break;

                        case ErrorSendEngineEnum.ParmsNotValid:
                            DisplayMessage.ShowMessageWarning(MobileResource.SendEngineControl_Label_State_3, CountMinutesShowMessageReport);
                            break;

                        case ErrorSendEngineEnum.Unknown:
                            DisplayMessage.ShowMessageWarning(MobileResource.SendEngineControl_Label_State_100, CountMinutesShowMessageReport);
                            break;
                    }
                }
            });
        }

        private void ClosePopup()
        {
            IsDisplayPopupSendEngineControl = false;
            PasswordPopup = false;
            WarningPopup = false;
        }

        /// <summary>Set dữ liệu đầu vào</summary>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  1/8/2020   created
        /// </Modified>
        public override ActionOnOffMachineLogRequest SetDataInput()
        {
            return new ActionOnOffMachineLogRequest
            {
                FK_UserID = StaticSettings.User.UserId,
                VehiclePlate = VehicleSelect.VehiclePlate,
                PageIndex = base.PagedNext,
                PageSize = base.PageSize,
                StartDate = base.FromDate,
                EndDate = base.ToDate
            };
        }

        /// <summary>Converts lại số thứ tự theo trang</summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  1/8/2020   created
        /// </Modified>
        public override IList<ActionOnOffMachineLogViewModel> ConvertDataBeforeDisplay(IList<ActionOnOffMachineLogViewModel> data)
        {
            int i = (PagedNext - 1) * PageSize;
            foreach (var item in data)
            {
                item.OrderNumber = ++i;
                item.ActionStr = item.Action == true ? MobileResource.SendEngineControl_Label_On : MobileResource.SendEngineControl_Label_Off;
                switch (item.State)
                {
                    case 0:
                        item.StateStr = MobileResource.SendEngineControl_Label_State_0;
                        break;

                    case 1:
                        item.StateStr = MobileResource.SendEngineControl_Label_State_1;
                        break;

                    case 2:
                        item.StateStr = MobileResource.SendEngineControl_Label_State_2;
                        break;

                    case 3:
                        item.StateStr = MobileResource.SendEngineControl_Label_State_3;
                        break;

                    case 100:
                        item.StateStr = MobileResource.SendEngineControl_Label_State_100;
                        break;
                }
            }
            return data;
        }

        /// <summary>Kiểm tra đầu vào</summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  1/8/2020   created
        /// </Modified>
        public override bool CheckValidateInput(ref string message)
        {
            if (!base.CheckValidateInput(ref message))
            {
                return false;
            }
            return true;
        }
    }
}