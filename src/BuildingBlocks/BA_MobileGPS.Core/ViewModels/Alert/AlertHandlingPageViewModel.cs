using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Reflection;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class AlertHandlingPageViewModel : ViewModelBase
    {
        private readonly IAlertService alertService;

        public string alertType;
        public string AlertType { get => alertType; set => SetProperty(ref alertType, value); }

        public bool isProccess;
        public bool IsProccess { get => isProccess; set => SetProperty(ref isProccess, value); }

        private AlertOnlineDetailModel alert;
        public AlertOnlineDetailModel Alert { get => alert; set => SetProperty(ref alert, value); }

        private string startTime;
        public string StartTime { get => startTime; set => SetProperty(ref startTime, value); }

        private ValidatableObject<string> handlingContent;
        public ValidatableObject<string> HandlingContent { get => handlingContent; set => SetProperty(ref handlingContent, value); }

        public ICommand CancelCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public AlertHandlingPageViewModel(INavigationService navigationService, IAlertService alertService)
            : base(navigationService)
        {
            try
            {
                this.alertService = alertService;

                AddValidations();

                SaveCommand = new DelegateCommand(() => SaveExecuteAsync());
                CancelCommand = new DelegateCommand(CancelExecute);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("alert") && parameters["alert"] is AlertOnlineDetailModel alert)
            {
                Alert = alert;
                StartTime = Alert.StartTime.FormatDateTime();
            }
        }

        private void AddValidations()
        {
            HandlingContent = new ValidatableObject<string>();

            HandlingContent.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = MobileResource.Alert_Message_RequiredContent });
            HandlingContent.Validations.Add(new MinLenghtRule<string> { ValidationMessage = string.Format(MobileResource.Alert_Message_RequireHandleMinLenght, 3), MinLenght = 3 });
        }

        private bool Validate()
        {
            if (IsProccess)
            {
                return handlingContent.Validate();
            }
            else
            {
                return true;
            }
        }

        private async void SaveExecuteAsync()
        {
            try
            {
                if (!Validate())
                {
                    return;
                }

                Xamarin.Forms.DependencyService.Get<IHUDProvider>().DisplayProgress("");

                var isSuccess = await alertService.HandleAlertAsync(new StatusAlertRequestModel
                {
                    PK_AlertDetailID = Alert.PK_AlertDetailID,
                    Status = IsProccess ? StatusAlert.Process : StatusAlert.Readed,
                    ProccessContent = HandlingContent.Value,
                    UserID = UserInfo.UserId,
                    FK_AlertTypeID = Alert.FK_AlertTypeID,
                    FK_VehicleID = Alert.FK_VehicleID,
                    StartTime = Alert.StartTime
                });

                if (isSuccess)
                {
                    var para = new NavigationParameters
                    {
                        { "isreload", true }
                    };

                    await NavigationService.GoBackAsync(para);

                    GlobalResources.Current.TotalAlert--;

                    DisplayMessage.ShowMessageInfo(MobileResource.Alert_Message_Alert_Success);
                }
                else
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Alert_Message_Alert_Fail);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                Xamarin.Forms.DependencyService.Get<IHUDProvider>().Dismiss();
            }
        }

        private async void CancelExecute()
        {
            await NavigationService.GoBackAsync();
        }
    }
}
