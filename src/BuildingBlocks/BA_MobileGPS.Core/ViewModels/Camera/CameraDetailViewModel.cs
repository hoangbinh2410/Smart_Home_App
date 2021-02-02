using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.GoogleMap.Behaviors;
using BA_MobileGPS.Entities.ResponeEntity.Camera;
using Prism.Navigation;
using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class CameraDetailViewModel : ViewModelBase
    {
        private string vehiclePlate;
        public string VehiclePlate { get => vehiclePlate; set => SetProperty(ref vehiclePlate, value); }

        private string positionString;
        public string PositionString { get => positionString; set => SetProperty(ref positionString, value); }

        private List<ImageCamera> listCameraImage;
        public List<ImageCamera> ListCameraImage { get => listCameraImage; set => SetProperty(ref listCameraImage, value); }

        private int position;
        public int Position { get => position; set => SetProperty(ref position, value, onChanged: OnPositionChanged); }

        private ImageCamera imageCamera;
        public ImageCamera ImageCamera { get => imageCamera; set => SetProperty(ref imageCamera, value); }

        public ICommand DownloadImageCommand { get; }

        public ICommand TabImageCommand { get; private set; }

        public ObservableCollection<Pin> Pins { get; set; } = new ObservableCollection<Pin>();
        private Pin selectedPin;
        public Pin SelectedPin { get => selectedPin; set => SetProperty(ref selectedPin, value); }

        public AnimateCameraRequest AnimateCameraRequest { get; } = new AnimateCameraRequest();

        private readonly IDownloader downloader;

        private ObservableCollection<Photo> listphotoImages;
        public ObservableCollection<Photo> ListphotoImages { get => listphotoImages; set => SetProperty(ref listphotoImages, value); }

        public CameraDetailViewModel(INavigationService navigationService, IDownloader downloader) : base(navigationService)
        {
            this.downloader = downloader;
            downloader.OnFileDownloaded += Downloader_OnFileDownloaded;
            DownloadImageCommand = new Command(DownloadImage);
            TabImageCommand = new Command(TabImage);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.TryGetValue(ParameterKey.ListImageCamera, out List<ImageCamera> listCameraImage)
               && parameters.TryGetValue(ParameterKey.ImageCamera, out ImageCamera imageCamera) && parameters.TryGetValue(ParameterKey.VehiclePlate, out string vehiclePlate))
            {
                VehiclePlate = vehiclePlate;
                ListCameraImage = listCameraImage;
                ImageCamera = imageCamera;

                Position = ListCameraImage.IndexOf(ImageCamera);
                PositionString = string.Format("{0}/{1}", Position + 1, ListCameraImage.Count);

                Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
                {
                    Pins.Clear();
                    Pins.Add(new Pin()
                    {
                        Type = PinType.Place,
                        Label = VehiclePlate,
                        Anchor = new Point(.5, .5),
                        Address = ListCameraImage[Position].Address,
                        Position = new Position(ListCameraImage[Position].Latitude, ListCameraImage[Position].Longitude),
                        Icon = BitmapDescriptorFactory.FromResource("car_blue.png"),
                        IsDraggable = false,
                        Tag = "CAMERA" + VehiclePlate
                    });
                    _ = AnimateCameraRequest.AnimateCamera(CameraUpdateFactory.NewPositionZoom(new Position(ImageCamera.Latitude, ImageCamera.Longitude), 14), TimeSpan.FromMilliseconds(10));
                    SelectedPin = Pins[0];
                    return false;
                });

                //Add vào zoom slide

                var tempListData = new ObservableCollection<Photo>();
                foreach (var item in ListCameraImage)
                {
                    tempListData.Add(new Photo()
                    {
                        URL = item.ImageLink,
                        //Info = "Kênh " + item.Channel + " - " + DateTimeHelper.FormatDateTimeWithoutSecond(item.CreatedDate),
                        Title = item.Address
                    });
                }

                if (tempListData != null && tempListData.Count > 0)
                {
                    ListphotoImages = new ObservableCollection<Photo>(tempListData);
                }
                else
                {
                    ListphotoImages = new ObservableCollection<Photo>();
                }
            }
        }

        private void OnPositionChanged()
        {
            try
            {
                if (ListCameraImage != null)
                {
                    if (Position >= 0 && Pins != null && Pins.Count > 0)
                    {
                        ImageCamera = ListCameraImage[Position];

                        PositionString = string.Format("{0}/{1}", Position + 1, ListCameraImage.Count);

                        Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
                        {
                            var Pin = Pins[0];
                            Pin.Position = new Position(ListCameraImage[Position].Latitude, ListCameraImage[Position].Longitude);
                            Pin.Address = ListCameraImage[Position].Address;
                            _ = AnimateCameraRequest.AnimateCamera(CameraUpdateFactory.NewPositionZoom(new Position(ListCameraImage[Position].Latitude, ListCameraImage[Position].Longitude), 14), TimeSpan.FromMilliseconds(10));
                            SelectedPin = Pins[0];
                            return false;
                        });
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>Xem chi tiết ảnh</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/6/2020   created
        /// </Modified>
        private void TabImage()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                new PhotoBrowser
                {
                    Photos = new List<Photo>(ListphotoImages),
                    ActionButtonPressed = (index) =>
                    {
                        PhotoBrowser.Close();
                    },
                    StartIndex = Position,
                    EnableGrid = true
                }.Show();
            }
            else
            {
                new PhotoBrowser
                {
                    Photos = new List<Photo>(ListphotoImages),
                    ActionButtonPressed = null,
                    StartIndex = Position,
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
                    downloader.DownloadFile(ImageCamera.ImageLink, "GPS-Camera");
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