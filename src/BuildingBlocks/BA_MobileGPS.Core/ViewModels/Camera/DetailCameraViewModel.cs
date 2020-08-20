using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Views;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class DetailCameraViewModel : ViewModelBase
    {
        public DetailCameraViewModel(INavigationService navigationService) : base(navigationService)
        {
            SetUpVlc();
            PlayCommand = new DelegateCommand(Play);
            PlayIconSource = "ic_stop_white.png";
            ScreenSizeChangedCommand = new DelegateCommand(ScreenSizeChanged);
            ScreenOrientPortrait = true;
            videoLoaded = false;

        }
        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
            MediaPlayer.Buffering += MediaPlayer_Buffering;
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
            set { SetProperty(ref mediaPlayer, value);
                RaisePropertyChanged();
            }
        }

        private string playIconSource;
        public string PlayIconSource
        {
            get { return playIconSource; }
            set { SetProperty(ref playIconSource, value);
                RaisePropertyChanged();
            }
        }

        public override void OnResume()
        {
            base.OnResume();
            var media = new Media(LibVLC,
                  new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4"));

            MediaPlayer = new MediaPlayer(media) { EnableHardwareDecoding = true };
            MediaPlayer.Play();
        }
        public ICommand PlayCommand { get; }
        public ICommand ScreenSizeChangedCommand { get; }

        private void SetUpVlc()
        {
            try
            {
                LibVLCSharp.Shared.Core.Initialize();
                LibVLC = new LibVLC();

                //var media = new Media(LibVLC,
                //    new Uri("rtsp://222.254.34.167:1935/live/869092030971235_CAM1"));

                var media = new Media(LibVLC,
                    new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4"));

                MediaPlayer = new MediaPlayer(media) { EnableHardwareDecoding = true };
                MediaPlayer.SetAdjustInt(VideoAdjustOption.Enable, 10);
                MediaPlayer.SetAdjustInt(VideoAdjustOption.Gamma, 80);
                MediaPlayer.Play();              
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private bool videoLoaded;
        public bool VideoLoaded
        {
            get { return videoLoaded; }
            set { SetProperty(ref videoLoaded, value); 
                RaisePropertyChanged(); }
        }

      
        private void MediaPlayer_Buffering(object sender, MediaPlayerBufferingEventArgs e)
        {
            if (!VideoLoaded)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    VideoLoaded = true;
                });                            
            }            
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

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (!ScreenOrientPortrait)
            {
                DependencyService.Get<IScreenOrientServices>().ForcePortrait();
            }
        }
    }
   
}
