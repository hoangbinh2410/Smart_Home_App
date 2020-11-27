using System;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IVehicleOnlineHubService
    {
        Task Connect();

        Task Disconnect();

        bool IsConnectedOrConnecting();

        void JoinGroupReceivedVehicleID(string vehicleIds);

        void LeaveGroupReceivedVehicleID(string vehicleIds);

        event EventHandler<string> onReceiveSendCarSignalR;

        event Action ConnectionReconnecting;

        event Action ConnectionReconnected;

        event Action ConnectionClosed;
    }
}