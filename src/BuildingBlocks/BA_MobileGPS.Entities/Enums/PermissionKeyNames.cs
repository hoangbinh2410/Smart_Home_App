namespace BA_MobileGPS.Entities
{
    /// <summary>
    /// Store  permission key name, mapping with database
    /// </summary>
    /// <Modified>
    /// Name                 date           comments
    /// congnt               28/03/12          created
    /// </Modified>
    public enum PermissionKeyNames
    {
        /// <summary>
        /// Không cần quyền
        /// </summary>
        NoPermission = -1,

        None = 0,

        // view module permissions (1-10)
        /// <summary>
        /// Module online: 1
        /// </summary>
        ViewModuleOnline = 1,

        /// <summary>
        /// Module tong quan: 2
        /// </summary>
        ViewModuleSumary = 2,

        /// <summary>
        /// Module bao cao: 3
        /// </summary>
        ViewModuleReports = 3,

        /// <summary>
        /// Module tien ich: 4
        /// </summary>
        ViewModuleUtilities = 4,

        /// <summary>
        /// Module quan tri: 5
        /// </summary>
        ViewModuleAdmin = 5,

        /// <summary>
        /// Module lộ trình: 5
        /// </summary>
        ViewModuleRoute = 6,

        /// <summary>
        /// Module bảo dưỡng
        /// </summary>
        ViewModuleMaintain = 9,

        /// <summary>
        /// Module quản trị BA
        /// </summary>
        ModuleBAAdmin = 10,

        // company permissions (11-20)
        /// <summary>
        /// Xem cong ty: 11
        /// </summary>
        CompanyView = 11,

        /// <summary>
        /// Tao moi cong ty: 12
        /// </summary>
        CompanyAdd = 12,

        /// <summary>
        /// Sua cong ty: 13
        /// </summary>
        CompanyUpdate = 13,

        /// <summary>
        /// Xoa cong ty: 14
        /// </summary>
        CompanyDelete = 14,

        /// <summary>
        /// Export cong ty: 15
        /// </summary>
        CompanyExport = 15,

        /// <summary>
        /// Phan quyen cong ty: 16
        /// </summary>
        CompanyOfferPermission = 16,

        /// <summary>
        /// Khoa cong ty: 17
        /// </summary>
        CompanyLock = 17,

        /// <summary>
        /// Cong ty khach le: 18
        /// </summary>
        CompanyOddCustomer = 18,

        //user permissions (21-30)
        /// <summary>
        /// Xem thong tin user, list user: 22
        /// </summary>
        UserView = 21,

        /// <summary>
        /// Tao moi user: 22
        /// </summary>
        UserAdd = 22,

        /// <summary>
        /// Sua user: 23
        /// </summary>
        UserUpdate = 23,

        /// <summary>
        /// Xoa user: 14
        /// </summary>
        UserDelete = 24,

        /// <summary>
        /// Export user: 25
        /// </summary>
        UserExport = 25,

        /// <summary>
        /// Phan quyen user: 26
        /// </summary>
        UserOfferPermission = 26,

        /// <summary>
        /// Khoa user: 27
        /// </summary>
        UserLock = 27,

        /// <summary>
        /// Xem danh sách user mới được thêm mới, sửa đổi
        /// </summary>
        UserNewestView = 28,

        //role permissions (31-40)
        /// <summary>
        /// Xem vai tro: 31
        /// </summary>
        RoleView = 31,

        /// <summary>
        /// Them moi vai tro: 32
        /// </summary>
        RoleAdd = 32,

        /// <summary>
        /// Sua vai tro: 33
        /// </summary>
        RoleUpdate = 33,

        /// <summary>
        /// Xoa vai tro: 34
        /// </summary>
        RoleDelete = 34,

        //Info company  (41-50)
        /// <summary>
        /// Xem thong tin cong ty cua minh: 41
        /// </summary>
        CompanyInfoView = 41,

        /// <summary>
        /// Sua 1 so thong tin cua cong ty minh: 42
        /// </summary>
        CompanyInfoUpdate = 42,

        //log permissions (51-60)
        /// <summary>
        /// Xem ket qua log cua cong ty minh: 51
        /// </summary>
        LogView = 51,

        //vehicle permissions (61-70)
        /// <summary>
        /// Xem xe: 61
        /// </summary>
        VehicleView = 61,

        /// <summary>
        /// Them xe: 62
        /// </summary>
        VehicleAdd = 62,

        /// <summary>
        /// Sua xe: 63
        /// </summary>
        VehicleUpdate = 63,

        /// <summary>
        /// Xoa xe: 64
        /// </summary>
        VehicleDelete = 64,

        /// <summary>
        /// Export ra file Excel: 65
        /// </summary>
        VehicleExport = 65,

        /// <summary>
        /// Khoa xe: 66
        /// </summary>
        VehicleLock = 66,

        /// <summary>
        /// In danh sach xe: 67
        /// </summary>
        VehiclePrint = 67,

        /// <summary>
        /// Cấu hình xe: 67
        /// </summary>
        VehicleConfig = 68,

        //utility permissions (71-80)
        /// <summary>
        /// View diem: 71
        /// </summary>
        LandmarkView = 71,

        /// <summary>
        /// Tao diem: 72
        /// </summary>
        LandmarkAdd = 72,

        /// <summary>
        /// Sua diem: 73
        /// </summary>
        LandmarkUpdate = 73,

        /// <summary>
        /// Xoa diem: 74
        /// </summary>
        LandmarkDelete = 74,

        /// <summary>
        /// Export diem: 75
        /// </summary>
        LandmarkExport = 75,

        /// <summary>
        /// Điểm - Xem nhiều: 76
        /// </summary>
        LandmarkBatchView = 76,

        /// <summary>
        /// Điểm - Sửa nhiều: 77
        /// </summary>
        LandmarkBatchUpdate = 77,

        /// <summary>
        /// Quyền xem điểm thu phí
        /// </summary>
        LandmarkChargeView = 78,

        //Help permissions
        /// <summary>
        /// Xem tro giup: 81
        /// </summary>
        HelpView = 81,

        /// <summary>
        /// Them moi tro giup
        /// </summary>
        HelpAdd = 82,

        /// <summary>
        /// Sua tro giup
        /// </summary>
        HelpUpdate = 83,

        /// <summary>
        /// Xoa tro giup
        /// </summary>
        HelpDelete = 84,

        //SIM permissions
        /// <summary>
        /// SIM xem
        /// </summary>
        SIMView = 126,

        /// <summary>
        /// SIM them moi
        /// </summary>
        SIMAdd = 127,

        /// <summary>
        /// SIM sua
        /// </summary>
        SIMUpdate = 128,

        /// <summary>
        /// SIM Xoa
        /// </summary>
        SIMDelete = 129,

        /// <summary>
        /// SIM Export
        /// </summary>
        SIMExport = 130,

        //Thong tin thiet bi permissions
        DeviceInfoView = 131,

        //point permissions

        //alert permissions

        //report permissions
        /// <summary>
        /// Báo cáo dừng đỗ - Xem
        /// </summary>
        ReportStopView = 86,

        /// <summary>
        /// Báo cáo dừng đỗ - Export
        /// </summary>
        ReportStopExport = 87,

        /// <summary>
        /// Báo cáo chi tiết hoạt động - Xem
        /// </summary>
        ReportActivityDetailView = 91,

        /// <summary>
        /// Báo cáo chi tiết hoạt động - Export
        /// </summary>
        ReportActivityDetailExport = 92,

        /// <summary>
        /// Báo cáo chi tiết hoạt động - Option
        /// </summary>
        ReportActivityDetailOption = 93,

        /// <summary>
        /// Báo cáo tổng hợp - Xem
        /// </summary>
        ReportActivitySummaryView = 96,

        /// <summary>
        /// Báo cáo tổng hợp - Export
        /// </summary>
        ReportActivitySummaryExport = 97,

        /// <summary>
        /// Báo cáo tổng hợp - Option
        /// </summary>
        ReportActivitySummaryOption = 98,

        /// <summary>
        /// Báo cáo ra vào trạm - Xem
        /// </summary>
        ReportStationView = 101,

        /// <summary>
        /// Báo cáo ra vào trạm - Export
        /// </summary>
        ReportStationExport = 102,

        /// <summary>
        /// Báo cáo quá tốc độ - Xem
        /// </summary>
        ReportSpeedOverView = 106,

        /// <summary>
        /// Báo cáo quá tốc độ - Export
        /// </summary>
        ReportSpeedOverExport = 107,

        /// <summary>
        /// Báo cáo quá tốc độ - Option
        /// </summary>
        ReportSpeedOverOption = 108,

        /// <summary>
        /// Báo cáo mở cửa - Xem
        /// </summary>
        ReportDoorView = 111,

        /// <summary>
        /// Báo cáo mở cửa - Export
        /// </summary>
        ReportDoorExport = 112,

        /// <summary>
        /// Báo cáo quá tốc độ - Option
        /// </summary>
        ReportDoorOption = 113,

        /// <summary>
        /// Báo cáo thời gian lái xe liên tục trong ngày - Xem
        /// </summary>
        ReportDrivingTimeContinuousView = 116,

        /// <summary>
        /// Báo cáo thời gian lái xe liên tục trong ngày - Export
        /// </summary>
        ReportDrivingTimeContinuousExport = 117,

        /// <summary>
        /// Báo cáo thời gian lái xe liên tục trong hành trình - Xem
        /// </summary>
        ReportDrivingTimeJourneyContinuousView = 121,

        /// <summary>
        /// Báo cáo thời gian lái xe liên tục trong hành trình - Export
        /// </summary>
        ReportDrivingTimeJourneyContinuousExport = 122,

        /// <summary>
        /// Báo cáo hành trình - Xem
        /// </summary>
        ReportItineraryView = 136,

        /// <summary>
        /// Báo cáo hành trình - Export
        /// </summary>
        ReportItineraryExport = 137,

        /// <summary>
        /// Báo cáo lịch trình tuyến - Xem
        /// </summary>
        ReportScheduleRouteView = 141,

        /// <summary>
        /// Báo cáo lịch trình tuyến - Export
        /// </summary>
        ReportScheduleRouteExport = 142,

        /// <summary>
        /// Báo cáo lịch trình tuyến - Option
        /// </summary>
        ReportScheduleRouteOption = 143,

        /// <summary>
        /// Báo cáo tổng hợp lịch trình tuyến - Xem
        /// </summary>
        ReportScheduleRouteSummaryView = 146,

        /// <summary>
        /// Báo cáo tổng hợp lịch trình tuyến - Export
        /// </summary>
        ReportScheduleRouteSummaryExport = 147,

        /// <summary>
        /// Báo cáo tổng hợp lịch trình tuyến - Option
        /// </summary>
        ReportScheduleRouteSummaryOption = 148,

        /// <summary>
        /// Báo cáo chuyến kinh doanh - Xem
        /// </summary>
        ReportBusinessTripView = 151,

        /// <summary>
        /// Báo cáo chuyến kinh doanh - Export
        /// </summary>
        ReportBusinessTripExport = 152,

        /// <summary>
        /// Báo cáo bật điều hòa - Xem
        /// </summary>
        ReportAirConditionerView = 156,

        /// <summary>
        /// Báo cáo bật điều hòa - Export
        /// </summary>
        ReportAirConditionerExport = 157,

        /// <summary>
        /// Ảnh Camera - Xem
        /// </summary>
        AdminUtilityImageView = 161,

        /// <summary>
        /// Quản lý ảnh camera - Xem
        /// </summary>
        AdminUtilityImageManagement = 162,

        /// <summary>
        /// Quản lý ảnh camera trên cửa sổ mới
        /// </summary>
        AdminCameraManagementDetail = 163,

        /// <summary>
        /// Quản lý ảnh camera trên cửa sổ mới
        /// </summary>
        AdminCameraViewDetail = 164,

        /// <summary>
        /// Quan ly anh camera 3G
        /// </summary>
        AdminCamera3G = 165,

        /// <summary>
        /// Báo cáo xe qua điểm thu phí - Xem: 166
        /// </summary>
        ReportChargeStationView = 166,

        /// <summary>
        /// Báo cáo xe qua điểm thu phí - Export: 167
        /// </summary>
        ReportChargeStationExport = 167,

        /// <summary>
        /// Báo cáo mất GPS - Xem: 171
        /// </summary>
        ReportSignalLossView = 171,

        /// <summary>
        /// Báo cáo mất GPS - Export: 172
        /// </summary>
        ReportSignalLossExport = 172,

        /// <summary>
        /// Báo cáo liên lạc - Xem: 176
        /// </summary>
        ReportConnectionLossView = 176,

        /// <summary>
        /// Báo cáo liên lạc - Export: 177
        /// </summary>
        ReportConnectionLossExport = 177,

        /// <summary>
        /// Báo cáo tổng hợp tháng - Xem: 181
        /// </summary>
        ReportActivitySummaryMonthView = 181,

        /// <summary>
        /// Báo cáo tổng hợp tháng - Export: 182
        /// </summary>
        ReportActivitySummaryMonthExport = 182,

        /// <summary>
        /// Báo cáo tổng hợp tháng - Option
        /// </summary>
        ReportActivitySummaryMonthOption = 183,

        /// <summary>
        /// Báo cáo nâng hạ cẩu cho xe cứu hộ - View: 186
        /// </summary>
        ReportRescueVehicleView = 186,

        /// <summary>
        ///  Báo cáo nâng hạ cẩu cho xe cứu hộ - Export: 187
        /// </summary>
        ReportRescueVehicleExport = 187,

        /// <summary>
        /// Nhân viên - Xem
        /// </summary>
        AdminEmployeeView = 191,

        /// <summary>
        /// Nhân viên - Tạo mới
        /// </summary>
        AdminEmployeeAdd = 192,

        /// <summary>
        /// Nhân viên - Sửa
        /// </summary>
        AdminEmployeeUpdate = 193,

        /// <summary>
        /// Nhân viên - Xóa
        /// </summary>
        AdminEmployeeDelete = 194,

        /// <summary>
        /// Nhân viên - Export
        /// </summary>
        AdminEmployeeExport = 195,

        /// <summary>
        /// Nạp lái xe xuống hộp đen - Xem
        /// </summary>
        AdminBlackBoxView = 201,

        /// <summary>
        /// Nạp lái xe xuống hộp đen - Tạo mới
        /// </summary>
        AdminBlackBoxAdd = 202,

        /// <summary>
        /// Nạp lái xe xuống hộp đen - Sửa
        /// </summary>
        AdminBlackBoxUpdate = 203,

        /// <summary>
        /// Nạp lái xe xuống hộp đen - Xóa
        /// </summary>
        AdminBlackBoxDelete = 204,

        /// <summary>
        /// Nạp lái xe xuống hộp đen
        /// </summary>
        AdminBlackBoxInstall = 205,

        /// <summary>
        /// Thẻ - Xem
        /// </summary>
        AdminRFIDView = 211,

        /// <summary>
        /// Thẻ - Tạo mới
        /// </summary>
        AdminRFIDAdd = 212,

        /// <summary>
        /// Thẻ - Sửa
        /// </summary>
        AdminRFIDUpdate = 213,

        /// <summary>
        /// Thẻ - Xóa
        /// </summary>
        AdminRFIDDelete = 214,

        /// <summary>
        /// Thẻ - Epxort
        /// </summary>
        AdminRFIDExport = 215,

        /// <summary>
        /// Báo cáo chấm công - Xem
        /// </summary>
        ReportCheckInCheckOutView = 221,

        /// <summary>
        /// Báo cáo chấm công - Export
        /// </summary>
        ReportCheckInCheckOutExport = 222,

        /// <summary>
        /// Báo cáo chi tiết chuyến sân bay - Xem
        /// </summary>
        ReportAirportTripDetailView = 235,

        /// <summary>
        /// Báo cáo chi tiết chuyến sân bay - Export
        /// </summary>
        ReportAirportTripDetailExport = 236,

        /// <summary>
        /// Báo cáo chi tiết chuyến sân bay - Option
        /// </summary>
        ReportAirportTripDetailOption = 237,

        /// <summary>
        /// Báo cáo tổng hợp chuyến sân bay - Xem
        /// </summary>
        ReportAirportTripSummaryView = 238,

        /// <summary>
        /// Báo cáo tổng hợp chuyến sân bay - Export
        /// </summary>
        ReportAirportTripSummaryExport = 239,

        /// <summary>
        /// Báo cáo tổng hợp chuyến sân bay - Option
        /// </summary>
        ReportAirportTripSummaryOption = 240,

        /// <summary>
        /// Báo cáo diễn giải chuyến sân bay - Xem
        /// </summary>
        ReportAirportTripDescriptionView = 241,

        /// <summary>
        /// Báo cáo diễn giải chuyến sân bay - Export
        /// </summary>
        ReportAirportTripDescriptionExport = 242,

        /// <summary>
        /// Báo cáo diễn giải chuyến sân bay - Option
        /// </summary>
        ReportAirportTripDescriptionOption = 243,

        /// <summary>
        /// Báo cáo lỗi xung - Xem
        /// </summary>
        ReportPulseErrorView = 247,

        /// <summary>
        /// Báo cáo lỗi xung - Export
        /// </summary>
        ReportPulseErrorExport = 248,

        /// <summary>
        /// Báo cáo lỗi xung - Option
        /// </summary>
        ReportPulseErrorOption = 249,

        /// <summary>
        /// Báo cáo đổ hút nhiên liệu - Xem
        /// </summary>
        ReportFuelView = 253,

        /// <summary>
        /// Báo cáo đổ hút nhiên liệu - Export
        /// </summary>
        ReportFuelExport = 254,

        /// <summary>
        /// Báo cáo đổ hút nhiên liệu option ẩn hiện cột
        /// </summary>
        ReportFuelOption = 255,

        /// <summary>
        /// Báo cáo bật động cơ - Xem
        /// </summary>
        ReportMachineOnView = 262,

        /// <summary>
        /// Báo cáo bật động cơ - Export
        /// </summary>
        ReportMachineOnExport = 263,

        /// <summary>
        /// Báo cáo bật động cơ - Option
        /// </summary>
        ReportMachineOnOption = 264,

        /// <summary>
        /// Báo cáo trạng thái động cơ - Xem
        /// </summary>
        ReportMachineStateView = 265,

        /// <summary>
        /// Báo cáo trạng thái động cơ - Export
        /// </summary>
        ReportMachineStateExport = 266,

        /// <summary>
        /// Báo cáo trạng thái động cơ - Option
        /// </summary>
        ReportMachineStateOption = 267,

        /// <summary>
        /// Báo cáo đổ bê tông - Xem
        /// </summary>
        ReportConcreteView = 268,

        /// <summary>
        /// Báo cáo đổ bê tông - Export
        /// </summary>
        ReportConcreteExport = 269,

        /// <summary>
        /// Báo cáo đổ bê tông - Option
        /// </summary>
        ReportConcreteOption = 270,

        /// <summary>
        /// Báo cáo tổng hợp nhiên liệu - Xem
        /// </summary>
        ReportConcreteFuelSummaryView = 283,

        /// <summary>
        /// Báo cáo tổng hợp nhiên liệu - Export
        /// </summary>
        ReportConcreteFuelSummaryExport = 284,

        /// <summary>
        /// Báo cáo tổng hợp nhiên liệu - Option
        /// </summary>
        ReportConcreteFuelSummaryOption = 285,

        /// <summary>
        /// Bảng tính lương chuyến bê tông - Xem
        /// </summary>
        ReportConcreteTripSalaryView = 289,

        /// <summary>
        /// Bảng tính lương chuyến bê tông - Export
        /// </summary>
        ReportConcreteTripSalaryExport = 290,

        /// <summary>
        /// Bảng tính lương chuyến bê tông - Option
        /// </summary>
        ReportConcreteTripSalaryOption = 291,

        /// <summary>
        /// Báo cáo xả hàng - Xem
        /// </summary>
        ReportDeliveryDischargeView = 295,

        /// <summary>
        /// Báo cáo xả hàng - Export
        /// </summary>
        ReportDeliveryDischargeExport = 296,

        /// <summary>
        /// Báo cáo xả hàng - Option
        /// </summary>
        ReportDeliveryDischargeOption = 297,

        /// <summary>
        /// Báo cáo xả hàng không hợp lệ - Xem
        /// </summary>
        ReportDeliveryDischargeInvalidView = 298,

        /// <summary>
        ///Báo cáo xả hàng không hợp lệ - Export
        /// </summary>
        ReportDeliveryDischargeInvalidExport = 299,

        /// <summary>
        /// Báo cáo xả hàng không hợp lệ - Option
        /// </summary>
        ReportDeliveryDischargeInvalidOption = 300,

        /// <summary>
        /// Tổng hợp lịch trình tuyến theo tháng - Xem
        /// </summary>
        ReportScheduleRouteSummaryInMonthView = 313,

        /// <summary>
        /// Tổng hợp lịch trình tuyến theo tháng - Export
        /// </summary>
        ReportScheduleRouteSummaryInMonthExport = 314,

        /// <summary>
        /// Tổng hợp lịch trình tuyến theo tháng - Option
        /// </summary>
        ReportScheduleRouteSummaryInMonthOption = 315,

        /// <summary>
        /// Báo cáo tổng hợp km xe hoạt động - Xem
        /// </summary>
        ReportActivityKMSummaryView = 433,

        /// <summary>
        /// Báo cáo tổng hợp km xe hoạt động - Export
        /// </summary>
        ReportActivityKMSummaryExport = 434,

        /// <summary>
        /// Báo cáo tổng hợp km xe hoạt động - Option
        /// </summary>
        ReportActivityKMSummaryOption = 435,

        /// <summary>
        /// Báo cáo xe ra vào trạm lưu đêm - Xem
        /// </summary>
        ReportStationNightView = 436,

        /// <summary>
        ///  Báo cáo xe ra vào trạm lưu đêm - Export
        /// </summary>
        ReportStationNightExport = 437,

        /// <summary>
        /// Báo cáo xe ra vào trạm lưu đêm - Option
        /// </summary>
        ReportStationNightOption = 438,

        /// <summary>
        /// Báo cáo nâng hạ ben - Xem
        /// </summary>
        ReportTruckDumperOnOffView = 460,

        /// <summary>
        /// Báo cáo nâng hạ ben - Export
        /// </summary>
        ReportTruckDumperOnOffExport = 461,

        /// <summary>
        /// Báo cáo nâng hạ ben - Option
        /// </summary>
        ReportTruckDumperOnOffOption = 462,

        /// <summary>
        /// Quản lý điểm - Xem
        /// </summary>
        AdminConfigurationLandmarkView = 467,

        /// <summary>
        /// Phân quyền nhóm điểm
        /// </summary>
        AdminConfigurationLandmarkAssign = 468,

        /// <summary>
        /// Tạo tuyến điểm
        /// </summary>
        AdminLandmarkRouteAsign = 469,

        /// <summary>
        /// Quản trị thông báo - Xem
        /// </summary>
        AdminNotificationView = 506,

        /// <summary>
        /// Quản trị thông báo - Tạo mới
        /// </summary>
        AdminNotificationAdd = 507,

        /// <summary>
        /// Quản trị thông báo - Sửa
        /// </summary>
        AdminNotificationUpdate = 508,

        /// <summary>
        /// Quản trị thông báo - Xóa
        /// </summary>
        AdminNotificationDelete = 509,

        /// <summary>
        /// Quyền xem biểu đồ nhiên liệu
        /// </summary>
        FuelChartView = 510,

        /// <summary>
        /// Báo cáo xe qua điểm chốt - Xem
        /// </summary>
        ReportBenchmarkView = 511,

        /// <summary>
        /// Báo cáo xe qua điểm chốt - Export
        /// </summary>
        ReportBenchmarkExport = 512,

        /// <summary>
        /// Báo cáo xe qua điểm chốt - Option
        /// </summary>
        ReportBenchmarkOption = 513,

        /// <summary>
        /// Báo cáo BGT Mở cửa - Xem
        /// </summary>
        BGTReportDoorView = 514,

        /// <summary>
        /// Báo cáo BGT Mở cửa - Export
        /// </summary>
        BGTReportDoorExport = 515,

        /// <summary>
        /// Báo cáo BGT Mở cửa - Option
        /// </summary>
        BGTReportDoorOption = 516,

        /// <summary>
        /// Báo cáo BGT quá vận tốc - Xem
        /// </summary>
        BGTReportSpeedOverView = 517,

        /// <summary>
        /// Báo cáo BGT quá vận tốc - Export
        /// </summary>
        BGTReportSpeedOverExport = 518,

        /// <summary>
        /// Báo cáo BGT quá vận tốc - Option
        /// </summary>
        BGTReportSpeedOverOption = 519,

        /// <summary>
        /// Báo cáo vi phạm TG lái xe liên tục - Xem
        /// </summary>
        BGTReportDrivingTimeView = 520,

        /// <summary>
        /// Báo cáo vi phạm TG lái xe liên tục - Export
        /// </summary>
        BGTReportDrivingTimeExport = 521,

        /// <summary>
        /// Báo cáo vi phạm TG lái xe liên tục - Option
        /// </summary>
        BGTReportDrivingTimeOption = 522,

        /// <summary>
        /// Báo cáo TG làm việc của lái xe  - Xem
        /// </summary>
        BGTReportDrivingTimeJourneyView = 523,

        /// <summary>
        /// Báo cáo TG làm việc của lái xe - Export
        /// </summary>
        BGTReportDrivingTimeJourneyExport = 524,

        /// <summary>
        /// Báo cáo TG làm việc của lái xe - Option
        /// </summary>
        BGTReportDrivingTimeJourneyOption = 525,

        /// <summary>
        /// Cảnh báo - Xem
        /// </summary>
        AdminAlertView = 526,

        /// <summary>
        /// Cảnh báo - Tạo mới
        /// </summary>
        AdminAlertAdd = 527,

        /// <summary>
        /// Cảnh báo - Sửa
        /// </summary>
        AdminAlertUpdate = 528,

        /// <summary>
        /// Cảnh báo - Xóa
        /// </summary>
        AdminAlertDelete = 529,

        /// <summary>
        /// Quan ly loi (online - Hoang Ha) - Xem
        /// </summary>
        AdminErrorView = 531,

        /// <summary>
        /// Quan ly loi (online - Hoang Ha) - Tạo mới
        /// </summary>
        AdminErrorAdd = 532,

        /// <summary>
        /// Quan ly loi (online - Hoang Ha) - Sửa
        /// </summary>
        AdminErrorUpdate = 533,

        /// <summary>
        /// Quan ly loi (online - Hoang Ha) - Xóa
        /// </summary>
        AdminErrorDelete = 534,

        /// <summary>
        /// Báo cáo xe lỗi - Xem
        /// </summary>
        ReportVehicleErrorView = 535,

        /// <summary>
        /// Báo cáo xe lỗi - Export
        /// </summary>
        ReportVehicleErrorExport = 536,

        /// <summary>
        /// Báo cáo xe lỗi - Option
        /// </summary>
        ReportVehicleErrorOption = 537,

        /// <summary>
        /// Nhập giá nhiên liệu - Xem
        /// </summary>
        InputMarketInformationView = 538,

        /// <summary>
        /// Nhập giá nhiên liệu - Tạo mới
        /// </summary>
        InputMarketInformationAdd = 539,

        /// <summary>
        ///  Nhập giá nhiên liệu - Sửa
        /// </summary>
        InputMarketInformationUpdate = 540,

        /// <summary>
        /// Nhập giá nhiên liệu - Xóa
        /// </summary>
        InputMarketInformationDelete = 541,

        /// <summary>
        /// Cấu hình tiền lương - Xem
        /// </summary>
        InputConcreteSalaryView = 542,

        /// <summary>
        /// Cấu hình tiền lương  - Sửa
        /// </summary>
        InputConcreteSalaryAdd = 543,

        /// <summary>
        /// Cấu hình tiền lương - Sửa
        /// </summary>
        InputConcreteSalaryUpdate = 544,

        /// <summary>
        /// Cấu hình tiền lương - Xóa
        /// </summary>
        InputConcreteSalaryDelete = 545,

        /// <summary>
        /// Nhập loại xe cho công ty - Xem
        /// </summary>
        InputVehicleTypeView = 546,

        /// <summary>
        /// Nhập loại xe cho công ty - Tạo mới
        /// </summary>
        InputVehicleTypeAdd = 547,

        /// <summary>
        /// 	Nhập loại xe cho công ty - Sửa
        /// </summary>
        InputVehicleTypeUpdate = 548,

        /// <summary>
        /// Nhập loại xe cho công ty - Xóa
        /// </summary>
        InputVehicleTypeDelete = 549,

        /// <summary>
        /// Nhập thông tin khách hàng - Xem
        /// </summary>
        InputCustomerView = 550,

        /// <summary>
        /// Nhập thông tin khách hàng - Tạo mới
        /// </summary>
        InputCustomerAdd = 551,

        /// <summary>
        /// Nhập thông tin khách hàng - Sửa
        /// </summary>
        InputCustomerUpdate = 552,

        /// <summary>
        /// Nhập thông tin khách hàng - Xóa
        /// </summary>
        InputCustomerDelete = 553,

        /// <summary>
        /// Gán xe điểm lưu đêm - Xem
        /// </summary>
        InputVehicleNightCarparkView = 554,

        /// <summary>
        /// Gán xe điểm lưu đêm - Tạo mới
        /// </summary>
        InputVehicleNightCarparkAdd = 555,

        /// <summary>
        /// Gán xe điểm lưu đêm - Sửa
        /// </summary>
        InputVehicleNightCarparkUpdate = 556,

        /// <summary>
        /// Gán xe điểm lưu đêm - Xóa
        /// </summary>
        InputVehicleNightCarparkDelete = 557,

        /// <summary>
        /// Gán xe khách hàng - Xem
        /// </summary>
        InputCustomerVehicleView = 558,

        /// <summary>
        /// Gán xe khách hàng - Tạo mới
        /// </summary>
        InputCustomerVehicleAdd = 559,

        /// <summary>
        /// Gán xe khách hàng - Sửa
        /// </summary>
        InputCustomerVehicleUpdate = 560,

        /// <summary>
        /// Gán xe khách hàng - Xóa
        /// </summary>
        InputCustomerVehicleDelete = 561,

        /// <summary>
        /// Báo cáo xe mới lắp - Xem: 562
        /// </summary>
        ReportVehicleStatisticView = 562,

        /// <summary>
        /// Báo cáo xe mới lắp - Export: 563
        /// </summary>
        ReportVehicleStatisticExport = 563,

        /// <summary>
        /// Báo cáo xe mới lắp - Option: 564
        /// </summary>
        ReportVehicleStatisticOption = 564,

        /// <summary>
        /// Lịch hoạt động - Xem
        /// </summary>
        InputWorkingScheduleView = 565,

        /// <summary>
        /// Lịch hoạt động - Tạo mới
        /// </summary>
        InputWorkingScheduleAdd = 566,

        /// <summary>
        /// Lịch hoạt động - Sửa
        /// </summary>
        InputWorkingScheduleUpdate = 567,

        /// <summary>
        /// Lịch hoạt động - Xóa
        /// </summary>
        InputWorkingScheduleDelete = 568,

        /// <summary>
        /// Báo cáo nhiên liệu - Xem
        /// </summary>
        ReportFuelSummaryView = 569,

        /// <summary>
        /// Báo cáo nhiên liệu - Export
        /// </summary>
        ReportFuelSummaryExport = 570,

        /// <summary>
        /// Báo cáo nhiên liệu - Option
        /// </summary>
        ReportFuelSummaryOption = 571,

        /// <summary>
        /// Gán xe điểm lưu đêm - Xem
        /// </summary>
        InputVehicleVehicleTypeView = 572,

        /// <summary>
        /// Gán xe điểm lưu đêm - Tạo mới
        /// </summary>
        InputVehicleVehicleTypeAdd = 573,

        /// <summary>
        /// Gán xe điểm lưu đêm - Sửa
        /// </summary>
        InputVehicleVehicleTypeUpdate = 574,

        /// <summary>
        /// Gán xe điểm lưu đêm - Xóa
        /// </summary>
        InputVehicleVehicleTypeDelete = 575,

        /// <summary>
        /// Báo cáo chi tiết xe đưa rước công nhân - Xem
        /// </summary>
        ReportShuttleView = 576,

        /// <summary>
        /// Báo cáo chi tiết xe đưa rước công nhân - Export
        /// </summary>
        ReportShuttleExport = 577,

        /// <summary>
        /// Báo cáo chi tiết xe đưa rước công nhân - Option
        /// </summary>
        ReportShuttleOption = 578,

        /// <summary>
        /// Báo cáo tổng hợp xe chạy đưa rước công nhân - Xem
        /// </summary>
        ReportShuttleSummaryView = 579,

        /// <summary>
        /// Báo cáo tổng hợp xe chạy đưa rước công nhân - Export
        /// </summary>
        ReportShuttleSummaryExport = 580,

        /// <summary>
        /// Báo cáo tổng hợp xe chạy đưa rước công nhân - Option
        /// </summary>
        ReportShuttleSummaryOption = 581,

        /// <summary>
        /// Báo cáo tổng hợp tuyến xe chạy đưa rước công nhân - Xem
        /// </summary>
        ReportShuttleSummaryInRouteView = 582,

        /// <summary>
        /// Báo cáo tổng hợp tuyến xe chạy đưa rước công nhân - Export
        /// </summary>
        ReportShuttleSummaryInRouteExport = 583,

        /// <summary>
        /// Báo cáo tổng hợp tuyến xe chạy đưa rước công nhân - Option
        /// </summary>
        ReportShuttleSummaryInRouteOption = 584,

        /// <summary>
        /// Báo cáo số lượt theo xe - Xem
        /// </summary>
        ReportFlightByVehicleView = 585,

        /// <summary>
        /// Báo cáo số lượt theo xe - Export
        /// </summary>
        ReportFlightByVehicleExport = 586,

        /// <summary>
        /// 	Báo cáo số lượt theo xe - Option
        /// </summary>
        ReportFlightByVehicleOption = 587,

        /// <summary>
        /// Báo cáo số lượt theo lái xe - Xem
        /// </summary>
        ReportFlightByDriverView = 588,

        /// <summary>
        /// Báo cáo số lượt theo lái xe - Export
        /// </summary>
        ReportFlightByDriverExport = 589,

        /// <summary>
        /// Báo cáo số lượt theo lái xe - Option
        /// </summary>
        ReportFlightByDriverOption = 590,

        /// <summary>
        /// Báo cáo km hoạt động theo xe - Xem
        /// </summary>
        ReportKmByVehicleView = 591,

        /// <summary>
        /// Báo cáo km hoạt động theo xe - Export
        /// </summary>
        ReportKmByVehicleExport = 592,

        /// <summary>
        /// Báo cáo km hoạt động theo xe - Option

        /// </summary>
        ReportKmByVehicleOption = 593,

        /// <summary>
        /// Báo cáo km hoạt động theo lái xe - Xem
        /// </summary>
        ReportKmByDriverView = 594,

        /// <summary>
        /// Báo cáo km hoạt động theo lái xe - Export
        /// </summary>
        ReportKmByDriverExport = 595,

        /// <summary>
        /// Báo cáo km hoạt động theo lái xe - Option
        /// </summary>
        ReportKmByDriverOption = 596,

        /// <summary>
        /// Quản trị yêu cầu cung cấp dữ liệu - Xem
        /// </summary>
        ReportBGTCompanyInfoView = 597,

        /// <summary>
        /// Quản trị yêu cầu cung cấp dữ liệu - Export
        /// </summary>
        ReportBGTCompanyInfoExport = 598,

        /// <summary>
        /// Quản trị yêu cầu cung cấp dữ liệu - Option
        /// </summary>
        ReportBGTCompanyInfoOption = 599,

        /// <summary>
        /// Cấu hình tuyến - Xem
        /// </summary>
        InputRouteConfigView = 600,

        /// <summary>
        /// Cấu hình tuyến - Tạo mới
        /// </summary>
        InputRouteConfigAdd = 601,

        /// <summary>
        /// Cấu hình tuyến - Sửa
        /// </summary>
        InputRouteConfigUpdate = 602,

        /// <summary>
        /// Cấu hình tuyến - Xóa
        /// </summary>
        InputRouteConfigDelete = 603,

        /// <summary>
        /// Yêu cầu cung cấp dữ liệu - Xem
        /// </summary>
        InputCompanyInfoView = 604,

        /// <summary>
        /// Yêu cầu cung cấp dữ liệu - Tạo mới
        /// </summary>
        InputCompanyInfoAdd = 605,

        /// <summary>
        /// Yêu cầu cung cấp dữ liệu - Sửa
        /// </summary>
        InputCompanyInfoUpdate = 606,

        /// <summary>
        /// Yêu cầu cung cấp dữ liệu - Xóa
        /// </summary>
        InputCompanyInfoDelete = 607,

        /// <summary>
        /// Nhập lịch đưa rước - Xem
        /// </summary>
        InputShuttleScheduleView = 608,

        /// <summary>
        /// Nhập lịch đưa rước - Tạo mới
        /// </summary>
        InputShuttleScheduleAdd = 609,

        /// <summary>
        /// Nhập lịch đưa rước - Sửa
        /// </summary>
        InputShuttleScheduleUpdate = 610,

        /// <summary>
        /// Nhập lịch đưa rước - Xóa
        /// </summary>
        InputShuttleScheduleDelete = 611,

        /// <summary>
        /// Đo khoảng cách (Hoàng Long)
        /// </summary>
        MeasureDistance = 612,

        /// <summary>
        /// Nhập thông tin đăng kiểm - Xem
        /// </summary>
        InputRegisterView = 613,

        /// <summary>
        /// Nhập thông tin đăng kiểm - Tạo mới
        /// </summary>
        InputRegisterAdd = 614,

        /// <summary>
        /// Nhập thông tin đăng kiểm - Sửa
        /// </summary>
        InputRegisterUpdate = 615,

        /// <summary>
        /// Nhập thông tin đăng kiểm - Xóa
        /// </summary>
        InputRegisterDelete = 616,

        /// <summary>
        /// 	Điểm ảnh- Xem
        /// </summary>
        AdminLandmarkImageView = 617,

        /// <summary>
        /// Điểm ảnh - Tạo mới
        /// </summary>
        AdminLandmarkImageAdd = 618,

        /// <summary>
        /// Điểm ảnh - Sửa
        /// </summary>
        AdminLandmarkImageUpdate = 619,

        /// <summary>
        /// Điểm ảnh - Xóa
        /// </summary>
        AdminLandmarkImageDelete = 620,

        /// <summary>
        /// Báo cáo dừng đỗ - Xem
        /// </summary>
        BGTReportStopView = 621,

        /// <summary>
        /// Báo cáo dừng đỗ - Export
        /// </summary>
        BGTReportStopExport = 622,

        /// <summary>
        /// 	Báo cáo dừng đỗ  - Option
        /// </summary>
        BGTReportStopOption = 623,

        /// <summary>
        /// Báo cáo tổng hợp - Xem
        /// </summary>
        BGTActivitySummaryView = 624,

        /// <summary>
        /// Báo cáo tổng hợp - Export
        /// </summary>
        BGTActivitySummaryExport = 625,

        /// <summary>
        /// Báo cáo tổng hợp - Option
        /// </summary>
        BGTActivitySummaryOption = 626,

        /// <summary>
        /// Nhập biểu đồ ngang - Xem
        /// </summary>
        InputScheduleView = 627,

        /// <summary>
        /// Nhập biểu đồ ngang - Tạo mới
        /// </summary>
        InputScheduleAdd = 628,

        /// <summary>
        /// Nhập biểu đồ ngang - Sửa
        /// </summary>
        InputScheduleUpdate = 629,

        /// <summary>
        /// Nhập biểu đồ ngang - Xóa
        /// </summary>
        InputScheduleDelete = 630,

        /// <summary>
        /// Nhập biểu đồ ngang - Xem
        /// </summary>
        InputScheduleDailyView = 631,

        /// <summary>
        /// Nhập biểu đồ ngang - Tạo mới
        /// </summary>
        InputScheduleDailyAdd = 632,

        /// <summary>
        /// Nhập biểu đồ ngang - Sửa
        /// </summary>
        InputScheduleDailyUpdate = 633,

        /// <summary>
        /// Nhập biểu đồ ngang - Xóa
        /// </summary>
        InputScheduleDailyDelete = 634,

        /// <summary>
        /// Nhập lượt xác minh - Xem
        /// </summary>
        InputFlightVerifyView = 635,

        /// <summary>
        /// Nhập lượt xác minh - Tạo mới
        /// </summary>
        InputFlightVerifyAdd = 636,

        /// <summary>
        /// Nhập lượt xác minh - Sửa
        /// </summary>
        InputFlightVerifyUpdate = 637,

        /// <summary>
        /// Nhập lượt xác minh - Xóa
        /// </summary>
        InputFlightVerifyDelete = 638,

        /// <summary>
        /// Báo cáo chi tiết lượt thực hiện - Xem
        /// </summary>
        ReportBusFlightDetailView = 639,

        /// <summary>
        /// Báo cáo chi tiết lượt thực hiện - Export
        /// </summary>
        ReportBusFlightDetailExport = 640,

        /// <summary>
        /// Báo cáo chi tiết lượt thực hiện - Option
        /// </summary>
        ReportBusFlightDetailOption = 641,

        /// <summary>
        /// Báo cáo tổng hợp lượt thực hiện - Xem
        /// </summary>
        ReportBusFlightSummaryView = 642,

        /// <summary>
        /// Báo cáo tổng hợp lượt thực hiện - Export
        /// </summary>
        ReportBusFlightSummaryExport = 643,

        /// <summary>
        /// Báo cáo tổng hợp lượt thực hiện - Option
        /// </summary>
        ReportBusFlightSummaryOption = 644,

        /// <summary>
        /// Báo cáo tổng hợp Km - Xem
        /// </summary>
        ReportAccumulatedKmView = 645,

        /// <summary>
        /// Báo cáo tổng hợp Km - Export
        /// </summary>
        ReportAccumulatedKmExport = 646,

        /// <summary>
        /// Báo cáo tổng hợp Km - Option
        /// </summary>
        ReportAccumulatedKmOption = 647,

        /// <summary>
        /// Báo cáo tắt máy từ xa - Xem
        /// </summary>
        ReportRemoteMachineOffView = 648,

        /// <summary>
        /// Báo cáo tắt máy từ xa - Export
        /// </summary>
        ReportRemoteMachineOffExport = 649,

        /// <summary>
        /// Báo cáo tắt máy từ xa - Option
        /// </summary>
        ReportRemoteMachineOffOption = 650,

        /// <summary>
        /// Nhập thông tin đăng kiểm - Xem
        /// </summary>
        AdditionalKmView = 651,

        /// <summary>
        /// Nhập thông tin đăng kiểm - Tạo mới
        /// </summary>
        AdditionalKmAdd = 652,

        /// <summary>
        /// Nhập thông tin đăng kiểm - Sửa
        /// </summary>
        AdditionalKmUpdate = 653,

        /// <summary>
        /// Nhập thông tin đăng kiểm - Xóa
        /// </summary>
        AdditionalKmDelete = 654,

        /// <summary>
        /// Lấy thông tin xe BGT
        /// </summary>
        RmenuVehicleBGTView = 655,

        /// <summary>
        /// Báo cáo BGT theo QCVN31
        /// </summary>
        ReportDriverListView = 656,

        /// <summary>
        /// Báo cáo vi phạm thời gian lái xe (mới) - Xem
        /// </summary>
        ReportViolateDrivingTimeView = 657,

        /// <summary>
        /// Báo cáo vi phạm thời gian lái xe (mới) - Export
        /// </summary>
        ReportViolateDrivingTimeExport = 658,

        /// <summary>
        /// Báo cáo vi phạm thời gian lái xe (mới) - Option
        /// </summary>
        ReportViolateDrivingTimeOption = 659,

        /// <summary>
        /// Lịch hẹn - xem
        /// </summary>
        InputNoticeAppointmentView = 660,

        /// <summary>
        /// Lịch hẹn - tạo mới
        /// </summary>
        InputNoticeAppointmentAdd = 661,

        /// <summary>
        /// Lịch hẹn - sửa
        /// </summary>
        InputNoticeAppointmentUpdate = 662,

        /// <summary>
        /// Lịch hẹn - xóa
        /// </summary>
        InputNoticeAppointmentDelete = 663,

        /// <summary>
        /// Báo cáo thời gian lái xe trong ngày - xem
        /// </summary>
        BGTReportTimeOfDayView = 664,

        /// <summary>
        /// Báo cáo thời gian lái xe trong ngày - Export
        /// </summary>
        BGTReportTimeOfDayExport = 665,

        /// <summary>
        /// Báo cáo thời gian lái xe trong ngày - Option
        /// </summary>
        BGTReportTimeOfDayOption = 666,

        /// <summary>
        /// Báo cáo thống kê quá tốc độ - Xem
        /// </summary>
        BGTReportSpeedOverStatisticView = 667,

        /// <summary>
        /// Báo cáo thống kê quá tốc độ - Export
        /// </summary>
        BGTReportSpeedOverStatisticExport = 668,

        /// <summary>
        /// Báo cáo thống kê quá tốc độ - Option
        /// </summary>
        BGTReportSpeedOverStatisticOption = 669,

        /// <summary>
        /// Báo cáo cấu hình tốc độ giới hạn - Xem
        /// </summary>
        BGTReportVehicleSpeedConfigView = 670,

        /// <summary>
        /// Báo cáo cấu hình tốc độ giới hạn - Export
        /// </summary>
        BGTReportVehicleSpeedConfigExport = 671,

        /// <summary>
        /// Báo cáo cấu hình tốc độ giới hạn - Option
        /// </summary>
        BGTReportVehicleSpeedConfigOption = 672,

        /// <summary>
        /// Kiểm soát xe lắp mới - Xem
        /// </summary>
        InputApprovedVehicleView = 673,

        /// <summary>
        /// Kiểm soát xe lắp mới - Tạo mới
        /// </summary>
        InputApprovedVehicleAdd = 674,

        /// <summary>
        /// Kiểm soát xe lắp mới - Sửa
        /// </summary>
        InputApprovedVehicleUpdate = 675,

        /// <summary>
        /// Kiểm soát xe lắp mới - Xóa
        /// </summary>
        InputApprovedVehicleDelete = 676,

        /// <summary>
        /// Báo cáo kiểm soát xe lắp mới - Xem
        /// </summary>
        ReportApprovedVehicleView = 677,

        /// <summary>
        /// Báo cáo kiểm soát xe lắp mới - Export
        /// </summary>
        ReportApprovedVehicleExport = 678,

        /// <summary>
        /// Báo cáo kiểm soát xe lắp mới - Option
        /// </summary>
        ReportApprovedVehicleOption = 679,

        /// <summary>
        /// Báo cáo chi tiết bê tông - Xem
        /// </summary>
        ReportConcreteDetailView = 680,

        /// <summary>
        /// Báo cáo chi tiết bê tông - Export
        /// </summary>
        ReportConcreteDetailExport = 681,

        /// <summary>
        /// Báo cáo chi tiết bê tông - Export
        /// </summary>
        ReportConcreteDetailOption = 682,

        /// <summary>
        /// Danh mục bảo dưỡng - Xem
        /// </summary>
        InputMaintainCategoryView = 475,

        /// <summary>
        /// Danh mục bảo dưỡng - Tạo mới
        /// </summary>
        InputMaintainCategoryAdd = 476,

        /// <summary>
        /// Danh mục bảo dưỡng - Sửa
        /// </summary>
        InputMaintainCategoryUpdate = 477,

        /// <summary>
        /// Danh mục bảo dưỡng - Xóa
        /// </summary>
        InputMaintainCategoryDelete = 478,

        /// <summary>
        /// Nhập thông tin bảo dưỡng  - Xem
        /// </summary>
        InputMaintainInfoView = 481,

        /// <summary>
        /// Nhập thông tin bảo dưỡng  - Tạo mới
        /// </summary>
        InputMaintainInfoAdd = 482,

        /// <summary>
        /// Nhập thông tin bảo dưỡng  - Sửa
        /// </summary>
        InputMaintainInfoUpdate = 483,

        /// <summary>
        /// Nhập thông tin bảo dưỡng  - Xóa
        /// </summary>
        InputMaintainInfoDelete = 484,

        /// <summary>
        /// Nhập hồ sơ xe - Xem
        /// </summary>
        InputVehicleProfileView = 683,

        /// <summary>
        /// Nhập hồ sơ xe - Tạo mới
        /// </summary>
        InputVehicleProfileAdd = 684,

        /// <summary>
        /// Nhập hồ sơ xe - Sửa
        /// </summary>
        InputVehicleProfileUpdate = 685,

        /// <summary>
        /// Nhập hồ sơ xe - Xóa
        /// </summary>
        InputVehicleProfileDelete = 686,

        /// <summary>
        /// Nhập thông tin điểm đến - Xem
        /// </summary>
        InputTargetInfoView = 687,

        /// <summary>
        /// Nhập thông tin điểm đến - Tạo mới
        /// </summary>
        InputTargetInfoAdd = 688,

        /// <summary>
        /// Nhập thông tin điểm đến - Sửa
        /// </summary>
        InputTargetInfoUpdate = 689,

        /// <summary>
        /// Nhập thông tin điểm đến - Xóa
        /// </summary>
        InputTargetInfoDelete = 690,

        /// <summary>
        /// Báo cáo hồ sơ xe - Xem
        /// </summary>
        ReportVehicleProfileView = 691,

        /// <summary>
        /// Báo cáo hồ sơ xe - Export
        /// </summary>
        ReportVehicleProfileExport = 692,

        /// <summary>
        /// Báo cáo hồ sơ xe - Option
        /// </summary>
        ReportVehicleProfileOption = 693,

        /// <summary>
        /// Báo cáo tổng hợp (Thành công) - Xem
        /// </summary>
        ReportThanhCongActititySummaryView = 694,

        /// <summary>
        /// Báo cáo tổng hợp (Thành công) - Export
        /// </summary>
        ReportThanhCongActititySummaryExport = 695,

        /// <summary>
        /// Báo cáo tổng hợp (Thành công) - Option
        /// </summary>
        ReportThanhCongActititySummaryOption = 696,

        /// <summary>
        /// Báo cáo lịch trình tuyến - Option
        /// </summary>
        ReportScheduleRouteCompletedSearch = 697,

        /// <summary>
        /// Báo cáo tổng hợp lịch trình tuyến - Option
        /// </summary>
        ReportScheduleRouteCompletedSummarySearch = 698,

        /// <summary>
        /// Báo cáo tổng hợp lịch trình tuyến tháng- Option
        /// </summary>
        ReportScheduleRouteCompletedSummaryMonthSearch = 699,

        /// <summary>
        /// Báo cáo chuyến kinh doanh tổng hợp - Xem
        /// </summary>
        ReportBusinessTripSummaryView = 700,

        /// <summary>
        /// Báo cáo chuyến kinh doanh tổng hợp - Export
        /// </summary>
        ReportBusinessTripSummaryExport = 701,

        /// <summary>
        /// Báo cáo chuyến kinh doanh tổng hợp - Option
        /// </summary>
        ReportBusinessTripSummaryOption = 702,

        /// <summary>
        /// Báo cáo chuyến hàng bê tông - Xem
        /// </summary>
        ReportConcreteTripView = 703,

        /// <summary>
        /// Báo cáo chuyến hàng bê tông - Export
        /// </summary>
        ReportConcreteTripExport = 704,

        /// <summary>
        /// Báo cáo chuyến hàng bê tông - Option
        /// </summary>
        ReportConcreteTripOption = 705,

        /// <summary>
        /// Báo cáo tổng hợp kiểm soát - Xem
        /// </summary>
        ReportConcreteTripSummaryView = 706,

        /// <summary>
        /// Báo cáo tổng hợp kiểm soát - Export
        /// </summary>
        ReportConcreteTripSummaryExport = 707,

        /// <summary>
        /// Báo cáo tổng hợp kiểm soát - Option
        /// </summary>
        ReportConcreteTripSummaryOption = 708,

        /// <summary>
        /// Cảnh báo quá vận tốc online - Xem
        /// </summary>
        AdminWarningView = 709,

        /// <summary>
        /// Báo cáo dừng đỗ theo quận huyện - Xem
        /// </summary>
        ReportStopDistrictView = 710,

        /// <summary>
        /// Báo cáo dừng đỗ theo quận huyện - Export
        /// </summary>
        ReportStopDistrictExport = 711,

        /// <summary>
        /// Báo cáo dừng đỗ theo quận huyện - Option
        /// </summary>
        ReportStopDistrictOption = 712,

        /// <summary>
        /// Báo cáo chuyến theo lái xe - Xem
        /// </summary>
        ReportEmployeeBusinessTripView = 713,

        /// <summary>
        /// Báo cáo chuyến theo lái xe - Export
        /// </summary>
        ReportEmployeeBusinessTripExport = 714,

        /// <summary>
        /// Báo cáo chuyến theo lái xe - Option
        /// </summary>
        ReportEmployeeBusinessTripOption = 715,

        /// <summary>
        /// Có ẩn KM GPS hay không?
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  7/1/2014   created
        /// </Modified>
        HideKmGPS = 716,

        /// <summary>
        /// Có quyền xem bảo dưỡng - Xem
        /// </summary>
        AdminMaintainView = 717,

        /// <summary>
        /// Báo cáo chi tiết chuyến lượt - Xem
        /// </summary>
        CitrancoFlightDetailView = 718,

        /// <summary>
        /// Báo cáo chi tiết chuyến lượt - Export
        /// </summary>
        CitrancoFlightDetailExport = 719,

        /// <summary>
        /// Báo cáo chi tiết chuyến lượt - Option
        /// </summary>
        CitrancoFlightDetailOption = 720,

        /// <summary>
        /// Báo cáo tổng hợp chuyến lượt - Xem
        /// </summary>
        CitrancoFlightSummaryView = 721,

        /// <summary>
        /// Báo cáo tổng hợp chuyến lượt - Export
        /// </summary>
        CitrancoFlightSummaryExport = 722,

        /// <summary>
        /// Báo cáo tổng hợp chuyến lượt - Option
        /// </summary>
        CitrancoFlightSummaryOption = 723,

        /// <summary>
        /// Báo cáo sản lượng theo tuyến - Xem
        /// </summary>
        CitrancoCapacitySummaryView = 724,

        /// <summary>
        /// Báo cáo sản lượng theo tuyến - Export
        /// </summary>
        CitrancoCapacitySummaryExport = 725,

        /// <summary>
        /// Báo cáo sản lượng theo tuyến - Option
        /// </summary>
        CitrancoCapacitySummaryOption = 726,

        /// <summary>
        /// Báo cáo tắt điều hòa - Xem
        /// </summary>
        CitrancoAirConditionerView = 727,

        /// <summary>
        /// Báo cáo tắt điều hòa - Export
        /// </summary>
        CitrancoAirConditionerExport = 728,

        /// <summary>
        /// Báo cáo tắt điều hòa  - Option
        /// </summary>
        CitrancoAirConditionerOption = 729,

        /// <summary>
        /// Báo cáo quá tốc độ - Xem
        /// </summary>
        CitrancoSpeedOverView = 730,

        /// <summary>
        /// Báo cáo quá tốc độ - Export
        /// </summary>
        CitrancoSpeedOverExport = 731,

        /// <summary>
        /// Báo cáo quá tốc độ - Option
        /// </summary>
        CitrancoSpeedOverOption = 732,

        /// <summary>
        /// Ẩn hiện cột thời gian vi phạm trong báo cáo quá tốc BGT
        /// </summary>
        BGTReportSpeedOverVisibleTotalTime = 733,

        /// <summary>
        /// Báo cáo lịch hẹn - Xem
        /// </summary>
        ReportAppointmentView = 734,

        /// <summary>
        /// Báo cáo lịch hẹn - Export
        /// </summary>
        ReportAppointmentExport = 735,

        /// <summary>
        /// Báo cáo lịch hẹn - Option
        /// </summary>
        ReportAppointmentOption = 736,

        /// <summary>
        /// QCVN31 - Báo cáo quá tốc độ - Xem
        /// </summary>
        BGTReportSpeedOverConfigView = 737,

        /// <summary>
        /// QCVN31 - Báo cáo quá tốc độ - Export
        /// </summary>
        BGTReportSpeedOverConfigExport = 738,

        /// <summary>
        /// QCVN31 - Báo cáo quá tốc độ - Option
        /// </summary>
        BGTReportSpeedOverConfigOption = 739,

        /// <summary>
        /// Báo cáo tiêu hao nhiên liệu - Xem
        /// </summary>
        FuelConsumptionDailyView = 740,

        /// <summary>
        /// Báo cáo tiêu hao nhiên liệu - Export
        /// </summary>
        FuelConsumptionDailyExport = 741,

        /// <summary>
        /// Báo cáo tiêu hao nhiên liệu - Option
        /// </summary>
        FuelConsumptionDailyOption = 742,

        /// <summary>
        /// Báo cáo tổng hợp cho sở An Giang - View
        /// </summary>
        BGTActivitySummaryAnGiangView = 743,

        /// <summary>
        /// Báo cáo tổng hợp cho sở An Giang - Export
        /// </summary>
        BGTActivitySummaryAnGiangExport = 744,

        /// <summary>
        /// Báo cáo tổng hợp cho sở An Giang - Option
        /// </summary>
        BGTActivitySummaryAnGiangOption = 745,

        /// <summary>
        /// Báo cáo tổng hợp Block 30 - Xem
        /// </summary>
        BGTActivitySummaryBlock30View = 746,

        /// <summary>
        /// Báo cáo tổng hợp Block 30 - Export
        /// </summary>
        BGTActivitySummaryBlock30Export = 747,

        /// <summary>
        /// Báo cáo tổng hợp Block 30 - Option
        /// </summary>
        BGTActivitySummaryBlock30Option = 748,

        /// <summary>
        /// Cảnh báo xe chưa đến điểm - Sửa
        /// </summary>
        DestinationAlertView = 750,

        /// <summary>
        /// Cảnh báo xe chưa đến điểm - Thêm mới
        /// </summary>
        DestinationAlertAdd = 751,

        /// <summary>
        /// Cảnh báo xe chưa đến điểm - Sửa
        /// </summary>
        DestinationAlertUpdate = 752,

        /// <summary>
        /// Cảnh báo xe chưa đến điểm - Xóa
        /// </summary>
        DestinationAlertDelete = 753,

        /// <summary>
        /// Báo cáo chi tiết cuốc khách - Xem
        /// </summary>
        ReportTaxiTripDetailView = 754,

        /// <summary>
        /// Báo cáo chi tiết cuốc khách - Export
        /// </summary>
        ReportTaxiTripDetailExport = 755,

        /// <summary>
        /// Báo cáo chi tiết cuốc khách - Option
        /// </summary>
        ReportTaxiTripDetailOption = 756,

        /// <summary>
        /// Báo cáo hành trình (sở Điện Biên) - Xem
        /// </summary>
        BGTReportItineraryView = 757,

        /// <summary>
        /// Báo cáo hành trình (sở Điện Biên) - Export
        /// </summary>
        BGTReportItineraryExport = 758,

        /// <summary>
        /// Báo cáo hành trình (sở Điện Biên) - Option
        /// </summary>
        BGTReportItineraryOption = 759,

        /// <summary>
        /// TCĐBVN - Xem
        /// </summary>
        AdminTCDBVNView = 760,

        /// <summary>
        /// Đixe.vn - Xem
        /// </summary>
        AdminDiXeView = 761,

        /// <summary>
        /// Báo cáo thống kê truyền dữ liệu - Xem
        /// </summary>
        VehicleSendDataView = 762,

        /// <summary>
        /// Báo cáo thống kê truyền dữ liệu - Export
        /// </summary>
        VehicleSendDataExport = 763,

        /// <summary>
        /// Báo cáo thống kê truyền dữ liệu - Option
        /// </summary>
        VehicleSendDataOption = 764,

        /// <summary>
        /// Quản lý thông tin công ty - Xem
        /// </summary>
        DiXeCompanyManagementView = 765,

        /// <summary>
        /// Quản lý thông tin công ty - Thêm mới
        /// </summary>
        DiXeCompanyManagementAdd = 766,

        /// <summary>
        /// Quản lý thông tin công ty  - Sửa
        /// </summary>
        DiXeCompanyManagementUpdate = 767,

        /// <summary>
        /// Quản lý thông tin công ty - Xóa
        /// </summary>
        DiXeCompanyManagementDelete = 768,

        /// <summary>
        /// Quản lý duyệt thông tin công ty
        /// </summary>
        DiXeCompanyApproveView = 769,

        /// <summary>
        /// Quản lý duyệt thông tin công ty - Thêm mới
        /// </summary>
        DiXeCompanyApproveAdd = 770,

        /// <summary>
        /// Quản lý duyệt thông tin công ty  - Sửa
        /// </summary>
        DiXeCompanyApproveUpdae = 771,

        /// <summary>
        /// Quản lý duyệt thông tin công ty - Xóa
        /// </summary>
        DiXeCompanyApproveDelete = 772,

        /// <summary>
        /// Điều khoản tham gia Đixe.vn - Xem
        /// </summary>
        DiXeCompanyJoinTermView = 773,

        /// <summary>
        /// Báo cáo thông tin chạy lại - View
        /// </summary>
        VehicleUpdateDataView = 774,

        /// <summary>
        /// Báo cáo thông tin chạy lại - Export
        /// </summary>
        VehicleUpdateDataExport = 775,

        /// <summary>
        /// Báo cáo thông tin chạy lại - Option
        /// </summary>
        VehicleUpdateDataOption = 776,

        /// <summary>
        /// Khach hang cua nhan vien kinh doanh
        /// </summary>
        SalePersonCustomer = 777,

        /// <summary>
        /// Nhập thông tin đăng kiểm - Xem
        /// </summary>
        InputRelatedInfoView = 778,

        /// <summary>
        /// Nhập thông tin đăng kiểm - Tạo mới
        /// </summary>
        InputRelatedInfoAdd = 779,

        /// <summary>
        /// Nhập thông tin đăng kiểm - Sửa
        /// </summary>
        InputRelatedInfoUpdate = 780,

        /// <summary>
        /// Nhập thông tin đăng kiểm - Xóa
        /// </summary>
        InputRelatedInfoDelete = 781,

        /// <summary>
        /// Nhập hạn giấy phép vào phố cấm - Xem
        /// </summary>
        VehicleRelatedInfoView = 778,

        /// <summary>
        /// Nhập hạn giấy phép vào phố cấm - Tạo mới
        /// </summary>
        VehicleRelatedInfoAdd = 779,

        /// <summary>
        /// Nhập hạn giấy phép vào phố cấm - Cập nhật
        /// </summary>
        VehicleRelatedInfoUpdate = 780,

        /// <summary>
        /// Nhập hạn giấy phép vào phố cấm - Xóa
        /// </summary>
        VehicleRelatedInfoDelete = 781,

        /// <summary>
        /// Báo cáo tắt điều hòa - Xem
        /// </summary>
        HikariAirConditionerView = 782,

        /// <summary>
        /// Báo cáo tắt điều hòa - Export
        /// </summary>
        HikariAirConditionerExport = 783,

        /// <summary>
        /// Báo cáo tắt điều hòa  - Option
        /// </summary>
        HikariAirConditionerOption = 784,

        /// <summary>
        /// Báo cáo tổng kết tắt điều hòa - Xem
        /// </summary>
        HikariAirConditionerSummaryView = 785,

        /// <summary>
        /// Báo cáo tổng kết tắt điều hòa - Export
        /// </summary>
        HikariAirConditionerSummaryExport = 786,

        /// <summary>
        /// Báo cáo tổng kết tắt điều hòa  - Option
        /// </summary>
        HikariAirConditionerSummaryOption = 787,

        /// <summary>
        /// Xem nhiều ảnh Camera - Xem
        /// </summary>
        AdminUtilityMultipleImageView = 788,

        /// <summary>
        /// Cảnh báo sim tiền online - Xem
        /// </summary>
        SimMoneyAlertView = 789,

        /// <summary>
        /// Báo cáo tổng hợp tiêu hao nhiên liệu - Xem
        /// </summary>
        FuelConsumptionSummaryView = 790,

        /// <summary>
        /// Báo cáo tổng hợp tiêu hao nhiên liệu - Export
        /// </summary>
        FuelConsumptionSummaryExport = 791,

        /// <summary>
        /// Báo cáo tổng hợp tiêu hao nhiên liệu - Option
        /// </summary>
        FuelConsumptionSummaryOption = 792,

        /// <summary>
        /// Nhập biểu đồ dọc - Xem
        /// </summary>
        InputVerticalScheduleView = 793,

        /// <summary>
        /// Nhập biểu đồ dọc - Tạo mới
        /// </summary>
        InputVerticalScheduleAdd = 794,

        /// <summary>
        /// Nhập biểu đồ dọc - Sửa
        /// </summary>
        InputVerticalScheduleUpdate = 795,

        /// <summary>
        /// Nhập biểu đồ dọc - Xóa
        /// </summary>
        InputVerticalScheduleDelete = 796,

        /// <summary>
        /// Nhập loại bảo hiểm - Xem
        /// </summary>
        InputInsuranceCategoryView = 797,

        /// <summary>
        /// Nhập loại bảo hiểm - Tạo mới
        /// </summary>
        InputInsuranceCategoryAdd = 798,

        /// <summary>
        /// Nhập loại bảo hiểm - Sửa
        /// </summary>
        InputInsuranceCategoryUpdate = 799,

        /// <summary>
        /// Nhập loại bảo hiểm - Xóa
        /// </summary>
        InputInsuranceCategoryDelete = 800,

        /// <summary>
        /// Nhập thông tin bảo hiểm - Xem
        /// </summary>
        InputInsuranceInfoView = 801,

        /// <summary>
        /// Nhập thông tin bảo hiểm - Tạo mới
        /// </summary>
        InputInsuranceInfoAdd = 802,

        /// <summary>
        /// Nhập thông tin bảo hiểm - Sửa
        /// </summary>
        InputInsuranceInfoUpdate = 803,

        /// <summary>
        /// Nhập thông tin bảo hiểm - Xóa
        /// </summary>
        InputInsuranceInfoDelete = 804,

        /// <summary>
        /// Nhập thông tin tem hợp đồng - Xem
        /// </summary>
        InputStampContractInfoView = 805,

        /// <summary>
        /// Nhập thông tin tem hợp đồng - Tạo mới
        /// </summary>
        InputStampContractInfoAdd = 806,

        /// <summary>
        /// Nhập thông tin tem hợp đồng - Sửa
        /// </summary>
        InputStampContractInfoUpdate = 807,

        /// <summary>
        /// Nhập thông tin tem hợp đồng - Xóa
        /// </summary>
        InputStampContractInfoDelete = 808,

        /// <summary>
        /// Báo cáo chi tiết thời gian ngoài giờ - Xem
        /// </summary>
        ReportOverTimeView = 809,

        /// <summary>
        /// Báo cáo chi tiết thời gian ngoài giờ - Export
        /// </summary>
        ReportOverTimeExport = 810,

        /// <summary>
        /// Nhập cấu hình thời gian ngoài giờ - Xem
        /// </summary>
        InputOverTimeConfigView = 811,

        /// <summary>
        /// Nhập cấu hình thời gian ngoài giờ - Tạo mới
        /// </summary>
        InputOverTimeConfigAdd = 812,

        /// <summary>
        /// Nhập cấu hình thời gian ngoài giờ - Sửa
        /// </summary>
        InputOverTimeConfigUpdate = 813,

        /// <summary>
        /// Nhập cấu hình thời gian ngoài giờ - Xóa
        /// </summary>
        InputOverTimeConfigDelete = 814,

        /// <summary>
        /// Gán người dùng: 815
        /// </summary>
        AssignAccountantBussiness = 815,

        /// <summary>
        /// Nhật kí sim tiền: 816
        /// </summary>
        SimHistoryView = 816,

        /// <summary>
        /// Gan lai xe: 817
        /// </summary>
        AssignVehicleView = 817,

        /// <summary>
        /// Cau hinh xe: 818
        /// </summary>
        VehicleConfigurationView = 818,

        /// <summary>
        /// Quan ly nhom xe: 819
        /// </summary>
        VehicleGroupView = 819,

        /// <summary>
        /// Phan quyen xe: 820
        /// </summary>
        PermissionUserVehicleGroupView = 820,

        /// <summary>
        /// Báo cáo TG làm việc của lái xe theo tháng  - Xem
        /// </summary>
        BGTReportDrivingTimeJourneySummaryView = 821,

        /// <summary>
        /// Báo cáo TG làm việc của lái xe theo tháng - Export
        /// </summary>
        BGTReportDrivingTimeJourneySummaryExport = 822,

        /// <summary>
        /// Báo cáo tổng hợp theo tháng cho sở An Giang - View
        /// </summary>
        BGTActivitySummaryAnGiangViewSummary = 823,

        /// <summary>
        /// Báo cáo tổng hợp theo tháng cho sở An Giang - Export
        /// </summary>
        BGTActivitySummaryAnGiangSummaryExport = 824,

        /// <summary>
        /// Báo cáo hoạt động xe máy - Xem
        /// </summary>
        FuelConsumptionView = 825,

        /// <summary>
        /// Báo cáo hoạt động xe máy - Export
        /// </summary>
        FuelConsumptionExport = 826,

        /// <summary>
        /// Báo cáo chi tiết đổ bê tông - Xem
        /// </summary>
        ReportConcreteDumpDetailView = 827,

        /// <summary>
        /// Báo cáo chi tiết đổ bê tông - Export
        /// </summary>
        ReportConcreteDumpDetailExport = 828,

        /// <summary>
        /// Báo cáo chi tiết đổ bê tông - Option
        /// </summary>
        ReportConcreteDumpDetailOption = 829,

        /// <summary>
        /// Quản trị phí dịch vụ - Xem
        /// </summary>
        VehicleChargedFeeView = 830,

        /// <summary>
        /// Hướng dẫn trả phí - Xem
        /// </summary>
        PayingFeeGuideView = 831,

        /// <summary>
        /// Điều khoản sử dụng - Xem
        /// </summary>
        TermsAndConditionsView = 832,

        /// <summary>
        /// Nhập phí quá trạm - Xem
        /// </summary>
        InputChargingStationFeeView = 833,

        /// <summary>
        /// Nhập phí quá trạm - Tạo mới
        /// </summary>
        InputChargingStationFeeAdd = 834,

        /// <summary>
        /// Nhập phí quá trạm - Sửa
        /// </summary>
        InputChargingStationFeeUpdate = 835,

        /// <summary>
        /// Nhập phí quá trạm - Xóa
        /// </summary>
        InputChargingStationFeeDelete = 836,

        /// <summary>
        /// Báo cáo cước qua điểm thu phí - Xem: 837
        /// </summary>
        ReportChargeStationFeeView = 837,

        /// <summary>
        /// Báo cáo cước qua điểm thu phí - Export: 838
        /// </summary>
        ReportChargeStationFeeExport = 838,

        /// <summary>
        /// Báo cáo xe dững đỗ tại điểm dừng - Xem
        /// </summary>
        ReportBusStationStopView = 839,

        /// <summary>
        /// Báo cáo xe dững đỗ tại điểm dừng - Export
        /// </summary>
        ReportBusStationStopExport = 840,

        /// <summary>
        /// Báo cáo xe dững đỗ tại điểm dừng  - Option
        /// </summary>
        ReportBusStationStopOption = 841,

        /// <summary>
        /// Báo cáo qua điểm chốt - Xem
        /// </summary>
        ReportBusBenchmarkView = 842,

        /// <summary>
        /// Báo cáo qua điểm chốt - Export
        /// </summary>
        ReportBusBenchmarkExport = 843,

        /// <summary>
        /// Báo cáo qua điểm chốt - Option
        /// </summary>
        ReportBusBenchmarkOption = 844,

        /// <summary>
        /// Nhập thời gian quy định đến điểm dừng - Xem
        /// </summary>
        InputScheduleTimeStationView = 845,

        /// <summary>
        /// Nhập thời gian quy định đến điểm dừng - Tạo mới
        /// </summary>
        InputScheduleTimeStationAdd = 846,

        /// <summary>
        /// Nhập thời gian quy định đến điểm dừng - Sửa
        /// </summary>
        InputScheduleTimeStationUpdate = 847,

        /// <summary>
        /// Nhập thời gian quy định đến điểm dừng - Xóa
        /// </summary>
        InputScheduleTimeStationDelete = 848,

        /// <summary>
        /// Báo cáo hành trình xe chạy - Xem
        /// </summary>
        ReportQCVN31ItineraryView = 849,

        /// <summary>
        /// Báo cáo hành trình xe chạy - Export
        /// </summary>
        ReportQCVN31ItineraryExport = 850,

        /// <summary>
        /// Báo cáo hành trình xe chạy - Option
        /// </summary>
        ReportQCVN31ItineraryOption = 851,

        /// <summary>
        /// Báo cáo thời gian lái xe liên tục - Xem
        /// </summary>
        ReportQCVN31DrivingContinuousView = 852,

        /// <summary>
        /// Báo cáo thời gian lái xe liên tục - Export
        /// </summary>
        ReportQCVN31DrivingContinuousExport = 853,

        /// <summary>
        /// Báo cáo thời gian lái xe liên tục - Option
        /// </summary>
        ReportQCVN31DrivingContinuousOption = 854,

        /// <summary>
        /// Báo cáo quá tốc độ giới hạn - Xem
        /// </summary>
        ReportQCVN31SpeedOverView = 855,

        /// <summary>
        /// Báo cáo quá tốc độ giới hạn - Export
        /// </summary>
        ReportQCVN31SpeedOverExport = 856,

        /// <summary>
        /// Báo cáo quá tốc độ giới hạn - Option
        /// </summary>
        ReportQCVN31SpeedOverOption = 857,

        /// <summary>
        /// Báo cáo tốc độ của xe - Xem
        /// </summary>
        ReportQCVN31SpeedView = 858,

        /// <summary>
        /// Báo cáo tốc độ của xe - Export
        /// </summary>
        ReportQCVN31SpeedExport = 859,

        /// <summary>
        /// Báo cáo tốc độ của xe - Option
        /// </summary>
        ReportQCVN31SpeedOption = 860,

        /// <summary>
        /// Báo cáo tổng hợp theo xe - Xem
        /// </summary>
        ReportQCVN31VehicleGeneralView = 861,

        /// <summary>
        /// Báo cáo tổng hợp theo xe - Export
        /// </summary>
        ReportQCVN31VehicleGeneralExport = 862,

        /// <summary>
        /// Báo cáo tổng hợp theo xe - Option
        /// </summary>
        ReportQCVN31VehicleGeneralOption = 863,

        /// <summary>
        /// Báo cáo tổng hợp theo lái xe - Xem
        /// </summary>
        ReportQCVN31DriverGeneralView = 864,

        /// <summary>
        /// Báo cáo tổng hợp theo lái xe - Export
        /// </summary>
        ReportQCVN31DriverGeneralExport = 865,

        /// <summary>
        /// Báo cáo tổng hợp theo lái xe - Option
        /// </summary>
        ReportQCVN31DriverGeneralOption = 866,

        /// <summary>
        /// Báo cáo dừng đỗ - Xem
        /// </summary>
        ReportQCVN31StopView = 867,

        /// <summary>
        /// Báo cáo dừng đỗ - Export
        /// </summary>
        ReportQCVN31StopExport = 868,

        /// <summary>
        /// Báo cáo dừng đỗ - Option
        /// </summary>
        ReportQCVN31StopOption = 869,

        /// <summary>
        /// Báo cáo mất tín hiệu Online - Xem
        /// </summary>
        SignalLossOnlineView = 870,

        /// <summary>
        /// Báo cáo tổng hợp mất tín hiệu - Xem
        /// </summary>
        SignalLossSummaryView = 871,

        /// <summary>
        /// Báo cáo hành trình (dành cho Hikari)- Xem
        /// </summary>
        ReportItineraryHikari2015View = 872,

        /// <summary>
        /// Hiện toàn bộ danh sách xe
        /// </summary>
        ListVehicleShowAll = 873,

        /// <summary>
        /// Hiện toàn bộ danh sách công ty
        /// </summary>
        ListCompanyShowAll = 874,

        /// <summary>
        /// Kiểm soát xe trùng IMEI - Xem
        /// </summary>
        InputDuplicatedIMEIVehicleView = 875,

        /// <summary>
        /// Kiểm soát xe trùng IMEI - Thêm
        /// </summary>
        InputDuplicatedIMEIVehicleAdd = 876,

        /// <summary>
        /// Kiểm soát xe trùng IMEI - Sửa
        /// </summary>
        InputDuplicatedIMEIVehicleUpdate = 877,

        /// <summary>
        /// Kiểm soát xe trùng IMEI - Xóa
        /// </summary>
        InputDuplicatedIMEIVehicleDelete = 878,

        /// <summary>
        /// Báo cáo tiêu hao nhiên liệu tự nhập theo ngày - Xem
        /// </summary>
        FuelConsumptionDailyManualView = 879,

        /// <summary>
        /// Báo cáo tiêu hao nhiên liệu tự nhập theo ngày - Export
        /// </summary>
        FuelConsumptionDailyManualExport = 880,

        /// <summary>
        /// Báo cáo tiêu hao nhiên liệu tự nhập theo ngày - Option
        /// </summary>
        FuelConsumptionDailyManualOption = 881,

        /// <summary>
        /// Form kế toán công ty
        /// </summary>
        AccountantGeneralView = 882,

        /// <summary>
        /// Báo cáo tốc độ của xe - Xem
        /// </summary>
        OnOffCameraConfigurationView = 883,

        /// <summary>
        /// Báo cáo tốc độ của xe - Export
        /// </summary>
        OnOffCameraConfigurationExport = 884,

        /// <summary>
        /// Báo cáo tốc độ của xe - Option
        /// </summary>
        OnOffCameraConfigurationOption = 885,

        /// <summary>
        /// Danh sách công ty (CSKH) - Xem
        /// </summary>
        ListCompaniesCustomerCareView = 886,

        /// <summary>
        /// Danh sách công ty (CSKH) - Export
        /// </summary>
        ListCompaniesCustomerCareExport = 887,

        /// <summary>
        /// Danh sách công ty (CSKH) - Option
        /// </summary>
        ListCompaniesCustomerCareOption = 888,

        /// <summary>
        /// Danh sách phương tiện đã lắp đặt thiết bị theo Quy chuẩn BGT - Xem
        /// </summary>
        CompaniesVehiclesDeviceTypeSummaryBGTView = 889,

        /// <summary>
        /// Danh sách phương tiện đã lắp đặt thiết bị theo Quy chuẩn BGT - Export
        /// </summary>
        CompaniesVehiclesDeviceTypeSummaryBGTExport = 890,

        /// <summary>
        /// Danh sách phương tiện đã lắp đặt thiết bị theo Quy chuẩn BGT - Option
        /// </summary>
        CompaniesVehiclesDeviceTypeSummaryBGTOption = 891,

        /// <summary>
        /// Báo cáo nhiệt độ - Xem
        /// </summary>
        ReportTemperatureView = 892,

        /// <summary>
        /// Báo cáo nhiệt độ - Export
        /// </summary>
        ReportTemperatureExport = 893,

        /// <summary>
        /// Báo cáo nhiệt độ - Option
        /// </summary>
        ReportTemperatureOption = 894,

        /// <summary>
        /// Báo cáo chi tiết khả nghi các lỗi cố ý mất nguồn thiết bị - Xem
        /// </summary>
        LosingPowerDetailsView = 895,

        /// <summary>
        /// Báo cáo chi tiết khả nghi các lỗi cố ý mất nguồn thiết bị - Export
        /// </summary>
        LosingPowerDetailsExport = 896,

        /// <summary>
        /// Báo cáo chi tiết khả nghi các lỗi cố ý mất nguồn thiết bị - Option
        /// </summary>
        LosingPowerDetailsOption = 897,

        /// <summary>
        /// Báo cáo tổng hợp khả nghi các lỗi cố ý mất nguồn thiết bị - Xem
        /// </summary>
        LosingPowerSummariesView = 898,

        /// <summary>
        /// Báo cáo tổng hợp khả nghi các lỗi cố ý mất nguồn thiết bị - Export
        /// </summary>
        LosingPowerSummariesExport = 899,

        /// <summary>
        /// Báo cáo tổng hợp khả nghi các lỗi cố ý mất nguồn thiết bị - Option
        /// </summary>
        LosingPowerSummariesOption = 900,

        /// <summary>
        /// Danh mục khả nghi các lỗi cố ý mất nguồn thiết bị - Xem
        /// </summary>
        LosingPowerCategoriesView = 901,

        /// <summary>
        /// Danh mục tổng hợp khả nghi các lỗi cố ý mất nguồn thiết bị - Export
        /// </summary>
        LosingPowerCategoriesExport = 902,

        /// <summary>
        /// Danh mục tổng hợp khả nghi các lỗi cố ý mất nguồn thiết bị - Option
        /// </summary>
        LosingPowerCategoriesOption = 903,

        /// <summary>
        /// Gán NVKD cho CSKH: 815
        /// </summary>
        AssignSupporterBussiness = 904,

        /// <summary>
        /// Đẩy xe lên online - Xem
        /// </summary>
        VehicleForceOnlineView = 905,

        /// <summary>
        /// Đẩy xe lên online - Export
        /// </summary>
        VehicleForceOnlineExport = 906,

        /// <summary>
        /// Đẩy xe lên online - Option
        /// </summary>
        VehicleForceOnlineOption = 907,

        /// <summary>
        /// Chạy lại dữ liệu - Xem
        /// </summary>
        RequestRerunView = 908,

        /// <summary>
        /// Chạy lại dữ liệu - Export
        /// </summary>
        RequestRerunExport = 909,

        /// <summary>
        /// Chạy lại dữ liệu - Option
        /// </summary>
        RequestRerunOption = 910,

        /// <summary>
        /// Báo cáo 3 điểm - Xem
        /// </summary>
        ReportScheduleRoute3PointsView = 911,

        /// <summary>
        /// Báo cáo 3 điểm - Export
        /// </summary>
        ReportScheduleRoute3PointsExport = 912,

        /// <summary>
        /// Báo cáo 3 điểm - Option
        /// </summary>
        ReportScheduleRoute3PointsOption = 913,

        //--------------------------------------

        /// <summary>
        /// Danh mục giấy tờ - Xem
        /// </summary>
        InputDocumentCategoriesView = 914,

        /// <summary>
        /// Danh mục giấy tờ - Tạo mới
        /// </summary>
        InputDocumentCategoriesAdd = 915,

        /// <summary>
        /// Danh mục giấy tờ - Sửa
        /// </summary>
        InputDocumentCategoriesUpdate = 916,

        /// <summary>
        /// Danh mục giấy tờ - Xóa
        /// </summary>
        InputDocumentCategoriesDelete = 917,

        /// <summary>
        /// Danh mục giấy tờ - Xem
        /// </summary>
        InputDocumentDetailView = 918,

        /// <summary>
        /// Danh mục giấy tờ - Tạo mới
        /// </summary>
        InputDocumentDetailAdd = 919,

        /// <summary>
        /// Danh mục giấy tờ - Sửa
        /// </summary>
        InputDocumentDetailUpdate = 920,

        /// <summary>
        /// Danh mục giấy tờ - Xóa
        /// </summary>
        InputDocumentDetailDelete = 921,

        /// <summary>
        /// Báo cáo lưu lượng - Xem
        /// </summary>
        ReportFlowView = 922,

        /// <summary>
        /// Báo cáo lưu lượng - Export
        /// </summary>
        ReportFlowExport = 923,

        /// <summary>
        /// Báo cáo lưu lượng - Option
        /// </summary>
        ReportFlowOption = 924,

        /// <summary>
        /// Báo cáo lưu lượng theo ngày - Xem
        /// </summary>
        ReportFlowDailyView = 925,

        /// <summary>
        /// Báo cáo lưu lượng theo ngày - Export
        /// </summary>
        ReportFlowDailyExport = 926,

        /// <summary>
        /// Báo cáo lưu lượng theo ngày - Option
        /// </summary>
        ReportFlowDailyOption = 927,

        /// <summary>
        /// Báo cáo bơm bê tông - Xem
        /// </summary>
        ReportConcretePumpView = 928,

        /// <summary>
        /// Báo cáo bơm bê tông - Export
        /// </summary>
        ReportConcretePumpExport = 929,

        /// <summary>
        /// Báo cáo bơm bê tông - Option
        /// </summary>
        ReportConcretePumpOption = 930,

        /// <summary>
        /// Xem tất cả nhân viên chăm sóc khách hàng trên trang duyệt xe
        /// </summary>
        ReportApprovedVehicleViewAllSupporter = 931,

        /// <summary>
        /// Báo cáo quá tốc độ OBC - Xem
        /// </summary>
        ObcSpeedOverView = 932,

        /// <summary>
        /// Báo cáo quá tốc độ OBC - Export
        /// </summary>
        ObcSpeedOverExport = 933,

        /// <summary>
        /// Báo cáo quá tốc độ OBC - Option
        /// </summary>
        ObcSpeedOverOption = 934,

        /// <summary>
        /// Báo cáo thời gian lái xe trong tuần OBC - Xem
        /// </summary>
        ObcDrivingTimeInWeekView = 935,

        /// <summary>
        /// Báo cáo thời gian lái xe trong tuần OBC - Export
        /// </summary>
        ObcDrivingTimeInWeekExport = 936,

        /// <summary>
        /// Báo cáo thời gian lái xe trong tuần OBC - Option
        /// </summary>
        ObcDrivingTimeInWeekOption = 937,

        /// <summary>
        /// Báo cáo tăng tốc phanh gấp OBC - Xem
        /// </summary>
        ObcSuddenlyIncDecVelocityView = 938,

        /// <summary>
        /// Báo cáo tăng tốc phanh gấp OBC - Export
        /// </summary>
        ObcSuddenlyIncDecVelocityExport = 939,

        /// <summary>
        /// Báo cáo tăng tốc phanh gấp OBC - Option
        /// </summary>
        ObcSuddenlyIncDecVelocityOption = 940,

        /// <summary>
        /// BC thời gian lái xe liên tục và trong ngày OBC - Xem
        /// </summary>
        ObcDrivingContinueInDayView = 941,

        /// <summary>
        /// BC thời gian lái xe liên tục và trong ngày OBC - Export
        /// </summary>
        ObcDrivingContinueInDayExport = 942,

        /// <summary>
        /// BC thời gian lái xe liên tục và trong ngày OBC - Option
        /// </summary>
        ObcDrivingContinueInDayOption = 943,

        /// <summary>
        /// BC vi phạm thời gian lái xe liên tục và trong ngày OBC - Xem
        /// </summary>
        ObcViolateDrivingContinueView = 944,

        /// <summary>
        /// BC vi phạm thời gian lái xe liên tục và trong ngày OBC - Export
        /// </summary>
        ObcViolateDrivingContinueExport = 945,

        /// <summary>
        /// BC vi phạm thời gian lái xe liên tục và trong ngày OBC - Option
        /// </summary>
        ObcViolateDrivingContinueOption = 946,

        /// <summary>
        /// BC vi phạm thời gian lái xe ban đêm OBC - Xem
        /// </summary>
        ObcViolateDrivingNightView = 947,

        /// <summary>
        /// BC vi phạm thời gian lái xe ban đêm OBC - Export
        /// </summary>
        ObcViolateDrivingNightExport = 948,

        /// <summary>
        /// BC vi phạm thời gian lái xe ban đêm OBC - Option
        /// </summary>
        ObcViolateDrivingNightOption = 949,

        /// <summary>
        /// Báo cáo thời gian lái xe trong ngày 2014  - xem
        /// </summary>
        BGTReportTimeOfDay2014View = 950,

        /// <summary>
        /// Báo cáo thời gian lái xe trong ngày 2014 - Export
        /// </summary>
        BGTReportTimeOfDay2014Export = 951,

        /// <summary>
        /// Báo cáo thời gian lái xe trong ngày 2014 - Option
        /// </summary>
        BGTReportTimeOfDay2014Option = 952,

        /// <summary>
        /// Báo cáo chi tiết hoạt động Ngôi sao Sài Gòn - Xem
        /// </summary>
        NssgDetailBusActivitiesView = 953,

        /// <summary>
        /// Báo cáo chi tiết hoạt động Ngôi sao Sài Gòn - Export
        /// </summary>
        NssgDetailBusActivitiesExport = 954,

        /// <summary>
        /// Báo cáo chi tiết hoạt động Ngôi sao Sài Gòn - Option
        /// </summary>
        NssgDetailBusActivitiesOption = 955,

        /// <summary>
        /// Báo cáo vi phạm hoạt động theo tuyến Ngôi sao Sài Gòn - Xem
        /// </summary>
        NssgSummaryBusActivitiesView = 956,

        /// <summary>
        /// Báo cáo vi phạm hoạt động theo tuyến Ngôi sao Sài Gòn - Export
        /// </summary>
        NssgSummaryBusActivitiesExport = 957,

        /// <summary>
        /// Báo cáo vi phạm hoạt động theo tuyến Ngôi sao Sài Gòn - Option
        /// </summary>
        NssgSummaryBusActivitiesOption = 958,

        /// <summary>
        /// Báo cáo tổng hợp vi phạm OBC - Xem
        /// </summary>
        ObcSummaryView = 959,

        /// <summary>
        /// Báo cáo tổng hợp vi phạm OBC - Export
        /// </summary>
        ObcSummaryExport = 960,

        /// <summary>
        /// Báo cáo tổng hợp vi phạm OBC - Option
        /// </summary>
        ObcSummaryOption = 961,

        /// <summary>
        /// Ra hạn thu phí của xe
        /// </summary>
        VehicleMaturityUpdate = 962,

        /// <summary>
        /// Báo cáo chi tiết chuyến lượt Ngôi sao Sài Gòn - Xem
        /// </summary>
        FlightDetailNSSGView = 963,

        /// <summary>
        /// Báo cáo chi tiết chuyến lượt Ngôi sao Sài Gòn - Export
        /// </summary>
        FlightDetailNSSGExport = 964,

        /// <summary>
        /// Báo cáo chi tiết chuyến lượt Ngôi sao Sài Gòn - Option
        /// </summary>
        FlightDetailNSSGOption = 965,

        /// <summary>
        /// Báo cáo dừng đỗ theo chuyến Ngôi sao Sài Gòn - Xem
        /// </summary>
        NssgStationView = 966,

        /// <summary>
        /// Báo cáo dừng đỗ theo chuyến Ngôi sao Sài Gòn - Export
        /// </summary>
        NssgStationExport = 967,

        /// <summary>
        /// Báo cáo dừng đỗ theo chuyến Ngôi sao Sài Gòn - Option
        /// </summary>
        NssgStationOption = 968,

        /// <summary>
        /// Hiển thị biểu đồ nhiên liệu trên online
        /// </summary>
        ShowFuelChartOnline = 969,

        /// <summary>
        /// Hiển thị thanh nhiên liệu trên hiện trạng online
        /// </summary>
        ShowFuelOnStatusOnline = 970,

        /// <summary>
        /// Hoạt động theo lịch hẹn - Xem
        /// </summary>
        AppointmentBeforesView = 971,

        /// <summary>
        /// Hoạt động theo lịch hẹn - Export
        /// </summary>
        AppointmentBeforesExport = 972,

        /// <summary>
        /// Hoạt động theo lịch hẹn - Option
        /// </summary>
        AppointmentBeforesOption = 973,

        //------------------------------------------
        /// <summary>
        /// Nhiên liệu Donacoop - Xem
        /// </summary>
        FuelConsumptionDailyDonacoopView = 974,

        /// <summary>
        /// Nhiên liệu Donacoop - Export
        /// </summary>
        FuelConsumptionDailyDonacoopExport = 975,

        /// <summary>
        /// Nhiên liệu Donacoop - Option
        /// </summary>
        FuelConsumptionDailyDonacoopOption = 976,

        /// <summary>
        /// Nhiên liệu Donacoop - Thêm
        /// </summary>
        FuelConsumptionDailyDonacoopAdd = 977,

        /// <summary>
        /// Nhiên liệu Donacoop - Sửa
        /// </summary>
        FuelConsumptionDailyDonacoopUpdate = 978,

        /// <summary>
        /// Nhiên liệu Donacoop - Xóa
        /// </summary>
        FuelConsumptionDailyDonacoopDelete = 979,

        /// <summary>
        /// Báo cáo xi măng tự nhập  - Xem
        /// </summary>
        DynamicConcreteTripView = 980,

        /// <summary>
        /// Báo cáo xi măng tự nhập  - Export
        /// </summary>
        DynamicConcreteTripExport = 981,

        /// <summary>
        /// Báo cáo xi măng tự nhập  - Option
        /// </summary>
        DynamicConcreteTripOption = 982,

        /// <summary>
        /// Báo cáo xi măng tự nhập  - Tạo mới
        /// </summary>
        DynamicConcreteTripAdd = 983,

        /// <summary>
        /// Báo cáo xi măng tự nhập  - Sửa
        /// </summary>
        DynamicConcreteTripUpdate = 984,

        /// <summary>
        /// Báo cáo xi măng tự nhập  - Xóa
        /// </summary>
        DynamicConcreteTripDelete = 985,

        /// <summary>
        /// KT check xe  - Xem
        /// </summary>
        TechniciansCheckVehiclesView = 986,

        /// <summary>
        /// KT check xe  - Export
        /// </summary>
        TechniciansCheckVehiclesExport = 987,

        /// <summary>
        /// KT check xe  - Option
        /// </summary>
        TechniciansCheckVehiclesOption = 988,

        /// <summary>
        /// Báo cáo quá tốc độ giới hạn thêm 3 cột Vmax , thời điểm kết thúc , số giây vp (HSST) - Xem
        /// </summary>
        ReportQCVN31SpeedOverHsHvView = 989,

        /// <summary>
        /// Báo cáo quá tốc độ giới hạn thêm 3 cột Vmax , thời điểm kết thúc , số giây vp (HSST) - Export
        /// </summary>
        ReportQCVN31SpeedOverHsHvExport = 990,

        /// <summary>
        /// Báo cáo quá tốc độ giới hạn thêm 3 cột Vmax , thời điểm kết thúc , số giây vp (HSST) - Option
        /// </summary>
        ReportQCVN31SpeedOverHsHvOption = 991,

        /// <summary>
        /// Báo cáo quá tốc độ NSSG - Xem
        /// </summary>
        ReportSpeedOverNSSGView = 992,

        /// <summary>
        /// Báo cáo quá tốc độ NSSG - Export
        /// </summary>
        ReportSpeedOverNSSGExport = 993,

        /// <summary>
        /// Báo cáo quá tốc độ NSSG - Option
        /// </summary>
        ReportSpeedOverNSSGOption = 994,

        /// <summary>
        /// Báo cáo dịch vụ vận chuyển CP - Xem
        /// </summary>
        ReportTransportServicesView = 995,

        /// <summary>
        /// Báo cáo dịch vụ vận chuyển CP - Export
        /// </summary>
        ReportTransportServicesExport = 996,

        /// <summary>
        /// Báo cáo dịch vụ vận chuyển CP - Xem
        /// </summary>
        ReportTransportServicesOption = 997,

        /// <summary>
        /// Giám sát nhiều xe
        /// </summary>
        ShowOnlineMultipleView = 998,

        /// <summary>
        /// Báo cáo chi tiết đổ bê tông - Xem
        /// </summary>
        ReportConcreteDumpDetailNewView = 999,

        /// <summary>
        /// Báo cáo chi tiết đổ bê tông - Export
        /// </summary>
        ReportConcreteDumpDetailNewExport = 1000,

        /// <summary>
        /// Báo cáo chi tiết đổ bê tông - Option
        /// </summary>
        ReportConcreteDumpDetailNewOption = 1001,

        /// <summary>
        /// Yêu cầu cung cấp dữ liệu - tích truyền
        /// </summary>
        InputCompanyInfoAllowTransfer = 1002,

        #region Báo cáo Hà Sơn

        /// <summary>
        /// Báo cáo hành trình chạy xe - Xem
        /// </summary>
        VehicleJourneyView = 1003,

        /// <summary>
        /// Báo cáo hành trình chạy xe - Export
        /// </summary>
        VehicleJourneyExport = 1004,

        /// <summary>
        /// Báo cáo tổng hợp km thực hiện - Xem
        /// </summary>
        VehicleJourneySummaryKmView = 1005,

        /// <summary>
        /// Báo cáo tổng hợp km thực hiện - Export
        /// </summary>
        VehicleJourneySummaryKmExport = 1006,

        /// <summary>
        /// Báo cáo tổng hợp km thực hiện - Option
        /// </summary>
        VehicleJourneySummaryKmOption = 1007,

        /// <summary>
        /// Báo cáo chi tiết km thực hiện - Xem
        /// </summary>
        VehicleJourneyDetailKmView = 1008,

        /// <summary>
        /// Báo cáo chi tiết km thực hiện - Export
        /// </summary>
        VehicleJourneyDetailKmExport = 1009,

        /// <summary>
        /// Báo cáo chi tiết km thực hiện - Option
        /// </summary>
        VehicleJourneyDetailKmOption = 1010,

        /// <summary>
        /// Báo cáo tổng hợp vi phạm tốc độ - Xem
        /// </summary>
        VehicleJourneySpeedOverSummaryView = 1011,

        /// <summary>
        /// Báo cáo tổng hợp vi phạm tốc độ - Export
        /// </summary>
        VehicleJourneySpeedOverSummaryExport = 1012,

        /// <summary>
        /// Báo cáo tổng hợp vi phạm tốc độ - Option
        /// </summary>
        VehicleJourneySpeedOverSummaryOption = 1013,

        /// <summary>
        /// Báo cáo chi tiết vi phạm tốc độ - Xem
        /// </summary>
        VehicleJourneySpeedOverDetailView = 1014,

        /// <summary>
        /// Báo cáo chi tiết vi phạm tốc độ - Export
        /// </summary>
        VehicleJourneySpeedOverDetailExport = 1015,

        /// <summary>
        /// Báo cáo chi tiết vi phạm tốc độ - Option
        /// </summary>
        VehicleJourneySpeedOverDetailOption = 1016,

        /// <summary>
        /// Báo cáo tổng hợp xe trung chuyển - Xem
        /// </summary>
        VehicleJourneySummaryView = 1017,

        /// <summary>
        /// Báo cáo tổng hợp xe trung chuyển - Export
        /// </summary>
        VehicleJourneySummaryExport = 1018,

        /// <summary>
        /// Báo cáo tổng hợp xe trung chuyển - Option
        /// </summary>
        VehicleJourneySummaryOption = 1019,

        /// <summary>
        /// Báo cáo chi tiết xe trung chuyển - Xem
        /// </summary>
        VehicleJourneyDetailView = 1020,

        /// <summary>
        /// Báo cáo chi tiết xe trung chuyển - Export
        /// </summary>
        VehicleJourneyDetailExport = 1021,

        /// <summary>
        /// Báo cáo chi tiết xe trung chuyển - Option
        /// </summary>
        VehicleJourneyDetailOption = 1022,

        /// <summary>
        /// Tổng hợp nhiên liệu Donacoop - Xem
        /// </summary>
        FuelConsumptionDailyDonacoopRView = 1023,

        /// <summary>
        /// Tổng hợp nhiên liệu Donacoop - Export
        /// </summary>
        FuelConsumptionDailyDonacoopRExport = 1024,

        /// <summary>
        /// Tổng hợp nhiên liệu Donacoop - Option
        /// </summary>
        FuelConsumptionDailyDonacoopROption = 1025,

        /// <summary>
        /// Báo cáo tổng hợp log gửi SMS - Xem
        /// </summary>
        LogSendSMSSummaryView = 1026,

        /// <summary>
        /// Báo cáo tổng hợp log gửi SMS - Export
        /// </summary>
        LogSendSMSSummaryExport = 1027,

        /// <summary>
        /// Báo cáo tổng hợp log gửi SMS - Option
        /// </summary>
        LogSendSMSSummaryOption = 1028,

        /// <summary>
        /// Báo cáo chi tiết log gửi SMS - Xem
        /// </summary>
        LogSendSMSDetailsView = 1029,

        /// <summary>
        /// Báo cáo chi tiết log gửi SMS - Export
        /// </summary>
        LogSendSMSDetailsExport = 1030,

        /// <summary>
        /// Báo cáo chi tiết log gửi SMS - Option
        /// </summary>
        LogSendSMSDetailsOption = 1031,

        /// <summary>
        /// Báo cáo truyền TCĐB của KCS - Xem
        /// </summary>
        KCSTransferTCDBView = 1032,

        /// <summary>
        /// Báo cáo truyền TCĐB của KCS - Export
        /// </summary>
        KCSTransferTCDBExport = 1033,

        /// <summary>
        /// Báo cáo truyền TCĐB của KCS - Option
        /// </summary>
        KCSTransferTCDBOption = 1034,

        /// <summary>
        /// Báo cáo dừng đỗ lâu ngoài điểm kiểm soát - Xem
        /// </summary>
        LongStopLandmarksView = 1035,

        /// <summary>
        /// Báo cáo dừng đỗ lâu ngoài điểm kiểm soát - Export
        /// </summary>
        LongStopLandmarksExport = 1036,

        /// <summary>
        /// Module CP - Xem
        /// </summary>
        AdminCPView = 1037,

        /// <summary>
        /// Khai báo chuyến hàng - Xem
        /// </summary>
        TripDetailInfosView = 1038,

        /// <summary>
        /// Khai báo chuyến hàng - Export
        /// </summary>
        TripDetailInfosExport = 1039,

        /// <summary>
        /// Khai báo chuyến hàng - Option
        /// </summary>
        TripDetailInfosOption = 1040,

        /// <summary>
        /// Khai báo chuyến hàng - Thêm
        /// </summary>
        TripDetailInfosAdd = 1041,

        /// <summary>
        /// Khai báo chuyến hàng - Sửa
        /// </summary>
        TripDetailInfosUpdate = 1042,

        /// <summary>
        /// Khai báo chuyến hàng - Xóa
        /// </summary>
        TripDetailInfosDelete = 1043,

        /// <summary>
        /// Nghiệm thu chuyến hàng - Xem
        /// </summary>
        TripDetailAcceptView = 1044,

        /// <summary>
        /// Nghiệm thu chuyến hàng - Export
        /// </summary>
        TripDetailAcceptExport = 1045,

        /// <summary>
        /// Nghiệm thu chuyến hàng - Option
        /// </summary>
        TripDetailAcceptOption = 1046,

        /// <summary>
        /// Nghiệm thu chuyến hàng - Sửa
        /// </summary>
        TripDetailAcceptUpdate = 1047,

        /// <summary>
        /// Xử lý vi phạm CP - Xem
        /// </summary>
        TripDetailProcessViolateView = 1048,

        /// <summary>
        ///  Xử lý vi phạm CP - Export
        /// </summary>
        TripDetailProcessViolateExport = 1049,

        /// <summary>
        ///  Xử lý vi phạm CP - Option
        /// </summary>
        TripDetailProcessViolateOption = 1050,

        /// <summary>
        ///  Xử lý vi phạm CP - Sửa
        /// </summary>
        TripDetailProcessViolateUpdate = 1051,

        /// <summary>
        /// Báo cáo chi tiết chuyến hàng - Xem
        /// </summary>
        TripDetailsReportView = 1052,

        /// <summary>
        /// Báo cáo chi tiết chuyến hàng - Export
        /// </summary>
        TripDetailsReportExport = 1053,

        /// <summary>
        /// Báo cáo chi tiết chuyến hàng - Option
        /// </summary>
        TripDetailsReportOption = 1054,

        /// <summary>
        /// Báo cáo tổng hợp chuyến hàng - Xem
        /// </summary>
        TripSummariesReportView = 1055,

        /// <summary>
        /// Báo cáo tổng hợp chuyến hàng - Export
        /// </summary>
        TripSummariesReportExport = 1056,

        /// <summary>
        /// Báo cáo tổng hợp chuyến hàng - Option
        /// </summary>
        TripSummariesReportOption = 1057,

        /// <summary>
        /// Báo cáo tổng hợp khả nghi vi phạm - Xem
        /// </summary>
        SuspectViolateSummariesView = 1058,

        /// <summary>
        /// Báo cáo tổng hợp khả nghi vi phạm - Export
        /// </summary>
        SuspectViolateSummariesExport = 1059,

        /// <summary>
        /// Báo cáo tổng hợp khả nghi vi phạm - Option
        /// </summary>
        SuspectViolateSummariesOption = 1060,

        /// <summary>
        /// Báo cáo tổng hợp chuyến Xuân Thành - Xem
        /// </summary>
        SummariesTripXTView = 1061,

        /// <summary>
        /// Báo cáo tổng hợp chuyến Xuân Thành - Export
        /// </summary>
        SummariesTripXTExport = 1062,

        /// <summary>
        /// Giám sát xe trung tâm
        /// </summary>
        WatchVehiclesCenter = 1063,

        /// <summary>
        /// Báo cáo thời gian làm việc 950 - Xem
        /// </summary>
        ReportWorkingTimeView = 1064,

        /// <summary>
        ///Báo cáo thời gian làm việc 950 - Export
        /// </summary>
        ReportWorkingTimeExport = 1065,

        /// <summary>
        /// Báo cáo định mức tuyến petrolimex - Xem
        /// </summary>
        NormsRouteView = 1066,

        /// <summary>
        ///Báo cáo định mức tuyến petrolimex - Export
        /// </summary>
        NormsRouteExport = 1067,

        /// <summary>
        ///Báo cáo định mức tuyến petrolimex - Option
        /// </summary>
        NormsRouteOption = 1068,

        /// <summary>
        /// Báo cáo chi tiết vi phạm truyền dữ liệu theo đơn vị vận tải - Xem
        /// </summary>
        DetailsViolateTransferDataView = 1069,

        /// <summary>
        ///Báo cáo chi tiết vi phạm truyền dữ liệu theo đơn vị vận tải - Export
        /// </summary>
        DetailsViolateTransferDataExport = 1070,

        /// <summary>
        ///Báo cáo chi tiết vi phạm truyền dữ liệu theo đơn vị vận tải - Option
        /// </summary>
        DetailsViolateTransferDataOption = 1071,

        /// <summary>
        /// Báo cáo xe that IMEI - Xem
        /// </summary>
        ChangeImeiView = 1072,

        /// <summary>
        ///Báo cáo xe that IMEI  - Export
        /// </summary>
        ChangeImeiExport = 1073,

        /// <summary>
        /// Báo cáo xả hàng khi di chuyển - Xem
        /// </summary>
        DischargeAndMoveView = 1074,

        /// <summary>
        ///Báo cáo xả hàng khi di chuyển  - Export
        /// </summary>
        DischargeAndMoveExport = 1075,

        /// <summary>
        /// Chạy lại dữ liệu - Xem
        /// </summary>
        RequestRerunBGT = 1076,

        /// <summary>
        /// Người dùng chuyển công ty - Xem
        /// </summary>
        UsersChangeCompanyView = 1077,

        /// <summary>
        /// Người dùng chuyển công ty - Export
        /// </summary>
        UsersChangeCompanyExport = 1078,

        /// <summary>
        /// Người dùng chuyển công ty - Thêm
        /// </summary>
        UsersChangeCompanyAdd = 1079,

        /// <summary>
        /// Báo cáo tổng hợp quảng cáo - Xem
        /// </summary>
        AdsSummaryView = 1080,

        /// <summary>
        ///Báo cáo tổng hợp quảng cáo  - Export
        /// </summary>
        AdsSummaryExport = 1081,

        /// <summary>
        /// Duyệt thông tin Sim _ Xe - Xem
        /// </summary>
        SyncSimVehiclesView = 1082,

        /// <summary>
        /// Duyệt thông tin Sim _ Xe - Sửa
        /// </summary>
        SyncSimVehiclesUpdate = 1083,

        /// <summary>
        /// Quản trị mẫu cảnh báo  - Xem
        /// </summary>
        CompanyTemplateView = 1084,

        /// <summary>
        /// Quản trị mẫu cảnh báo  - Export
        /// </summary>
        CompanyTemplateExport = 1085,

        /// <summary>
        /// Quản trị mẫu cảnh báo  - Tạo mới
        /// </summary>
        CompanyTemplateAdd = 1086,

        /// <summary>
        /// Quản trị mẫu cảnh báo  - Sửa
        /// </summary>
        CompanyTemplateUpdate = 1087,

        /// <summary>
        /// Quản trị mẫu cảnh báo  - Xóa
        /// </summary>
        CompanyTemplateDelete = 1088,

        //Cấu hình cảnh báo người dùng - Xem
        AlertUserConfigurationsView = 1089,

        //Cấu hình cảnh báo người dùng - Thêm
        AlertUserConfigurationsAdd = 1090,

        //Cấu hình cảnh báo người dùng - Sửa
        AlertUserConfigurationsEdit = 1091,

        //Báo cáo cấu hình cảnh báo người dùng - Xem
        ReportAlertConfigView = 1092,

        //Báo cáo cấu hình cảnh báo người dùng - Export
        ReportAlertConfigExport = 1093,

        //user permissions (21-30)
        /// <summary>
        /// Quyền xác thực người dùng
        /// </summary>
        UserVerify = 1094,

        /// <summary>
        /// Báo cáo người dùng thực hiện xác thực - Xem
        /// </summary>
        ReportVerifyLoginView = 1095,

        /// <summary>
        /// Báo cáo người dùng thực hiện xác thực - Exxport
        /// </summary>
        ReportVerifyLoginExport = 1096,

        /// <summary>
        /// Báo cáo người dùng thực hiện xác thực - Option
        /// </summary>
        ReportVerifyLoginOption = 1097,

        /// <summary>
        /// Báo cáo tổng hợp thời gian làm việc 950 - Xem
        /// </summary>
        ReportWorkingTimeSummaryView = 1098,

        /// <summary>
        ///Báo cáo tổng hợp thời gian làm việc 950 - Export
        /// </summary>
        ReportWorkingTimeSummaryExport = 1099,

        /// <summary>
        /// Chi tiết vi phạm quá tốc độ TT09-2015 - Xem
        /// </summary>
        ViolateSpeedOverDetailsView = 1100,

        /// <summary>
        /// Chi tiết vi phạm quá tốc độ TT09-2015 - Export
        /// </summary>
        ViolateSpeedOverDetailsExport = 1101,

        /// <summary>
        /// Tổng hợp vi phạm quá tốc độ TT09-2015 - Xem
        /// </summary>
        ViolateSpeedOverSummariesView = 1102,

        /// <summary>
        /// Tổng hợp vi phạm quá tốc độ TT09-2015 - Export
        /// </summary>
        ViolateSpeedOverSummariesExport = 1103,

        /// <summary>
        /// Chi tiết vi phạm thời gian lái xe 4h - Xem
        /// </summary>
        DrivingContinuousDetailsView = 1104,

        /// <summary>
        /// Chi tiết vi phạm thời gian lái xe 4h - Export
        /// </summary>
        DrivingContinuousDetailsExport = 1105,

        /// <summary>
        /// Chi tiết vi phạm thời gian lái xe 10h/ngày - Xem
        /// </summary>
        DrivingInDayDetailsView = 1106,

        /// <summary>
        /// Chi tiết vi phạm thời gian lái xe 10h/ngày - Export
        /// </summary>
        DrivingInDayDetailsExport = 1107,

        /// <summary>
        /// Tổng hợp vi phạm - Xem
        /// </summary>
        ViolateSummariesView = 1108,

        /// <summary>
        /// Tổng hợp vi phạm - Export
        /// </summary>
        ViolateSummariesExport = 1109,

        /// <summary>
        /// Chi tiết vi phạm truyền - Xem
        /// </summary>
        ViolateTransferDetailsView = 1110,

        /// <summary>
        /// Chi tiết vi phạm truyền - Export
        /// </summary>
        ViolateTransferDetailsExport = 1111,

        /// <summary>
        /// Module SMS - Xem
        /// </summary>
        SMSModuleView = 1112,

        /// <summary>
        /// Log nợ phí kế toán  - Xem
        /// </summary>
        VehicleMaturityFeeLogsView = 1113,

        /// <summary>
        /// Log nợ phí kế toán  - Export
        /// </summary>
        VehicleMaturityFeeLogsExport = 1114,

        ///// <summary>
        ///// Log nợ phí kế toán  - Option
        ///// </summary>
        //VehicleMaturityFeeLogsOption = 988,

        /// <summary>
        /// Danh sách người dùng yêu cầu xác thực SMS - Xem
        /// </summary>
        ListUserRequestVerifiySMSView = 1115,

        /// <summary>
        /// Danh sách người dùng yêu cầu xác thực SMS - Exxport
        /// </summary>
        ListUserRequestVerifiySMSExport = 1116,

        /// <summary>
        /// Vi phạm truyền dữ liệu theo đơn vị vận tải - Mẫu TCĐB - Xem
        /// </summary>
        ViolateTransferData7DaysContinuousView = 1117,

        /// <summary>
        ///Vi phạm truyền dữ liệu theo đơn vị vận tải - Mẫu TCĐB - Exxport
        /// </summary>
        ViolateTransferData7DaysContinuousExport = 1118,

        /// <summary>
        /// Báo cáo truyền dữ liệu theo danh sách xe - Xem
        /// </summary>
        ViolateTransferData7DaysContinuousDetailsView = 1119,

        /// <summary>
        ///Báo cáo truyền dữ liệu theo danh sách xe - Exxport
        /// </summary>
        ViolateTransferData7DaysContinuousDetailsExport = 1120,

        /// <summary>
        /// Báo cáo chi tiết hành trình - TCĐB - 2016 - Xem
        /// </summary>
        ItineraryView = 1121,

        /// <summary>
        ///Báo cáo chi tiết hành trình - TCĐB - 2016 - Exxport
        /// </summary>
        ItineraryExport = 1122,

        /// <summary>
        /// Tao moi nhanh cong ty
        /// </summary>
        CompanyAddQuickly = 1123,

        /// <summary>
        /// lấy dữ liệu công ty từ BAP
        /// </summary>
        GetDataCompanyFromBAP = 1124,

        /// <summary>
        /// Bù km
        /// </summary>
        RouteOffset = 1125,

        /// <summary>
        /// Báo cáo lịch sử khóa xóa xe  - Xem
        /// </summary>
        LockDeleteVehiclesView = 1126,

        /// <summary>
        /// Báo cáo lịch sử khóa xóa xe  - Export
        /// </summary>
        LockDeleteVehiclesExport = 1127,

        /// <summary>
        /// Báo cáo thời gian làm việc mới 950 - Xem
        /// </summary>
        ReportWorkingTimeNewView = 1128,

        /// <summary>
        ///Báo cáo thời gian làm việc mới 950 - Export
        /// </summary>
        ReportWorkingTimeNewExport = 1129,

        /// <summary>
        /// Báo cáo chi tiết tin gửi theo công ty - Xem
        /// </summary>
        DetailsByCompaniesView = 1130,

        /// <summary>
        ///Báo cáo chi tiết tin gửi theo công ty - Export
        /// </summary>
        DetailsByCompaniesExport = 1131,

        /// <summary>
        /// Báo cáo tổng hợp tin gửi theo công ty - Xem
        /// </summary>
        SummaryByCompaniesView = 1132,

        /// <summary>
        ///Báo cáo tổng hợp tin gửi theo công ty - Export
        /// </summary>
        SummaryByCompaniesExport = 1133,

        /// <summary>
        /// Danh sách điểm theo công ty - Xem
        /// </summary>
        LandmarksByCompanyView = 1134,

        /// <summary>
        ///Danh sách điểm theo công ty - Export
        /// </summary>
        LandmarksByCompanyExport = 1135,

        /// <summary>
        /// Danh sách điểm theo công ty - Option
        /// </summary>
        LandmarksByCompanyOption = 1136,

        /// <summary>
        /// them diem tu Excel: 1137
        /// </summary>
        LandmarkQuickImport = 1137,

        /// <summary>
        /// them cuoc nhien lieu
        /// </summary>
        ReportFuelAdd = 1138,

        /// <summary>
        /// Copy cấu hình xe - Xem
        /// </summary>
        CopyVehicleConfigurationsView = 1139,

        /// <summary>
        /// Copy cấu hình xe - Export
        /// </summary>
        CopyVehicleConfigurationsExport = 1140,

        /// <summary>
        /// Copy cấu hình xe
        /// </summary>
        CopyVehicleConfigurations = 1141,

        /// <summary>
        /// Xe vào hệ thống - Xem
        /// </summary>
        VehiclesJoinSystemView = 1142,

        /// <summary>
        /// Xe vào hệ thống - Export
        /// </summary>
        VehiclesJoinSystemExport = 1143,

        /// <summary>
        /// Danh sách công ty hệ thống rút gọn - Xem
        /// </summary>
        ListCompaniesBaseView = 1144,

        /// <summary>
        /// Danh sách công ty hệ thống rút gọn - Export
        /// </summary>
        ListCompaniesBaseExport = 1145,

        /// <summary>
        /// Báo cáo tra log khai báo tích truyền cho xe  - Xem
        /// </summary>
        BGTInfoHistoriesView = 1146,

        /// <summary>
        /// Báo cáo tra log khai báo tích truyền cho xe  - Export
        /// </summary>
        BGTInfoHistoriesExport = 1147,

        /// <summary>
        /// Báo cáo tra log khai báo tích truyền cho xe - Option
        /// </summary>
        BGTInfoHistoriesOption = 1148,

        /// <summary>
        /// Quản lý tin tức - Xem
        /// </summary>
        NewsManagementView = 1149,

        /// <summary>
        /// Quản lý tin tức - Export
        /// </summary>
        NewsManagementExport = 1150,

        /// <summary>
        /// Quản lý tin tức - Option
        /// </summary>
        NewsManagementOption = 1151,

        /// <summary>
        /// Tin tức - Thêm
        /// </summary>
        NewsAdd = 1152,

        /// <summary>
        /// Tin tức - Sửa
        /// </summary>
        NewsUpdate = 1153,

        /// <summary>
        /// Tin tức - Xóa
        /// </summary>
        NewsDelete = 1154,

        /// <summary>
        /// Báo cáo vận chuyển thức ăn chăn nuôi - Đức Minh (1100) - Xem
        /// </summary>
        TransportAnimalFeedView = 1155,

        /// <summary>
        /// Báo cáo vận chuyển thức ăn chăn nuôi - Đức Minh (1100) - Export
        /// </summary>
        TransportAnimalFeedExport = 1156,

        /// <summary>
        /// Lộ trình mẫu
        /// </summary>
        RouteOffsetTemplate = 1157,

        /// <summary>
        /// Người dùng - Cấu hình mở rộng
        /// </summary>
        UserGeneralConfig = 1158,

        /// <summary>
        /// Công ty - Gán nhanh công ty cho nhân viên kinh doanh
        /// </summary>
        QuickAssignSalePerson = 1159,

        /// <summary>
        /// Cấu hình cảnh báo người dùng - Hiển thị checkbox SMS
        /// </summary>
        AlertUserConfigurationsShowSMS = 1160,

        /// <summary>
        /// Báo cáo tích truyền thông tin xe lên TCDB Việt Nam - Xem
        /// </summary>
        BGTForwardDrvnView = 1161,

        /// <summary>
        /// Báo cáo tích truyền thông tin xe lên TCDB Việt Nam - Export
        /// </summary>
        BGTForwardDrvnExport = 1162,

        /// <summary>
        /// Báo cáo chi tiết hoạt động (bao gồm MTH) - Xem
        /// </summary>
        ActivityDetailIncludeSignalLossView = 1163,

        /// <summary>
        /// Báo cáo chi tiết hoạt động (bao gồm MTH)  - Export
        /// </summary>
        ActivityDetailIncludeSignalLossExport = 1164,

        /// <summary>
        /// Báo cáo chi tiết hoạt động (bao gồm MTH)  - Option
        /// </summary>
        ActivityDetailIncludeSignalLossOption = 1165,

        /// <summary>
        /// SIM BAP
        /// </summary>
        SIMBap = 1166,

        /// <summary>
        /// Báo cáo tổng hợp - Gộp theo ngày
        /// </summary>
        ReportActivitySummaryGroupByDay = 1167,

        /// <summary>
        /// Báo cáo tổng hợp thời gian làm việc 544 V2 - Xem
        /// </summary>
        ReportWorkingTimeSummaryV2View = 1168,

        /// <summary>
        ///Báo cáo tổng hợp thời gian làm việc 544 V2 - Export
        /// </summary>
        ReportWorkingTimeSummaryV2Export = 1169,

        /// <summary>
        /// Báo cáo thời gian làm việc 950 V2 - Xem
        /// </summary>
        ReportWorkingTimeV2View = 1170,

        /// <summary>
        ///Báo cáo thời gian làm việc 950 V2 - Export
        /// </summary>
        ReportWorkingTimeV2Export = 1171,

        /// <summary>
        /// Trang thông tin ngày lễ - Xem
        /// </summary>
        HolidaysManagementView = 1172,

        /// <summary>
        /// Cấu hình thời gian làm việc (chính thức - thêm giờ) - Xem
        /// </summary>
        AllWorkingTimeConfigurationsView = 1173,

        #endregion Báo cáo Hà Sơn

        /// <summary>
        /// Cấu hình lọc xe theo BAP!
        /// </summary>
        IsFilterFollowBAP = 1174,

        VehicleConfigVolageFuel = 1176,

        /// <summary>
        /// Sàng lọc IP nghi vấn - Xem
        /// </summary>
        SuspectIPsFilterView = 1175,

        /// <summary>
        /// Sàng lọc IP nghi vấn - Export
        /// </summary>
        SuspectIPsFilterExport = 1177,

        /// <summary>
        /// Báo cáo nhiên liệu đầu ngày - Xem
        /// </summary>
        FuelTankStartDailyView = 1178,

        /// <summary>
        /// Báo cáo nhiên liệu đầu ngày - Export
        /// </summary>
        FuelTankStartDailyExport = 1179,

        /// <summary>
        /// Báo cáo chi tiết đổ bê tông Việt Hàn - Xem
        /// </summary>
        ReportConcreteDumpDetailVHView = 1180,

        /// <summary>
        /// Báo cáo chi tiết đổ bê tông Việt Hàn - Export
        /// </summary>
        ReportConcreteDumpDetailVHExport = 1181,

        /// <summary>
        /// Báo cáo chi tiết đổ bê tông Việt Hàn - Option
        /// </summary>
        ReportConcreteDumpDetailVHOption = 1182,

        /// <summary>
        /// Cấu hình checkbox nhận cảnh báo  SMS
        /// </summary>
        AdminAlertSMSConfig = 1183,

        /// <summary>
        /// Cấu hình checkbox nhận cảnh bảo Email
        /// </summary>
        AdminAlertEmailConfig = 1184,

        /// <summary>
        /// Gán đồng hồ cho vùng cảnh báo - Xem
        /// </summary>
        AssignVehicleForAlertAreaView = 1185,

        /// <summary>
        /// Gán đồng hồ cho vùng cảnh báo - Export
        /// </summary>
        AssignVehicleForAlertAreaExport = 1186,

        /// <summary>
        /// Gán đồng hồ cho vùng cảnh báo - Option
        /// </summary>
        AssignVehicleForAlertAreaOption = 1187,

        /// <summary>
        /// Module khóa điện từ
        /// </summary>
        LockerModule = 1188,

        /// <summary>
        /// khóa điện từ - Online
        /// </summary>
        LockerOnlineView = 1189,

        /// <summary>
        /// khóa điện từ - Lộ trình
        /// </summary>
        LockerRouteView = 1190,

        /// <summary>
        /// Thêm nhanh người dùng
        /// </summary>
        QuickAddUser = 1195,

        /// <summary>
        /// Phụ lục 4 thông tư 63_2014 BGT - Xem
        /// </summary>
        AppendixForthTT632014View = 1196,

        /// <summary>
        /// Phụ lục 4 thông tư 63_2014 BGT - Export
        /// </summary>
        AppendixForthTT632014Export = 1197,

        /// <summary>
        /// Báo cáo hoạt động xe cho hợp tác xã - View
        /// </summary>
        ActivityReportForCooperativeView = 1198,

        /// <summary>
        /// Xuất  Báo cáo hoạt động xe cho hợp tác xã - Report
        /// </summary>
        ActivityReportForCooperativeExport = 1199,

        /// <summary>
        /// Xem báo cáo Route của GsLine
        /// </summary>
        GsLineRouteReportView = 1200,

        /// <summary>
        /// Xuất báo cáo Route của GsLine
        /// </summary>
        GsLineRouteReportExport = 1201,

        /// <summary>
        /// Theo dõi xe theo tuyến mẫu
        /// </summary>
        GsLineVehicleTrackingWithRoute = 1202,

        /// <summary>
        /// Tạo vùng báo cáo
        /// </summary>
        SealLandmarkAdd = 1203,

        /// <summary>
        /// Gán thẻ RFID cho quản lý
        /// </summary>
        AssignManagerToRfid = 1204,

        /// <summary>
        /// Cấp dải mã xí nghiệp cho đối tác
        /// </summary>
        AddListXnCodeToPartnerView = 1205,

        VehicleNoDataSearch = 1206,
        VehicleNoDataSearchExport = 1207,
        VehicleNoDataReport = 1208,
        VehicleNoDataReportExport = 1209,

        InputRegisterExport = 1210,
        FeedbackSend = 1211,

        /// <summary>
        /// Tin nhắn online tầu cá
        /// </summary>
        ViewMessageOnline = 2001,

        TrackingVideosView = 1346,
        TrackingOnlineByImagesView = 1345,
        /// <summary>
        /// Thêm mới giấy tờ phương tiện
        /// </summary>
        PaperAddNew = 2021,
        /// <summary>
        /// Xem thông tin giấy tờ phương tiện
        /// </summary>
        PaperView = 2022,
        /// <summary>
        /// Chỉnh sửa thông tin giấy tờ
        /// </summary>
        PaperUpdate = 2023
    }
}