using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class IdentityHubService : IIdentityHubService
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
                var url = string.Format("{0}/identityHub?PK_UserID={1}&CompanyId={2}", ServerConfig.ServerIdentityHubIP, StaticSettings.User.UserId.ToString().ToUpper(),
                    StaticSettings.User.CompanyId.ToString().ToUpper());
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

                    _connection.On("PushLogoutToAllUserInCompany", (string message) => onReceivePushLogoutToAllUserInCompany(this, message));

                    _connection.On("PushLogoutToUser", (string message) => onReceivePushLogoutToUser(this, message));
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

        public async void PushLogoutToAllUserInCompany(string companyid, string message)
        {
            try
            {
                if (_connection.State == HubConnectionState.Connected)
                {
                    await _connection.InvokeAsync("PushLogoutToAllUserInCompany", companyid, message);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        public async void PushLogoutToUser(string who, string message)
        {
            try
            {
                if (_connection.State == HubConnectionState.Connected)
                {
                    await _connection.InvokeAsync("PushLogoutToUser", who, message);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }


        public event Action ConnectionReconnecting;

        public event Action ConnectionReconnected;

        public event Action ConnectionClosed;

        public event EventHandler<string> onReceivePushLogoutToAllUserInCompany;
        public event EventHandler<string> onReceivePushLogoutToUser;
    }
}
