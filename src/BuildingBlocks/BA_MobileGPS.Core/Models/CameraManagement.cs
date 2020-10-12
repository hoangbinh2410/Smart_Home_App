﻿using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Views.Camera.MonitoringCamera;
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
    public class CameraManagement : BindableBase
    {
        private LibVLC libVLC { get; }
        private int maxLoadingTime { get; }
        private Timer countLoadingTimer;
        private int counter { get; set; } // timer counter
        private bool internalError { get; set; }
        private long oldTime { get; set; } // get time to compare while error was happen

        public CameraManagement(int maxTimeLoadingMedia, LibVLC libVLC, CameraEnum position)
        {
            maxLoadingTime = maxTimeLoadingMedia;
            this.libVLC = libVLC;
            InitMediaPlayer();
            totalTime = 1;
            this.position = position;
            countLoadingTimer = new Timer(1000);
            countLoadingTimer.Elapsed += CountLoadingTimer_Elapsed;
            counter = maxLoadingTime;
            internalError = false;
        }

        private void CountLoadingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            counter -= 1;
            if (internalError)
            {
                SetMedia(data.Link);
                if (counter <= 0)
                {
                    countLoadingTimer.Stop();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IsLoaded = true;
                        IsError = true;
                    });
                }
            }
            else
            {
                if (isLoaded)
                {
                    //check error link broken
                    if (MediaPlayer.Time != oldTime)
                    {
                        oldTime = MediaPlayer.Time;
                    }
                    else // error
                    {
                        IsError = true;
                        TotalTime = 0;
                        countLoadingTimer.Stop();
                    }
                }
            }
        }

        private void InitMediaPlayer()
        {
            mediaPlayer = new MediaPlayer(libVLC);
            mediaPlayer.TimeChanged += MediaPlayer_TimeChanged;
            mediaPlayer.EncounteredError += MediaPlayer_EncounteredError;
            mediaPlayer.AspectRatio = "16:9";
            mediaPlayer.Scale = 0;
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

        public StreamStart Data
        {
            get { return data; }
            set
            {
                SetProperty(ref data, value);
            }
        }

        private CameraEnum? position;

        public CameraEnum? Position
        {
            get { return position; }
            set
            {
                SetProperty(ref position, value);
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

        public virtual void Clear()
        {
            try
            {
                internalError = false;
                countLoadingTimer.Stop();
                counter = maxLoadingTime;
                ThreadPool.QueueUserWorkItem((a) => { MediaPlayer.Stop(); });
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoaded = false;
                    Data = null;
                    IsError = false;
                    TotalTime = 1;
                });
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public virtual bool CanExcute()
        {
            if (MediaPlayer.Media != null && MediaPlayer.Time > 0 && !internalError)
            {
                return true;
            }
            return false;
        }

        ~CameraManagement()
        {
            countLoadingTimer.Stop();
            countLoadingTimer.Elapsed -= CountLoadingTimer_Elapsed;
            countLoadingTimer.Dispose();
            mediaPlayer.TimeChanged -= MediaPlayer_TimeChanged;
            mediaPlayer.EncounteredError -= MediaPlayer_EncounteredError;
            var media = MediaPlayer;
            MediaPlayer = null;
            media?.Dispose();
        }
    }
}