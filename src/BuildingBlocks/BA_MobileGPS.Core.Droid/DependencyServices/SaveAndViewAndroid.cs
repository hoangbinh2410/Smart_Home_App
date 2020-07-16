using Android.Content;
using Android.OS;
using Android.Support.V4.Content;
using BA_MobileGPS.Core.Droid.DependencyServices;
using Java.IO;

using Plugin.CurrentActivity;

using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveAndViewAndroid))]

namespace BA_MobileGPS.Core.Droid.DependencyServices
{
    public class SaveAndViewAndroid : ISaveAndView
    {
        //Method to save document as a file in Android and view the saved document
        public void SaveAndView(string fileName, String contentType, MemoryStream stream)
        {
            string exception = string.Empty;
            string root = null;
            if (Android.OS.Environment.IsExternalStorageEmulated)
            {
                root = Android.OS.Environment.ExternalStorageDirectory.ToString();
            }
            else
            {
                root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            }

            Java.IO.File myDir = new Java.IO.File(root + "/GPS");
            myDir.Mkdir();

            Java.IO.File file = new Java.IO.File(myDir, fileName);

            if (file.Exists())
            {
                file.Delete();
            }

            try
            {
                FileOutputStream outs = new FileOutputStream(file);
                outs.Write(stream.ToArray());

                outs.Flush();
                outs.Close();
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

            if (file.Exists() && contentType != "application/html")
            {
                Android.Net.Uri path;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                {
                    path = Xamarin.Essentials.FileProvider.GetUriForFile(Android.App.Application.Context, AppInfo.PackageName + ".fileprovider", file);
                }
                else
                {
                    path = Android.Net.Uri.FromFile(file);
                }

                string extension = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(path.ToString());
                string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);
                Intent intent = new Intent(Intent.ActionView);
                intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                intent.SetDataAndType(path, mimeType);
                intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                Forms.Context.StartActivity(Intent.CreateChooser(intent, "Choose App"));
            }
        }
    }
}