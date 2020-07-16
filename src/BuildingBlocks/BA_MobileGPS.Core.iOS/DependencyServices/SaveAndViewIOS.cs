using QuickLook;

using System;
using System.IO;

using UIKit;

using Xamarin.Forms;

[assembly: Dependency(typeof(SaveAndViewIOS))]

internal class SaveAndViewIOS : ISaveAndView
{
    //Method to save document as a file in iOS and view the saved document.
    public void SaveAndView(string filename, string contentType, MemoryStream stream)
    {
        string exception = string.Empty;
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        string filePath = Path.Combine(path, filename);
        try
        {
            FileStream fileStream = File.Open(filePath, FileMode.Create);
            stream.Position = 0;
            stream.CopyTo(fileStream);
            fileStream.Flush();
            fileStream.Close();
        }
        catch (Exception e)
        {
            exception = e.ToString();
        }
        finally
        {
            if (contentType != "application/html")
            {
                stream.Dispose();
            }
        }

        if (contentType == "application/html" || exception != string.Empty)
        {
            return;
        }

        UIViewController currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;
        while (currentController.PresentedViewController != null)
        {
            currentController = currentController.PresentedViewController;
        }

        UIView currentView = currentController.View;

        QLPreviewController preview = new QLPreviewController();
        QLPreviewItem item = new QLPreviewItemBundle(filename, filePath);
        preview.DataSource = new PreviewControllerDS(item);

        // UIViewController uiView = currentView as UIViewController;
        currentController.PresentViewController((UIViewController)preview, true, (Action)null);
    }
}