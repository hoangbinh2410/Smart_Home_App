using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
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
    public class ActivityDetailsViewModel : ReportBase<ActivityDetailsRequest, ActivityDetailsService, ActivityDetailsModel>
    {
        private readonly IShowHideColumnService showHideColumnService;

        public ActivityDetailsViewModel(INavigationService navigationService, IShowHideColumnService showHideColumnService)
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

            IsExportExcel = CheckPermision((int)PermissionKeyNames.ReportActivityDetailExport);
        }

        public ICommand DetailVehicleCommand { get; private set; }

        // cấu hình không quá số ngày cho phép để tìm kiếm dữ liệu
        public override int AddDayMinfromDate { get; set; } = MobileSettingHelper.ViewReport;

        public override int ShowHideColumnTableID { get; set; } = (int)TableReportEnum.Details;

        private bool showVehicleType = false;
        public bool ShowVehicleType { get => showVehicleType; set => SetProperty(ref showVehicleType, value); }

        private bool showVehiclePlate = false;
        public bool ShowVehiclePlate { get => showVehiclePlate; set => SetProperty(ref showVehiclePlate, value); }

        private bool showTripCompensatory = true;
        public bool ShowTripCompensatory { get => showTripCompensatory; set => SetProperty(ref showTripCompensatory, value); }

        private bool showCurrentTime = true;
        public bool ShowCurrentTime { get => showCurrentTime; set => SetProperty(ref showCurrentTime, value); }

        private bool showTimeActive = true;
        public bool ShowTimeActive { get => showTimeActive; set => SetProperty(ref showTimeActive, value); }

        private bool showKmGPS = true;
        public bool ShowKmGPS { get => showKmGPS; set => SetProperty(ref showKmGPS, value); }

        private bool showQuotaFuel = true;
        public bool ShowQuotaFuel { get => showQuotaFuel; set => SetProperty(ref showQuotaFuel, value); }

        private bool showKmCO = true;
        public bool ShowKmCO { get => showKmCO; set => SetProperty(ref showKmCO, value); }

        private bool showQuotaFuelConsume = true;
        public bool ShowQuotaFuelConsume { get => showQuotaFuelConsume; set => SetProperty(ref showQuotaFuelConsume, value); }

        private bool showStartAddress = true;
        public bool ShowStartAddress { get => showStartAddress; set => SetProperty(ref showStartAddress, value); }

        private bool showEndAddress = true;
        public bool ShowEndAddress { get => showEndAddress; set => SetProperty(ref showEndAddress, value); }

        private ActivityDetailsModel selectDetailsItem;
        public ActivityDetailsModel SelectDetailsItem { get => selectDetailsItem; set => SetProperty(ref selectDetailsItem, value); }

        private ObservableCollection<ShowHideColumnResponse> listShowHideComlumn;
        public ObservableCollection<ShowHideColumnResponse> ListShowHideComlumn { get => listShowHideComlumn; set => SetProperty(ref listShowHideComlumn, value); }

        public bool IsExportExcel { get; set; }

        /// <summary>Set dữ liệu đầu vào</summary>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/13/2019   created
        /// </Modified>
        public override ActivityDetailsRequest SetDataInput()
        {
            return new ActivityDetailsRequest
            {
                CompanyID = CurrentComanyID,
                VehicleIDs = VehicleSelect.VehicleId.ToString(),
                FromDate = base.FromDate,
                ToDate = base.ToDate,
                IsAddress = true,
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
        public override IList<ActivityDetailsModel> ConvertDataBeforeDisplay(IList<ActivityDetailsModel> data)
        {
            int i = (PagedNext - 1) * PageSize;
            foreach (var item in data)
            {
                item.OrderNumber = ++i;
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
                SelectDetailsItem = ListDataSearch.Where(x => x.OrderNumber == OrderNumber).FirstOrDefault();
                SelectDetailsItem.VehiclePlate = VehicleSelect.VehiclePlate;
                if (string.IsNullOrEmpty(SelectDetailsItem.StartAddress) && string.IsNullOrEmpty(SelectDetailsItem.EndAddress))
                {
                    var startLat = SelectDetailsItem.StartLatitude;
                    var startLong = SelectDetailsItem.StartLongitude;
                    var endLat = SelectDetailsItem.EndLatitude;
                    var endLong = SelectDetailsItem.EndLongitude;
                    if (string.IsNullOrEmpty(SelectDetailsItem.StartAddress) && string.IsNullOrEmpty(SelectDetailsItem.EndAddress))
                    {
                        var input = string.Format("{0} {1};{2} {3}", startLat, startLong, endLat, endLong);
                        var response = await BaseServiceReport.GetAddressReport(input);
                        if (response.Count >= 2)
                        {
                            SelectDetailsItem.StartAddress = response[0];
                            SelectDetailsItem.EndAddress = response[1];
                        }
                    }
                    else if (string.IsNullOrEmpty(SelectDetailsItem.StartAddress))
                    {
                        var input = string.Format("{0} {1}}", startLat, startLong);
                        var response = await BaseServiceReport.GetAddressReport(input);
                        SelectDetailsItem.StartAddress = response[0];
                    }
                    else if (string.IsNullOrEmpty(SelectDetailsItem.EndAddress))
                    {
                        var input = string.Format("{0} {1}}", endLat, endLong);
                        var response = await BaseServiceReport.GetAddressReport(input);
                        SelectDetailsItem.EndAddress = response[0];
                    }
                }
                var p = new NavigationParameters
                {
                    { ParameterKey.ReportDetailsSelected, SelectDetailsItem }
                };
                await NavigationService.NavigateAsync("ActivityDetailsDetailReportPage", p, useModalNavigation: false,false);
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
        public override void FillDataTableExcell(IList<ActivityDetailsModel> data, ref IWorksheet worksheet)
        {
            try
            {
                //Gán lại tên file
                ReportTitle = ReportHelper.GetFileName(MobileResource.DetailsReport_Label_TilePage);

                int numberrow = 4;
                int numbercolum = 1;
                // STT
                worksheet.Range[numberrow, 1].Text = MobileResource.DetailsReport_Table_Serial;
                // Ngày tháng
                if (ShowCurrentTime)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ActivitySummariesReport_Table_CurrentTime;
                }
                // Giờ đi giờ đến
                numbercolum += 1;
                worksheet.Range[numberrow, numbercolum].Text = MobileResource.DetailsReport_Table_StartEndTime;
                // Thời gian hoạt động
                if (ShowTimeActive)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.DetailsReport_Table_TimeActive;
                }
                // Km GPS
                if (ShowKmGPS)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.DetailsReport_Table_KmGPS;
                }
                // KmCO
                if (ShowKmCO)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.DetailsReport_Table_KmCO;
                }
                // Loại xe
                if (ShowVehicleType)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.DetailsReport_Table_VehicleType;
                }
                // Biển số xe
                if (ShowVehiclePlate)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.DetailsReport_Table_VehiclePlate;
                }
                // Định mức nhiên liệu
                if (ShowQuotaFuel)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.DetailsReport_Table_QuotaFuel;
                }
                // Nhiên liệu tiêu thụ
                if (ShowQuotaFuelConsume)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.DetailsReport_Table_QuotaFuelConsume;
                }
                // Địa chỉ đi
                if (ShowStartAddress)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.DetailsReport_Table_StartAddress;
                }
                // Địa chỉ đến
                if (ShowEndAddress)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.DetailsReport_Table_EndAddress;
                }
                // Cuốc bù
                if (ShowTripCompensatory)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.DetailsReport_Table_TripCompensatory;
                }

                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.Font.Bold = true;
                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.ColorIndex = ExcelKnownColors.Sky_blue;
                //head
                worksheet.Range[1, 1].Text = MobileResource.DetailsReport_Label_TilePage;
                worksheet.Range[1, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[1, 1].CellStyle.Font.Bold = true;
                worksheet.Range[1, 1].CellStyle.Font.Size = 16;
                worksheet.Range[1, 1, 1, numbercolum].Merge();
                worksheet.Range[2, 1].Text = MobileResource.Common_Label_PlaceHolder_FromDate + ": " + DateTimeHelper.FormatDateTime(FromDate) + " " + MobileResource.Common_Label_PlaceHolder_ToDate + ": " + DateTimeHelper.FormatDateTime(ToDate);
                worksheet.Range[2, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[2, 1, 2, numbercolum].Merge();
                worksheet.Range[3, 1].Text = MobileResource.Common_Label_Grid_VehiclePlate + ": " + VehicleSelect.PrivateCode;
                worksheet.Range[3, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[3, 1, 3, numbercolum].Merge();
                // data
                for (int i = 0, length = data.Count; i < length; i++)
                {
                    numberrow += 1;
                    numbercolum = 1;
                    // Số thứ tự
                    worksheet.Range[numberrow, numbercolum].Text = data[i].OrderNumber.ToString();
                    // Ngày tháng
                    if (ShowCurrentTime)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = DateTimeHelper.FormatOnlyDate(data[i].Date);
                    }
                    // Giờ đi giờ đến
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:hh\\:mm}", data[i].StartTimes) + " - " + String.Format("{0:hh\\:mm}", data[i].EndTimes);
                    // Thời gian hoạt động
                    if (ShowTimeActive)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:hh\\:mm}", data[i].TotalTimes);
                    }
                    // Km GPS
                    if (ShowKmGPS)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data[i].TotalKm);
                    }
                    // KmCO
                    if (ShowKmCO)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data[i].KmOfMechanical);
                    }
                    // Loại xe
                    if (ShowVehicleType)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].NameType;
                    }
                    // Biển số xe
                    if (ShowVehiclePlate)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].PrivateCode;
                    }
                    // Định mức nhiên liệu
                    if (ShowQuotaFuel)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data[i].ConstantNorms);
                    }
                    // Nhiên liệu tiêu thụ
                    if (ShowQuotaFuelConsume)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data[i].Norms);
                    }
                    // Địa chỉ đi
                    if (ShowStartAddress)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].StartAddress;
                    }
                    // Địa chỉ đến
                    if (ShowEndAddress)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].EndAddress;
                    }
                    // Cuốc bù
                    if (ShowTripCompensatory)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].DataTypeStr;
                    }
                }

                worksheet.Range[4, 1, numberrow, numbercolum].BorderAround();
                worksheet.Range[4, 1, numberrow, numbercolum].BorderInside(ExcelLineStyle.Thin, ExcelKnownColors.Black);

                // sum
                numberrow += 1;
                numbercolum = 1;
                // Ngày tháng
                if (ShowCurrentTime)
                {
                    numbercolum += 1;
                }
                // Giờ đi giờ đến
                numbercolum += 1;
                // Thời gian hoạt động
                if (ShowTimeActive)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = DateTimeHelper.FormatTimeSpan24h(new TimeSpan(data.Sum(x => x.TotalTimes.Ticks)));
                }
                // Km GPS
                if (ShowKmGPS)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = Math.Round(data.Sum(x => x.TotalKm), 2).ToString();
                }
                // KmCO
                if (ShowKmCO)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", Math.Round(data.Sum(x => x.KmOfMechanical), 1));
                }
                // Loại xe
                if (ShowVehicleType)
                {
                    numbercolum += 1;
                }
                // Biển số xe
                if (ShowVehiclePlate)
                {
                    numbercolum += 1;
                }
                // Định mức nhiên liệu
                if (ShowQuotaFuel)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = ((int)Math.Round(data.Where(item => item.TotalKm > 0).Average(x => x.ConstantNorms))).ToString();
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
                    { 1, ShowVehicleType },
                    { 2, ShowVehiclePlate },
                    { 3, ShowTripCompensatory },
                    { 4, ShowCurrentTime },
                    { 5, ShowTimeActive },
                    { 6, ShowKmGPS },
                    { 7, ShowQuotaFuel },
                    { 8, ShowKmCO },
                    { 9, ShowQuotaFuelConsume },
                    { 10, ShowStartAddress },
                    { 11, ShowEndAddress }
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
                            ShowVehicleType = item.Value;
                            break;

                        case 2:
                            ShowVehiclePlate = item.Value;
                            break;

                        case 3:
                            ShowTripCompensatory = item.Value;
                            break;

                        case 4:
                            ShowCurrentTime = item.Value;
                            break;

                        case 5:
                            ShowTimeActive = item.Value;
                            break;

                        case 6:
                            ShowKmGPS = item.Value;
                            break;

                        case 7:
                            ShowQuotaFuel = item.Value;
                            break;

                        case 8:
                            ShowKmCO = item.Value;
                            break;

                        case 9:
                            ShowQuotaFuelConsume = item.Value;
                            break;

                        case 10:
                            ShowStartAddress = item.Value;
                            break;

                        case 11:
                            ShowEndAddress = item.Value;
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
            return true;
        }
    }
}