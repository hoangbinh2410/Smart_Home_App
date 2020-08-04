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
                return "Đã có bản cập nhật mới";
            }
        }

        public string UpdateVersionMessage
        {
            get
            {
                return "Cập nhật lên phiên bản mới nhất sẽ có nhiều cải tiến và nâng cấp bảo mật giúp bạn có trải nghiệm tốt hơn, an toàn và tiện dụng hơn.";
            }
        }

        public string UpdateVersionAccept
        {
            get
            {
                return "Cập nhật";
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