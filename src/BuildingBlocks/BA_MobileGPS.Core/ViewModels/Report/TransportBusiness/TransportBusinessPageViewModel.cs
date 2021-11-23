﻿using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Entities.RequestEntity.Report.TransportBusiness;
using BA_MobileGPS.Entities.ResponeEntity.Report.TransportBusiness;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.Service.Report.TransportBusiness;
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
    public class TransportBusinessPageViewModel : ReportBase<TransportBusinessRequest, TransportBusinessService, TransportBusinessResponse>
    {

        #region Property

        // cấu hình không quá số ngày cho phép để tìm kiếm dữ liệu
        public override int AddDayMinfromDate { get; set; } = MobileSettingHelper.ViewReport;
        public override int ShowHideColumnTableID { get; set; } = (int)TableReportEnum.Details;

        // ẩn hiện cột
        private bool _showStartTime = true;
        public bool ShowStartTime { get => _showStartTime; set => SetProperty(ref _showStartTime, value); }

        private bool _showTimeActive = true;
        public bool ShowTimeActive { get => _showTimeActive; set => SetProperty(ref _showTimeActive, value); }

        private bool _showEndTime = true;
        public bool ShowEndTime { get => _showEndTime; set => SetProperty(ref _showEndTime, value); }

        private bool _showKmGPS = true;
        public bool ShowKmGPS { get => _showKmGPS; set => SetProperty(ref _showKmGPS, value); }

        private bool _showStartAddress = true;
        public bool ShowStartAddress { get => _showStartAddress; set => SetProperty(ref _showStartAddress, value); }

        private bool _showQuotaFuelConsume = true;
        public bool ShowQuotaFuelConsume { get => _showQuotaFuelConsume; set => SetProperty(ref _showQuotaFuelConsume, value); }

        private bool _showEndAddress = true;
        public bool ShowEndAddress { get => _showEndAddress; set => SetProperty(ref _showEndAddress, value); }

        private bool _showQuotaFuel = true;
        public bool ShowQuotaFuel { get => _showQuotaFuel; set => SetProperty(ref _showQuotaFuel, value); }

        private TransportBusinessRequest _objRequest;
        private ActivityDetailsModel selectDetailsItem;
        public ActivityDetailsModel SelectDetailsItem { get => selectDetailsItem; set => SetProperty(ref selectDetailsItem, value); }

        private ObservableCollection<ShowHideColumnResponse> listShowHideComlumn;
        public ObservableCollection<ShowHideColumnResponse> ListShowHideComlumn { get => listShowHideComlumn; set => SetProperty(ref listShowHideComlumn, value); }

        #endregion

        #region Contructor

        private readonly IShowHideColumnService showHideColumnService;
        public ICommand FilterDetailsCommand { get; private set; }
        public ICommand PushToViewRouteCommand { get; private set; }
        public ICommand PushToViewPicturnCommand { get; private set; }
        public TransportBusinessPageViewModel(INavigationService navigationService, IShowHideColumnService showHideColumnService)
            : base(navigationService)
        {
            this.showHideColumnService = showHideColumnService;

            ListShowHideComlumn = new ObservableCollection<ShowHideColumnResponse>()
            {
                new ShowHideColumnResponse() { IDColumn = 1, Value = true},
                new ShowHideColumnResponse() { IDColumn = 2, Value = false}
            };

            FilterDetailsCommand = new DelegateCommand(ExecuteFilterDetails);
            PushToViewRouteCommand = new DelegateCommand<int?>(PushToViewRoute);
            PushToViewPicturnCommand = new DelegateCommand<int?>(PushToViewPicturn);
            DisplayComlumnHide();
            IsExportExcel = CheckPermision((int)PermissionKeyNames.ReportActivityDetailExport);
            ToDate = DateTime.Now;
        }

        #endregion

        #region Lifecycle

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("DataBusinessRequest") && parameters.GetValue<TransportBusinessRequest>("DataBusinessRequest") is TransportBusinessRequest objRequest)
            {
                _objRequest = objRequest;
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.ReportActivityDetail,
                Type = UserBehaviorType.End
            });
        }

        public override void OnDestroy()
        {
            base.Dispose();
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.ReportActivityDetail,
                Type = UserBehaviorType.Start
            });
        }

        #endregion

        #region Menthod

        public bool IsExportExcel { get; set; }

        /// <summary>Set dữ liệu đầu vào</summary>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  15/11/2021   created
        /// </Modified>
        public override TransportBusinessRequest SetDataInput()
        {
            if(_objRequest != null)
            {
                return _objRequest;
            }    
            string vehicleIDs = "";
            if (!string.IsNullOrEmpty(VehicleSelect.VehiclePlate))
            {
                vehicleIDs = VehicleSelect.VehicleId.ToString();
            }
            // test
            vehicleIDs = "347503";
            //
            return new TransportBusinessRequest
            {
                CompanyID = CurrentComanyID,
                VehicleIDs = vehicleIDs,
                FromDate = FromDate,
                ToDate = ToDate,
            };
        }

        /// <summary>Converts lại số thứ tự theo trang</summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  11/13/2019   created
        /// </Modified>
        public override IList<TransportBusinessResponse> ConvertDataBeforeDisplay(IList<TransportBusinessResponse> data)
        {
            int stt = 0;
            foreach (var item in data)
            {
                item.OrderNumber = ++stt;
            }
            return data;
        }

        /// <summary>Put dữ liệu vào excel</summary>
        /// <param name="data">The data.</param>
        /// <param name="worksheet">The worksheet.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  26/11/2021   created
        /// </Modified>
        public override void FillDataTableExcell(IList<TransportBusinessResponse> data, ref IWorksheet worksheet)
        {
            try
            {
                //Gán lại tên file
                ReportTitle = ReportHelper.GetFileName("Báo cáo chuyến kinh doanh");
                int numberrow = 4;
                int numbercolum = 1;
                // STT
                worksheet.Range[numberrow, numbercolum].Text = MobileResource.DetailsReport_Table_Serial;
                // Điểm đi
                if (ShowStartAddress)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = "Điểm đi";
                }
                // Điểm đến
                if (ShowEndAddress)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = "Điểm đến";
                }
                // Giờ đi
                if (ShowStartTime)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = "Giờ đi";
                }
                // Giờ đến
                if (ShowEndTime)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = "Giờ đến";
                }
                // Số phút hoạt động
                if (ShowTimeActive)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = "Số phút hoạt động";
                }
                // Km GPS
                if (ShowKmGPS)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = "Km GPS";
                }
                // Km cơ
                worksheet.Range[numberrow, numbercolum].Text = "Km cơ";
                // NL tiêu thụ
                if (ShowQuotaFuelConsume)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = "NL tiêu thụ";
                }
                // Định mức NL trên 1km
                worksheet.Range[numberrow, numbercolum].Text = "Định mức NL trên 1km";
                // NL tiêu thụ định mức
                if (ShowQuotaFuel)
                {
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = "NL tiêu thụ định mức";
                }
                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.Font.Bold = true;
                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.ColorIndex = ExcelKnownColors.Sky_blue;

                //head
                worksheet.Range[1, 1].Text = "Báo cáo ra vào trạm";
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
                    // Điểm đi
                    if (ShowStartAddress)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].StartAddress.ToString();
                    }
                    // Điểm đến
                    if (ShowEndAddress)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].EndAddress.ToString();
                    }
                    // Giờ đi
                    if (ShowStartTime)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].StartTime.ToString("ss:mm:HH dd:MM:yyyy");
                    }
                    // Giờ đến
                    if (ShowEndTime)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].EndTime.ToString("ss:mm:HH dd:MM:yyyy");
                    }
                    // Số phút hoạt động
                    if (ShowTimeActive)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].TotalTime.ToString();
                    }
                    // Km GPS
                    if (ShowKmGPS)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].TotalKmGps.ToString();
                    }
                    // Km GPS
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data[i].KmOfPulseMechanical.ToString();
                    // NL tiêu thụ
                    if (ShowQuotaFuelConsume)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].TotalLitersOutsideStation.ToString();
                    }
                    // Định mức NL trên 1km
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data[i].ConstantNorms.ToString();
                    // NL tiêu thụ định mức
                    if (ShowQuotaFuel)
                    {
                        numbercolum += 1;
                        worksheet.Range[numberrow, numbercolum].Text = data[i].Norms.ToString();
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

        /// <summary>Truyền sang trang lọc chi tiết nâng cao</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  15/11/2021   created
        /// </Modified>
        private void ExecuteFilterDetails()
        {
            string message = "";
            if (string.IsNullOrEmpty(VehicleSelect.VehiclePlate))
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_NoSelectVehiclePlate, 5000);
                return;
            }
            if (!base.CheckValidateInput(ref message))
            {
                DisplayMessage.ShowMessageInfo(message, 5000);
                return;
            }
            var parameters = new NavigationParameters
            {
                { ParameterKey.Vehicle, VehicleSelect },
                { "FromDate", FromDate },
                { "ToDate", ToDate },
            };
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("DetailedFilterPage", parameters);
            });
        }
        /// <summary>Lưu các thông tin ẩn hiện cột</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  15/11/2021   created
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
                    { 1, ShowStartTime },
                    { 2, ShowTimeActive },
                    { 3, ShowEndTime },
                    { 4, ShowKmGPS },
                    { 5, ShowStartAddress },
                    { 6, ShowQuotaFuelConsume },
                    { 7, ShowEndAddress },
                    { 8, ShowQuotaFuel },
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
                            ShowStartTime = item.Value;
                            break;

                        case 2:
                            ShowTimeActive = item.Value;
                            break;

                        case 3:
                            ShowEndTime = item.Value;
                            break;

                        case 4:
                            ShowKmGPS = item.Value;
                            break;

                        case 5:
                            ShowStartAddress = item.Value;
                            break;

                        case 6:
                            ShowQuotaFuelConsume = item.Value;
                            break;

                        case 7:
                            ShowEndAddress = item.Value;
                            break;

                        case 8:
                            ShowQuotaFuel = item.Value;
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
        /// ducpv  15/11/2021   created
        /// </Modified>
        public override bool CheckValidateInput(ref string message)
        {

            if (!base.CheckValidateInput(ref message))
            {
                DisplayMessage.ShowMessageInfo(message, 5000);
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

        /// <summary>Put data xem lộ trình.</summary>
        /// <param name="obj">The object.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  28/11/2021   created
        /// </Modified>
        private void PushToViewRoute(int? obj)
        {
            SafeExecute(async () =>
            {
                if (CheckPermision((int)PermissionKeyNames.ViewModuleRoute))
                {
                    var model = ListDataSearch.Where(x => x.STT == obj).FirstOrDefault();
                    var modelparam = new Vehicle();
                    modelparam.VehiclePlate = VehicleSelect.VehiclePlate;
                    modelparam.PrivateCode = VehicleSelect.PrivateCode;
                    modelparam.VehicleId = VehicleSelect.VehicleId;
                    var p = new NavigationParameters
                    {
                        {"ReportDate", new Tuple<DateTime,DateTime>(model.TimeInStation,model.TimeOutStation) },
                        { ParameterKey.VehicleRoute, modelparam }
                    };
                    await NavigationService.NavigateAsync("RouteReportPage", p);
                }
                else
                {
                    DisplayMessage.ShowMessageInfo("Bạn không có quyền truy cập chức năng này");
                }
            });
        }

        /// <summary>Put data xem video.</summary>
        /// <param name="obj">The object.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  28/11/2021   created
        /// </Modified>
        private void PushToViewPicturn(int? obj)
        {
            SafeExecute(async () =>
            {
                if (CheckPermision(1354) || CheckPermision(1355))
                {
                    var model = ListDataSearch.Where(x => x.STT == obj).FirstOrDefault();
                }
                else
                {
                    DisplayMessage.ShowMessageInfo("Bạn không có quyền truy cập chức năng này");
                }
            });
        }

        #endregion
    }
}