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
    public class SignalLossViewModel : ReportBase<SignalLossRequest, SignalLossServices, SignalLossResponse>
    {
        private readonly IShowHideColumnService showHideColumnService;

        public SignalLossViewModel(INavigationService navigationService, IShowHideColumnService showHideColumnService) : base(navigationService)
        {
            this.showHideColumnService = showHideColumnService;

            statusSignalLossSelected = new ComboboxResponse()
            {
                Key = 0,
                Value = MobileResource.ReportSignalLoss_TitleStatus_All
            };

            ListShowHideComlumn = new ObservableCollection<ShowHideColumnResponse>()
            {
                new ShowHideColumnResponse() { IDColumn = 1, Value = true},
                new ShowHideColumnResponse() { IDColumn = 2, Value = false}
            };

            DetailSignalLossCommand = new DelegateCommand<int?>(ExecuteDetailSignalLoss);

            PushStatusSignalLossCommand = new DelegateCommand(ExecuteStatusSignalLossCombobox);

            DisplayComlumnHide();

            IsExportExcel = CheckPermision((int)PermissionKeyNames.ReportSignalLossExport);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.ReportSignalLoss,
                Type = UserBehaviorType.Start
            });
        }

        public override void OnDestroy()
        {
            base.Dispose();
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.ReportSignalLoss,
                Type = UserBehaviorType.End
            });
        }

        public ICommand DetailSignalLossCommand { get; private set; }

        public ICommand PushStatusSignalLossCommand { get; private set; }

        // cấu hình không quá số ngày cho phép để tìm kiếm dữ liệu
        public override int AddDayMinfromDate { get; set; } = MobileSettingHelper.ViewReport;

        public override int ShowHideColumnTableID { get; set; } = (int)TableReportEnum.Details;

        private bool showTimeLosing = true;
        public bool ShowTimeLosing { get => showTimeLosing; set => SetProperty(ref showTimeLosing, value); }

        private bool showStartTime = true;
        public bool ShowStartTime { get => showStartTime; set => SetProperty(ref showStartTime, value); }

        private bool showEndTime = true;
        public bool ShowEndTime { get => showEndTime; set => SetProperty(ref showEndTime, value); }

        private bool showStartLosingLocation = true;
        public bool ShowStartLosingLocation { get => showStartLosingLocation; set => SetProperty(ref showStartLosingLocation, value); }

        private bool showStopLosingLocation = true;
        public bool ShowStopLosingLocation { get => showStopLosingLocation; set => SetProperty(ref showStopLosingLocation, value); }

        private bool showStatus = true;
        public bool ShowStatus { get => showStatus; set => SetProperty(ref showStatus, value); }

        private string minTimeLosing;

        public string MinTimeLosing
        {
            get { return minTimeLosing; }
            set
            {
                SetProperty(ref minTimeLosing, value);
            }
        }

        public bool IsExportExcel { get; set; }

        private SignalLossResponse selectDetailsItem;
        public SignalLossResponse SelectDetailsItem { get => selectDetailsItem; set => SetProperty(ref selectDetailsItem, value); }

        private ComboboxResponse statusSignalLossSelected;
        public ComboboxResponse StatusSignalLossSelected { get => statusSignalLossSelected; set => SetProperty(ref statusSignalLossSelected, value); }

        private ObservableCollection<ShowHideColumnResponse> listShowHideComlumn;
        public ObservableCollection<ShowHideColumnResponse> ListShowHideComlumn { get => listShowHideComlumn; set => SetProperty(ref listShowHideComlumn, value); }

        public override SignalLossRequest SetDataInput()
        {
            return new SignalLossRequest
            {
                CompanyID = CurrentComanyID,
                VehicleIDs = VehicleSelect.VehicleId.ToString(),
                FromDate = base.FromDate,
                ToDate = base.ToDate,
                MinuteLossSignal = string.IsNullOrEmpty(MinTimeLosing) ? 5 : int.Parse(MinTimeLosing),
                PageIndex = base.PagedNext,
                PageSize = base.PageSize,
                Type = (byte)StatusSignalLossSelected.Key
            };
        }

        public override IList<SignalLossResponse> ConvertDataBeforeDisplay(IList<SignalLossResponse> data)
        {
            foreach (var item in data)
            {
                TempRowNumber += 1;
                item.OrderNumber = TempRowNumber;
            }
            return data;
        }

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
                    { 1, ShowTimeLosing },
                    { 2, ShowStartTime },
                    { 3, ShowEndTime },
                    { 4, ShowStartLosingLocation },
                    { 5, ShowStopLosingLocation },
                    { 6, ShowStatus }
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
                            ShowTimeLosing = item.Value;
                            break;

                        case 2:
                            ShowStartTime = item.Value;
                            break;

                        case 3:
                            ShowEndTime = item.Value;
                            break;

                        case 4:
                            ShowStartLosingLocation = item.Value;
                            break;

                        case 5:
                            ShowStopLosingLocation = item.Value;
                            break;

                        case 6:
                            ShowStatus = item.Value;
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public override void UpdateCombobox(ComboboxResponse param)
        {
            base.UpdateCombobox(param);
            if (param != null)
            {
                var dataResponse = param;
                if (dataResponse.ComboboxType == (Int16)ComboboxType.First)
                {
                    StatusSignalLossSelected = dataResponse;
                }
            }
        }

        private List<ComboboxRequest> LoadAllStatusSignalLoss()
        {
            return new List<ComboboxRequest>() {
                    new ComboboxRequest(){Key = 0 , Value = MobileResource.ReportSignalLoss_TitleStatus_All},
                    new ComboboxRequest(){Key = 1 , Value = MobileResource.ReportSignalLoss_TitleStatus_GPS},
                    new ComboboxRequest(){Key = 2 , Value = MobileResource.ReportSignalLoss_TitleStatus_GMS},
                };
        }

        public async void ExecuteStatusSignalLossCombobox()
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
                    { "dataCombobox", LoadAllStatusSignalLoss() },
                    { "ComboboxType", ComboboxType.First },
                    { "Title", MobileResource.ReportSignalLoss_TitleStatus }
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

        private async void ExecuteDetailSignalLoss(int? OrderNumber)
        {
            try
            {
                // gọi service truyền dữ liệu sang bên trang chi tiết

                SelectDetailsItem = ListDataSearch.Where(x => x.OrderNumber == OrderNumber).FirstOrDefault();

                var p = new NavigationParameters
                {
                    { ParameterKey.ReportSignalLossSelected, SelectDetailsItem }
                };
                await NavigationService.NavigateAsync("SignalLossReportDetailPage", p, useModalNavigation: false, true);
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
        public override void FillDataTableExcell(IList<SignalLossResponse> data, ref IWorksheet worksheet)
        {
            try
            {
                //Gán lại tên file
                ReportTitle = ReportHelper.GetFileName(MobileResource.ReportSignalLoss_Label_TitlePage);

                int numberrow = 4;
                int numbercolum = 1;
                // STT
                worksheet.Range[numberrow, numbercolum].Text = MobileResource.DetailsReport_Table_Serial;

                // Thời gian bắt đầu
                if (ShowStartTime)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ReportSignalLoss_Title_StartTime;
                }
                // Thời gian kết thúc
                if (ShowEndTime)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ReportSignalLoss_Title_EndTime;
                }
                // Thời gian mất tin hiệu
                if (ShowTimeLosing)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ReportSignalLoss_Title_TimeLosing;
                }

                // Địa điểm bắt đầu
                if (ShowStartLosingLocation)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ReportSignalLoss_Title_StartAddress;
                }

                // Địa điểm kết thúc
                if (ShowStopLosingLocation)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ReportSignalLoss_Title_EndAddress;
                }

                // Trạng thái
                if (ShowStatus)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ReportSignalLoss_TitleStatus;
                }

                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.Font.Bold = true;
                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.ColorIndex = ExcelKnownColors.Sky_blue;
                //head
                worksheet.Range[1, 1].Text = MobileResource.ReportSignalLoss_Label_TitlePage;
                worksheet.Range[1, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[1, 1].CellStyle.Font.Bold = true;
                worksheet.Range[1, 1].CellStyle.Font.Size = 16;
                worksheet.Range[1, 1, 1, numbercolum].Merge();
                worksheet.Range[2, 1].Text = MobileResource.Common_Label_PlaceHolder_FromDate + ": " + DateTimeHelper.FormatDateTime(FromDate) + " " + MobileResource.Common_Label_PlaceHolder_ToDate + ": " + DateTimeHelper.FormatDateTime(ToDate);
                worksheet.Range[2, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[2, 1, 2, numbercolum].Merge();
                string option = MobileResource.Common_Label_Grid_VehiclePlate + ": " + VehicleSelect.PrivateCode;
                option += "| " + MobileResource.ReportSignalLoss_TitleStatus + ": " + StatusSignalLossSelected.Value;
                if (!string.IsNullOrEmpty(MinTimeLosing))
                    option += "| " + MobileResource.ReportSignalLoss_Title_MinTimeLosing + " " + MinTimeLosing;
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

                    // Thời gian bắt đầu
                    if (ShowStartTime)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = DateTimeHelper.FormatDateTime(data[i].StartTime);
                    }

                    // Thời gian kết thúc
                    if (ShowEndTime)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = DateTimeHelper.FormatDateTime(data[i].EndTime);
                    }

                    // Thời gian mất tin hiệu
                    if (ShowTimeLosing)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = DateTimeHelper.FormatTimeSpan24h(data[i].TotalTimes);
                    }

                    // Địa điểm bắt đầu
                    if (ShowStartLosingLocation)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].StartAddress;
                    }

                    // Địa điểm kết thúc
                    if (ShowStopLosingLocation)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].EndAddress;
                    }

                    // Trạng thái
                    if (ShowStatus)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].Status;
                    }
                }

                worksheet.Range[4, 1, numberrow, numbercolum].BorderAround();
                worksheet.Range[4, 1, numberrow, numbercolum].BorderInside(ExcelLineStyle.Thin, ExcelKnownColors.Black);

                // sum
                numberrow += 1;
                numbercolum = 1;

                // Thời gian bắt đầu
                if (ShowStartTime)
                {
                    numbercolum += 1;
                }

                // Thời gian kết thúc
                if (ShowEndTime)
                {
                    numbercolum += 1;
                }

                // Thời gian mất tin hiệu
                if (ShowTimeLosing)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = DateTimeHelper.FormatTimeSpan24h(new TimeSpan(data.Sum(x => x.TotalTimes.Ticks)));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
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

            if (!string.IsNullOrEmpty(MinTimeLosing))
            {
                // check trường số phút - chỉ cho nhập kiểu int
                if (!int.TryParse(MinTimeLosing, out int minTimeLosing))
                {
                    message = MobileResource.ReportSignalLoss_Message_ValidateError_MinTimeLosing;
                    return false;
                }
                else
                {
                    if (int.Parse(MinTimeLosing) < 5)
                    {
                        message = MobileResource.ReportSignalLoss_Message_ValidateError_MinTimeLosing2;
                        return false;
                    }
                }
            }

            return true;
        }
    }
}