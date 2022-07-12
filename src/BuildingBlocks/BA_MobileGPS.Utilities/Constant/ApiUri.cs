namespace BA_MobileGPS.Utilities.Constant
{
    public class ApiUri
    {
        #region authorization

        public const string POST_LOGIN = "api/v1/authentication/login";

        public const string POST_CHANGE_PASS = "api/v1/authentication/changepassword";

        public const string GET_MOBILEVERSION = "api/v1/mobileversion/getbyos";

        public const string GET_DATABASEVERSION = "api/v1/resource/getdatabaseversions";

        public const string GET_MOBILERESOURCE = "api/v1/resource/getresourcebyculture";

       // public const string GET_LANGUAGETYPE = "api/language/getalllanguage";

       // public const string POST_UPDATELANGUAGEUSER = "api/language/updatelanguagebyuser";

        public const string GET_MOBILECONFIG = "api/v1/mobileconfigurations/getall";

      //  public const string GET_PARTNERCONFIG = "api/mobileconfigurations/getpartnerconfigbycompanyid";
       
        #endregion authorization

        #region Notification

       //public const string GET_NOTIFICATION = "api/v2/notification/getnotification";

        public const string GET_LIST_NOTIFICATION = "api/v1/notice/getlistnotice";

        public const string GET_NOTIFICATION_DETAIL = "api/v1/notice/getnoticedetail";

        public const string GET_NOTIFICATION_BODY = "api/v1/notice/getnoticebody";

        public const string POST_UPDATEISREAD_NOTIFICATION = "api/v1/notice/insertnoticereadedbyuser";

        public const string POST_DELETE_NOTIFICATION_BYUSER = "api/v1/notice/insertnoticedeletedbyuser";

        public const string POST_DELETERANGE_NOTIFICATION_BYUSER = "api/v1/notice/insertrangenoticedeletedbyuser";

       // public const string GET_NOTIFICATION_WHEN_LOGIN = "api/v2/notification/notificationwhenlogin";

        public const string GET_NOTIFICATION_AFTER_LOGIN = "api/v1/notice/getnoticeafterlogin";

        public const string POST_INSERT_FEEDBACK_NOTIFICATION_BYUSER = "api/v1/notice/insertfeedbackbyuser";

        #endregion Notification

        #region vehicle       

        public const string GET_VEHICLEONLINE = "api/v1/vehicleonline/getlistvehicleonline";

        public const string GET_VEHICLEONLINESYNC = "api/v1/vehicleonline/syncvehicleonline";

        public const string GET_VEHICLE_GROUP = "api/v1/vehicles/getlistgroups";

        public const string GET_VEHICLE_COMPANY = "api/v1/vehicles/getlistcompanyid";

        public const string GET_VEHICLE_COMPANY_BY_BUSINESSUSER = "api/v1/vehicles/getlistcompanyidbybusinessuser";

        public const string GET_VEHICLE_ROUTE_HISTORY = "api/v1/route/getroutehistory";

        public const string GET_VALIDATE_USER_CONFIG_ROUTE_HISTORY = "api/v1/route/validatehistoryroute";

        public const string GET_LIST_POLYGON = "api/v1/landmark/polygon";

        public const string GET_LIST_POLYGONPARACELISLANDS = "api/v1/landmark/polygonparacelislands";

        #endregion vehicle

        #region smarthome
        public const string GET_ALL_STATUS = "api/v1";
        #endregion smarthome

        #region alert

        // Đổi thành post vì quá nhiều xe => url quá dài, dài quá 2000 ký tự thì sẽ ko xử lý được
        // Đổi thành phương thức post
        public const string GET_ALERT_ONLINE = "api/v1/getalert";

        public const string GET_COUNT_ALERT_ONLINE = "api/v1/alert/getcountalert";

        public const string GET_ALERT_TYPE = "api/v1/alert/getalerttypebycompanyid";

        public const string POST_ALERT_HANDLE = "api/v1/alert/handlealert";

       // public const string GET_LIST_ALERT_COMPANY_CONFIG_BY_COMPANYID = "api/alerts/getalertcompanyconfig";

        public const string GET_ALERT_USER_CONFIGURATIONS = "api/v1/alert/getalertuserconfigurationbyuserid";

        public const string SEND_ALERT_USER_CONFIG = "api/v1/alert/insertorupdateuseralertconfig";
        public const string GET_ALERT_MASK_DETAIL = "api/v1/alert/getalertmaskdetail";

        #endregion alert

        #region home

        public const string GET_HOME_MENU = "api/v1/menu/getmenubyculture";
        public const string POST_SAVE_CONFIG_HOME_MENU = "api/v1/menu/insertupdateusersettingmenu";

        #endregion home

        #region Feedback

       // public const string POST_SAVE_FEEDBACK = "api/admin/feedback";
       // public const string GET_FEEDBACK_TYPE = "api/category/getcategoryfeedbacktype";

        #endregion Feedback

        #region Address

        public const string GET_GETADDRESSBYLATLNG = "api/v1/geocode/getaddresslandmark";

        #endregion Address

        #region RegisterConsult

        public const string REGISTERCONSULT = "api/v1/registryadvisory/insertregistryadvisory";

        public const string GET_LISTTRANSPORTTYPES = "api/category/getlisttransporttypes";

        public const string GET_LISTPROVINCES = "api/category/getlistprovinces";

        #endregion RegisterConsult

        #region ForgotPassword

        public const string VALIDATEPHONEBYUSER = "api/v1/authentication/validatephonebyuser";

        public const string SENDVERIFYCODE = "api/v1/otp/sendverifycode";

        public const string CHECKVERIFYCODE = "api/v1/otp/checkverifycode";

        public const string GETOTP = "api/v1/otp/getotpzalo";

        public const string CHANGEPASSWORDFORGET = "api/v1/authentication/changepasswordforget";

        #endregion ForgotPassword

        #region Category

        public const string CATEGORY_LIST_GENDER = "api/v1/category/getcategorybyname";

        #endregion Category

        #region AppDevice

        public const string INSERT_UPDATE_APP_DEVICE = "api/v1/appdevice/insertupdateappdevice";

        #endregion AppDevice

        #region User

        public const string GET_USERINFOMATION = "api/v1/user/getuserinfor";
        public const string GET_USERBYUSERNAME = "api/v1/authentication/getuserinfobyusername";
        public const string USER_UPDATE_AVATAR = "api/v1/upload/uploadimagebase64";
        public const string USER_UPDATE_INFO = "api/v1/user/updateuserinfor";
        public const string USER_SET_SETTINGS = "api/v1/mobileusersetting/updatemobileusersetting";
        public const string ADMIN_USER_SET_SETTINGS = "api/v1/user/userconfiguration";

        #endregion User

        #region vehicledebtmoney
        
        public const string GET_LISTVEHICLEDEBTMONEY = "api/v1/vehicles/getlistexpired";
        public const string GET_COUNTVEHICLEDEBTMONEY = "api/v1/vehicles/countexpired";
        public const string GET_LISTVEHICLEFREE = "api/v1/vehicles/getallvehiclefree";

        #endregion vehicledebtmoney

        #region Vehicle detail

        public const string GET_VEHICLEDETAIL = "api/v1/vehicleonline/getvehicledetail";

       // public const string GET_ADDRESSESBYLATLNG = "api/geocode/batchaddress";

        #endregion Vehicle detail

        #region report

        public const string GET_VALIDATEDATETIME = "api/v1/reports/validatedatetimereport";
        public const string GET_REPORTTEMPERATURE = "api/v1/reports/temperature";
        public const string GET_REPORTADDRESS = "api/v1/geocode/getaddresslandmark";
        public const string GET_MACHINEVEHICLE = "api/v1/reports/machine";
        public const string GET_FUELVEHICLE = "api/v1/reports/fuel";
        public const string GET_FUELCHART = "api/v1/reports/fuelchart";
        public const string GET_SPEEDOVERS = "api/v1/reports/speedover";
        public const string GET_STOPPARKING = "api/v1/reports/stop";
        public const string GET_DETAILS = "api/v1/reports/activitydetail";
        public const string GET_SIGNALLOSS = "api/v1/reports/signalloss";
        public const string GET_ACTIVITYSUMMARIES = "api/v1/reports/activitysummary";
        public const string GET_FUELSSUMMARIES = "api/v1/reports/fuelconsumptiondaily";
        public const string GET_FUELSSUMMARIESTOTAL = "api/v1/reports/fuelconsumptiontotal";
        public const string GET_HISTORY_PACKAGE = "api/fishingvesselsms/historypackage";
        public const string GET_CURRENT_PACKAGE = "api/fishingvesselsms/packageinfor";
        public const string GET_SHIP_PACKAGE = "api/fishingvesselsms/shippackage";
        public const string GET_GetQCVN31SpeedReport = "api/v1/reports/getqcvn31report";

        public const string GET_GetListLocationStation = "api/v1/landmark/getlandmarkbycompanyid";
        public const string GET_GetStationDetails = "api/v1/reports/station";
        public const string GET_GetTransportBusiness = "api/v1/reports/transportbusiness";
        #endregion report
        #region Camera

        public const string GET_CAMERAIMAGE = "api/v1/camera/getimageinfo";       
        public const string GET_IMAGES = "api/v1/camera/getimagepreview";
        public const string POST_LISTVIDEONOTUPLOAD = "api/v1/camera/getvideonotupload";
        public const string POST_RESTREAM_LISTUPLOAD = "api/v1/camera/getvideouploaded";
        public const string POST_CHART_DATA = "api/v1/camera/videochart";
        public const string POST_LISTPLAYBACKINFO = "api/v1/camera/getlistvideoplayback";
        public const string POST_GetPACKETBYXNPLATE = "api/v1/camera/getpackagebyxnplate";      
        public const string GET_LISTVEHICLECAMERA = "api/v1/camera/listvideocamera";
        public const string GET_LISTCAMERACLOUD = "api/v1/camera/getlistvideocloud";
        public const string GET_DEVICESINFO = "api/v1/camera/devices";
        public const string POST_DEVICESTART = "api/v1/camera/start";
        public const string POST_DEVICESTARTMULTIPLE = "api/v1/camera/startmultiple";
        public const string POST_DEVICESTOP = "api/v1/camera/stop";
        public const string POST_DEVICESTOPSESSION = "api/v1/camera/stopsession";
        public const string POST_DEVICEPING = "api/v1/camera/ping";
        public const string POST_DEVICEPINGMULTIPLE = "api/v1/camera/pingmultiple";
        public const string POST_PLAYBACKSTART = "api/v1/camera/playbackstart";
        public const string POST_PLAYBACKSTOP = "api/v1/camera/playbackstop";
        public const string POST_PLAYBACKSTOPALL = "api/v1/camera/playbackstopall";
        public const string POST_UPLOADSTART = "api/v1/camera/uploadstart";
        public const string POST_UPLOADSTOP = "api/v1/camera/uploadstop";
        public const string POST_UPLOADPROGRESS = "api/v1/camera/uploadprogress";
        public const string POST_HOSTSPOT = "api/v1/camera/sethospot";

        #endregion Camera       
        #region Guide

        public const string GET_GUIDE = "api/guide/getguides";

        #endregion Guide

        #region FishShip

        
        public const string SEND_FISH_TRIP = "api/fishingvessel/insertdatatofishtrip";
        public const string GET_SHIPDETAIL = "api/fishingvessel/getshipdetail";
        public const string POST_SMSPACKAGE = "api/fishingvesselsms/smspackage";

        #endregion FishShip

        #region message

        public const string POST_SEND_MESSAGE = "api/fishingvesselsms/sendsms";

        #endregion message

        #region sendenginecontrol

        public const string GET_SEND_ENGINE_CONTROL = "api/engine/sendenginecontrol";
        public const string GET_LIST_ENGINE = "api/v1/engine/listactiononoffmachine";

        #endregion sendenginecontrol

        #region landmark

        public const string GET_ALL_LANDMARK_GROUP_BY_USERID = "api/v1/userlandmarkgroup/getalllandmarkgroupbyuserid";
        public const string GET_ALL_LANDMARK_CATEGORY_BY_USERID = "api/v1/landmarkcategory/getalllandmarkcategorybyuserid";
        public const string INSERT_CONFIG_VISIBLE_GROUP_LANDMARK = "api/v1/configvisiblegrouplandmark/insertconfigvisiblegrouplandmark";
        public const string GET_LANDMARK_BY_GROUPID = "api/v1/landmark/getlandmarkbygroupid";
        public const string GET_LANDMARK_BY_CATEGORY = "api/v1/landmark/getlandmarkbycategory";

        #endregion landmark

        #region ping

       // public const string GET_PING_SERVER_STATUS = "api/ping/serverstatus";
        public const string GET_TIMESERVER = "api/v1/ping/timeserver";

        #endregion ping

        #region MOTO

        public const string GET_MOTO_DETAIL = "api/vehicles/getmotodetail";

        public const string GET_MOTO_PROPERTIES = "api/engine/getmotoproperties";

        public const string SEND_CONFIG_MOTO = "api/engine/sendconfigmoto";

        public const string GET_SIM_MONEY = "api/vehicles/getsimmoney";

        #endregion MOTO

        #region DriverInformation

        public const string GET_LIST_DRIVER = "api/v1/hrmemployees/gethrmemployeesbycompanyid";

        public const string POST_ADD_DRIVER = "api/v1/hrmemployees/insertorupdatehrmemployees";       

        public const string POST_DELETE_DRIVER = "api/v1/hrmemployees/deletehrmemployees";

        #endregion DriverInformation

        #region PapersInformation

        public const string GET_LIST_PAPER_CATEGORY = "api/v1/papercategory/getpapercategory";
        public const string GET_LIST_INSURANCE_CATEGORY = "api/v1/paperinsurancecategory/getpaperinsurancecategory";
        public const string POST_INSERT_PAPER_INSURANCE = "api/v1/paperinfo/insertpaperinfoinsurrance";
        public const string POST_INSERT_PAPER_REGISTRATION = "api/v1/paperinfo/insertpaperinforegistry";
        public const string POST_INSERT_PAPER_SIGN = "api/v1/paperinfo/insertpaperinfosign";
        public const string POST_UPDATE_PAPER_INSURANCE = "api/v1/paperinfo/updatepaperinfoinsurrance";
        public const string POST_UPDATE_PAPER_REGISTRATION = "api/v1/paperinfo/updatepaperinforegistry";
        public const string POST_UPDATE_PAPER_SIGN = "api/v1/paperinfo/updatepaperinfosign";
        public const string GET_LIST_ALL_PAPER = "api/v1/paperinfo/getpaperinfobycompanyid";
        public const string GET_LIST_ALL_PAPER_HISTORY = "api/v1/paperinfo/gethistorypaperinfobycompanyid";
        public const string GET_LAST_PAPER_DATE_BY_VEHICLE = "api/v1/paperinfo/getexpiredatebyvehicle";
        public const string GET_LAST_PAPER_PaperCategory = "api/v1/paperinfo/getpaperinfobyvehicle";

        #endregion PapersInformation

        #region Issue

        // public const string GET_ISSUE_BYCOMPANYID = "api/issue/getissuebycompanyid";

        public const string GET_ISSUE_BYUSERID = "api/v1/issue/getissuebyuserid";

        public const string GET_ISSUE_BYISSUECODE = "api/v1/issue/getissuebyissuecode";

        #endregion Issue

        #region KPI

        public const string GET_DRIVERKPI_CHART = "api/v1/kpidriver/getdriverkpichart";
        public const string GET_DRIVERKPI_RANKING = "api/v1/kpidriver/getdriverranking";

        #endregion KPI

        #region Support
        public const string GET_List_SupportCategory = "api/v1/supportcategory/getlistsupportcategory";
        public const string GET_List_SupportContent = "api/v1/supportcategory/getlistsupportcontentbyid";
        public const string POST_MessageSupport = "api/v1/supportcategory/insertsupportbap";
        #endregion
        #region Expense
        public const string GET_List_ExpensesCategory = "api/v1/expenses/getlistexpensescategorybycompany";
        public const string POST_Import_Expense = "api/v1/expenses/insert";
        public const string GET_List_Expenses = "api/v1/expenses/searchexpenses";
        public const string Delete_Multiple = "api/v1/expenses/deletemultiple";
        #endregion Expense
        #region OTP
        public const string GET_Vehicle_OTP_SMS = "api/v1/otp/verifyotp";
        public const string Post_Numberphone_OTP_SMS = "api/v1/otp/verifyphonenumberotp";
        #endregion
    }
}