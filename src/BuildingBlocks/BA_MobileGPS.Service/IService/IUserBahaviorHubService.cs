using BA_MobileGPS.Entities.RequestEntity;
using System;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IUserBahaviorHubService
    {
        Task Connect();

        Task Disconnect();

        bool IsConnectedOrConnecting();

        void SendUserBehavior(UserBehaviorRequest request);

        event Action ConnectionReconnecting;

        event Action ConnectionReconnected;

        event Action ConnectionClosed;
    }
}