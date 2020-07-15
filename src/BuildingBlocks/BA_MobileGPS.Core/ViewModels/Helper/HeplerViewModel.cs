using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Service;

using Prism.Commands;
using Prism.Navigation;

using System.Collections.Generic;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class HeplerViewModel : ViewModelBase
    {
        private readonly IHelperService helperService;

        private List<HeplerModel> Response => new List<HeplerModel>
        {
            new HeplerModel () { Icon = "ic_list_black", Title = MobileResource.Helper_Label_HeplperOnline, GuideType = GuideType.Online, IsShow =
            CheckPermision(new List<PermissionKeyNames>
            {
                PermissionKeyNames.ViewModuleOnline,
            })},
            new HeplerModel () { Icon = "ic_list_black", Title = MobileResource.Helper_Label_Router, GuideType = GuideType.Router, IsShow =
            CheckPermision(new List<PermissionKeyNames>
            {
                PermissionKeyNames.ViewModuleRoute,
            })},
            new HeplerModel () { Icon = "ic_list_black", Title = MobileResource.Helper_Label_Camera, GuideType = GuideType.Camera, IsShow  =
            CheckPermision(new List<PermissionKeyNames>
            {
                PermissionKeyNames.AdminUtilityImageView,
                PermissionKeyNames.AdminUtilityImageManagement
            })},
            new HeplerModel () { Icon = "ic_list_black", Title = MobileResource.Helper_Label_VihcleDebt, GuideType = GuideType.ListVihicleDebt, IsShow = MobileSettingHelper.IsUseVehicleDebtMoney },
            new HeplerModel () { Icon = "ic_list_black", Title = MobileResource.Helper_Label_listVihicle, GuideType = GuideType.ListVihicle, IsShow = true },
            new HeplerModel () { Icon = "ic_list_black", Title = MobileResource.Helper_Label_PourFuel, GuideType = GuideType.Report, IsShow = CheckPermision(new List<PermissionKeyNames>
            {
                PermissionKeyNames.ViewModuleReports,
                PermissionKeyNames.ReportMachineOnView,
                PermissionKeyNames.ReportMachineStateView
            })},
        };

        private List<HeplerModel> listHelper;
        public List<HeplerModel> ListHelper { get => listHelper; set => SetProperty(ref listHelper, value); }

        public ICommand ItemSelectedCommand { get; set; }

        public HeplerViewModel(INavigationService navigationService,
            IHelperService helperService)
            : base(navigationService)
        {
            this.helperService = helperService;

            ItemSelectedCommand = new DelegateCommand<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemSelected);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            ListHelper = Response.FindAll(h => h.IsShow);
        }

        private void GetListHelper()
        {
            var response = helperService.GetHelper(new HelperRequest
            {
            });
        }

        private void ItemSelected(Syncfusion.ListView.XForms.ItemTappedEventArgs args)
        {
            if (args.ItemData is HeplerModel hepler)
            {
                NavigationService.NavigateAsync("TutorialPage", parameters: new NavigationParameters
                {
                    { ParameterKey.HelperKey, hepler.GuideType }
                });
            }
        }
    }
}