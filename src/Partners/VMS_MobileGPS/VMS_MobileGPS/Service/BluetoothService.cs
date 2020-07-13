using BA_MobileGPS.Core;

using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using VMS_MobileGPS.Constant;

using Xamarin.Forms;

namespace VMS_MobileGPS.Service
{
    public class BluetoothService : IBluetoothService
    {
        private const string ServiceID = "6e400001-b5a3-f393-e0a9-e50e24dcca9e";
        private const string RxCharID = "6e400003-b5a3-f393-e0a9-e50e24dcca9e";
        private const string TxCharID = "6e400002-b5a3-f393-e0a9-e50e24dcca9e";

        private readonly IBluetoothLE BLE;
        private readonly IAdapter Adapter;

        public static IDevice ConnectedDevice { get; set; }

        public static ICharacteristic RxCharacteristic { get; set; }

        public static ICharacteristic TxCharacteristic { get; set; }

        public List<IDevice> DeviceList { get; set; }

        public event EventHandler<string> OnReceiveNotificationBLE;

        public BluetoothService()
        {
            BLE = CrossBluetoothLE.Current;
            Adapter = CrossBluetoothLE.Current.Adapter;

            DeviceList = new List<IDevice>();

            Adapter.DeviceDiscovered -= Adapter_DeviceDiscovered;
            Adapter.DeviceDiscovered += Adapter_DeviceDiscovered;

            // Thiết bị ghép nối BLE chủ động ngắt kết nối
            Adapter.DeviceDisconnected -= Adapter_DeviceDisconnected;
            Adapter.DeviceDisconnected += Adapter_DeviceDisconnected;

            // Mất kết nối với thiết bị BLE (VD: Tắt nguồn BLE device)
            Adapter.DeviceConnectionLost -= Adapter_DeviceConnectionLost;
            Adapter.DeviceConnectionLost += Adapter_DeviceConnectionLost;
        }

        public BleConnectionState State
        {
            get
            {
                if (TxCharacteristic == null || ConnectedDevice == null || ConnectedDevice.State != DeviceState.Connected)
                    return BleConnectionState.NO_CONNECTION;
                else
                    return BleConnectionState.CONNECTED;
            }
        }

        private void Adapter_DeviceDiscovered(object sender, DeviceEventArgs a)
        {
            DeviceList.Add(a.Device);
        }

        private Task<BluetoothState> GetBluetoothState()
        {
            var tcs = new TaskCompletionSource<BluetoothState>();

            if (BLE.State != BluetoothState.Unknown)
            {
                // If we can detect state out of box just returning in
                tcs.SetResult(BLE.State);
            }
            else
            {
                BLE.StateChanged += handler;

                // Otherwise let's setup dynamic event handler and wait for first state update
                void handler(object o, BluetoothStateChangedArgs e)
                {
                    BLE.StateChanged -= handler;
                    // and return it as our state
                    // we can have an 'Unknown' check here, but in normal situation it should never occur
                    tcs.SetResult(e.NewState);
                }
            }

            return tcs.Task;
        }

        public async Task Scan(int Timeout = 10000)
        {
            // Yêu cầu bật Bluetooth
            var state = await GetBluetoothState();
            if (state != BluetoothState.On)
            {
                var action = await App.Current.MainPage.DisplayAlert("Thông báo", "Hãy bật Bluetooth để quét tìm thiết bị", "Đồng ý", "Bỏ qua");
                if (action == false) return;
                DependencyService.Get<ISettingsService>().OpenBluetoothSettings();
            }
            else
            {
                DeviceList.Clear();
                Adapter.ScanMode = ScanMode.LowLatency;
                if (Device.RuntimePlatform == Device.iOS)
                {
                    Adapter.ScanTimeout = Timeout;

                    if (!BLE.Adapter.IsScanning)
                    {
                        await Adapter.StartScanningForDevicesAsync();
                    }
                }
                else
                {
                    //nếu là Android thì mới cần có location thì mới kết nối được BLE
                    if (await PermissionHelper.CheckLocationPermissions())
                    {
                        Adapter.ScanTimeout = Timeout;

                        if (!BLE.Adapter.IsScanning)
                        {
                            await Adapter.StartScanningForDevicesAsync();
                        }
                    }
                }
            }
        }

