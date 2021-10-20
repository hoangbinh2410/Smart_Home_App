using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.IService.Support;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class SupportFeePageViewModel : ViewModelBase
    {
        #region Contructor

        private readonly IVehicleOnlineService vehicleOnlineService;
        public ICommand BackPageCommand { get; private set; }
        public ICommand PushChangeLicensePlateCommand { get; private set; }
        public ICommand PushMessageSuportPageCommand { get; private set; }
        private ISupportCategoryService _iSupportCategoryService;

        public SupportFeePageViewModel(INavigationService navigationService, ISupportCategoryService iSupportCategoryService, IVehicleOnlineService vehicleOnlineService) : base(navigationService)
        {
            this.vehicleOnlineService = vehicleOnlineService;         
            BackPageCommand = new DelegateCommand(BackPage);
            PushChangeLicensePlateCommand = new DelegateCommand(PushChangeLicensePlate);
            Title = "Hỗ trợ khách hàng";
            PushMessageSuportPageCommand = new DelegateCommand(PushMessageSuportPage);
            _iSupportCategoryService = iSupportCategoryService;
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {           
            base.Initialize(parameters);
            if(parameters != null)
            {
                if (parameters.ContainsKey("Support") && parameters.GetValue<SupportCategoryRespone>("Support") is SupportCategoryRespone obj)
                {
                    GetListMessage(obj, 11, 6, DateTime.Now, DateTime.Now);
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

        private SupportCategoryRespone Item { get; set; }
       
        public List<MessageSupportRespone> ListSupportContent { get; set; }
       
        private MessageSupportRespone _SupportContent = new MessageSupportRespone();

        public MessageSupportRespone SupportContent
        {
            get { return _SupportContent; }
            set { SetProperty(ref _SupportContent, value); }
        }

        #endregion Property

        #region PrivateMethod

        private void GetListMessage(SupportCategoryRespone obj,int messageId, int dataExt, DateTime GPSTime, DateTime VehicleTime)
        {
            RunOnBackground(async () =>
            {
                NamePage = obj.Name;
                ListSupportContent = await _iSupportCategoryService.GetMessagesSupport(obj.ID);
                if (ListSupportContent != null && ListSupportContent.Count > 0)
                {
                    //nếu messageId==1,2,3,128 thì là xe dừng dịch vụ hoac dang no phi chuyen vao trang no phi
                    if (StateVehicleExtension.IsVehicleDebtMoney(messageId, dataExt) == true || StateVehicleExtension.IsVehicleDebtMoneyViview(messageId, dataExt) == true || StateVehicleExtension.IsVehicleStopService(messageId) == true)
                    {
                        var query = ListSupportContent.Where(s => s.OrderNo == 1).FirstOrDefault();
                        Question = query.Questions;
                        Guide = query.Guides;
                        ISupportDisconnectView = false;
                    }
                    // Nếu không kiểm tra xe mất tín hiệu
                    else
                        if (StateVehicleExtension.IsLostGPS(GPSTime, VehicleTime) == true && StateVehicleExtension.IsLostGSM(VehicleTime) == true)
                    {
                        var query = ListSupportContent.Where(s => s.OrderNo == 0).FirstOrDefault();
                        Question = query.Questions;
                        Guide = query.Guides;
                        ISupportDisconnectView = true;
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
            });
        }

        public void BackPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.GoBackAsync();
            });
        }

        public void PushChangeLicensePlate()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("ChangeLicensePlate");
            });
        }

        public void PushMessageSuportPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("MessageSuportPage");
            });
        }

        #endregion PrivateMethod
    }
}