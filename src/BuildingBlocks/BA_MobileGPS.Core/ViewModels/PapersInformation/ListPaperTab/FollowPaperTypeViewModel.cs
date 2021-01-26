using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class FollowPaperTypeViewModel : ViewModelBase
    {
        private int pageIndex { get; set; } = 0;
        private int pageCount { get; } = 10;
        private readonly IPapersInforService paperinforService;
        public ICommand SelectPaperCommand { get; }
        public ICommand LoadMoreItemsCommand { get; }
        public ICommand GotoAddPaperPageCommand { get; } //=>>>>>>>
        public ICommand SelectPaperTypeCommand { get; }
        public ICommand SelectAlertTypeCommand { get;  }
        public FollowPaperTypeViewModel(INavigationService navigationService,IPapersInforService papersInforService) : base(navigationService)
        {
            this.paperinforService = papersInforService;
            SelectPaperCommand = new DelegateCommand<object>(SelectPaper);          
            LoadMoreItemsCommand = new DelegateCommand(LoadMoreItems, CanLoadMoreItems);
            GotoAddPaperPageCommand = new DelegateCommand(GotoAddPaperPage);
            SelectPaperTypeCommand = new DelegateCommand(SelectPaperType);
            SelectAlertTypeCommand = new DelegateCommand(SelectAlertType);
            ListPapersDisplay = new ObservableCollection<PaperItemInfor>();
            AllPapers = new List<PaperItemInfor>(); 

        }

      

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetAllPaperData();
            CheckUserPermission();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters?.GetValue<bool>("RefreshData") is bool refresh)
            {
                if (refresh)
                {
                    GetAllPaperData();
                }
            }
        }
        #region property
        private bool insertVisible;
        public bool InsertVisible
        {
            get { return insertVisible; }
            set { SetProperty(ref insertVisible, value); }
        }


        private List<PaperItemInfor> allPapers;
        public List<PaperItemInfor> AllPapers
        {
            get { return allPapers; }
            set
            {
                SetProperty(ref allPapers, value);
                if (ViewHasAppeared)
                {
                    SourceChange();
                }
            }
        }

        private void SourceChange()
        {
            ListPapersDisplay.Clear();
            pageIndex = 0;
            LoadMore();
        }

        private ObservableCollection<PaperItemInfor> listPapersDisplay;
        public ObservableCollection<PaperItemInfor> ListPapersDisplay
        {
            get { return listPapersDisplay; }
            set
            {
                SetProperty(ref listPapersDisplay, value);
                RaisePropertyChanged();
            }
        }
        private bool listViewBusy;
        public bool ListViewBusy
        {
            get { return listViewBusy; }
            set { SetProperty(ref listViewBusy, value); }
        }

        public string searchedText;
        public string SearchedText { get => searchedText; set => SetProperty(ref searchedText, value); }
        #endregion


        private void GetAllPaperData()
        {
            allPapers.Clear();
            if (!IsBusy)
            {
                IsBusy = true;
            }
            if (ListPapersDisplay.Count > 0)
            {
                ListPapersDisplay.Clear();
            }
            RunOnBackground(async () =>
            {
                return await paperinforService.GetListPaper(UserInfo.CompanyId);
            }, result =>
            {
                if (result != null && result.Count > 0)
                {                 
                    AllPapers = result.Where(x=>!string.IsNullOrEmpty(x.VehiclePlate)).OrderBy(x=>x.VehiclePlate).ToList();
                }
                IsBusy = false;
            });
        }

        private bool CanLoadMoreItems()
        {
            if (AllPapers.Count <= pageIndex * pageCount)
                return false;
            return true;
        }

        private void LoadMoreItems()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            try
            {
                LoadMore();
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        private void LoadMore()
        {
            try
            {
                var source = allPapers.Skip(pageIndex * pageCount).Take(pageCount).ToList();
                pageIndex++;
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (source != null && source.Count() > 0)
                    {
                        for (int i = 0; i < source.Count; i++)
                        {
                            ListPapersDisplay.Add(source[i]);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        

        private void SelectPaper(object obj)
        {
            if (obj != null && obj is Syncfusion.ListView.XForms.ItemTappedEventArgs agrs)
            {
                //var item = (PaperItemInfor)agrs.ItemData;
                //var param = new NavigationParameters();
                //param.Add("paperInfor", item);
                //SafeExecute(async () =>
                //{
                //    var a = await NavigationService.NavigateAsync("NavigationPage/DetailAndEditPaperPage", param, true, true);
                //});

            }
        }


        private void GotoAddPaperPage()
        {
            SafeExecute(async () =>
            {
                var a = await NavigationService.NavigateAsync("NavigationPage/AddPaperInfoPage", null, true, true);
            });
        }

        private void CheckUserPermission()
        {
            //var userPer = UserInfo.Permissions.Distinct();
            //var insertPer = (int)PermissionKeyNames.AdminEmployeeAdd;
            //// var updatePer = (int)PermissionKeyNames.AdminEmployeeUpdate;
            //var deletePer = (int)PermissionKeyNames.AdminEmployeeDelete;
            //if (userPer.Contains(insertPer))
            //{
            //    InsertVisible = true;
            //}
        }

        private void SelectAlertType()
        {
            SafeExecute(async() =>{

                var a = await NavigationService.NavigateAsync("NavigationPage/SelectAlertTypePage",null,true,true);
            });
        }

        private void SelectPaperType()
        {
            SafeExecute(async () => {

                var a = await NavigationService.NavigateAsync("NavigationPage/SelectPaperTypePage",null,true,true);
            });
        }
    }
}
