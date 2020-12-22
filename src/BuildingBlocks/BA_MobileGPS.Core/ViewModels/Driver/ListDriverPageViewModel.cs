using BA_MobileGPS.Entities;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ListDriverPageViewModel : ViewModelBase
    {
        private CancellationTokenSource cts;
        public ICommand SelectDriverCommand { get; }
        public ICommand DeleteDriverCommand { get; }
        public ICommand SearchDriverCommand { get; }
        public ListDriverPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SelectDriverCommand = new DelegateCommand<object>(SelectDriver);
            DeleteDriverCommand = new DelegateCommand<object>(DeleteDriver);
            SearchDriverCommand = new DelegateCommand<TextChangedEventArgs>(SearchDriver);
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
                return ListDriverOrigin.FindAll(v => v.FullName.ToUpper().Contains(searchedText.ToUpper()));
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
      

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            SetDriverSource();
        }

        private void SetDriverSource()
        {
            for (int i = 0; i < 10; i++)
            {
                ListDriver.Add(new DriverInfor());
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
