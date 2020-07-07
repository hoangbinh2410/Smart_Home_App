using BA_MobileGPS.Core;
using BA_MobileGPS.Utilities;
using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using VMS_MobileGPS.Constant;
using VMS_MobileGPS.Events;
using Xamarin.Forms;

namespace VMS_MobileGPS.ViewModels
{
    public class VMSPermissionGrantedViewModel : BindableBase
    {
        public VMSPermissionGrantedViewModel()
        {
            GrantAllCommand = new DelegateCommand(GrantAll);
            GrantPermissionCommand = new Command<PermissionOfApp>(GrantPermission);
            CloseCommand = new DelegateCommand(Close);
        }

        private async void GrantPermission(PermissionOfApp param)
        {
            var temp = GlobalResourcesVMS.Current.PermissionManager;

            switch (param)
            {
                case PermissionOfApp.Camera:
                    await GrantCameraPermission(temp.IsCameraGranted);
                    break;

                case PermissionOfApp.Location:
                    await GrantLocationPermission(temp.IsLocationGranted);
                    break;

                case PermissionOfApp.Photo:
                    await GrantPhotoPermission(temp.IsPhotoGranted);
                    break;

                case PermissionOfApp.Storage:
                    await GrantStoragePermission(temp.IsStorageGranted);
                    break;

                default:
                    break;
            }
        }

        private async Task<bool> GrantCameraPermission(bool param)
        {
            if (!param)
            {
                var status = await PermissionHelper.CheckCameraPermissions();
                if (status)
                {
                    GlobalResourcesVMS.Current.PermissionManager.IsCameraGranted = true;
                }
                else return false;
            }
            return true;
        }

        private async Task<bool> GrantLocationPermission(bool param)
        {
            if (!param)
            {
                var status = await PermissionHelper.CheckLocationPermissions();
                if (status)
                {
                    GlobalResourcesVMS.Current.PermissionManager.IsLocationGranted = true;
                }
                else return false;
            }
            return true;
        }

        private async Task<bool> GrantStoragePermission(bool param)
        {
            if (!param)
            {
                var status = await PermissionHelper.CheckStoragePermissions();
                if (status)
                {
                    GlobalResourcesVMS.Current.PermissionManager.IsStorageGranted = true;
                }
                else return false;
            }
            return true;
        }

        private async Task<bool> GrantPhotoPermission(bool param)
        {
            if (!param)
            {
                var status = await PermissionHelper.CheckPhotoPermissions();
                if (status)
                {
                    GlobalResourcesVMS.Current.PermissionManager.IsPhotoGranted = true;
                }
                else return false;
            }
            return true;
        }

        public ICommand GrantAllCommand { get; }
        public ICommand GrantPermissionCommand { get; }

        public ICommand CloseCommand { get; }

        private async void GrantAll()
        {
            var temp = GlobalResourcesVMS.Current.PermissionManager;
            var cam = await GrantCameraPermission(temp.IsCameraGranted);
            var location = await GrantLocationPermission(temp.IsLocationGranted);
            var photo = await GrantPhotoPermission(temp.IsPhotoGranted);
            var storage = await GrantStoragePermission(temp.IsStorageGranted);
            if (cam && location && photo && storage)
            {
                Close();
            }
        }

        private void Close()
        {
            PopupNavigation.Instance.PopAsync();
            if (!Settings.IsLoadedMap)
            {
                Settings.IsLoadedMap = true;
                var eventCenter = PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
                eventCenter.GetEvent<StartShinnyEvent>().Publish();
                Settings.IsLoadedMap = true;
            }
        }
    }
}