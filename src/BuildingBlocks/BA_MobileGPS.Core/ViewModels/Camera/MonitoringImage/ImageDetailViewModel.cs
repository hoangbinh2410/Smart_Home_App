using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.GoogleMap.Behaviors;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ImageDetailViewModel : ViewModelBase
    {
        private readonly IRealmBaseService<LastViewVehicleRealm, LastViewVehicleRespone> _lastViewVehicleRepository;

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

        private string filePath;
        public string FilePath { get => filePath; set => SetProperty(ref filePath, value); }

        private ObservableCollection<Photo> listphotoImages;
        public ObservableCollection<Photo> ListphotoImages { get => listphotoImages; set => SetProperty(ref listphotoImages, value); }

        public ObservableCollection<Pin> Pins { get; set; } = new ObservableCollection<Pin>();
        private Pin selectedPin;
        public Pin SelectedPin { get => selectedPin; set => SetProperty(ref selectedPin, value); }
        public AnimateCameraRequest AnimateCameraRequest { get; } = new AnimateCameraRequest();

        public ImageDetailViewModel(INavigationService navigationService,
            IStreamCameraService streamCameraService,
            IRealmBaseService<LastViewVehicleRealm, LastViewVehicleRespone> lastViewVehicleRepository,
            IDownloader downloader) : base(navigationService)
        {
            Title = MobileResource.CameraImage_Label_TitleDetailPage;
            _streamCameraService = streamCameraService;
            _lastViewVehicleRepository = lastViewVehicleRepository;
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
            var userId = StaticSettings.User.UserId;

            if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
            {
                userId = Settings.CurrentCompany.UserId;
            }

            var lst = _lastViewVehicleRepository.All()?.Where(x => x.UserId == userId.ToString()).ToList();
            if (lst != null && lst.Count > 0)
            {
                var lstVehicle = lst.FirstOrDefault(x => x.VehiclePlate == VehiclePlate);
                if (lstVehicle == null) // chưa có thì mới add
                {
                    var index = _lastViewVehicleRepository.GetLastId();

                    if (lst.Count == Settings.ShowViewVehicleImage) // nếu bằng số lastview
                    {
                        // xóa phần tử
                        _lastViewVehicleRepository.Delete(lst.FirstOrDefault().Id);
                    }

                    // add
                    _lastViewVehicleRepository.Add(new LastViewVehicleRespone()
                    {
                        Id = index + 1,
                        UserId = userId.ToString(),
                        VehiclePlate = VehiclePlate
                    });
                }
            }
            else
            {
                _lastViewVehicleRepository.Add(new LastViewVehicleRespone()
                {
                    Id = 1,
                    UserId = userId.ToString(),
                    VehiclePlate = VehiclePlate
                });
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

                ListCameraImage = await _streamCameraService.GetCaptureImageLimit(xnCode, VehiclePlate, 50);
                if (ListCameraImage != null && ListCameraImage.Count > 0)
                {
                    if (ImageCamera != null)
                    {
                        Position = ListCameraImage.Select(x => x.Url).ToList().IndexOf(ImageCamera.Url);
                    }
                    else
                    {
                        Position = 0;
                        ImageCamera = ListCameraImage.FirstOrDefault();
                    }

                    PositionString = string.Format("{0}/{1}", Position + 1, ListCameraImage.Count);

                    Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
                    {
                        Pins.Clear();
                        Pins.Add(new Pin()
                        {
                            Type = PinType.Place,
                            Label = VehiclePlate,
                            Anchor = new Point(.5, .5),
                            Address = ListCameraImage[Position].CurrentAddress,
                            Position = new Position(ListCameraImage[Position].Lat, ListCameraImage[Position].Lng),
                            Icon = BitmapDescriptorFactory.FromResource("car_blue.png"),
                            IsDraggable = false,
                            Tag = "CAMERA" + VehiclePlate
                        });
                        _ = AnimateCameraRequest.AnimateCamera(CameraUpdateFactory.NewPositionZoom(new Position(ImageCamera.Lat, ImageCamera.Lng), 14), TimeSpan.FromMilliseconds(10));
                        SelectedPin = Pins[0];
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
                    if (Position >= 0 && Pins != null && Pins.Count > 0)
                    {
                        ImageCamera = ListCameraImage[Position];

                        PositionString = string.Format("{0}/{1}", Position + 1, ListCameraImage.Count);

                        Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
                        {
                            var Pin = Pins[0];
                            Pin.Position = new Position(ListCameraImage[Position].Lat, ListCameraImage[Position].Lng);
                            _ = AnimateCameraRequest.AnimateCamera(CameraUpdateFactory.NewPositionZoom(Pin.Position, 14), TimeSpan.FromMilliseconds(10));
                            SelectedPin = Pins[0];
                            return false;
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>Xem chi tiết ảnh</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/6/2020   created
        /// </Modified>
        private void TabImage()
        {
            try
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
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
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
            TryExecute(async () =>
            {
                var photoPermission = await PermissionHelper.CheckPhotoPermissions();
                var storagePermission = await PermissionHelper.CheckStoragePermissions();
                if (photoPermission && storagePermission)
                {
                    FilePath = downloader.DownloadFileGetPath(ImageCamera.Url, "GPS-Camera");
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
                    // Lưu ảnh vào gallery
                    if (File.Exists(FilePath))
                    {
                        DependencyService.Get<ICameraSnapShotServices>().SaveSnapShotToGalery(FilePath);
                        // xóa ảnh tại path cũ
                        //File.Delete(FilePath);
                    }

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