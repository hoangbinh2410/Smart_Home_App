using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Service.IService;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class SelectPaperTypePageViewModel : ViewModelBase
    {
        private CancellationTokenSource cts;
        private readonly IPapersInforService papersInforService;
        public ICommand SearchPapersTypeCommand { get; }
        public ICommand SelectPaperCommand { get; }
        public SelectPaperTypePageViewModel(INavigationService navigationService, IPapersInforService papersInforService) : base(navigationService)
        {
            this.papersInforService = papersInforService;
            SearchPapersTypeCommand = new DelegateCommand<TextChangedEventArgs>(SearchPapersType);
            SelectPaperCommand = new DelegateCommand<object>(SelectPaper);
            hiddenSearch = true;
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.TryGetValue(ParameterKey.PaperType, out bool visibleSearch))
            {
                HiddenSearch = visibleSearch;
            }
            GetListPapers();
        }

        private void GetListPapers()
        {
            SafeExecute(async() =>
            {
                ListPapersOrigin = await papersInforService.GetPaperCategories();
                if (hiddenSearch)
                {
                    var temp = new PaperCategory() { PaperName = "Tất cả giấy tờ", PaperCategoryType = 0 };
                    ListPapersOrigin.Insert(0, temp);
                }
                ListPapersDisplay = ListPapersOrigin;
            });
        }   


        private void SelectPaper(object obj)
        {
            if (obj != null && obj is Syncfusion.ListView.XForms.ItemTappedEventArgs agrs)
            {
                var item = (PaperCategory)agrs.ItemData;
                var param = new NavigationParameters();
                param.Add(ParameterKey.PaperType, item);
                SafeExecute(async () =>
                {
                    var a = await NavigationService.GoBackAsync(param, true, true);
                });
            }
        }

        private void SearchPapersType(TextChangedEventArgs args)
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
                    return ListPapersOrigin;
                }
                var temp = ListPapersOrigin.FindAll(v => v.PaperName.ToUpper().Contains(SearchedText.ToUpper()));
                return temp;
            }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    ListPapersDisplay = task.Result;
                }
            }));
        }

        private string searchedText;
        public string SearchedText
        {
            get { return searchedText; }
            set { SetProperty(ref searchedText, value); }
        }

        private bool hiddenSearch;
        public bool HiddenSearch
        {
            get { return hiddenSearch; }
            set { SetProperty(ref hiddenSearch, value); }
        }

        private List<PaperCategory> ListPapersOrigin { get; set; } = new List<PaperCategory>();

        private List<PaperCategory> listPapersDisplay;
        public List<PaperCategory> ListPapersDisplay
        {
            get { return listPapersDisplay; }
            set
            {
                SetProperty(ref listPapersDisplay, value);
            }
        }


    }

    

}
