using System;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IAlertHubService
    {
        Task Connect();

        Task Disconnect();

        bool IsConnectedOrConnecting();


        event EventHandler<string> onReceiveAlertSignalR;

        event Action ConnectionReconnecting;

        event Action ConnectionReconnected;

        event Action ConnectionClosed;
    }
}