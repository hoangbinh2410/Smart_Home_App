using BA_MobileGPS.Core.ViewModels.Permissions;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views.Permissions
{
    public partial class StoragePermission : PopupPage
    {
        public StoragePermission(Action callback = null)
        {

            BindingContext = new PermissionDialogViewModel(callback);
            InitializeComponent();
        }
    }
}
