using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Utilities;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class UserBahaviorHubService : IUserBahaviorHubService
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
                var url = string.Format("{0}/userBehaviorHub?PK_UserID={1}", "http://192.168.1.49:8028", StaticSettings.User.UserId.ToString().ToUpper());
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
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        public async void SendUserBehavior(UserBehaviorRequest request)
        {
            try
            {
                if (_connection.State == HubConnectionState.Connected)
                {
                    await _connection.InvokeAsync("SendUserBehavior", request);
                }
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

        public event Action ConnectionReconnecting;

        public event Action ConnectionReconnected;

        public event Action ConnectionClosed;
    }
}