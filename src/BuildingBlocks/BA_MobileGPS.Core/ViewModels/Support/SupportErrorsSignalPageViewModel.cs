using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    internal class SupportErrorsSignalPageViewModel : ViewModelBase
    {
        #region Property

        private int _selectedIndex =0;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        private ObservableCollection<RotatorModel> _pageCollection = new ObservableCollection<RotatorModel>();

        public ObservableCollection<RotatorModel> PageCollection
        {
            get { return _pageCollection; }
            set { _pageCollection = value; }
        }

        #endregion Property

        #region Contructor

        public ICommand BackPageCommand { get; private set; }

        public SupportErrorsSignalPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Xamarin.Forms.MessagingCenter.Subscribe<RotatorModel>(this, "NavigationPage", (sender) =>
            {
                switch(SelectedIndex)
                {
                    case 0:
                        SelectedIndex = 1;
                        break;
                    case 1:
                        SelectedIndex = 2;
                        break;
                    case 2:
                        SelectedIndex = 0;
                        SafeExecute(async () =>
                        {
                            await NavigationService.NavigateAsync("FeedbackErrorsSignalPage");
                        });
                        break;
                    default:
                        SelectedIndex = 0;
                        break;
                }
            });
            BackPageCommand = new DelegateCommand(BackPageClicked);
            PageCollection.Add(new RotatorModel(navigationService,"1", "Xe đang sửa chữa, ngắt mát ?", "Có", "Không", "Bước 1: Qúy khách vui lòng bật máy và chờ 10 phút.", "Bước 2: Quay lại trang giám sát hoặc danh sách phương tiện và kiểm tra tín hiệu xe.", "Nếu vẫn mất tín hiệu quý khách rút nguồn và cắm lại."));
            PageCollection.Add(new RotatorModel(navigationService, "2", "Xe đang dưới hầm ?", "Có", "Không", "Bước 1: Qúy khách vui lòng di chuyển xe khỏi hầm đến nơi thoáng.", "Bước 2: Quay lại trang giám sát hoặc danh sách phương tiện và kiểm tra tín hiệu xe.", "Nếu vẫn mất tín hiệu quý khách rút nguồn và cắm lại."));
            PageCollection.Add(new RotatorModel(navigationService, "3", "Qúy khách đã thực hiện rút nguồn và cắm lại ?", "Chưa thực hiện", "Đã thực hiện", "Bước 1: Qúy khách vui lòng rút nguồn thiết bị trên xe, cắm lại và chờ 5 phút.", "Bước 2: Quay lại trang giám sát hoặc danh sách phương tiện và kiểm tra tín hiệu xe.",""));
        }

        #endregion Contructor

        #region PrivateMethod

        private void BackPageClicked()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("/SupportClientPage");
            });
        }

        #endregion PrivateMethod

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnDestroy()
        {
        }

        #endregion Lifecycle
    }
}