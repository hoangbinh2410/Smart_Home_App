using System;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface ISignalRServices
    {
        Task Connect(bool dismissCurrentConnection = false);

        void Disconnect();

        bool IsConnectedOrConnecting();

        void JoinGroupReceivedCarOnline(string vehiclePlates);

        void LeaveGroupReceivedCarOnline(string vehiclePlates);

        void JoinGroupReceivedVehicleID(string vehicleIds);

        void LeaveGroupReceivedVehicleID(string vehicleIds);

        void JoinGroupCompany(string companyid);

        void LeaveGroupCompany(string companyid);

        event EventHandler<string> onReceiveSendCarSignalR;

        event EventHandler<string> onReceiveAlertSignalR;

        event EventHandler<int> onReceiveNotificationSignalR;

        event EventHandler<string> onReceiveLogoutAllUserInCompany;



        event Action ConnectionReconnecting;

        event Action ConnectionReconnected;

        event Action ConnectionClosed;
    }
}