using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class DetailCameraViewModel : ViewModelBase
    {
        private string videoUrl = "";
        private readonly IStreamCameraService streamCameraService;
        private Timer timer;

        public DetailCameraViewModel(INavigationService navigationService, IStreamCameraService streamCameraService) : base(navigationService)
        {
            this.streamCameraService = streamCameraService;
            PlayIconSource = "ic_stop_white.png";
            PlayCommand = new DelegateCommand(Play);
            ScreenSizeChangedCommand = new DelegateCommand(ScreenSizeChanged);
            TakeScreenShotCommand = new DelegateCommand(TakeScreenShot);
            NavigationBackTappedCommand = new DelegateCommand(NavigationBackTapped);
            RequestMoreStreamTimeCommand = new DelegateCommand(RequestMoreTimeStream);
            ScreenOrientPortrait = true;
            VideoLoaded = false;
            RemainTime = "0:00";

        }

        private int time = 180;

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            time -= 1;
            if (time == 0)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var a = await NavigationService.GoBackAsync();
                });
            }
            TimeSpan t = TimeSpan.FromSeconds(time);
            RemainTime = string.Format("{0:D2}:{1:D2}",
                t.Minutes,
                t.Seconds);
            if (time == 90)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    RequestMoreTimeStream();
                });
            }
        }

        private void RequestMoreTimeStream()
        {
            TryExecute(async () =>
            {
                await SendRequestMoreTime();
            });
        }

        private async Task SendRequestMoreTime()
        {
            var response = await streamCameraService.RequestMoreStreamTime(new StreamPingRequest()
            {
                CustomerID = request.CustomerID,
                Duration = 600,
                VehicleName = request.VehicleName,
                Channel = channel
            });
            if (response.Data)
            {
                time += 600;
            }
            else
            {
                await SendRequestMoreTime();
            }
        }

        private StreamStartRequest request = new StreamStartRequest();
        private int channel = 0;

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters?.GetValue<StreamStart>("Channel") is StreamStart link && (parameters?.GetValue<StreamStartRequest>("Request") is StreamStartRequest request))
            {
                this.request = request;
                videoUrl = link.Link;
                channel = link.Channel;
                Title = "Kênh " + link.Channel.ToString();
                SetUpVlc();
            }
            else
            {
                NavigationService.GoBack();
            }
            base.OnNavigatedTo(parameters);
        }       

        public override void OnSleep()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await NavigationService.GoBackAsync();
            });
            base.OnSleep();          
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (!ScreenOrientPortrait)
            {
                DependencyService.Get<IScreenOrientServices>().ForcePortrait();
            }

            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }

        private string remainTime;

        public string RemainTime
        {
            get { return remainTime; }
            set
            {
                SetProperty(ref remainTime, value);
                RaisePropertyChanged();
            }
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
        public ICommand NavigationBackTappedCommand { get; }
        public ICommand RequestMoreStreamTimeCommand { get; }

        private void SetUpVlc()
        {
            LibVLCSharp.Shared.Core.Initialize();
            LibVLC = new LibVLC();
            InitMediaPlayer();
        }

        private void MediaPlayer_EncounteredError(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                await Task.Delay(5000);
                Device.BeginInvokeOnMainThread(() =>
                {
                    InitMediaPlayer();
                });             
            });
        }

        private void InitMediaPlayer()
        {
            var media = new Media(LibVLC,
                   new Uri(videoUrl));
            MediaPlayer = new MediaPlayer(media) { EnableHardwareDecoding = true };
            MediaPlayer.EncounteredError += MediaPlayer_EncounteredError;
            MediaPlayer.Buffering += MediaPlayer_Buffering;
            MediaPlayer.Play();
            count = 0;
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
                if (timer != null && !timer.Enabled)
                {
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
                timer.Stop();
                PlayIconSource = "ic_play_arrow_white.png";
            }
            else
            {
                MediaPlayer.Play();
                timer.Start();
                PlayIconSource = "ic_stop_white.png";
            }
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

        private void NavigationBackTapped()
        {
            TryExecute(async () =>
            {
                var title = "Thông báo";
                var content = "Bạn muốn quay lại, việc load lai camera sẽ mất 10s";
                var res = await PageDialog.DisplayAlertAsync(title, content, "Đồng ý", "Hủy");
                if (res)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var a = await NavigationService.GoBackAsync();
                    });
                }
            });
        }
    }
}