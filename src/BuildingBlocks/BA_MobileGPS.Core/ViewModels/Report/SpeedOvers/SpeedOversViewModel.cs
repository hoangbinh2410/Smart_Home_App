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
    public class SpeedOversViewModel : ReportBase<SpeedOversRequest, SpeedOversService, SpeedOversModel>
    {
        private readonly IShowHideColumnService showHideColumnService;

        public SpeedOversViewModel(INavigationService navigationService, IShowHideColumnService showHideColumnService)
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

            IsExportExcel = CheckPermision((int)PermissionKeyNames.ReportSpeedOverExport);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = "SpeedOvers",
                Type = UserBehaviorType.End
            });
        }

        public override void OnDestroy()
        {
            base.Dispose();
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = "SpeedOvers",
                Type = UserBehaviorType.Start
            });
        }

        public ICommand DetailVehicleCommand { get; private set; }

        // cấu hình không quá số ngày cho phép để tìm kiếm dữ liệu
        public override int AddDayMinfromDate { get; set; } = MobileSettingHelper.ViewReport;

        public override int ShowHideColumnTableID { get; set; } = (int)TableReportEnum.SpeedOvers;

        private bool showSpeedOverTotalTime = true;
        public bool ShowSpeedOverTotalTime { get => showSpeedOverTotalTime; set => SetProperty(ref showSpeedOverTotalTime, value); }

        private bool showSpeedOverTotalDistance = true;
        public bool ShowSpeedOverTotalDistance { get => showSpeedOverTotalDistance; set => SetProperty(ref showSpeedOverTotalDistance, value); }

        private bool showVmax = true;
        public bool ShowVmax { get => showVmax; set => SetProperty(ref showVmax, value); }

        private bool showStartAddress = true;
        public bool ShowStartAddress { get => showStartAddress; set => SetProperty(ref showStartAddress, value); }

        private bool showEndAddress = true;
        public bool ShowEndAddress { get => showEndAddress; set => SetProperty(ref showEndAddress, value); }

        private bool showNote = true;
        public bool ShowNote { get => showNote; set => SetProperty(ref showNote, value); }

        private string speed = string.Empty;
        public string Speed { get => speed; set => SetProperty(ref speed, value); }

        private SpeedOversModel selectSpeedOversVehicleItem;
        public SpeedOversModel SelectSpeedOversVehicleItem { get => selectSpeedOversVehicleItem; set => SetProperty(ref selectSpeedOversVehicleItem, value); }

        private ObservableCollection<ShowHideColumnResponse> listShowHideComlumn;
        public ObservableCollection<ShowHideColumnResponse> ListShowHideComlumn { get => listShowHideComlumn; set => SetProperty(ref listShowHideComlumn, value); }

        public bool IsExportExcel { get; set; }

        /// <summary>Set dữ liệu đầu vào</summary>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/13/2019   created
        /// </Modified>
        public override SpeedOversRequest SetDataInput()
        {
            return new SpeedOversRequest
            {
                CompanyID = CurrentComanyID,
                VehicleIDs = VehicleSelect.VehicleId.ToString(),
                FromDate = base.FromDate,
                ToDate = base.ToDate,
                VelocityMax = string.IsNullOrEmpty(Speed) ? 0 : int.Parse(Speed),
                IsAddress = ShowStartAddress || ShowEndAddress,
                PageIndex = base.PagedNext,
                PageSize = base.PageSize,
            };
        }

        /// <summary>Converts lại số thứ tự theo trang</summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/13/2019   created
        /// </Modified>
        public override IList<SpeedOversModel> ConvertDataBeforeDisplay(IList<SpeedOversModel> data)
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
                SelectSpeedOversVehicleItem = ListDataSearch.Where(x => x.OrderNumber == OrderNumber).FirstOrDefault();
                SelectSpeedOversVehicleItem.VehiclePlate = VehicleSelect.VehiclePlate;
                if (string.IsNullOrEmpty(SelectSpeedOversVehicleItem.StartAddress) && string.IsNullOrEmpty(SelectSpeedOversVehicleItem.EndAddress))
                {
                    var startLat = SelectSpeedOversVehicleItem.StartLatitude;
                    var startLong = SelectSpeedOversVehicleItem.StartLongitude;
                    var endLat = SelectSpeedOversVehicleItem.EndLatitude;
                    var endLong = SelectSpeedOversVehicleItem.EndLongitude;
                    if (string.IsNullOrEmpty(SelectSpeedOversVehicleItem.StartAddress) && string.IsNullOrEmpty(SelectSpeedOversVehicleItem.EndAddress))
                    {
                        var input = string.Format("{0} {1};{2} {3}", startLat, startLong, endLat, endLong);
                        var response = await BaseServiceReport.GetAddressReport(input);
                        if (response.Count >= 2)
                        {
                            SelectSpeedOversVehicleItem.StartAddress = response[0];
                            SelectSpeedOversVehicleItem.EndAddress = response[1];
                        }
                    }
                    else if (string.IsNullOrEmpty(SelectSpeedOversVehicleItem.StartAddress))
                    {
                        var input = string.Format("{0} {1}}", startLat, startLong);
                        var response = await BaseServiceReport.GetAddressReport(input);
                        SelectSpeedOversVehicleItem.StartAddress = response[0];
                    }
                    else if (string.IsNullOrEmpty(SelectSpeedOversVehicleItem.EndAddress))
                    {
                        var input = string.Format("{0} {1}}", endLat, endLong);
                        var response = await BaseServiceReport.GetAddressReport(input);
                        SelectSpeedOversVehicleItem.EndAddress = response[0];
                    }
                }
                var p = new NavigationParameters
                {
                    { ParameterKey.ReportSpeedOversSelected, SelectSpeedOversVehicleItem }
                };
                await NavigationService.NavigateAsync("SpeedOversDetailReportPage", p, useModalNavigation: false, true);
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
        public override void FillDataTableExcell(IList<SpeedOversModel> data, ref IWorksheet worksheet)
        {
            try
            {
                //Gán lại tên file
                ReportTitle = ReportHelper.GetFileName(MobileResource.SpeedOversReport_Label_TilePage);

                int numberrow = 4;
                int numbercolum = 1;
                // STT
                worksheet.Range[numberrow, numbercolum].Text = MobileResource.SpeedOversReport_Table_Serial;

                // Biển số xe
                numbercolum += 1;
                worksheet.Range[numberrow, numbercolum].Text = MobileResource.SpeedOversReport_Table_DetailVehiclePlate;

                // Thời điểm
                numbercolum += 1;
                worksheet.Range[numberrow, numbercolum].Text = MobileResource.SpeedOversReport_Table_Date;

                // Thời gian (s)
                if (ShowSpeedOverTotalTime)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.SpeedOversReport_Table_Time;
                }
                // Quãng đường
                if (ShowSpeedOverTotalDistance)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.SpeedOversReport_Table_Distance;
                }
                // Tốc độ cực đại
                if (ShowVmax)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.SpeedOversReport_Table_Vmax;
                }
                //  Điểm bắt đầu
                if (ShowStartAddress)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.SpeedOversReport_Table_StartAddress;
                }
                // Điểm kết thúc
                if (ShowEndAddress)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.SpeedOversReport_Table_EndAddress;
                }

                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.Font.Bold = true;
                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.ColorIndex = ExcelKnownColors.Sky_blue;
                //head
                worksheet.Range[1, 1].Text = MobileResource.SpeedOversReport_Label_TilePage;
                worksheet.Range[1, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[1, 1].CellStyle.Font.Bold = true;
                worksheet.Range[1, 1].CellStyle.Font.Size = 16;
                worksheet.Range[1, 1, 1, numbercolum].Merge();
                worksheet.Range[2, 1].Text = MobileResource.Common_Label_PlaceHolder_FromDate + ": " + DateTimeHelper.FormatDateTime(FromDate) + " " + MobileResource.Common_Label_PlaceHolder_ToDate + ": " + DateTimeHelper.FormatDateTime(ToDate);
                worksheet.Range[2, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[2, 1, 2, numbercolum].Merge();
                string option = MobileResource.Common_Label_Grid_VehiclePlate + ": " + VehicleSelect.PrivateCode;
                if (!string.IsNullOrEmpty(Speed))
                    option += "| " + MobileResource.SpeedOversReport_Label_PlaceHolder_Speed + " " + Speed;
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
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data[i].PrivateCode.ToString();

                    //  Thời điểm
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = DateTimeHelper.FormatDateTime(data[i].StartTime);

                    //  Thời gian (s)
                    if (ShowSpeedOverTotalTime)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].SpeedOverTotalTime.ToString();
                    }

                    // Quãng đường
                    if (ShowSpeedOverTotalDistance)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].SpeedOverDistance.ToString();
                    }

                    // Tốc độ cực đại
                    if (ShowVmax)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].Vmax.ToString();
                    }

                    // Điểm bắt đầu
                    if (ShowStartAddress)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].StartAddress;
                    }

                    // Điểm kết thúc
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
                numbercolum = 3;
                //  Thời gian (s)
                if (ShowSpeedOverTotalTime)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data.Sum(x => x.SpeedOverTotalTime).ToString();
                }

                // Quãng đường
                if (ShowSpeedOverTotalDistance)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data.Sum(x => x.SpeedOverDistance).ToString();
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
                    { 1, ShowSpeedOverTotalTime },
                    { 2, ShowSpeedOverTotalDistance },
                    { 3, ShowVmax },
                    { 4, ShowStartAddress },
                    { 5, ShowEndAddress },
                    { 6, ShowNote }
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
                            ShowSpeedOverTotalTime = item.Value;
                            break;

                        case 2:
                            ShowSpeedOverTotalDistance = item.Value;
                            break;

                        case 3:
                            ShowVmax = item.Value;
                            break;

                        case 4:
                            ShowStartAddress = item.Value;
                            break;

                        case 5:
                            ShowEndAddress = item.Value;
                            break;

                        case 6:
                            ShowNote = item.Value;
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
            if (!string.IsNullOrEmpty(Speed))
            {
                // check trường tốc độ - chỉ cho nhập kiểu int
                if (!int.TryParse(Speed, out int speed))
                {
                    message = MobileResource.SpeedOversReport_Message_ValidateError_Speed;
                    return false;
                }
            }
            return true;
        }
    }
}