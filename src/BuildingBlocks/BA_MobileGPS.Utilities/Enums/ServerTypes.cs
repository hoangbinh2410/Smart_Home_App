using System.ComponentModel;

namespace BA_MobileGPS.Utilities.Enums
{
    /// <summary>
    /// Server đang trên máy nào?
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// Namth  16/1/2018   created
    /// </Modified>
    public enum ServerIdentityHubTypes
    {
        [Description("http://logincnn.bagroup.vn")]
        ServerCNN,

        [Description("http://loginbagps.bagroup.vn")]
        ServerThat,

        [Description("10.1.11.131")]
        ServerNamth,

        [Description("http://192.168.1.49:8020")]
        ServerTest,

        [Description("http://loginviview.bagroup.vn")]
        ServerVIVIEW,

        [Description("http://loginviview.bagroup.vn")]
        ServerGISVIET,

        [Description("http://loginviview.bagroup.vn")]
        ServerUNITEL,

        [Description("http://loginviview.bagroup.vn")]
        ServerGSHT,

        [Description("http://loginbagps.bagroup.vn")]
        ServerVMS,

        [Description("http://loginmoto.bagroup.vn")]
        ServerMoto,
    }

    /// <summary>
    /// Server đang trên máy nào?
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// Namth  16/1/2018   created
    /// </Modified>
    public enum ServerVehicleOnlineHubTypes
    {
        [Description("http://vehicleonlinecnn.bagroup.vn")]
        ServerCNN,

        [Description("http://vehicleonlinegps.bagroup.vn")]
        ServerThat,

        [Description("10.1.11.131")]
        ServerNamth,

        [Description("http://125.212.226.154:6656")]
        ServerTest,

        [Description("http://vehicleonlineviview.bagroup.vn")]
        ServerVIVIEW,

        [Description("http://vehicleonlineviview.bagroup.vn")]
        ServerGISVIET,

        [Description("http://vehicleonlineviview.bagroup.vn")]
        ServerUNITEL,

        [Description("http://vehicleonlineviview.bagroup.vn")]
        ServerGSHT,

        [Description("http://vehicleonlinegps.bagroup.vn")]
        ServerVMS,

        [Description("http://vehicleonlinemoto.bagroup.vn")]
        ServerMoto,
    }

    /// <summary>
    /// Server đang trên máy nào?
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// Namth  16/1/2018   created
    /// </Modified>
    public enum ServerAlertHubTypes
    {
        [Description("http://alertcnn.bagroup.vn")]
        ServerCNN,

        [Description("http://alertgps.bagroup.vn")]
        ServerThat,

        [Description("10.1.11.110")]
        ServerNamth,

        [Description("http://125.212.226.154:6656")]
        ServerTest,

        [Description("http://alertviview.bagroup.vn")]
        ServerVIVIEW,

        [Description("http://alertviview.bagroup.vn")]
        ServerGISVIET,

        [Description("http://alertviview.bagroup.vn")]
        ServerUNITEL,

        [Description("http://alertviview.bagroup.vn")]
        ServerGSHT,

        [Description("http://alertvms.bagroup.vn")]
        ServerVMS,

        [Description("http://alertmoto.bagroup.vn")]
        ServerMoto,
    }


    /// <summary>
    /// Server đang trên máy nào?
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// Namth  16/1/2018   created
    /// </Modified>
    public enum ServerUserBehaviorHubTypes
    {
        [Description("http://125.212.192.175:8095")]
        ServerCNN,

        [Description("http://125.212.192.175:8095")]
        ServerThat,

        [Description("10.1.11.131")]
        ServerNamth,

        [Description("http://192.168.1.49:8028")]
        ServerTest,

        [Description("http://125.212.192.175:8095")]
        ServerVIVIEW,

        [Description("http://125.212.192.175:8095")]
        ServerGISVIET,

        [Description("http://125.212.192.175:8095")]
        ServerUNITEL,

        [Description("http://125.212.192.175:8095")]
        ServerGSHT,

        [Description("http://125.212.192.175:8095")]
        ServerVMS,

        [Description("http://125.212.192.175:8095")]
        ServerMoto,
    }
}