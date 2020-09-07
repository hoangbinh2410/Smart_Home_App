using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        #region báo cáo nhiệt độ

        public static string ReportTemperature_Label_TitlePage => Get(MobileResourceNames.ReportTemperature_Label_TitlePage, "Báo cáo nhiệt độ", "Temperature Report");

        public static string ReportTemperature_Label_PlaceHolder_Temperature => Get(MobileResourceNames.ReportTemperature_Label_Temperature, "Nhiệt độ >=", "Temperature >=");

        public static string ReportTemperature_Label_Grid_Kilomet => Get(MobileResourceNames.ReportTemperature_Label_Grid_Kilomet, "KM", "KM");

        public static string ReportTemperature_Label_Grid_DateTime => Get(MobileResourceNames.ReportTemperature_Label_Grid_DateTime, "Ngày tháng", "Date Time");
        public static string ReportTemperature_Label_Grid_Temperature => Get(MobileResourceNames.ReportTemperature_Label_Grid_Temperature, "Nhiệt độ", "Temperature");

        public static string ReportTemperature_Label_Grid_NumberOfSensors => Get(MobileResourceNames.ReportTemperature_Label_Grid_NumberOfSensors, "Cảm biến nhiệt độ", "Number Of Sensors");

        public static string ReportTemperature_Label_Grid_StartAddress => Get(MobileResourceNames.ReportTemperature_Label_Grid_StartAddress, "Địa chỉ đi", "Start Address");

        public static string ReportTemperature_Label_Grid_EndAddress => Get(MobileResourceNames.ReportTemperature_Label_Grid_EndAddress, "Địa chỉ đến", "End Address");
        public static string ReportTemperature_Label_Grid_Description => Get(MobileResourceNames.ReportTemperature_Label_Grid_Description, "Ghi chú", "Notes");

        public static string ReportTemperature_Label_Grid_Router => Get(MobileResourceNames.ReportTemperature_Label_Grid_Router, "Lộ trình", "Router");

        public static string ReportTemperature_Message_ValidateError_Temperation => Get(MobileResourceNames.ReportTemperature_Message_ValidateError_Temperation, "Nhiệt độ không đúng định dạng", "Router");

        public static string ReportTemperature_Label_DetailVehiclePlate => Get(MobileResourceNames.ReportTemperature_Label_DetailVehiclePlate, "Biển số xe: ", "Vehicle Plate: ");

        public static string ReportTemperature_Label_DetailFromDate => Get(MobileResourceNames.ReportTemperature_Label_DetailFromDate, "Từ: ", "From: ");
        public static string ReportTemperature_Label_DetailKM => Get(MobileResourceNames.ReportTemperature_Label_DetailKM, "KM: ", "Kilomet: ");
        public static string ReportTemperature_Label_DetailTemperature => Get(MobileResourceNames.ReportTemperature_Label_DetailTemperature, "Nhiệt độ: ", "Temperature: ");
        public static string ReportTemperature_Label_DetailStartAddress => Get(MobileResourceNames.ReportTemperature_Label_DetailStartAddress, "Địa chỉ đi: ", "StartAddress: ");
        public static string ReportTemperature_Label_DetailEndAddress => Get(MobileResourceNames.ReportTemperature_Label_DetailEndAddress, "Địa chỉ đến: ", "EndAddress: ");
        public static string ReportTemperature_Label_DetailDescription => Get(MobileResourceNames.ReportTemperature_Label_DetailDescription, "Ghi chú: ", "Description: ");

        public static string ReportTemperature_Label_TitlePageDetail => Get(MobileResourceNames.ReportTemperature_Label_TitlePageDetail, "Thông tin chi tiết", "Detail");

        #endregion báo cáo nhiệt độ

        #region báo cáo động cơ

        public static string ReportMachine_Label_Grid_DateTime => Get(MobileResourceNames.ReportMachine_Label_Grid_DateTime, "Ngày tháng", "Date Time");

        public static string ReportMachine_Label_TitlePage => Get(MobileResourceNames.ReportMachine_Label_TitlePage, "Báo cáo động cơ", "Report machine");
        public static string ReportMachine_Combobox_LabelAll_StatusMachine => Get(MobileResourceNames.ReportTemperature_Combobox_LabelAll_StatusMachine, "Tất cả", "All");

        public static string ReportMachine_TitleStatusMachine => Get(MobileResourceNames.ReportMachine_TitleStatusMachine, "Chọn trạng thái động cơ", "Search Status Machine");
        public static string ReportMachine_Label_PlaceHolder_CountMinutes => Get(MobileResourceNames.ReportMachine_Label_PlaceHolder_CountMinutes, "Số phút >=", "Minutes >=");
        public static string ReportMachine_Label_PlaceHolder_StatusMachine => Get(MobileResourceNames.ReportMachine_Label_PlaceHolder_StatusMachine, "Trạng thái", "Status Machine");

        public static string ReportMachine_Label_Grid_Minutes => Get(MobileResourceNames.ReportMachine_Label_Grid_Minutes, "Số phút", "Minutes");
        public static string ReportMachine_Label_Grid_DateTimeEnd => Get(MobileResourceNames.ReportMachine_Label_Grid_DateTimeEnd, "Giờ đến", "Date Time End");
        public static string ReportMachine_Label_Grid_Fuel => Get(MobileResourceNames.ReportMachine_Label_Grid_Fuel, "Nhiên liệu", "Fuel");

        public static string ReportMachine_Label_Grid_StartAddress => Get(MobileResourceNames.ReportMachine_Label_Grid_StartAddress, "Địa điểm bắt đầu", "Start Address");

        public static string ReportMachine_Label_Grid_EndAddress => Get(MobileResourceNames.ReportMachine_Label_Grid_EndAddress, "Địa điểm kết thúc", "End Address");

        public static string ReportMachine_Label_DetailVehiclePlate => Get(MobileResourceNames.ReportMachine_Label_DetailVehiclePlate, "Biển số xe: ", "VehiclePlate: ");
        public static string ReportMachine_Label_DetailFromDate => Get(MobileResourceNames.ReportMachine_Label_DetailFromDate, "Từ: ", "From: ");
        public static string ReportMachine_Label_DetailMinutes => Get(MobileResourceNames.ReportMachine_Label_DetailMinutes, "Số phút: ", "Minutes: ");
        public static string ReportMachine_Label_DetailFuel => Get(MobileResourceNames.ReportMachine_Label_DetailFuel, "Nhiên liệu: ", "Fuel: ");
        public static string ReportMachine_Label_DetailStartAddress => Get(MobileResourceNames.ReportMachine_Label_DetailStartAddress, "Địa chỉ đi: ", "StartAddress: ");
        public static string ReportMachine_Label_DetailEndAddress => Get(MobileResourceNames.ReportMachine_Label_DetailEndAddress, "Địa chỉ đến: ", "EndAddress: ");
        public static string ReportMachine_Message_ValidateError_Minutes => Get(MobileResourceNames.ReportMachine_Message_ValidateError_Minutes, "Số phút không đúng định dạng", "You don't input number of minutes");

        public static string ReportMachine_Label_TitlePageDetail => Get(MobileResourceNames.ReportMachine_Label_TitlePageDetail, "Thông tin chi tiết", "Detail");

        public static string ReportMachine_Label_EngineTurnOn => Get(MobileResourceNames.ReportMachine_Label_EngineTurnOn, "Bật động cơ", "Engine On");

        public static string ReportMachine_Label_EngineTurnOff => Get(MobileResourceNames.ReportMachine_Label_EngineTurnOff, "Tắt động cơ", "Engine OFF");

        #endregion báo cáo động cơ

        #region báo cáo đổ hút nhiên liệu

        public static string PourFuelReport_Label_TilePage => Get(MobileResourceNames.PourFuelReport_Label_TitlePage, "Báo cáo đổ hút nhiên liệu", "Pour fuel report");
        public static string PourFuelReport_Label_TileDetailPage => Get(MobileResourceNames.PourFuelReport_Label_TitleDetailPage, "Chi tiết đổ hút nhiên liệu", "Pour fuel report detail");
        public static string PourFuelReport_Label_TileChartDetailPage => Get(MobileResourceNames.PourFuelReport_Label_TileChartDetailPage, "Biểu đồ nhiên liệu", "Fuel chart");
        public static string PourFuelReport_Label_PlaceHolder_StatusPourFuel => Get(MobileResourceNames.PourFuelReport_Label_StatusPourFuel, "Trạng thái", "Status pour fuel");

        public static string PourFuelReport_Label_PlaceHolder_Liters => Get(MobileResourceNames.PourFuelReport_Label_PlaceHolder_Liters, "Số Lít >=", "Liters >=");

        public static string PourFuelReport_CheckBox_Number => Get(MobileResourceNames.PourFuelReport_Label_CheckBox_Number, "STT", "Serial");

        public static string PourFuelReport_CheckBox_Liters => Get(MobileResourceNames.PourFuelReport_Label_CheckBox_Liters, "Số lít", "Liters");

        public static string PourFuelReport_CheckBox_Date => Get(MobileResourceNames.PourFuelReport_Label_CheckBox_Date, "Thời gian", "Date");

        public static string PourFuelReport_CheckBox_Address => Get(MobileResourceNames.PourFuelReport_Label_CheckBox_Address, "Địa chỉ", "Address");

        public static string PourFuelReport_CheckBox_Status => Get(MobileResourceNames.PourFuelReport_Label_Detail_Status, "Trạng thái", "Status");

        public static string PourFuelReport_Detail_Liters => Get(MobileResourceNames.PourFuelReport_Label_Detail_Liters, "Số lít: ", "Liters");

        public static string PourFuelReport_Detail_Date => Get(MobileResourceNames.PourFuelReport_Label_Detail_Date, "Thời gian: ", "Date");

        public static string PourFuelReport_Detail__Address => Get(MobileResourceNames.PourFuelReport_Label_Detail_Address, "Địa chỉ:", "Address");

        public static string PourFuelReport_Detail__Status => Get(MobileResourceNames.PourFuelReport_Label_Detail_Status, "Trạng thái:", "Status");

        public static string PourFuelReport_Detail_Label_Liters => Get(MobileResourceNames.PourFuelReport_Detail_Label_Liters, "lít", "Liter");
        public static string PourFuelReport_ComboBox_Status_Pour => Get(MobileResourceNames.PourFuelReport_ComboBox_Status_Pour, "Đổ", "Pour");

        public static string PourFuelReport_ComboBox_Status_Absorb => Get(MobileResourceNames.PourFuelReport_ComboBox_Status_Absorb, "Hút", "Absorb");

        public static string PourFuelReport_ComboBox_Status_SuspiciousPour => Get(MobileResourceNames.PourFuelReport_ComboBox_Status_SuspiciousPour, "Nghi ngờ đổ", "SuspiciousPour");

        public static string PourFuelReport_ComboBox_Status_SuspiciousAbsorb => Get(MobileResourceNames.PourFuelReport_ComboBox_Status_SuspiciousAbsorb, "Nghi ngờ hút", "SuspiciousAbsorb");

        public static string PourFuelReport_Table_Serial => Get(MobileResourceNames.PourFuelReport_Table_Serial, "STT", "Serial");

        public static string PourFuelReport_Table_Date => Get(MobileResourceNames.PourFuelReport_Table_Date, "Thời gian", "Date");

        public static string PourFuelReport_Table_Status => Get(MobileResourceNames.PourFuelReport_Table_Status, "Trạng thái", "Status");

        public static string PourFuelReport_Table_Liters => Get(MobileResourceNames.PourFuelReport_Table_Liters, "Số lít", "Liters");

        public static string PourFuelReport_Table_Address => Get(MobileResourceNames.PourFuelReport_Table_Address, "Địa chỉ", "Address");
        public static string PourFuelReport_Message_ValidateError_Liters => Get(MobileResourceNames.PourFuelReport_Message_ValidateError_Liters, "Số lít không đúng định dạng", "You don't input number of minutes");

        public static string PourFuelReport_Status_ValidateError_Pour => Get(MobileResourceNames.PourFuelReport_Status_ValidateError_Pour, "Đổ", "Pour");

        public static string PourFuelReport_Status_ValidateError_Absorb => Get(MobileResourceNames.PourFuelReport_Status_ValidateError_Absorb, "Hút", "Absorb");

        public static string PourFuelReport_Status_ValidateError_SuspiciousPour => Get(MobileResourceNames.PourFuelReport_Status_ValidateError_SuspiciousPour, "Khả nghi đổ", "SuspiciousPour");

        public static string PourFuelReport_Status_ValidateError_SuspiciousAbsorb => Get(MobileResourceNames.PourFuelReport_Status_ValidateError_SuspiciousAbsorb, "Khả nghi hút", "SuspiciousAbsorb");

        #endregion báo cáo đổ hút nhiên liệu

        #region Biểu đồ đổ hút nhiên liệu

        public static string ChartFuelReport_Y_Liters => Get(MobileResourceNames.ChartFuelReport_Y_Liters, "Số lít", "Liters");

        public static string ChartFuelReport_Tank => Get(MobileResourceNames.ChartFuelReport_Tank, "Bình", "Tank");

        public static string ChartFuelReport_Total => Get(MobileResourceNames.ChartFuelReport_Total, "Tổng", "Total");

        public static string ChartFuelReport_X_Date => Get(MobileResourceNames.ChartFuelReport_X_Date, "Thời gian", "Time");

        public static string ChartFuelReport_ShowByTime => Get(MobileResourceNames.ChartFuelReport_ShowByTime, "Theo thời gian", "Show by time");

        #endregion Biểu đồ đổ hút nhiên liệu

        #region Báo cáo tiêu hao nhiên liệu

        public static string FuelsSummariesReport_Label_TilePage => Get(MobileResourceNames.FuelsSummariesReport_Label_TilePage, "Báo cáo tiêu hao nhiên liệu", "Fuels Summaries Report");

        public static string FuelsSummariesReportTotal_Label_TilePage => Get(MobileResourceNames.FuelsSummariesReportTotal_Label_TilePage, "Báo cáo tổng hợp tiêu hao nhiên liệu", "Fuels Summaries Report");

        public static string FuelsSummariesReport_Label_TitlePageDetail => Get(MobileResourceNames.FuelsSummariesReport_Label_TitlePageDetail, "Thông tin chi tiết", "Detail");

        public static string FuelsSummariesReport_Label_DetailVehiclePlate => Get(MobileResourceNames.FuelsSummariesReport_Label_DetailVehiclePlate, "Biển số xe", "VehiclePlate");

        public static string FuelsSummariesReport_Label_Date => Get(MobileResourceNames.FuelsSummariesReport_Label_Date, "Ngày tháng", "Date");

        public static string FuelsSummariesReport_Label_StartTime => Get(MobileResourceNames.FuelsSummariesReport_Label_StartTime, "Giờ bắt đầu di chuyển", "Start Time");

        public static string FuelsSummariesReport_Label_EndTime => Get(MobileResourceNames.FuelsSummariesReport_Label_EndTime, "Giờ kết thúc di chuyển", "End Time");

        public static string FuelsSummariesReport_Label_StartTime2 => Get(MobileResourceNames.FuelsSummariesReport_Label_StartTime2, "Từ ngày", "Start Time");

        public static string FuelsSummariesReport_Label_EndTime2 => Get(MobileResourceNames.FuelsSummariesReport_Label_EndTime2, "Đến ngày", "End Time");

        public static string FuelsSummariesReport_Label_NumberOfPour => Get(MobileResourceNames.FuelsSummariesReport_Label_NumberOfPour, "Số lần đổ", "Number Of Pour");

        public static string FuelsSummariesReport_Label_NumberOfSuction => Get(MobileResourceNames.FuelsSummariesReport_Label_NumberOfSuction, "Số lần hút", "Number Of Suction");

        public static string FuelsSummariesReport_Label_FirtLits => Get(MobileResourceNames.FuelsSummariesReport_Label_FirtLits, "Số lít đầu ngày", "Firt Lits");

        public static string FuelsSummariesReport_Label_LitersOfPour => Get(MobileResourceNames.FuelsSummariesReport_Label_LitersOfPour, "Số lít đổ", "Liters Of Pour");

        public static string FuelsSummariesReport_Label_LitersOfSuction => Get(MobileResourceNames.FuelsSummariesReport_Label_LitersOfSuction, "Số lít hút", "Liters Of Suction");

        public static string FuelsSummariesReport_Label_LastLits => Get(MobileResourceNames.FuelsSummariesReport_Label_LastLits, "Số lít tồn", "Last Lits");

        public static string FuelsSummariesReport_Label_LitsConsumable => Get(MobileResourceNames.FuelsSummariesReport_Label_LitsConsumable, "Số lít tiêu hao", "Lits Consumable");

        public static string FuelsSummariesReport_Label_MinutesOfManchineOn => Get(MobileResourceNames.FuelsSummariesReport_Label_MinutesOfManchineOn, "Thời gian nổ máy (phút):", "Minutes Of Manchine On");

        public static string FuelsSummariesReport_Label_MinuteOfMachineOnStop => Get(MobileResourceNames.FuelsSummariesReport_Label_MinuteOfMachineOnStop, "Thời gian dừng đỗ nổ máy (phút):", "Minutes Of Machine On Stop");

        public static string FuelsSummariesReport_Label_TotalKmGps => Get(MobileResourceNames.FuelsSummariesReport_Label_TotalKmGps, "Km (GPS)", "Total Km Gps");

        public static string FuelsSummariesReport_Label_QuotaRegulations => Get(MobileResourceNames.FuelsSummariesReport_Label_QuotaRegulations, "Định mức quy định", "Quota Regulations");

        public static string FuelsSummariesReport_Label_QuotaReality => Get(MobileResourceNames.FuelsSummariesReport_Label_QuotaReality, "Định mức thực tế", "Quota Reality");

        public static string FuelsSummariesReport_Label_PopupStart => Get(MobileResourceNames.FuelsSummariesReport_Label_PopupStart, "Bắt đầu: Giờ xe bắt đầu di chuyển trong ngày", "Start: The time the vehicle begins to move during the day");

        public static string FuelsSummariesReportTotal_Label_PopupStart => Get(MobileResourceNames.FuelsSummariesReportTotal_Label_PopupStart, "Bắt đầu: Giờ bắt đầu xe di chuyển 'từ ngày'", "Start: The bus start time 'from day'");

        public static string FuelsSummariesReport_Label_PopupEnd => Get(MobileResourceNames.FuelsSummariesReport_Label_PopupEnd, "Kết thúc: Giờ xe di chuyển cuôi cùng trong ngày", "End: Time for the vehicle to move at the end of the day");

        public static string FuelsSummariesReportTotal_Label_PopupEnd => Get(MobileResourceNames.FuelsSummariesReportTotal_Label_PopupEnd, "Kết thúc: Giờ xe di chuyển cuối cùng 'đến ngày'", "End: The last moving hour 'to the day'");

        public static string FuelsSummariesReport_Label_PopupLastLits => Get(MobileResourceNames.FuelsSummariesReport_Label_PopupLastLits, "Số lít tồn: Số lít trong bình tại thời điểm cuối ngày", "Number of liters in stock: The number of liters in the tank at the end of the day");

        public static string FuelsSummariesReportTotal_Label_PopupLastLits => Get(MobileResourceNames.FuelsSummariesReportTotal_Label_PopupLastLits, "Số lít tồn: Số lít trong bình tại thời điểm cuối ngày 'đến ngày'", "Number of liters in stock: The number of liters in the tank at the end of the day 'to date'");

        public static string FuelsSummariesReport_Label_PopupLitsConsumable => Get(MobileResourceNames.FuelsSummariesReport_Label_PopupLitsConsumable, "Số lít tiêu hao = Số lít đầu ngày + Số lít đổ - Số lít hút - Số lít tồn", "Lits Consumable = Firt Lits + Liters Of Pour - Liters Of Suction - Last Lits");

        public static string FuelsSummariesReportTotal_Label_PopupLitsConsumable => Get(MobileResourceNames.FuelsSummariesReportTotal_Label_PopupLitsConsumable, "Số lít tiêu hao: Tổng số lit tiêu hao trong khoảng thời gian tìm kiếm", "Number of liters consumed: The total number of liters consumed in the search period");

        public static string FuelsSummariesReport_Table_Serial => Get(MobileResourceNames.FuelsSummariesReport_Table_Serial, "STT", "Serial");

        public static string FuelsSummariesReport_Table_Date => Get(MobileResourceNames.FuelsSummariesReport_Table_Date, "Ngày", "Date");

        public static string FuelsSummariesReport_Table_StartEndTime => Get(MobileResourceNames.FuelsSummariesReport_Table_StartEndTime, "Bắt đầu Kết thúc", "Start Time End Time");

        public static string FuelsSummariesReport_Table_StartEndTime2 => Get(MobileResourceNames.FuelsSummariesReport_Table_StartEndTime2, "Từ ngày Đến ngày", "Start Time End Time");

        public static string FuelsSummariesReport_Table_NumberOfPour => Get(MobileResourceNames.FuelsSummariesReport_Table_NumberOfPour, "Số lần đổ", "Number Of Pour");

        public static string FuelsSummariesReport_Table_NumberOfSuction => Get(MobileResourceNames.FuelsSummariesReport_Table_NumberOfSuction, "Số lần hút", "Number Of Suction");

        public static string FuelsSummariesReport_Table_FirtLits => Get(MobileResourceNames.FuelsSummariesReport_Table_FirtLits, "Số lít đầu ngày", "Firt Lits");

        public static string FuelsSummariesReport_Table_LitersOfPour => Get(MobileResourceNames.FuelsSummariesReport_Table_LitersOfPour, "Số lít đổ", "Liters Of Pour");

        public static string FuelsSummariesReport_Table_LitersOfSuction => Get(MobileResourceNames.FuelsSummariesReport_Table_LitersOfSuction, "Số lít hút", "Liters Of Suction");

        public static string FuelsSummariesReport_Table_LastLits => Get(MobileResourceNames.FuelsSummariesReport_Table_LastLits, "Số lít tồn", "Last Lits");

        public static string FuelsSummariesReport_Table_LitsConsumable => Get(MobileResourceNames.FuelsSummariesReport_Table_LitsConsumable, "Số lít tiêu hao", "Lits Consumable");

        public static string FuelsSummariesReport_Table_TotalKmGps => Get(MobileResourceNames.FuelsSummariesReport_Table_TotalKmGps, "Km (GPS)", "Total Km Gps");

        public static string FuelsSummariesReport_Table_MinutesOfManchineOn => Get(MobileResourceNames.FuelsSummariesReport_Table_MinutesOfManchineOn, "Thời gian nổ máy (phút)", "Minutes Of Manchine On");

        public static string FuelsSummariesReport_Table_MinuteOfMachineOnStop => Get(MobileResourceNames.FuelsSummariesReport_Table_MinuteOfMachineOnStop, "Thời gian dừng đỗ nổ máy (phút)", "Minutes Of Machine On Stop");

        public static string FuelsSummariesReport_Table_QuotaRegulations => Get(MobileResourceNames.FuelsSummariesReport_Table_QuotaRegulations, "Định mức quy định", "Quota Regulations");

        public static string FuelsSummariesReport_Table_QuotaReality => Get(MobileResourceNames.FuelsSummariesReport_Table_QuotaReality, "Định mức thực tế", "Quota Reality");

        public static string FuelsSummariesReport_CheckBox_Serial => Get(MobileResourceNames.FuelsSummariesReport_CheckBox_Serial, "STT", "Serial");

        public static string FuelsSummariesReport_CheckBox_Date => Get(MobileResourceNames.FuelsSummariesReport_CheckBox_Date, "Ngày tháng", "Date");

        public static string FuelsSummariesReport_CheckBox_StartEndTime => Get(MobileResourceNames.FuelsSummariesReport_CheckBox_StartEndTime, "Bắt đầu/Kết thúc", "Start Time/End Time");

        public static string FuelsSummariesReport_CheckBox_NumberOfPour => Get(MobileResourceNames.FuelsSummariesReport_CheckBox_NumberOfPour, "Số lần đổ", "Number Of Pour");

        public static string FuelsSummariesReport_CheckBox_NumberOfSuction => Get(MobileResourceNames.FuelsSummariesReport_CheckBox_NumberOfSuction, "Số lần hút", "Number Of Suction");

        public static string FuelsSummariesReport_CheckBox_FirtLits => Get(MobileResourceNames.FuelsSummariesReport_CheckBox_FirtLits, "Số lít đầu ngày", "Firt Lits");

        public static string FuelsSummariesReport_CheckBox_LitersOfPour => Get(MobileResourceNames.FuelsSummariesReport_CheckBox_LitersOfPour, "Số lít đổ", "Liters Of Pour");

        public static string FuelsSummariesReport_CheckBox_LitersOfSuction => Get(MobileResourceNames.FuelsSummariesReport_CheckBox_LitersOfSuction, "Số lít hút", "Liters Of Suction");

        public static string FuelsSummariesReport_CheckBox_LastLits => Get(MobileResourceNames.FuelsSummariesReport_CheckBox_LastLits, "Số lít tồn", "Last Lits");

        public static string FuelsSummariesReport_CheckBox_LitsConsumable => Get(MobileResourceNames.FuelsSummariesReport_CheckBox_LitsConsumable, "Số lít tiêu hao", "Lits Consumable");

        public static string FuelsSummariesReport_CheckBox_TotalKmGps => Get(MobileResourceNames.FuelsSummariesReport_CheckBox_TotalKmGps, "Km Gps", "Total Km Gps");

        public static string FuelsSummariesReport_CheckBox_MinutesOfManchineOn => Get(MobileResourceNames.FuelsSummariesReport_CheckBox_MinutesOfManchineOn, "Thời gian nổ máy (phút)", "Minutes Of Manchine On");

        public static string FuelsSummariesReport_CheckBox_MinuteOfMachineOnStop => Get(MobileResourceNames.FuelsSummariesReport_CheckBox_MinuteOfMachineOnStop, "Dừng đỗ nổ máy (phút)", "Minutes Of Machine On Stop");

        public static string FuelsSummariesReport_CheckBox_QuotaRegulations => Get(MobileResourceNames.FuelsSummariesReport_CheckBox_QuotaRegulations, "Định mức quy định", "Quota Regulations");

        public static string FuelsSummariesReport_CheckBox_QuotaReality => Get(MobileResourceNames.FuelsSummariesReport_CheckBox_QuotaReality, "Định mức thực tế", "Quota Reality");

        #endregion Báo cáo tiêu hao nhiên liệu

        #region Báo cáo tổng hợp

        public static string ActivitySummariesReport_Label_TilePage => Get(MobileResourceNames.ActivitySummariesReport_Label_TilePage, "Báo cáo tổng hợp hoạt động", "Activity Summaries report");

        public static string ActivitySummariesReport_Label_TitlePageDetail => Get(MobileResourceNames.ActivitySummariesReport_Label_TitlePageDetail, "Thông tin chi tiết", "Detail");

        public static string ActivitySummariesReport_Label_TitleJoinNameDate => Get(MobileResourceNames.ActivitySummariesReport_Label_TitleJoinNameDate, "Ngày hoạt động thực tế", "Days actual operation");

        public static string ActivitySummariesReport_Label_VehicleType => Get(MobileResourceNames.ActivitySummariesReport_Label_VehicleType, "Loại xe", "Vehicle Type");

        public static string ActivitySummariesReport_Label_JoinDay => Get(MobileResourceNames.ActivitySummariesReport_Label_JoinDay, "Gộp ngày", "Join Day");

        public static string ActivitySummariesReport_Label_VehiclePlate => Get(MobileResourceNames.ActivitySummariesReport_Label_VehiclePlate, "Biển số xe", "Vehicle Plate");

        public static string ActivitySummariesReport_Label_CurrentTime => Get(MobileResourceNames.ActivitySummariesReport_Label_CurrentTime, "Ngày tháng", "Date");

        public static string ActivitySummariesReport_Label_StartTime => Get(MobileResourceNames.ActivitySummariesReport_Label_StartTime, "Giờ đi", "Start Time");

        public static string ActivitySummariesReport_Label_EndTime => Get(MobileResourceNames.ActivitySummariesReport_Label_EndTime, "Giờ đến", "End Time");

        public static string ActivitySummariesReport_Label_ActiveTime => Get(MobileResourceNames.ActivitySummariesReport_Label_ActiveTime, "Thời gian lăn bánh", "Active Time");

        public static string ActivitySummariesReport_Label_StopParkingTime => Get(MobileResourceNames.ActivitySummariesReport_Label_StopParkingTime, "Thời gian dừng đỗ", "Stop Parking Time");

        public static string ActivitySummariesReport_Label_KmGPS => Get(MobileResourceNames.ActivitySummariesReport_Label_KmGPS, "Km (GPS)", "Km (GPS)");

        public static string ActivitySummariesReport_Label_KmCO => Get(MobileResourceNames.ActivitySummariesReport_Label_KmCO, "Km cơ", "KmCO");

        public static string ActivitySummariesReport_Label_NumberOfStopParking => Get(MobileResourceNames.ActivitySummariesReport_Label_NumberOfStopParking, "Số lần dừng đỗ", "Number Of Stop Parking");

        public static string ActivitySummariesReport_Label_MinutesTurnOnAirConditioner => Get(MobileResourceNames.ActivitySummariesReport_Label_MinutesTurnOnAirConditioner, "Bật điều hoà (phút)", "Minutes Turn On Air Conditioner");

        public static string ActivitySummariesReport_Label_Vmax => Get(MobileResourceNames.ActivitySummariesReport_Label_Vmax, "Vận tốc cực đại", "Vmax");

        public static string ActivitySummariesReport_Label_Vmedium => Get(MobileResourceNames.ActivitySummariesReport_Label_Vmedium, "Vận tốc trung bình", "Vmedium");

        public static string ActivitySummariesReport_Table_Serial => Get(MobileResourceNames.ActivitySummariesReport_Table_Serial, "STT", "Serial");

        public static string ActivitySummariesReport_Table_CurrentTime => Get(MobileResourceNames.ActivitySummariesReport_Table_CurrentTime, "Ngày tháng", "Date");

        public static string ActivitySummariesReport_Table_StartEndTime => Get(MobileResourceNames.ActivitySummariesReport_Table_StartEndTime, "Giờ đi - Giờ đến", "Start Time - End Time");

        public static string ActivitySummariesReport_Table_ActiveTime => Get(MobileResourceNames.ActivitySummariesReport_Table_ActiveTime, "Thời gian lăn bánh", "Active Time");

        public static string ActivitySummariesReport_Table_StopParkingTime => Get(MobileResourceNames.ActivitySummariesReport_Table_StopParkingTime, "Thời gian dừng đỗ", "Stop Parking Time");

        public static string ActivitySummariesReport_Table_KmGPS => Get(MobileResourceNames.ActivitySummariesReport_Table_KmGPS, "Km (GPS)", "Km (GPS)");

        public static string ActivitySummariesReport_Table_KmCO => Get(MobileResourceNames.ActivitySummariesReport_Table_KmCO, "Km cơ", "KmCO");

        public static string ActivitySummariesReport_Table_VehicleType => Get(MobileResourceNames.ActivitySummariesReport_Table_VehicleType, "Loại xe", "Vehicle Type");

        public static string ActivitySummariesReport_Table_VehiclePlate => Get(MobileResourceNames.ActivitySummariesReport_Table_VehiclePlate, "Biển số xe", "Vehicle Plate");

        public static string ActivitySummariesReport_Table_NumberOfStopParking => Get(MobileResourceNames.ActivitySummariesReport_Table_NumberOfStopParking, "Số lần dừng đỗ", "Number Of Stop Parking");

        public static string ActivitySummariesReport_Table_MinutesTurnOnAirConditioner => Get(MobileResourceNames.ActivitySummariesReport_Table_MinutesTurnOnAirConditioner, "Bật điều hoà (phút)", "Minutes Turn On Air Conditioner");

        public static string ActivitySummariesReport_Table_Vmax => Get(MobileResourceNames.ActivitySummariesReport_Table_Vmax, "Vận tốc cực đại", "Vmax");

        public static string ActivitySummariesReport_Table_Vmedium => Get(MobileResourceNames.ActivitySummariesReport_Table_Vmedium, "Vận tốc trung bình", "Vmedium");

        public static string ActivitySummariesReport_CheckBox_VehicleType => Get(MobileResourceNames.ActivitySummariesReport_CheckBox_VehicleType, "Loại xe", "Vehicle Type");

        public static string ActivitySummariesReport_CheckBox_VehiclePlate => Get(MobileResourceNames.ActivitySummariesReport_CheckBox_VehiclePlate, "Biển số xe", "Vehicle Plate");

        public static string ActivitySummariesReport_CheckBox_Vmax => Get(MobileResourceNames.ActivitySummariesReport_CheckBox_Vmax, "Vận tốc cực đại", "Vmax");

        public static string ActivitySummariesReport_CheckBox_CurrentTime => Get(MobileResourceNames.ActivitySummariesReport_CheckBox_CurrentTime, "Ngày tháng", "Date");

        public static string ActivitySummariesReport_CheckBox_ActiveTime => Get(MobileResourceNames.ActivitySummariesReport_CheckBox_ActiveTime, "Thời gian lăn bánh", "Active Time");

        public static string ActivitySummariesReport_CheckBox_StopParkingTime => Get(MobileResourceNames.ActivitySummariesReport_CheckBox_StopParkingTime, "Thời gian dừng đỗ", "Stop Parking Time");

        public static string ActivitySummariesReport_CheckBox_KmGPS => Get(MobileResourceNames.ActivitySummariesReport_CheckBox_KmGPS, "Km (GPS)", "Km (GPS)");

        public static string ActivitySummariesReport_CheckBox_KmCO => Get(MobileResourceNames.ActivitySummariesReport_CheckBox_KmCO, "Km cơ", "KmCO");

        public static string ActivitySummariesReport_CheckBox_NumberOfStopParking => Get(MobileResourceNames.ActivitySummariesReport_CheckBox_NumberOfStopParking, "Số lần dừng đỗ", "Number Of Stop Parking");

        public static string ActivitySummariesReport_CheckBox_MinutesTurnOnAirConditioner => Get(MobileResourceNames.ActivitySummariesReport_CheckBox_MinutesTurnOnAirConditioner, "Bật điều hoà", "Minutes Turn On Air Conditioner");

        public static string ActivitySummariesReport_CheckBox_Vmedium => Get(MobileResourceNames.ActivitySummariesReport_CheckBox_Vmedium, "Vận tốc trung bình", "Vmedium");

        #endregion Báo cáo tổng hợp

        #region Báo cáo chi tiết

        public static string DetailsReport_Label_TilePage => Get(MobileResourceNames.DetailsReport_Label_TilePage, "Báo cáo chi tiết hoạt động", "Detail report");

        public static string DetailsReport_Label_TitlePageDetail => Get(MobileResourceNames.DetailsReport_Label_TitlePageDetail, "Thông tin chi tiết", "Detail");

        public static string DetailsReport_Label_VehicleType => Get(MobileResourceNames.DetailsReport_Label_VehicleType, "Loại xe", "Vehicle Type");

        public static string DetailsReport_Label_VehiclePlate => Get(MobileResourceNames.DetailsReport_Label_VehiclePlate, "Biển số xe", "Vehicle Plate");

        public static string DetailsReport_Label_CurrentTime => Get(MobileResourceNames.DetailsReport_Label_CurrentTime, "Ngày tháng", "Date");

        public static string DetailsReport_Label_StartTime => Get(MobileResourceNames.DetailsReport_Label_StartTime, "Giờ đi", "Start Time");

        public static string DetailsReport_Label_EndTime => Get(MobileResourceNames.DetailsReport_Label_EndTime, "Giờ đến", "End Time");

        public static string DetailsReport_Label_TimeActive => Get(MobileResourceNames.DetailsReport_Label_TimeActive, "Thời gian hoạt động", "Time Active");

        public static string DetailsReport_Label_KmGPS => Get(MobileResourceNames.DetailsReport_Label_KmGPS, "Km (GPS)", "Km (GPS)");

        public static string DetailsReport_Label_KmCO => Get(MobileResourceNames.DetailsReport_Label_KmCO, "Km cơ", "KmCO");

        public static string DetailsReport_Label_QuotaFuel => Get(MobileResourceNames.DetailsReport_Label_QuotaFuel, "Định mức nhiên liệu / km", "Quota Fuel / km");

        public static string DetailsReport_Label_QuotaFuelConsume => Get(MobileResourceNames.DetailsReport_Label_QuotaFuelConsume, "Nhiên liệu tiêu thụ định mức", "Quota Fuel Consume");

        public static string DetailsReport_Label_StartAddress => Get(MobileResourceNames.DetailsReport_Label_StartAddress, "Địa chỉ đi", "StartAddress");

        public static string DetailsReport_Label_EndAddress => Get(MobileResourceNames.DetailsReport_Label_EndAddress, "Địa chỉ đến", "End Address");

        public static string DetailsReport_Label_TripCompensatory => Get(MobileResourceNames.DetailsReport_Label_TripCompensatory, "Cuốc bù", "Trip Compensatory");

        public static string DetailsReport_Table_Serial => Get(MobileResourceNames.DetailsReport_Table_Serial, "STT", "Serial");

        public static string DetailsReport_Table_CurrentTime => Get(MobileResourceNames.DetailsReport_Table_CurrentTime, "Ngày tháng", "Date");
        public static string DetailsReport_Table_StartEndTime => Get(MobileResourceNames.DetailsReport_Table_StartEndTime, "Giờ đi Giờ đến", "Start Time End Time");

        public static string DetailsReport_Table_TimeActive => Get(MobileResourceNames.DetailsReport_Table_TimeActive, "Thời gian hoạt động", "Time Active");

        public static string DetailsReport_Table_KmGPS => Get(MobileResourceNames.DetailsReport_Table_KmGPS, "Km (GPS)", "Km (GPS)");

        public static string DetailsReport_Table_KmCO => Get(MobileResourceNames.DetailsReport_Table_KmCO, "Km cơ", "KmCO");

        public static string DetailsReport_Table_VehicleType => Get(MobileResourceNames.DetailsReport_Table_VehicleType, "Loại xe", "Vehicle Type");

        public static string DetailsReport_Table_VehiclePlate => Get(MobileResourceNames.DetailsReport_Table_VehiclePlate, "Biển số xe", "Vehicle Plate");

        public static string DetailsReport_Table_QuotaFuel => Get(MobileResourceNames.DetailsReport_Table_QuotaFuel, "Định mức nhiên liệu", "Quota Fuel");

        public static string DetailsReport_Table_QuotaFuelConsume => Get(MobileResourceNames.DetailsReport_Table_QuotaFuelConsume, "Nhiên liệu tiêu thụ", "Quota Fuel Consume");

        public static string DetailsReport_Table_StartAddress => Get(MobileResourceNames.DetailsReport_Table_StartAddress, "Địa chỉ đi", "StartAddress");

        public static string DetailsReport_Table_EndAddress => Get(MobileResourceNames.DetailsReport_Table_EndAddress, "Địa chỉ đến", "End Address");

        public static string DetailsReport_Table_TripCompensatory => Get(MobileResourceNames.DetailsReport_Table_TripCompensatory, "Cuốc bù", "Trip Compensatory");

        public static string DetailsReport_CheckBox_VehicleType => Get(MobileResourceNames.DetailsReport_CheckBox_VehicleType, "Loại xe", "Vehicle Type");

        public static string DetailsReport_CheckBox_VehiclePlate => Get(MobileResourceNames.DetailsReport_CheckBox_VehiclePlate, "Biển số xe", "Vehicle Plate");

        public static string DetailsReport_CheckBox_TripCompensatory => Get(MobileResourceNames.DetailsReport_CheckBox_TripCompensatory, "Cuốc bù", "Trip Compensatory");

        public static string DetailsReport_CheckBox_CurrentTime => Get(MobileResourceNames.DetailsReport_CheckBox_CurrentTime, "Ngày tháng", "Date");

        public static string DetailsReport_CheckBox_TimeActive => Get(MobileResourceNames.DetailsReport_CheckBox_TimeActive, "Thời gian hoạt động", "Time Active");

        public static string DetailsReport_CheckBox_KmGPS => Get(MobileResourceNames.DetailsReport_CheckBox_KmGPS, "Km (GPS)", "Km (GPS)");

        public static string DetailsReport_CheckBox_QuotaFuel => Get(MobileResourceNames.DetailsReport_CheckBox_QuotaFuel, "Định mức nhiên liệu", "Quota Fuel");

        public static string DetailsReport_CheckBox_KmCO => Get(MobileResourceNames.DetailsReport_CheckBox_KmCO, "Km cơ", "KmCO");

        public static string DetailsReport_CheckBox_QuotaFuelConsume => Get(MobileResourceNames.DetailsReport_CheckBox_QuotaFuelConsume, "Nhiên liệu tiêu thụ", "Quota Fuel Consume");

        public static string DetailsReport_CheckBox_StartAddress => Get(MobileResourceNames.DetailsReport_CheckBox_StartAddress, "Địa chỉ đi", "StartAddress");

        public static string DetailsReport_CheckBox_EndAddress => Get(MobileResourceNames.DetailsReport_CheckBox_EndAddress, "Địa chỉ đến", "End Address");

        #endregion Báo cáo chi tiết

        #region Báo cáo dừng đỗ

        public static string StopParkingReport_Label_TilePage => Get(MobileResourceNames.StopParkingReport_Label_TilePage, "Báo cáo dừng đỗ", "Stop parking report");

        public static string StopParkingReport_Label_TitlePageDetail => Get(MobileResourceNames.StopParkingReport_Label_TitlePageDetail, "Thông tin chi tiết", "Detail");

        public static string StopParkingReport_Label_VehiclePlate => Get(MobileResourceNames.StopParkingReport_Label_VehiclePlate, "Biển số xe", "Vehicle Plate");

        public static string StopParkingReport_Label_DriverName => Get(MobileResourceNames.StopParkingReport_Label_DriverName, "Tên lái xe", "Driver Name");

        public static string StopParkingReport_Label_CodeName => Get(MobileResourceNames.StopParkingReport_Label_CodeName, "Mã nhân viên", "Code");

        public static string StopParkingReport_Label_Phone => Get(MobileResourceNames.StopParkingReport_Label_Phone, "Số điện thoại", "Phone");

        public static string StopParkingReport_Label_Date => Get(MobileResourceNames.StopParkingReport_Label_Date, "Thời gian", "Date");

        public static string StopParkingReport_Label_Time => Get(MobileResourceNames.StopParkingReport_Label_Time, "Thời gian (phút)", "Time (minutes)");

        public static string StopParkingReport_Label_StopParkingTime => Get(MobileResourceNames.StopParkingReport_Label_StopParkingTime, "Thời gian dừng đỗ", "Time Stop Parking");

        public static string StopParkingReport_Label_MinutesStopRunEngine => Get(MobileResourceNames.StopParkingReport_Label_MinutesStopRunEngine, "Nổ máy khi dừng (phút)", "Stop Run Engine (minutes)");

        public static string StopParkingReport_Label_MinutesTurnOnAirConditioner => Get(MobileResourceNames.StopParkingReport_Label_MinutesTurnOnAirConditioner, "Bật ĐH khi dừng (phút)", "Turn On Air Conditioner (minutes)");

        public static string StopParkingReport_Label_StopLocation => Get(MobileResourceNames.StopParkingReport_Label_StopLocation, "Địa điểm", "Location");

        public static string StopParkingReport_Label_PlaceHolder_NumberOfStopParking => Get(MobileResourceNames.StopParkingReport_Label_PlaceHolder_NumberOfStopParking, "Dừng đỗ (phút) >=", "Stop Parking (minutes) >=");

        public static string StopParkingReport_Label_PlaceHolder_NumberOfStopRunEngine => Get(MobileResourceNames.StopParkingReport_Label_PlaceHolder_NumberOfStopRunEngine, "Dừng nổ máy (phút) >=", "Stop Run Engine (minutes) >=");

        public static string StopParkingReport_Message_ValidateError_StopParkingTime => Get(MobileResourceNames.StopParkingReport_Message_ValidateError_StopParkingTime, "Thời gian dừng đỗ không đúng định dạng", "You don't input number of minutes");

        public static string StopParkingReport_Message_ValidateError_MinutesStopRunEngine => Get(MobileResourceNames.StopParkingReport_Message_ValidateError_MinutesStopRunEngine, "Thời gian nổ máy khi dừng không đúng định dạng", "You don't input number of minutes");

        public static string StopParkingReport_Table_Serial => Get(MobileResourceNames.StopParkingReport_Table_Serial, "STT", "Serial");

        public static string StopParkingReport_Table_DriverName => Get(MobileResourceNames.StopParkingReport_Table_DriverName, "Tên lái xe", "Driver Name");

        public static string StopParkingReport_Table_CodeName => Get(MobileResourceNames.StopParkingReport_Table_CodeName, "Mã nhân viên", "Code");

        public static string StopParkingReport_Table_Phone => Get(MobileResourceNames.StopParkingReport_Table_Phone, "Số điện thoại", "Phone");

        public static string StopParkingReport_Table_VehiclePlate => Get(MobileResourceNames.StopParkingReport_Table_VehiclePlate, "Biển số xe", "Vehicle Plate");

        public static string StopParkingReport_Table_Date => Get(MobileResourceNames.StopParkingReport_Table_Date, "Thời gian", "Date");

        public static string StopParkingReport_Table_Time => Get(MobileResourceNames.StopParkingReport_Table_Time, "Thời gian (phút)", "Time (minutes)");

        public static string StopParkingReport_Table_StopParkingTime => Get(MobileResourceNames.StopParkingReport_Table_StopParkingTime, "Thời gian dừng đỗ (hh:mm:ss)", "Time Stop Parking (hh:mm:ss)");

        public static string StopParkingReport_Table_MinutesStopRunEngine => Get(MobileResourceNames.StopParkingReport_Table_MinutesStopRunEngine, "Nổ máy khi dừng (phút)", "Stop Run Engine (minutes)");

        public static string StopParkingReport_Table_MinutesTurnOnAirConditioner => Get(MobileResourceNames.StopParkingReport_Table_MinutesTurnOnAirConditioner, "Bật ĐH khi dừng (phút)", "Turn On Air Conditioner (minutes)");

        public static string StopParkingReport_Table_Temperature => Get(MobileResourceNames.StopParkingReport_Table_Temperature, "Nhiệt độ", "Temperature");

        public static string StopParkingReport_Table_StopLocation => Get(MobileResourceNames.StopParkingReport_Table_StopLocation, "Địa điểm", "Location");

        public static string StopParkingReport_CheckBox_DriverName => Get(MobileResourceNames.StopParkingReport_CheckBox_DriverName, "Tên lái xe", "Driver Name");

        public static string StopParkingReport_CheckBox_CodeName => Get(MobileResourceNames.StopParkingReport_CheckBox_CodeName, "Mã nhân viên", "Code");

        public static string StopParkingReport_CheckBox_Phone => Get(MobileResourceNames.StopParkingReport_CheckBox_Phone, "Số điện thoại", "Phone");

        public static string StopParkingReport_CheckBox_VehiclePlate => Get(MobileResourceNames.StopParkingReport_CheckBox_VehiclePlate, "Biển số xe", "Vehicle Plate");

        public static string StopParkingReport_CheckBox_Time => Get(MobileResourceNames.StopParkingReport_CheckBox_Time, "Thời gian (phút)", "Time (minutes)");

        public static string StopParkingReport_CheckBox_StopParkingTime => Get(MobileResourceNames.StopParkingReport_CheckBox_StopParkingTime, "Thời gian dừng đỗ", "Time Stop Parking");

        public static string StopParkingReport_CheckBox_MinutesStopRunEngine => Get(MobileResourceNames.StopParkingReport_CheckBox_MinutesStopRunEngine, "Nổ máy khi dừng", "Stop Run Engine");

        public static string StopParkingReport_CheckBox_MinutesTurnOnAirConditioner => Get(MobileResourceNames.StopParkingReport_CheckBox_MinutesTurnOnAirConditioner, "Bật ĐH khi dừng", "Turn On Air Conditioner");

        public static string StopParkingReport_CheckBox_StopLocation => Get(MobileResourceNames.StopParkingReport_CheckBox_StopLocation, "Địa điểm", "Location");

        #endregion Báo cáo dừng đỗ

        #region Báo cáo quá tốc độ

        public static string SpeedOversReport_Label_TilePage => Get(MobileResourceNames.SpeedOversReport_Label_TilePage, "Báo cáo quá tốc độ", "Speed overs report");

        public static string SpeedOversReport_Label_Group => Get(MobileResourceNames.SpeedOversReport_Label_Group, "Nhóm", "Group");

        public static string SpeedOversReport_Label_PlaceHolder_Speed => Get(MobileResourceNames.SpeedOversReport_Label_PlaceHolder_Speed, "Tốc độ >=", "Speed >=");

        public static string SpeedOversReport_Label_TitlePageDetail => Get(MobileResourceNames.SpeedOversReport_Label_TitlePageDetail, "Thông tin chi tiết", "Detail");

        public static string SpeedOversReport_Label_DetailVehiclePlate => Get(MobileResourceNames.SpeedOversReport_Label_DetailVehiclePlate, "Biển số xe", "VehiclePlate");

        public static string SpeedOversReport_Label_Date => Get(MobileResourceNames.SpeedOversReport_Label_Date, "Thời điểm", "Date");

        public static string SpeedOversReport_Label_Time => Get(MobileResourceNames.SpeedOversReport_Label_Time, "Thời gian (s)", "Time (s)");

        public static string SpeedOversReport_Label_Distance => Get(MobileResourceNames.SpeedOversReport_Label_Distance, "Quãng đường (m)", "Distance (m)");

        public static string SpeedOversReport_Label_Vmax => Get(MobileResourceNames.SpeedOversReport_Label_Vmax, "Tốc độ cực đại", "VMax");

        public static string SpeedOversReport_Label_StartAddress => Get(MobileResourceNames.SpeedOversReport_Label_StartAddress, "Điểm bắt đầu", "Start Address");

        public static string SpeedOversReport_Label_EndAddress => Get(MobileResourceNames.SpeedOversReport_Label_EndAddress, "Điểm kết thúc", "End Address");

        public static string SpeedOversReport_Message_ValidateError_Speed => Get(MobileResourceNames.SpeedOversReport_Message_ValidateError_Speed, "Tốc độ không đúng định dạng", "You don't input number of minutes");

        public static string SpeedOversReport_CheckBox_TotalTime => Get(MobileResourceNames.SpeedOversReport_CheckBox_TotalTime, "Thời gian", "Time");

        public static string SpeedOversReport_CheckBox_TotalDistance => Get(MobileResourceNames.SpeedOversReport_CheckBox_TotalDistance, "Quãng đường", "Distance");

        public static string SpeedOversReport_CheckBox_Vmax => Get(MobileResourceNames.SpeedOversReport_CheckBox_Vmax, "Tốc độ cực đại", "VMax");

        public static string SpeedOversReport_CheckBox_StartAddress => Get(MobileResourceNames.SpeedOversReport_CheckBox_StartAddress, "Điểm bắt đầu", "Start Address");

        public static string SpeedOversReport_CheckBox_EndAddress => Get(MobileResourceNames.SpeedOversReport_CheckBox_EndAddress, "Điểm kết thúc", "End Address");

        public static string SpeedOversReport_Table_Serial => Get(MobileResourceNames.SpeedOversReport_Table_Serial, "STT", "Serial");

        public static string SpeedOversReport_Table_DetailVehiclePlate => Get(MobileResourceNames.SpeedOversReport_Table_DetailVehiclePlate, "Biển số xe", "VehiclePlate");

        public static string SpeedOversReport_Table_Date => Get(MobileResourceNames.SpeedOversReport_Table_Date, "Thời điểm", "Date");

        public static string SpeedOversReport_Table_Time => Get(MobileResourceNames.SpeedOversReport_Table_Time, "Thời gian (s)", "Time (s)");

        public static string SpeedOversReport_Table_Distance => Get(MobileResourceNames.SpeedOversReport_Table_Distance, "Quãng đường (m)", "Distance (m)");

        public static string SpeedOversReport_Table_Vmax => Get(MobileResourceNames.SpeedOversReport_Table_Vmax, "Tốc độ cực đại", "VMax");

        public static string SpeedOversReport_Table_StartAddress => Get(MobileResourceNames.SpeedOversReport_Table_StartAddress, "Điểm bắt đầu", "Start Address");

        public static string SpeedOversReport_Table_EndAddress => Get(MobileResourceNames.SpeedOversReport_Table_EndAddress, "Điểm kết thúc", "End Address");

        #endregion Báo cáo quá tốc độ

        #region Báo cáo mất tín hiệu

        public static string ReportSignalLoss_Label_TitlePage => Get(MobileResourceNames.ReportSignalLoss_Label_TitlePage, "Báo cáo mất tín hiệu", "Report signal loss");

        public static string ReportSignalLoss_Label_TitlePageDetail => Get(MobileResourceNames.ReportSignalLoss_Label_TitlePageDetail, "Thông tin chi tiết", "Detail");

        public static string ReportSignalLoss_TitleStatus => Get(MobileResourceNames.ReportSignalLoss_TitleStatus, "Trạng thái", "Status");

        public static string ReportSignalLoss_Title_MinTimeLosing => Get(MobileResourceNames.ReportSignalLoss_Title_MinTimeLosing, "Số phút mất tín hiệu >=", "Minutes lost signal >=");

        public static string ReportSignalLoss_Title_StartTime => Get(MobileResourceNames.ReportSignalLoss_Title_StartTime, "Thời gian bắt đầu", "Start Time");

        public static string ReportSignalLoss_Title_EndTime => Get(MobileResourceNames.ReportSignalLoss_Title_EndTime, "Thời gian kết thúc", "End Time");

        public static string ReportSignalLoss_Title_TimeLosing => Get(MobileResourceNames.ReportSignalLoss_Title_TimeLosing, "Thời gian mất tín hiệu", "Time lost the signal");

        public static string ReportSignalLoss_Title_StartAddress => Get(MobileResourceNames.ReportSignalLoss_Title_StartAddress, "Địa điểm bắt đầu", "Start address");

        public static string ReportSignalLoss_Title_EndAddress => Get(MobileResourceNames.ReportSignalLoss_Title_EndAddress, "Địa điểm kết thúc", "End address");

        public static string ReportSignalLoss_TitleStatus_All => Get(MobileResourceNames.ReportSignalLoss_TitleStatus_All, "Tất cả", "All");

        public static string ReportSignalLoss_TitleStatus_GPS => Get(MobileResourceNames.ReportSignalLoss_TitleStatus_GPS, "GPS", "GPS");

        public static string ReportSignalLoss_TitleStatus_GMS => Get(MobileResourceNames.ReportSignalLoss_TitleStatus_GMS, "GSM", "GSM");

        public static string ReportSignalLoss_Message_ValidateError_MinTimeLosing => Get(MobileResourceNames.ReportSignalLoss_Message_ValidateError_MinTimeLosing, "Số phút mất tín hiệu không đúng định dạng", "You don't input number of minutes");

        public static string ReportSignalLoss_Message_ValidateError_MinTimeLosing2 => Get(MobileResourceNames.ReportSignalLoss_Message_ValidateError_MinTimeLosing2, "Số phút mất tín hiệu nằm trong khoảng từ 5 đến 1000000000", "The number of minutes of signal loss ranges from 5 to 10 billion");

        #endregion Báo cáo mất tín hiệu
    }
}