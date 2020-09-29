using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Camera;
using BA_MobileGPS.Utilities;
using Prism.Navigation;
using BA_MobileGPS.Service.IService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Xamarin.Forms;
using Realms.Exceptions;
using Xamarin.Forms.Extensions;
using System.Linq;
using BA_MobileGPS.Core.Resources;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ImageDetailViewModel : ViewModelBase
    {
        private readonly IStreamCameraService _streamCameraService;

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

        private readonly IDownloader downloader;

        private ObservableCollection<Photo> listphotoImages;
        public ObservableCollection<Photo> ListphotoImages { get => listphotoImages; set => SetProperty(ref listphotoImages, value); }

        public ImageDetailViewModel(INavigationService navigationService, IStreamCameraService streamCameraService, IDownloader downloader) : base(navigationService)
        {
            Title = MobileResource.CameraImage_Label_TitleDetailPage;
            _streamCameraService = streamCameraService;
            this.downloader = downloader;
            downloader.OnFileDownloaded += Downloader_OnFileDownloaded;
            DownloadImageCommand = new Command(DownloadImage);
            TabImageCommand = new Command(TabImage);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.GetValue<string>(ParameterKey.VehiclePlate) is string vehiclePlate)
            {
                VehiclePlate = vehiclePlate;
            }

            if (parameters.TryGetValue(ParameterKey.ImageCamera, out CaptureImageData imageCamera))
            {
                ImageCamera = imageCamera;
            }

            ShowDetailImage();
            UpdateLastViewVehicleImage();
        }

        private void UpdateLastViewVehicleImage()
        {
            if(Settings.LastViewVehicleImage == string.Empty) // lần đầu
            {
                Settings.LastViewVehicleImage = VehiclePlate;
            }
            else
            {
                var split = Settings.LastViewVehicleImage.Split(',');

                split = split.Where(x => x != VehiclePlate).ToArray();

                string[] temp = new string[split.Length];

                if (split.Length < Settings.ShowViewVehicleImage) // nếu chưa số lastview
                {
                    temp = new string[split.Length + 1];

                    temp[0] = VehiclePlate;

                    for (int i = 0; i < split.Length; i++)
                    {
                        temp[i + 1] = split[i];
                    }
                }
                else // nếu đủ rồi và là 1 biển số mới
                {
                    temp[0] = VehiclePlate;

                    for (int i = 0; i < split.Length - 1; i++)
                    {
                        temp[i + 1] = split[i];
                    }
                }
               
                Settings.LastViewVehicleImage = string.Join(",", temp);
            }
        }


        private void ShowDetailImage()
        {
            TryExecute(async () =>
            {
                var xnCode = StaticSettings.User.XNCode;

                if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
                {
                    xnCode = Settings.CurrentCompany.XNCode;
                }

                ListCameraImage = new List<CaptureImageData>();
                ListCameraImage = await _streamCameraService.GetCaptureImageLimit(xnCode, VehiclePlate, 50);
                if (ListCameraImage != null && ListCameraImage.Count > 0)
                {
                    if(ImageCamera !=null)
                    {
                        Position = ListCameraImage.IndexOf(ImageCamera);
                    }
                    else
                    {
                        Position = 0;
                    }

                    PositionString = string.Format("{0}/{1}", Position + 1, ListCameraImage.Count);

                    Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
                    {
                        if (GetControl<Map>("Map") is Map map)
                        {
                            map.Pins.Clear();
                            map.Pins.Add(new Pin()
                            {
                                Type = PinType.Place,
                                Label = VehiclePlate,
                                Anchor = new Point(.5, .5),
                                Address = ListCameraImage[Position].CurrentAddress,
                                Position = new Position(ListCameraImage[Position].Lat, ListCameraImage[Position].Lng),
                                Icon = BitmapDescriptorFactory.FromResource("car_blue.png"),
                                IsDraggable = false
                            });
                            map.AnimateCamera(CameraUpdateFactory.NewPositionZoom(new Position(ImageCamera.Lat, ImageCamera.Lng), 14), TimeSpan.FromMilliseconds(10));
                            map.SelectedPin = map.Pins[0];
                        }
                        return false;
                    });

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
                }
            });
        }

        private void OnPositionChanged()
        {
            try
            {
                if (ListCameraImage != null)
                {
                    if (Position >= 0)
                    {
                        ImageCamera = ListCameraImage[Position];

                        PositionString = string.Format("{0}/{1}", Position + 1, ListCameraImage.Count);

                        Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
                        {
                            if (GetControl<Map>("Map") is Map map)
                            {
                                map.Pins.Clear();
                                map.Pins.Add(new Pin()
                                {
                                    Type = PinType.Place,
                                    Label = VehiclePlate,
                                    Anchor = new Point(.5, .5),
                                    Address = ListCameraImage[Position].CurrentAddress,
                                    Position = new Position(ListCameraImage[Position].Lat, ListCameraImage[Position].Lng),
                                    Icon = BitmapDescriptorFactory.FromResource("car_blue.png"),
                                    IsDraggable = false
                                });
                                map.AnimateCamera(CameraUpdateFactory.NewPositionZoom(new Position(ImageCamera.Lat, ImageCamera.Lng), 14), TimeSpan.FromMilliseconds(10));
                                map.SelectedPin = map.Pins[0];
                            }
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
                    await PageDialog.DisplayAlertAsync("Camera", "Lưu hình ảnh thành công", "OK");
                }
                else
                {
                    await PageDialog.DisplayAlertAsync("Camera", "Lưu hình ảnh không thành công", "OK");
                }
            });
        }

        public override void OnDestroy()
        {
            downloader.OnFileDownloaded -= Downloader_OnFileDownloaded;
        }
    }
}