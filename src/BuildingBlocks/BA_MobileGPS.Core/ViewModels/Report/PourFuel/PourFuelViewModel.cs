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
    public class PourFuelViewModel : ReportBase<FuelReportRequest, PourFuelService, FuelVehicleModel>
    {
        private readonly IShowHideColumnService showHideColumnService;

        public PourFuelViewModel(INavigationService navigationService, IShowHideColumnService showHideColumnService)
            : base(navigationService)
        {
            this.showHideColumnService = showHideColumnService;

            statusPourFuelSelected = new ComboboxResponse()
            {
                Key = 0,
                Value = MobileResource.PourFuelReport_ComboBox_Status_Pour
            };

            ListShowHideComlumn = new ObservableCollection<ShowHideColumnResponse>()
            {
                new ShowHideColumnResponse() { IDColumn = 1, Value = true},
                new ShowHideColumnResponse() { IDColumn = 2, Value = false}
            };

            DetailPourFuelCommand = new DelegateCommand<int?>(ExecuteDetailPourFuel);

            PushStatusPourFuelCommand = new DelegateCommand(ExecuteStatusPourFuelCombobox);

            DisplayComlumnHide();

            IsExportExcel = CheckPermision((int)PermissionKeyNames.ReportFuelExport);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.ReportPourFuel,
                Type = UserBehaviorType.Start
            });
        }

        public override void OnDestroy()
        {
            base.Dispose();
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.ReportPourFuel,
                Type = UserBehaviorType.End
            });
        }

        public ICommand DetailPourFuelCommand { get; private set; }
        public ICommand PushStatusPourFuelCommand { get; private set; }

        // cấu hình không quá số ngày cho phép để tìm kiếm dữ liệu
        public override int AddDayMinfromDate { get; set; } = MobileSettingHelper.ViewReport;

        public override int ShowHideColumnTableID { get; set; } = (int)TableReportEnum.PourFuel;

        private bool showSerial = true;
        public bool ShowSerrial { get => showSerial; set => SetProperty(ref showSerial, value); }

        private bool showdToDate = true;
        public bool ShowToDate { get => showdToDate; set => SetProperty(ref showdToDate, value); }

        private bool showLiters = true;
        public bool ShowLiters { get => showLiters; set => SetProperty(ref showLiters, value); }

        private bool showStartAddress = true;
        public bool ShowStartAddress { get => showStartAddress; set => SetProperty(ref showStartAddress, value); }

        private bool showEndAddress = true;
        public bool ShowEndAddress { get => showEndAddress; set => SetProperty(ref showEndAddress, value); }

        private bool showAddress = true;
        public bool ShowAddress { get => showAddress; set => SetProperty(ref showAddress, value); }

        private bool showStatus = true;
        public bool ShowStatus { get => showStatus; set => SetProperty(ref showStatus, value); }

        private FuelVehicleModel selectPourFuelVehicleItem;
        public FuelVehicleModel SelectPourFuelVehicleItem { get => selectPourFuelVehicleItem; set => SetProperty(ref selectPourFuelVehicleItem, value); }

        private string liters = string.Empty;
        public string Liters { get => liters; set => SetProperty(ref liters, value); }

        private ComboboxResponse statusPourFuelSelected;
        public ComboboxResponse StatusPourFuelSelected { get => statusPourFuelSelected; set => SetProperty(ref statusPourFuelSelected, value); }

        private ObservableCollection<ShowHideColumnResponse> listShowHideComlumn;
        public ObservableCollection<ShowHideColumnResponse> ListShowHideComlumn { get => listShowHideComlumn; set => SetProperty(ref listShowHideComlumn, value); }

        public bool IsExportExcel { get; set; }

        private List<ComboboxRequest> LoadAllStatusMachine()
        {
            return new List<ComboboxRequest>() {
                    new ComboboxRequest(){Key = 0 , Value = MobileResource.PourFuelReport_ComboBox_Status_Pour},
                    new ComboboxRequest(){Key = 1 , Value = MobileResource.PourFuelReport_ComboBox_Status_Absorb},
                    new ComboboxRequest(){Key = 2 , Value = MobileResource.PourFuelReport_ComboBox_Status_SuspiciousPour},
                    new ComboboxRequest(){Key = 3 , Value = MobileResource.PourFuelReport_ComboBox_Status_SuspiciousAbsorb},
                };
        }

        public override FuelReportRequest SetDataInput()
        {
            return new FuelReportRequest
            {
                CompanyID = CurrentComanyID,
                ListVehicleID = VehicleSelect.VehicleId.ToString(),
                DateStart = base.FromDate,
                DateEnd = base.ToDate,
                PageIndex = base.PagedNext,
                PageSize = base.PageSize,
                IsAddress = ShowStartAddress || ShowEndAddress,
                SearchType = (FuelStatusEnum)StatusPourFuelSelected.Key,
                NumberOfLitter = string.IsNullOrEmpty(Liters) ? 0 : float.Parse(Liters),
            };
        }

        public override IList<FuelVehicleModel> ConvertDataBeforeDisplay(IList<FuelVehicleModel> data)
        {
            int i = (PagedNext - 1) * PageSize;
            foreach (var item in data)
            {
                item.OrderNumber = ++i;
                item.FuelStatus = StatusPourFuelSelected.Value;
            }
            return data;
        }

        public async void ExecuteStatusPourFuelCombobox()
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

        public override void UpdateCombobox(ComboboxResponse param)
        {
            base.UpdateCombobox(param);
            if (param != null)
            {
                var dataResponse = param;
                if (dataResponse.ComboboxType == (Int16)ComboboxType.First)
                {
                    StatusPourFuelSelected = dataResponse;
                }
            }
        }

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

        public override IDictionary<int, bool> ShowHideColumnDictionary
        {
            get
            {
                return new Dictionary<int, bool>
                {
                    { 1, ShowAddress },
                    { 2, ShowStatus }
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
                            ShowAddress = item.Value;
                            break;

                        case 2:
                            ShowStatus = item.Value;
                            break;
                    }
                }
            }
            catch (Exception)
            {
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
            if (!string.IsNullOrEmpty(Liters))
            {
                // check trường nhiệt độ - chỉ cho nhập kiểu float
                if (!float.TryParse(Liters, out float liters))
                {
                    message = MobileResource.PourFuelReport_Message_ValidateError_Liters;
                    return false;
                }
            }
            return true;
        }

        private async void ExecuteDetailPourFuel(int? OrderNumber)
        {
            try
            {
                // gọi service truyền dữ liệu sang bên trang chi tiết

                SelectPourFuelVehicleItem = ListDataSearch.Where(x => x.OrderNumber == OrderNumber).FirstOrDefault();
                SelectPourFuelVehicleItem.VehiclePlate = VehicleSelect.VehiclePlate;
                if (string.IsNullOrEmpty(SelectPourFuelVehicleItem.CurrentAddress))
                {
                    if (string.IsNullOrEmpty(SelectPourFuelVehicleItem.StartAddress) && string.IsNullOrEmpty(SelectPourFuelVehicleItem.EndAddress))
                    {
                        var startLat = SelectPourFuelVehicleItem.StartLatitude;
                        var startLong = SelectPourFuelVehicleItem.StartLongitude;
                        var endLat = SelectPourFuelVehicleItem.EndLatitude;
                        var endLong = SelectPourFuelVehicleItem.EndLongitude;
                        if (string.IsNullOrEmpty(SelectPourFuelVehicleItem.StartAddress) && string.IsNullOrEmpty(SelectPourFuelVehicleItem.EndAddress))
                        {
                            var input = string.Format("{0} {1};{2} {3}", startLat, startLong, endLat, endLong);
                            var response = await BaseServiceReport.GetAddressReport(input);
                            if (response.Count >= 2)
                            {
                                SelectPourFuelVehicleItem.StartAddress = response[0];
                                SelectPourFuelVehicleItem.EndAddress = response[1];
                            }
                        }
                        else if (string.IsNullOrEmpty(SelectPourFuelVehicleItem.StartAddress))
                        {
                            var input = string.Format("{0} {1}}", startLat, startLong);
                            var response = await BaseServiceReport.GetAddressReport(input);
                            SelectPourFuelVehicleItem.StartAddress = response[0];
                        }
                        else if (string.IsNullOrEmpty(SelectPourFuelVehicleItem.EndAddress))
                        {
                            var input = string.Format("{0} {1}}", endLat, endLong);
                            var response = await BaseServiceReport.GetAddressReport(input);
                            SelectPourFuelVehicleItem.EndAddress = response[0];
                        }
                    }
                }
                var p = new NavigationParameters
                {
                    { ParameterKey.ReportPourFuelSelected, SelectPourFuelVehicleItem }
                };
                await NavigationService.NavigateAsync("PourFuelDetailReportPage", p, useModalNavigation: false, true);
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
        public override void FillDataTableExcell(IList<FuelVehicleModel> data, ref IWorksheet worksheet)
        {
            try
            {
                //Gán lại tên file
                ReportTitle = ReportHelper.GetFileName(MobileResource.PourFuelReport_Label_TilePage);

                int numberrow = 4;
                int numbercolum = 1;
                // STT
                worksheet.Range[numberrow, numbercolum].Text = MobileResource.PourFuelReport_Table_Serial;

                // thời gian
                numbercolum += 1;
                worksheet.Range[numberrow, numbercolum].Text = MobileResource.PourFuelReport_Table_Date;

                // Số Lít ShowLiters
                if (ShowLiters)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.PourFuelReport_CheckBox_Liters;
                }
                // Trạng thái
                if (ShowStatus)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.PourFuelReport_Table_Status;
                }
                // Địa chỉ
                if (ShowAddress)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.PourFuelReport_Table_Address;
                }

                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.Font.Bold = true;
                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.ColorIndex = ExcelKnownColors.Sky_blue;
                //head
                worksheet.Range[1, 1].Text = MobileResource.PourFuelReport_Label_TitlePage;
                worksheet.Range[1, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[1, 1].CellStyle.Font.Bold = true;
                worksheet.Range[1, 1].CellStyle.Font.Size = 16;
                worksheet.Range[1, 1, 1, numbercolum].Merge();
                worksheet.Range[2, 1].Text = MobileResource.Common_Label_PlaceHolder_FromDate + ": " + DateTimeHelper.FormatDateTime(FromDate) + " " + MobileResource.Common_Label_PlaceHolder_ToDate + ": " + DateTimeHelper.FormatDateTime(ToDate);
                worksheet.Range[2, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[2, 1, 2, numbercolum].Merge();
                string option = MobileResource.Common_Label_Grid_VehiclePlate + ": " + VehicleSelect.PrivateCode;
                option += "| " + MobileResource.PourFuelReport_Label_PlaceHolder_StatusPourFuel + ": " + StatusPourFuelSelected.Value;
                if (!string.IsNullOrEmpty(Liters))
                    option += "| " + MobileResource.PourFuelReport_Label_PlaceHolder_Liters + " " + Liters;
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

                    // thời gian
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = DateTimeHelper.FormatDateTime(data[i].StartTime);

                    // Số Lít ShowLiters
                    if (ShowLiters)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].ChangingFuel.ToString();
                    }
                    // Trạng thái
                    if (ShowStatus)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].FuelStatus;
                    }
                    // Địa chỉ
                    if (ShowAddress)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].CurrentAddress;
                    }
                }

                worksheet.Range[4, 1, numberrow, numbercolum].BorderAround();
                worksheet.Range[4, 1, numberrow, numbercolum].BorderInside(ExcelLineStyle.Thin, ExcelKnownColors.Black);

                // sum
                numberrow += 1;
                numbercolum = 2;

                // Thời gian hoạt động
                if (ShowLiters)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = Math.Round(data.Sum(x => x.ChangingFuel), 1).ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }
    }
}