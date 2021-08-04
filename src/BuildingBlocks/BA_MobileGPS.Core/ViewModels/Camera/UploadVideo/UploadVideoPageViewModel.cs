using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Enums;
using BA_MobileGPS.Service;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
                    ConvertVideo(result);
                }
            }, showLoading: true);
        }

        private void ConvertVideo(VideoRestreamInfo video)
        {
            TryExecute(async () =>
            {
                VideoRestreamInfo = video;
                var listvideo = await ListStatusVideo(video);
                foreach (var item in video.Data)
                {
                    if (item.IsUploaded)
                    {
                        item.Status = VideoUploadStatus.Uploaded;
                    }
                    else
                    {
                        if (listvideo != null && listvideo.Count > 0)
                        {
                            var model = listvideo.FirstOrDefault(x => x.Time.Equals(item.StartTime));
                            if (model != null)
                            {
                                item.Status = (VideoUploadStatus)model.State;
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
                }
                var ischeckall = video.Data.FirstOrDefault(x => x.IsSelected == true
                && x.IsUploaded == false
                && (x.Status == VideoUploadStatus.NotUpload
                || x.Status == VideoUploadStatus.UploadErrorCancel)
                || x.Status == VideoUploadStatus.UploadErrorDevice
                || x.Status == VideoUploadStatus.UploadErrorTimeout);
                if (ischeckall != null)
                {
                    IsShowBtnUpload = true;
                }
                else
                {
                    IsShowBtnUpload = false;
                }
                ListVideo = video.Data.OrderBy(x => x.StartTime).ToObservableCollection();
            });
        }

        private async Task<List<UploadFiles>> ListStatusVideo(VideoRestreamInfo video)
        {
            List<UploadFiles> result = new List<UploadFiles>();
            try
            {
                var respone = await streamCameraService.GetUploadingProgressInfor(new UploadStatusRequest()
                {
                    CustomerID = UserInfo.XNCode,
                    VehicleName = new List<string>() { video.VehicleName },
                    Source = (int)CameraSourceType.App,
                    User = UserInfo.UserName,
                });
                if (respone != null && respone.Count > 0)
                {
                    var videoupload = respone.FirstOrDefault(x => x.Channel == RequestInfo.Channel);
                    if (videoupload != null)
                    {
                        result = videoupload.UploadedFiles;
                    }
                }
            }
            catch (System.Exception)
            {
                return result;
            }
            return result;
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
                        var video = new UploadStartRequest()
                        {
                            Channel = VideoRestreamInfo.Channel,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            VehicleName = VideoRestreamInfo.VehicleName,
                            CustomerID = UserInfo.XNCode,
                            Source = (int)CameraSourceType.App,
                            User = UserInfo.UserName,
                        };
                        UploadVideoRestream(video);
                    }
                    await PageDialog.DisplayAlertAsync(MobileResource.Camera_Alert_Title, string.Format("{0} {1}", lstvideoSelected.Count, MobileResource.UploadVideoTo_Alert_NoteUploaded), MobileResource.Common_Label_Close);

                    await NavigationService.GoBackAsync();

                    EventAggregator.GetEvent<UploadVideoEvent>().Publish(true);
                }
                else
                {
                    DisplayMessage.ShowMessageInfo("Chọn 1 video để tải về server");
                }
            });
        }

        private void UploadVideoRestream(UploadStartRequest arg)
        {
            RunOnBackground(async () =>
            {
                return await streamCameraService.UploadToServerStart(arg);
            }, (result) =>
            {
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