namespace BA_MobileGPS.Utilities.Constant
{
    public class ApiUri
    {
        #region authorization

        public const string POST_LOGIN = "api/v3/authentcation/login";

        public const string POST_CHANGE_PASS = "api/v2/authentcation/changepassword";

        public const string POST_LOGOUT = "api/authentcation/logout";

        public const string GET_MOBILEVERSION = "api/v2/version/getbyos";

        public const string GET_DATABASEVERSION = "api/v2/menu/getdatabaseversions";

        public const string GET_MOBILERESOURCE = "api/menu/getresourcebyculture";

        public const string GET_LANGUAGETYPE = "api/language/getalllanguage";

        public const string POST_UPDATELANGUAGEUSER = "api/language/updatelanguagebyuser";

        public const string GET_MOBILECONFIG = "api/mobileconfigurations/getall";

        public const string GET_SENTOTP = "api/v2/authentcation/sentotp";
        public const string GET_VERIFYOTP = "api/v2/authentcation/verifyotp";

        #endregion authorization

        #region Notification

        public const string GET_NOTIFICATION = "api/v2/notification/getnotification";

        public const string GET_LIST_NOTIFICATION = "api/v2/notification/listnotification";

        public const string GET_NOTIFICATION_DETAIL = "api/v2/notification/notificationdetail";

        public const string GET_NOTIFICATION_BODY = "api/v2/notification/notificationbody";

        public const string POST_UPDATEISREAD_NOTIFICATION = "api/v2/notification/updateisread";

        public const string POST_DELETE_NOTIFICATION_BYUSER = "api/v2/notification/deletenoticebyuser";

        public const string POST_DELETERANGE_NOTIFICATION_BYUSER = "api/v2/notification/deleterangenoticebyuser";

        public const string GET_NOTIFICATION_WHEN_LOGIN = "api/v2/notification/notificationwhenlogin";

        public const string GET_NOTIFICATION_AFTER_LOGIN = "api/v2/notification/notificationafterlogin";

        public const string POST_INSERT_FEEDBACK_NOTIFICATION_BYUSER = "api/notification/insertfeedbackbyuser";

        #endregion Notification

        #region vehicle

        public const string GET_VEHICLE_LIST = "api/vehicles/getlistvehicleplate";

        public const string GET_VEHICLEONLINE = "api/v3/vehicles/getlistvehicleonline";

        public const string GET_VEHICLEONLINESYNC = "api/v2/vehicles/syncvehicleonline";

        public const string GET_VEHICLE_GROUP = "api/vehicles/getlistgroups";

        public const string GET_VEHICLE_COMPANY = "api/v2/vehicles/getlistcompanyid";

        public const string GET_VEHICLE_COMPANY_BY_BUSINESSUSER = "api/vehicles/getlistcompanyidbybusinessuser";

        public const string GET_VEHICLE_ROUTE_HISTORY = "api/route/gethistoryroute";

        public const string GET_VALIDATE_USER_CONFIG_ROUTE_HISTORY = "api/route/validationuserconfiggethistoryroute";

        public const string GET_LIST_POLYGON = "api/landmark/polygon";

        public const string GET_LIST_POLYGONPARACELISLANDS = "api/landmark/polygonparacelislands";
        

        #endregion vehicle

        #region alert

        // Đổi thành post vì quá nhiều xe => url quá dài, dài quá 2000 ký tự thì sẽ ko xử lý được
        // Đổi thành phương thức post
        public const string GET_ALERT_ONLINE = "api/v2/alerts/getalert";

        public const string GET_COUNT_ALERT_ONLINE = "api/v3/alerts/getcountalert";

        public const string GET_ALERT_TYPE = "api/v2/alerts/getalerttypebycompanyid";

        public const string POST_ALERT_HANDLE = "api/v2/alerts/handlealert";

        public const string GET_LIST_ALERT_COMPANY_CONFIG_BY_COMPANYID = "api/alerts/getalertcompanyconfig";

        public const string GET_ALERT_USER_CONFIGURATIONS = "api/v2/alerts/getalertuserconfigurationbyuserid";

        public const string SEND_ALERT_USER_CONFIG = "api/v2/alerts/insertorupdateuseralertconfig";

        #endregion alert

        #region home

        public const string GET_HOME_MENU = "api/v2/menu/getmenubyculture";
        public const string POST_SAVE_CONFIG_HOME_MENU = "api/menu/insertupdateusersettingmenu";

        #endregion home

        #region Feedback

        public const string POST_SAVE_FEEDBACK = "api/admin/feedback";
        public const string GET_FEEDBACK_TYPE = "api/category/getcategoryfeedbacktype";

        #endregion Feedback

        #region Address

