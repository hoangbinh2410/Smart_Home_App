using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BA_MobileGPS.Core.Interfaces
{
    public interface IDownloadVideoService
    {
        Task DownloadFileAsync(string url, IProgress<double> progress, CancellationToken token);
    }
}
