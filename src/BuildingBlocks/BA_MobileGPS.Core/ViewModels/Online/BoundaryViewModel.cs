using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class BoundaryViewModel : ViewModelBase
    {
        private readonly IUserLandmarkGroupService userLandmarkGroupService;

        public List<UserLandmarkGroupRespone> ListLandmark { get; set; }

        private ObservableCollection<UserLandmarkGroupRespone> listLandmarkGroup = new ObservableCollection<UserLandmarkGroupRespone>();
        public ObservableCollection<UserLandmarkGroupRespone> ListLandmarkGroup { get => listLandmarkGroup; set => SetProperty(ref listLandmarkGroup, value); }

        private ObservableCollection<UserLandmarkGroupRespone> listLandmarkCategory = new ObservableCollection<UserLandmarkGroupRespone>();
        public ObservableCollection<UserLandmarkGroupRespone> ListLandmarkCategory { get => listLandmarkCategory; set => SetProperty(ref listLandmarkCategory, value); }

        public ICommand UpdateCommand { get; private set; }

        private bool isListGroup = true;
        public bool IsListGroup { get => isListGroup; set => SetProperty(ref isListGroup, value); }

        private int numberRow = 1;
        public int NumberRow { get => numberRow; set => SetProperty(ref numberRow, value); }

        public BoundaryViewModel(INavigationService navigationService, IUserLandmarkGroupService userLandmarkGroupService) : base(navigationService)
        {
            this.userLandmarkGroupService = userLandmarkGroupService;

            Title = "Cấu hình hiển thị điểm";

            UpdateCommand = new Command(Update);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetListGroup();
            GetListCategory();
        }

        /// <summary>Lấy danh sách nhóm điểm công ty</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/19/2020   created
        /// </Modified>
        private void GetListGroup()
        {
            TryExecute(async () =>
            {
                var list = await userLandmarkGroupService.GetDataGroupByUserId(StaticSettings.User.UserId);
                if (list != null && list.Count > 0)
                {
                    ListLandmarkGroup = new ObservableCollection<UserLandmarkGroupRespone>(list);
                    foreach (var item in ListLandmarkGroup)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                }
                else
                {
                    ListLandmarkGroup = new ObservableCollection<UserLandmarkGroupRespone>();
                    IsListGroup = false;
                    NumberRow = 0;
                }
            });
        }

        /// <summary>Lấy danh sách nhóm điểm hệ thống</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/19/2020   created
        /// </Modified>
        private void GetListCategory()
        {
            TryExecute(async () =>
            {
                var list = await userLandmarkGroupService.GetDataCategoryByUserId(StaticSettings.User.UserId);
                if (list != null && list.Count > 0)
                {
                    ListLandmarkCategory = new ObservableCollection<UserLandmarkGroupRespone>(list);
                    foreach (var item in ListLandmarkCategory)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                }
                else
                {
                    ListLandmarkCategory = new ObservableCollection<UserLandmarkGroupRespone>();
                }
            });
        }

        /// <summary>Kiểm tra xem có thay đổi trạng thái cũ không</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/19/2020   created
        /// </Modified>
        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(UserLandmarkGroupRespone.IsVisible)))
            {
                (sender as UserLandmarkGroupRespone).IsChanged = true;
            }
            else if (e.PropertyName.Equals(nameof(UserLandmarkGroupRespone.IsDisplayBound)))
            {
                (sender as UserLandmarkGroupRespone).IsChanged = true;
            }
            else if (e.PropertyName.Equals(nameof(UserLandmarkGroupRespone.IsDisplayName)))
            {
                (sender as UserLandmarkGroupRespone).IsChanged = true;
            }
        }

        /// <summary>Cập nhật và gửi thông tin điểm sang trang online</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/19/2020   created
        /// </Modified>
        private void Update()
        {
            TryExecute(async () =>
            {
                ListLandmark = new List<UserLandmarkGroupRespone>();
                foreach (var item in ListLandmarkGroup)
                {
                    if (item.IsChanged)
                    {
                        var input = new UserLandmarkGroupRequest()
                        {
                            FK_UserID = StaticSettings.User.UserId,
                            FK_LandmarkGroupID = item.PK_LandmarksGroupID,
                            IsSystem = false,
                            IsDisplayBound = item.IsDisplayBound,
                            IsDisplayName = item.IsDisplayName,
                            IsVisible = item.IsVisible
                        };
                        var result = await userLandmarkGroupService.SendUpdate(input);
                        if (result != null && result.Success && result.Data)
                        {
                        }
                    }
                    // Điểm nào tích mới thêm thông tin gửi trang online
                    if (item.IsVisible)
                    {
                        ListLandmark.Add(new UserLandmarkGroupRespone()
                        {
                            PK_LandmarksGroupID = item.PK_LandmarksGroupID,
                            IsSystem = false,
                            IsDisplayBound = item.IsDisplayBound,
                            IsDisplayName = item.IsDisplayName,
                            IsVisible = item.IsVisible
                        });
                    }
                }
                foreach (var item in ListLandmarkCategory)
                {
                    if (item.IsChanged)
                    {
                        var input = new UserLandmarkGroupRequest()
                        {
                            FK_UserID = StaticSettings.User.UserId,
                            FK_LandmarkGroupID = item.PK_LandmarksGroupID,
                            IsSystem = true,
                            IsDisplayBound = item.IsDisplayBound,
                            IsDisplayName = item.IsDisplayName,
                            IsVisible = item.IsVisible
                        };
                        var result = await userLandmarkGroupService.SendUpdate(input);
                        if (result != null && result.Success && result.Data)
                        {
                        }
                    }
                    // Điểm nào tích mới thêm thông tin gửi trang online
                    if (item.IsVisible)
                    {
                        ListLandmark.Add(new UserLandmarkGroupRespone()
                        {
                            PK_LandmarksGroupID = item.PK_LandmarksGroupID,
                            IsSystem = true,
                            IsDisplayBound = item.IsDisplayBound,
                            IsDisplayName = item.IsDisplayName,
                            IsVisible = item.IsVisible
                        });
                    }
                }

                await NavigationService.GoBackAsync(parameters: new NavigationParameters
                {
                    { ParameterKey.Landmark, ListLandmark }
                });
            });
        }
    }
}