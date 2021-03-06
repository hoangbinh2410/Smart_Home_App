namespace BA_MobileGPS.Utilities
{
    /// <summary>
    /// Enum định nghĩa tập các tên cấu hình dùng cho AppKH
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// Namth  30/12/2017   created
    /// </Modified>
    public enum MobileConfigurationNames
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// Có bật cấu hình gửi dữ liệu vị trí về server không?
        /// </summary>
        EnableSendLocation,

        /// <summary>
        /// Số giây cho phép cập nhật status.
        /// Mặc định là 5s
        /// </summary>
        SecondsUpdateStatus,

        /// <summary>
        ///
        /// </summary>
        WebAddressUrl,

        /// <summary>
        ///
        /// </summary>
        WebHelpPageAddressUrl,

        /// <summary>
        /// Có phải là chế độ Debug không? mặc định là có
        /// </summary>
        IsDebugMode,

        /// <summary>
        /// Tọa độ Bình Anh: Số 30 61, Ngõ 88 Giáp Nhị, Giáp Nhị, Thịnh Liệt.
        /// </summary>
        DefaultLatitude,

        /// <summary>
        /// Tọa độ Bình Anh: Số 30 61, Ngõ 88 Giáp Nhị, Giáp Nhị, Thịnh Liệt.
        /// </summary>
        DefaultLongitude,

        HotlineTeleSaleGps,
        HotlineGps,
        EmailSupport,
        WebGps,
        Network,
        LinkAppStore,
        LinkCHPlay,
        LinkShareApp,
        LinkExperience,
        ConfigUseAccountKit,
        NumberOfDigitVerifyCode,
        AuthenticationSecond,
        TimeVehicleSync,
        TimmerVehicleSync,
        TimeSleep,
        Mapzoom,
        ClusterMapzoom,
        LengthAndPrefixNumberPhone,
        ConfigDangerousChar,
        ConfigUnitDebtMoney,

        ConfigPageNextReport,
        ConfigPageSizeReport,
        ConfigTotalCountDefault,
        ConfigIsShowDataAfterLoadFormReport,
        ConfigTitleDefaultReport,
        ConfigIsEnableButtonExport,
        ConfigCountMinutesShowMessageReport,
        CountUseCluster,
        ViewReport,
        CopyRight,
        LabelApp,
        LabelExperienceBA,
        LabelMenuAbout,
        UserNameToken,
        PasswordToken,
        IsUseExperience,
        IsUseNetwork,
        IsUseRegisterSupport,
        IsUseVehicleDebtMoney,
        IsUseBAGPSIntroduce,
        IsStartOnlinePage,
        CountDateOfPayment,
        DefaultMinTimeLossGPS,
        DefaultMaxTimeLossGPS,
        DefaultTimeLossConnect,
        TimeVehicleOffline,
        CountVehicleUsingCluster,
        IsUseForgotpassword,
        LinkYoutube,
        LinkBAGPS,
        LinkAdvertising,

        /// <summary>
        /// trungtq: có cần đồng bộ dạng request giống Web không? mặc định là có
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// trungtq  3/10/2020   created
        /// </Modified>
        EnableLongPoolRequest,

        /// <summary>
        /// trungtq: Mức đồng bộ cho phần đồng bộ online
        /// Level1: Đồng bộ 1 phần như của Namth đang làm (hiện tại vẫn bị)
        /// Level2: Đồng bộ tất, giống như web đang làm, hơi tốn máu nhưng an toàn
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// trungtq  3/10/2020   created
        /// </Modified>
        SynOnlineLevel,
        VehicleOnlineAddressEnabled,
        UseHelper,
        UseUserBehavior,
        UseCameraRTMP,
        MinZoomLevelGoogleMap,
        LinkFacebook,
        LinkZalo,
        LinkTiktok,
        LinkExportVideoCamera,
        LinkHelpExportVideoCamera,
        UrlBackgroudBottomHomePage,
        XNStartStreamAfterLogin
    }
}