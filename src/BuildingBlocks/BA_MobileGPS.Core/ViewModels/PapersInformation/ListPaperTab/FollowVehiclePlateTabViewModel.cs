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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class FollowVehiclePlateTabViewModel : ViewModelBase
    {
        private CancellationTokenSource cts;
        private readonly IPapersInforService paperinforService;
        public ICommand SelectPaperCommand { get; }
        public ICommand LoadMoreItemsCommand { get; }
        public ICommand SearchVehicleCommand { get; }
        public ICommand SelectAlertTypeCommand { get; }
        private List<PaperCategory> paperCat { get; set; }

        public FollowVehiclePlateTabViewModel(INavigationService navigationService, IPapersInforService papersInforService) : base(navigationService)
        {
            this.paperinforService = papersInforService;
            SelectPaperCommand = new DelegateCommand<object>(SelectPaper);

            SearchVehicleCommand = new DelegateCommand<TextChangedEventArgs>(SearchVehicle);
            SelectAlertTypeCommand = new DelegateCommand(SelectAlertType);
            ListPapersDisplay = new ObservableCollection<PaperItemInfor>();
            allPapers = new List<PaperItemInfor>();
            originSource = new List<PaperItemInfor>();
            GetAllPaperCat();
            AlertTypeName = PaperAlertTypeEnum.All.ToDescription();
        }

        private PaperAlertTypeEnum filterTypeAlert { get; set; } = PaperAlertTypeEnum.All;
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("AlertType") && parameters.GetValue<PaperAlertTypeEnum>("AlertType") is PaperAlertTypeEnum alertType)
            {
                if (filterTypeAlert != alertType)
                {
                    filterTypeAlert = alertType;
                    AlertTypeName = alertType.ToDescription();
                    Filter();
                }
            }
            else if (parameters.ContainsKey("paperSource") && parameters.GetValue<List<PaperItemInfor>>("paperSource") is List<PaperItemInfor> allSource)
            {
                originSource = allSource;
                AllPapers = originSource;

                //Clear text ô filter :
                if (!string.IsNullOrEmpty(searchedText))
                {
                    SearchedText = string.Empty;
                }
                if (filterTypeAlert != PaperAlertTypeEnum.All)
                {
                    filterTypeAlert = PaperAlertTypeEnum.All;
                    AlertTypeName = PaperAlertTypeEnum.All.ToDescription();
                }
            }
        }
        #region property

        private string searchedText;
        public string SearchedText
        {
            get { return searchedText; }
            set { SetProperty(ref searchedText, value); }
        }

        private List<PaperItemInfor> allPapers;
        public List<PaperItemInfor> AllPapers
        {
            get { return allPapers; }
            set
            {
                SetProperty(ref allPapers, value);
            }
        }

        private string alertTypeName;
        public string AlertTypeName
        {
            get { return alertTypeName; }
            set { SetProperty(ref alertTypeName, value); }
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
        private List<PaperItemInfor> originSource { get; set; }
        #endregion


        private void SelectPaper(object obj)
        {
            SafeExecute(async () =>
            {
                if (!CheckPermision((int)PermissionKeyNames.PaperUpdate))
                {
                    return;
                }


                if (obj != null && obj is Syncfusion.ListView.XForms.ItemTappedEventArgs agrs)
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
                }
            });

        }

        private void SelectAlertType()
        {
            SafeExecute(async () =>
            {
                var a = await NavigationService.NavigateAsync("NavigationPage/SelectAlertTypePage", null, true, true);
            });
        }


        private void Filter()
        {
            SafeExecute(() =>
            {
                var temp = originSource.Where(v => string.IsNullOrWhiteSpace(searchedText) || v.VehiclePlate.ToUpper().Contains(searchedText.ToUpper()))
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

        private void SearchVehicle(TextChangedEventArgs args)
        {
            if (args.NewTextValue == null)
                return;

            if (cts != null)
                cts.Cancel(true);
            cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                await Task.Delay(500, cts.Token);

                Filter();
            });
        }

        private void GetAllPaperCat()
        {
            RunOnBackground(async () =>
            {
                paperCat = await paperinforService.GetPaperCategories();
            });
        }
    }
}
