using BA_MobileGPS.Core.ViewModels.Permissions;
using Rg.Plugins.Popup.Pages;
using System;

namespace BA_MobileGPS.Core.Views.Permissions
{
    public partial class PhotoPermission : PopupPage
    {
        public PhotoPermission(Action action)
        {
            BindingContext = new PermissionDialogViewModel(action);
            InitializeComponent();
        }
    }
}