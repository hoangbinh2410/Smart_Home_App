﻿using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Enums;
using BA_MobileGPS.Service;
using Plugin.Permissions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class MyVideoTabViewModel : ViewModelBase
    {
        public ICommand VideoItemTapCommand { get; set; }
        public ICommand SelectVehicleCameraCommand { get; }
        public ICommand DowloadVideoInListTappedCommand { get; }
        public ICommand HelpUploadCommand { get; }

        public ICommand SearchCommand { get; }
        private readonly IStreamCameraService _streamCameraService;
        private readonly IDownloadVideoService _downloadService;

        public MyVideoTabViewModel(INavigationService navigationService, IStreamCameraService streamCameraService, IDownloadVideoService downloadService) : base(navigationService)
        {
            _streamCameraService = streamCameraService;
            _downloadService = downloadService;
            VideoItemTapCommand = new DelegateCommand<VideoUploadInfo>(VideoSelectedChange);
            SelectVehicleCameraCommand = new DelegateCommand(SelectVehicleCamera);
            SearchCommand = new DelegateCommand(SearchData);
            DowloadVideoInListTappedCommand = new DelegateCommand<VideoUploadInfo>(DowloadVideoInList);
            HelpUploadCommand = new DelegateCommand(HelpUpload);
            vehicle = new CameraLookUpVehicleModel();
            EventAggregator.GetEvent<UploadVideoEvent>().Subscribe(UploadVideoRestream);
            EventAggregator.GetEvent<UploadFinishVideoEvent>().Subscribe(UploadFinishVideo);
            listChannel = new List<ChannelModel> { new ChannelModel() { Name = MobileResource.Camera_Lable_AllChannel, Value = 0 } };
            selectedChannel = listChannel[0];
        }

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            SetChannelSource();
            GetListVideoUpload();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<CameraLookUpVehicleModel>(ParameterKey.Vehicle) is CameraLookUpVehicleModel vehicle)
            {
                Vehicle = vehicle;
            }
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
            StartTimmer();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            timer.Stop();
            timer.Dispose();
            EventAggregator.GetEvent<UploadVideoEvent>().Unsubscribe(UploadVideoRestream);
            EventAggregator.GetEvent<UploadFinishVideoEvent>().Unsubscribe(UploadFinishVideo);
        }

        public override void OnSleep()
        {
            base.OnSleep();
        }

        public override void OnResume()
        {
            base.OnResume();
        }

        #endregion Lifecycle

        #region Property

        private System.Timers.Timer timer;

        private CameraLookUpVehicleModel vehicle = new CameraLookUpVehicleModel();
        public CameraLookUpVehicleModel Vehicle { get => vehicle; set => SetProperty(ref vehicle, value); }

        private DateTime dateStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);

        public DateTime DateStart
        {
            get => dateStart;
            set => SetProperty(ref dateStart, value);
        }

        private DateTime dateEnd = DateTime.Now;

        public DateTime DateEnd
        {
            get => dateEnd;
            set => SetProperty(ref dateEnd, value);
        }

        private ObservableCollection<VideoUpload> listVideoUpload;

        /// <summary>
        /// Source ảnh để chọn video
        /// </summary>
        public ObservableCollection<VideoUpload> ListVideoUpload { get => listVideoUpload; set => SetProperty(ref listVideoUpload, value); }

        private ObservableCollection<VideoUploadInfo> videoItemsSource;

        /// <summary>
        /// Source ảnh để chọn video
        /// </summary>
        public ObservableCollection<VideoUploadInfo> VideoItemsSource { get => videoItemsSource; set => SetProperty(ref videoItemsSource, value); }

        private List<ChannelModel> listChannel;

        /// <summary>
        /// Danh sách kênh
        /// </summary>
        public List<ChannelModel> ListChannel
        {
            get { return listChannel; }
            set { SetProperty(ref listChannel, value); }
        }

        private ChannelModel selectedChannel;

        /// <summary>
        /// Kênh được chọn
        /// </summary>
        public ChannelModel SelectedChannel
        {
            get { return selectedChannel; }
            set { SetProperty(ref selectedChannel, value); }
        }

        private double _progressValue;

        public double ProgressValue
        {
            get { return _progressValue; }
            set { SetProperty(ref _progressValue, value); }
        }

        private bool _isDownloading;

        public bool IsDownloading
        {
            get { return _isDownloading; }
            set { SetProperty(ref _isDownloading, value); }
        }

        #endregion Property

        #region PrivateMethod

        private void StartTimmer()
        {
            timer = new System.Timers.Timer
            {
                Interval = 5000
            };
            timer.Elapsed += T_Elapsed;

            timer.Start();
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            //nếu ko còn video nào upload thì ngừng timmer
            if (ListVideoUpload != null && ListVideoUpload.Count > 0)
            {
                GetListVideoUpload();
            }
        }

        /// <summary>
        /// Set dữ liệu cho picker channel
        /// Hard 4 kênh (Đã confirm)
        /// </summary>
        private void SetChannelSource()
        {
            try
            {
                var lstchannel = new List<int>() { 1, 2, 3, 4 };
                var source = new List<ChannelModel>();
                source.Add(new ChannelModel() { Name = MobileResource.Camera_Lable_AllChannel, Value = 0 });
                if (lstchannel != null)
                {
                    foreach (var item in lstchannel)
                    {
                        var temp = new ChannelModel()
                        {
                            Value = item,
                            Name = string.Format("{0} {1}", MobileResource.Camera_Lable_Channel, item)
                        };
                        source.Add(temp);
                    }
                }
                ListChannel = source;
                SelectedChannel = source.FirstOrDefault();
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private bool ValidateInput()
        {
            if (dateStart > dateEnd)
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Route_Label_StartDateMustSmallerThanEndDate);
                return false;
            }
            if (dateStart.Date != dateEnd.Date)
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_RequiredDateTimeOver);
                return false;
            }
            else if (Vehicle == null || Vehicle.VehicleId == 0)
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_RequiredVehicle);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void SelectVehicleCamera()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleCameraLookup", null, useModalNavigation: true, animated: true);
            });
        }

        /// <summary>
        /// Lấy danh sách video từ server
        /// </summary>
        private void GetListVideoDataFrom()
        {
            try
            {
                if (ValidateInput())
                {
                    VideoItemsSource = new ObservableCollection<VideoUploadInfo>();
                    RunOnBackground(async () =>
                    {
                        return await _streamCameraService.GetListVideoOnCloud(new Entities.CameraRestreamRequest()
                        {
                            CustomerId = UserInfo.XNCode,
                            Date = DateStart.Date,
                            VehicleNames = Vehicle.VehiclePlate
                        });
                    }, (result) =>
                    {
                        if (result != null && result.Count > 0)
                        {
                            var lstvideoupload = new List<VideoUploadInfo>();
                            foreach (var item in result)
                            {
                                foreach (var item1 in item.Data)
                                {
                                    item1.Channel = item.Channel;
                                    item1.VehicleName = item.VehicleName;
                                    item1.EndTime = item1.StartTime.AddSeconds(item1.Duration);
                                }
                                lstvideoupload.AddRange(item.Data);
                            }
                            if (lstvideoupload != null && lstvideoupload.Count > 0)
                            {
                                var lstvideo = new List<VideoUploadInfo>();
                                var video = lstvideoupload.Where(x => x.StartTime >= DateStart
                                && x.StartTime <= DateEnd
                                && (x.Channel == SelectedChannel.Value
                                || SelectedChannel.Value == 0
                                || SelectedChannel == null)).OrderBy(x => x.StartTime).ToList();
                                lstvideo.AddRange(video);
                                VideoItemsSource = lstvideo.ToObservableCollection();
                            }
                        }
                    }, showLoading: true);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private async void GetListVideoUpload()
        {
            List<VideoUpload> result = new List<VideoUpload>();
            try
            {
                if (StaticSettings.ListVehilceCamera != null && StaticSettings.ListVehilceCamera.Count > 0)
                {
                    var listVehicle = StaticSettings.ListVehilceCamera.Select(x => x.VehiclePlate).ToList();
                    var respone = await _streamCameraService.GetUploadingProgressInfor(new UploadStatusRequest()
                    {
                        CustomerID = UserInfo.XNCode,
                        VehicleName = listVehicle,
                        Source = (int)CameraSourceType.App,
                        User = UserInfo.UserName,
                    });
                    if (respone != null && respone.Count > 0)
                    {
                        foreach (var item in respone)
                        {
                            if (item.UploadFiles != null && item.UploadFiles.Count > 0)
                            {
                                var list = item.UploadFiles.Where(x => x.State != (int)VideoUploadStatus.Uploaded);
                                foreach (var item1 in list)
                                {
                                    var model = new VideoUpload()
                                    {
                                        Channel = item.Channel,
                                        StartTime = item1.Time,
                                        EndTime = item.EndTime,
                                        Status = (VideoUploadStatus)item1.State,
                                        VehicleName = item.VehicleName
                                    };
                                    result.Add(model);
                                }
                            }
                        }
                        ListVideoUpload = new ObservableCollection<VideoUpload>(result.OrderBy(x => x.Status).ThenBy(x => x.StartTime));
                    }
                }
            }
            catch (System.Exception)
            {
            }
        }

        private void SearchData()
        {
            GetListVideoDataFrom();
        }

        private void VideoSelectedChange(VideoUploadInfo obj)
        {
            if (obj != null)
            {
                SafeExecute(async () =>
                {
                    var parameters = new NavigationParameters
                    {
                        {   ParameterKey.VideoUploaded, obj
                        },
                        {
                            ParameterKey.LstVideoUploaded, VideoItemsSource.ToList()
                        }
                    };
                    _ = await NavigationService.NavigateAsync("BaseNavigationPage/ViewVideoUploadedPage", parameters, useModalNavigation: true, animated: true);
                });
            }
        }

        private void UploadVideoRestream(bool obj)
        {
            GetListVideoUpload();
        }

        private void UploadFinishVideo(bool obj)
        {
            GetListVideoUpload();
        }

        private void DowloadVideoInList(VideoUploadInfo obj)
        {
            SafeExecute(async () =>
            {
                if (obj != null && !string.IsNullOrEmpty(obj.Link))
                {
                    var action = await PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification, MobileResource.Camera_Message_DoYouWantDowloadVideo, MobileResource.Common_Button_OK, MobileResource.Common_Message_Skip);
                    if (action)
                    {
                        var progressIndicator = new Progress<double>(ReportProgress);
                        var cts = new CancellationTokenSource();
                        IsDownloading = true;
                        var permissionStatus = await CrossPermissions.Current.RequestPermissionAsync<Plugin.Permissions.StoragePermission>();
                        if (permissionStatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                        {
                            await _downloadService.DownloadFileAsync(obj.Link, progressIndicator, cts.Token);
                        }
                    }
                }
            });
        }

        internal void ReportProgress(double value)
        {
            ProgressValue = value;
            if (value == 100)
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Camera_Message_DowloadVideoSuccess);
                IsDownloading = false;
            }
        }

        private void HelpUpload()
        {
            SafeExecute(async () =>
            {
                await PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification, MobileResource.Camera_Message_VideoSaveToServer, MobileResource.Common_Message_Skip);
            });
        }

        #endregion PrivateMethod
    }
}