using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class AlertConfigSettingPageViewModel : ViewModelBase
    {
        private readonly IAlertService alertService;

        private ObservableCollection<AlertCompanyConfigRespone> listAlertCompanyConfig;
        public ObservableCollection<AlertCompanyConfigRespone> ListAlertCompanyConfig { get => listAlertCompanyConfig; set => SetProperty(ref listAlertCompanyConfig, value); }

        private bool isAllAlertCompanySelected;

        public bool IsAllAlertCompanySelected { get => isAllAlertCompanySelected; set => SetProperty(ref isAllAlertCompanySelected, value); }

        public ICommand TapAlertCompanyConfig { get; private set; }

        public ICommand PushToSettingReceiveAlertCommand { get; private set; }

        public ICommand TapListAlertCompanyCommand { get; private set; }

        public ICommand SelectedAllAlertCompanyCommand { get; private set; }

        public AlertUserConfigurationsRespone AlertConfigRespone { get; set; }

        public AlertConfigSettingPageViewModel(INavigationService navigationService, IAlertService alertService) : base(navigationService)
        {
            this.alertService = alertService;
            TapAlertCompanyConfig = new Command(PushAlertCompanyConfig);
            TapListAlertCompanyCommand = new DelegateCommand<Syncfusion.ListView.XForms.ItemTappedEventArgs>(SelectedAlertCompany);
            SelectedAllAlertCompanyCommand = new DelegateCommand(SelectedAllAlertCompany);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetAlertUserConfigurations();
        }

        private void SelectedAllAlertCompany()
        {
            IsAllAlertCompanySelected = !IsAllAlertCompanySelected;

            if (ListAlertCompanyConfig != null && ListAlertCompanyConfig.Count > 0)
            {
                foreach (var item in ListAlertCompanyConfig)
                {
                    item.IsVisible = IsAllAlertCompanySelected;
                }
            }
        }

        private void SelectedAlertCompany(Syncfusion.ListView.XForms.ItemTappedEventArgs args)
        {
            SafeExecute(() =>
            {
                var selected = (args.ItemData as AlertCompanyConfigRespone);
                if (selected != null)
                {
                    foreach (var item in ListAlertCompanyConfig)
                    {
                        if (item.FK_AlertTypeID == selected.FK_AlertTypeID)
                        {
                            item.IsVisible = !selected.IsVisible;
                            break;
                        }
                    }
                    var check = ListAlertCompanyConfig.All(l => l.IsVisible);
                    IsAllAlertCompanySelected = check;
                }
            });
        }

        private void GetAlertUserConfigurations()
        {
            TryExecute(async () =>
            {
                var respone = await alertService.GetAlertUserConfigurations(UserInfo.UserId);
                if (respone != null)
                {
                    AlertConfigRespone = respone;
                }
                else
                {
                    AlertConfigRespone = new AlertUserConfigurationsRespone();
                }
                GetListAlertCompanyConfig();
            });
        }

        /// <summary>Lấy danh sách cảnh báo công ty</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv 6/29/2020   created
        /// </Modified>
        private void GetListAlertCompanyConfig()
        {
            TryExecute(async () =>
            {
                DependencyService.Get<IHUDProvider>().DisplayProgress("");

                var list = await alertService.GetAlertCompanyConfig(UserInfo.CompanyId);

                DependencyService.Get<IHUDProvider>().Dismiss();
                if (list != null && list.Count > 0)
                {
                    ListAlertCompanyConfig = new ObservableCollection<AlertCompanyConfigRespone>(list);

                    var splitstr = AlertConfigRespone.AlertTypeIDs;
                    if (!string.IsNullOrEmpty(splitstr))
                    {
                        var split = splitstr.Split(',');
                        foreach (var itemsplit in split)
                        {
                            foreach (var item in ListAlertCompanyConfig)
                            {
                                if (item.FK_AlertTypeID == int.Parse(itemsplit))
                                {
                                    item.IsVisible = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ListAlertCompanyConfig = new ObservableCollection<AlertCompanyConfigRespone>();
                }
            });
        }

        private void PushAlertCompanyConfig()
        {
            TryExecute(async () =>
            {
                var respones = ListAlertCompanyConfig.Where(l => l.IsVisible).ToList();

                if (respones != null && respones.Count > 0)
                {
                    var AlertTypeIDs = string.Empty;

                    for (int i = 0; i < respones.Count; i++)
                    {
                        if (i == (respones.Count - 1))
                        {
                            AlertTypeIDs += string.Format("{0}", respones[i].FK_AlertTypeID);
                        }
                        else
                        {
                            AlertTypeIDs += string.Format("{0},", respones[i].FK_AlertTypeID);
                        }
                    }

                    var alertConfigRequest = new AlertUserConfigurationsRequest()
                    {
                        FK_UserID = UserInfo.UserId,
                        FK_CompanyID = UserInfo.CompanyId,
                        AlertTypeIDs = AlertTypeIDs,
                        VehicleIDs = AlertConfigRespone.VehicleIDs ?? string.Empty,
                        ReceiveTimes = AlertConfigRespone.ReceiveTimes ?? string.Empty,
                    };

                    RedirectAlertCompanyConfig(alertConfigRequest);
                }
                else
                {
                    await PageDialog.DisplayAlertAsync(MobileResource.AlertConfig_Label_Alter, MobileResource.AlertConfig_Label_Warning_Alert, MobileResource.Common_Button_OK);
                }
            });
        }

        private void RedirectAlertCompanyConfig(AlertUserConfigurationsRequest alertConfigRequest)
        {
            TryExecute(async () =>
            {
                await NavigationService.NavigateAsync("AlertVehicleSettingPage", new NavigationParameters
                {
                    { ParameterKey.AlertCompanyConfig, alertConfigRequest }
                }, useModalNavigation: false);
            });
        }
    }
}