using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.IService.Support;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class SupportFeePageViewModel : ViewModelBase
    {
        #region Contructor

        private readonly IVehicleOnlineService vehicleOnlineService;
        public ICommand BackPageCommand { get; private set; }
        public ICommand PushMessageSuportPageCommand { get; private set; }
        private ISupportCategoryService _iSupportCategoryService;

        public SupportFeePageViewModel(INavigationService navigationService, ISupportCategoryService iSupportCategoryService, IVehicleOnlineService vehicleOnlineService) : base(navigationService)
        {
            this.vehicleOnlineService = vehicleOnlineService;
            BackPageCommand = new DelegateCommand(BackPage);
            Title = "Hỗ trợ khách hàng";
            PushMessageSuportPageCommand = new DelegateCommand(PushMessageSuportPage);
            _iSupportCategoryService = iSupportCategoryService;
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            if (parameters != null)
            {
                if (parameters.ContainsKey("NoData") && parameters.GetValue<bool>("NoData") is bool para)
                {
                    MessageData = para;
                }
                else
                if (parameters.ContainsKey("Support1") && parameters.GetValue<MessageSupportRespone>("Support1") is MessageSupportRespone item)
                {
                    //if (parameters.ContainsKey(ParameterKey.VehicleRoute) && parameters.GetValue<Vehicle>(ParameterKey.VehicleRoute) is Vehicle vehicle)
                    //{
                    //    
                    //}
                    if (parameters.ContainsKey("Support") && parameters.GetValue<SupportCategoryRespone>("Support") is SupportCategoryRespone obj && parameters.ContainsKey(ParameterKey.VehicleRoute) && parameters.GetValue<Vehicle>(ParameterKey.VehicleRoute) is Vehicle vehicle)
                    {
                        //GetListMessage(obj, 3, 6, DateTime.Now, DateTime.Now);
                        ISupportDisconnectView = parameters.GetValue<bool>("BoolPage");
                        Question = item.Questions;
                        Guide = item.Guides;
                        NamePage = obj.Name;
                        DataPlate = vehicle.VehiclePlate;
                    }
                    else
                    {
                        MessageData = true;
                    }
                }
                else
                {
                    MessageData = true;
                }
            }
            else
            {
                MessageData = true;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
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

        #region Property

        private bool messageData = false;
        public bool MessageData { get => messageData; set => SetProperty(ref messageData, value); }
        private bool IsupportDisconnectView = false;

        public bool ISupportDisconnectView
        {
            get { return IsupportDisconnectView; }
            set
            {
                SetProperty(ref IsupportDisconnectView, value);
            }
        }

        private string question = String.Empty;

        public string Question
        {
            get { return question; }
            set
            {
                SetProperty(ref question, value);
            }
        }

        private string guide = String.Empty;

        public string Guide
        {
            get { return guide; }
            set
            {
                SetProperty(ref guide, value);
            }
        }

        private string namepage = String.Empty;

        public string NamePage
        {
            get { return namepage; }
            set
            {
                SetProperty(ref namepage, value);
            }
        }

        private string DataPlate { get; set; }

        public List<MessageSupportRespone> ListSupportContent { get; set; }

        private MessageSupportRespone _SupportContent = new MessageSupportRespone();

        public MessageSupportRespone SupportContent
        {
            get { return _SupportContent; }
            set { SetProperty(ref _SupportContent, value); }
        }

        #endregion Property

        #region PrivateMethod

        public void BackPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.GoBackAsync();
            });
        }

        public void PushMessageSuportPage()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
            {
                { "Support", NamePage },
                { ParameterKey.VehicleRoute, DataPlate },
            };
                await NavigationService.NavigateAsync("MessageSuportPage", parameters);
            });
        }

        #endregion PrivateMethod
    }
}