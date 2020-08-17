using BA_MobileGPS.Entities;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BA_MobileGPS.Core.ViewModels
{
    public class CameraStreamViewModel : ViewModelBase
    {
        public CameraStreamViewModel(INavigationService navigationService) : base(navigationService)
        {

        }
        private VehicleOnline vehicleOnline { get; set; }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            try
            {
                if (parameters?.GetValue<VehicleOnline>("Camera") is VehicleOnline cardetail)
                {
                    vehicleOnline = cardetail;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            SetUpVlc();
        }

        private LibVLC libVLC;
        public LibVLC LibVLC
        {
            get { return libVLC; }
            set { SetProperty(ref libVLC, value);
                RaisePropertyChanged();
            }
        }
        private MediaPlayer mediaPlayer;
        public MediaPlayer MediaPlayer
        {
            get { return mediaPlayer; }
            set { SetProperty(ref mediaPlayer, value); }
        }

        private void SetUpVlc()
        {
            try
            {
                LibVLCSharp.Shared.Core.Initialize();
                LibVLC = new LibVLC();

                var media = new Media(LibVLC,
                    new Uri("rtsp://222.254.34.167:1935/live/869092030973074_CAM1"));

                MediaPlayer = new MediaPlayer(media) { EnableHardwareDecoding = true };
            }
            catch (Exception ex)
            {

                throw;
            }

            MediaPlayer.Play();
        }
    }
}
