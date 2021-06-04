using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ExportVideoPageViewModel : ViewModelBase
    {
        private readonly IStreamCameraService _streamCameraService;
        public ICommand SetHostspotCommand { get; }

        public ICommand GotoLinkExportCommand { get; }

        public ICommand GotoLinkHelpCommand { get; }

        public ICommand SelectVehicleCameraCommand { get; }

        public ExportVideoPageViewModel(INavigationService navigationService, IStreamCameraService streamCameraService) : base(navigationService)
        {
            _streamCameraService = streamCameraService;
            SetHostspotCommand = new DelegateCommand(SetHostspot);
            GotoLinkExportCommand = new DelegateCommand(GotoLinkExport);
            GotoLinkHelpCommand = new DelegateCommand(GotoLinkHelp);
            SelectVehicleCameraCommand = new DelegateCommand(SelectVehicleCamera);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            //Check parameter key
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<CameraLookUpVehicleModel>(ParameterKey.Vehicle) is CameraLookUpVehicleModel vehicle)
            {
                Vehicle = vehicle;
                WifiName = Vehicle.VehiclePlate + "-" + Vehicle.Imei.Substring(Vehicle.Imei.Length - 6);
                WifiPassword = string.Format("ba@{0}", DateTime.Now.ToString("ddMMyy"));
            }

            base.OnNavigatedTo(parameters);
        }

        private CameraLookUpVehicleModel vehicle = new CameraLookUpVehicleModel();
        public CameraLookUpVehicleModel Vehicle { get => vehicle; set => SetProperty(ref vehicle, value); }

        private string wifiName;
        public string WifiName { get => wifiName; set => SetProperty(ref wifiName, value); }

        private string wifiPassword;
        public string WifiPassword { get => wifiPassword; set => SetProperty(ref wifiPassword, value); }

        private void SelectVehicleCamera()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleCameraLookup", null, useModalNavigation: true, animated: true);
            });
        }

        private void SetHostspot()
        {
            SafeExecute(() =>
            {
                if (Vehicle != null && Vehicle.VehicleId > 0)
                {
                    RunOnBackground(async () =>
                    {
                        return await _streamCameraService.SetHotspot(UserInfo.XNCode, Vehicle.VehiclePlate, 1);
                    },
                   (result) =>
                   {
                       if (result)
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               var action = await PageDialog.DisplayAlertAsync("Thông báo", "Bật wifi thiết bị thành công! Tìm và kết nối với wifi của thiết bị trong cài đặt wifi", "Đến cài đặt", "Đóng");
                               DependencyService.Get<ISettingsService>().OpenWifiSettings();
                           });
                       }
                       else
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               var messgae = string.Format("Bật wifi thiết bị không thành công! Vui lòng thử lại hoặc gọi số tổng đài {0} để được hỗ trợ ",
                                   MobileSettingHelper.HotlineGps);
                               await PageDialog.DisplayAlertAsync("Thông báo", messgae, "Đóng");
                           });
                       }
                   }, showLoading: true);
                }
                else
                {
                    DisplayMessage.ShowMessageInfo("Vui lòng chọn phương tiện của bạn");
                }
            });
        }

        private void GotoLinkExport()
        {
            SafeExecute(async () =>
            {
                await Launcher.OpenAsync(new Uri(MobileSettingHelper.LinkExportVideoCamera));
            });
        }

        private void GotoLinkHelp()
        {
            SafeExecute(async () =>
            {
                await Launcher.OpenAsync(new Uri(MobileSettingHelper.LinkHelpExportVideoCamera));
            });
        }
    }
}