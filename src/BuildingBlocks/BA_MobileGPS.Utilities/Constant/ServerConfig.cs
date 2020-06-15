using BA_MobileGPS.Utilities.Enums;
using BA_MobileGPS.Utilities.Extensions;
using BA_MobileGPS.Utilities.Helpers;

namespace BA_MobileGPS.Utilities.Constant
{
    /// <summary>
    /// Thông tin cấu hình của server
    /// Tất cả thông tin cấu hình cho vào đây để không bị loãng
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// Namth  16/1/2018   created
    /// </Modified>
    public class ServerConfig
    {
        /// <summary>
        /// Loại Server là gì
        /// Mặc định là:            ServerTypes.Server20
        /// Khi chạy thật là:       ServerTypes.ServerThat
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  16/1/2018   created
        /// </Modified>
        public static ServerTypes ServerTypes = ServerTypes.ServerTest;

        /// <summary>
        /// Đường dẫn API ở đâu
        /// Mặc định là:        ApiEndpointTypes.Server20
        /// Khi chạy thật là:   ApiEndpointTypes.ServerThat
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  16/1/2018   created
        /// </Modified>
        public static ApiEndpointTypes ApiEndpointTypes = ApiEndpointTypes.ServerTest;

        /// <summary>
        /// Thông tin phiên bản App trên AppStore hoặc Google Play
        /// Khi đẩy thật thì chỉnh lại theo version trên market.
        /// </summary>
        public static string AppMarketVersion = "2.0.4";

        /// <summary>
        /// Thông tin phiên bản App trên AppStore hoặc Google Play
        /// Khi đẩy thật thì chỉnh lại theo version trên market.
        /// </summary>
        public static int BuildVersion = 1;

        /// <summary>
        /// IP server chạy TCP
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  16/1/2018   created
        /// </Modified>
        public static string ServerIP
        {
            get
            {
                switch (ServerTypes)
                {
                    case ServerTypes.ServerCNN:
                        return DNSHelper.GetIPAddressServer(ServerTypes.ServerCNN.ToDescription(), PortSignalRTypes.ServerCNN.ToDescription());

                    case ServerTypes.ServerThat:
                        return DNSHelper.GetDomainAddressServer(ServerTypes.ServerThat.ToDescription(), PortSignalRTypes.ServerThat.ToDescription());

                    case ServerTypes.ServerTest:
                        return ServerTypes.ServerTest.ToDescription();

                    case ServerTypes.ServerTestCNN:
                        return DNSHelper.GetIPAddressServer(ServerTypes.ServerTestCNN.ToDescription(), PortSignalRTypes.ServerTestCNN.ToDescription());

                    case ServerTypes.ServerVNSAT:
                        return DNSHelper.GetIPAddressServer(ServerTypes.ServerVNSAT.ToDescription(), PortSignalRTypes.ServerVNSAT.ToDescription());

                    case ServerTypes.ServerGISVIET:
                        return DNSHelper.GetIPAddressServer(ServerTypes.ServerGISVIET.ToDescription(), PortSignalRTypes.ServerGISVIET.ToDescription());

                    case ServerTypes.ServerVIVIEW:
                        return DNSHelper.GetIPAddressServer(ServerTypes.ServerVIVIEW.ToDescription(), PortSignalRTypes.ServerVIVIEW.ToDescription());

                    case ServerTypes.ServerVMS:
                        return DNSHelper.GetDomainAddressServer(ServerTypes.ServerVMS.ToDescription(), PortSignalRTypes.ServerVMS.ToDescription());

                    case ServerTypes.ServerMoto:
                        return DNSHelper.GetIPAddressServer(ServerTypes.ServerMoto.ToDescription(), PortSignalRTypes.ServerMoto.ToDescription());

                    default:
                        return DNSHelper.GetIPAddressServer(ServerTypes.ServerTest.ToDescription(), PortSignalRTypes.ServerTest.ToDescription());
                }
            }
        }

        /// <summary>
        /// IP của Service API
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  16/1/2018   created
        /// </Modified>
        public static string ApiEndpoint
        {
            get
            {
                switch (ApiEndpointTypes)
                {
                    case ApiEndpointTypes.ServerCNN:
                        return ApiEndpointTypes.ServerCNN.ToDescription();

                    case ApiEndpointTypes.ServerThat:
                        return ApiEndpointTypes.ServerThat.ToDescription();

                    case ApiEndpointTypes.ServerNamth:
                        return ApiEndpointTypes.ServerNamth.ToDescription();

                    case ApiEndpointTypes.ServerTest:
                        return ApiEndpointTypes.ServerTest.ToDescription();

                    case ApiEndpointTypes.ServerTestCNN:
                        return ApiEndpointTypes.ServerTestCNN.ToDescription();

                    case ApiEndpointTypes.ServerPhuongPV:
                        return ApiEndpointTypes.ServerPhuongPV.ToDescription();

                    case ApiEndpointTypes.ServerLinhLV:
                        return ApiEndpointTypes.ServerLinhLV.ToDescription();

                    case ApiEndpointTypes.ServerVNSAT:
                        return ApiEndpointTypes.ServerVNSAT.ToDescription();

                    case ApiEndpointTypes.ServerGISVIET:
                        return ApiEndpointTypes.ServerGISVIET.ToDescription();

                    case ApiEndpointTypes.ServerVIVIEW:
                        return ApiEndpointTypes.ServerVIVIEW.ToDescription();

                    case ApiEndpointTypes.ServerVMS:
                        return ApiEndpointTypes.ServerVMS.ToDescription();

                    case ApiEndpointTypes.ServerMoto:
                        return ApiEndpointTypes.ServerMoto.ToDescription();

                    default:
                        return ApiEndpointTypes.ServerTest.ToDescription();
                }
            }
        }
    }
}