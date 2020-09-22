using BA_MobileGPS.Core.Views.Camera.MonitoringCamera;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class CameraManagingPageViewModel : ViewModelBase
    {
        private readonly string playIconSource = "ic_play_arrow_white.png";
        private readonly string stopIconSource = "ic_stop_white.png";
        private string videoUrl1 = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerJoyrides.mp4";
        private string videoUrl2 = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/Sintel.mp4";
        private string videoUrl3 = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/SubaruOutbackOnStreetAndDirt.mp4";
        private string videoUrl4 = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/TearsOfSteel.mp4";
        //private string videoUrl1 = "rtsp://222.254.34.167:1935/live/868133043442003_CAM1";
        //private string videoUrl2 = "rtsp://222.254.34.167:1935/live/868133043442003_CAM1";
        //private string videoUrl3 = "rtsp://222.254.34.167:1935/live/868133043442003_CAM1";
        //private string videoUrl4 = "rtsp://222.254.34.167:1935/live/868133043442003_CAM1";
        public CameraManagingPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PlayCommand = new DelegateCommand(Play);
            CameraFrameTappedCommand = new DelegateCommand<object>(CameraFrameTapped);
            VolumeChangedCommand = new DelegateCommand(VolumeChanged);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }
        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            SetUpVlc();
        }
        public ICommand PlayCommand { get; }

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
        private MediaPlayer mediaPlayerNo1;

        public MediaPlayer MediaPlayerNo1
        {
            get { return mediaPlayerNo1; }
            set
            {
                SetProperty(ref mediaPlayerNo1, value);
                RaisePropertyChanged();
            }
        }
        private bool isCam1Loaded;
        public bool IsCam1Loaded
        {
            get { return isCam1Loaded; }
            set
            {
                SetProperty(ref isCam1Loaded, value);
                RaisePropertyChanged();
            }
        }
        private MediaPlayer mediaPlayerNo2;
        public MediaPlayer MediaPlayerNo2
        {
            get { return mediaPlayerNo2; }
            set
            {
                SetProperty(ref mediaPlayerNo2, value);
                RaisePropertyChanged();
            }
        }
        private bool isCam2Loaded;
        public bool IsCam2Loaded
        {
            get { return isCam2Loaded; }
            set
            {
                SetProperty(ref isCam2Loaded, value);
                RaisePropertyChanged();
            }
        }
        private MediaPlayer mediaPlayerNo3;
        public MediaPlayer MediaPlayerNo3
        {
            get { return mediaPlayerNo3; }
            set
            {
                SetProperty(ref mediaPlayerNo3, value);
                RaisePropertyChanged();
            }
        }
        private bool isCam3Loaded;
        public bool IsCam3Loaded
        {
            get { return isCam3Loaded; }
            set
            {
                SetProperty(ref isCam3Loaded, value);
                RaisePropertyChanged();
            }
        }
        private MediaPlayer mediaPlayerNo4;
        public MediaPlayer MediaPlayerNo4
        {
            get { return mediaPlayerNo4; }
            set
            {
                SetProperty(ref mediaPlayerNo4, value);
                RaisePropertyChanged();
            }
        }
        private bool isCam4Loaded;
        public bool IsCam4Loaded
        {
            get { return isCam4Loaded; }
            set
            {
                SetProperty(ref isCam4Loaded, value);
                RaisePropertyChanged();
            }
        }
        private string vehicleSelectedPlate;
        public string VehicleSelectedPlate
        {
            get { return vehicleSelectedPlate; }
            set
            {
                SetProperty(ref vehicleSelectedPlate, value);
                RaisePropertyChanged();
            }
        }

        private CameraSelectedEnum selectedCamera;
        public CameraSelectedEnum SelectedCamera
        {
            get { return selectedCamera; }
            set
            {
                SetProperty(ref selectedCamera, value);
                RaisePropertyChanged();
            }
        }
        private string playButtonIconSource;
        public string PlayButtonIconSource
        {
            get { return playButtonIconSource; }
            set
            {
                SetProperty(ref playButtonIconSource, value);
                RaisePropertyChanged();
            }
        }

        private void Play()
        {
            switch (SelectedCamera)
            {
                case CameraSelectedEnum.FirstCamera:
                    if (MediaPlayerNo1.CanPause)
                    {
                        MediaPlayerNo1.Pause();
                        PlayButtonIconSource = playIconSource;
                    }
                    else {
                        MediaPlayerNo1.Play();
                        PlayButtonIconSource = stopIconSource;
                    }                  
                    break;
                case CameraSelectedEnum.SecondCamera:
                    if (MediaPlayerNo2.CanPause)
                    {
                        MediaPlayerNo2.Pause();
                        PlayButtonIconSource = playIconSource;
                    }
                    else
                    {
                        MediaPlayerNo2.Play();
                        PlayButtonIconSource = stopIconSource;
                    }
                    break;
                case CameraSelectedEnum.ThirdCamera:
                    if (MediaPlayerNo3.CanPause)
                    {
                        MediaPlayerNo3.Pause();
                        PlayButtonIconSource = playIconSource;
                    }
                    else
                    {
                        MediaPlayerNo3.Play();
                        PlayButtonIconSource = stopIconSource;
                    }
                    break;
                case CameraSelectedEnum.FourthCamera:
                    if (MediaPlayerNo4.CanPause)
                    {
                        MediaPlayerNo4.Pause();
                        PlayButtonIconSource = playIconSource;
                    }
                    else
                    {
                        MediaPlayerNo4.Play();
                        PlayButtonIconSource = stopIconSource;
                    }
                    break;
            }
        }

        private void SetUpVlc()
        {
            LibVLCSharp.Shared.Core.Initialize();
            LibVLC = new LibVLC();
            InitMediaPlayer();
        }

        private void InitMediaPlayer()
        {
            var mediaNo1 = new Media(LibVLC, new Uri(videoUrl1));
            var mediaNo2 = new Media(LibVLC, new Uri(videoUrl2));
            var mediaNo3 = new Media(LibVLC, new Uri(videoUrl3));
            var mediaNo4 = new Media(LibVLC, new Uri(videoUrl4));
            MediaPlayerNo1 = new MediaPlayer(mediaNo1) { AspectRatio = "4:3", Scale = 0 };
            MediaPlayerNo1.TimeChanged += MediaPlayerNo1_TimeChanged;
            MediaPlayerNo1.Play();
            MediaPlayerNo2 = new MediaPlayer(mediaNo2) { AspectRatio = "4:3", Scale = 0 };
            MediaPlayerNo2.TimeChanged += MediaPlayerNo2_TimeChanged;
            MediaPlayerNo2.Play();
            MediaPlayerNo3 = new MediaPlayer(mediaNo3) { AspectRatio = "4:3", Scale = 0 };
            MediaPlayerNo3.TimeChanged += MediaPlayerNo3_TimeChanged;
            MediaPlayerNo3.Play();
            MediaPlayerNo4 = new MediaPlayer(mediaNo4) { AspectRatio = "4:3", Scale = 0 };
            MediaPlayerNo4.TimeChanged += MediaPlayerNo4_TimeChanged;
            MediaPlayerNo4.Play();
        }

        private void MediaPlayerNo4_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0)
            {
                IsCam4Loaded = true;
                MediaPlayerNo4.TimeChanged -= MediaPlayerNo4_TimeChanged;
            }            
        }

        private void MediaPlayerNo3_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time>0)
            {
                IsCam3Loaded = true;
                MediaPlayerNo3.TimeChanged -= MediaPlayerNo3_TimeChanged;
            }           
        }

        private void MediaPlayerNo2_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0)
            {
                IsCam2Loaded = true;
                MediaPlayerNo2.TimeChanged -= MediaPlayerNo2_TimeChanged;
            }           
        }

        private void MediaPlayerNo1_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0)
            {
                IsCam1Loaded = true;
                MediaPlayerNo1.TimeChanged -= MediaPlayerNo1_TimeChanged;
            }           
        }
  
        public ICommand VolumeChangedCommand { get; }
        private void VolumeChanged()
        {
            switch (SelectedCamera)
            {
                case CameraSelectedEnum.FirstCamera:
                    MediaPlayerNo1.Mute = !MediaPlayerNo1.Mute;
                    break;
                case CameraSelectedEnum.SecondCamera:
                    MediaPlayerNo2.Mute = !MediaPlayerNo2.Mute;
                    break;
                case CameraSelectedEnum.ThirdCamera:
                    MediaPlayerNo3.Mute = !MediaPlayerNo3.Mute;
                    break;
                case CameraSelectedEnum.FourthCamera:
                    MediaPlayerNo4.Mute = !MediaPlayerNo4.Mute;
                    break;
            }
        }
        public ICommand CameraFrameTappedCommand { get; }
        private void CameraFrameTapped(object obj)
        {
            SelectedCamera = (CameraSelectedEnum)obj;
            switch (SelectedCamera)
            {
                case CameraSelectedEnum.FirstCamera:
                    PlayButtonIconSource = MediaPlayerNo1.IsPlaying ? stopIconSource : playIconSource;
                    break;
                case CameraSelectedEnum.SecondCamera:
                    PlayButtonIconSource = MediaPlayerNo2.IsPlaying ? stopIconSource : playIconSource;
                    break;
                case CameraSelectedEnum.ThirdCamera:
                    PlayButtonIconSource = MediaPlayerNo3.IsPlaying ? stopIconSource : playIconSource;
                    break;
                case CameraSelectedEnum.FourthCamera:
                    PlayButtonIconSource = MediaPlayerNo4.IsPlaying ? stopIconSource : playIconSource;
                    break;
            }
        }

        public override void OnDestroy()
        {
            var mediaPlayer1 = MediaPlayerNo1;
            MediaPlayerNo1 = null;
            mediaPlayer1?.Dispose();

            var mediaPlayer2 = MediaPlayerNo2;
            MediaPlayerNo2 = null;
            mediaPlayer2?.Dispose();

            var mediaPlayer3 = MediaPlayerNo3;
            MediaPlayerNo3 = null;
            mediaPlayer3?.Dispose();

            var mediaPlayer4 = MediaPlayerNo4;
            MediaPlayerNo4 = null;
            mediaPlayer4?.Dispose();

            LibVLC?.Dispose();
            LibVLC = null;
        }

    }
}
