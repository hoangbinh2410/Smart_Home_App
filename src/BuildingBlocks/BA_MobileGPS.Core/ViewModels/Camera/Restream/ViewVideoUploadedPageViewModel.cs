using BA_MobileGPS.Core.GoogleMap.Behaviors;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using LibVLCSharp.Shared;
using Plugin.Permissions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ViewVideoUploadedPageViewModel : RestreamChildVMBase
    {
        public ICommand ScreenShotTappedCommand { get; }

        /// <summary>
        /// Chụp ảnh màn hình
        /// </summary>
        public ICommand DowloadVideoCommand { get; }

        public ICommand DowloadVideoInListTappedCommand { get; }

        public ICommand PreviousVideoCommand { get; }

        public ICommand NextVideoCommand { get; }

        private readonly IDownloadVideoService _downloadService;
        private readonly IVehicleRouteService _vehicleRouteService;

        public ViewVideoUploadedPageViewModel(INavigationService navigationService,
          IStreamCameraService cameraService, IVehicleRouteService vehicleRouteService,
          IScreenOrientServices screenOrientServices, IDownloadVideoService downloadService) : base(navigationService, cameraService, screenOrientServices)
        {
            _downloadService = downloadService;
            _vehicleRouteService = vehicleRouteService;
            ScreenShotTappedCommand = new DelegateCommand(TakeSnapShot);
            DowloadVideoCommand = new DelegateCommand(DowloadVideo);
            DowloadVideoInListTappedCommand = new DelegateCommand<VideoUploadInfo>(DowloadVideoInListTapped);
            PreviousVideoCommand = new DelegateCommand(PreviousVideo);
            NextVideoCommand = new DelegateCommand(NextVideo);
        }

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
            LibVLCSharp.Shared.Core.Initialize();
            LibVLC = new LibVLC();
            InitVideoVideo(new VideoUploadInfo()
            {
                Channel = 1,
                FileName = "29H123456_CH1_000000323.mp4",
                Link = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4",
                StartTime = DateTime.Now,
                Status = VideoUploadStatus.Uploaded
            });
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            DisposeVLC();
        }

        public override void OnSleep()
        {
            if (MediaPlayer != null)
            {
                MediaPlayer.Pause();
            }
            base.OnSleep();
        }

        public override void OnResume()
        {
            base.OnResume();
            DependencyService.Get<IScreenOrientServices>().ForcePortrait();
        }

        #endregion Lifecycle

        #region Property
        private RouteHistoryResponse RouteHistory;
        public ObservableCollection<Pin> Pins { get; set; } = new ObservableCollection<Pin>();

        public ObservableCollection<Polyline> Polylines { get; set; } = new ObservableCollection<Polyline>();
        public MoveCameraRequest MoveCameraRequest { get; } = new MoveCameraRequest();

        public AnimateCameraRequest AnimateCameraRequest { get; } = new AnimateCameraRequest();

        public string BusyIndicatorText { get; set; }

        private LibVLC libVLC;

        public LibVLC LibVLC
        {
            get { return libVLC; }
            set { SetProperty(ref libVLC, value); }
        }

        private MediaPlayer mediaPlayer;

        public MediaPlayer MediaPlayer
        {
            get => mediaPlayer; set => SetProperty(ref mediaPlayer, value);
        }

        private VideoUploadInfo videoSlected;

        /// <summary>
        /// Ảnh được focus
        /// </summary>
        public VideoUploadInfo VideoSlected
        {
            get => videoSlected;
            set
            {
                SetProperty(ref videoSlected, value);
            }
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

        private bool _autoSwitch = true;

        public bool AutoSwitch
        {
            get { return _autoSwitch; }
            set { SetProperty(ref _autoSwitch, value); }
        }

        private double _seekBarValue;

        public double SeekBarValue
        {
            get { return _seekBarValue; }
            set { SetProperty(ref _seekBarValue, value); }
        }

        #endregion Property

        #region PrivateMethod

        private void GetHistoryRoute()
        {
            var currentCompany = Settings.CurrentCompany;
            Xamarin.Forms.DependencyService.Get<IHUDProvider>().DisplayProgress("Đang tải dữ liệu...");
            RunOnBackground(async () =>
            {
                return await _vehicleRouteService.GetHistoryRoute(new RouteHistoryRequest
                {
                    UserId = currentCompany?.UserId ?? UserInfo.UserId,
                    CompanyId = currentCompany?.FK_CompanyID ?? CurrentComanyID,
                    VehiclePlate = "",
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now,
                });
            }, (result) =>
            {
                if (result != null)
                {
                    try
                    {
                        ClearRoute();

                        RouteHistory = result;

                        InitRoute();
                    }
                    catch (Exception)
                    {
                        PageDialog.DisplayAlertAsync("", "Không tải được dữ liệu", MobileResource.Common_Button_OK);
                    }
                }
                else
                {
                    PageDialog.DisplayAlertAsync("", "Không tải được dữ liệu", MobileResource.Common_Button_OK);
                }
                Xamarin.Forms.DependencyService.Get<IHUDProvider>().Dismiss();
            });
        }


        private void InitRoute()
        {
            try
            {
                if (RouteHistory.DirectionDetail == null)
                    return;

                var listLatLng = GeoHelper.DecodePoly(RouteHistory.DirectionDetail);

                if (listLatLng == null || listLatLng.Count == 0)
                {
                    return;
                }

                if (listLatLng.Count == 1)
                {
                    listLatLng.Add(new Position(listLatLng[0].Latitude, listLatLng[0].Longitude));
                    RouteHistory.TimePoints.AddedTimes.Add(RouteHistory.TimePoints.AddedTimes[0]);
                }

                var startTime = RouteHistory.TimePoints.StartTime;
                List<VehicleRoute> listRoute = new List<VehicleRoute>();
                void AddRoute(int index)
                {
                    var route = new VehicleRoute()
                    {
                        Index = index,
                        Latitude = listLatLng[index].Latitude,
                        Longitude = listLatLng[index].Longitude,
                        State = RouteHistory.StatePoints.FirstOrDefault(stp => stp.StartIndex <= index && index <= stp.EndIndex),
                        Velocity = RouteHistory.VelocityPoints[index],
                        Time = startTime
                    };

                    listRoute.Add(route);
                }

                AddRoute(0);

                startTime = startTime.AddSeconds(RouteHistory.TimePoints.AddedTimes[0]);

                for (int i = 1; i < listLatLng.Count; i++)
                {
                    startTime = startTime.AddSeconds(RouteHistory.TimePoints.AddedTimes[i]);

                    if (listLatLng[i - 1].Latitude == listLatLng[i].Latitude
                        && listLatLng[i - 1].Longitude == listLatLng[i].Longitude
                        && !RouteHistory.StatePoints.Any(stp => stp.StartIndex == i && i == stp.EndIndex))
                        continue;

                    AddRoute(i);
                }
                if (listRoute.Count == 1)
                {
                    listRoute.Add(listRoute[0].DeepCopy());
                }

                if (listRoute !=null && listRoute.Count > 0)
                {
                    DrawRoute(listRoute);
                }
                
            }
            catch (Exception ex)
            {
                PageDialog.DisplayAlertAsync("Init Route Error", ex.Message, MobileResource.Common_Button_OK);
            }
        }

        private void DrawLine(List<VehicleRoute> listRoute)
        {
            var line = new Polyline
            {
                IsClickable = false,
                StrokeColor = Color.FromHex("#4285f4"),
                StrokeWidth = 3f,
                ZIndex = 1
            };
            line.Positions.Add(new Position(listRoute[0].Latitude, listRoute[0].Longitude));

            for (int i = 0; i < listRoute.Count; i++)
            {
                line.Positions.Add(new Position(listRoute[i].Latitude, listRoute[i].Longitude));
                if (listRoute[i].State != null && listRoute[i].State.State == StateType.Stop)
                {
                    DrawStopPoint(listRoute[i]);
                }
            }
            Polylines.Add(line);
        }

        private void DrawMarkerStartEnd(List<VehicleRoute> listRoute)
        {
            Pins.Add(new Pin()
            {
                Type = PinType.Place,
                Label = "Bắt đầu",
                Position = new Position(listRoute[0].Latitude, listRoute[0].Longitude),
                Icon = BitmapDescriptorFactory.FromResource("ic_start.png"),
                ZIndex = 0,
                Tag = "pin_start_route",
                IsDraggable = false
            });

            Pins.Add(new Pin()
            {
                Type = PinType.Place,
                Label = "Kết thúc",
                Position = new Position(listRoute[listRoute.Count - 1].Latitude, listRoute[listRoute.Count - 1].Longitude),
                Icon = BitmapDescriptorFactory.FromResource("ic_end.png"),
                ZIndex = 0,
                Tag = "pin_end_route",
                IsDraggable = false
            });
        }

        private void DrawStopPoint(VehicleRoute vehicle)
        {
            var pin = new Pin()
            {
                Type = PinType.Place,
                Label = PinLabel(vehicle),
                Anchor = new Point(.5, .5),
                Position = new Position(vehicle.Latitude, vehicle.Longitude),
                Icon = BitmapDescriptorFactory.FromResource("ic_stop.png"),
                Tag = "state_stop_route",
                ZIndex = 1,
                IsDraggable = false
            };

            Pins.Add(pin);
        }

        private string PinLabel(VehicleRoute vehicle)
        {
            if (Device.RuntimePlatform == Device.iOS)
                return string.Format("{0} {1}", vehicle.State.StartTime.FormatDateTimeWithSecond(), vehicle.State.Duration.SecondsToString());
            else
                return string.Format("{0} {1}: {2}", vehicle.State.StartTime.FormatDateTimeWithSecond(), MobileResource.Common_Label_Duration2, vehicle.State.Duration.SecondsToStringShort());
        }

        private void DrawDiretionMarker(List<VehicleRoute> listRoute)
        {
            List<DirectionMarker> diretionList = new List<DirectionMarker>();
            for (int i = 0; i < listRoute.Count - 1; i++)
            {
                diretionList.Add(new DirectionMarker().InitDirectionPoint(
                        listRoute[i].Latitude,
                        listRoute[i].Longitude,
                        listRoute[i + 1].Latitude,
                        listRoute[i + 1].Longitude,
                        listRoute[i].Time.FormatDateTimeWithSecond()));
            }
            // Lưu giá trị index được vẽ marker
            int indexDraw = 0;
            for (int i = 0; i < diretionList.Count - 1; i++)
            {
                // Tính hướng tối thiểu
                float phi = Math.Abs(diretionList[i].Direction
                        - diretionList[i + 1].Direction) % 360;
                float directionMin = phi > 180 ? 360 - phi : phi;
                // Tính khoản cách tối thiểu
                float distance = (float)GeoHelper.ComputeDistanceBetween(
                        diretionList[indexDraw].Position.Latitude, diretionList[indexDraw].Position.Longitude,
                        diretionList[indexDraw + 1].Position.Latitude, diretionList[indexDraw + 1].Position.Longitude);
                if (directionMin > 30 || distance > 1200)
                {
                    var pin = diretionList[indexDraw].DrawPointDiretion();
                    Pins.Add(pin);
                    indexDraw = i + 1;
                }
            }
        }

        private void ClearRoute()
        {
            if (Polylines != null)
                Polylines.Clear();
            if (Pins != null)
                Pins.Clear();
        }

        private void DrawRoute(List<VehicleRoute> listRoute)
        {
            DrawMarkerStartEnd(listRoute);
            DrawDiretionMarker(listRoute);
            DrawLine(listRoute);
        }

        private void InitVLC(string url)
        {
            try
            {
                MediaPlayer = new MediaPlayer(LibVLC);
                MediaPlayer.TimeChanged += Media_TimeChanged;
                MediaPlayer.EndReached += Media_EndReached;
                MediaPlayer.EncounteredError += Media_EncounteredError;
                MediaPlayer.SnapshotTaken += MediaPlayer_SnapshotTaken;
                MediaPlayer.Media = new Media(libVLC, new Uri(url));
                MediaPlayer.Play();
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void MediaPlayer_SnapshotTaken(object sender, MediaPlayerSnapshotTakenEventArgs e)
        {
            if (File.Exists(e.Filename))
            {
                DependencyService.Get<ICameraSnapShotServices>().SaveSnapShotToGalery(e.Filename);
            }
        }

        private void DisposeVLC()
        {
            try
            {
                if (MediaPlayer != null)
                {
                    MediaPlayer.TimeChanged -= Media_TimeChanged;
                    MediaPlayer.EndReached -= Media_EndReached;
                    MediaPlayer.EncounteredError -= Media_EncounteredError;
                    MediaPlayer.SnapshotTaken -= MediaPlayer_SnapshotTaken;

                    if (MediaPlayer.Media != null)
                    {
                        MediaPlayer.Media?.Dispose();
                        MediaPlayer.Media = null;
                    }

                    var media = MediaPlayer;
                    MediaPlayer = null;
                    media?.Dispose();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>
        /// Err : Fail connect server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        private void Media_EncounteredError(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Err : BỊ abort sau 10s không nhận tín hiệu từ server
        /// hoặc hết video hoặc video bị gọi đóng từ thiết bị khác
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Media_EndReached(object sender, EventArgs e)
        {
        }

        private void Media_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (MediaPlayer.Time > 1 && BusyIndicatorActive)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    BusyIndicatorActive = false;
                });
            }
        }

        public void SeekBarValueChanged(double value)
        {
            if (value <= 0)
            {
                if (AutoSwitch && isSelectPreOrNext == false)
                {
                    NextVideo();
                    isSelectPreOrNext = false;
                }
                else
                {
                    isSelectPreOrNext = false;
                }
            }
            else
            {
                isSelectPreOrNext = false;
            }
        }

        private bool isSelectPreOrNext = false;

        private void NextVideo()
        {
            //if (VideoSlected != null && !string.IsNullOrEmpty(VideoSlected.Link))
            //{
            //    isSelectPreOrNext = true;
            //    var index = VideoItemsSource.ToList().FindIndex(VideoSlected);
            //    if (index >= VideoItemsSource.Count - 1)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        VideoSlected = VideoItemsSource[index + 1];
            //    }
            //    InitVideoVideo(VideoSlected);

            //    isSelectPreOrNext = false;
            //}
        }

        private void PreviousVideo()
        {
            //if (VideoSlected != null && !string.IsNullOrEmpty(VideoSlected.Link))
            //{
            //    isSelectPreOrNext = true;
            //    var index = VideoItemsSource.ToList().FindIndex(VideoSlected);
            //    if (index == 0)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        VideoSlected = VideoItemsSource[index - 1];
            //    }

            //    InitVideoVideo(VideoSlected);

            //    isSelectPreOrNext = false;
            //}
        }

        private void InitVideoVideo(VideoUploadInfo obj)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IsError = false;
                BusyIndicatorActive = true;
            });
            // init ở đây :
            InitVLC(obj.Link);

            VideoSlected = obj; // Set màu select cho item
        }

        private void VideoSelectedChange(VideoUploadInfo obj)
        {
            if (obj != null)
            {
                SafeExecute(() =>
                {
                    isSelectPreOrNext = true;
                    InitVideoVideo(obj);
                });
            }
        }

        private void TakeSnapShot()
        {
            try
            {
                if (VideoSlected != null)
                {
                    var folderPath = DependencyService.Get<ICameraSnapShotServices>().GetFolderPath();
                    var current = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var fileName = current + ".jpg";
                    var filePath = Path.Combine(folderPath, fileName);
                    bool result = MediaPlayer.TakeSnapshot(0, filePath, 0, 0);
                    //if (File.Exists(filePath) && result)
                    //{
                    //    DependencyService.Get<ICameraSnapShotServices>().SaveSnapShotToGalery(filePath);
                    //}
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void DowloadVideo()
        {
            SafeExecute(async () =>
            {
                if (VideoSlected != null && !string.IsNullOrEmpty(VideoSlected.Link))
                {
                    var action = await PageDialog.DisplayAlertAsync("Thông báo", "Bạn có muốn tải video này về điện thoại không ?", "Đồng ý", "Bỏ qua");
                    if (action)
                    {
                        var progressIndicator = new Progress<double>(ReportProgress);
                        var cts = new CancellationTokenSource();
                        IsDownloading = true;
                        var permissionStatus = await CrossPermissions.Current.RequestPermissionAsync<Plugin.Permissions.StoragePermission>();
                        if (permissionStatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                        {
                            await _downloadService.DownloadFileAsync(VideoSlected.Link, progressIndicator, cts.Token);
                        }
                    }
                }
            });
        }

        internal void ReportProgress(double value)
        {
            ProgressValue = value;
            if (value == 100)
            {
                DisplayMessage.ShowMessageInfo("Đã tải video thành công");
                IsDownloading = false;
            }
        }

        private void DowloadVideoInListTapped(VideoUploadInfo obj)
        {
            SafeExecute(async () =>
            {
                if (obj != null && !string.IsNullOrEmpty(obj.Link))
                {
                    var action = await PageDialog.DisplayAlertAsync("Thông báo", "Bạn có muốn tải video này về điện thoại không ?", "Đồng ý", "Bỏ qua");
                    if (action)
                    {
                        var progressIndicator = new Progress<double>(ReportProgress);
                        var cts = new CancellationTokenSource();
                        IsDownloading = true;
                        var permissionStatus = await CrossPermissions.Current.RequestPermissionAsync<Plugin.Permissions.StoragePermission>();
                        if (permissionStatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                        {
                            await _downloadService.DownloadFileAsync(obj.Link, progressIndicator, cts.Token);
                        }
                    }
                }
            });
        }

        #endregion PrivateMethod
    }
}