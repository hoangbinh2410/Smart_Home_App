using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Extensions;
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
    public class PaperHistoriesPageViewModel : ViewModelBase
    {
        private CancellationTokenSource cts;
        private int pageIndex { get; set; } = 0;
        private int pageCount { get; } = 15;

        public ICommand SelectPaperTypeCommand { get; }
        public ICommand SearchVehicleCommand { get; }
        public ICommand LoadMoreItemsCommand { get; }
        public ICommand SelectFromDateCommand { get; }
        public ICommand SelectToDateCommand { get; }
        private readonly IPapersInforService paperInforService;
        public PaperHistoriesPageViewModel(INavigationService navigationService, IPapersInforService paperInforService) : base(navigationService)
        {
            this.paperInforService = paperInforService;
            SearchVehicleCommand = new DelegateCommand<TextChangedEventArgs>(SearchVehicle);
            SelectPaperTypeCommand = new DelegateCommand(SelectPaperType);
            LoadMoreItemsCommand = new DelegateCommand(LoadMoreItems, CanLoadMoreItems);
            SelectFromDateCommand = new DelegateCommand(SelectFromDate);
            SelectToDateCommand = new DelegateCommand(SelectToDate);
            listPaperDisplay = new ObservableCollection<PaperItemHistoryModel>();
            listPaperAfterSearch = new List<PaperItemHistoryModel>();
            toDate = DateTime.Now;
            fromDate = DateTime.Now.AddDays(-90);
            paperType = PaperCategoryTypeEnum.None.ToDescription();
        }



        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            RunOnBackground(async () =>
            {
                return await paperInforService.GetListPaperHistory(UserInfo.CompanyId);
            },
            res =>
            {
                listPaperOrigin = res.Where(x=> !string.IsNullOrEmpty(x.VehiclePlate)).ToList();
                Filter();
            });
        }

        private Guid paperTypeIdFilter { get; set; } = new Guid();
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.PaperType) && parameters.GetValue<PaperCategory>(ParameterKey.PaperType) is PaperCategory paper)
            {
                if (PaperTypeName != ((PaperCategoryTypeEnum)paper.PaperCategoryType).ToDescription())
                {
                    PaperTypeName = ((PaperCategoryTypeEnum)paper.PaperCategoryType).ToDescription();
                    paperTypeIdFilter = paper.Id;
                    Filter();
                }
            }
            else if (parameters.ContainsKey(ParameterKey.DateResponse)
               && parameters.GetValue<PickerDateTimeResponse>(ParameterKey.DateResponse) is PickerDateTimeResponse date)
            {
                if (date.PickerType == (int)ComboboxType.First)
                {
                    //toDate
                    ToDate = date.Value;
                    Filter();
                }
                else if (date.PickerType == (int)ComboboxType.Second)
                {
                    //fromDate
                    FromDate = date.Value;
                    Filter();
                }
            }
        }

        private bool CanLoadMoreItems()
        {
            return listPaperAfterSearch.Count > pageIndex * pageCount;
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
                var source = listPaperAfterSearch.Skip(pageIndex * pageCount).Take(pageCount).ToList();
                pageIndex++;
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (source != null && source.Count() > 0)
                    {
                        for (int i = 0; i < source.Count; i++)
                        {
                            ListPaperDisplay.Add(source[i]);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void SelectPaperType()
        {
            SafeExecute(async () =>
            {
                var param = new NavigationParameters();
                var visibleSearch = false;
                param.Add(ParameterKey.PaperType, visibleSearch);
                var a = await NavigationService.NavigateAsync("NavigationPage/SelectPaperTypePage", param, true, true);
            });
        }

        private void SearchVehicle(TextChangedEventArgs args)
        {
            if (args.NewTextValue == null)
                return;


            if (cts != null)
            {
                cts.Cancel(true);
                cts.Dispose();
                cts = new CancellationTokenSource();
            }


            Task.Run(async () =>
            {
                await Task.Delay(500, cts.Token);

                Filter();
            });
        }

        private string searchedText;
        public string SearchedText
        {
            get { return searchedText; }
            set { SetProperty(ref searchedText, value); }
        }

        private string paperType;
        public string PaperTypeName
        {
            get { return paperType; }
            set { SetProperty(ref paperType, value); }
        }

        private DateTime fromDate;
        public DateTime FromDate
        {
            get { return fromDate; }
            set { SetProperty(ref fromDate, value); }
        }

        private DateTime toDate;
        public DateTime ToDate
        {
            get { return toDate; }
            set { SetProperty(ref toDate, value); }
        }

        private ObservableCollection<PaperItemHistoryModel> listPaperDisplay;
        public ObservableCollection<PaperItemHistoryModel> ListPaperDisplay
        {
            get { return listPaperDisplay; }
            set { SetProperty(ref listPaperDisplay, value); }
        }

        private List<PaperItemHistoryModel> listPaperOrigin { get; set; } = new List<PaperItemHistoryModel>();
        private List<PaperItemHistoryModel> listPaperAfterSearch;
        public List<PaperItemHistoryModel> ListPaperAfterSearch
        {
            get { return listPaperAfterSearch; }
            set { SetProperty(ref listPaperAfterSearch, value); }
        }

        private void Filter()
        {
            ListPaperDisplay = new ObservableCollection<PaperItemHistoryModel>();
            pageIndex = 0;
            var temp = listPaperOrigin.Where(x => (string.IsNullOrWhiteSpace(searchedText) || x.VehiclePlate.Contains(searchedText))
                                     && (paperTypeIdFilter == new Guid() || x.FK_PaperCategoryID == paperTypeIdFilter)
                                     && (x.CreatedDate.Date >= fromDate)
                                     && (x.CreatedDate.Date <= toDate)).ToList();

            ListPaperAfterSearch = temp;
            LoadMore();
        }

        private void SelectToDate()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters();
                parameters.Add("PickerType", (short)ComboboxType.First);
                parameters.Add("DataPicker", toDate);
                await NavigationService.NavigateAsync("SelectDatePicker", parameters);
            });
        }

        private void SelectFromDate()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters();
                parameters.Add("PickerType", (short)ComboboxType.Second);
                parameters.Add("DataPicker", fromDate);
                await NavigationService.NavigateAsync("SelectDatePicker", parameters);
            });
        }
    }
}
