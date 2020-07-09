using BA_MobileGPS.Core;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using VMS_MobileGPS.Models;
using VMS_MobileGPS.Service;

using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace VMS_MobileGPS.ViewModels
{
    public class BluetoothViewModel : VMSBaseViewModel
    {
        private readonly IDisplayMessage DisplayMessage;
        private readonly IBluetoothService bluetoothHelper;

        private CancellationTokenSource cts;

        public BluetoothViewModel(INavigationService navigationService, IDisplayMessage displayMessage) : base(navigationService)
        {
            DisplayMessage = displayMessage;

            bluetoothHelper = AppManager.BluetoothService;

            listBluetoothDevice = new ObservableCollection<BluetoothDevice>();
        }

        public override void OnPageAppearingFirstTime()
        {
            SearchBluetooth();
        }

        #region Property

        private ObservableCollection<BluetoothDevice> listBluetoothDevice = new ObservableCollection<BluetoothDevice>();
        public ObservableCollection<BluetoothDevice> ListBluetoothDevice { get => listBluetoothDevice; set => SetProperty(ref listBluetoothDevice, value); }

        public List<BluetoothDevice> ListBluetoothDeviceOrigin = new List<BluetoothDevice>();

        private bool hasBluetooth = true;
        public bool HasBluetooth { get => hasBluetooth; set => SetProperty(ref hasBluetooth, value); }

        private bool hasFindBle = false;
        public bool HasFindBle { get => hasFindBle; set => SetProperty(ref hasFindBle, value); }

        #endregion Property

        #region Private Method

        public ICommand SearchBluetoothCommand => new DelegateCommand(SearchBluetooth);

        private async void SearchBluetooth()
        {
            try
            {
                if (HasFindBle)
                    return;

                HasFindBle = true;

                await bluetoothHelper.Scan(5000);

                var devicelist = bluetoothHelper.DeviceList;
                if (devicelist != null && devicelist.Count > 0)
                {
                    var listBlut = new List<BluetoothDevice>();
                    devicelist.ForEach(x =>
                    {
                        if (!string.IsNullOrEmpty(x.Name))
                        {
                            listBlut.Add(new BluetoothDevice()
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Rssi = x.Rssi
                            });
                        }
                    });
                    ListBluetoothDeviceOrigin = listBlut;
                    ListBluetoothDevice = listBlut.ToObservableCollection();
                }
                else
                {
                    ListBluetoothDeviceOrigin = new List<BluetoothDevice>();
                    ListBluetoothDevice = new ObservableCollection<BluetoothDevice>();
                }

                HasBluetooth = ListBluetoothDevice.Count > 0;
            }
            catch (Exception)
            {
                DisplayMessage.ShowMessageInfo("Có lỗi xảy ra, vui lòng thử lại.");
            }
            finally
            {
                HasFindBle = false;
            }
        }

        public ICommand SearchCommand => new DelegateCommand<TextChangedEventArgs>(SearchBluetoothWithText);

        private void SearchBluetoothWithText(TextChangedEventArgs args)
        {
            if (cts != null)
                cts.Cancel(true);

            cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                await Task.Delay(500, cts.Token);

                return ListBluetoothDeviceOrigin.FindAll(v => v.Name.ToUpper().Contains(args.NewTextValue.ToUpper()));
            }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    ListBluetoothDevice = task.Result.ToObservableCollection();

                    HasBluetooth = ListBluetoothDevice.Count > 0;
                }
            }));
        }

        public ICommand TapListVehicleCommand => new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(async (args) =>
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                var selected = (args.ItemData as BluetoothDevice);
                if (selected != null)
                {
                    var blueselected = bluetoothHelper.DeviceList.FirstOrDefault(x => x.Name == selected.Name);
                    if (blueselected != null)
                    {
                        using (new HUDService())
                        {
                            var ret = await bluetoothHelper.Connect(blueselected, 3000);
                            if (!ret.Data)
                            {
                                DisplayMessage.ShowMessageInfo("Không kết nối được đến thiết bị");
                            }
                            else
                            {
                                ListBluetoothDevice.Remove(selected);
                            }
                        }
                    }
                    else
                    {
                        DisplayMessage.ShowMessageInfo("Không kết nối được đến thiết bị");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                IsBusy = false;
            }
        });

        public ICommand DisconnectBluetoothCommand => new DelegateCommand(DisconnectBLE);

        private void DisconnectBLE()
        {
            SafeExecute(async () =>
            {
                var action = await PageDialog.DisplayAlertAsync("BA SAT", "Bạn có muốn ngắt kết nối bluetooth không?", "Ngắt kết nối", "Bỏ qua");
                if (action)
                {
                    await bluetoothHelper.Disconnect();

                    SearchBluetooth();
                }
            });
        }

        #endregion Private Method
    }
}