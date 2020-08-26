using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.IO;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;



namespace BA_MobileGPS.Core.ViewModels
{
    public class DetailCameraViewModel : ViewModelBase
    {
        private string videoUrl = "rtsp://222.254.34.167:1935/live/869092030971235_CAM1";
        private readonly IStreamCameraService streamCameraService;
        private Timer timer;
        public DetailCameraViewModel(INavigationService navigationService,IStreamCameraService streamCameraService) : base(navigationService)
        {
            this.streamCameraService = streamCameraService;            
            PlayIconSource = "ic_stop_white.png";
            PlayCommand = new DelegateCommand(Play);            
            ScreenSizeChangedCommand = new DelegateCommand(ScreenSizeChanged);
            TakeScreenShotCommand = new DelegateCommand(TakeScreenShot);
            ScreenOrientPortrait = true;
            VideoLoaded = false;
            var a = new StreamStartRequest() { CustomerID = 1010, Channel = 5, VehicleName = "PNC.CAM1" };
            RemainTime = "0:00";
         
            
        }
        private int time = 180;
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            time -= 1;
            TimeSpan t = TimeSpan.FromSeconds(time);
            RemainTime = string.Format("{0:D2}:{1:D2}",
                t.Minutes,
                t.Seconds);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters?.GetValue<string>("Channel") is string link)
            {
                //videoUrl = link;
                SetUpVlc();
            }
            else
            {
                NavigationService.GoBack();
            }
            base.OnNavigatedTo(parameters);                  
        }
        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
            MediaPlayer.Buffering += MediaPlayer_Buffering;
        }

        public override void OnResume()
        {
            base.OnResume();
            var media = new Media(LibVLC,
                  new Uri(videoUrl));

            MediaPlayer = new MediaPlayer(media) { EnableHardwareDecoding = true };
            MediaPlayer.Play();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (!ScreenOrientPortrait)
            {
                DependencyService.Get<IScreenOrientServices>().ForcePortrait();
            }
            timer.Dispose();
            //streamCameraService.StopStream(new StreamStopRequest() { })
        }

        private string remainTime;
        public string RemainTime
        {
            get { return remainTime; }
            set { SetProperty(ref remainTime, value);
                RaisePropertyChanged(); }
        }

        private bool screenOrientPortrait;

        public bool ScreenOrientPortrait  //true = doc
        {
            get { return screenOrientPortrait; }
            set
            {
                SetProperty(ref screenOrientPortrait, value);
                RaisePropertyChanged();
            }
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

        private MediaPlayer mediaPlayer;

        public MediaPlayer MediaPlayer
        {
            get { return mediaPlayer; }
            set
            {
                SetProperty(ref mediaPlayer, value);
                RaisePropertyChanged();
            }
        }

        private string playIconSource;

        public string PlayIconSource
        {
            get { return playIconSource; }
            set
            {
                SetProperty(ref playIconSource, value);
                RaisePropertyChanged();
            }
        }

        public ICommand PlayCommand { get; }
        public ICommand ScreenSizeChangedCommand { get; }
        public ICommand TakeScreenShotCommand { get; }
        private void SetUpVlc()
        {
            LibVLCSharp.Shared.Core.Initialize();
            LibVLC = new LibVLC();

            var media = new Media(LibVLC,
                new Uri(videoUrl));

            MediaPlayer = new MediaPlayer(media) { EnableHardwareDecoding = true };
            MediaPlayer.SetAdjustInt(VideoAdjustOption.Enable, 10);
            MediaPlayer.SetAdjustInt(VideoAdjustOption.Gamma, 80);
            MediaPlayer.Play();
        }

        private bool videoLoaded;

        public bool VideoLoaded
        {
            get { return videoLoaded; }
            set
            {
                SetProperty(ref videoLoaded, value);
                RaisePropertyChanged();
            }
        }
        private int count = 0;
        private void MediaPlayer_Buffering(object sender, MediaPlayerBufferingEventArgs e)
        {
          
            if (count > 1)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    VideoLoaded = true;
                });
                if (timer == null)
                {
                    timer = new Timer();
                    timer.Interval = 1000;
                    timer.Elapsed += Timer_Elapsed;

                    timer.Start();
                }
               
            }
            count += 1;
        }

        private void Play()
        {
            if (MediaPlayer.IsPlaying)
            {
                MediaPlayer.Pause();
                PlayIconSource = "ic_play_arrow_white.png";
            }
            else
            {
                MediaPlayer.Play();
                PlayIconSource = "ic_stop_white.png";
            }
            var a = MediaPlayer.AdjustInt(VideoAdjustOption.Enable);
            var b = MediaPlayer.AdjustInt(VideoAdjustOption.Gamma);
        }

        private void ScreenSizeChanged()
        {
            if (ScreenOrientPortrait)
            {
                DependencyService.Get<IScreenOrientServices>().ForceLandscape();
            }
            else
            {
                DependencyService.Get<IScreenOrientServices>().ForcePortrait();
            }
            ScreenOrientPortrait = !ScreenOrientPortrait;
        }
        private void TakeScreenShot()
        {
           // var a = MediaPlayer.TakeSnapshot(0,"BAGPS",800,600);

           // var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
           
        }
    }
}