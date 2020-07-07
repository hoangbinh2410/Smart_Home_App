//using Acr.UserDialogs;

using System;
using System.Threading.Tasks;

namespace BA_MobileGPS.Core
{
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

            //UserDialogs.Instance.ShowLoading(message, MaskType.Black);
        }

        public void Dispose()
        {
            _cancel = true;

           // UserDialogs.Instance.HideLoading();
        }
    }
}