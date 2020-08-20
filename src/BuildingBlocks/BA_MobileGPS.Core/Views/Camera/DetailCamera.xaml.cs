using BA_MobileGPS.Core.Interfaces;
using LibVLCSharp.Shared;
using Prism.Events;
using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Prism.Ioc;

namespace BA_MobileGPS.Core.Views
{
    public partial class DetailCamera : ContentPage
    {
        public DetailCamera()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var eventCenter =  Prism.PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
            eventCenter.GetEvent<SetFitScreenEvent>().Subscribe(SetFitScreen);
        }


        private void SetFitScreen()
        {
            var videoView = media.VideoView;
            var videoTrack = GetVideoTrack(media.MediaPlayer);
            if (videoTrack == null)
            {
                return;
            }
            var track = (VideoTrack)videoTrack;
            var videoSwapped = IsVideoSwapped(track);
            var videoWidth = videoSwapped ? track.Height : track.Width;
            var videoHeigth = videoSwapped ? track.Width : track.Height;
            if (videoWidth == 0 || videoHeigth == 0)
            {
                media.MediaPlayer.Scale = 0;
            }
            else
            {
                if (track.SarNum != track.SarDen)
                {
                    videoWidth = videoWidth * track.SarNum / track.SarDen;
                }

                var ar = videoWidth / (double)videoHeigth;
                var videoViewWidth = videoView.Width;
                var videoViewHeight = videoView.Height;
                var dar = videoViewWidth / videoViewHeight;

                var rawPixelsPerViewPixel = Device.Info.ScalingFactor;
                var displayWidth = videoViewWidth * rawPixelsPerViewPixel;
                var displayHeight = videoViewHeight * rawPixelsPerViewPixel;
                media.MediaPlayer.Scale = (float)(dar >= ar ? (displayWidth / videoWidth) : (displayHeight / videoHeigth));
            }
            media.MediaPlayer.AspectRatio = null;
        }

        private VideoTrack? GetVideoTrack(MediaPlayer mediaPlayer)
        {
            if (mediaPlayer == null)
            {
                return null;
            }
            var selectedVideoTrack = mediaPlayer.VideoTrack;
            if (selectedVideoTrack == -1)
            {
                return null;
            }

            try
            {
                var videoTrack = mediaPlayer.Media?.Tracks?.FirstOrDefault(t => t.Id == selectedVideoTrack);
                return videoTrack == null ? (VideoTrack?)null : ((MediaTrack)videoTrack).Data.Video;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool IsVideoSwapped(VideoTrack videoTrack)
        {
            var orientation = videoTrack.Orientation;
            return orientation == VideoOrientation.LeftBottom || orientation == VideoOrientation.RightTop;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var eventCenter = Prism.PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
            eventCenter.GetEvent<SetFitScreenEvent>().Unsubscribe(SetFitScreen);
        }
    }

    public class SetFitScreenEvent : PubSubEvent
    {

    }
}
