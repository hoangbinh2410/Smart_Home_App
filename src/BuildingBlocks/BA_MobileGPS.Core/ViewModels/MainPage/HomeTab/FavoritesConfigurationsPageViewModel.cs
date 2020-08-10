using AutoMapper;

using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Core.ViewModels
{
    public class FavoritesConfigurationsPageViewModel : ViewModelBase
    {
        private readonly IDisplayMessage _displayMessage;
        private readonly IHomeService _homeService;
        private readonly IMapper _mapper;
        public DelegateCommand<object> TapMenuCommand { get; set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand SearchBarCommand { get; private set; }
        public DelegateCommand<object> NavigateCommand { get; private set; }
        public DelegateCommand InitMenuCommand { get; private set; }

        public FavoritesConfigurationsPageViewModel(INavigationService navigationService,
            IMapper mapper,
            IHomeService homeService, IDisplayMessage displayMessage)
            : base(navigationService)
        {
            this._displayMessage = displayMessage;
            this._homeService = homeService;
            this._mapper = mapper;

            //TapMenuCommand = new DelegateCommand<object>(OnTappedMenuAsync);
            SearchBarCommand = new DelegateCommand(SearchBarExecute);
            SaveCommand = new DelegateCommand(async () => await SaveExecuteAsync());

            InitMenuCommand = new DelegateCommand(InitMenuItems);

            InitMenuCommand.Execute();
        }

        private async Task SaveExecuteAsync()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            try
            {
                var listMenu = FavoriteMenuItems.Count > 0 ? string.Join(",", FavoriteMenuItems.ToList().Select(x => x.PK_MenuItemID)) : "";

                using (new HUDService(MobileResource.Common_Message_Processing))
                {
                    var req = new MenuConfigRequest()
                    {
                        FK_UserID = UserInfo.UserId,
                        NameConfig = MenuConfig.MenuFavorite,
                        ListMenus = listMenu
                    };

                    var isSuccess = await _homeService.SaveConfigMenuAsync(req);

                    if (isSuccess)
                    {
                        var mbSetting = StaticSettings.User.MobileUserSetting;

                        bool hasFavorite = false;

                        foreach (var item in mbSetting)
                        {
                            if (item.Name.Equals(MenuConfig.MenuFavorite))
                            {
                                item.Value = listMenu;
                                hasFavorite = true;
                            }
                        }

                        if (!hasFavorite)
                        {
                            mbSetting.Add(new MobileUserSetting
                            {
                                Name = req.NameConfig,
                                Value = req.ListMenus
                            });
                        }

                        _displayMessage.ShowMessageSuccess(MobileResource.Favorites_Message_SaveSuccess);

                        var para = new NavigationParameters();
                        para.Add("IsFavoriteChange", true);

                        await NavigationService.GoBackAsync(para);
                    }
                    else
                    {
                        _displayMessage.ShowMessageError(MobileResource.Favorites_Message_SaveFail);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private static List<HomeMenuItemViewModel> listMenu = new List<HomeMenuItemViewModel>();

        private void InitMenuItems()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            try
            {
                IsAllowSave = false;
                GenMenu();
            }
            catch (System.Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void GenMenu()
        {
            listMenu = _mapper.Map<List<HomeMenuItemViewModel>>(StaticSettings.ListMenu);

            FavoriteMenuItems = new ObservableCollection<HomeMenuItemViewModel>(listMenu.FindAll(m => m.IsFavorited == true));
            MenuItems = new ObservableCollection<HomeMenuItemViewModel>(listMenu);
        }

        public bool isAllowSave = true;

        public bool IsAllowSave
        { get => isAllowSave; set => SetProperty(ref isAllowSave, value, nameof(IsAllowSave)); }

        #region property

        private ObservableCollection<HomeMenuItemViewModel> menuItems = new ObservableCollection<HomeMenuItemViewModel>();

        public ObservableCollection<HomeMenuItemViewModel> MenuItems
        {
            get
            {
                return menuItems;
            }
            set
            {
                SetProperty(ref menuItems, value);
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<HomeMenuItemViewModel> favoriteMenuItems = new ObservableCollection<HomeMenuItemViewModel>();

        public ObservableCollection<HomeMenuItemViewModel> FavoriteMenuItems
        {
            get
            {
                return favoriteMenuItems;
            }
            set
            {
                SetProperty(ref favoriteMenuItems, value);
                RaisePropertyChanged();
            }
        }

        public string searchedText;

        public string SearchedText
        {
            get { return searchedText; }
            set
            {
                SetProperty(ref searchedText, value);
                RaisePropertyChanged();
            }
        }

        #endregion property

        public void SearchBarExecute()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            var searchText = string.IsNullOrWhiteSpace(SearchedText) ? "" : SearchedText.TrimStart().TrimEnd();

            var listResult = listMenu.FindAll(v =>
            v.NameByCulture.ToUpper().Contains(searchText.ToUpper()));

            MenuItems = new ObservableCollection<HomeMenuItemViewModel>(listResult);

            IsNotFound = MenuItems.Count > 0 ? false : true;

            IsBusy = false;
        }

        /// <summary>
        /// Lấy menu với nhóm ban đầu
        /// phục vụ cho việc bỏ check ưa thích.
        /// </summary>
        /// <returns></returns>
        /// Name     Date         Comments
        /// TruongPV  3/14/2019   created
        /// </Modified>
        public List<HomeMenuItemViewModel> GetOriginMenu()
        {
            return _mapper.Map<List<HomeMenuItemViewModel>>(StaticSettings.ListMenuOriginGroup);
        }

        /// <summary>
        /// Lấy nhóm ban đầu khi chưa vào ưa thích
        /// </summary>
        /// <param name="menuID">The menu identifier.</param>
        /// <returns></returns>
        /// Name     Date         Comments
        /// TruongPV  3/14/2019   created
        /// </Modified>
        public string GetOriginGroupName(int menuID)
        {
            var tmpMenu = GetOriginMenu().FirstOrDefault(m => m.PK_MenuItemID == menuID);
            return tmpMenu.GroupName;
        }

        public void ReSort()
        {
            var memus = (from m in MenuItems.ToList()
                         orderby m.IsFavorited descending,
                         m.SortOrder ascending
                         select m).ToList();

            MenuItems = new ObservableCollection<HomeMenuItemViewModel>(memus);
        }
    }
}