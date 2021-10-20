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
            //var model = new AlertMaskModel()
            //{
            //    WarningType = 74,
            //    VehicleId = 468259,
            //    VehiclePlate = "22B00800_C",
            //    TimeStart = DateTime.Now,
            //    Latitude = 21.117395401000977,
            //    Longitude = 105.78012084960938,
            //    WarningContent = "",
            //    Url = "https://image35.binhanh.vn/2021/09/20/Image5385/869336030172603_CAM1_20092021_175926_A6YQJK.jpg",
            //    CountUserNotMask = 1,
            //    CountUserUseMask = 4,
            //    ListMask = new List<int> { 521, 94, 17, 31, 488, 81, 20, 31, 347, 50, 23, 30, 313, 396, 43, 42 },
            //    ListNoMask = new List<int> { 266, 49, 24, 30 },
            //    PersonCount = 20,
            //    DistanceViolationCount = 2,
            //    Seat = 16
            //};
            //AlertMaskModel = model;
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    GetAddress(model.Latitude.ToString(), model.Longitude.ToString());
            //    DrawLine(model.Url, model.ListMask, model.ListNoMask);
            //});

            RunOnBackground(async () =>
            {
                return await alertService.GetAlertMaskDetail(id);
            }, (result) =>
            {
                if (result != null && !string.IsNullOrEmpty(result.Url))
                {
                    AlertMaskModel = result;
                    DrawLine(result.Url, result.ListMask, result.ListNoMask);
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

        private async void DrawLine(string url, List<int> listMask, List<int> listNoMask)
        {
            var stream = await ImageService.Instance.LoadUrl(url).AsJPGStreamAsync();
            var bitmap = SKBitmap.Decode(stream);
            var canvas = new SKCanvas(bitmap);
            if (listMask != null && listMask.Count > 0 && listMask.Count % 4 == 0)
            {
                for (int i = 0; i < listMask.Count; i = i + 4)
                {
                    var rect = SKRect.Create(listMask[i], listMask[i + 1], listMask[i + 2], listMask[i + 3]);
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
            if (listNoMask != null && listNoMask.Count > 0 && listNoMask.Count % 4 == 0)
            {
                for (int i = 0; i < listNoMask.Count; i = i + 4)
                {
                    var rect = SKRect.Create(listNoMask[i], listNoMask[i + 1], listNoMask[i + 2], listNoMask[i + 3]);
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