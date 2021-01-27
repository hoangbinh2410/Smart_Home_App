using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Enums;
using BA_MobileGPS.Utilities.Extensions;
using Prism.Commands;
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
        public ICommand SelectPaperTypeCommand { get; }
        public ICommand SelectAlertTypeCommand { get; }
        private List<PaperCategory> paperCat { get; set; }

        public FollowPaperTypeViewModel(INavigationService navigationService, IPapersInforService papersInforService) : base(navigationService)
        {
            this.paperinforService = papersInforService;
            SelectPaperCommand = new DelegateCommand<object>(SelectPaper);
            LoadMoreItemsCommand = new DelegateCommand(LoadMoreItems, CanLoadMoreItems);

            SelectPaperTypeCommand = new DelegateCommand(SelectPaperType);
            SelectAlertTypeCommand = new DelegateCommand(SelectAlertType);
            ListPapersDisplay = new ObservableCollection<PaperItemInfor>();
            AllPapers = new List<PaperItemInfor>();
            originSource = new List<PaperItemInfor>();

        }



        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetAllPaperCat();
            GetAllPaperData();
            PaperTypeName = PaperCategoryTypeEnum.None.ToDescription();
            AlertTypeName = PaperAlertTypeEnum.All.ToDescription();

        }

        private Guid filterTypeId { get; set; } = new Guid();
        private PaperAlertTypeEnum filterTypeAlert { get; set; } = PaperAlertTypeEnum.All;
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.PaperType) && parameters.GetValue<PaperCategory>(ParameterKey.PaperType) is PaperCategory paper)
            {
                if (PaperTypeName != ((PaperCategoryTypeEnum)paper.PaperCategoryType).ToDescription())
                {
                    PaperTypeName = ((PaperCategoryTypeEnum)paper.PaperCategoryType).ToDescription();
                    filterTypeId = paper.Id;
                    Filter();
                }
            }
            else if (parameters.ContainsKey("AlertType") && parameters.GetValue<PaperAlertTypeEnum>("AlertType") is PaperAlertTypeEnum alertType)
            {
                if (filterTypeAlert != alertType)
                {
                    filterTypeAlert = alertType;
                    AlertTypeName = alertType.ToDescription();
                    Filter();
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

        private string paperTypeName;
        public string PaperTypeName
        {
            get { return paperTypeName; }
            set { SetProperty(ref paperTypeName, value); }
        }

        private string alertTypeName;
        public string AlertTypeName
        {
            get { return alertTypeName; }
            set { SetProperty(ref alertTypeName, value); }
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

        #endregion

        private List<PaperItemInfor> originSource { get; set; }

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
                    originSource = result.Where(x => !string.IsNullOrEmpty(x.VehiclePlate)).OrderBy(x => x.VehiclePlate).ToList();
                    AllPapers = originSource;
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
                SafeExecute(async () =>
                {
                    var item = (PaperItemInfor)agrs.ItemData;
                    var temp = StaticSettings.ListVehilceOnline.FirstOrDefault(x => x.VehicleId == item.FK_VehicleID);
                    var vehicle = new Vehicle()
                    {
                        PrivateCode = temp.PrivateCode
                    };

                    var paperType = paperCat.FirstOrDefault(x => x.Id == item.FK_PaperCategoryID);

                    var param = new NavigationParameters();
                    param.Add(ParameterKey.PaperType, paperType);
                    param.Add(ParameterKey.Vehicle, vehicle);

                    var a = await NavigationService.NavigateAsync("NavigationPage/InvalidPapersPage", param, true, true);
                });

            }
        }

        private void SelectAlertType()
        {
            SafeExecute(async () =>
            {
                var a = await NavigationService.NavigateAsync("NavigationPage/SelectAlertTypePage", null, true, true);
            });
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

        private void GetAllPaperCat()
        {
            RunOnBackground(async () =>
            {
                paperCat = await paperinforService.GetPaperCategories();
            });
        }

        private void Filter()
        {
            SafeExecute(() =>
            {
                var temp = originSource.Where(x => filterTypeId == new Guid() || x.FK_PaperCategoryID == filterTypeId)
                                       .Where(s =>
                                       {
                                           var day = (s.ExpireDate - new TimeSpan(CompanyConfigurationHelper.DayAllowRegister, 0, 0, 0)).Date;
                                           switch (filterTypeAlert)
                                           {
                                               case PaperAlertTypeEnum.All:
                                                   return true;

                                               case PaperAlertTypeEnum.UndueAlert:

                                                   if (DateTime.Now.Date < day)
                                                   {
                                                       return true;
                                                   }
                                                   return false;

                                               case PaperAlertTypeEnum.DueAlert:

                                                   if (s.ExpireDate.Date > DateTime.Now.Date && DateTime.Now.Date >= day)
                                                   {
                                                       return true;
                                                   }
                                                   return false;

                                               case PaperAlertTypeEnum.ExpireAlert:
                                                   if (s.ExpireDate.Date <= DateTime.Now.Date)
                                                       return true;
                                                   return false;

                                               default:
                                                   return false;
                                           }
                                       }).ToList();
                AllPapers = temp;
            });
        }
    }
}
