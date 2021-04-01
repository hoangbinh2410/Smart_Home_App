using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;

using Syncfusion.XlsIO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BA_MobileGPS.Core.ViewModels
{
    public class MachineVehicleReportViewModel : ReportBase<MachineVehcleRequest, MachineVehicleService, MachineVehicleResponse>
    {
        private readonly IShowHideColumnService showHideColumnService;

        public MachineVehicleReportViewModel(INavigationService navigationService,
            IShowHideColumnService showHideColumnService) : base(navigationService)
        {
            try
            {
                this.showHideColumnService = showHideColumnService;
                // khải báo phần button và sự kiện

                DetailMachineVehicleCommand = new DelegateCommand<int?>(ExecuteDetailMachineVehicle);
                PushStatusMachineComboboxCommand = new DelegateCommand(ExecuteStatusMachineCombobox);
                SelectMachineVehicleItem = new MachineVehicleResponse();

                _minutesCondition = string.Empty;
                _statusMachineSelected = new ComboboxResponse()
                {
                    Key = 1,
                    Value = MobileResource.ReportMachine_Label_EngineTurnOn
                };
                DisplayComlumnHide();

                IsExportExcel = CheckPermision((int)PermissionKeyNames.ReportMachineOnExport);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.AdminReportMachine,
                Type = UserBehaviorType.Start
            });
        }

        public override void OnDestroy()
        {
            base.Dispose();
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.AdminReportMachine,
                Type = UserBehaviorType.End
            });
        }

        #region property

        // thuộc tính số phút tìm kiếm
        private string _minutesCondition = string.Empty;

        //public override DateTime FromDate { get; set; } = DateTime.Now;
        public string MinutesCondition
        {
            get { return _minutesCondition; }
            set { SetProperty(ref _minutesCondition, value); }
        }

        // thuộc tính số phút tìm kiếm
        private ComboboxResponse _statusMachineSelected;

        public ComboboxResponse StatusMachineSelected
        {
            get { return _statusMachineSelected; }
            set { SetProperty(ref _statusMachineSelected, value); }
        }

        public MachineVehicleResponse SelectMachineVehicleItem
        {
            get;
            set;
        }

        // cấu hình không quá số ngày cho phép để tìm kiếm dữ liệu
        public override int AddDayMinfromDate { get; set; } = MobileSettingHelper.ViewReport;

        public override string Title { get; set; } = MobileResource.ReportMachine_Label_TitlePage;

        public override int ShowHideColumnTableID { get; set; } = (int)TableReportEnum.MachineReport;
        private bool _showdToDate = true;

        public bool ShowToDate
        {
            get { return _showdToDate; }
            set { SetProperty(ref _showdToDate, value); }
        }

        private bool _showStartAddress = true;

        public bool ShowStartAddress
        {
            get { return _showStartAddress; }
            set { SetProperty(ref _showStartAddress, value); }
        }

        private bool _showEndAddress = true;

        public bool ShowEndAddress
        {
            get { return _showEndAddress; }
            set { SetProperty(ref _showEndAddress, value); }
        }

        private bool _showFuel = true;

        public bool ShowFuel
        {
            get { return _showFuel; }
            set { SetProperty(ref _showFuel, value); }
        }

        public bool IsExportExcel { get; set; }

        public override IDictionary<int, bool> ShowHideColumnDictionary
        {
            get
            {
                IDictionary<int, bool> ShowHideColumnDictionary = new Dictionary<int, bool>
                {
                    { 1, ShowToDate },
                    { 2, ShowStartAddress },
                    { 3, ShowEndAddress },
                    { 4, ShowFuel }
                };
                return ShowHideColumnDictionary;
            }
        }

        #endregion property

        #region Command

        public DelegateCommand<int?> DetailMachineVehicleCommand { get; private set; }
        public DelegateCommand PushStatusMachineComboboxCommand { get; private set; }

        #endregion Command

        #region Execute command

        /// <summary>
        /// Hàm gọi sang bên chi tiết để show thông tin
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  3/28/2019   created
        /// </Modified>
        private async void ExecuteDetailMachineVehicle(int? OrderNumber)
        {
            try
            {
                // gọi service truyền dữ liệu sang bên trang chi tiết

                SelectMachineVehicleItem = ListDataSearch.Where(x => x.OrderNumber == OrderNumber).FirstOrDefault();
                SelectMachineVehicleItem.VehiclePlate = VehicleSelect.VehiclePlate;
                if (string.IsNullOrEmpty(SelectMachineVehicleItem.StartAddress) && string.IsNullOrEmpty(SelectMachineVehicleItem.EndAddress))
                {
                    var startLat = SelectMachineVehicleItem.StartLatitude;
                    var startLong = SelectMachineVehicleItem.StartLongitude;
                    var endLat = SelectMachineVehicleItem.EndLatitude;
                    var endLong = SelectMachineVehicleItem.EndLongitude;
                    if (string.IsNullOrEmpty(SelectMachineVehicleItem.StartAddress) && string.IsNullOrEmpty(SelectMachineVehicleItem.EndAddress))
                    {
                        var input = string.Format("{0} {1};{2} {3}", startLat, startLong, endLat, endLong);
                        var response = await BaseServiceReport.GetAddressReport(input);
                        if (response.Count >= 2)
                        {
                            SelectMachineVehicleItem.StartAddress = response[0];
                            SelectMachineVehicleItem.EndAddress = response[1];
                        }
                    }
                    else if (string.IsNullOrEmpty(SelectMachineVehicleItem.StartAddress))
                    {
                        var input = string.Format("{0} {1}}", startLat, startLong);
                        var response = await BaseServiceReport.GetAddressReport(input);
                        SelectMachineVehicleItem.StartAddress = response[0];
                    }
                    else if (string.IsNullOrEmpty(SelectMachineVehicleItem.EndAddress))
                    {
                        var input = string.Format("{0} {1}}", endLat, endLong);
                        var response = await BaseServiceReport.GetAddressReport(input);
                        SelectMachineVehicleItem.EndAddress = response[0];
                    }
                }
                var p = new NavigationParameters
                {
                    { ParameterKey.ReportMachineVehicleSelected, SelectMachineVehicleItem }
                };
                await NavigationService.NavigateAsync("MachineDetailVehicleReport", p, useModalNavigation: false, true);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>Đổ dữ liệu vào excel</summary>
        /// <param name="data">The data.</param>
        /// <param name="worksheet">The worksheet.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/27/2019   created
        /// </Modified>
        public override void FillDataTableExcell(IList<MachineVehicleResponse> data, ref IWorksheet worksheet)
        {
            try
            {
                //Gán lại tên file
                ReportTitle = ReportHelper.GetFileName(MobileResource.ReportMachine_Label_TitlePage);

                int numberrow = 4;
                int numbercolum = 1;
                // title

                worksheet.Range[numberrow, numbercolum].Text = MobileResource.Common_Label_TitleGrid_OrderNumber;

                // thời gian bắt đầu
                numbercolum += 1;
                worksheet.Range[numberrow, numbercolum].Text = MobileResource.Common_Label_Grid_FromDate;

                //  thời gian kết thúc
                if (ShowToDate)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.Common_Label_Grid_ToDate;
                }

                // Số phút
                numbercolum += 1;
                worksheet.Range[numberrow, numbercolum].Text = MobileResource.ReportMachine_Label_Grid_Minutes;

                // nhiên liệu
                if (ShowFuel)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ReportMachine_Label_Grid_Fuel;
                }

                // địa chỉ đi
                if (ShowStartAddress)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ReportMachine_Label_Grid_StartAddress;
                }

                // địa chỉ đến
                if (ShowEndAddress)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ReportMachine_Label_Grid_EndAddress;
                }

                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.Font.Bold = true;
                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.ColorIndex = ExcelKnownColors.Sky_blue;
                //head
                worksheet.Range[1, 1].Text = MobileResource.ReportMachine_Label_TitlePage;
                worksheet.Range[1, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[1, 1].CellStyle.Font.Bold = true;
                worksheet.Range[1, 1].CellStyle.Font.Size = 16;
                worksheet.Range[1, 1, 1, numbercolum].Merge();
                worksheet.Range[2, 1].Text = MobileResource.Common_Label_PlaceHolder_FromDate + ": " + DateTimeHelper.FormatDateTime(FromDate) + " " + MobileResource.Common_Label_PlaceHolder_ToDate + ": " + DateTimeHelper.FormatDateTime(ToDate);
                worksheet.Range[2, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[2, 1, 2, numbercolum].Merge();
                string option = MobileResource.Common_Label_Grid_VehiclePlate + ": " + VehicleSelect.PrivateCode;
                if (!string.IsNullOrEmpty(MinutesCondition))
                    option += "| " + MobileResource.ReportMachine_Label_PlaceHolder_CountMinutes + " " + MinutesCondition;
                option += "| " + MobileResource.ReportMachine_Label_PlaceHolder_StatusMachine + ": " + StatusMachineSelected.Value;
                worksheet.Range[3, 1].Text = option;
                worksheet.Range[3, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[3, 1, 3, numbercolum].Merge();
                // data
                for (int i = 0, length = data.Count; i < length; i++)
                {
                    numberrow += 1;
                    numbercolum = 1;
                    // số thứ tự
                    worksheet.Range[numberrow, numbercolum].Text = data[i].OrderNumber.ToString();

                    // thời gian bắt đầu
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = DateTimeHelper.FormatDateTime(data[i].StartTime);

                    // thời gian kết thúc
                    if (ShowToDate)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = DateTimeHelper.FormatDateTime(data[i].EndTime);
                    }

                    // Số phút
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data[i].NumberMinutesTurn;

                    // nhiên liệu
                    if (ShowFuel)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].FuelConsumed.ToString();
                    }
                    if (ShowStartAddress)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].StartAddress;
                    }
                    if (ShowEndAddress)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].EndAddress;
                    }
                }

                worksheet.Range[4, 1, numberrow, numbercolum].BorderAround();
                worksheet.Range[4, 1, numberrow, numbercolum].BorderInside(ExcelLineStyle.Thin, ExcelKnownColors.Black);

                // sum
                numberrow += 1;
                numbercolum = 4;
                worksheet.Range[numberrow, numbercolum].Text = DateTimeHelper.FormatTimeSpan24h(new TimeSpan(data.Sum(x => TimeSpan.Parse(x.NumberMinutesTurn).Ticks)));
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        public override void InitForm()
        {
            try
            {
                base.InitForm();
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        private List<ComboboxRequest> LoadAllStatusMachine()
        {
            return new List<ComboboxRequest>() {
                    new ComboboxRequest(){Key = 1 , Value = MobileResource.ReportMachine_Label_EngineTurnOn},
                    new ComboboxRequest(){Key = 2 , Value = MobileResource.ReportMachine_Label_EngineTurnOff},
                };
        }

        public override MachineVehcleRequest SetDataInput()
        {
            var input = new MachineVehcleRequest
            {
                CompanyID = (UserHelper.isCompanyPartner(UserInfo.CompanyType) || (UserHelper.isCompanyEndUserWithPermisstion(UserInfo.CompanyType))) ? CurrentComanyID : StaticSettings.User.CompanyId,
                ListVehicleID = VehicleSelect.VehicleId.ToString(),
                NumberOfMinutes = string.IsNullOrEmpty(MinutesCondition) ? 0 : int.Parse(MinutesCondition),
                State = StatusMachineSelected.Key == 0 ? (bool?)null : (StatusMachineSelected.Key == 1 ? true : false),
                PageIndex = PagedNext,
                PageSize = PageSize,
                DateStart = FromDate,
                DateEnd = ToDate,
                IsAddress = ShowStartAddress || ShowEndAddress,
            };
            return input;
        }

        public override bool CheckValidateInput(ref string message)
        {
            if (!base.CheckValidateInput(ref message))
            {
                return false;
            }

            //không chọn biển số xe
            if (string.IsNullOrEmpty(VehicleSelect.VehiclePlate))
            {
                message = MobileResource.Common_Message_NoSelectVehiclePlate;
                return false;
            }

            if (!string.IsNullOrEmpty(MinutesCondition))
            {
                // check trường nhiệt độ - chỉ cho nhập kiểu float
                if (!int.TryParse(MinutesCondition, out _))
                {
                    message = MobileResource.ReportMachine_Message_ValidateError_Minutes;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// chuẩn hoá dữ liệu trước khi hiển thị lên form
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  3/31/2019   created
        /// </Modified>
        public override IList<MachineVehicleResponse> ConvertDataBeforeDisplay(IList<MachineVehicleResponse> data)
        {
            int i = (PagedNext - 1) * PageSize;
            foreach (var item in data)
            {
                item.OrderNumber = ++i;
            }
            return data;
        }

        /// <summary>
        /// thực hiện ẩn hiện cộtd
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  4/3/2019   created
        /// </Modified>
        public override void ExecuteSaveComlumnHide()
        {
            // thực hiện lưu các thông tin liên quan tới ẩn hiện cột vào đây
            foreach (var item in ShowHideColumnDictionary)
            {
                // đẩy xuống db
                var model = showHideColumnService.Get(r => r.IDTable == ShowHideColumnTableID && r.IDColumn == item.Key);
                if (model != null)
                {
                    if (model.Value != item.Value)
                    {
                        model.Value = item.Value;
                        showHideColumnService.Update(model);
                    }
                }
                else
                {
                    // Thêm resouce vào realm db
                    showHideColumnService.Add(new ShowHideColumnResponse()
                    {
                        IDTable = ShowHideColumnTableID,
                        IDColumn = item.Key,
                        Value = item.Value
                    });
                }
            }
        }

        /// <summary>
        /// Ẩn hiện cột
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  4/8/2019   created
        /// </Modified>
        public override void DisplayComlumnHide()
        {
            try
            {
                var temp = showHideColumnService.Find(r => r.IDTable == ShowHideColumnTableID);
                foreach (var item in temp)
                {
                    switch (item.IDColumn)
                    {
                        case 1:
                            ShowToDate = item.Value;
                            break;

                        case 2:
                            ShowStartAddress = item.Value;
                            break;

                        case 3:
                            ShowEndAddress = item.Value;
                            break;

                        case 4:
                            ShowFuel = item.Value;
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public async void ExecuteStatusMachineCombobox()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            try
            {
                var p = new NavigationParameters
                {
                    { "dataCombobox", LoadAllStatusMachine() },
                    { "ComboboxType", ComboboxType.First },
                    { "Title", MobileResource.ReportMachine_TitleStatusMachine }
                };
                await NavigationService.NavigateAsync("BaseNavigationPage/ComboboxPage", p, useModalNavigation: true, true);
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
        /// Update Combobox
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  3/29/2019   created
        /// </Modified>
        public override void UpdateCombobox(ComboboxResponse param)
        {
            base.UpdateCombobox(param);
            if (param != null)
            {
                var dataResponse = param;
                if (dataResponse.ComboboxType == (Int16)ComboboxType.First)
                {
                    StatusMachineSelected = dataResponse;
                }
            }
        }

        #endregion Execute command
    }
}