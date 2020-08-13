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

namespace BA_MobileGPS.Core.ViewModels
{
    public class ReportTableTemperatureViewModel : ReportBase<TemperartureVehicleRequest, ReportTemperatureService, TemperatureVehicleResponse>
    {
        private readonly IShowHideColumnService showHideColumnService;

        public ReportTableTemperatureViewModel(INavigationService navigationService, IShowHideColumnService showHideColumnService)
            : base(navigationService)
        {
            try
            {
                this.showHideColumnService = showHideColumnService;
                // khải báo phần button và sự kiện

                DetailTemperatureCommand = new DelegateCommand<int?>(ExecuteDetailTemperature);
                SelectTemperatureItem = new TemperatureVehicleResponse();

                DisplayComlumnHide();

                ListShowHideComlumn = new ObservableCollection<ShowHideColumnResponse>()
                {
                    new ShowHideColumnResponse() { IDColumn = 1, Value = true},
                   new ShowHideColumnResponse() { IDColumn = 2, Value = false}
                };

                IsExportExcel = CheckPermision((int)PermissionKeyNames.ReportTemperatureExport);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        #region property

        public override int PermissionKeyView { get; set; } = (int)PermissionKey.ReportTemperatureView;

        // thuộc tính nhiệt độ tìm kiếm
        private string _temperatureCondition;

        public string TemperatureCondition
        {
            get { return _temperatureCondition; }
            set { SetProperty(ref _temperatureCondition, value); }
        }

        private ObservableCollection<ShowHideColumnResponse> _listShowHideComlumn;

        public ObservableCollection<ShowHideColumnResponse> ListShowHideComlumn
        {
            get { return _listShowHideComlumn; }
            set { SetProperty(ref _listShowHideComlumn, value); }
        }

        public TemperatureVehicleResponse SelectTemperatureItem
        {
            get;
            set;
        }

        // cấu hình không quá số ngày cho phép để tìm kiếm dữ liệu
        public override int AddDayMinfromDate { get; set; } = MobileSettingHelper.ViewReport;

        public override string Title { get; set; } = MobileResource.ReportTemperature_Label_TitlePage;

        public override int ShowHideColumnTableID { get; set; } = (int)TableReportEnum.TemperatureReport;

        private bool _showdToDate = true;

        public bool ShowToDate
        {
            get { return _showdToDate; }
            set { SetProperty(ref _showdToDate, value); }
        }

        private bool _showKM = true;

        public bool ShowKM
        {
            get { return _showKM; }
            set { SetProperty(ref _showKM, value); }
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

        public bool IsExportExcel { get; set; }

        public override IDictionary<int, bool> ShowHideColumnDictionary
        {
            get
            {
                IDictionary<int, bool> ShowHideColumnDictionary = new Dictionary<int, bool>
                {
                    { 1, ShowToDate },
                    { 2, ShowKM },
                    { 3, ShowStartAddress },
                    { 4, ShowEndAddress }
                };
                return ShowHideColumnDictionary;
            }
        }

        #endregion property

        #region Command

        public DelegateCommand<int?> DetailTemperatureCommand { get; private set; }

        #endregion Command

        #region Execute command

        private async void ExecuteDetailTemperature(int? OrderNumber)
        {
            try
            {
                // gọi service truyền dữ liệu sang bên trang chi tiết

                SelectTemperatureItem = ListDataSearch.Where(x => x.OrderNumber == OrderNumber).FirstOrDefault();
                SelectTemperatureItem.VehiclePlate = VehicleSelect.VehiclePlate;
                if (string.IsNullOrEmpty(SelectTemperatureItem.StartAddress) && string.IsNullOrEmpty(SelectTemperatureItem.EndAddress))
                {
                    var startLat = SelectTemperatureItem.StartLatitude;
                    var startLong = SelectTemperatureItem.StartLongitude;
                    var endLat = SelectTemperatureItem.EndLatitude;
                    var endLong = SelectTemperatureItem.EndLongitude;
                    if (string.IsNullOrEmpty(SelectTemperatureItem.StartAddress) && string.IsNullOrEmpty(SelectTemperatureItem.EndAddress))
                    {
                        var input = string.Format("{0} {1};{2} {3}", startLat, startLong, endLat, endLong);
                        var response = await BaseServiceReport.GetAddressReport(input);
                        if (response.Count >= 2)
                        {
                            SelectTemperatureItem.StartAddress = response[0];
                            SelectTemperatureItem.EndAddress = response[1];
                        }
                    }
                    else if (string.IsNullOrEmpty(SelectTemperatureItem.StartAddress))
                    {
                        var input = string.Format("{0} {1}}", startLat, startLong);
                        var response = await BaseServiceReport.GetAddressReport(input);
                        SelectTemperatureItem.StartAddress = response[0];
                    }
                    else if (string.IsNullOrEmpty(SelectTemperatureItem.EndAddress))
                    {
                        var input = string.Format("{0} {1}}", endLat, endLong);
                        var response = await BaseServiceReport.GetAddressReport(input);
                        SelectTemperatureItem.EndAddress = response[0];
                    }
                }

                var p = new NavigationParameters
                {
                    { ParameterKey.ReportTemperatureSelected, SelectTemperatureItem }
                };
                await NavigationService.NavigateAsync("ReportDetailTemperaturePage", p, useModalNavigation: false);
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
        public override void FillDataTableExcell(IList<TemperatureVehicleResponse> data, ref IWorksheet worksheet)
        {
            try
            {
                //Gán lại tên file
                ReportTitle = ReportHelper.GetFileName(MobileResource.ReportTemperature_Label_TitlePage);

                int numberrow = 4;
                int numbercolum = 1;
                // STT
                worksheet.Range[numberrow, numbercolum].Text = MobileResource.Common_Label_TitleGrid_OrderNumber;

                // thời gian bắt đầu
                numbercolum += 1;
                worksheet.Range[numberrow, numbercolum].Text = MobileResource.Common_Label_Grid_FromDate;

                //thời gian kết thúc
                if (ShowToDate)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.Common_Label_Grid_ToDate;
                }
                // Kilomet
                if (ShowKM)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ReportTemperature_Label_Grid_Kilomet;
                }

                // Nhiệt độ
                numbercolum += 1;
                worksheet.Range[numberrow, numbercolum].Text = MobileResource.ReportTemperature_Label_Grid_Temperature;

                // Cảm biến nhiệt độ
                numbercolum += 1;
                worksheet.Range[numberrow, numbercolum].Text = MobileResource.ReportTemperature_Label_Grid_NumberOfSensors;

                // địa chỉ đi
                if (ShowStartAddress)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ReportTemperature_Label_Grid_StartAddress;
                }
                // địa chỉ đến
                if (ShowEndAddress)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = MobileResource.ReportTemperature_Label_Grid_EndAddress;
                }

                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.Font.Bold = true;
                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.ColorIndex = ExcelKnownColors.Sky_blue;
                //head
                worksheet.Range[1, 1].Text = MobileResource.ReportTemperature_Label_TitlePage;
                worksheet.Range[1, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[1, 1].CellStyle.Font.Bold = true;
                worksheet.Range[1, 1].CellStyle.Font.Size = 16;
                worksheet.Range[1, 1, 1, numbercolum].Merge();
                worksheet.Range[2, 1].Text = MobileResource.Common_Label_PlaceHolder_FromDate + ": " + DateTimeHelper.FormatDateTime(FromDate) + " " + MobileResource.Common_Label_PlaceHolder_ToDate + ": " + DateTimeHelper.FormatDateTime(ToDate);
                worksheet.Range[2, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[2, 1, 2, numbercolum].Merge();
                string option = MobileResource.Common_Label_Grid_VehiclePlate + ": " + VehicleSelect.PrivateCode;
                if (!string.IsNullOrEmpty(TemperatureCondition))
                    option += "| " + MobileResource.ReportTemperature_Label_PlaceHolder_Temperature + " " + TemperatureCondition;
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

                    //  thời gian bắt đầu
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = DateTimeHelper.FormatDateTime(data[i].StartTime);

                    //  thời gian kết thúc
                    if (ShowToDate)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = DateTimeHelper.FormatDateTime(data[i].EndTime);
                    }

                    // Kilomet
                    if (ShowKM)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].Kilometer.ToString();
                    }

                    //  Nhiệt độ
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data[i].Temperature.ToString();

                    // Cảm biến nhiệt độ
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data[i].NumberOfSensorsSTR;

                    // địa chỉ đi
                    if (ShowStartAddress)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].StartAddress;
                    }
                    // địa chỉ đến
                    if (ShowEndAddress)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].EndAddress;
                    }
                }

                worksheet.Range[4, 1, numberrow, numbercolum].BorderAround();
                worksheet.Range[4, 1, numberrow, numbercolum].BorderInside(ExcelLineStyle.Thin, ExcelKnownColors.Black);
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
                // load danh sách công ty
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        public override TemperartureVehicleRequest SetDataInput()
        {
            var input = new TemperartureVehicleRequest
            {
                CompanyID = (UserHelper.isCompanyPartner(UserInfo.CompanyType) || (UserHelper.isCompanyEndUserWithPermisstion(UserInfo.CompanyType))) ? CurrentComanyID : StaticSettings.User.CompanyId,
                ListVehicleID = VehicleSelect.VehicleId.ToString(),
                PageIndex = PagedNext,
                PageSize = PageSize,
                DateStart = FromDate,
                DateEnd = ToDate,
                Temperature = string.IsNullOrEmpty(TemperatureCondition) ? "-1000" : TemperatureCondition,
                IsAddress = ShowEndAddress || ShowStartAddress,
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

            // check trường nhiệt độ - chỉ cho nhập kiểu double
            if (!string.IsNullOrEmpty(TemperatureCondition))
            {
                if (!double.TryParse(TemperatureCondition, out _))
                {
                    message = MobileResource.ReportTemperature_Message_ValidateError_Temperation;
                    return false;
                }
                else
                {
                    TemperatureCondition = FormatHelper.ConvertToDouble(TemperatureCondition).ToString();
                }
            }

            return true;
        }

        public override IList<TemperatureVehicleResponse> ConvertDataBeforeDisplay(IList<TemperatureVehicleResponse> data)
        {
            int i = (PagedNext - 1) * PageSize;
            foreach (var item in data)
            {
                item.OrderNumber = ++i;
                item.NumberOfSensorsSTR = MobileResource.ReportTemperature_Label_Grid_NumberOfSensors + " " + item.NumberOfSensors;
            }
            return data;
        }

        public override void ExecuteSaveComlumnHide()
        {
            // thực hiện lưu các thông tin liên quan tới ẩn hiện cột vào đây
            ListShowHideComlumn.Clear();
            foreach (var item in ShowHideColumnDictionary)
            {
                ListShowHideComlumn.Add(new ShowHideColumnResponse() { IDColumn = item.Key, Value = item.Value });
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
                            ShowKM = item.Value;
                            break;

                        case 3:
                            ShowStartAddress = item.Value;
                            break;

                        case 4:
                            ShowEndAddress = item.Value;
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion Execute command
    }
}