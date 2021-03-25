using Android.Content;
using Android.OS;
using BA_MobileGPS.Core.Droid.DependencyServices;
using BA_MobileGPS.Core.Interfaces;
using Plugin.CurrentActivity;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Android.Provider.MediaStore;

[assembly: Dependency(typeof(DownloadVideoService))]

namespace BA_MobileGPS.Core.Droid.DependencyServices
{
    public class DownloadVideoService : IDownloadVideoService
    {
        private HttpClient _client = new HttpClient(new Xamarin.Android.Net.AndroidClientHandler());

        private int bufferSize = 4095;

        public async Task DownloadFileAsync(string url, IProgress<double> progress, CancellationToken token)
        {
            var name = DateTime.Now.ToString("yyyyMMddHHmmss");
            var IMAGES_FOLDER_NAME = "Cam_Download";
            ContentResolver resolver = CrossCurrentActivity.Current.Activity.ContentResolver;
            try
            {
                // Step 1 : Get call
                var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));
                }

                // Step 2 : Get total of data
                var totalData = response.Content.Headers.ContentLength.GetValueOrDefault(-1L);
                var canSendProgress = totalData != -1L && progress != null;

                Stream fos;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                {
                    // Step 3 : Create file in DCIM
                    ContentValues contentValues = new ContentValues();
                    contentValues.Put(IMediaColumns.DisplayName, name);
                    contentValues.Put(IMediaColumns.MimeType, "video/mp4");
                    contentValues.Put(IMediaColumns.RelativePath, "DCIM/" + IMAGES_FOLDER_NAME);
                    Android.Net.Uri imageUri = resolver.Insert(Video.Media.ExternalContentUri, contentValues);
                    fos = resolver.OpenOutputStream(imageUri);
                }
                else
                {
                    var imagesDir = Android.OS.Environment.GetExternalStoragePublicDirectory(
                   Android.OS.Environment.DirectoryDownloads).ToString() + "/" + IMAGES_FOLDER_NAME;
                    var fileP = imagesDir + "/" + name + ".mp4";
                    if (!Directory.Exists(imagesDir))
                        Directory.CreateDirectory(imagesDir);
                    fos = new FileStream(fileP, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize);
                }

                // Step 4 : Download data
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    var totalRead = 0L;
                    var buffer = new byte[bufferSize];
                    var isMoreDataToRead = true;

                    do
                    {
                        token.ThrowIfCancellationRequested();

                        var read = await stream.ReadAsync(buffer, 0, buffer.Length, token);

                        if (read == 0)
                        {
                            isMoreDataToRead = false;
                        }
                        else
                        {
                            // Write data on disk.
                            await fos.WriteAsync(buffer, 0, read);

                            totalRead += read;

                            if (canSendProgress)
                            {
                                progress.Report((totalRead * 1d) / (totalData * 1d) * 100);
                            }
                        }
                    } while (isMoreDataToRead);
                }
                fos?.Flush();
                fos?.Close();
            }
            catch (Exception e)
            {
                // Manage the exception as you need here.
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
    }
}