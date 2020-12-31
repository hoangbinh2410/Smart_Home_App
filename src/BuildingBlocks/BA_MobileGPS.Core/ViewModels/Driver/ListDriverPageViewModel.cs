using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ListDriverPageViewModel : ViewModelBase
    {
        private int pageIndex { get; set; } = 0;
        private int pageCount { get; } = 10;
        private CancellationTokenSource cts;
        private readonly IDriverInforService driverInforService;
        public ICommand SelectDriverCommand { get; }
        public ICommand DeleteDriverCommand { get; }
        public ICommand SearchDriverCommand { get; }
        public ICommand LoadMoreItemsCommand { get; }
        public ICommand GotoAddDriverPageCommand { get; }
        public ListDriverPageViewModel(INavigationService navigationService, IDriverInforService driverInforService
            ) : base(navigationService)
        {
            this.driverInforService = driverInforService;
            SelectDriverCommand = new DelegateCommand<object>(SelectDriver);
            DeleteDriverCommand = new DelegateCommand<object>(DeleteDriver);
            SearchDriverCommand = new DelegateCommand<TextChangedEventArgs>(SearchDriver);
            LoadMoreItemsCommand = new DelegateCommand(LoadMoreItems, CanLoadMoreItems);
            GotoAddDriverPageCommand = new DelegateCommand(GotoAddDriverPage);
            ListDriverDisplay = new ObservableCollection<DriverInfor>();
            ListDriverSearch = new List<DriverInfor>();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            GetAllDriverData();            
        }

        private void GetAllDriverData()
        {
            ListDriverOrigin.Clear();
            if (!IsBusy)
            {
                IsBusy = true;
            }
            if (ListDriverDisplay.Count > 0)
            {
                ListDriverDisplay.Clear();
            }
            RunOnBackground(async () =>
            {
                return await driverInforService.GetListDriverByCompanyId(UserInfo.CompanyId);
            }, result =>
            {
                if (result != null && result.Count > 0)
                {
                    ListDriverOrigin = result;
                    ListDriverSearch = result;
                }
                IsBusy = false;
            });
        }

        private bool CanLoadMoreItems()
        {
            if (ListDriverSearch.Count <= pageIndex * pageCount)
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
                var source = listDriverSearch.Skip(pageIndex * pageCount).Take(pageCount).ToList();
                pageIndex++;
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (source != null && source.Count() > 0)
                    {
                        for (int i = 0; i < source.Count; i++)
                        {
                            ListDriverDisplay.Add(source[i]);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void SearchDriver(TextChangedEventArgs args)
        {
            if (args.NewTextValue == null)
                return;

            if (cts != null)
                cts.Cancel(true);
            cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                await Task.Delay(500, cts.Token);

                if (string.IsNullOrWhiteSpace(args.NewTextValue))
                {
                    return ListDriverOrigin;
                }
                var temp = ListDriverOrigin.FindAll(v => v.DisplayName.ToUpper().Contains(SearchedText.ToUpper()));
                return temp;
            }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    ListDriverSearch = task.Result;
                }
            }));
        }


        private void DeleteDriver(object obj)
        {
            if (obj != null && obj is DriverInfor item)
            {
                var req = new DriverDeleteRequest()
                {
                    PK_EmployeeID = item.PK_EmployeeID,
                    UpdatedByUser = UserInfo.UserId
                };
                RunOnBackground(async () =>
                {
                    var temp = await driverInforService.DeleteDriverInfor(req);
                    return temp;
                }, result =>
                {
                    if (result == item.PK_EmployeeID)
                    {
                        GetAllDriverData();
                        SearchedText = string.Empty;
                    }
                });

            }
        }

        private void SelectDriver(object obj)
        {
            if (obj != null && obj is Syncfusion.ListView.XForms.ItemTappedEventArgs agrs)
            {
                var item = (DriverInfor)agrs.ItemData;
                var param = new NavigationParameters();
                param.Add(ParameterKey.DriverInformation, item);
                SafeExecute(async() =>
                {
                    var a = await NavigationService.NavigateAsync("NavigationPage/DetailAndEditDriverPage", param, true, true);
                });
              
            }
        }
        private List<DriverInfor> listDriverSearch;
        public List<DriverInfor> ListDriverSearch
        {
            get { return listDriverSearch; }
            set
            {
                SetProperty(ref listDriverSearch, value);
                if (ViewHasAppeared)
                {
                    SourceChange();
                }
            }
        }

        private void SourceChange()
        {
            ListDriverDisplay.Clear();
            pageIndex = 0;
            LoadMore();
        }

        private List<DriverInfor> ListDriverOrigin { get; set; } = new List<DriverInfor>();
        private ObservableCollection<DriverInfor> listDriverDisplay;
        public ObservableCollection<DriverInfor> ListDriverDisplay
        {
            get { return listDriverDisplay; }
            set
            {
                SetProperty(ref listDriverDisplay, value);
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

        private void GotoAddDriverPage()
        {
            SafeExecute(async () =>
            {
                var a = await NavigationService.NavigateAsync("NavigationPage/AddDriverInforPage", null, true, true);
            });
        }
    }


}
