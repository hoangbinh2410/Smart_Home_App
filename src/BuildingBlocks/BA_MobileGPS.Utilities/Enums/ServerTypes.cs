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
    public enum ServerTypes
    {
        [Description("signalr.vietnamcnn.vn")]
        ServerCNN,

        [Description("https://signalr.bagroup.vn")]
        ServerThat,

        [Description("10.1.11.131")]
        ServerNamth,

        [Description("http://125.212.226.154:6656")]
        ServerTest,

        [Description("signalr.vietnamcnn.vn")]
        ServerTestCNN,

        [Description("signalr.vietnamcnn.vn")]
        ServerVNSAT,

        [Description("signalrviview.bagroup.vn")]
        ServerVIVIEW,

        [Description("signalr.vietnamcnn.vn")]
        ServerGISVIET,

        [Description("https://signalr.bagroup.vn")]
        ServerVMS,

        [Description("signalrmoto.bagroup.vn")]
        ServerMoto,
    }

    /// <summary>
    /// Server đang trên máy nào?
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// Namth  16/1/2018   created
    /// </Modified>
    public enum ServerIdentityHubTypes
    {
        [Description("signalr.vietnamcnn.vn")]
        ServerCNN,

        [Description("http://loginbagps.bagroup.vn")]
        ServerThat,

        [Description("10.1.11.131")]
        ServerNamth,

        [Description("http://125.212.226.154:6656")]
        ServerTest,

        [Description("signalrviview.bagroup.vn")]
        ServerVIVIEW,

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
        [Description("signalr.vietnamcnn.vn")]
        ServerCNN,

        [Description("http://vehicleonlinegps.bagroup.vn")]
        ServerThat,

        [Description("10.1.11.131")]
        ServerNamth,

        [Description("http://125.212.226.154:6656")]
        ServerTest,

        [Description("signalrviview.bagroup.vn")]
        ServerVIVIEW,

        [Description("http://vehicleonlinegps.bagroup.vn")]
        ServerVMS,

        [Description("http://115.84.179.34:1656")]
        ServerMoto,
    }
}