using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BA_MobileGPS.Core.ViewModels
{
    public class DownloadVideoViewModel : ViewModelBase
    {
        public DownloadVideoViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Main Page";
            UrlFile = "https://www.learningcontainer.com/wp-content/uploads/2020/05/sample-mp4-file.mp4";
            _downloadService = downloadService;
            StartDownloadCommand = new DelegateCommand(StartDownloadAsync);
        }

        private double _progressValue;
        public double ProgressValue
        {
            get { return _progressValue; }
            set { SetProperty(ref _progressValue, value); }
        }

        private bool _isDownloading;
        public bool IsDownloading
        {
            get { return _isDownloading; }
            set { SetProperty(ref _isDownloading, value); }
        }
        private string urlFile;
        public string UrlFile
        {
            get { return urlFile; }
            set { SetProperty(ref urlFile, value); }
        }

        private readonly IDownloader _downloadService;

        public ICommand StartDownloadCommand { get; }

        public async void StartDownloadAsync()
        {
            var progressIndicator = new Progress<double>(ReportProgress);
            var cts = new CancellationTokenSource();
            try
            {
                IsDownloading = true;

                var url = urlFile;
                var permissionStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                if (permissionStatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    await _downloadService.DownloadFileAsync(url, progressIndicator, cts.Token);
                }
            }
            catch (OperationCanceledException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                //Manage cancellation here
            }
            finally
            {
                IsDownloading = false;
            }
        }

        internal void ReportProgress(double value)
        {
            ProgressValue = value;
        }
    }
}