        public const string GET_GETADDRESSBYLATLNG = "api/geocode/getaddresslandmarkbylatlng";

        #endregion Address

        #region RegisterConsult

        public const string REGISTERCONSULT = "api/support/register";

        public const string GET_LISTTRANSPORTTYPES = "api/category/getlisttransporttypes";

        public const string GET_LISTPROVINCES = "api/category/getlistprovinces";

        #endregion RegisterConsult

        #region ForgotPassword

        public const string VALIDATEPHONEBYUSER = "api/authentcation/validatephonebyuser";

        public const string SENDVERIFYCODE = "api/sms/sendverifycode";

        public const string CHECKVERIFYCODE = "api/sms/checkverifycode";

        public const string CHANGEPASSWORDFORGET = "api/v2/authentcation/changepasswordforget";

        #endregion ForgotPassword

        #region Category

        public const string CATEGORY_LIST_GENDER = "api/category/getcategorygender";
        public const string CATEGORY_LIST_RELIGION = "api/category/getcategoryreligion";

        #endregion Category

        #region AppDevice

        public const string INSERT_UPDATE_APP_DEVICE = "/api/appdevice/insertupdate";

        #endregion AppDevice

        #region User

        public const string GET_USERINFOMATION = "api/user/getuserinfo";
        public const string USER_UPDATE_AVATAR = "api/image/uploadimageuser";
        public const string USER_UPDATE_INFO = "api/user/updateuserinfor";
        public const string USER_SET_SETTINGS = "api/user/setusersetting";
        public const string ADMIN_USER_SET_SETTINGS = "api/user/userconfiguration";

        #endregion User

        #region vehicledebtmoney

        public const string GET_VEHICLEPLATEDEBTMONEY_AUTOCOMPLETE = "api/vehicles/getlistexpired";
        public const string GET_LISTVEHICLEDEBTMONEY = "api/vehicles/getlistexpired";
        public const string GET_COUNTVEHICLEDEBTMONEY = "/api/vehicles/countexpired";
        public const string GET_LISTVEHICLEFREE = "api/vehicles/getallvehiclefree";

        #endregion vehicledebtmoney

        #region Vehicle detail

        public const string GET_VEHICLEDETAIL = "api/v3/vehicles/getvehicledetail";

        public const string GET_ADDRESSESBYLATLNG = "api/geocode/batchaddress";

        #endregion Vehicle detail

        #region report

        public const string GET_REPORTTEMPERATURE = "api/reports/temperature";
        public const string GET_REPORTADDRESS = "api/reports/address";
        public const string GET_MACHINEVEHICLE = "api/reports/machine";
        public const string GET_FUELVEHICLE = "api/reports/fuel";
        public const string GET_FUELCHART = "api/reports/fuelchart";
        public const string GET_SPEEDOVERS = "api/reports/speedover";
        public const string GET_STOPPARKING = "api/reports/stop";
        public const string GET_DETAILS = "api/reports/activitydetail";
        public const string GET_SIGNALLOSS = "api/reports/signalloss";
        public const string GET_ACTIVITYSUMMARIES = "api/reports/activitysummary";
        public const string GET_FUELSSUMMARIES = "api/reports/fuelconsumptiondaily";
        public const string GET_FUELSSUMMARIESTOTAL = "api/reports/fuelconsumptiontotal";
        public const string GET_HISTORY_PACKAGE = "api/fishingvesselsms/historypackage";
        public const string GET_CURRENT_PACKAGE = "api/fishingvesselsms/packageinfor";
        public const string GET_SHIP_PACKAGE = "api/fishingvesselsms/shippackage";

        #endregion report

        #region Camera

        public const string GET_CAMERAIMAGE = "api/cameras/getcameraimage";
        public const string POST_READYFORSTREAM = "api/stream/start";
        public const string POST_ENDSTREAM = "api/stream/stop";
        public const string POST_GETMORETIMESTREAM = "api/stream/ping";
        public const string GET_DEVICESTREAMINFOR = "api/stream/devices";
        public const string GET_IMAGESLIMIT = "api/stream/imagelimit";
        public const string GET_IMAGESTIME = "api/stream/imagetime";
        public const string GET_IMAGES = "api/stream/image";
        public const string POST_RESTREAM_INFOR = "api/stream/restreaminfo";
        public const string POST_RESTREAM_LISTUPLOAD = "api/stream/restreamuploadinfo";
        public const string POST_RESTREAM_START = "api/stream/restreamstart";
        public const string POST_RESTREAM_STOP = "api/stream/restreamstop";
        public const string GET_RESTREAM_IMAGES = "api/stream/restreamimage";
        public const string POST_RESTREAM_UPLOAD = "api/stream/uploadcloud";
        public const string POST_RESTREAM_CANCELUPLOAD = "api/stream/canceluploadcloud";
        public const string POST_RESTREAM_UPLOADPROGRESS = "api/stream/uploadprogress";
        public const string POST_CHART_DATA = "api/stream/chartdata";
        public const string GET_RESTREAM_DEVICETAB_VIDEO_INFOR = "api/v2/stream/restreamdevicetabvideo";

