using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using Microsoft.AspNet.SignalR.Client;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class SignalRService : ISignalRServices
    {
        private static HubConnection _connection
        {
            get;
            set;
        }

        private static IHubProxy _proxy
        {
            get;
            set;
        }

        public async Task Connect(bool dismissCurrentConnection = false)
        {
            Debug.WriteLine("Connect SignalR:Connec ");
            try
            {
                if (_connection != null)
                {
                    if (!dismissCurrentConnection && _connection.State == ConnectionState.Connected)
                    {
                        return;
                    }
                    _connection.Reconnecting -= Reconnecting;
                    _connection.Reconnected -= Reconnected;
                    _connection.Closed -= Disconnected;

                    // DON´T call connection.Dispose() or it may block for 20 seconds
                    _connection = null;
                    _proxy = null;
                }
                // Connect to the server
                var query = new Dictionary<string, string>();
                query.Add("Name", StaticSettings.User.UserId.ToString().ToUpper());
                _connection = new HubConnection(ServerConfig.ServerIP, query);
                _proxy = _connection.CreateHubProxy("GPSMobileHub");
                _connection.Reconnecting += Reconnecting;
                _connection.Reconnected += Reconnected;
                _connection.Closed += Disconnected;
                Debug.WriteLine("Connect SignalR:Connecting " + StaticSettings.User.UserId.ToString().ToUpper());
                if (_connection.State == ConnectionState.Disconnected)
                {
                    Debug.WriteLine("Connect SignalR:Connected " + ServerConfig.ServerIP.ToUpper());
                    await _connection.Start();

                    _proxy.On("sendCarSignalR", (string message) => onReceiveSendCarSignalR(this, message));

                    _proxy.On("pushAlertToUsers", (string message) => onReceiveAlertSignalR(this, message));

                    _proxy.On("sendNotificationToUser", (int
                        message) => onReceiveNotificationSignalR(this, message));

                    _proxy.On("logoutAllUserInCompany", (string message) => onReceiveLogoutAllUserInCompany(this, message));

                    Debug.WriteLine("Connect SignalR:Connected 1" + ServerConfig.ServerIP.ToString().ToUpper());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        public void Disconnect()
        {
            try
            {
                if (_connection != null && _connection.State != ConnectionState.Disconnected)
                    _connection.Stop();
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        private void OnConnectionStateChangedHandler(StateChange change)
        {
            if (StaticSettings.User != null
                && change.NewState == ConnectionState.Disconnected)
            {
                // SignalR doesn´t do anything after disconnected state, so we need to manually reconnect
                //await Connect(true);
            }
        }

        public void JoinGroupReceivedCarOnline(string vehiclePlates)
        {
            try
            {
                if (_connection.State == ConnectionState.Connected)
                {
                    _proxy.Invoke("JoinGroupVehicePlate", vehiclePlates);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        public void LeaveGroupReceivedCarOnline(string vehiclePlates)
        {
            try
            {
                _proxy.Invoke("LeaveGroupVehicePlate", vehiclePlates);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        public void JoinGroupReceivedVehicleID(string vehicleIds)
        {
            try
            {
                if (_connection.State == ConnectionState.Connected)
                {
                    _proxy.Invoke("JoinGroupByVehicleId", vehicleIds);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        public void LeaveGroupReceivedVehicleID(string vehicleIds)
        {
            try
            {
                _proxy.Invoke("LeaveGroupByVehicleId", vehicleIds);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }


        public void JoinGroupCompany(string companyid)
        {
            try
            {
                if (_connection.State == ConnectionState.Connected)
                {
                    _proxy.Invoke("JoinGroupCompany", companyid);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        public void LeaveGroupCompany(string companyid)
        {
            try
            {
                _proxy.Invoke("LeaveGroupCompany", companyid);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        private void Disconnected()
        {
            ConnectionClosed?.Invoke();
        }

        private void Reconnected()
        {
            ConnectionReconnected?.Invoke();
        }

        private void Reconnecting()
        {
            ConnectionReconnecting?.Invoke();
        }

        public bool IsConnectedOrConnecting()
        {
            if (_connection != null)
            {
                return _connection.State == ConnectionState.Connected;
            }
            else
            {
                return false;
            }
        }

        public event EventHandler<string> onReceiveSendCarSignalR;

        public event EventHandler<string> onReceiveAlertSignalR;

        public event EventHandler<int> onReceiveNotificationSignalR;

        public event Action ConnectionReconnecting;

        public event Action ConnectionReconnected;

        public event Action ConnectionClosed;


        public event EventHandler<string> onReceiveLogoutAllUserInCompany;
    }
}