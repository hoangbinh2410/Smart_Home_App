using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class UploadVideoPageViewModel : ViewModelBase
    {
        private readonly IStreamCameraService streamCameraService;
        private CancellationTokenSource cts = new CancellationTokenSource();
        public ICommand UploadVideoCommand { get; }

        public ICommand GotoMyVideoPageCommand { get; }

        public UploadVideoPageViewModel(INavigationService navigationService, IStreamCameraService streamCameraService) : base(navigationService)
        {
            this.streamCameraService = streamCameraService;
            UploadVideoCommand = new Command(UploadVideo);
            GotoMyVideoPageCommand = new Command(GotoMyVideoPage);
            isCheckAll = true;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.TryGetValue("UploadVideo", out CameraUploadRequest obj))
                {
                    RequestInfo = obj;
                    GetListVideoUploadActive();
                }
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private CameraUploadRequest requestInfo;

        /// <summary>
        /// Ảnh được focus
        /// </summary>
        public CameraUploadRequest RequestInfo
        {
            get => requestInfo;
            set
            {
                SetProperty(ref requestInfo, value);
            }
        }

        private bool isCheckAll;

        /// <summary>
        /// Ảnh được focus
        /// </summary>
        public bool IsCheckAll
        {
            get => isCheckAll;
            set
            {
                SetProperty(ref isCheckAll, value);
            }
        }

        private bool isShowBtnUpload = true;

        /// <summary>
        /// Ảnh được focus
        /// </summary>
        public bool IsShowBtnUpload
        {
            get => isShowBtnUpload;
            set
            {
                SetProperty(ref isShowBtnUpload, value);
            }
        }

        private VideoRestreamInfo videoRestreamInfo = new VideoRestreamInfo();
        public VideoRestreamInfo VideoRestreamInfo { get => videoRestreamInfo; set => SetProperty(ref videoRestreamInfo, value); }
        private ObservableCollection<VideoUploadTimeInfo> listVideo = new ObservableCollection<VideoUploadTimeInfo>();
        public ObservableCollection<VideoUploadTimeInfo> ListVideo { get => listVideo; set => SetProperty(ref listVideo, value); }

        private void GetListVideoUploadActive()
        {
            RunOnBackground(async () =>
            {
                return await streamCameraService.GetListVideoNotUpload(RequestInfo);
            }, (result) =>
            {
                if (result != null && result.Data != null && result.Data.Count > 0)
                {
                    VideoRestreamInfo = result;
                    foreach (var item in result.Data)
                    {
                        if (item.IsUploaded)
                        {
                            item.Status = VideoUploadStatus.Uploaded;
                        }
                        else
                        {
                            if (StaticSettings.ListVideoUpload != null && StaticSettings.ListVideoUpload.Count > 0)
                            {
                                var model = StaticSettings.ListVideoUpload.FirstOrDefault(x => x.FileName.Equals(item.FileName));
                                if (model != null)
                                {
                                    item.Status = model.Status;
                                }
                                else
                                {
                                    item.Status = VideoUploadStatus.NotUpload;
                                }
                            }
                            else
                            {
                                item.Status = VideoUploadStatus.NotUpload;
                            }
                        }
                        if (item.IsSelected == true && item.IsUploaded == false && item.Status == VideoUploadStatus.NotUpload)
                        {
                            IsShowBtnUpload = true;
                        }
                        else
                        {
                            IsShowBtnUpload = false;
                        }
                    }
                    ListVideo = result.Data.OrderBy(x => x.StartTime).ToObservableCollection();
                }
            }, showLoading: true);
        }

        private void UploadVideo(object obj)
        {
            SafeExecute(async () =>
            {
                var lstvideoSelected = ListVideo.Where(x => x.IsSelected == true && x.IsUploaded == false).ToList();
                if (lstvideoSelected != null && lstvideoSelected.Count > 0)
                {
                    foreach (var item in lstvideoSelected)
                    {
                        //nếu end-start nhỏ hơn 60s thì phải thêm s cho nó đủ 60s
                        var totals = item.EndTime.Subtract(item.StartTime).TotalSeconds;
                        if (item.EndTime.Subtract(item.StartTime).TotalSeconds < 60)
                        {
                            item.EndTime = item.EndTime.AddSeconds(60 - totals + 1);
                        }
                        if (StaticSettings.ListVideoUpload != null)
                        {
                            var video = new VideoUpload()
                            {
                                Channel = VideoRestreamInfo.Channel,
                                StartTime = item.StartTime,
                                EndTime = item.EndTime,
                                FileName = item.FileName,
                                Status = VideoUploadStatus.WaitingUpload,
                                VehicleName = VideoRestreamInfo.VehicleName,
                                VehicleID = VideoRestreamInfo.VehicleID
                            };
                            StaticSettings.ListVideoUpload.Add(video);
                        }
                        else
                        {
                            StaticSettings.ListVideoUpload = new List<VideoUpload>()
                            {
                                new VideoUpload()
                                {
                                    Channel=VideoRestreamInfo.Channel,
                                    StartTime=item.StartTime,
                                    EndTime=item.EndTime,
                                    FileName=item.FileName,
                                    Status=VideoUploadStatus.WaitingUpload,
                                    VehicleName=VideoRestreamInfo.VehicleName,
                                    VehicleID = VideoRestreamInfo.VehicleID
                                }
                            };
                        }
                    }
                    await PageDialog.DisplayAlertAsync("Thông báo", string.Format("{0} video đang được tải về server. Quý khách vui lòng xem video đã tải trong tab tải về", lstvideoSelected.Count), "Đóng");

                    await NavigationService.GoBackAsync();

                    EventAggregator.GetEvent<UploadVideoEvent>().Publish(true);
                }
                else
                {
                    DisplayMessage.ShowMessageInfo("Chọn 1 video để tải về server");
                }
            });
        }

        private void GotoMyVideoPage(object obj)
        {
            SafeExecute(async () =>
            {
                var navigationPara = new NavigationParameters();
                navigationPara.Add(ParameterKey.GotoMyVideoPage, true);
                await NavigationService.GoBackAsync(navigationPara, useModalNavigation: true, true);
            });
        }
    }
}