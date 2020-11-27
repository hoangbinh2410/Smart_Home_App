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
    public class ActivitySummariesViewModel : ReportBase<ActivitySummariesRequest, ActivitySummariesService, ActivitySummariesModel>
    {
        private readonly IShowHideColumnService showHideColumnService;

        public ActivitySummariesViewModel(INavigationService navigationService, IShowHideColumnService showHideColumnService)
            : base(navigationService)
        {
            this.showHideColumnService = showHideColumnService;

            ListShowHideComlumn = new ObservableCollection<ShowHideColumnResponse>()
            {
                new ShowHideColumnResponse() { IDColumn = 1, Value = true},
                new ShowHideColumnResponse() { IDColumn = 2, Value = false}
            };

            DetailVehicleCommand = new DelegateCommand<int?>(ExecuteDetailVehicle);

            SelectJoinDayCommand = new DelegateCommand(ExecuteSelectJoinDay);

            DisplayComlumnHide();

            IsExportExcel = CheckPermision((int)PermissionKeyNames.ReportActivitySummaryExport);
        }

        public ICommand DetailVehicleCommand { get; private set; }

        public ICommand SelectJoinDayCommand { get; private set; }

        // cấu hình không quá số ngày cho phép để tìm kiếm dữ liệu
        public override int AddDayMinfromDate { get; set; } = MobileSettingHelper.ViewReport;

        public override int ShowHideColumnTableID { get; set; } = (int)TableReportEnum.ActivitySummaries;

        private bool showVehicleType = false;
        public bool ShowVehicleType { get => showVehicleType; set => SetProperty(ref showVehicleType, value); }

        private bool showVehiclePlate = false;
        public bool ShowVehiclePlate { get => showVehiclePlate; set => SetProperty(ref showVehiclePlate, value); }

        private bool showVmax = true;
        public bool ShowVmax { get => showVmax; set => SetProperty(ref showVmax, value); }

        private bool showCurrentTime = true;
        public bool ShowCurrentTime { get => showCurrentTime; set => SetProperty(ref showCurrentTime, value); }

        private bool showActiveTime = true;
        public bool ShowActiveTime { get => showActiveTime; set => SetProperty(ref showActiveTime, value); }

        private bool showKmGPS = true;
        public bool ShowKmGPS { get => showKmGPS; set => SetProperty(ref showKmGPS, value); }

        private bool showStopParkingTime = true;
        public bool ShowStopParkingTime { get => showStopParkingTime; set => SetProperty(ref showStopParkingTime, value); }

        private bool showKmCO = true;
        public bool ShowKmCO { get => showKmCO; set => SetProperty(ref showKmCO, value); }

        private bool showNumberOfStopParking = true;
        public bool ShowNumberOfStopParking { get => showNumberOfStopParking; set => SetProperty(ref showNumberOfStopParking, value); }

        private bool showMinutesTurnOnAirConditioner = true;
        public bool ShowMinutesTurnOnAirConditioner { get => showMinutesTurnOnAirConditioner; set => SetProperty(ref showMinutesTurnOnAirConditioner, value); }

        private bool showVmedium = true;
        public bool ShowVmedium { get => showVmedium; set => SetProperty(ref showVmedium, value); }

        private ActivitySummariesModel selectActivitySummariesItem;
        public ActivitySummariesModel SelectActivitySummariesItem { get => selectActivitySummariesItem; set => SetProperty(ref selectActivitySummariesItem, value); }

        private ObservableCollection<ShowHideColumnResponse> listShowHideComlumn;
        public ObservableCollection<ShowHideColumnResponse> ListShowHideComlumn { get => listShowHideComlumn; set => SetProperty(ref listShowHideComlumn, value); }

        private ObservableCollection<ActivitySummariesModel> listBeforeJoinDay;
        public ObservableCollection<ActivitySummariesModel> ListBeforeJoinDay { get => listBeforeJoinDay; set => SetProperty(ref listBeforeJoinDay, value); }

        public bool IsExportExcel { get; set; }

        /// <summary>Set dữ liệu đầu vào</summary>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/13/2019   created
        /// </Modified>
        public override ActivitySummariesRequest SetDataInput()
        {
            return new ActivitySummariesRequest
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
        public override IList<ActivitySummariesModel> ConvertDataBeforeDisplay(IList<ActivitySummariesModel> data)
        {
            int i = (PagedNext - 1) * PageSize;
            foreach (var item in data)
            {
                item.OrderNumber = ++i;
                item.JoinFK_Date = DateTimeHelper.FormatOnlyDate(item.FK_Date);
                item.JoinStartTimes = string.Format("{0:hh\\:mm}", item.StartTimes);
                item.JoinEndTimes = string.Format("{0:hh\\:mm}", item.EndTimes);
                item.JoinActivityTimes = string.Format("{0:hh\\:mm}", item.ActivityTimes);
                item.JoinTotalTimeStops = string.Format("{0:hh\\:mm}", item.TotalTimeStops);
                item.TotalKmGps = item.StartTimes.Hours == 0
                    && item.StartTimes.Minutes == 0
                    && item.EndTimes.Hours == 0
                    && item.EndTimes.Minutes == 0
                    ? 0 : item.TotalKmGps;
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
                SelectActivitySummariesItem = ListDataSearch.Where(x => x.OrderNumber == OrderNumber).FirstOrDefault();
                SelectActivitySummariesItem.VehiclePlate = VehicleSelect.VehiclePlate;

                var p = new NavigationParameters
                {
                    { ParameterKey.ReportActivitySummariesSelected, SelectActivitySummariesItem }
                };
                await NavigationService.NavigateAsync("ActivitySummariesDetailReportPage", p, useModalNavigation: false, true);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>Gộp ngày, tính toán lại thông số</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/18/2019   created
        /// </Modified>
        private void ExecuteSelectJoinDay()
        {
            try
            {
                IsJoinDay = !IsJoinDay;
                if (IsJoinDay == true)
                {
                    if (ListDataSearch != null && ListDataSearch.Count > 0)
                    {
                        ListBeforeJoinDay = new ObservableCollection<ActivitySummariesModel>(ListDataSearch);
                        if (ListDataSearch.Count > 1)
                        {
                            var respone = new List<ActivitySummariesModel>();
                            //Số ngày hoạt động thực tế
                            var count = ListDataSearch.Where(item => item.TotalKmGps > 0).GroupBy(u => u.FK_Date.ToString("dd/MM/yyyy")).ToList().Count;

                            var list = ListDataSearch.GroupBy(u => u.FK_Date.ToString("dd/MM/yyyy")).Select(grp => grp.ToList()).ToList();
                            //lấy giờ nhỏ nhất ngày đầu tiên
                            var iStartTimes = list[0].Select(x => x.StartTimes).Min();
                            //lấy giờ lớn nhất ngày cuối cùng
                            var iEndTimes = list[list.Count - 1].Select(x => x.EndTimes).Max();
                            //lấy ngày bắt đầu
                            var iStartFK_Date = ListDataSearch.Select(x => x.FK_Date).Min();
                            //lấy ngày kết thúc
                            var iEndFK_Date = ListDataSearch.Select(x => x.FK_Date).Max();

                            respone.Add(new ActivitySummariesModel()
                            {
                                OrderNumber = 1,
                                NameType = list[0][0].NameType,
                                VehiclePlate = list[0][0].VehiclePlate,
                                JoinFK_Date = string.Format("{0} {1}", count, MobileResource.ActivitySummariesReport_Label_TitleJoinNameDate.ToLower()),
                                JoinStartTimes = string.Format("{0} {1}", string.Format("{0:hh\\:mm}", iStartTimes), DateTimeHelper.FormatOnlyDate(iStartFK_Date)),
                                JoinEndTimes = string.Format("{0} {1}", string.Format("{0:hh\\:mm}", iEndTimes), DateTimeHelper.FormatOnlyDate(iEndFK_Date)),
                                JoinActivityTimes = DateTimeHelper.FormatTimeSpan24h(new TimeSpan(ListDataSearch.Sum(x => x.ActivityTimes.Ticks))),
                                JoinTotalTimeStops = DateTimeHelper.FormatTimeSpan24h(new TimeSpan(ListDataSearch.Sum(x => x.TotalTimeStops.Ticks))),
                                TotalTimeStops = new TimeSpan(ListDataSearch.Where(item => item.TotalKmGps > 0).Sum(x => x.TotalTimeStops.Ticks)),
                                TotalKmGps = (float)Math.Round(ListDataSearch.Sum(x => x.TotalKmGps), 1),
                                KmOfMechanical = ListDataSearch.Sum(x => x.KmOfMechanical),
                                StopCount = ListDataSearch.Sum(x => x.StopCount),
                                MinutesOfAirConditioningOn = ListDataSearch.Sum(x => x.MinutesOfAirConditioningOn),
                                Vmax = ListDataSearch.Select(x => x.Vmax).Max(),
                                Varg = (int)Math.Round(ListDataSearch.Where(item => item.TotalKmGps > 0).Average(x => x.Varg))
                            });
                            ListDataSearch = respone;
                        }
                    }
                    else
                    {
                        DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_NoDataJoinDay, CountMinutesShowMessageReport);
                        IsJoinDay = false;
                    }
                }
                else
                {
                    ListDataSearch = ListBeforeJoinDay;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>Đổ dữ liệu vào excel</summary>
        /// <param name="data">The data.</param>
        /// <param name="worksheet">The worksheet.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/27/2019   created
        /// </Modified>
        public override void FillDataTableExcell(IList<ActivitySummariesModel> data, ref IWorksheet worksheet)
        {
            try
            {
                //Gán lại tên file
                ReportTitle = ReportHelper.GetFileName(MobileResource.ActivitySummariesReport_Label_TilePage);

                int numberrow = 4;
                int numbercolum = 1;
                // title
                worksheet.Range[numberrow, 1].Text = MobileResource.ActivitySummariesReport_Table_Serial;
                if (ShowCurrentTime)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ActivitySummariesReport_Table_CurrentTime;
                }
                numbercolum += 1;
                worksheet.Range[numberrow, numbercolum].Text = MobileResource.ActivitySummariesReport_Table_StartEndTime;
                if (ShowActiveTime)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ActivitySummariesReport_Table_ActiveTime;
                }
                if (ShowStopParkingTime)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ActivitySummariesReport_Table_StopParkingTime;
                }
                if (ShowKmGPS)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ActivitySummariesReport_Table_KmGPS;
                }
                if (ShowKmCO)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ActivitySummariesReport_Table_KmCO;
                }
                if (ShowVehicleType)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ActivitySummariesReport_Table_VehicleType;
                }
                if (ShowVehiclePlate)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ActivitySummariesReport_Table_VehiclePlate;
                }
                if (ShowNumberOfStopParking)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ActivitySummariesReport_Table_NumberOfStopParking;
                }
                if (ShowMinutesTurnOnAirConditioner)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ActivitySummariesReport_Table_MinutesTurnOnAirConditioner;
                }
                if (ShowVmax)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ActivitySummariesReport_Table_Vmax;
                }
                if (ShowVmedium)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ActivitySummariesReport_Table_Vmedium;
                }
                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.Font.Bold = true;
                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.ColorIndex = ExcelKnownColors.Sky_blue;
                //head
                worksheet.Range[1, 1].Text = MobileResource.ActivitySummariesReport_Label_TilePage;
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
                    numbercolum = 1;
                    worksheet.Range[numberrow, 1].Text = data[i].OrderNumber.ToString();
                    if (ShowCurrentTime)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].JoinFK_Date;
                    }
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data[i].JoinStartTimes + " - " + data[i].JoinEndTimes;
                    if (ShowActiveTime)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].JoinActivityTimes;
                    }
                    if (ShowStopParkingTime)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].JoinTotalTimeStops;
                    }
                    if (ShowKmGPS)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data[i].TotalKmGps);
                    }
                    if (ShowKmCO)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data[i].KmOfMechanical);
                    }
                    if (ShowVehicleType)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].NameType;
                    }
                    if (ShowVehiclePlate)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].VehiclePlate;
                    }
                    if (ShowNumberOfStopParking)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].StopCount.ToString();
                    }
                    if (ShowMinutesTurnOnAirConditioner)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].MinutesOfAirConditioningOn.ToString();
                    }
                    if (ShowVmax)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].Vmax.ToString();
                    }
                    if (ShowVmedium)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", data[i].Varg);
                    }
                }

                worksheet.Range[4, 1, numberrow, numbercolum].BorderAround();
                worksheet.Range[4, 1, numberrow, numbercolum].BorderInside(ExcelLineStyle.Thin, ExcelKnownColors.Black);

                // sum
                numberrow += 1;
                numbercolum = 1;
                if (ShowCurrentTime)
                {
                    numbercolum += 1;
                }
                numbercolum += 1;
                if (ShowActiveTime)
                {
                    numbercolum += 1;
                    if (data.Count == 1)
                    {
                        worksheet.Range[numberrow, numbercolum].Text = data[0].JoinActivityTimes;
                    }
                    else
                    {
                        worksheet.Range[numberrow, numbercolum].Text = DateTimeHelper.FormatTimeSpan24h(new TimeSpan(data.Sum(x => x.ActivityTimes.Ticks)));
                    }
                }
                if (ShowStopParkingTime)
                {
                    numbercolum += 1;
                }
                if (ShowKmGPS)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = Math.Round(data.Sum(x => x.TotalKmGps), 2).ToString();
                }
                if (ShowKmCO)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = String.Format("{0:0.##}", Math.Round(data.Sum(x => x.KmOfMechanical), 1));
                }
                if (ShowVehicleType)
                {
                    numbercolum += 1;
                }
                if (ShowVehiclePlate)
                {
                    numbercolum += 1;
                }
                if (ShowNumberOfStopParking)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data.Sum(x => x.StopCount).ToString();
                }
                if (ShowMinutesTurnOnAirConditioner)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data.Sum(x => x.MinutesOfAirConditioningOn).ToString();
                }
                if (ShowVmax)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data.Select(x => x.Vmax).Max().ToString();
                }
                if (ShowVmedium)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = ((int)Math.Round(data.Where(item => item.TotalKmGps > 0).Average(x => x.Varg))).ToString();
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
                    { 3, ShowVmax },
                    { 4, ShowCurrentTime },
                    { 5, ShowActiveTime },
                    { 6, ShowKmGPS },
                    { 7, ShowStopParkingTime },
                    { 8, ShowKmCO },
                    { 9, ShowNumberOfStopParking },
                    { 10, ShowMinutesTurnOnAirConditioner },
                    { 11, ShowVmedium }
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
                            ShowVmax = item.Value;
                            break;

                        case 4:
                            ShowCurrentTime = item.Value;
                            break;

                        case 5:
                            ShowActiveTime = item.Value;
                            break;

                        case 6:
                            ShowKmGPS = item.Value;
                            break;

                        case 7:
                            ShowStopParkingTime = item.Value;
                            break;

                        case 8:
                            ShowKmCO = item.Value;
                            break;

                        case 9:
                            ShowNumberOfStopParking = item.Value;
                            break;

                        case 10:
                            ShowMinutesTurnOnAirConditioner = item.Value;
                            break;

                        case 11:
                            ShowVmedium = item.Value;
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