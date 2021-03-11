using System;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IUserBahaviorHubService
    {
        Task Connect();

        Task Disconnect();

        bool IsConnectedOrConnecting();

        void SendUserBehavior(Guid userID, string page, int type, int apptype);

        event Action ConnectionReconnecting;

        event Action ConnectionReconnected;

        event Action ConnectionClosed;
    }
}