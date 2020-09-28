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
        private string videoUrl1 = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerJoyrides.mp4";
        private string videoUrl2 = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/Sintel.mp4";
        private string videoUrl3 = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/SubaruOutbackOnStreetAndDirt.mp4";
        private string videoUrl4 = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/TearsOfSteel.mp4";
        public CameraManagingPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PlayCommand = new DelegateCommand(Play);
            CameraFrameTappedCommand = new DelegateCommand<object>(CameraFrameTapped);
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

        private void Play()
        {

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
            MediaPlayerNo1 = new MediaPlayer(mediaNo1) {  AspectRatio = "4:3", Scale = 0 };
            MediaPlayerNo1.Play();
            MediaPlayerNo2 = new MediaPlayer(mediaNo2) {  AspectRatio = "4:3", Scale = 0 };
            MediaPlayerNo2.Play();
            MediaPlayerNo3 = new MediaPlayer(mediaNo3) { AspectRatio = "4:3", Scale = 0 };
            MediaPlayerNo3.Play();
            MediaPlayerNo4 = new MediaPlayer(mediaNo4) {  AspectRatio = "4:3", Scale = 0 };
            MediaPlayerNo4.Play();
        }

        public override void OnSleep()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await NavigationService.GoBackAsync();
            });
            base.OnSleep();
        }

        public ICommand CameraFrameTappedCommand { get; }
        private void CameraFrameTapped(object obj)
        {
            var a = (CameraSelectedEnum)obj;
            SelectedCamera = a;
        }
        public override void OnDestroy()
        {
            try
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

                //MediaPlayerNo1.Stop();
                //MediaPlayerNo1.Dispose();
                //MediaPlayerNo2.Stop();
                //MediaPlayerNo2.Dispose();
                //MediaPlayerNo3.Stop();
                //MediaPlayerNo3.Dispose();
                //MediaPlayerNo4.Stop();
                //MediaPlayerNo4.Dispose();
                LibVLC?.Dispose();
                LibVLC = null;

            }
            catch (Exception ex)
            {

                throw;
            }



        }

    }
}
