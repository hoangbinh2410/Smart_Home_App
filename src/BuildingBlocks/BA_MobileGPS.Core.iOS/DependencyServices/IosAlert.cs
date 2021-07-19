using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.iOS.DependencyServices;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(IosAlert))]

namespace BA_MobileGPS.Core.iOS.DependencyServices
{
    public class IosAlert : IAlert
    {
        public Task<string> Display(string title, string message, string firstButton, string secondButton, string cancel)
        {
            var taskCompletionSource = new TaskCompletionSource<string>();
            var alerController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

            alerController.AddAction(UIAlertAction.Create(firstButton, UIAlertActionStyle.Default, alert =>
            {
                taskCompletionSource.SetResult(firstButton);
            }));

            alerController.AddAction(UIAlertAction.Create(secondButton, UIAlertActionStyle.Default, alert =>
            {
                taskCompletionSource.SetResult(secondButton);
            }));

            alerController.AddAction(UIAlertAction.Create(cancel, UIAlertActionStyle.Cancel, alert =>
            {
                taskCompletionSource.SetResult(cancel);
            }));

            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }

            vc.PresentViewController(alerController, true, null);
            return taskCompletionSource.Task;
        }
    }
}