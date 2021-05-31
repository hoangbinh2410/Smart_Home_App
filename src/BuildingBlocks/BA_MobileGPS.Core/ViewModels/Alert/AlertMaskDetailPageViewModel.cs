using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.GoogleMap.Behaviors;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<Pin> Pins { get; set; } = new ObservableCollection<Pin>();
        private Pin selectedPin;
        public Pin SelectedPin { get => selectedPin; set => SetProperty(ref selectedPin, value); }

        public AnimateCameraRequest AnimateCameraRequest { get; } = new AnimateCameraRequest();

        private readonly IDownloader downloader;
        private readonly IAlertService alertService;

        public AlertMaskDetailPageViewModel(INavigationService navigationService, IAlertService alertService, IDownloader downloader) : base(navigationService)
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

                    Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
                    {
                        Pins.Clear();
                        Pins.Add(new Pin()
                        {
                            Type = PinType.Place,
                            Label = result.VehiclePlate,
                            Anchor = new Point(.5, .5),
                            Address = result.CurrentAddress,
                            Position = new Position(result.Latitude, result.Longitude),
                            Icon = BitmapDescriptorFactory.FromResource("car_blue.png"),
                            IsDraggable = false,
                            Tag = "CAMERA" + result.VehiclePlate
                        });
                        _ = AnimateCameraRequest.AnimateCamera(CameraUpdateFactory.NewPositionZoom(new Position(result.Latitude, result.Longitude), 14), TimeSpan.FromMilliseconds(10));
                        SelectedPin = Pins[0];
                        return false;
                    });
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
    }
}