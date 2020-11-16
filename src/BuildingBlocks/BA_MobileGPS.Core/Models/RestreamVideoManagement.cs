using BA_MobileGPS.Entities;
using LibVLCSharp.Shared;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BA_MobileGPS.Core.Models
{
    public class RestreamVideoManagement : BindableBase, IDisposable
    {

        public RestreamVideoManagement(LibVLC libVLC)
        {
            this.LibVLC = libVLC;
            InitMediaPlayer();
        }

        private void InitMediaPlayer()
        {
            mediaPlayer = new MediaPlayer(libVLC);
            mediaPlayer.TimeChanged += MediaPlayer_TimeChanged;
            mediaPlayer.EncounteredError += MediaPlayer_EncounteredError;
            mediaPlayer.EndReached += MediaPlayer_EndReached;
            mediaPlayer.AspectRatio = "16:9";
            mediaPlayer.Scale = 0;
        }

        public void SetMedia(string url)
        {
            MediaPlayer.Media = new Media(libVLC, new Uri(url));
            MediaPlayer.Play();
        }

        // Hết video?
        private void MediaPlayer_EndReached(object sender, EventArgs e)
        {

        }

        private void MediaPlayer_EncounteredError(object sender, EventArgs e)
        {

        }

        private void MediaPlayer_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {

        }

        public StreamStart Data { get; set; }

        private LibVLC libVLC;
        public LibVLC LibVLC
        {
            get { return libVLC; }
            set
            {
                SetProperty(ref libVLC, value);
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
        // Đổi media, giư nguyên behaviour
        public void ClearMedia()
        {
            if (mediaPlayer.Media != null)
            {
                ThreadPool.QueueUserWorkItem((r) => { MediaPlayer.Stop(); });
                mediaPlayer.Media.Dispose();
                mediaPlayer.Media = null;
            }
        }
        // Chỉ dc gọi khi đóng TAb
        public void Dispose()
        {
            mediaPlayer.TimeChanged -= MediaPlayer_TimeChanged;
            mediaPlayer.EncounteredError -= MediaPlayer_EncounteredError;
            mediaPlayer.EndReached -= MediaPlayer_EndReached;
            mediaPlayer.Media?.Dispose();
            mediaPlayer.Media = null;
            var media = MediaPlayer;
            MediaPlayer = null;
            media?.Dispose();
            //mediaPlayer?.Media?.Dispose();
            //mediaPlayer?.Dispose();
        }
    }
}
