using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
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
using Xamarin.Forms.Extensions;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ComboboxPageViewModel : ViewModelBase
    {
        public ComboboxPageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                base.OnNavigatedTo(parameters);

                if (parameters.ContainsKey("dataCombobox") && parameters.GetValue<List<ComboboxRequest>>("dataCombobox") is List<ComboboxRequest> tempData)
                {
                    DataFull = tempData;

                    DataSearch = DataFull.ToObservableCollection();

                    ComboboxType = parameters.GetValue<short>("ComboboxType");
                    Title = parameters.GetValue<string>("Title");
                }
            }
            catch (Exception)
            {
            }
        }

        public List<ComboboxRequest> DataFull = new List<ComboboxRequest>();

        private ObservableCollection<ComboboxRequest> _dataSearch;

        public ObservableCollection<ComboboxRequest> DataSearch
        {
            get
            {
                return _dataSearch;
            }
            set
            {
                SetProperty(ref _dataSearch, value);
            }
        }

        private string _searchText;

        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                SetProperty(ref _searchText, value);
            }
        }
        private CancellationTokenSource cts;
        public short ComboboxType { get; set; }

        public ICommand SearchValueCommmand
        {
            get
            {
                return new Command<TextChangedEventArgs>((arg) =>
                {
                    SearchValue(arg);
                });
            }
        }

        private void SearchValue(TextChangedEventArgs args)
        {
            if (cts != null)
                cts.Cancel(true);

            cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                await Task.Delay(500, cts.Token);

                if (string.IsNullOrWhiteSpace(args.NewTextValue))
                {
                    return DataFull.ToList();
                }
                else
                {
                    return DataFull.FindAll(vg => vg.Value != null && vg.Value.UnSignContains(args.NewTextValue));
                }
            }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    var result = task.Result;
                    DataSearch = result.ToObservableCollection();
                }
            }));
        }

        public Command SelectedValueCommand
        {
            get
            {
                return new Command<ItemTappedEventArgs>(async (args) =>
                {
                    if (!(args.ItemData is ComboboxRequest item))
                        return;
                    if (item != null)
                    {
                        try
                        {
                            var input = new ComboboxResponse()
                            {
                                Key = item.Key,
                                Keys = item.Keys,
                                Value = item.Value,
                                ComboboxType = ComboboxType
                            };
                            EventAggregator.GetEvent<SelectComboboxEvent>().Publish(input);
                            await NavigationService.GoBackAsync(null, useModalNavigation: true, true);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
                        }
                    }
                });
            }
        }
    }
}