using BA_MobileGPS.Core.Constant;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class SelectPaperTypePageViewModel : ViewModelBase
    {
        private CancellationTokenSource cts;
        public ICommand SearchPapersTypeCommand { get; }
        public ICommand SelectPaperCommand { get; }
        public SelectPaperTypePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SearchPapersTypeCommand = new DelegateCommand<TextChangedEventArgs>(SearchPapersType);
            SelectPaperCommand = new DelegateCommand<object>(SelectPaper);
            ListPapersOrigin = new List<PaperModel>()
            {
                new PaperModel(){ PaperType= PaperEnum.A1},
                new PaperModel(){ PaperType= PaperEnum.B1},
                new PaperModel(){ PaperType= PaperEnum.C2}
            };
            ListPapersDisplay = ListPapersOrigin;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.PaperType) && parameters.GetValue<PaperModel>(ParameterKey.PaperType) is PaperModel vehicle)
            {
                SelectedPaper = vehicle;
            }
        }

        private void SelectPaper(object obj)
        {
            if (obj != null && obj is Syncfusion.ListView.XForms.ItemTappedEventArgs agrs)
            {
                var item = (PaperModel)agrs.ItemData;
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
                var temp = ListPapersOrigin.FindAll(v => v.Name.ToUpper().Contains(SearchedText.ToUpper()));
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

        private List<PaperModel> ListPapersOrigin { get; set; } = new List<PaperModel>();

        private List<PaperModel> listPapersDisplay;
        public List<PaperModel> ListPapersDisplay
        {
            get { return listPapersDisplay; }
            set
            {
                SetProperty(ref listPapersDisplay, value);
            }
        }

        private PaperModel selectedPaper;
        public PaperModel SelectedPaper
        {
            get { return selectedPaper; }
            set { SetProperty(ref selectedPaper, value); }
        }
    }

    public class PaperModel
    {

        public PaperEnum PaperType { get; set; }
        public string Name
        {
            get
            {
                return "Type " + PaperType.ToString();
            }
        }
    }
    public enum PaperEnum
    {
        A1, B1, C2
    }

}