        #endregion Camera

        #region Guide

        public const string GET_GUIDE = "api/guide/getguides";

        #endregion Guide

        #region FishShip

        public const string GET_LIST_FISH = "api/fishingvessel/getlistkindoffish";
        public const string SEND_FISH_TRIP = "api/fishingvessel/insertdatatofishtrip";
        public const string GET_SHIPDETAIL = "api/fishingvessel/getshipdetail";
        public const string POST_SMSPACKAGE = "api/fishingvesselsms/smspackage";

        #endregion FishShip

        #region message

        public const string POST_SEND_MESSAGE = "api/fishingvesselsms/sendsms";

        #endregion message

        #region sendenginecontrol

        public const string GET_SEND_ENGINE_CONTROL = "api/engine/sendenginecontrol";
        public const string GET_LIST_ENGINE = "api/engine/listactiononoffmachine";

        #endregion sendenginecontrol

        #region landmark

        public const string GET_ALL_LANDMARK_GROUP_BY_USERID = "api/userlandmarkgroup/getalllandmarkgroupbyuserid";
        public const string GET_ALL_LANDMARK_CATEGORY_BY_USERID = "api/landmarkcategory/getalllandmarkcategorybyuserid";
        public const string INSERT_CONFIG_VISIBLE_GROUP_LANDMARK = "api/configvisiblegrouplandmark/insertconfigvisiblegrouplandmark";
        public const string GET_LANDMARK_BY_GROUPID = "api/landmark/getlandmarkbygroupid";
        public const string GET_LANDMARK_BY_CATEGORY = "api/landmark/getlandmarkbycategory";

        #endregion landmark

        #region ping

        public const string GET_PING_SERVER_STATUS = "api/ping/serverstatus";
        public const string GET_TIMESERVER = "api/ping/timeserver";

        #endregion ping

        #region MOTO

        public const string GET_MOTO_DETAIL = "api/vehicles/getmotodetail";

        public const string GET_MOTO_PROPERTIES = "api/engine/getmotoproperties";

        public const string SEND_CONFIG_MOTO = "api/engine/sendconfigmoto";

        public const string GET_SIM_MONEY = "api/vehicles/getsimmoney";

        #endregion MOTO
        #region DriverInformation
        public const string GET_LIST_DRIVER = "api/driverinfor/getall";

        public const string POST_ADDORUPDATE_DRIVER = "api/driverinfor/insertorupdate";

        public const string POST_DELETE_DRIVER = "api/driverinfor/deletedriverbyid";
        #endregion

        #region PapersInformation
        public const string GET_LIST_PAPER_CATEGORY = "api/paperinfor/getpapercategory";
        public const string GET_LIST_INSURANCE_CATEGORY = "api/paperinfor/getinsurancecategories";
        public const string GET_LAST_PAPER_INSURANCE = "api/paperinfor/getpaperinsurance";
        public const string GET_LAST_PAPER_REGISTRATION = "api/paperinfor/getpaperregistration";
        public const string GET_LAST_PAPER_SIGN = "api/paperinfor/getpapersign";
        public const string POST_INSERT_PAPER_INSURANCE = "api/paperinfor/insertinsurancepaper";
        public const string POST_INSERT_PAPER_REGISTRATION = "api/paperinfor/insertregistrationpaper";
        public const string POST_INSERT_PAPER_SIGN = "api/paperinfor/insertsignpaper";
        public const string POST_UPDATE_PAPER_INSURANCE = "api/paperinfor/updateinsurancepaper";
        public const string POST_UPDATE_PAPER_REGISTRATION = "api/paperinfor/updateregistrationpaper";
        public const string POST_UPDATE_PAPER_SIGN = "api/paperinfor/updatesignpaper";
        public const string GET_LIST_ALL_PAPER = "api/paperinfor/getlistpaper";
        public const string GET_LIST_ALL_PAPER_HISTORY = "api/paperinfor/getlistpaperhistory";
        public const string GET_LAST_PAPER_DATE_BY_VEHICLE = "api/paperinfor/getlastpaperdatebyvehicle";
        #endregion


        #region Issue
        public const string GET_ISSUE_BYCOMPANYID = "api/issue/getissuebycompanyid";

        public const string GET_ISSUE_BYISSUECODE = "api/issue/getissuebyissuecode";
        #endregion
    }
}