using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.GoogleMap.Behaviors;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using FFImageLoading;
using Prism.Navigation;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class AlertMaskDetailPageViewModel : ViewModelBase
    {
        private AlertMaskModel alertMaskModel;
        public AlertMaskModel AlertMaskModel { get => alertMaskModel; set => SetProperty(ref alertMaskModel, value); }

        public ICommand DownloadImageCommand { get; }

        public ICommand TabImageCommand { get; private set; }

        public AnimateCameraRequest AnimateCameraRequest { get; } = new AnimateCameraRequest();

        private ImageSource _sourceImage;

        public ImageSource SourceImage
        {
            get { return _sourceImage; }
            set { SetProperty(ref _sourceImage, value); }
        }

        private readonly IDownloader downloader;
        private readonly IAlertService alertService;

        public AlertMaskDetailPageViewModel(INavigationService navigationService,
            IAlertService alertService,
            IDownloader downloader) : base(navigationService)
        {
            this.alertService = alertService;
            this.downloader = downloader;
            downloader.OnFileDownloaded += Downloader_OnFileDownloaded;
            DownloadImageCommand = new Command(DownloadImage);
            TabImageCommand = new Command(TabImage);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            //GetAlertMaskDetail(new Guid());
            if (parameters.TryGetValue(ParameterKey.AlertMask, out Guid id))
            {
                GetAlertMaskDetail(id);
            }
        }

        private void GetAlertMaskDetail(Guid id)
        {
            RunOnBackground(async () =>
            {
                return await alertService.GetAlertMaskDetail(id);
            }, (result) =>
            {
                if (result != null && !string.IsNullOrEmpty(result.Url))
                {
                    AlertMaskModel = result;
                    DrawLine(result);
                }
            }, showLoading: true);
        }

        /// <summary>Xem chi tiết ảnh</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/6/2020   created
        /// </Modified>
        private void TabImage()
        {
            var lstphotoImages = new List<Photo>()
            {
                new Photo()
                {
                    URL= AlertMaskModel.Url,
                    Title=AlertMaskModel.CurrentAddress,
                    Info= ""
                }
            };
            if (Device.RuntimePlatform == Device.Android)
            {
                new PhotoBrowser
                {
                    Photos = new List<Photo>(lstphotoImages),
                    ActionButtonPressed = (index) =>
                    {
                        PhotoBrowser.Close();
                    },
                    StartIndex = 0,
                    EnableGrid = true
                }.Show();
            }
            else
            {
                new PhotoBrowser
                {
                    Photos = new List<Photo>(lstphotoImages),
                    ActionButtonPressed = null,
                    StartIndex = 0,
                    EnableGrid = true
                }.Show();
            }
        }

        /// <summary>
        ///   <para>
        ///  Kiểm tra quyền và gọi hàm download
        /// </para>
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/4/2020   created
        /// </Modified>
        private void DownloadImage()
        {
            SafeExecute(async () =>
            {
                if (await PermissionHelper.CheckStoragePermissions())
                {
                    downloader.DownloadFile(AlertMaskModel.Url, "GPS-Camera");
                }
            });
        }

        /// <summary>Kiểm tra xem có download thành công không</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DownloadEventArgs"/> instance containing the event data.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/4/2020   created
        /// </Modified>
        private void Downloader_OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            SafeExecute(async () =>
            {
                if (e.FileSaved)
                {
                    await Application.Current.MainPage.DisplayAlert("Camera", "Lưu hình ảnh thành công", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Camera", "Lưu hình ảnh không thành công", "OK");
                }
            });
        }

        public override void OnDestroy()
        {
            downloader.OnFileDownloaded -= Downloader_OnFileDownloaded;
        }

        private async void DrawLine(AlertMaskModel alert)
        {
            var stream = await ImageService.Instance.LoadUrl(alert.Url).AsJPGStreamAsync();
            var bitmap = SKBitmap.Decode(stream);
            var canvas = new SKCanvas(bitmap);

            if (alert.UseMask && alert.ListMask != null && alert.ListMask.Count > 0 && alert.ListMask.Count % 4 == 0)
            {
                for (int i = 0; i < alert.ListMask.Count; i = i + 4)
                {
                    var rect = SKRect.Create(alert.ListMask[i], alert.ListMask[i + 1], alert.ListMask[i + 2], alert.ListMask[i + 3]);
                    // the brush (fill with blue)
                    var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = 2,
                        Color = SKColor.Parse("#4BF70C")
                    };
                    // draw stroke
                    canvas.DrawRect(rect, paint);
                }
            }
            if (alert.UseMask && alert.ListNoMask != null && alert.ListNoMask.Count > 0 && alert.ListNoMask.Count % 4 == 0)
            {
                for (int i = 0; i < alert.ListNoMask.Count; i = i + 4)
                {
                    var rect = SKRect.Create(alert.ListNoMask[i], alert.ListNoMask[i + 1], alert.ListNoMask[i + 2], alert.ListNoMask[i + 3]);
                    // the brush (fill with blue)
                    var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = 2,
                        Color = SKColor.Parse("#ff0000")
                    };
                    // draw stroke
                    canvas.DrawRect(rect, paint);
                }
            }

            if (alert.UseDistance && alert.DistanceViolation != null && alert.DistanceViolation.Count > 0 && alert.DistanceViolation.Count % 4 == 0)
            {
                for (int i = 0; i < alert.DistanceViolation.Count; i = i + 4)
                {
                    var rect = SKRect.Create(alert.DistanceViolation[i], alert.DistanceViolation[i + 1], alert.DistanceViolation[i + 2], alert.DistanceViolation[i + 3]);
                    // the brush (fill with blue)
                    var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = 2,
                        Color = SKColor.Parse("#9a12b3")
                    };
                    // draw stroke
                    canvas.DrawRect(rect, paint);
                }
            }

            var imageSK = SKImage.FromBitmap(bitmap);
            SKData encoded = imageSK.Encode();
            // get a stream over the encoded data
            var streamend = encoded.AsStream();
            SourceImage = ImageSource.FromStream(() => streamend);

            imageSK.Dispose();
            bitmap.Dispose();
        }
    }
}