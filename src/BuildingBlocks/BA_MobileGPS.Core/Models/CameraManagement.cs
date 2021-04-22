using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using LibVLCSharp.Shared;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Timer = System.Timers.Timer;

namespace BA_MobileGPS.Core.Models
{
    public class CameraManagement : BindableBase, IDisposable
    {
        private int maxLoadingTime { get; }
        private Timer countLoadingTimer;
        private int counter { get; set; } // timer counter
        private bool internalError { get; set; }
        private bool loadingErr { get; set; }
        private readonly IStreamCameraService streamCameraService;
        private StreamStartRequest startRequest { get; set; }
        private CancellationTokenSource cts = new CancellationTokenSource();
        private readonly IEventAggregator _eventAggregator;

        public CameraManagement(int maxTimeLoadingMedia, LibVLC libVLC,
            IStreamCameraService streamCameraService, StreamStartRequest startRequest, IEventAggregator eventAggregator)
        {
            this.streamCameraService = streamCameraService;
            maxLoadingTime = maxTimeLoadingMedia;
            this.LibVLC = libVLC;
            InitMediaPlayer();
            totalTime = 1;
            countLoadingTimer = new Timer(1000);
            countLoadingTimer.Elapsed += CountLoadingTimer_Elapsed;
            counter = maxLoadingTime;
            internalError = false;
            AutoRequestPing = true;
            this.startRequest = startRequest;
            Channel = startRequest.Channel;
            _eventAggregator = eventAggregator;
            StartWorkUnit(startRequest.VehiclePlate);
        }

        private LibVLC libVLC;

        public LibVLC LibVLC
        {
            get { return libVLC; }
            set
            {
                SetProperty(ref libVLC, value);
                RaisePropertyChanged();
            }
        }

        private int totalTime;

        public int TotalTime
        {
            get { return totalTime; }
            set
            {
                SetProperty(ref totalTime, value);
                RaisePropertyChanged();
            }
        }

        private bool isError;

        /// <summary>
        /// Bật màn hình lỗi
        /// </summary>
        public bool IsError
        {
            get { return isError; }
            set
            {
                SetProperty(ref isError, value);
                RaisePropertyChanged();
            }
        }

        private StreamStart data;

