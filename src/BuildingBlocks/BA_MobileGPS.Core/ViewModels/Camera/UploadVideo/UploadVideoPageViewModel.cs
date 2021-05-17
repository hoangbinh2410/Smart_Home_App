using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using Prism.Navigation;
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

        public UploadVideoPageViewModel(INavigationService navigationService, IStreamCameraService streamCameraService) : base(navigationService)
        {
            this.streamCameraService = streamCameraService;
            UploadVideoCommand = new Command(UploadVideo);
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
                    ListVideo = result.Data.OrderBy(x => x.StartTime).ToObservableCollection();
                    var ischeckall = result.Data.Where(x => x.IsSelected == true && x.IsUploaded == false).ToList();
                    if (ischeckall != null && ischeckall.Count > 0)
                    {
                        IsCheckAll = true;
                    }
                    else
                    {
                        IsCheckAll = false;
                        IsShowBtnUpload = false;
                    }
                }
            }, showLoading: true);
        }

        private void UploadVideo(object obj)
        {
            SafeExecute(async () =>
            {
                if (GlobalResources.Current.TotalVideoUpload > 0)
                {
                    DisplayMessage.ShowMessageInfo("Đang có video được tải. Bạn vui lòng đợi tải xong thì mới được tải tiêp");
                }
                else
                {
                    var lstvideoSelected = ListVideo.Where(x => x.IsSelected == true && x.IsUploaded == false).ToList();
                    if (lstvideoSelected != null && lstvideoSelected.Count > 0)
                    {
                        bool isvalid = true;
                        foreach (var item in lstvideoSelected)
                        {
                            if (!string.IsNullOrEmpty(item.Note))
                            {
                                var DangerousChar = "['\"<>/&]";
                                var dangerList = DangerousChar.ToCharArray();
                                foreach (var str in dangerList)
                                {
                                    if (item.Note.Contains(str))
                                    {
                                        isvalid = false;
                                    }
                                }
                            }
                            //nếu end-start nhỏ hơn 60s thì phải thêm s cho nó đủ 60s
                            var totals = item.EndTime.Subtract(item.StartTime).TotalSeconds;
                            if (item.EndTime.Subtract(item.StartTime).TotalSeconds < 60)
                            {
                                item.EndTime = item.EndTime.AddSeconds(60 - totals + 1);
                            }
                        }
                        if (isvalid)
                        {
                            await PageDialog.DisplayAlertAsync("Thông báo", "Video đang được tải về server. Quý khách có thể xem các video đã tải trên tab Yêu cầu", "Đóng");
                            EventAggregator.GetEvent<UploadVideoEvent>().Publish(new VideoRestreamInfo()
                            {
                                Channel = VideoRestreamInfo.Channel,
                                Data = lstvideoSelected,
                                VehicleName = VideoRestreamInfo.VehicleName,
                                VehicleID = RequestInfo.VehicleID
                            });
                            await NavigationService.GoBackAsync();
                        }
                        else
                        {
                            DisplayMessage.ShowMessageInfo("Bạn không được nhập các ký tự đặc biệt [,',\",<,>,/, &,]");
                        }
                    }
                    else
                    {
                        DisplayMessage.ShowMessageInfo("Chọn 1 video để tải về server");
                    }
                }
            });
        }
    }
}