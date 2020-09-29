using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Events;
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
    public class LanguagePageViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ILanguageService _languageTypeService;

        public LanguagePageViewModel(INavigationService navigationService,
            IEventAggregator eventAggregator, ILanguageService languageTypeService)
            : base(navigationService)
        {
            this._eventAggregator = eventAggregator;
            this._languageTypeService = languageTypeService;

            Title = MobileResource.Login_Lable_SelectLanguage;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            GetAllLanguage();
        }

        private ObservableCollection<LanguageRespone> _LanguageResponeCollection;

        public ObservableCollection<LanguageRespone> LanguageResponeCollection
        {
            get
            {
                return _LanguageResponeCollection;
            }
            set
            {
                _LanguageResponeCollection = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Grouping<string, LanguageRespone>> _LanguageResponeCollectionSearched;

        public ObservableCollection<Grouping<string, LanguageRespone>> LanguageResponeCollectionSearched
        {
            get
            {
                return _LanguageResponeCollectionSearched;
            }
            set
            {
                _LanguageResponeCollectionSearched = value;
                RaisePropertyChanged();
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
                _searchText = value;
                RaisePropertyChanged();
            }
        }

        public void GetAllLanguage()
        {
            try
            {
                var lst = new List<LanguageRespone>()
                {
                    new LanguageRespone()
                    {
                        CodeName = CultureCountry.Vietnamese,
                        Icon = "flag_vn.png",
                        Description = "Tiếng Việt",
                        PK_LanguageID = 1
                    },
                    new LanguageRespone()
                    {
                        CodeName = CultureCountry.English,
                        Icon = "flag_us.png",
                        Description = "English",
                        PK_LanguageID = 2
                    },
                };
                if (lst != null && lst.Count > 0)
                {
                    var groupedData =
                       lst.OrderBy(p => p.CodeName)
                           .GroupBy(p => p.NameSort)
                           .Select(p => new Grouping<string, LanguageRespone>(p))
                           .ToList();

                    LanguageResponeCollection = new ObservableCollection<LanguageRespone>(lst);
                    LanguageResponeCollectionSearched = new ObservableCollection<Grouping<string, LanguageRespone>>(groupedData);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError("GetAllLanguage", ex.Message);
            }
        }

        public Command SearchLanguageCommmand
        {
            get
            {
                return new Command<string>((arg) =>
                {
                    try
                    {
                        if (LanguageResponeCollection.Count > 0)
                        {
                            var lst = new List<LanguageRespone>();

                            if (!string.IsNullOrEmpty(SearchText))
                            {
                                string searchText = SearchText.Trim().ToUpper();
                                foreach (var item in LanguageResponeCollection)
                                {
                                    if (item.Description.Trim().ToUpper().Contains(searchText) || item.CodeName.Trim().ToUpper().Contains(searchText))
                                    {
                                        lst.Add(item);
                                    }
                                }
                            }
                            else
                            {
                                lst = new List<LanguageRespone>(LanguageResponeCollection);
                            }

                            var groupedData =
                                    lst.OrderBy(p => p.CodeName)
                                        .GroupBy(p => p.NameSort)
                                        .Select(p => new Grouping<string, LanguageRespone>(p))
                                        .ToList();

                            LanguageResponeCollectionSearched = new ObservableCollection<Grouping<string, LanguageRespone>>(groupedData);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
                    }
                });
            }
        }

        public ICommand SelectedCommand
        {
            get
            {
                return new DelegateCommand<Syncfusion.ListView.XForms.ItemTappedEventArgs>(async (obj) =>
                {
                    var item = (LanguageRespone)obj.ItemData;
                    if (item != null)
                    {
                        try
                        {
                            _eventAggregator.GetEvent<SelectLanguageTypeEvent>().Publish(item);
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