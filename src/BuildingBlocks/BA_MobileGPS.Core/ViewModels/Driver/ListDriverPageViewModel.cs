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
        public ListDriverPageViewModel(INavigationService navigationService, IDriverInforService driverInforService
            ) : base(navigationService)
        {
            this.driverInforService = driverInforService;
            SelectDriverCommand = new DelegateCommand<object>(SelectDriver);
            DeleteDriverCommand = new DelegateCommand<object>(DeleteDriver);
            SearchDriverCommand = new DelegateCommand<TextChangedEventArgs>(SearchDriver);
            LoadMoreItemsCommand = new DelegateCommand(LoadMoreItems, CanLoadMoreItems);
            ListDriver = new ObservableCollection<DriverInfor>();
        }

        public override void OnPageAppearingFirstTime()
        {
            GetAllDriverData();
            base.OnPageAppearingFirstTime();          
        }

        private void GetAllDriverData()
        {
            ListDriverOrigin.Clear();
            if (!IsBusy)
            {
                IsBusy = true;
            }
            if (ListDriver.Count > 0)
            {
                ListDriver.Clear();
            }
            RunOnBackground(async () =>
            {
                return await driverInforService.GetListDriverByCompanyId(UserInfo.CompanyId);
            }, result =>
            {
                if (result != null && result.Count > 0)
                {
                    ListDriverOrigin = result;
                    LoadMore();
                }
                IsBusy = false;
            });
        }

        private bool CanLoadMoreItems()
        {
            if (ListDriverOrigin.Count <= pageIndex * pageCount)
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
                var source = ListDriverOrigin.Skip(pageIndex * pageCount).Take(pageCount).ToList();
                pageIndex++;
                if (source != null && source.Count() > 0)
                {
                    for (int i = 0; i < source.Count; i++)
                    {
                        ListDriver.Add(source[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        private void SearchDriver(TextChangedEventArgs args)
        {
            if (args.NewTextValue == null || string.IsNullOrWhiteSpace(args.NewTextValue))
                return;

            if (cts != null)
                cts.Cancel(true);
            cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                await Task.Delay(500, cts.Token);
                return ListDriverOrigin.FindAll(v => v.DisplayName.ToUpper().Contains(SearchedText.ToUpper()));
            }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    ListDriver = new ObservableCollection<DriverInfor>(task.Result);
                }
            }));
        }


        private void DeleteDriver(object obj)
        {
            if (obj != null && obj is Syncfusion.ListView.XForms.ItemTappedEventArgs agrs)
            {
                var item = (DriverInfor)agrs.ItemData;
            }
        }

        private void SelectDriver(object obj)
        {
            if (obj != null && obj is Syncfusion.ListView.XForms.ItemTappedEventArgs agrs)
            {
                var item = (DriverInfor)agrs.ItemData;
            }
        }

        private List<DriverInfor> ListDriverOrigin { get; set; } = new List<DriverInfor>();
        private ObservableCollection<DriverInfor> listDriver;
        public ObservableCollection<DriverInfor> ListDriver
        {
            get { return listDriver; }
            set { SetProperty(ref listDriver, value); }
        }
        private bool listViewBusy;
        public bool ListViewBusy
        {
            get { return listViewBusy; }
            set { SetProperty(ref listViewBusy, value); }
        }

        public string searchedText;
        public string SearchedText { get => searchedText; set => SetProperty(ref searchedText, value); }
    }


}
