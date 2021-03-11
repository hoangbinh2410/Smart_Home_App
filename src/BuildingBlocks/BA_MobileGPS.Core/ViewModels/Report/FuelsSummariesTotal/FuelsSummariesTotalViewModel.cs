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
    public class FuelsSummariesTotalViewModel : ReportBase<FuelsSummariesTotalRequest, FuelsSummariesTotalService, FuelsSummariesTotalResponse>
    {
        private readonly IShowHideColumnService showHideColumnService;

        public FuelsSummariesTotalViewModel(INavigationService navigationService, IShowHideColumnService showHideColumnService)
            : base(navigationService)
        {
            this.showHideColumnService = showHideColumnService;

            ListShowHideComlumn = new ObservableCollection<ShowHideColumnResponse>()
            {
                new ShowHideColumnResponse() { IDColumn = 1, Value = true},
                new ShowHideColumnResponse() { IDColumn = 2, Value = false}
            };

            DetailVehicleCommand = new DelegateCommand<int?>(ExecuteDetailVehicle);

            DetailRedirectVehicleCommand = new DelegateCommand(ExecuteDetailRedirectVehicle);

            OpenPopupCommand = new DelegateCommand(OpenPopup);

            DisplayComlumnHide();

            IsExportExcel = CheckPermision((int)PermissionKeyNames.FuelConsumptionSummaryExport);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = "FuelsSummariesTotal",
                Type = UserBehaviorType.End
            });
        }

        public override void OnDestroy()
        {
            base.Dispose();
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = "FuelsSummariesTotal",
                Type = UserBehaviorType.Start
            });
        }

        public ICommand DetailVehicleCommand { get; private set; }

        public ICommand DetailRedirectVehicleCommand { get; private set; }

        public ICommand OpenPopupCommand { get; set; }

        // cấu hình không quá số ngày cho phép để tìm kiếm dữ liệu
        public override int AddDayMinfromDate { get; set; } = MobileSettingHelper.ViewReport;

        public override int ShowHideColumnTableID { get; set; } = (int)TableReportEnum.FuelsSummariesTotal;

        private bool displayPopup;
        public bool DisplayPopup { get => displayPopup; set => SetProperty(ref displayPopup, value); }

        private bool showOrderNumber = true;
        public bool ShowOrderNumber { get => showOrderNumber; set => SetProperty(ref showOrderNumber, value); }

        private bool showFK_Date = true;
        public bool ShowFK_Date { get => showFK_Date; set => SetProperty(ref showFK_Date, value); }

        private bool showStartEndTime = true;
        public bool ShowStartEndTime { get => showStartEndTime; set => SetProperty(ref showStartEndTime, value); }

        private bool showNumberOfPour = false;
        public bool ShowNumberOfPour { get => showNumberOfPour; set => SetProperty(ref showNumberOfPour, value); }

        private bool showNumberOfSuction = false;
        public bool ShowNumberOfSuction { get => showNumberOfSuction; set => SetProperty(ref showNumberOfSuction, value); }

        private bool showFirtLits = true;
        public bool ShowFirtLits { get => showFirtLits; set => SetProperty(ref showFirtLits, value); }

        private bool showLitersOfPour = false;
        public bool ShowLitersOfPour { get => showLitersOfPour; set => SetProperty(ref showLitersOfPour, value); }

        private bool showLitersOfSuction = true;
        public bool ShowLitersOfSuction { get => showLitersOfSuction; set => SetProperty(ref showLitersOfSuction, value); }

        private bool showLastLits = true;
        public bool ShowLastLits { get => showLastLits; set => SetProperty(ref showLastLits, value); }

        private bool showLitsConsumable = true;
        public bool ShowLitsConsumable { get => showLitsConsumable; set => SetProperty(ref showLitsConsumable, value); }

        private bool showTotalKmGps = false;
        public bool ShowTotalKmGps { get => showTotalKmGps; set => SetProperty(ref showTotalKmGps, value); }

        private bool showMinutesOfManchineOn = false;
        public bool ShowMinutesOfManchineOn { get => showMinutesOfManchineOn; set => SetProperty(ref showMinutesOfManchineOn, value); }

        private bool showMinuteOfMachineOnStop = false;
        public bool ShowMinuteOfMachineOnStop { get => showMinuteOfMachineOnStop; set => SetProperty(ref showMinuteOfMachineOnStop, value); }

        private bool showQuotaRegulations = false;
        public bool ShowQuotaRegulations { get => showQuotaRegulations; set => SetProperty(ref showQuotaRegulations, value); }

        private bool showQuotaReality = false;
        public bool ShowQuotaReality { get => showQuotaReality; set => SetProperty(ref showQuotaReality, value); }

        private FuelsSummariesTotalResponse selectFuelsSummariesItem;
        public FuelsSummariesTotalResponse SelectFuelsSummariesItem { get => selectFuelsSummariesItem; set => SetProperty(ref selectFuelsSummariesItem, value); }

        private FuelsSummariesTotalRequest selectFuelsSummariesTotalItem;
        public FuelsSummariesTotalRequest SelectFuelsSummariesTotalItem { get => selectFuelsSummariesTotalItem; set => SetProperty(ref selectFuelsSummariesTotalItem, value); }

        private ObservableCollection<ShowHideColumnResponse> listShowHideComlumn;
        public ObservableCollection<ShowHideColumnResponse> ListShowHideComlumn { get => listShowHideComlumn; set => SetProperty(ref listShowHideComlumn, value); }

        public bool IsExportExcel { get; set; }

        /// <summary>Set dữ liệu đầu vào</summary>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/26/2019   created
        /// </Modified>
        public override FuelsSummariesTotalRequest SetDataInput()
        {
            return new FuelsSummariesTotalRequest
            {
                CompanyID = CurrentComanyID,
                VehicleIDs = VehicleSelect.VehicleId.ToString(),
                FromDate = base.FromDate,
                ToDate = base.ToDate,
                PageIndex = base.PagedNext,
                PageSize = 10000
            };
        }

        /// <summary>Converts lại số thứ tự theo trang</summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/26/2019   created
        /// </Modified>
        public override IList<FuelsSummariesTotalResponse> ConvertDataBeforeDisplay(IList<FuelsSummariesTotalResponse> data)
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
        /// linhlv  11/26/2019   created
        /// </Modified>
        private async void ExecuteDetailVehicle(int? OrderNumber)
        {
            try
            {
                SelectFuelsSummariesItem = ListDataSearch.Where(x => x.OrderNumber == OrderNumber).FirstOrDefault();

                SelectFuelsSummariesItem.VehiclePlate = VehicleSelect.VehiclePlate;

                var p = new NavigationParameters
                {
                    { ParameterKey.ReportFuelsSummariesTotalSelected, SelectFuelsSummariesItem }
                };
                await NavigationService.NavigateAsync("FuelsSummariesTotalDetailReportPage", p, useModalNavigation: false, true);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>Gọi service truyền dữ liệu sang bên trang báo cao tiêu hao</summary>
        /// <param name="OrderNumber">The order number.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/26/2019   created
        /// </Modified>
        private async void ExecuteDetailRedirectVehicle()
        {
            try
            {
                var request = new FuelsSummariesTotalRequest
                {
                    VehicleIDs = VehicleSelect.VehicleId.ToString(),
                    VehiclePlate = VehicleSelect.VehiclePlate,
                    FromDate = FromDate,
                    ToDate = ToDate
                };

                var p = new NavigationParameters
                {
                    { ParameterKey.ReportFuelsSummariesTotalSelected, request }
                };
                await NavigationService.NavigateAsync("NavigationPage/FuelsSummariesReportPage", p, useModalNavigation: true, true);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>Mở Popup</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  12/4/2019   created
        /// </Modified>
        private void OpenPopup()
        {
            DisplayPopup = true;
        }

        /// <summary>Đổ dữ liệu vào excel</summary>
        /// <param name="data">The data.</param>
        /// <param name="worksheet">The worksheet.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/27/2019   created
        /// </Modified>
        public override void FillDataTableExcell(IList<FuelsSummariesTotalResponse> data, ref IWorksheet worksheet)
        {
            try
            {
                //Gán lại tên file
                ReportTitle = ReportHelper.GetFileName(MobileResource.FuelsSummariesReportTotal_Label_TilePage);

                int numberrow = 4;
                int numbercolum = 0;
                // title
                // Số thứ tự
                if (ShowOrderNumber)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Table_Serial;
                }
                // Từ ngày / Đến ngày
                if (ShowStartEndTime)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Table_StartEndTime2;
                }
                // Số lần đổ
                if (ShowNumberOfPour)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Table_NumberOfPour;
                }
                // Số lần hút
                if (ShowNumberOfSuction)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Table_NumberOfSuction;
                }
                // Số lít đầu ngày
                if (ShowFirtLits)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Table_FirtLits;
                }
                // Số lít đổ
                if (ShowLitersOfPour)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Table_LitersOfPour;
                }
                // Số lít hút
                if (ShowLitersOfSuction)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Table_LitersOfSuction;
                }
                // Số lít tồn
                if (ShowLastLits)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Table_LastLits;
                }
                // Số lít tiêu hao
                if (ShowLitsConsumable)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Table_LitsConsumable;
                }
                // Tổng Km GPS
                if (ShowTotalKmGps)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Table_TotalKmGps;
                }
                // Thời gian nổ máy (phút)
                if (ShowMinutesOfManchineOn)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Table_MinutesOfManchineOn;
                }
                // Thời gian dừng đỗ nổ máy (phút)
                if (ShowMinuteOfMachineOnStop)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Table_MinuteOfMachineOnStop;
                }
                // Định mức quy định
                if (ShowQuotaRegulations)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Table_QuotaRegulations;
                }
                // Định mức thực tế
                if (ShowQuotaReality)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Table_QuotaReality;
                }

                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.Font.Bold = true;
                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.ColorIndex = ExcelKnownColors.Sky_blue;
                //head
                worksheet.Range[1, 1].Text = MobileResource.FuelsSummariesReportTotal_Label_TilePage;
                worksheet.Range[1, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[1, 1].CellStyle.Font.Bold = true;
                worksheet.Range[1, 1].CellStyle.Font.Size = 16;
                worksheet.Range[1, 1, 1, numbercolum].Merge();
                worksheet.Range[2, 1].Text = MobileResource.Common_Label_PlaceHolder_FromDate + ": " + DateTimeHelper.FormatOnlyDate(FromDate) + " " + MobileResource.Common_Label_PlaceHolder_ToDate + ": " + DateTimeHelper.FormatOnlyDate(ToDate);
                worksheet.Range[2, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[2, 1, 2, numbercolum].Merge();
                worksheet.Range[3, 1].Text = MobileResource.Common_Label_Grid_VehiclePlate + ": " + VehicleSelect.PrivateCode;
                worksheet.Range[3, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[3, 1, 3, numbercolum].Merge();
                // data
                for (int i = 0, length = data.Count; i < length; i++)
                {
                    numberrow += 1;
                    numbercolum = 0;
                    // Số thứ tự
                    if (ShowOrderNumber)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].OrderNumber.ToString();
                    }
                    // Từ ngày / Đến ngày
                    if (ShowStartEndTime)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = DateTimeHelper.FormatDateTime(data[i].StartTime) + " - " + DateTimeHelper.FormatDateTime(data[i].EndTime);
                    }
                    // Số lần đổ
                    if (ShowNumberOfPour)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].SumPourCount.ToString();
                    }
                    // Số lần hút
                    if (ShowNumberOfSuction)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].SumSuckCount.ToString();
                    }
                    // Số lít đầu ngày
                    if (ShowFirtLits)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data[i].SumFirstLits);
                    }
                    // Số lít đổ
                    if (ShowLitersOfPour)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data[i].SumPourTotal);
                    }
                    // Số lít hút
                    if (ShowLitersOfSuction)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data[i].SumSuckTotal);
                    }
                    // Số lít tồn
                    if (ShowLastLits)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data[i].SumLastLits);
                    }
                    // Số lít tiêu hao
                    if (ShowLitsConsumable)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data[i].SumLiterConsumable);
                    }
                    // Tổng Km GPS
                    if (ShowTotalKmGps)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data[i].SumTotalKmGps);
                    }
                    // Thời gian nổ máy (phút)
                    if (ShowMinutesOfManchineOn)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].SumMinuteOfMachineOn.ToString();
                    }
                    // Thời gian dừng đỗ nổ máy (phút)
                    if (ShowMinuteOfMachineOnStop)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].SumMinuteOfMachineOnStop.ToString();
                    }
                    // Định mức quy định
                    if (ShowQuotaRegulations)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data[i].SumNormsOfGasonline);
                    }
                    // Định mức thực tế
                    if (ShowQuotaReality)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data[i].SumRealNormsOfGasonline);
                    }
                }

                worksheet.Range[4, 1, numberrow, numbercolum].BorderAround();
                worksheet.Range[4, 1, numberrow, numbercolum].BorderInside(ExcelLineStyle.Thin, ExcelKnownColors.Black);

                //sum
                numberrow += 1;
                numbercolum = 0;
                // Số thứ tự
                if (ShowOrderNumber)
                {
                    numbercolum += 1;
                }
                // Từ ngày / Đến ngày
                if (ShowStartEndTime)
                {
                    numbercolum += 1;
                }

                // Số lần đổ
                if (ShowNumberOfPour)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data.Sum(x => x.SumPourCount).ToString();
                }
                // Số lần hút
                if (ShowNumberOfSuction)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data.Sum(x => x.SumSuckCount).ToString();
                }
                // Số lít đầu ngày
                if (ShowFirtLits)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data.Sum(x => x.SumFirstLits));
                }
                // Số lít đổ
                if (ShowLitersOfPour)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data.Sum(x => x.SumPourTotal));
                }
                // Số lít hút
                if (ShowLitersOfSuction)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data.Sum(x => x.SumSuckTotal));
                }
                // Số lít tồn
                if (ShowLastLits)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data.Sum(x => x.SumLastLits));
                }
                // Số lít tiêu hao
                if (ShowLitsConsumable)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data.Sum(x => x.SumLiterConsumable));
                }
                // Tổng Km GPS
                if (ShowTotalKmGps)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data.Sum(x => x.SumTotalKmGps));
                }
                // Thời gian nổ máy (phút)
                if (ShowMinutesOfManchineOn)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data.Sum(x => x.SumMinuteOfMachineOn).ToString();
                }
                // Thời gian dừng đỗ nổ máy (phút)
                if (ShowMinuteOfMachineOnStop)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data.Sum(x => x.SumMinuteOfMachineOnStop).ToString();
                }
                // Định mức quy định
                if (ShowQuotaRegulations)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data.Sum(x => x.SumNormsOfGasonline));
                }
                // Định mức thực tế
                if (ShowQuotaReality)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data.Sum(x => x.SumRealNormsOfGasonline));
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
        /// linhlv  11/26/2019   created
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
                    { 1, ShowOrderNumber },
                    { 2, ShowStartEndTime },
                    { 3, ShowNumberOfPour },
                    { 4, ShowNumberOfSuction },
                    { 5, ShowFirtLits },
                    { 6, ShowLitersOfPour },
                    { 7, ShowLitersOfSuction },
                    { 8, ShowLastLits },
                    { 9, ShowLitsConsumable },
                    { 10, ShowTotalKmGps },
                    { 11, ShowMinutesOfManchineOn },
                    { 12, ShowMinuteOfMachineOnStop },
                    { 13, ShowQuotaRegulations },
                    { 14, ShowQuotaReality }
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
                            ShowOrderNumber = item.Value;
                            break;

                        case 2:
                            ShowStartEndTime = item.Value;
                            break;

                        case 3:
                            ShowNumberOfPour = item.Value;
                            break;

                        case 4:
                            ShowNumberOfSuction = item.Value;
                            break;

                        case 5:
                            ShowFirtLits = item.Value;
                            break;

                        case 6:
                            ShowLitersOfPour = item.Value;
                            break;

                        case 7:
                            ShowLitersOfSuction = item.Value;
                            break;

                        case 8:
                            ShowLastLits = item.Value;
                            break;

                        case 9:
                            ShowLitsConsumable = item.Value;
                            break;

                        case 10:
                            ShowTotalKmGps = item.Value;
                            break;

                        case 11:
                            ShowMinutesOfManchineOn = item.Value;
                            break;

                        case 12:
                            ShowMinuteOfMachineOnStop = item.Value;
                            break;

                        case 13:
                            ShowQuotaRegulations = item.Value;
                            break;

                        case 14:
                            ShowQuotaReality = item.Value;
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
        /// linhlv  11/26/2019   created
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