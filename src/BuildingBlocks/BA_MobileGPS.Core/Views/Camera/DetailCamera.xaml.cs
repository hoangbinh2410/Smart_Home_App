using LibVLCSharp.Shared;
using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

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
            media.MediaPlayer.UpdateViewpoint(90,90,0,120);
            media.MediaPlayer.SetAdjustInt(VideoAdjustOption.Enable, 1);
            media.MediaPlayer.SetAdjustInt(VideoAdjustOption.Hue, 50);
            media.MediaPlayer.SetAdjustInt(VideoAdjustOption.Brightness, 100);
        }
        private bool Straight = true; // dọc
        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            if (Straight)
            {
                media.MediaPlayer.UpdateViewpoint(90, 90, 0, 120);
                media.MediaPlayer.SetAdjustInt(VideoAdjustOption.Enable, 1);
                media.MediaPlayer.SetAdjustInt(VideoAdjustOption.Hue, 50);
                media.MediaPlayer.SetAdjustInt(VideoAdjustOption.Brightness, 100);
                var mediaPlayer = media.MediaPlayer;
                
                var videoTrack = GetVideoTrack(mediaPlayer);
                var videoView = media.VideoView;
               // media.RotateTo(90);
                var height = videoView.Height;
                var width = videoView.Width;
                //if (videoTrack == null)
                //{
                //    return;
                //}
                //var track = (VideoTrack)videoTrack;
                
                //var videoSwapped = IsVideoSwapped(track);
                //var videoWidth = videoSwapped ? track.Height : track.Width;
                //var videoHeigth = videoSwapped ? track.Width : track.Height;
                //if (videoWidth == 0 || videoHeigth == 0)
                //{
                //    mediaPlayer.Scale = 0;
                //}
                //else
                //{
                //    if (track.SarNum != track.SarDen)
                //    {
                //        videoWidth = videoWidth * track.SarNum / track.SarDen;
                //    }

                //    var ar = videoWidth / (double)videoHeigth;
                //    var videoViewWidth = videoView.Width;
                //    var videoViewHeight = videoView.Height;
                //    var dar = videoViewWidth / videoViewHeight;

                //    var rawPixelsPerViewPixel = DeviceDisplay.MainDisplayInfo.Density;
                //    var displayWidth = videoViewWidth * rawPixelsPerViewPixel;
                //    var displayHeight = videoViewHeight * rawPixelsPerViewPixel;
                //    mediaPlayer.Scale = (float)(dar >= ar ? (displayWidth / videoWidth) : (displayHeight / videoHeigth));
                //}
                //mediaPlayer.AspectRatio = null;
            }
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
    }
}
