using System;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IIdentityHubService
    {
        Task Connect();

        Task Disconnect();

        bool IsConnectedOrConnecting();

        void PushLogoutToAllUserInCompany(string companyid, string message);

        void PushLogoutToUser(string who, string message);

        void PushMessageToUser(string who, string message);

        event EventHandler<string> onReceivePushLogoutToAllUserInCompany;

        event EventHandler<string> onReceivePushLogoutToUser;

        event EventHandler<string> onReceivePushMessageToUser;

        event Action ConnectionReconnecting;

        event Action ConnectionReconnected;

        event Action ConnectionClosed;
    }
}