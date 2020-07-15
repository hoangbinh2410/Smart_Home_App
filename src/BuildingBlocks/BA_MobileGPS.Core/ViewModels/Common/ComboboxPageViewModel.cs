using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;

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
                    DataFull = new ObservableCollection<ComboboxRequest>(tempData);

                    //var groupedData = DataFull.OrderBy(p => p.Value).ToList();
                    //DataSearch = new ObservableCollection<ComboboxRequest>(groupedData);

                    DataSearch = DataFull;

                    ComboboxType = parameters.GetValue<short>("ComboboxType");
                    Title = parameters.GetValue<string>("Title");
                }
            }
            catch (Exception)
            {
            }
        }

        private ObservableCollection<ComboboxRequest> _dataFull;

        public ObservableCollection<ComboboxRequest> DataFull
        {
            get
            {
                return _dataFull;
            }
            set
            {
                SetProperty(ref _dataFull, value);
            }
        }

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

        public short ComboboxType { get; set; }

        public ICommand SearchValueCommmand
        {
            get
            {
                return new Command<string>((arg) =>
                {
                    if (arg != null)
                    {
                        try
                        {
                            if (DataFull.Count > 0)
                            {
                                var lst = new List<ComboboxRequest>();

                                if (!string.IsNullOrEmpty(SearchText))
                                {
                                    string searchText = SearchText.Trim().ToUpper();
                                    foreach (var item in DataFull)
                                    {
                                        if (item.Value.ToUpper().Contains(searchText))
                                        {
                                            lst.Add(item);
                                        }
                                    }
                                }
                                else
                                {
                                    lst = new List<ComboboxRequest>(DataFull);
                                }

                                var groupedData =
                                        lst.OrderBy(p => p.Value)
                                            .ToList();

                                DataSearch = new ObservableCollection<ComboboxRequest>(groupedData);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
                        }
                    }
                });
            }
        }

        public Command SelectedValueCommand
        {
            get
            {
                return new Command<ComboboxRequest>(async (item) =>
                {
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
                            await NavigationService.GoBackAsync(useModalNavigation: true);
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