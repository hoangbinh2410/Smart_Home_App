using System;

namespace BA_MobileGPS.Core
{
    public interface IDownloader
    {
        void DownloadFile(string url, string folder);

        string DownloadFileGetPath(string url, string folder);

        event EventHandler<DownloadEventArgs> OnFileDownloaded;
    }

    public class DownloadEventArgs : EventArgs
    {
        public bool FileSaved = false;

        public DownloadEventArgs(bool fileSaved)
        {
            FileSaved = fileSaved;
        }
    }
}