using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.Service;
using BA_MobileGPS.Utilities;
using Prism.Navigation;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BA_MobileGPS.Core.ViewModels
{
    public class QCVN31SpeedReportViewModel : ReportBase<ReportQCVN31SpeedRequest, ReportQCVN31SpeedService, ReportQCVN31SpeedRespone>
    {
        public QCVN31SpeedReportViewModel(INavigationService navigationService) : base(navigationService)
        {
            IsExportExcel = CheckPermision((int)PermissionKeyNames.ReportSignalLossExport);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            FromDate = DateTime.Now.AddHours(-1);
            ToDate = DateTime.Now;
        }

        public override void OnDestroy()
        {
            base.Dispose();
        }

        // cấu hình không quá số ngày cho phép để tìm kiếm dữ liệu
        public override int AddDayMinfromDate { get; set; } = MobileSettingHelper.ViewReport;

        public bool IsExportExcel { get; set; }

        private bool isOptionData;
        public bool IsOptionData { get => isOptionData; set => SetProperty(ref isOptionData, value); }

        public override ReportQCVN31SpeedRequest SetDataInput()
        {
            return new ReportQCVN31SpeedRequest
            {
                XnCode = UserInfo.XNCode,
                VehiclePlate = VehicleSelect.VehiclePlate.ToString(),
                DateStart = base.FromDate,
                DateEnd = base.ToDate,
                OptionData = IsOptionData,
                Imei = VehicleSelect.Imei
            };
        }

        public override IList<ReportQCVN31SpeedRespone> ConvertDataBeforeDisplay(IList<ReportQCVN31SpeedRespone> data)
        {
            foreach (var item in data)
            {
                TempRowNumber += 1;
                item.OrderNumber = TempRowNumber;
            }
            return data;
        }

        /// <summary>Đổ dữ liệu vào excel</summary>
        /// <param name="data">The data.</param>
        /// <param name="worksheet">The worksheet.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/27/2019   created
        /// </Modified>
        public override void FillDataTableExcell(IList<ReportQCVN31SpeedRespone> data, ref IWorksheet worksheet)
        {
            try
            {
                //Gán lại tên file
                ReportTitle = ReportHelper.GetFileName("Tốc độ của xe");

                int numberrow = 4;
                int numbercolum = 1;
                // STT
                worksheet.Range[numberrow, numbercolum].Text = MobileResource.DetailsReport_Table_Serial;

                // Thời điểm
                numbercolum += 1;
                worksheet.Range[numberrow, numbercolum].Text = "Thời điểm";
                // các tốc độ
                numbercolum += 1;
                worksheet.Range[numberrow, numbercolum].Text = "Các tốc độ (km/h)";
              

                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.Font.Bold = true;
                worksheet.Range[numberrow, 1, numberrow, numbercolum].CellStyle.ColorIndex = ExcelKnownColors.Sky_blue;
                //head
                worksheet.Range[1, 1].Text = "Tốc độ của xe";
                worksheet.Range[1, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[1, 1].CellStyle.Font.Bold = true;
                worksheet.Range[1, 1].CellStyle.Font.Size = 16;
                worksheet.Range[1, 1, 1, numbercolum].Merge();
                worksheet.Range[2, 1].Text = MobileResource.Common_Label_PlaceHolder_FromDate + ": " + DateTimeHelper.FormatDateTime(FromDate) + " " + MobileResource.Common_Label_PlaceHolder_ToDate + ": " + DateTimeHelper.FormatDateTime(ToDate);
                worksheet.Range[2, 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[2, 1, 2, numbercolum].Merge();
                string option = MobileResource.Common_Label_Grid_VehiclePlate + ": " + VehicleSelect.PrivateCode;
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

                    // Thời điểm
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = DateTimeHelper.FormatDateTime(data[i].DT);

                    //  các tốc độ
                    numbercolum += 1;
                    worksheet.Range[numberrow, numbercolum].Text = data[i].Velocities;
                }

                worksheet.Range[4, 1, numberrow, numbercolum].BorderAround();
                worksheet.Range[4, 1, numberrow, numbercolum].BorderInside(ExcelLineStyle.Thin, ExcelKnownColors.Black);
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
            // check thời gian có vượt quá khoảng cho phép hay không
            if ((ToDate - FromDate).TotalDays > 2)
            {
                message = "Hệ thống hỗ trợ lấy dữ liệu trong khoảng 2 ngày liên tiếp";
                return false;
            }
            return true;
        }
    }
}