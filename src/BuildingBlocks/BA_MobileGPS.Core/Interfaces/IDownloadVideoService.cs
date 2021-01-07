using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Core.Interfaces
{
    public interface IDownloadVideoService
    {
        Task DownloadFileAsync(string url, IProgress<double> progress, CancellationToken token);
    }
}
