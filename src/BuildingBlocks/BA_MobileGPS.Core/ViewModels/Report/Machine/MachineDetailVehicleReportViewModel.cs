using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;

using Prism.Navigation;

namespace BA_MobileGPS.Core.ViewModels
{
    public class MachineDetailVehicleReportViewModel : ViewModelBase
    {
        public MachineDetailVehicleReportViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = MobileResource.ReportMachine_Label_TitlePage;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters?.GetValue<MachineVehicleResponse>(ParameterKey.ReportMachineVehicleSelected) is MachineVehicleResponse machineDetail)
            {
                machineDetail.ShowDateDetailSTR = DateTimeHelper.DisplayDateTimeVer2(machineDetail.StartTime, machineDetail.EndTime, Settings.CurrentLanguage);

                SelectMachineDetail = machineDetail;
            }
        }

        #region property

        private MachineVehicleResponse _selectMachine;

        public MachineVehicleResponse SelectMachineDetail
        {
            get { return _selectMachine; }
            set { SetProperty(ref _selectMachine, value); }
        }

        #endregion property
    }
}