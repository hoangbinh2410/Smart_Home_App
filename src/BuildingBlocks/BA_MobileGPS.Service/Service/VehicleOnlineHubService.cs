using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class VehicleOnlineHubService : IVehicleOnlineHubService
    {
        private static HubConnection _connection
        {
            get;
            set;
        }

        public async Task Connect()
        {
            try
            {
                // Connect to the server
                var url = string.Format("{0}/onlineHub?PK_UserID={1}", ServerConfig.ServerVehicleOnlineHubIP, StaticSettings.User.UserId.ToString().ToUpper());
                _connection = new HubConnectionBuilder()
                      .AddJsonProtocol()
                      .WithUrl(url)
                      .Build();
                _connection.Reconnecting += Reconnecting;
                _connection.Reconnected += Reconnected;
                _connection.Closed += Disconnected;
                if (_connection.State == HubConnectionState.Disconnected)
                {
                    await _connection.StartAsync();

                    _connection.On("SendCarSignalRByGroup", (string message) => onReceiveSendCarSignalR(this, message));

                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        public async void JoinGroupReceivedVehicleID(string vehicleIds)
        {
            try
            {
                if (_connection.State == HubConnectionState.Connected)
                {
                    await _connection.InvokeAsync("JoinGroupByVehicleId", vehicleIds);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        public async void LeaveGroupReceivedVehicleID(string vehicleIds)
        {
            try
            {
                await _connection.InvokeAsync("LeaveGroupByVehicleId", vehicleIds);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        public async Task Disconnect()
        {
            try
            {
                if (_connection != null && _connection.State != HubConnectionState.Disconnected)
                    await _connection.StopAsync();
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        private async Task Disconnected(Exception arg)
        {
            ConnectionClosed?.Invoke();
        }

        private async Task Reconnected(string arg)
        {
            ConnectionReconnected?.Invoke();
        }

        private async Task Reconnecting(Exception arg)
        {
            ConnectionReconnecting?.Invoke();
        }

        public bool IsConnectedOrConnecting()
        {
            if (_connection != null)
            {
                return _connection.State == HubConnectionState.Connected;
            }
            else
            {
                return false;
            }
        }

        public event EventHandler<string> onReceiveSendCarSignalR;

        public event Action ConnectionReconnecting;

        public event Action ConnectionReconnected;

        public event Action ConnectionClosed;
    }
}