using LibVLCSharp.Forms.Shared;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class DetailCameraViewModel : ViewModelBase
    {
        public DetailCameraViewModel(INavigationService navigationService) : base(navigationService)
        {
            SetUpVlc();
            CameraLoading = true;
            PlayCommand = new DelegateCommand(Play);
            PlayIconSource = "ic_heart.png";
   
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
        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
            MediaPlayer.Buffering += MediaPlayer_Buffering;
        }
        private MediaPlayer mediaPlayer;     
        public MediaPlayer MediaPlayer
        {
            get { return mediaPlayer; }
            set { SetProperty(ref mediaPlayer, value);
                RaisePropertyChanged();
            }
        }

        private bool cameraLoading;
        public bool CameraLoading
        {
            get { return cameraLoading; }
            set { SetProperty(ref cameraLoading, value);
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

        public ICommand PlayCommand { get; }
 
        private void SetUpVlc()
        {
            try
            {
                LibVLCSharp.Shared.Core.Initialize();
                LibVLC = new LibVLC();
                
                var media = new Media(LibVLC,
                    new Uri("rtsp://222.254.34.167:1935/live/869092030971235_CAM1"));

                MediaPlayer = new MediaPlayer(media) { EnableHardwareDecoding = true };      
                MediaPlayer.Play();              
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private bool LoadingSupport = true;
        private void MediaPlayer_Buffering(object sender, MediaPlayerBufferingEventArgs e)
        {
            if (LoadingSupport)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CameraLoading = false;
                });
                LoadingSupport = false;
            }           
        }
        private void Play()
        {
            if (MediaPlayer.IsPlaying)
            {
                MediaPlayer.Pause();
                PlayIconSource = "ic_lock.png";
            }
            else
            {
                MediaPlayer.Play();
                PlayIconSource = "ic_heart.png";
            }
        }

       

     


    }
   
}
