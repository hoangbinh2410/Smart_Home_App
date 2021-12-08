using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.GoogleMap.Behaviors;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Prism.Navigation;
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

        private List<CaptureImageData> listCameraImage;
        public List<CaptureImageData> ListCameraImage { get => listCameraImage; set => SetProperty(ref listCameraImage, value); }

        private int position;
        public int Position { get => position; set => SetProperty(ref position, value, onChanged: OnPositionChanged); }

        private CaptureImageData imageCamera;
        public CaptureImageData ImageCamera { get => imageCamera; set => SetProperty(ref imageCamera, value); }

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
            if (parameters.TryGetValue(ParameterKey.ListImageCamera, out List<CaptureImageData> listCameraImage)
               && parameters.TryGetValue(ParameterKey.ImageCamera, out CaptureImageData imageCamera) && parameters.TryGetValue(ParameterKey.VehiclePlate, out string vehiclePlate))
            {
                SafeExecute(() =>
                {
                    if (listCameraImage ==null || listCameraImage.Count==0)
                    {
                        return;
                    }
                    VehiclePlate = vehiclePlate;
                    ListCameraImage = listCameraImage;
                    ImageCamera = imageCamera;

                    Position = ListCameraImage.IndexOf(ImageCamera);
                    PositionString = string.Format("{0}/{1}", Position + 1, ListCameraImage.Count);
                    if (imageCamera.Lat!=0 && imageCamera.Lng!=0)
                    {
                        Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
                        {
                            Pins.Clear();
                            Pins.Add(new Pin()
                            {
                                Type = PinType.Place,
                                Label = VehiclePlate,
                                Anchor = new Point(.5, .5),
                                Address = imageCamera.CurrentAddress,
                                Position = new Position(imageCamera.Lat, imageCamera.Lng),
                                Icon = BitmapDescriptorFactory.FromResource("car_blue.png"),
                                IsDraggable = false,
                                Tag = "CAMERA" + VehiclePlate
                            });
                            _ = AnimateCameraRequest.AnimateCamera(CameraUpdateFactory.NewPositionZoom(new Position(ImageCamera.Lat, ImageCamera.Lng), 14), TimeSpan.FromMilliseconds(10));
                            SelectedPin = Pins[0];
                            return false;
                        });
                    }

                    //Add vào zoom slide

                    var tempListData = new ObservableCollection<Photo>();
                    foreach (var item in ListCameraImage)
                    {
                        tempListData.Add(new Photo()
                        {
                            URL = item.Url,
                            Info = "Kênh " + item.Channel + " - " + DateTimeHelper.FormatDateTimeWithoutSecond(item.Time),
                            Title = item.CurrentAddress
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
                });
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
                        if (ListCameraImage[Position].Lat !=0 && ListCameraImage[Position].Lng !=0)
                        {
                            Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
                            {
                                var Pin = Pins[0];
                                Pin.Position = new Position(ListCameraImage[Position].Lat, ListCameraImage[Position].Lng);
                                Pin.Address = ListCameraImage[Position].CurrentAddress;
                                _ = AnimateCameraRequest.AnimateCamera(CameraUpdateFactory.NewPositionZoom(new Position(ListCameraImage[Position].Lat, ListCameraImage[Position].Lng), 14), TimeSpan.FromMilliseconds(10));
                                SelectedPin = Pins[0];
                                return false;
                            });
                        }
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
                    downloader.DownloadFile(ImageCamera.Url, "GPS-Camera");
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
                    await Application.Current.MainPage.DisplayAlert(MobileResource.Common_Label_Notification, MobileResource.Camera_Message_SaveImageSuccess, MobileResource.Common_Button_OK);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(MobileResource.Common_Label_Notification, MobileResource.Camera_Message_SaveImageError, MobileResource.Common_Button_OK);
                }
            });
        }

        public override void OnDestroy()
        {
            downloader.OnFileDownloaded -= Downloader_OnFileDownloaded;
        }
    }
}