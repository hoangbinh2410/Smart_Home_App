using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class StopParkingVehicleViewModel : ReportBase<StopParkingVehicleRequest, StopsParkingVehicleService, StopParkingVehicleModel>
    {
        private readonly IShowHideColumnService showHideColumnService;

        public StopParkingVehicleViewModel(INavigationService navigationService, IShowHideColumnService showHideColumnService)
            : base(navigationService)
        {
            this.showHideColumnService = showHideColumnService;

            ListShowHideComlumn = new ObservableCollection<ShowHideColumnResponse>()
            {
                new ShowHideColumnResponse() { IDColumn = 1, Value = true},
                new ShowHideColumnResponse() { IDColumn = 2, Value = false}
            };

            DetailVehicleCommand = new DelegateCommand<int?>(ExecuteDetailVehicle);

            DisplayComlumnHide();

            IsExportExcel = CheckPermision((int)PermissionKeyNames.ReportStopExport);
        }

        public ICommand DetailVehicleCommand { get; private set; }

        // cấu hình không quá số ngày cho phép để tìm kiếm dữ liệu
        public override int AddDayMinfromDate { get; set; } = MobileSettingHelper.ViewReport;

        public override int ShowHideColumnTableID { get; set; } = (int)TableReportEnum.StopParking;

        private bool showDriverName = false;
        public bool ShowDriverName { get => showDriverName; set => SetProperty(ref showDriverName, value); }

        private bool showCodeName = false;
        public bool ShowCodeName { get => showCodeName; set => SetProperty(ref showCodeName, value); }

        private bool showPhone = false;
        public bool ShowPhone { get => showPhone; set => SetProperty(ref showPhone, value); }

        private bool showVehiclePlate = true;
        public bool ShowVehiclePlate { get => showVehiclePlate; set => SetProperty(ref showVehiclePlate, value); }

        private bool showMinutes = true;
        public bool ShowMinutes { get => showMinutes; set => SetProperty(ref showMinutes, value); }

        private bool showStopParkingTime = true;
        public bool ShowStopParkingTime { get => showStopParkingTime; set => SetProperty(ref showStopParkingTime, value); }

        private bool showMinutesStopRunEngine = true;
        public bool ShowMinutesStopRunEngine { get => showMinutesStopRunEngine; set => SetProperty(ref showMinutesStopRunEngine, value); }

        private bool showMinutesTurnOnAirConditioner = true;
        public bool ShowMinutesTurnOnAirConditioner { get => showMinutesTurnOnAirConditioner; set => SetProperty(ref showMinutesTurnOnAirConditioner, value); }

        private bool showStopLocation = true;
        public bool ShowStopLocation { get => showStopLocation; set => SetProperty(ref showStopLocation, value); }

        private string numberOfStopParking = string.Empty;
        public string NumberOfStopParking { get => numberOfStopParking; set => SetProperty(ref numberOfStopParking, value); }

        private string numberOfStopRunEngine = string.Empty;
        public string NumberOfStopRunEngine { get => numberOfStopRunEngine; set => SetProperty(ref numberOfStopRunEngine, value); }

        private StopParkingVehicleModel selectStopParkingVehicleItem;
        public StopParkingVehicleModel SelectStopParkingVehicleItem { get => selectStopParkingVehicleItem; set => SetProperty(ref selectStopParkingVehicleItem, value); }

        private ObservableCollection<ShowHideColumnResponse> listShowHideComlumn;
        public ObservableCollection<ShowHideColumnResponse> ListShowHideComlumn { get => listShowHideComlumn; set => SetProperty(ref listShowHideComlumn, value); }

        public bool IsExportExcel { get; set; }

        /// <summary>Set dữ liệu đầu vào</summary>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/13/2019   created
        /// </Modified>
        public override StopParkingVehicleRequest SetDataInput()
        {
            return new StopParkingVehicleRequest
            {
                CompanyID = CurrentComanyID,
                VehicleIDs = VehicleSelect.VehicleId.ToString(),
                FromDate = base.FromDate,
                ToDate = base.ToDate,
                MinutesOfManchineOn = string.IsNullOrEmpty(NumberOfStopRunEngine) ? 0 : int.Parse(NumberOfStopRunEngine),
                TotalTimeStop = string.IsNullOrEmpty(NumberOfStopParking) ? 0 : int.Parse(NumberOfStopParking),
                IsAddress = ShowStopLocation,
                PageIndex = base.PagedNext,
                PageSize = base.PageSize
            };
        }

        /// <summary>Converts lại số thứ tự theo trang</summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/13/2019   created
        /// </Modified>
        public override IList<StopParkingVehicleModel> ConvertDataBeforeDisplay(IList<StopParkingVehicleModel> data)
        {
            int i = (PagedNext - 1) * PageSize;
            foreach (var item in data)
            {
                item.OrderNumber = ++i;
                item.StopParkingTime = MinusTimeSpanHelper.MinusTimeSpan(item.EndTime, item.StartTime);
            }
            return data;
        }



        /// <summary>Gọi service truyền dữ liệu sang bên trang chi tiết</summary>
        /// <param name="OrderNumber">The order number.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/13/2019   created
        /// </Modified>
        private async void ExecuteDetailVehicle(int? OrderNumber)
        {
            try
            {
                SelectStopParkingVehicleItem = ListDataSearch.Where(x => x.OrderNumber == OrderNumber).FirstOrDefault();
                SelectStopParkingVehicleItem.VehiclePlate = VehicleSelect.VehiclePlate;
                if (string.IsNullOrEmpty(SelectStopParkingVehicleItem.StartAddress) && string.IsNullOrEmpty(SelectStopParkingVehicleItem.EndAddress))
                {
                    var startLat = SelectStopParkingVehicleItem.StartLatitude;
                    var startLong = SelectStopParkingVehicleItem.StartLongitude;
                    var endLat = SelectStopParkingVehicleItem.EndLatitude;
                    var endLong = SelectStopParkingVehicleItem.EndLongitude;
                    if (string.IsNullOrEmpty(SelectStopParkingVehicleItem.StartAddress) && string.IsNullOrEmpty(SelectStopParkingVehicleItem.EndAddress))
                    {
                        var input = string.Format("{0} {1};{2} {3}", startLat, startLong, endLat, endLong);
                        var response = await BaseServiceReport.GetAddressReport(input);
                        if (response.Count >= 2)
                        {
                            SelectStopParkingVehicleItem.StartAddress = response[0];
                            SelectStopParkingVehicleItem.EndAddress = response[1];
                        }
                    }
                    else if (string.IsNullOrEmpty(SelectStopParkingVehicleItem.StartAddress))
                    {
                        var input = string.Format("{0} {1}}", startLat, startLong);
                        var response = await BaseServiceReport.GetAddressReport(input);
                        SelectStopParkingVehicleItem.StartAddress = response[0];
                    }
                    else if (string.IsNullOrEmpty(SelectStopParkingVehicleItem.EndAddress))
                    {
                        var input = string.Format("{0} {1}}", endLat, endLong);
                        var response = await BaseServiceReport.GetAddressReport(input);
                        SelectStopParkingVehicleItem.EndAddress = response[0];
                    }
                }
                var p = new NavigationParameters
                {
                    { ParameterKey.ReportStopParkingVehicleSelected, SelectStopParkingVehicleItem }
                };
                await NavigationService.NavigateAsync("StopParkingVehicleDetailReportPage", p, useModalNavigation: false);
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
        public override void FillDataTableExcell(IList<StopParkingVehicleModel> data, ref IWorksheet worksheet)
        {
            try
            {
                //Gán lại tên file
                ReportTitle = ReportHelper.GetFileName(MobileResource.StopParkingReport_Label_TilePage);

                int numberrow = 4;
                int numbercolum = 1;
                // STT
                worksheet.Range[numberrow, 1].Text = MobileResource.StopParkingReport_Table_Serial;
                // Biển số xe
                if (ShowVehiclePlate)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.StopParkingReport_Table_VehiclePlate;
                }
                // Thời gian
                numbercolum += 1;
                worksheet.Range[numberrow, numbercolum].Text = MobileResource.StopParkingReport_Table_Date;
                // Thời gian (phút)
                if (ShowMinutes)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.StopParkingReport_Table_Time;
                }
                // Thời gian dừng đỗ
                if (ShowStopParkingTime)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.StopParkingReport_Table_StopParkingTime;
                }
                // Nổ máy khi dừng
                if (ShowMinutesStopRunEngine)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.StopParkingReport_Table_MinutesStopRunEngine;
                }
                // Bật điều hoà khi dừng
                if (ShowMinutesTurnOnAirConditioner)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.StopParkingReport_Table_MinutesTurnOnAirConditioner;
                }
                // Tên lái xe
                if (ShowDriverName)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.StopParkingReport_Table_DriverName;
                }
                // Mã nhân viên
                if (ShowCodeName)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.StopParkingReport_Table_CodeName;
                }
                // Số điện thoại
                if (ShowPhone)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.StopParkingReport_Table_Phone;
                }
                // Địa điểm
                if (ShowStopLocation)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.StopParkingReport_Table_StopLocation;
                }


                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.Font.Bold = true;
                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.ColorIndex = ExcelKnownColors.Sky_blue;
                //head
                worksheet.Range[1, 1].Text = MobileResource.StopParkingReport_Label_TilePage;
                worksheet.Range[1, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[1, 1].CellStyle.Font.Bold = true;
                worksheet.Range[1, 1].CellStyle.Font.Size = 16;
                worksheet.Range[1, 1, 1, numbercolum].Merge();
                worksheet.Range[2, 1].Text = MobileResource.Common_Label_PlaceHolder_FromDate + ": " + DateTimeHelper.FormatDateTime(FromDate) + " " + MobileResource.Common_Label_PlaceHolder_ToDate + ": " + DateTimeHelper.FormatDateTime(ToDate);
                worksheet.Range[2, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[2, 1, 2, numbercolum].Merge();
                string option = MobileResource.Common_Label_Grid_VehiclePlate + ": " + VehicleSelect.PrivateCode;
                if (!string.IsNullOrEmpty(NumberOfStopParking))
                    option += "| " + MobileResource.StopParkingReport_Label_PlaceHolder_NumberOfStopParking + " " + NumberOfStopParking;
                if (!string.IsNullOrEmpty(NumberOfStopRunEngine))
                    option += "| " + MobileResource.StopParkingReport_Label_PlaceHolder_NumberOfStopRunEngine + " " + NumberOfStopRunEngine;
                worksheet.Range[3, 1].Text = option;
                worksheet.Range[3, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[3, 1, 3, numbercolum].Merge();
                // data
                for (int i = 0, length = data.Count; i < length; i++)
                {
                    numberrow += 1;
                    numbercolum = 1;
                    // Số thứ tự 
                    worksheet.Range[numberrow, numbercolum].Text = data[i].OrderNumber.ToString();
                    // Biển số xe
                    if (ShowVehiclePlate)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].PrivateCode;
                    }
                    //  Thời gian
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = DateTimeHelper.FormatDateTime(data[i].StartTime);
                    //  Thời gian (phút) 
                    if (ShowMinutes)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].TotalTimeStop.ToString();
                    }
                    // Thời gian dừng đỗ 
                    if (ShowStopParkingTime)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].StopParkingTime.ToString();
                    }
                    // Nổ máy khi dừng
                    if (ShowMinutesStopRunEngine)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].MinutesOfManchineOn.ToString();
                    }
                    // Bật điều hoà khi dừng
                    if (ShowMinutesTurnOnAirConditioner)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].MinutesOfAirConditioningOn.ToString();
                    }
                    // Tên lái xe  
                    if (ShowDriverName)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].DriverName;
                    }
                    // Mã nhân viên
                    if (ShowCodeName)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].EmployeeCode;
                    }
                    // Số điện thoại
                    if (ShowPhone)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].Phone;
                    }
                    //  Địa điểm
                    if (ShowStopLocation)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].Address;
                    }
                }

                worksheet.Range[4, 1, numberrow, numbercolum].BorderAround();
                worksheet.Range[4, 1, numberrow, numbercolum].BorderInside(ExcelLineStyle.Thin, ExcelKnownColors.Black);

                // sum
                numberrow += 1;
                numbercolum = 2;
                // Biển số xe
                if (ShowVehiclePlate)
                {
                    numbercolum += 1;
                }
                //  Thời gian (phút) 
                if (ShowMinutes)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data.Sum(x => x.TotalTimeStop).ToString();
                }
                // Thời gian dừng đỗ 
                if (ShowStopParkingTime)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MinusTimeSpanHelper.MinusTimeSpan(new TimeSpan(data.Sum(x => TimeSpan.Parse(x.StopParkingTime).Ticks)));
                }
                // Nổ máy khi dừng
                if (ShowMinutesStopRunEngine)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data.Sum(x => x.MinutesOfManchineOn).ToString();
                }
                // Bật điều hoà khi dừng
                if (ShowMinutesTurnOnAirConditioner)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data.Sum(x => x.MinutesOfAirConditioningOn).ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>Lưu các thông tin ẩn hiện cột</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/13/2019   created
        /// </Modified>
        public override void ExecuteSaveComlumnHide()
        {
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

        public override IDictionary<int, bool> ShowHideColumnDictionary
        {
            get
            {
                return new Dictionary<int, bool>
                {
                    { 1, ShowDriverName },
                    { 2, ShowCodeName },
                    { 3, ShowPhone },
                    { 4, ShowVehiclePlate },
                    { 5, ShowMinutes },
                    { 6, ShowStopParkingTime },
                    { 7, ShowMinutesStopRunEngine },
                    { 8, ShowMinutesTurnOnAirConditioner },
                    { 9, ShowStopLocation }
                };
            }
        }

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
                            ShowDriverName = item.Value;
                            break;

                        case 2:
                            ShowCodeName = item.Value;
                            break;

                        case 3:
                            ShowPhone = item.Value;
                            break;

                        case 4:
                            ShowVehiclePlate = item.Value;
                            break;

                        case 5:
                            ShowMinutes = item.Value;
                            break;

                        case 6:
                            ShowStopParkingTime = item.Value;
                            break;

                        case 7:
                            ShowMinutesStopRunEngine = item.Value;
                            break;

                        case 8:
                            ShowMinutesTurnOnAirConditioner = item.Value;
                            break;

                        case 9:
                            ShowStopLocation = item.Value;
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>Kiểm tra đầu vào</summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/13/2019   created
        /// </Modified>
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
            if (!string.IsNullOrEmpty(NumberOfStopParking))
            {
                // check thời gian dừng đỗ int
                if (!int.TryParse(NumberOfStopParking, out int numberOfStopParking))
                {
                    message = MobileResource.StopParkingReport_Message_ValidateError_StopParkingTime;
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(NumberOfStopRunEngine))
            {
                // check thời gian bật máy kiểu int
                if (!int.TryParse(NumberOfStopRunEngine, out int numberOfStopRunEngine))
                {
                    message = MobileResource.StopParkingReport_Message_ValidateError_MinutesStopRunEngine;
                    return false;
                }
            }
            return true;
        }
    }
}