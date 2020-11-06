using System.ComponentModel;

namespace BA_MobileGPS.Utilities.Enums
{
    public enum ApiEndpointTypes
    {
        [Description("http://api.vietnamcnn.vn")]
        ServerCNN,

        [Description("http://api.bagroup.vn")]
        ServerThat,

        [Description("http://10.1.11.113:6990")]
        ServerNamth,

        [Description("http://192.168.1.49:8012")]
        ServerTest,

        [Description("http://125.212.226.154:3990")]
        ServerVNSAT,

        [Description("http://apiviview.bagroup.vn")]
        ServerVIVIEW,

        [Description("http://125.212.226.154:4990")]
        ServerGISVIET,

        [Description("http://apivms.bagroup.vn")]
        ServerVMS,

        [Description("http://apimoto.bagroup.vn")]
        ServerMoto,

        [Description("http://192.168.1.50:9992")]
        ServerLinhLV,

        [Description("http://10.1.11.188:9990")]
        ServerDongLH,
    }
}