using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class FeedbackErrorsSignalPageViewModel : ViewModelBase
    {
        #region Property

        private bool _isVisibleFeedback;
        public bool IsVisibleFeedback
        {
            get => _isVisibleFeedback;
            set => SetProperty(ref _isVisibleFeedback, value);
        }
        private bool _isVisibleSuccess;
        public bool IsVisibleSuccess
        {
            get => _isVisibleSuccess;
            set => SetProperty(ref _isVisibleSuccess, value);
        }

        private SupportCategoryRespone _objSupport = new SupportCategoryRespone();
        public SupportCategoryRespone ObjSupport
        {
            get => _objSupport;
            set => SetProperty(ref _objSupport, value);
        }

        private Vehicle _vehicle = new Vehicle();
        public Vehicle Vehicle
        {
            get => _vehicle;
            set => SetProperty(ref _vehicle, value);
        }

        private string _entryPhoneNumber = string.Empty;
        public string EntryPhoneNumber
        {
            get => _entryPhoneNumber;
            set => SetProperty(ref _entryPhoneNumber, value);
        }

        private string _editorFeedbackContent = string.Empty;
        public string EditorFeedbackContent
        {
            get => _editorFeedbackContent;
            set => SetProperty(ref _editorFeedbackContent, value);
        }

        #endregion Property

        #region Contructor

        public ICommand SendFeedbackCommand { get; private set; }
        public ICommand BackPageCommand { get; private set; }
        public FeedbackErrorsSignalPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = MobileResource.SupportClient_Label_Title;
            SendFeedbackCommand = new DelegateCommand(SendFeedbackClicked);
            BackPageCommand = new DelegateCommand(BackPageClicked);
            IsVisibleFeedback = true;
            IsVisibleSuccess = false;
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.ContainsKey("ObjSupport") && parameters.GetValue<SupportCategoryRespone>("ObjSupport") is SupportCategoryRespone objSupport)
                {
                    ObjSupport = objSupport;
                }
                if (parameters.ContainsKey(ParameterKey.VehicleRoute) && parameters.GetValue<Vehicle>(ParameterKey.VehicleRoute) is Vehicle vehicle)
                {
                    Vehicle = vehicle;
                }
            }
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnDestroy()
        {
        }

        #endregion Lifecycle

        #region PrivateMethod
        private void SendFeedbackClicked()
        {
            if (EntryPhoneNumber.Length == 0 )
            {
                DisplayMessage.ShowMessageInfo("Số điện thoại không được bỏ trống", 5000);
                return;
            }
            if (EditorFeedbackContent.Length == 0)
            {
                DisplayMessage.ShowMessageInfo("Nội dung không được bỏ trống", 5000);
                return;
            }
            IsVisibleFeedback = false;
            IsVisibleSuccess = true;
        }
        private void BackPageClicked()
        {             
            SafeExecute(async () =>
            {
                await NavigationService.GoBackToRootAsync(null);
            });
        }
        #endregion PrivateMethod

        
    }
}
