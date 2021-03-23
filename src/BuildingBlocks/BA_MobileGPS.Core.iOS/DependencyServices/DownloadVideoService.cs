using AssetsLibrary;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.iOS.DependencyServices;
using Foundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(DownloadVideoService))]

namespace BA_MobileGPS.Core.iOS.DependencyServices
{
   public class DownloadVideoService : IDownloadVideoService
    {
        public async Task DownloadFileAsync(string url, IProgress<double> progress, CancellationToken token)
        {
            int bufferSize = 4095;
            var name = DateTime.Now.ToString("yyyyMMddHHmmss");

            var filePath = System.IO.Path.Combine(GetFolderPath(), name + ".mp4");
            if (File.Exists(filePath))
                File.Delete(filePath);

            var _client = new HttpClient();
            try
            {
                // Step 1 : Get call
                var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));
                }

                // Step 3 : Get total of data
                var totalData = response.Content.Headers.ContentLength.GetValueOrDefault(-1L);
                var canSendProgress = totalData != -1L && progress != null;



                // Step 5 : Download data
                using (var fileStream = OpenStream(filePath))
                {
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
                                await fileStream.WriteAsync(buffer, 0, read);

                                totalRead += read;

                                if (canSendProgress)
                                {
                                    progress.Report((totalRead * 1d) / (totalData * 1d) * 100);
                                }
                            }
                        } while (isMoreDataToRead);
                    }
                }

                //Save to Album
                ALAssetsLibrary lib = new ALAssetsLibrary();
                lib.WriteVideoToSavedPhotosAlbum(NSUrl.FromFilename(filePath), (t, u) => { });
            }
            catch (Exception e)
            {
                // Manage the exception as you need here.
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        private string GetFolderPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }
        private Stream OpenStream(string path)
        {
            return new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4095);
        }
    }
}