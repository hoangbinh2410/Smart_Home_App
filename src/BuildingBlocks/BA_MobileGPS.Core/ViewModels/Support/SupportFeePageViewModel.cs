using BA_MobileGPS.Entities.ResponeEntity.Support;
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

        public ICommand BackPageCommand { get; private set; }
        public ICommand PushChangeLicensePlateCommand { get; private set; }
        public ICommand PushMessageSuportPageCommand { get; private set; }
        private IMessageSuportService _iMessageSuport;

        public SupportFeePageViewModel(INavigationService navigationService, IMessageSuportService iMessageSuportService) : base(navigationService)
        {
            BackPageCommand = new DelegateCommand(BackPage);
            PushChangeLicensePlateCommand = new DelegateCommand(PushChangeLicensePlate);
            Title = "Hỗ trợ khách hàng";
            PushMessageSuportPageCommand = new DelegateCommand(PushMessageSuportPage);
            _iMessageSuport = iMessageSuportService;
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            Item = parameters.GetValue<SupportCategoryRespone>("Support");
            GetListMessage();

            base.Initialize(parameters);
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

        private SupportCategoryRespone Item { get; set; }

        private List<MessageSupportRespone> _ListSupportContent = new List<MessageSupportRespone>();

        public List<MessageSupportRespone> ListSupportContent
        {
            get { return _ListSupportContent; }
            set { SetProperty(ref _ListSupportContent, value); }
        }

        private MessageSupportRespone _SupportContent = new MessageSupportRespone();

        public MessageSupportRespone SupportContent
        {
            get { return _SupportContent; }
            set { SetProperty(ref _SupportContent, value); }
        }

        #endregion Property

        #region PrivateMethod

        private void GetListMessage()
        {

            RunOnBackground(async () =>
            {
            ListSupportContent = await _iMessageSuport.GetMessagesSupport(Item.ID);
                if (ListSupportContent.Count > 0)
                {
                    foreach (var item in ListSupportContent)
                    {
                        if (item.OrderNo == 1)
                        {
                            var query = ListSupportContent.Where(s => s.OrderNo == 1).FirstOrDefault();
                            Question = query.Questions;
                            Guide = query.Guides;
                            ISupportDisconnectView = false;
                        }
                        else if (item.OrderNo == 0)
                        {
                            var query = ListSupportContent.Where(s => s.OrderNo == 0).FirstOrDefault();
                            Question = query.Questions;
                            Guide = query.Guides;
                            ISupportDisconnectView = true;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    return;
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