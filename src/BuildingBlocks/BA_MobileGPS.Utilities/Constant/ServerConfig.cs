using BA_MobileGPS.Utilities.Enums;
using BA_MobileGPS.Utilities.Extensions;

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
        /// Loại Server là gì
        /// Mặc định là:            ServerTypes.Server20
        /// Khi chạy thật là:       ServerTypes.ServerThat
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  16/1/2018   created
        /// </Modified>
        public static ServerIdentityHubTypes ServerIdentityHubType = ServerIdentityHubTypes.ServerThat;

        /// <summary>
        /// Loại Server là gì
        /// Mặc định là:            ServerTypes.Server20
        /// Khi chạy thật là:       ServerTypes.ServerThat
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  16/1/2018   created
        /// </Modified>
        public static ServerVehicleOnlineHubTypes ServerVehicleOnlineHubType = ServerVehicleOnlineHubTypes.ServerThat;

        /// <summary>
        /// Loại Server là gì
        /// Mặc định là:            ServerTypes.Server20
        /// Khi chạy thật là:       ServerTypes.ServerThat
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  16/1/2018   created
        /// </Modified>
        public static ServerAlertHubTypes ServerAlertHubType = ServerAlertHubTypes.ServerThat;

        public static ServerUserBehaviorHubTypes ServerUserBehaviorHubType = ServerUserBehaviorHubTypes.ServerThat;

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

                    case ApiEndpointTypes.ServerLinhLV:
                        return ApiEndpointTypes.ServerLinhLV.ToDescription();

                    case ApiEndpointTypes.ServerDongLH:
                        return ApiEndpointTypes.ServerDongLH.ToDescription();

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

                    case ApiEndpointTypes.ServerUNITEL:
                        return ApiEndpointTypes.ServerUNITEL.ToDescription();

                    case ApiEndpointTypes.ServerGSHT:
                        return ApiEndpointTypes.ServerGSHT.ToDescription();
                        
                    default:
                        return ApiEndpointTypes.ServerTest.ToDescription();
                }
            }
        }

        /// <summary>
        /// IP server chạy TCP
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  16/1/2018   created
        /// </Modified>
        public static string ServerIdentityHubIP
        {
            get
            {
                switch (ServerIdentityHubType)
                {
                    case ServerIdentityHubTypes.ServerCNN:
                        return ServerIdentityHubTypes.ServerCNN.ToDescription();

                    case ServerIdentityHubTypes.ServerThat:
                        return ServerIdentityHubTypes.ServerThat.ToDescription();

                    case ServerIdentityHubTypes.ServerTest:
                        return ServerIdentityHubTypes.ServerTest.ToDescription();

                    case ServerIdentityHubTypes.ServerVIVIEW:
                        return ServerIdentityHubTypes.ServerVIVIEW.ToDescription();

                    case ServerIdentityHubTypes.ServerGISVIET:
                        return ServerIdentityHubTypes.ServerGISVIET.ToDescription();

                    case ServerIdentityHubTypes.ServerUNITEL:
                        return ServerIdentityHubTypes.ServerUNITEL.ToDescription();

                    case ServerIdentityHubTypes.ServerGSHT:
                        return ServerIdentityHubTypes.ServerGSHT.ToDescription();

                    case ServerIdentityHubTypes.ServerVMS:
                        return ServerIdentityHubTypes.ServerVMS.ToDescription();

                    case ServerIdentityHubTypes.ServerMoto:
                        return ServerIdentityHubTypes.ServerMoto.ToDescription();

                    default:
                        return ServerIdentityHubTypes.ServerTest.ToDescription();
                }
            }
        }

        /// <summary>
        /// IP server chạy TCP
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  16/1/2018   created
        /// </Modified>
        public static string ServerVehicleOnlineHubIP
        {
            get
            {
                switch (ServerVehicleOnlineHubType)
                {
                    case ServerVehicleOnlineHubTypes.ServerCNN:
                        return ServerVehicleOnlineHubTypes.ServerCNN.ToDescription();

                    case ServerVehicleOnlineHubTypes.ServerThat:
                        return ServerVehicleOnlineHubTypes.ServerThat.ToDescription();

                    case ServerVehicleOnlineHubTypes.ServerTest:
                        return ServerVehicleOnlineHubTypes.ServerTest.ToDescription();

                    case ServerVehicleOnlineHubTypes.ServerVIVIEW:
                        return ServerVehicleOnlineHubTypes.ServerVIVIEW.ToDescription();

                    case ServerVehicleOnlineHubTypes.ServerGISVIET:
                        return ServerVehicleOnlineHubTypes.ServerGISVIET.ToDescription();

                    case ServerVehicleOnlineHubTypes.ServerUNITEL:
                        return ServerVehicleOnlineHubTypes.ServerUNITEL.ToDescription();

                    case ServerVehicleOnlineHubTypes.ServerGSHT:
                        return ServerVehicleOnlineHubTypes.ServerGSHT.ToDescription();

                    case ServerVehicleOnlineHubTypes.ServerVMS:
                        return ServerVehicleOnlineHubTypes.ServerVMS.ToDescription();

                    case ServerVehicleOnlineHubTypes.ServerMoto:
                        return ServerVehicleOnlineHubTypes.ServerMoto.ToDescription();

                    default:
                        return ServerVehicleOnlineHubTypes.ServerTest.ToDescription();
                }
            }
        }

        /// <summary>
        /// IP server chạy TCP
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  16/1/2018   created
        /// </Modified>
        public static string ServerAlertHubIP
        {
            get
            {
                switch (ServerAlertHubType)
                {
                    case ServerAlertHubTypes.ServerCNN:
                        return ServerAlertHubTypes.ServerCNN.ToDescription();

                    case ServerAlertHubTypes.ServerThat:
                        return ServerAlertHubTypes.ServerThat.ToDescription();

                    case ServerAlertHubTypes.ServerTest:
                        return ServerAlertHubTypes.ServerTest.ToDescription();

                    case ServerAlertHubTypes.ServerVIVIEW:
                        return ServerAlertHubTypes.ServerVIVIEW.ToDescription();

                    case ServerAlertHubTypes.ServerGISVIET:
                        return ServerAlertHubTypes.ServerGISVIET.ToDescription();

                    case ServerAlertHubTypes.ServerUNITEL:
                        return ServerAlertHubTypes.ServerUNITEL.ToDescription();

                    case ServerAlertHubTypes.ServerGSHT:
                        return ServerVehicleOnlineHubTypes.ServerGSHT.ToDescription();

                    case ServerAlertHubTypes.ServerVMS:
                        return ServerAlertHubTypes.ServerVMS.ToDescription();

                    case ServerAlertHubTypes.ServerMoto:
                        return ServerAlertHubTypes.ServerMoto.ToDescription();

                    default:
                        return ServerAlertHubTypes.ServerTest.ToDescription();
                }
            }
        }

        public static string ServerUserBehaviorHubIP
        {
            get
            {
                switch (ServerUserBehaviorHubType)
                {
                    case ServerUserBehaviorHubTypes.ServerCNN:
                        return ServerUserBehaviorHubTypes.ServerCNN.ToDescription();

                    case ServerUserBehaviorHubTypes.ServerThat:
                        return ServerUserBehaviorHubTypes.ServerThat.ToDescription();

                    case ServerUserBehaviorHubTypes.ServerTest:
                        return ServerUserBehaviorHubTypes.ServerTest.ToDescription();

                    case ServerUserBehaviorHubTypes.ServerVIVIEW:
                        return ServerUserBehaviorHubTypes.ServerVIVIEW.ToDescription();

                    case ServerUserBehaviorHubTypes.ServerGISVIET:
                        return ServerUserBehaviorHubTypes.ServerGISVIET.ToDescription();

                    case ServerUserBehaviorHubTypes.ServerUNITEL:
                        return ServerUserBehaviorHubTypes.ServerUNITEL.ToDescription();

                    case ServerUserBehaviorHubTypes.ServerGSHT:
                        return ServerUserBehaviorHubTypes.ServerGSHT.ToDescription();

                    case ServerUserBehaviorHubTypes.ServerVMS:
                        return ServerUserBehaviorHubTypes.ServerVMS.ToDescription();

                    case ServerUserBehaviorHubTypes.ServerMoto:
                        return ServerUserBehaviorHubTypes.ServerMoto.ToDescription();

                    default:
                        return ServerUserBehaviorHubTypes.ServerTest.ToDescription();
                }
            }
        }
    }
}