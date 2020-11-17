using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using LibVLCSharp.Shared;
using Prism.Mvvm;
using System;
using System.Reflection;
using System.Threading;
using System.Timers;
using Xamarin.Forms;
using Timer = System.Timers.Timer;

namespace BA_MobileGPS.Core.Models
{
    public class RestreamVideoManagement : BindableBase, IDisposable
    {
        private int maxLoadingTime { get; }
        private Timer countLoadingTimer { get; set; }
        private int counter { get; set; } // timer counter
        private bool internalError { get; set; }
        public RestreamVideoManagement(int maxTimeLoadingMedia, LibVLC libVLC)
        {
            maxLoadingTime = maxTimeLoadingMedia;
            this.LibVLC = libVLC;
            InitMediaPlayer();      
            countLoadingTimer = new Timer(1000);
            countLoadingTimer.Elapsed += CountLoadingTimer_Elapsed;
            counter = maxLoadingTime;
            internalError = false;     
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

        private bool isError;

        /// <summary>
        /// Bật màn hình lỗi
        /// </summary>
        public bool IsError
        {
            get { return isError; }
            set
            {
                SetProperty(ref isError, value);
                RaisePropertyChanged();
            }
        }
        private bool isLoaded;

        public bool IsLoaded
        {
            get { return isLoaded; }
            set
            {
                SetProperty(ref isLoaded, value);
                RaisePropertyChanged();
            }
        }

        private string errorMessenger;

        public string ErrorMessenger
        {
            get { return errorMessenger; }
            set
            {
                SetProperty(ref errorMessenger, value);
                RaisePropertyChanged();
            }
        }
        private void CountLoadingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            counter -= 1;
            if (!isLoaded)
            {
                if (internalError)
                {
                    if (counter <= 0)
                    {
                        var err = MobileResource.Camera_Label_Connection_Error;
                        SetError(err);
                    }
                    else SetMedia(Data.Link);
                }
            }
            else
            {
                if (internalError)
                {
                    var err = MobileResource.Camera_Label_Connection_Error;
                    SetError(err);
                }
            }
        }

        private void SetError(string errMessenger)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IsError = true;
                IsLoaded = true;
                ErrorMessenger = errMessenger;
            });
            countLoadingTimer.Stop();
            ThreadPool.QueueUserWorkItem((r) => { MediaPlayer.Stop(); });
            mediaPlayer.Media.Dispose();
            mediaPlayer.Media = null;
        }

        // Hết video?
        private void MediaPlayer_EndReached(object sender, EventArgs e)
        {
            internalError = true;
        }

        private void MediaPlayer_EncounteredError(object sender, EventArgs e)
        {
            internalError = true;
        }

        public void SetMedia(string url)
        {
            if (!countLoadingTimer.Enabled && counter == maxLoadingTime)
            {
                countLoadingTimer.Start();
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    IsLoaded = false;
                    MediaPlayer.Media = new Media(libVLC, new Uri(url));
                    MediaPlayer.Play();
                }
                catch (Exception ex)
                {
                    LoggerHelper.WriteError("SetMedia", ex);
                }
            });
        }

        private void MediaPlayer_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 1 && !IsLoaded)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoaded = true;
                    IsError = false;
                });
                internalError = false;
            }
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
            try
            {
                if (mediaPlayer.Media != null)
                {
                    ThreadPool.QueueUserWorkItem((r) => { MediaPlayer.Stop(); });
                    mediaPlayer.Media.Dispose();
                    mediaPlayer.Media = null;
                }
            
                internalError = false;
                ErrorMessenger = string.Empty;
                countLoadingTimer.Stop();
                countLoadingTimer.Interval = 1000;
                counter = maxLoadingTime;
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoaded = false;
                    IsError = false;
                });
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        // Chỉ dc gọi khi đóng TAb
        public void Dispose()
        {
            try
            {
                if (countLoadingTimer != null)
                {
                    countLoadingTimer.Stop();
                    countLoadingTimer.Elapsed -= CountLoadingTimer_Elapsed;
                    countLoadingTimer.Dispose();
                }
                if (MediaPlayer != null)
                {
                    mediaPlayer.TimeChanged -= MediaPlayer_TimeChanged;
                    mediaPlayer.EncounteredError -= MediaPlayer_EncounteredError;
                    mediaPlayer.EndReached -= MediaPlayer_EndReached;
                    mediaPlayer.Media?.Dispose();
                    mediaPlayer.Media = null;
                    var media = MediaPlayer;
                    MediaPlayer = null;
                    media?.Dispose();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }
    }
}
