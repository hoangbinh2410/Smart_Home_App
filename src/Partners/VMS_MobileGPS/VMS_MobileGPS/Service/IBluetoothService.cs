using Plugin.BLE.Abstractions.Contracts;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VMS_MobileGPS.Service
{
    public interface IBluetoothService
    {
        List<IDevice> DeviceList { get; set; }

        Task Scan(int Timeout = 10000);

        Task<BaseResponse<bool>> Connect(IDevice device, int Timeout = 10000);

        Task Disconnect();

        Task<BaseResponse<bool>> Send(byte[] Packet);

        Task<BaseResponse<bool>> Send(string Message);

        event EventHandler<string> OnReceiveNotificationBLE;
    }
}