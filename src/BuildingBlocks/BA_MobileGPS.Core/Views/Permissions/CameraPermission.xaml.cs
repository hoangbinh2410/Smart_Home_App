using BA_MobileGPS.Core.ViewModels.Permissions;
using Rg.Plugins.Popup.Pages;
using System;

namespace BA_MobileGPS.Core.Views.Permissions
{
    public partial class CameraPermission : PopupPage
    {
        public CameraPermission(Action action = null)
        {
            BindingContext = new PermissionDialogViewModel(action);

            InitializeComponent();
        }
    }
}