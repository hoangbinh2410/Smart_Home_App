using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Enums;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service.IService.Support;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    internal class SupportErrorsSignalPageViewModel : ViewModelBase
    {
        #region Property

        private String _lbTabIndex;

        public String LbTabIndex
        {
            get { return _lbTabIndex; }
            set { _lbTabIndex = value; }
        }

        private String _lbQuestions;

        public String LbQuestions
        {
            get { return _lbQuestions; }
            set { _lbQuestions = value; }
        }

        private String _textBtnYes;

        public String TextBtnYes
        {
            get { return _textBtnYes; }
            set { _textBtnYes = value; }
        }

        private String _textBtnNo;

        public String TextBtnNo
        {
            get { return _textBtnNo; }
            set { _textBtnNo = value; }
        }

        private String _lbTextPage;

        public String LbTextPage
        {
            get { return _lbTextPage; }
            set { _lbTextPage = value; }
        }

        private bool _isVisibleYesNo;

        public bool IsVisibleYesNo
        {
            get => _isVisibleYesNo;
            set => SetProperty(ref _isVisibleYesNo, value);
        }

        private bool _isPageCollection;

        public bool IsPageCollection
        {
            get => _isPageCollection;
            set => SetProperty(ref _isPageCollection, value);
        }

        private bool _isShowIconBtnYes;

        public bool ISShowIconBtnYes
        {
            get => _isShowIconBtnYes;
            set => SetProperty(ref _isShowIconBtnYes, value);
        }

        private Color _backgroundColorBtnYes = Color.White;

        public Color BackgroundColorBtnYes
        {
            get => _backgroundColorBtnYes;
            set => SetProperty(ref _backgroundColorBtnYes, value);
        }

        private Color _textColorBtnYes = Color.DarkBlue;

        public Color TextColorBtnYes
        {
            get => _textColorBtnYes;
            set => SetProperty(ref _textColorBtnYes, value);
        }

        private int _borderWidthBtnYes;

        public int BorderWidthBtnYes
        {
            get => _borderWidthBtnYes;
            set => SetProperty(ref _borderWidthBtnYes, value);
        }

        private int _selectedIndex = 0;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        private ObservableCollection<SupportErrorsSignalPageViewModel> _pageCollection = new ObservableCollection<SupportErrorsSignalPageViewModel>();

        public ObservableCollection<SupportErrorsSignalPageViewModel> PageCollection
        {
            get { return _pageCollection; }
            set { _pageCollection = value; }
        }

        private Vehicle _vehicle = new Vehicle();

        public Vehicle Vehicle
        {
            get => _vehicle;
            set => SetProperty(ref _vehicle, value);
        }

        private SupportCategoryRespone _objSupport = new SupportCategoryRespone();

        public SupportCategoryRespone ObjSupport
        {
            get => _objSupport;
            set => SetProperty(ref _objSupport, value);
        }

        #endregion Property

        #region Contructor

        public ICommand BackPageCommand { get; private set; }
        public ICommand SfButtonYesCommand { get; private set; }
        public ICommand SfButtonNoCommand { get; private set; }
        private ISupportCategoryService _iSupportCategoryService;
        private INavigationService _navigationService;

        public SupportErrorsSignalPageViewModel(INavigationService navigationService, ISupportCategoryService iSupportCategoryService)
            : base(navigationService)
        {
            Title = MobileResource.SupportClient_Label_Title;
            _navigationService = navigationService;
            _iSupportCategoryService = iSupportCategoryService;
            BackPageCommand = new DelegateCommand(BackPageClicked);
            Xamarin.Forms.MessagingCenter.Subscribe<SupportErrorsSignalPageViewModel>(this, "SelectedIndex", (sender) =>
            {
                switch (SelectedIndex)
                {
                    case 0:
                        if (SelectedIndex == PageCollection.Count -1)
                        {
                            NavigationFeedbackPage();
                            return;
                        }
                        break;

                    case 1:
                        if (SelectedIndex == PageCollection.Count -1)
                        {
                            NavigationFeedbackPage();
                            return;
                        }
                        break;

                    case 2:
                        if (SelectedIndex == PageCollection.Count - 1)
                        {
                            NavigationFeedbackPage();                          
                        }
                        break;

                    default:                     
                        break;
                }             
                SelectedIndex++;
            });
        }

        public SupportErrorsSignalPageViewModel(INavigationService navigationService, string lbTabIndex, string lbQuestions, string textBtnYes, string textBtnNo, string lbTextPage)
           : base(navigationService)
        {
            LbTabIndex = lbTabIndex;
            LbQuestions = lbQuestions;
            TextBtnYes = textBtnYes;
            TextBtnNo = textBtnNo;
            LbTextPage = lbTextPage;
            IsVisibleYesNo = false;
            BorderWidthBtnYes = 2;
            SfButtonYesCommand = new DelegateCommand(SfButtonYesClicked);
            SfButtonNoCommand = new DelegateCommand(SfButtonNoClicked);
        }

        #endregion Contructor

        #region PrivateMethod

        private void BackPageClicked()
        {
            SafeExecute(async () =>
            {
                await NavigationService.GoBackToRootAsync(null);
            });
        }

        private void SfButtonYesClicked()
        {
            IsVisibleYesNo = true;
            BackgroundColorBtnYes = Color.DeepSkyBlue;
            ISShowIconBtnYes = true;
            TextColorBtnYes = Color.White;
            BorderWidthBtnYes = 0;
        }

        private void SfButtonNoClicked()
        {
            IsVisibleYesNo = false;
            BackgroundColorBtnYes = Color.White;
            ISShowIconBtnYes = false;
            TextColorBtnYes = Color.DarkBlue;
            BorderWidthBtnYes = 2;
            Xamarin.Forms.MessagingCenter.Send<SupportErrorsSignalPageViewModel>(this, "SelectedIndex");
        }

        private void GetCollectionPage(SupportCategoryRespone obj)
        {
            SafeExecute(async () =>
            {
                if (IsConnected)
                {
                    using (new HUDService(MobileResource.Common_Message_Processing))
                    {
                        List<MessageSupportRespone> items = await _iSupportCategoryService.GetMessagesSupport(obj.ID);
                        int index = 0;
                        if (items != null && items.Count > 0)
                        {
                            IsPageCollection = false;
                            foreach (var item in items)
                            {
                                index++;
                                if (item.OrderNo == 2 && obj.Code == "MTH")
                                {
                                    PageCollection.Add(new SupportErrorsSignalPageViewModel(_navigationService, index.ToString(), item.Questions, MobileResource.SupportClient_Text_Unfinished, MobileResource.SupportClient_Text_Accomplished, item.Guides));
                                }
                                else
                                {
                                    PageCollection.Add(new SupportErrorsSignalPageViewModel(_navigationService, index.ToString(), item.Questions, MobileResource.SupportClient_Text_Yes, MobileResource.SupportClient_Text_No, item.Guides));
                                }
                            }
                        }
                        else
                        {
                            IsPageCollection = true;
                        }
                    }
                }
                else
                {
                    IsPageCollection = true;
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
                }
            });
        }

        private void NavigationFeedbackPage()
        {
            SelectedIndex = 0;
            var parameters = new NavigationParameters
            {
                { "ObjSupport", _objSupport },
                { ParameterKey.VehicleRoute, Vehicle },
            };
            SafeExecute(async () =>
            {
                await NavigationService.GoBackToRootAsync(null);
            });
        }

        #endregion PrivateMethod

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
                if (parameters.ContainsKey("Support") && parameters.GetValue<SupportCategoryRespone>("Support") is SupportCategoryRespone objSupport)
                {
                    ObjSupport = objSupport;
                    GetCollectionPage(objSupport);
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
    }
}