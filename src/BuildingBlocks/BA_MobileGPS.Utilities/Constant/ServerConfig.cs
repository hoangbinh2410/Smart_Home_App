﻿using BA_MobileGPS.Utilities.Enums;
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

                    case ServerVehicleOnlineHubTypes.ServerVMS:
                        return ServerVehicleOnlineHubTypes.ServerThat.ToDescription();

                    case ServerVehicleOnlineHubTypes.ServerMoto:
                        return ServerVehicleOnlineHubTypes.ServerMoto.ToDescription();

                    default:
                        return ServerVehicleOnlineHubTypes.ServerTest.ToDescription();
                }
            }
        }
    }
}