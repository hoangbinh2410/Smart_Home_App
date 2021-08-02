using BA_MobileGPS.Core.Resources;
using Prism.Navigation;

using System;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class UpdateVersionViewModel : ViewModelBase
    {
        public UpdateVersionViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public string UpdateVersionTitle
        {
            get
            {
                return MobileResource.Login_Message_UpdateVersionNew;
            }
        }

        public string UpdateVersionMessage
        {
            get
            {
                return MobileResource.Login_Message_UpdateVersionMessage;
            }
        }

        public string UpdateVersionAccept
        {
            get
            {
                return MobileResource.Common_Button_Update;
            }
        }

        public Command UpdateNewVersionCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!string.IsNullOrEmpty(Settings.AppLinkDownload))
                    {
                        // Mở link download
                        await Launcher.OpenAsync(new Uri(Settings.AppLinkDownload));
                    }

                    await NavigationService.GoBackAsync();
                });
            }
        }
    }
}