        public async Task<BaseResponse<bool>> Connect(IDevice device, int Timeout = 10000)
        {
            BaseResponse<bool> rtnValue = new BaseResponse<bool>
            {
                Data = false
            };

            try
            {
                var state = await GetBluetoothState();
                if (state != BluetoothState.On)
                {
                    var action = await App.Current.MainPage.DisplayAlert("Thông báo", "Hãy bật Bluetooth để quét tìm thiết bị", "Đồng ý", "Bỏ qua");
                    if (action == false) return rtnValue;
                    DependencyService.Get<ISettingsService>().OpenBluetoothSettings();
                }
                else
                {
                    using (var cts = new CancellationTokenSource(Timeout))
                    {
                        if (device.State == DeviceState.Connected)
                        {
                            await device.UpdateRssiAsync();
                        }
                        else
                        {
                            await Adapter.ConnectToDeviceAsync(device, new ConnectParameters(autoConnect: false, forceBleTransport: false), cts.Token);

                            ConnectedDevice = device;

                            using (var service = await ConnectedDevice.GetServiceAsync(Guid.Parse(ServiceID)))
                            {
                                if (service == null)
                                {
                                    return rtnValue;
                                }
                                RxCharacteristic = await service.GetCharacteristicAsync(Guid.Parse(RxCharID));
                                if (RxCharacteristic == null)
                                {
                                    return rtnValue;
                                }

                                if (RxCharacteristic.CanUpdate == false)
                                {
                                    return rtnValue;
                                }

                                await RxCharacteristic.StartUpdatesAsync();

                                TxCharacteristic = await service.GetCharacteristicAsync(Guid.Parse(TxCharID));

                                if (TxCharacteristic == null)
                                {
                                    return rtnValue;
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                rtnValue.Data = false;
                rtnValue.Error = true;
                rtnValue.Message = ex.Message;
                return rtnValue;
            }

            RxCharacteristic.ValueUpdated -= BltCharacteristic_ValueUpdated;
            RxCharacteristic.ValueUpdated += BltCharacteristic_ValueUpdated;

            GlobalResourcesVMS.Current.DeviceManager.State = BleConnectionState.CONNECTED;
            GlobalResourcesVMS.Current.DeviceManager.DeviceName = ConnectedDevice.Name;

            rtnValue.Data = true;
            rtnValue.Error = false;
            rtnValue.Message = "Kết nối đến thiết bị thành công";
            return rtnValue;
        }

        public async Task Disconnect()
        {
            if (RxCharacteristic != null)
                RxCharacteristic.ValueUpdated -= BltCharacteristic_ValueUpdated;

            if (ConnectedDevice != null)
                await Adapter.DisconnectDeviceAsync(ConnectedDevice);

            GlobalResourcesVMS.Current.DeviceManager.State = BleConnectionState.NO_CONNECTION;
            GlobalResourcesVMS.Current.DeviceManager.DeviceName = string.Empty;
            GlobalResourcesVMS.Current.DeviceManager.DevicePlate = string.Empty;
        }

        public async Task<BaseResponse<bool>> Send(byte[] Packet)
        {
            BaseResponse<bool> rtnValue = new BaseResponse<bool>
            {
                Data = false,
                Error = true
            };

            if (TxCharacteristic == null || ConnectedDevice == null || ConnectedDevice.State != DeviceState.Connected)
            {
                rtnValue.Message = "Bạn vui lòng kết nối Bluetooth với thiết bị lắp trên tàu để thực hiện gửi SOS";
                return rtnValue;
            }

            int index = 0;
            while (index < Packet.Length)
            {
                int length = Packet.Length - index;
                if (length > 240) length = 240;

                byte[] send = new byte[length];
                Buffer.BlockCopy(Packet, index, send, 0, length);

                bool ret;
                try
                {
                    ret = await TxCharacteristic.WriteAsync(send);
                    index += length;
                }
                catch (Exception)
                {
                    rtnValue.Message = "Lỗi khi gửi dữ liệu Bluetooth";
                    return rtnValue;
                }

                if (ret == false) break;
            }

            rtnValue.Data = true;
            rtnValue.Error = false;
            return rtnValue;
        }

        public async Task<BaseResponse<bool>> Send(string Message)
        {
            Debug.Write(Message);
            return await Send(Encoding.ASCII.GetBytes(Message));
        }

        private void Adapter_DeviceConnectionLost(object sender, DeviceErrorEventArgs e)
        {
            ProcessDisconnect();
        }

        private void Adapter_DeviceDisconnected(object sender, DeviceEventArgs e)
        {
            ProcessDisconnect();
        }

        private void ProcessDisconnect()
        {
            if (RxCharacteristic != null)
                RxCharacteristic.ValueUpdated -= BltCharacteristic_ValueUpdated;

            GlobalResourcesVMS.Current.DeviceManager.DeviceName = string.Empty;
            GlobalResourcesVMS.Current.DeviceManager.DevicePlate = string.Empty;
            GlobalResourcesVMS.Current.DeviceManager.State = BleConnectionState.NO_CONNECTION;
        }

        private void BltCharacteristic_ValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
        {
            string value = e.Characteristic.StringValue;

            if (value.Contains(":PING"))
            {
                if (string.IsNullOrEmpty(GlobalResourcesVMS.Current.DeviceManager.DevicePlate))
                {
                    int index = value.IndexOf(":PING");

                    GlobalResourcesVMS.Current.DeviceManager.DevicePlate = value.Substring(0, index);

                    if (Settings.LastDeviceVMS != value.Substring(0, index))
                    {
                        Settings.LastDeviceVMS = value.Substring(0, index);
                    }

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Send("GCFG,22");

                        await Task.Delay(500);

                        await Send("GCFG,310");

                        await Task.Delay(500);

                        await Send("GCFG,999");
                    });
                    GlobalResourcesVMS.Current.DeviceManager.State = BleConnectionState.PING_OK;
                }
            }
            else
            {
                OnReceiveNotificationBLE(this, value);
            }
        }
    }

    public enum BleConnectionState : int
    {
        NO_CONNECTION = 0,
        CONNECTED = 1,
        PING_OK = 2
    }

    public class BaseResponse<T>
    {
        public int Status;
        public string Message;
        public bool Error;
        public T Data;

        public BaseResponse()
        {
            Error = true;
            Message = string.Empty;
        }
    }
}