using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.IService.Support;
using Prism.Commands;
using Prism.Navigation;
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
                if(parameters.ContainsKey("NoData") && parameters.GetValue<bool>("NoData") is bool para) {

                  MessageData = para ;
                }
                else
                if (parameters.ContainsKey("Support1") && parameters.GetValue<MessageSupportRespone>("Support1") is MessageSupportRespone item)
                {
                    if (parameters.ContainsKey("Support") && parameters.GetValue<SupportCategoryRespone>("Support") is SupportCategoryRespone obj)
                    {
                        //GetListMessage(obj, 3, 6, DateTime.Now, DateTime.Now);
                        ISupportDisconnectView = parameters.GetValue<bool>("BoolPage");
                        Question = item.Questions;
                        Guide = item.Guides;
                        NamePage = obj.Name;                     
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

        private void GetListMessage(SupportCategoryRespone obj, Vehicle vehicle)
        {
            RunOnBackground(async () =>
            {
                var qry = StaticSettings.ListVehilceOnline.FirstOrDefault(x => x.VehicleId == vehicle.VehicleId);
                if (qry != null)
                {
                    ListSupportContent = await _iSupportCategoryService.GetMessagesSupport(obj.ID);
                    if (ListSupportContent != null && ListSupportContent.Count > 0)
                    {
                        // Nếu không kiểm tra xe mất tín hiệu
                        if (StateVehicleExtension.IsLostGPS(qry.GPSTime, qry.VehicleTime) == true || StateVehicleExtension.IsLostGSM(qry.VehicleTime) == true)
                        {
                            var query = ListSupportContent.Where(s => s.OrderNo == 0).FirstOrDefault();
                            Question = query.Questions;
                            Guide = query.Guides;
                            ISupportDisconnectView = true;
                        }
                        else //nếu messageId==1,2,3,128 thì là xe dừng dịch vụ hoac dang no phi chuyen vao trang no phi
                        if (StateVehicleExtension.IsVehicleDebtMoney(qry.MessageId, qry.DataExt) == true || StateVehicleExtension.IsVehicleStopService(qry.MessageId) == true)
                        {
                            var query = ListSupportContent.Where(s => s.OrderNo == 1).FirstOrDefault();
                            Question = query.Questions;
                            Guide = query.Guides;
                            ISupportDisconnectView = false;
                        }
                        else
                        // Nếu xe còn phí chuyển đến trang đổi biển
                        {
                            SafeExecute(async () =>
                            {
                                var parameters = new NavigationParameters { { ParameterKey.VehicleRoute, DataPlate } };

                                await NavigationService.NavigateAsync("ChangeLicensePlate", parameters);
                            });
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
            });
        }

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