using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

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
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class CompanyLookUpViewModel : ViewModelBase
    {
        private readonly IVehicleOnlineService vehicleOnlineService;

        private CancellationTokenSource cts;

        private bool hasCompany = true;
        public bool HasCompany { get => hasCompany; set => SetProperty(ref hasCompany, value); }

        private List<Company> ListCompanyOrigin = new List<Company>();

        private ObservableCollection<Company> listCompany = new ObservableCollection<Company>();
        public ObservableCollection<Company> ListCompany { get => listCompany; set => SetProperty(ref listCompany, value); }

        public ICommand SearchVehicleGroupCommand { get; private set; }

        public CompanyLookUpViewModel(INavigationService navigationService, IVehicleOnlineService vehicleOnlineService)
            : base(navigationService)
        {
            this.vehicleOnlineService = vehicleOnlineService;

            SearchVehicleGroupCommand = new DelegateCommand<TextChangedEventArgs>(SearchVehicleGroup);
        }

        public override void Initialize(INavigationParameters parameters)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Device.StartTimer(TimeSpan.FromMilliseconds(300), () =>
            {
                SetFocus("SearchBar");

                return false;
            });
        }

        public override void OnPageAppearingFirstTime()
        {
            using (new HUDService(""))
            {
                if (UserHelper.isBusinessUser(UserInfo))
                {
                    GetCompanyByBusinessUser();
                }
                else
                {
                    GetCompany();
                }
            }
        }

        public async void GetCompany()
        {
            if (StaticSettings.ListCompany != null && StaticSettings.ListCompany.Count > 0)
            {
                InitData(StaticSettings.ListCompany);
            }
            else
            {
                var listCompany = await vehicleOnlineService.GetListCompanyAsync(UserInfo.UserId, UserInfo.CompanyId);

                if (listCompany != null)
                {
                    StaticSettings.ListCompany = listCompany;

                    InitData(listCompany);

                    if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0 && listCompany.FirstOrDefault(x => x.FK_CompanyID == Settings.CurrentCompany.FK_CompanyID) == null)
                    {
                        Settings.CurrentCompany = new Company();
                    }
                }
                else
                {
                    StaticSettings.ListCompany = new List<Company>();
                }
            }
        }

        public async void GetCompanyByBusinessUser()
        {
            if (StaticSettings.ListCompany != null && StaticSettings.ListCompany.Count > 0)
            {
                InitData(StaticSettings.ListCompany);
            }
            else
            {
                var listCompany = await vehicleOnlineService.GetListCompanyByBusinessUserAsync(UserInfo.UserId);

                if (listCompany != null)
                {
                    StaticSettings.ListCompany = listCompany;

                    InitData(listCompany);

                    if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0 && listCompany.FirstOrDefault(x => x.FK_CompanyID == Settings.CurrentCompany.FK_CompanyID) == null)
                    {
                        Settings.CurrentCompany = new Company();
                    }
                }
                else
                {
                    StaticSettings.ListCompany = new List<Company>();
                }
            }
        }

        private void InitData(List<Company> lstCompany)
        {
            try
            {
                ListCompanyOrigin = lstCompany.DeepCopy();

                if (Settings.CurrentCompany != null)
                {
                    ListCompanyOrigin.ForEach(c =>
                    {
                        if (Settings.CurrentCompany.FK_CompanyID == c.FK_CompanyID)
                        {
                            c.IsSelected = true;
                        }
                        else
                        {
                            c.IsSelected = false;
                        }
                    });

                    ListCompanyOrigin = ListCompanyOrigin.OrderByDescending(c => c.IsSelected).ToList();
                }

                var listCompany = new List<Company>();
                void FuckYourSelf(List<Company> companies, int level)
                {
                    foreach (var company in companies)
                    {
                        for (int i = 0; i < level; i++)
                        {
                            company.CompanyName = "-" + company.CompanyName;
                        }

                        listCompany.Add(company);

                        if (level >= 10)
                            return;

                        FuckYourSelf(ListCompanyOrigin.FindAll(g => g.ParentCompanyID == company.FK_CompanyID
                            && !listCompany.Exists(e => e.FK_CompanyID == g.FK_CompanyID)), level + 1);
                    }
                }

                FuckYourSelf(ListCompanyOrigin.FindAll(g => !ListCompanyOrigin.Exists(g2 => g.FK_CompanyID != g2.FK_CompanyID && g.ParentCompanyID == g2.FK_CompanyID)), 0);

                ListCompany = listCompany.ToObservableCollection();

                HasCompany = ListCompany.Count > 0;
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public void SearchVehicleGroup(TextChangedEventArgs args)
        {
            if (cts != null)
                cts.Cancel(true);

            cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                await Task.Delay(500, cts.Token);

                if (string.IsNullOrWhiteSpace(args.NewTextValue))
                {
                    return ListCompanyOrigin.ToList();
                }
                else
                {
                    return ListCompanyOrigin.FindAll(vg => vg.CompanyName != null && vg.CompanyName.UnSignContains(args.NewTextValue));
                }
            }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    ListCompany = new ObservableCollection<Company>(task.Result);

                    HasCompany = ListCompany.Count > 0;
                }
            }));
        }

        public ICommand TapCommand
        {
            get
            {
                return new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>((args) =>
                {
                    SafeExecute(async () =>
                    {
                        if (args == null || !(args.ItemData is Company seleted))
                            return;

                        if (Settings.CurrentCompany?.FK_CompanyID != seleted.FK_CompanyID)
                        {
                            Settings.CurrentCompany = seleted;

                            await GetListVehicleOnline();

                            EventAggregator.GetEvent<SelectedCompanyEvent>().Publish(seleted.FK_CompanyID);
                        }

                        _ = await NavigationService.GoBackAsync(useModalNavigation: true, parameters: new NavigationParameters
                        {
                            { ParameterKey.Company, seleted }
                        });
                    });
                });
            }
        }

        private async Task GetListVehicleOnline()
        {
            try
            {
                var userID = UserInfo.UserId;
                var companyID = UserInfo.CompanyId;
                var xnCode = UserInfo.XNCode;
                var userType = UserInfo.UserType;
                var companyType = UserInfo.CompanyType;

                if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
                {
                    userID = Settings.CurrentCompany.UserId;
                    companyID = Settings.CurrentCompany.FK_CompanyID;
                    xnCode = Settings.CurrentCompany.XNCode;
                    userType = Settings.CurrentCompany.UserType;
                    companyType = Settings.CurrentCompany.CompanyType;
                }
                int vehicleGroup = 0;
                var list = await vehicleOnlineService.GetListVehicleOnline(userID, vehicleGroup, companyID, xnCode, userType, companyType);
                if (list != null && list.Count > 0)
                {
                    list.ForEach(x =>
                    {
                        x.IconImage = IconCodeHelper.GetMarkerResource(x);

                        x.StatusEngineer = StateVehicleExtension.EngineState(x);

                        if (!StateVehicleExtension.IsLostGPS(x.GPSTime, x.VehicleTime) && !StateVehicleExtension.IsLostGSM(x.VehicleTime))
                        {
                            x.SortOrder = 1;
                        }
                        else
                        {
                            x.SortOrder = 0;
                        }
                    });
                    StaticSettings.ListVehilceOnline = list;
                }
                else
                {
                    StaticSettings.ListVehilceOnline = new List<VehicleOnline>();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }
    }
}