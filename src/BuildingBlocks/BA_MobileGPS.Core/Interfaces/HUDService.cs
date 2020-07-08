using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public interface IHUDProvider
    {
        void DisplayProgress(string message);

        void Dismiss();
    }

    public class HUDService : IDisposable
    {
        private bool _cancel;

        public HUDService(string message = "")
        {
            StartHUD(message);
        }

        private async void StartHUD(string message)
        {
            await Task.Delay(100);

            if (_cancel)
            {
                _cancel = false;
                return;
            }

            _cancel = false;
            DependencyService.Get<IHUDProvider>().DisplayProgress(message);
        }

        public void Dispose()
        {
            _cancel = true;

            DependencyService.Get<IHUDProvider>().Dismiss();
        }
    }
}