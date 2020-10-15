using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Entities;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Reflection;
using System.Threading;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;
using Timer = System.Timers.Timer;

namespace BA_MobileGPS.Core.Models
{
    public class CameraManagement : BindableBase, IDisposable
    {
        private LibVLC libVLC { get; }
        private int maxLoadingTime { get; }
        private Timer countLoadingTimer;
        private int counter { get; set; } // timer counter
        private bool internalError { get; set; }
        private long oldTime { get; set; } // get time to compare while error was happen

        public CameraManagement(int maxTimeLoadingMedia, LibVLC libVLC)
        {
            maxLoadingTime = maxTimeLoadingMedia;
            this.libVLC = libVLC;
            InitMediaPlayer();
            totalTime = 1;
            countLoadingTimer = new Timer(1000);
            countLoadingTimer.Elapsed += CountLoadingTimer_Elapsed;
            counter = maxLoadingTime;
            internalError = false;
            ReloadCommand = new DelegateCommand(Reload);
        }

        private int totalTime;
        public int TotalTime
        {
            get { return totalTime; }
            set
            {
                SetProperty(ref totalTime, value);
                RaisePropertyChanged();
            }
        }

        private bool isError;

        /// <summary>
        /// Bật màn hình lỗi khi true
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

        private StreamStart data;

        /// <summary>
        /// Save current data
        /// </summary>
        public StreamStart Data
        {
            get { return data; }
            set
            {
                SetProperty(ref data, value);
                RaisePropertyChanged();
            }
        }
         private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { SetProperty(ref isSelected, value);
                RaisePropertyChanged();
            }
        }

        private MediaPlayer mediaPlayer;

        /// <summary>
        /// Main class manage status od video
        /// </summary>
        public MediaPlayer MediaPlayer
        {
            get { return mediaPlayer; }
            set
            {
                SetProperty(ref mediaPlayer, value);
                RaisePropertyChanged();
            }
        }
        private double height;
        public double Height
        {
            get { return height; }
            set { SetProperty(ref height, value);
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

        public bool AutoRequestPing { get; set; }

        private void CountLoadingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            counter -= 1;
            if (!isLoaded)
            {
                if (internalError)
                {
                    if (counter <= 0)
                    {
                        SetError();
                    }
                    else
                    SetMedia(data.Link);                  
                }
            }
            else
            {
                if (internalError)
                {
                    SetError();
                }
                else if (MediaPlayer.IsPlaying)
                {
                    if (MediaPlayer.Time != oldTime)
                    {
                        oldTime = MediaPlayer.Time;
                        if (TotalTime > 0)
                        {
                            TotalTime -= 1;
                        }
                        else
                        {
                            SetError();
                        }
                    }
                }
                else // error
                {
                    SetError();
                }

            }
            //if (internalError && !isLoaded)
            //{
            //    SetMedia(data.Link);
            //    if (counter <= 0)
            //    {
            //        SetError();
            //    }
            //}
            //else
            //{
            //    if (isLoaded && MediaPlayer.IsPlaying)
            //    {
            //        //check error link broken
            //        if (MediaPlayer.Time != oldTime)
            //        {
            //            oldTime = MediaPlayer.Time;
            //            if (TotalTime > 0)
            //            {
            //                TotalTime -= 1;
            //            }
            //            else
            //            {
            //                SetError();
            //            }
            //        }
            //        else // error
            //        {
            //            SetError();
            //        }
            //    }
            //}
        }

        private void SetError()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IsError = true;
                IsLoaded = true;
                TotalTime = 0;
            });
            countLoadingTimer.Stop();
        }

        private void InitMediaPlayer()
        {
            mediaPlayer = new MediaPlayer(libVLC);
            mediaPlayer.TimeChanged += MediaPlayer_TimeChanged;
            mediaPlayer.EncounteredError += MediaPlayer_EncounteredError;
            mediaPlayer.EndReached += MediaPlayer_EndReached;
            mediaPlayer.Buffering += MediaPlayer_Buffering;
            mediaPlayer.AspectRatio = "16:9";
            mediaPlayer.Scale = 0;
        }

        private void MediaPlayer_Buffering(object sender, MediaPlayerBufferingEventArgs e)
        {
           
        }

        private void MediaPlayer_EndReached(object sender, EventArgs e)
        {
            
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

        private void MediaPlayer_EncounteredError(object sender, EventArgs e)
        {
            internalError = true;
        }

        private void MediaPlayer_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 1 && !IsLoaded)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoaded = true;
                    TotalTime = 180;
                    IsError = false;
                });
                internalError = false;
                //countLoadingTimer.Stop();
            }
        }

        public virtual void Clear()
        {
            try
            {
                internalError = false;
                countLoadingTimer.Stop();
                countLoadingTimer.Interval = 1000;
                counter = maxLoadingTime;
                //ThreadPool.QueueUserWorkItem((a) => { MediaPlayer.Stop(); });
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoaded = false;
                    IsError = false;
                    TotalTime = 1;
                });
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        public ICommand ReloadCommand { get; }

        private void Reload()
        {
            Clear();
            SetMedia(Data.Link);
        }

        public virtual bool CanExcute()
        {
            if (MediaPlayer != null && 
                MediaPlayer.Media != null && 
                MediaPlayer.Time > 0 && 
                !internalError)
            {
                return true;
            }
            return false;
        }

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
                    mediaPlayer.Buffering -= MediaPlayer_Buffering;
                    var media = MediaPlayer;
                    MediaPlayer = null;
                    media?.Dispose();
                }             
            }
            catch (Exception ex)
            {

            }
           
        }

        ~CameraManagement()
        {
            Dispose();
        }
    }
}