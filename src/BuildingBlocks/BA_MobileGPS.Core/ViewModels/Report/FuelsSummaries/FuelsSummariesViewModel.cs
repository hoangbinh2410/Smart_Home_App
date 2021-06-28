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
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class FuelsSummariesViewModel : ReportBase<FuelsSummariesRequest, FuelsSummariesService, FuelsSummariesModel>
    {
        private readonly IShowHideColumnService showHideColumnService;

        public FuelsSummariesViewModel(INavigationService navigationService, IShowHideColumnService showHideColumnService)
            : base(navigationService)
        {
            this.showHideColumnService = showHideColumnService;

            ListShowHideComlumn = new ObservableCollection<ShowHideColumnResponse>()
            {
                new ShowHideColumnResponse() { IDColumn = 1, Value = true},
                new ShowHideColumnResponse() { IDColumn = 2, Value = false}
            };

            DetailVehicleCommand = new DelegateCommand<int?>(ExecuteDetailVehicle);

            OpenPopupCommand = new DelegateCommand(OpenPopup);

            DisplayComlumnHide();

            IsExportExcel = CheckPermision((int)PermissionKeyNames.FuelConsumptionDailyExport);
        }

        public ICommand DetailVehicleCommand { get; private set; }

        public ICommand OpenPopupCommand { get; set; }

        // cấu hình không quá số ngày cho phép để tìm kiếm dữ liệu
        public override int AddDayMinfromDate { get; set; } = MobileSettingHelper.ViewReport;

        public override int ShowHideColumnTableID { get; set; } = (int)TableReportEnum.FuelsSummaries;

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

        private string privateCode;
        public string PrivateCode { get => privateCode; set => SetProperty(ref privateCode, value); }

        private FuelsSummariesModel selectFuelsSummariesItem;
        public FuelsSummariesModel SelectFuelsSummariesItem { get => selectFuelsSummariesItem; set => SetProperty(ref selectFuelsSummariesItem, value); }

        private ObservableCollection<ShowHideColumnResponse> listShowHideComlumn;
        public ObservableCollection<ShowHideColumnResponse> ListShowHideComlumn { get => listShowHideComlumn; set => SetProperty(ref listShowHideComlumn, value); }

        public bool IsExportExcel { get; set; }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            //TempFileName = "Book1.xlsx";

            if (parameters.TryGetValue(ParameterKey.ReportFuelsSummariesTotalSelected, out FuelsSummariesTotalRequest request))
            {
                FromDate = request.FromDate;
                ToDate = request.ToDate;
                VehicleSelect.VehicleId = long.Parse(request.VehicleIDs);
                VehicleSelect.VehiclePlate = request.VehiclePlate;
                VehicleSelect.PrivateCode = request.VehiclePlate;
                PrivateCode = request.VehiclePlate;
                SearchDataCommand.Execute(null);
            }
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.FUELCONSUMPTIONDAILY,
                Type = UserBehaviorType.Start
            });
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.TryGetValue(ParameterKey.Vehicle, out Vehicle request))
            {
                PrivateCode = request.VehiclePlate;
            }
        }

        public override void OnDestroy()
        {
            base.Dispose();
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.FUELCONSUMPTIONDAILY,
                Type = UserBehaviorType.End
            });
        }

        /// <summary>Set dữ liệu đầu vào</summary>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/26/2019   created
        /// </Modified>
        public override FuelsSummariesRequest SetDataInput()
        {
            return new FuelsSummariesRequest
            {
                CompanyID = CurrentComanyID,
                VehicleIDs = VehicleSelect.VehicleId.ToString(),
                FromDate = base.FromDate,
                ToDate = base.ToDate,
                PageIndex = base.PagedNext,
                PageSize = base.PageSize
            };
        }

        /// <summary>Converts lại số thứ tự theo trang</summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/26/2019   created
        /// </Modified>
        public override IList<FuelsSummariesModel> ConvertDataBeforeDisplay(IList<FuelsSummariesModel> data)
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
                    { ParameterKey.ReportFuelsSummariesSelected, SelectFuelsSummariesItem }
                };
                await NavigationService.NavigateAsync("FuelsSummariesDetailReportPage", p, useModalNavigation: false, true);
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
        public override void FillDataTableExcell(IList<FuelsSummariesModel> data, ref IWorksheet worksheet)
        {
            try
            {
                //Gán lại tên file
                ReportTitle = ReportHelper.GetFileName(MobileResource.FuelsSummariesReport_Label_TilePage);

                int numberrow = 4;
                // title
                worksheet.Range[numberrow, 1].Text = MobileResource.Common_Label_TitleGrid_OrderNumber;
                worksheet.Range[numberrow, 2].Text = MobileResource.FuelsSummariesReport_Label_Date;
                worksheet.Range[numberrow, 3].Text = MobileResource.FuelsSummariesReport_Label_StartTime;
                worksheet.Range[numberrow, 4].Text = MobileResource.FuelsSummariesReport_Label_EndTime;
                ///
                int numbercolum = 4;
                if (ShowNumberOfPour)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Label_NumberOfPour;
                }
                if (ShowNumberOfSuction)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Label_NumberOfSuction;
                }
                if (ShowFirtLits)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Label_FirtLits;
                }
                if (ShowLitersOfPour)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Label_LitersOfPour;
                }
                if (ShowLitersOfSuction)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Label_LitersOfSuction;
                }
                if (ShowLastLits)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Label_LastLits;
                }
                if (ShowLitsConsumable)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Label_LitsConsumable;
                }
                if (ShowMinutesOfManchineOn)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Label_MinutesOfManchineOn;
                }
                if (ShowMinuteOfMachineOnStop)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Label_MinuteOfMachineOnStop;
                }
                if (ShowTotalKmGps)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Label_TotalKmGps;
                }
                if (ShowQuotaRegulations)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Label_QuotaRegulations;
                }
                if (ShowQuotaReality)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.FuelsSummariesReport_Label_QuotaReality;
                }
                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.Font.Bold = true;
                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.ColorIndex = ExcelKnownColors.Sky_blue;
                //head
                worksheet.Range[1, 1].Text = MobileResource.FuelsSummariesReport_Label_TilePage;
                worksheet.Range[1, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[1, 1].CellStyle.Font.Bold = true;
                worksheet.Range[1, 1].CellStyle.Font.Size = 16;
                worksheet.Range[1, 1, 1, numbercolum].Merge();
                worksheet.Range[2, 1].Text = MobileResource.Common_Label_PlaceHolder_FromDate + ": " + DateTimeHelper.FormatOnlyDate(FromDate) + " " + MobileResource.Common_Label_PlaceHolder_ToDate + ": " + DateTimeHelper.FormatOnlyDate(ToDate);
                worksheet.Range[2, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[2, 1, 2, numbercolum].Merge();
                worksheet.Range[3, 1].Text = MobileResource.Common_Label_Grid_VehiclePlate + ": " + PrivateCode;
                worksheet.Range[3, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[3, 1, 3, numbercolum].Merge();
                // data
                for (int i = 0, length = data.Count; i < length; i++)
                {
                    numberrow += 1;
                    worksheet.Range[numberrow, 1].Text = data[i].RowNumber.ToString();
                    worksheet.Range[numberrow, 2].Text = data[i].Date.FormatOnlyDate();
                    worksheet.Range[numberrow, 3].Text = data[i].StartTime.FormatTime();
                    worksheet.Range[numberrow, 4].Text = data[i].EndTime.FormatTime();
                    numbercolum = 4;
                    if (ShowNumberOfPour)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].PourCount.ToString();
                    }
                    if (ShowNumberOfSuction)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].SuckCount.ToString();
                    }
                    if (ShowFirtLits)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].FirstLits.ToString();
                    }
                    if (ShowLitersOfPour)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].PourTotal.ToString();
                    }
                    if (ShowLitersOfSuction)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].SuckTotal.ToString();
                    }
                    if (ShowLastLits)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].LastLits.ToString();
                    }
                    if (ShowLitsConsumable)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data[i].LiterConsumable);
                    }
                    if (ShowMinutesOfManchineOn)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].MinuteOfMachineOn.ToString();
                    }
                    if (ShowMinuteOfMachineOnStop)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].MinuteOfMachineOnStop.ToString();
                    }
                    if (ShowTotalKmGps)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].TotalKmGps.ToString();
                    }
                    if (ShowQuotaRegulations)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].NormsOfGasonline.ToString();
                    }
                    if (ShowQuotaReality)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].RealNormsOfGasonline.ToString();
                    }
                }

                worksheet.Range[4, 1, numberrow, numbercolum].BorderAround();
                worksheet.Range[4, 1, numberrow, numbercolum].BorderInside(ExcelLineStyle.Thin, ExcelKnownColors.Black);

                //sum
                numberrow += 1;
                numbercolum = 4;
                if (ShowNumberOfPour)
                {
                    numbercolum += 1;
                }
                if (ShowNumberOfSuction)
                {
                    numbercolum += 1;
                }
                if (ShowFirtLits)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", Math.Round(data.Sum(x => x.FirstLits), 1));
                }
                if (ShowLitersOfPour)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", Math.Round(data.Sum(x => x.PourTotal), 1));
                }
                if (ShowLitersOfSuction)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", Math.Round(data.Sum(x => x.SuckTotal), 1));
                }
                if (ShowLastLits)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", Math.Round(data.Sum(x => x.LastLits), 1));
                }
                if (ShowLitsConsumable)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", Math.Round(data.Sum(x => x.LiterConsumable), 1));
                }
                if (ShowMinutesOfManchineOn)
                {
                    numbercolum += 1;
                }
                if (ShowMinuteOfMachineOnStop)
                {
                    numbercolum += 1;
                }
                if (ShowTotalKmGps)
                {
                    numbercolum += 1;
                }
                if (ShowQuotaRegulations)
                {
                    numbercolum += 1;
                }
                if (ShowQuotaReality)
                {
                    numbercolum += 1;
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
                    { 2, ShowFK_Date },
                    { 3, ShowStartEndTime },
                    { 4, ShowNumberOfPour },
                    { 5, ShowNumberOfSuction },
                    { 6, ShowFirtLits },
                    { 7, ShowLitersOfPour },
                    { 8, ShowLitersOfSuction },
                    { 9, ShowLastLits },
                    { 10, ShowLitsConsumable },
                    { 11, ShowTotalKmGps },
                    { 12, ShowMinutesOfManchineOn },
                    { 13, ShowMinuteOfMachineOnStop },
                    { 14, ShowQuotaRegulations },
                    { 15, ShowQuotaReality }
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
                            ShowFK_Date = item.Value;
                            break;

                        case 3:
                            ShowStartEndTime = item.Value;
                            break;

                        case 4:
                            ShowNumberOfPour = item.Value;
                            break;

                        case 5:
                            ShowNumberOfSuction = item.Value;
                            break;

                        case 6:
                            ShowFirtLits = item.Value;
                            break;

                        case 7:
                            ShowLitersOfPour = item.Value;
                            break;

                        case 8:
                            ShowLitersOfSuction = item.Value;
                            break;

                        case 9:
                            ShowLastLits = item.Value;
                            break;

                        case 10:
                            ShowLitsConsumable = item.Value;
                            break;

                        case 11:
                            ShowTotalKmGps = item.Value;
                            break;

                        case 12:
                            ShowMinutesOfManchineOn = item.Value;
                            break;

                        case 13:
                            ShowMinuteOfMachineOnStop = item.Value;
                            break;

                        case 14:
                            ShowQuotaRegulations = item.Value;
                            break;

                        case 15:
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