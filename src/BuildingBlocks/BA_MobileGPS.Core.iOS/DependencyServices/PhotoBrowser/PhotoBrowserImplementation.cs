using BA_MobileGPS.Core.iOS.DependencyService;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhotoBrowserImplementation))]

namespace BA_MobileGPS.Core.iOS.DependencyService
{
    public class PhotoBrowserImplementation : IPhotoBrowser
    {
        protected static MyMWPhotoBrower _mainBrowser;

        public void Show(PhotoBrowser photoBrowser)
        {
            _mainBrowser = new MyMWPhotoBrower(photoBrowser);
            _mainBrowser.Show();
        }

        public void Close()
        {
            if (_mainBrowser != null)
            {
                _mainBrowser.Close();
                _mainBrowser = null;
            }
        }
    }
}