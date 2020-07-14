using BA_MobileGPS.Utilities;
using Prism.Commands;

using Rg.Plugins.Popup.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels.Permissions
{
    public class PermissionDialogViewModel
    {
        private Action action { get; set; }

        public PermissionDialogViewModel(Action action = null)
        {
            GrantCommand = new Command<PermissionOfApp>(Grant);
            SkipCommand = new DelegateCommand(Skip);
            this.action = action;
        }

        public ICommand GrantCommand { get; }
        public ICommand SkipCommand { get; }

        private async void Grant(PermissionOfApp param)
        {
            switch (param)
            {
                case PermissionOfApp.Camera:
                    if (await PermissionHelper.CheckCameraPermissions())
                    {
                        await PopupNavigation.Instance.PopAsync();
                        action?.Invoke();
                    }
                    break;

                case PermissionOfApp.Location:
                    if (await PermissionHelper.CheckLocationPermissions())
                    {
                        await PopupNavigation.Instance.PopAsync();
                        action?.Invoke();
                    }
                    break;

                case PermissionOfApp.Photo:
                    if (await PermissionHelper.CheckPhotoPermissions())
                    {
                        await PopupNavigation.Instance.PopAsync();
                        action?.Invoke();
                    }
                    break;

                case PermissionOfApp.Storage:
                    if (await PermissionHelper.CheckStoragePermissions())
                    {
                        await PopupNavigation.Instance.PopAsync();
                        action?.Invoke();
                    }
                    break;

                default:
                    break;
            }
        }

        private void Skip()
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}