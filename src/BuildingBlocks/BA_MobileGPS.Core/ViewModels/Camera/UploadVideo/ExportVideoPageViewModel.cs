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
            }

            base.OnNavigatedTo(parameters);
        }

        private CameraLookUpVehicleModel vehicle = new CameraLookUpVehicleModel();
        public CameraLookUpVehicleModel Vehicle { get => vehicle; set => SetProperty(ref vehicle, value); }

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
                               var messgae = string.Format("Wifi trên thiết bị đang được bật!\nVui lòng kết nối điện thoại của bạn với wifi “{0}” để trích xuất dữ liệu từ thiết bị này.", Vehicle.VehiclePlate + "-" + Vehicle.Imei.Substring(Vehicle.Imei.Length - 6));
                               var action = await PageDialog.DisplayAlertAsync("Thông báo", messgae, "Đến cài đặt", "Bỏ qua");
                               if (action)
                               {
                                   DependencyService.Get<ISettingsService>().OpenWifiSettings();
                               }
                           });
                       }
                       else
                       {
                           DisplayMessage.ShowMessageInfo("Wifi trên thiết bị chưa bật thành công, vui lòng thử lại");
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
                await Launcher.OpenAsync(new Uri("http://192.168.43.1:8080"));
            });
        }

        private void GotoLinkHelp()
        {
            SafeExecute(async () =>
            {
                await Launcher.OpenAsync(new Uri("https://www.youtube.com/watch?v=3KHS015dexo"));
            });
        }
    }
}