        /// <summary>
        /// Save current data
        /// </summary>
        public StreamStart Data
        {
            get { return data; }
            set
            {
                SetProperty(ref data, value);
                RaisePropertyChanged();
            }
        }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                SetProperty(ref isSelected, value);
                RaisePropertyChanged();
            }
        }

        private MediaPlayer mediaPlayer;

        /// <summary>
        /// Main class manage status od video
        /// </summary>
        public MediaPlayer MediaPlayer
        {
            get { return mediaPlayer; }
            set
            {
                SetProperty(ref mediaPlayer, value);
                RaisePropertyChanged();
            }
        }

        private double height;

        public double Height
        {
            get { return height; }
            set
            {
                SetProperty(ref height, value);
                RaisePropertyChanged();
            }
        }

        private double width;

        public double Width
        {
            get { return width; }
            set
            {
                SetProperty(ref width, value);
                RaisePropertyChanged();
            }
        }

        private bool isLoaded;

        /// <summary>
        /// control busy indicator
        /// </summary>
        public bool IsLoaded
        {
            get { return isLoaded; }
            set
            {
                SetProperty(ref isLoaded, value);
                RaisePropertyChanged();
            }
        }

        private string errorMessenger;

        public string ErrorMessenger
        {
            get { return errorMessenger; }
            set
            {
                SetProperty(ref errorMessenger, value);
                RaisePropertyChanged();
            }
        }

        public bool AutoRequestPing { get; set; }

        public int Channel { get; set; }

        private void CountLoadingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            counter -= 1;
            if (!isLoaded)
            {
                if (internalError || loadingErr)
                {
                    if (counter <= 0)
                    {
                        var err = MobileResource.Camera_Label_Connection_Error;
                        SetError(err);
                    }
                    else
                    {
                        if (counter % 10 == 0)
                        {
                            Task.Run(async () =>
                            {
                                var requestStartResponse = await streamCameraService.StartStream(startRequest);
                            });
                        }
                    }
                }
            }
            else
            {
                if (TotalTime == 0)
                {
                    var err = MobileResource.Camera_Label_Timeout_Error;
                    SetError(err);
                }
                else
                {
                    TotalTime -= 1;
                    if (internalError)
                    {
                        var err = MobileResource.Camera_Label_Connection_Error;
                        SetError(err);
                    }
                }
            }
        }

        private void SetError(string errMessenger)
        {
            try
            {
                IsError = true;
                IsLoaded = true;
                ErrorMessenger = errMessenger;
                if (countLoadingTimer != null && countLoadingTimer.Enabled)
                {
                    countLoadingTimer.Stop();
                }
                if (mediaPlayer != null)
                {
                    ThreadPool.QueueUserWorkItem((r) => { MediaPlayer.Stop(); });
                    mediaPlayer?.Media?.Dispose();
                    mediaPlayer.Media = null;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>
        /// Init base vlc voi cac behaviour can thiet, chua co url media
        /// </summary>
        private void InitMediaPlayer()
        {
            mediaPlayer = new MediaPlayer(libVLC);
            mediaPlayer.TimeChanged += MediaPlayer_TimeChanged;
            mediaPlayer.EncounteredError += MediaPlayer_EncounteredError;
            mediaPlayer.EndReached += MediaPlayer_EndReached;
            mediaPlayer.AspectRatio = "16:9";
            mediaPlayer.Scale = 0;
        }

        /// <summary>
        /// raise khi co su co ket noi (loi mang hoac out time livestram)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaPlayer_EndReached(object sender, EventArgs e)
        {
            internalError = true;
        }

        /// <summary>
        /// thuong raise khi url sai hoac chua co ket noi den url
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaPlayer_EncounteredError(object sender, EventArgs e)
        {
            internalError = true;
        }

        /// <summary>
        /// start via trace current device,which has camera
        /// this func work after a request start sent successfully
        /// </summary>
        /// <param name="url">cam url</param>
        /// <param name="vehicle">vehicle Plate</param>
        public void StartWorkUnit(string vehicle)
        {
            Task.Run(async () =>
            {
                //startRequest.Channel = (int)Math.Pow(2, startRequest.Channel - 1);
                var requestStartResponse = await streamCameraService.StartStream(startRequest);
                // case trả về mã lỗi # 0 => báo lỗi
                if (requestStartResponse.StatusCode == 0)
                {
                    var startData = requestStartResponse?.Data?.FirstOrDefault();
                    if (startData != null)
                    {
                        Data = startData;
                        if (!countLoadingTimer.Enabled && counter == maxLoadingTime)
                        {
                            countLoadingTimer.Start();
                        }
                        IsLoaded = false;
                        //Check status:
                        StartTrackDeviceStatus(vehicle);
                    }
                }
                else
                {
                    Data = new StreamStart()
                    {
                        Channel = Channel,
                        Link = string.Empty
                    };
                    _eventAggregator.GetEvent<SendErrorCameraEvent>().Publish(Channel);
                    SetError(requestStartResponse.UserMessage);
                }
            });
        }

        private void StartTrackDeviceStatus(string vehicle)
        {
            int oldTimeout = 0;
            int oldTotalPackage = 0;
            try
            {
                Task.Run(async () =>
                {
                    while (!IsLoaded && MediaPlayer != null)
                    {
                        var deviceStatus = await streamCameraService.GetDevicesStatus(ConditionType.BKS, vehicle);
                        var device = deviceStatus?.Data?.FirstOrDefault();
                        if (device != null && device.CameraChannels != null)
                        {
                            var streamDevice = device.CameraChannels.FirstOrDefault(x => x.Channel == Data.Channel);

                            if (streamDevice != null && streamDevice.IsStreaming)
                            {
                                if (streamDevice.StreamingTotal != oldTotalPackage
                                    && streamDevice.StreamingTimeout != oldTimeout)
                                {
                                    //Set url nếu internal Err đã raise hoặc lần đầu khởi tạo.
                                    SetUrlMedia();
                                    oldTimeout = streamDevice.StreamingTimeout;
                                    oldTotalPackage = streamDevice.StreamingTotal;
                                }
                                else loadingErr = true;
                            }
                            else loadingErr = true;
                            await Task.Delay(2000);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void SetUrlMedia()
        {
            try
            {
                if (MediaPlayer != null && (internalError || MediaPlayer.Media == null))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IsError = false;
                        var url = string.Empty;
                        if (MobileSettingHelper.UseCameraRTMP)
                        {
                            url = Data.Link.Replace("rtsp", "rtmp");
                        }
                        else
                        {
                            url = Data.Link;
                        }
                        MediaPlayer.Media = new Media(libVLC, new Uri(url));
                        MediaPlayer.Play();
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError("SetMedia", ex);
            }
        }

        private void MediaPlayer_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 1 && !IsLoaded)
            {
                IsLoaded = true;
                TotalTime = 600;
                IsError = false;

                internalError = false;
            }
        }

        public virtual void Clear()
        {
            try
            {
                internalError = false;
                ErrorMessenger = string.Empty;
                loadingErr = false;
                countLoadingTimer.Stop();
                countLoadingTimer.Interval = 1000;
                counter = maxLoadingTime;
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoaded = false;
                    IsError = false;
                    TotalTime = 1;
                });
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public virtual bool CanExcute()
        {
            if (MediaPlayer != null &&
                MediaPlayer.Media != null &&
                MediaPlayer.Time > 0 &&
                !internalError && !isError)
            {
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            try
            {
                if (countLoadingTimer != null)
                {
                    countLoadingTimer.Stop();
                    countLoadingTimer.Elapsed -= CountLoadingTimer_Elapsed;
                    countLoadingTimer.Dispose();
                }
                if (MediaPlayer != null)
                {
                    mediaPlayer.TimeChanged -= MediaPlayer_TimeChanged;
                    mediaPlayer.EncounteredError -= MediaPlayer_EncounteredError;
                    mediaPlayer.EndReached -= MediaPlayer_EndReached;
                    mediaPlayer.Media?.Dispose();
                    mediaPlayer.Media = null;
                    var media = MediaPlayer;
                    MediaPlayer = null;
                    media?.Dispose();
                }
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        ~CameraManagement()
        {
            Dispose();
        }
    }
}