using System.ComponentModel;

namespace BA_MobileGPS.Utilities.Enums
{
    public enum ApiEndpointTypes
    {
        [Description("http://api.vietnamcnn.vn")]
        ServerCNN,

        [Description("http://apigateway.bagroup.vn")]
        ServerThat,

        [Description("http://10.1.11.110:6161")]
        ServerNamth,

        [Description("http://192.168.1.49:8031/")]
        ServerTest,

        [Description("http://125.212.226.154:3990")]
        ServerVNSAT,

        [Description("http://apiviview.bagroup.vn")]
        ServerVIVIEW,

        [Description("http://apigisviet.bagroup.vn")]
        ServerGISVIET,

        [Description("http://apivms.bagroup.vn")]
        ServerVMS,

        [Description("http://apimoto.bagroup.vn")]
        ServerMoto,

        [Description("http://192.168.1.50:9992")]
        ServerLinhLV,

        [Description("http://10.1.11.188:9990")]
        ServerDongLH,

        [Description("http://apiunitel.bagroup.vn")]
        ServerUNITEL,

        [Description("http://apigsht.bagroup.vn")]
        ServerGSHT,

    }
}