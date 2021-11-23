using BA_MobileGPS.Core.Constant;
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
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class BACloudTabViewModel : ViewModelBase
    {
        public ICommand VideoItemTapCommand { get; set; }
        public ICommand SelectVehicleCameraCommand { get; }
        public ICommand DowloadVideoInListTappedCommand { get; }
        public ICommand HelpUploadCommand { get; }

        public ICommand SearchCommand { get; }
        private readonly IStreamCameraService _streamCameraService;
        private readonly IDownloadVideoService _downloadService;

        public BACloudTabViewModel(INavigationService navigationService, IStreamCameraService streamCameraService, IDownloadVideoService downloadService) : base(navigationService)
        {
            _streamCameraService = streamCameraService;
            _downloadService = downloadService;
            VideoItemTapCommand = new DelegateCommand<VideoUploadInfo>(VideoSelectedChange);
            SelectVehicleCameraCommand = new DelegateCommand(SelectVehicleCamera);
            SearchCommand = new DelegateCommand(SearchData);
            DowloadVideoInListTappedCommand = new DelegateCommand<VideoUploadInfo>(DowloadVideoInList);
            HelpUploadCommand = new DelegateCommand(HelpUpload);
            vehicle = new CameraLookUpVehicleModel();
            listChannel = new List<ChannelModel> { new ChannelModel() { Name = MobileResource.Camera_Lable_AllChannel, Value = 0 } };
            selectedChannel = listChannel[0];
        }

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            SetChannelSource();
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
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
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

        private CameraLookUpVehicleModel vehicle = new CameraLookUpVehicleModel();
        public CameraLookUpVehicleModel Vehicle { get => vehicle; set => SetProperty(ref vehicle, value); }

        private DateTime fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);

        public DateTime FromDate
        {
            get => fromDate;
            set => SetProperty(ref fromDate, value);
        }

        private DateTime toDate = DateTime.Now;

        public DateTime ToDate
        {
            get => toDate;
            set => SetProperty(ref toDate, value);
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
            if (FromDate > ToDate)
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Route_Label_StartDateMustSmallerThanEndDate);
                return false;
            }
            if (FromDate.Date != ToDate.Date)
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_RequiredDateTimeOver);
                return false;
            }
            else if (Vehicle == null || Vehicle.VehicleId == 0)
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_RequiredVehicle);
                return false;
            }
            else if (Vehicle != null && SelectedChannel.Value > Vehicle.Channel)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var action = await PageDialog.DisplayAlertAsync("Thông báo",
                          string.Format("Gói cước bạn đang sử dụng chỉ xem được {0} kênh. \nVui lòng liên hệ tới hotline {1} để được hỗ trợ",
                          Vehicle.Channel, MobileSettingHelper.HotlineGps),
                          "Liên hệ", "Bỏ qua");
                    if (action)
                    {
                        PhoneDialer.Open(MobileSettingHelper.HotlineGps);
                    }
                });
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
        private void GetListVideoDataFromCloud()
        {
            try
            {
                if (ValidateInput())
                {
                    var lstData = new List<VideoUploadInfo>();
                    RunOnBackground(async () =>
                    {
                        return await _streamCameraService.GetListCameraCloud(new Entities.GetCameraCloudRequest()
                        {
                            Customer = UserInfo.XNCode,
                            StartTime = FromDate,
                            EndTime = ToDate,
                            Vehicle = Vehicle.VehiclePlate,
                            Source = (int)CameraSourceType.App,
                            User = UserInfo.UserName,
                            SessionID = StaticSettings.SessionID
                        });
                    }, (result) =>
                    {
                        if (result != null && result.Data != null)
                        {
                            foreach (var item in result.Data)
                            {
                                lstData.Add(new VideoUploadInfo()
                                {
                                    Channel = (byte)item.Channel,
                                    EndTime = item.EndTime,
                                    StartTime = item.StartTime,
                                    Link = item.Link,
                                    VehicleName = result.Vehicle
                                });
                            }
                            VideoItemsSource = lstData.ToObservableCollection();
                        }
                    }, showLoading: true);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void SearchData()
        {
            GetListVideoDataFromCloud();
        }

        private void VideoSelectedChange(VideoUploadInfo obj)
        {
            if (obj != null)
            {
                SafeExecute(async () =>
                {
                    var parameters = new NavigationParameters
                    {
                        {   ParameterKey.VideoUploaded,obj
                        },
                        {
                            ParameterKey.LstVideoUploaded, VideoItemsSource.ToList()
                        }
                    };
                    _ = await NavigationService.NavigateAsync("BaseNavigationPage/ViewVideoUploadedPage", parameters, useModalNavigation: true, animated: true);
                });
            }
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